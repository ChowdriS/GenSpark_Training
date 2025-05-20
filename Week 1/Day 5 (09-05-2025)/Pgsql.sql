-- SELECT Queries
-- List all films with their length and rental rate, sorted by length descending.
-- Columns: title, length, rental_rate

SELECT title, length, rental_rate
FROM film
ORDER BY length DESC;

-- Find the top 5 customers who have rented the most films.
-- Hint: Use the rental and customer tables.
select c.customer_id, count(r.rental_id) as total_rentals
from customer c
join rental r on c.customer_id = r.customer_id
group by c.customer_id
order by total_rentals desc
limit 5;


-- Display all films that have never been rented.
-- Hint: Use LEFT JOIN between film and inventory → rental.

select f.title as films_neverRented
from film f
left join inventory i on f.film_id = i.film_id
left join rental r on i.inventory_id = r.inventory_id
where r.rental_id is null;



-- JOIN Queries
-- List all actors who appeared in the film ‘Academy Dinosaur’.
-- Tables: film, film_actor, actor
select concat(a.first_name,' ',a.last_name) as Name
from actor a
join film_actor fa on a.actor_id = fa.actor_id
join film f on fa.film_id = f.film_id
where f.title = 'academy dinosaur';



-- List each customer along with the total number of rentals they made and the total amount paid.
-- Tables: customer, rental, payment

select concat(c.first_name,' ',c.last_name) as customer_name,
   count(r.rental_id) as total_rentals,
   sum(p.amount) as total_paid
from customer c
join rental r on c.customer_id = r.customer_id
join payment p on r.rental_id = p.rental_id
group by c.customer_id order by total_rentals desc;


-- CTE-Based Queries
-- Using a CTE, show the top 3 rented movies by number of rentals.
-- Columns: title, rental_count

with cte_topRental as(
	select f.film_id as film_id, count(r.rental_id) as tot_rent
	from inventory i
	join rental r on i.inventory_id = r.inventory_id
	join film f on i.film_id = f.film_id
	group by f.film_id
	order by tot_rent desc
)

select * from cte_topRental limit 3;

-- Find customers who have rented more than the average number of films.
-- Use a CTE to compute the average rentals per customer, then filter.

with customer_rentals as (
  select customer_id, count(*) as rental_count
  from rental
  group by customer_id
), avg_rentals as (
  select avg(rental_count) as avg_rental
  from customer_rentals
)
select c.first_name, c.last_name, cr.rental_count
from customer c
join customer_rentals cr on c.customer_id = cr.customer_id,
     avg_rentals ar
where cr.rental_count > ar.avg_rental;


--  Function Questions
-- Write a function that returns the total number of rentals for a given customer ID.
-- Function: get_total_rentals(customer_id INT)

create or replace function get_total_rentals(customer_id int)
returns int as $$
declare
  total int;
begin
  select count(*) into total
  from rental
  where rental.customer_id = get_total_rentals.customer_id;

  return total;
end;
$$ language plpgsql;

select get_total_rentals(1);

-- Stored Procedure Questions
-- Write a stored procedure that updates the rental rate of a film by film ID and new rate.
-- Procedure: update_rental_rate(film_id INT, new_rate NUMERIC)

create or replace procedure update_rental_rate(film_id int, new_rate numeric)
language plpgsql
as $$
begin
  update film
  set rental_rate = new_rate
  where film_id = update_rental_rate.film_id;
end;
$$;

call update_rental_rate(10, 5.99);


-- Write a procedure to list overdue rentals (return date is NULL and rental date older than 7 days).
-- Procedure: get_overdue_rentals() that selects relevant columns.

create or replace procedure get_overdue_rentals()
language plpgsql
as $$
begin
  create temp table overdue_rentals (
  rental_id int,
  customer_id int,
  inventory_id int,
  rental_date timestamp
	);
  insert into overdue_rentals
  select r.rental_id, r.customer_id, r.inventory_id, r.rental_date
  from rental r
  where r.return_date is null
    and r.rental_date < now() - interval '7 days';
end;
$$;

call get_overdue_rentals();

-- drop table overdue_rentals;
select * from overdue_rentals;