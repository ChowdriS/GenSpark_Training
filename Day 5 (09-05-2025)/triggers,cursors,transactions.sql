-- Cursor-Based Questions (5)
-- Write a cursor that loops through all films and prints titles longer than 120 minutes.

do $$
declare film_record record;
 film_cursor cursor for 
	select title from film where length > 120;

begin
	open film_cursor;
	fetch film_cursor into film_record;
	while found loop
		raise notice '%',film_record.title;
		fetch film_cursor into film_record;
	end loop;
	close film_cursor;
end
$$;
 
-- Create a cursor that iterates through all customers and counts how many rentals each made.
do $$
declare rent_rec record;
	rent_cnt int;
	rent_cur cursor for select customer_id, first_name from customer;
begin
	open rent_cur;
	fetch rent_cur into rent_rec;
	while found loop
		select count(*) into rent_cnt from rental where customer_id = rent_rec.customer_id;
		raise notice 'cutomer:% - rented_film:%', rent_rec.first_name, rent_cnt;
		fetch rent_cur into rent_rec;
	end loop;
	close rent_cur;
end 
$$;
 
-- Using a cursor, update rental rates: Increase rental rate by $1 for films with less than 5 rentals.

do $$
declare
    film_rec record;
    film_cursor cursor for select film_id from film;
    rental_count int;
begin
    open film_cursor;
	fetch film_cursor into film_rec;
    while found loop
		-- Print the film_id for debugging
  --       select * from (select inventory_id from inventory where film_id = 333) as j
		-- join rental r on r.inventory_id = j.inventory_id;

		select count(*) into rental_count
        from inventory i 
        join rental r on i.inventory_id = r.inventory_id 
        where i.film_id = film_rec.film_id;
        raise notice 'Checking film_id: % - rental_count:%', film_rec.film_id, rental_count;

        if rental_count < 5 then
            update film 
            set rental_rate = rental_rate + 1 
            where film_id = film_rec.film_id;
            raise notice 'updated film id % (rentals: %)', film_rec.film_id, rental_count;
        end if;
		fetch film_cursor into film_rec;
    end loop;
    close film_cursor;
end$$;

-- SELECT r.inventory_id, r.rental_id
-- FROM rental r
-- JOIN inventory i ON r.inventory_id = i.inventory_id
-- WHERE i.film_id = 333;



-- select i.inventory_id, count(*) from rental r join inventory i on i.inventory_id = r.inventory_id group by i.inventory_id;
-- select r.film_id , count(*) from film r join inventory i on i.film_id = r.film_id group by r.film_id;

 
-- Create a function using a cursor that collects titles of all films from a particular category.

create or replace function get_titles_by_category(cat_name text)
returns table(title text) as $$
declare
    film_rec record;
    film_cursor cursor for
        select f.title
        from film f
        join film_category fc on f.film_id = fc.film_id
        join category c on fc.category_id = c.category_id
        where c.name = cat_name;
begin
    open film_cursor;
    loop
        fetch film_cursor into film_rec;
        exit when not found;
        title := film_rec.title;
        return next;
    end loop;
    close film_cursor;
end;
$$ language plpgsql;

select * from get_titles_by_category('Action');

-- select * from category;

-- Loop through all stores and count how many distinct films are available in each store using a cursor.

do $$
declare
    store_rec record;
    film_count int;
    store_cursor cursor for select store_id from store;
begin
    open store_cursor;
    loop
        fetch store_cursor into store_rec;
        exit when not found;
        select count(distinct film_id)
        into film_count
        from inventory
        where store_id = store_rec.store_id;
        raise notice 'store % has % distinct films', store_rec.store_id, film_count;
    end loop;
    close store_cursor;
end$$;

select * from store
 
-- Trigger-Based Questions (5)
-- Write a trigger that logs whenever a new customer is inserted.

	create table logger (id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY, customer_id int,
	log_message varchar(16),log_time timestamp default current_timestamp)
	
	create trigger trg_log_new_customer
	after insert on customer
	for each row
	execute function log_customer_insert();
	
	create or replace function log_customer_insert()
	returns trigger as $$
	begin
	    insert into logger (customer_id ,log_message) values (new.customer_id,'added');
	    return null;
	end;
	$$ language plpgsql;
	
 
