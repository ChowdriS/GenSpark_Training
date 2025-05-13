do $$
declare
    rental_record record;
    rental_cursor cursor for
        select r.rental_id, c.first_name, c.last_name, r.rental_date
        from rental r
        join customer c on r.customer_id = c.customer_id
        order by r.rental_id;
begin
    open rental_cursor;

    fetch rental_cursor into rental_record;
    while found loop

        raise notice 'rental id: %, customer: % %, date: %',
                     rental_record.rental_id,
                     rental_record.first_name,
                     rental_record.last_name,
                     rental_record.rental_date;
        fetch rental_cursor into rental_record;
    end loop;

    close rental_cursor;
end;
$$;



create table rental_tax_log (
    rental_id int,
    customer_name text,
    rental_date timestamp,
    amount numeric,
    tax numeric
);

select * from rental_tax_log
do $$
declare
    rec record;
    cur cursor for
        select r.rental_id, 
               c.first_name || ' ' || c.last_name as customer_name,
               r.rental_date,
               p.amount
        from rental r
        join payment p on r.rental_id = p.rental_id
        join customer c on r.customer_id = c.customer_id;
begin
    open cur;

    loop
        fetch cur into rec;
        exit when not found;

        insert into rental_tax_log (rental_id, customer_name, rental_date, amount, tax)
        values (
            rec.rental_id,
            rec.customer_name,
            rec.rental_date,
            rec.amount,
            rec.amount * 0.10
        );
    end loop;

    close cur;
end;
$$;

-- Cursors 
-- Write a cursor to list all customers and how many rentals each made. Insert these into a summary table.
create table rental_summary (id serial primary key, customer_id int, rental_count int);

do $$
declare rental_rec record ; 
rental_cur  cursor for
select c.customer_id as c_id, count(*) as rental_count from customer c 
join rental r on c.customer_id = r.customer_id group by c.customer_id;
begin
	open rental_cur;
	fetch rental_cur into rental_rec;
	while found loop
		insert into rental_summary ( customer_id, rental_count) values (rental_rec.c_id, rental_rec.rental_count);
		fetch rental_cur into rental_rec;
	end loop;
	close rental_cur;
end; $$


select * from rental_summary;

-- Using a cursor, print the titles of films in the 'Comedy' category rented more than 10 times.

do $$ 
declare film_rec record;
film_cur cursor for
	select f.title as name, count(*) as rental_count from inventory i join
	rental r on i.inventory_id = r.inventory_id 
	join film_category fc on fc.film_id = i.film_id 
	join category c on c.category_id = fc.category_id
	join film f on f.film_id = i.film_id
	where c.name like 'Comedy' group by f.title order by rental_count desc;
begin
	open film_cur;
	fetch film_cur into film_rec;
	while found loop
		raise notice 'Name - %, Rental_count - %', film_rec.name, film_rec.rental_count;
		fetch film_cur into film_rec;
	end loop;
	close film_cur;
end;
$$

 
-- Create a cursor to go through each store and count the number of distinct films available, and insert results into a report table.
create table tbl_report (id serial primary key, store_id int, film_owned int);

do $$ 
declare film_rec record;
film_cur cursor for SELECT s.store_id as _id, COUNT(DISTINCT i.film_id) AS cnt FROM 
	store s JOIN inventory i ON s.store_id = i.store_id 
	GROUP BY s.store_id ORDER BY s.store_id;
begin
	open film_cur;
	fetch film_cur into film_rec;
	while found loop
		insert into tbl_report (store_id, film_owned) values (film_rec._id, film_rec.cnt);
		fetch film_cur into film_rec;
	end loop;
	close film_cur;
end;
$$

select *  from tbl_report;

-- Loop through all customers who haven't rented in the last 6 months and insert their details into an inactive_customers table.
create table inactive_customers (id serial primary key, inactive_customers_id int);