-- Create a trigger that prevents inserting a payment of amount 0.

create trigger trg_block_zero_payment
before insert on payment
for each row
when (new.amount = 0)
execute function raise_zero_payment();

create or replace function raise_zero_payment()
returns trigger as $$
begin
    raise exception 'payment amount cannot be zero';
end;
$$ language plpgsql;

 
-- Set up a trigger to automatically set last_update on the film table before update.

create trigger trg_set_last_update
before update on film
for each row
execute function set_last_update();

create or replace function set_last_update()
returns trigger as $$
begin
    new.last_update := current_timestamp;
    return new;
end;
$$ language plpgsql;

 
-- Create a trigger to log changes in the inventory table (insert/delete).

create table inventory_log (
    inventory_id int,
    action text,
    log_time timestamp default current_timestamp
);

create trigger trg_log_inventory_insert
after insert on inventory
for each row
execute function log_inventory_change_insert();

create trigger trg_log_inventory_delete
after delete on inventory
for each row
execute function log_inventory_change_delete();

create or replace function log_inventory_change_insert()
returns trigger as $$
begin
    insert into inventory_log (inventory_id, action) values (new.inventory_id, 'insert');
    return null;
end;
$$ language plpgsql;

create or replace function log_inventory_change_delete()
returns trigger as $$
begin
    insert into inventory_log (inventory_id, action) values (old.inventory_id, 'delete');
    return null;
end;
$$ language plpgsql;

 
-- Write a trigger that ensures a rental can’t be made for a customer who owes more than $50.

create trigger trg_block_high_debt_rental
before insert on rental
for each row
execute function check_customer_debt();

create or replace function check_customer_debt()
returns trigger as $$
declare
    total_due numeric;
begin
    select sum(amount) * -1 into total_due
    from payment
    where customer_id = new.customer_id;

    if total_due > 50 then
        raise exception 'customer owes more than $50';
    end if;
    return new;
end;
$$ language plpgsql;

 
-- Transaction-Based Questions (5)
-- Write a transaction that inserts a customer and an initial rental in one atomic operation.

begin;

insert into customer (customer_id, store_id, first_name, last_name, email, address_id, activebool, create_date)
values (600, 1, 'john', 'doe', 'john.doe@example.com', 1, true, current_timestamp);

insert into rental (rental_id, rental_date, inventory_id, customer_id, staff_id, last_update)
values (20000, current_timestamp, 1, 600, 1, current_timestamp);

commit;


rollback;

select * from logger;

select * from customer;
select * from rental order by rental_id desc;


 
-- Simulate a failure in a multi-step transaction (update film + insert into inventory) and roll back.

begin;
	insert into customer (customer_id,store_id, first_name, last_name, email, address_id, activebool, create_date)
	values (600,1, 'john', 'doe', 'john.doe@example.com', 1, true, current_timestamp)
	returning customer_id;
	
	insert into rental (rental_id,rental_date, inventory_id, customer_id, staff_id, last_update)
	values (1005,current_timestamp, 1, 1005, 1, current_timestamp);
commit;

rollback;
 
-- Create a transaction that transfers an inventory item from one store to another.

begin;
	update inventory
	set store_id = 2, last_update = current_timestamp
	where inventory_id = 10;
commit;

 
-- Demonstrate SAVEPOINT and ROLLBACK TO SAVEPOINT by updating payment amounts, then undoing one.

begin;

	update payment set amount = amount + 5 where payment_id = 1;
	
	savepoint before_second_update;
	
	update payment set amount = amount + 10 where payment_id = 2;
	
	rollback to savepoint before_second_update;

commit;

 
-- Write a transaction that deletes a customer and all associated rentals and payments, ensuring atomicity.

begin;

	delete from payment
	where customer_id = 5;
	
	delete from rental
	where customer_id = 5;
	
	delete from customer
	where customer_id = 5;

commit;