do $$ 
declare film_rec record;
film_cur cursor for SELECT 
    c.customer_id as id
	FROM 
	    customer c
	LEFT JOIN 
	    rental r ON c.customer_id = r.customer_id
	GROUP BY 
	    c.customer_id
	HAVING 
	    MAX(r.rental_date) IS NULL 
	    OR MAX(r.rental_date) < CURRENT_DATE - INTERVAL '6 months'
	ORDER BY 
	    c.customer_id;
begin
	open film_cur;
	fetch film_cur into film_rec;
	while found loop
		insert into inactive_customers (inactive_customers_id) values (film_rec.id);
		fetch film_cur into film_rec;
	end loop;
	close film_cur;
end;
$$

select * from inactive_customers;

-- Transactions
-- Write a transaction that inserts a new customer, adds their rental, and logs the payment – all atomically.

begin;
	insert into customer (customer_id, store_id, first_name, last_name, email, address_id, activebool, create_date)
	values (600, 1, 'john', 'doe', 'john.doe@example.com', 1, true, current_timestamp);
	
	insert into rental (rental_id, rental_date, inventory_id, customer_id, staff_id, last_update)
	values (20000, current_timestamp, 1, 600, 1, current_timestamp);
	
	insert into payment (customer_id, staff_id, rental_id, amount, payment_date)
	values (600, 1, 10000, 4.99, NOW());
commit;

 
-- Simulate a transaction where one update fails (e.g., invalid rental ID), and ensure the entire transaction rolls back.
begin;
	update payment set amount = 10 where payment_id = 17629;
	update rental set return_date = now() where rental_id = 999999; -- invalid id
rollback;

 
-- Use SAVEPOINT to update multiple payment amounts. Roll back only one payment update using ROLLBACK TO SAVEPOINT.

begin;
	update payment set amount = amount + 5 where payment_id = 17830;
	savepoint s1;
	update payment set amount = amount + 10 where payment_id = 17580;
	rollback to savepoint s1;
commit;

-- select * from payment;

 
-- Perform a transaction that transfers inventory from one store to another (delete + insert) safely.
begin;
	delete from inventory where inventory_id = 10;
	insert into inventory (inventory_id, film_id, store_id, last_update)
	values (10, 5, 2, now());
commit;

 
-- Create a transaction that deletes a customer and all associated records (rental, payment), ensuring referential integrity.

begin;
	delete from payment where customer_id = 5;
	delete from rental where customer_id = 5;
	delete from customer where customer_id = 5;
commit;


-- Triggers
-- Create a trigger to prevent inserting payments of zero or negative amount.

select * from payment order by payment_id;

create or replace function trg_prev_invalid_payments () returns trigger as $$
begin
	if new.amount <= 0 then
        raise exception 'Payment amount must be positive';
    end if;
    return new;
end;
$$ language plpgsql;

create trigger trg_checkPaymentAmt
before insert on payment for each row execute function trg_prev_invalid_payments();


 
-- Set up a trigger that automatically updates last_update on the film table when the title or rental rate is changed.

select * from film;

create or replace function func_ensureUpdate() returns trigger as $$
begin
	if(new.title <> old.title or new.rental_rate <> old.rental_rate) then
		new.last_update := current_timestamp;
	end if;
	return new;
end;
$$ language plpgsql;

create trigger trg_ensureUpdate 
before update on film for each row execute function func_ensureUpdate();

 
-- Write a trigger that inserts a log into rental_log whenever a film is rented more than 3 times in a week.

create table rental_log(id serial primary key, film_id int, time_stamp timestamp default current_timestamp);

create or replace function func_rental_log() returns trigger as $$
declare film_count int;
begin
	select count(*) into film_count
	    from rental r
	    join inventory i on r.inventory_id = i.inventory_id
	    where i.film_id = (select film_id from inventory where inventory_id = NEW.inventory_id limit 1)
      	and r.rental_date >= current_date - interval '7 days';
	if(film_count > 3) then
		insert into rental_log(film_id) values(
			(select film_id from inventory where inventory_id = NEW.inventory_id limit 1)
		);
	end if;
	return new;
end;
$$ language plpgsql;


create trigger trg_AddFreqRented after insert on rental 
for each row execute function func_rental_log(); 