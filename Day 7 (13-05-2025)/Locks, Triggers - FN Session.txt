/*
Locking Mechanism
PostgreSQL automatically applies locks, but you can control them manually when needed.

Types of Locks

MVCC VS Locks
MVCC allows readers and writers to work together without blocking.
Locks are needed when multiple writers try to touch the same row or table.

Simple Rule of Locks
Readers don’t block each other.
Writers block other writers on the same row.


Row-Level Locking (Default Behavior) / Implicit Lock
Two Users updating the same row
-- Trans A
*/
BEGIN;
UPDATE products
SET price = 500
WHERE id = 1;
-- Trans A holds a lock on row id = 1

-- Trans B
BEGIN;
UPDATE products
SET price = 600
WHERE id = 1;

/*
Result:
B waits until A commits or rollbacks
Row Level Locking
*/

-- Table-Level Locks / Explicit Table Lock
1. ACCESS SHARE -- select
-- Allows reads and writes

2. ROW SHARE
-- SELECT ... FOR UPDATE -> lock the selected row for later update

BEGIN;
LOCK TABLE products
IN ACCESS SHARE MODE;
-- Allows other SELECTS, even INSERT/DELETE at the same time.

BEGIN;
LOCK TABLE products
IN ROW SHARE MODE;
-- SELECT .. FOR UPDATE, reads are allowed, conflicting row locks are blocked, writes allowed

3. EXCLUSIVE
-- Blocks writes (INSERT, UPDATE, DELETE) but allows reads (SELECT)

BEGIN;
LOCK TABLE products
IN EXCLUSIVE MODE;

4. ACCESS EXCLUSIVE  -- Most agressive lock 
-- Blocks everything, Used by ALTER TABLE, DROP TABLE, TRUNCATE, 
-- Internally used by DDL.


-- A
BEGIN;
LOCK TABLE products IN ACCESS EXCLUSIVE MODE;
-- Table is now fully locked!

-- B
SLEECT * FROM products;
-- B will wait until A commits or rollbacks.

-- Explicit Row Locks --> SELECT ... FOR UPDATE
-- A
BEGIN;
SELECT * FROM products
WHERE id = 1
FOR UPDATE;
-- Row id = 1 is now locked

-- B
BEGIN;
UPDATE products
SET price = 700
WHERE id = 1;
-- B is blocked until A finishes.

-- SELECT ... FOR UPDATE locks the row early so no one can change it midway.
-- Banking, Ticket Booking, Inventory Management Systems
/*
A deadlock happens when:
Transaction A waits for B
Transaction B waits for A
They both wait forever.

-- Trans A
*/
BEGIN;
UPDATE products
SET price = 500
WHERE id = 1;
-- A locks row 1

-- Trans B
BEGIN;
UPDATE products
SET price = 600
WHERE id = 2;
-- B locks row 2

-- Trans A
UPDATE products
SET price = 500
WHERE id = 2;
-- A locks row 2 (already locked by B)

-- Trans B
UPDATE products
SET price = 600
WHERE id = 1
--B locks row 1 (already locked by A)

/*
psql detects a deadlock
ERROR: deadlock detected
It automatically aborts a transaction to resolve deadlock.
*/

-- Advisory Lock / Custom Locks
-- Get a lock with ID 12345
SELECT pg_advisory_lock(12345);

-- critical ops

-- Releas the lock
SELECT pg_advisory_unlock(12345);


--------------------------------------------------------------------------------

create table product (id serial primary key, name varchar(32), price int, stock int);

-- drop table product;

insert into product (name,price,stock) values ('phone',10000,10),('laptop',20000,20);

select * from product;

-- 1. Two Concurrent Updates to Same Row — Observe Locking

-- session 1
begin;
update product set stock = stock - 1 where id = 2;

-- session 2
begin;
update product set stock = stock + 1 where id = 2;

-- Session 2 has to wait because Session 1 already locked that row.

-- 2. SELECT ... FOR UPDATE — Locking Rows for Safe Reads

-- session 1

begin;
select * from product where id = 1 for update ;

-- session 2

update prodcut set stock = stock - 1 where id = 1;

-- Locks the row with id = 1 so no other transaction can update/delete it.
-- Other sessions can still read it unless they also use FOR UPDATE

-- 3. Create a Deadlock

insert into product (name,price,stock) values ('watch',10000,10),('television',20000,20);

-- session 1
begin;
update product set stock = stock + 1 where id = 2;

-- session 2
begin;
update product set stock = stock + 1 where id = 1;

-- session 1
update product set stock = stock + 1 where id = 1;

-- session 2
update product set stock = stock + 1 where id = 2;

-- create a deadlock and pgsql detects and drops a transaction
-- ERROR: deadlock detected
-- DETAIL: Process 1234 waits for ExclusiveLock on tuple...

	
-- 4 .Monitor Locks with pg_locks

SELECT pid, relation::regclass, mode, granted, query
FROM pg_locks
JOIN pg_stat_activity USING (pid)
WHERE relation IS NOT NULL;

-- 5.Explore PostgreSQL Lock Modes
-- Lock Mode, 			Purpose, 						Conflicts With
-- ACCESS SHARE, 		Normal SELECT queries, 			None
-- ROW SHARE, 			SELECT ... FOR UPDATE, 			EXCLUSIVE, ACCESS EXCLUSIVE
-- ROW EXCLUSIVE, 		INSERT, UPDATE, DELETE, 		Many higher modes
-- SHARE, 				Used for some advisory locks, 	Conflicts with UPDATE/DELETE (not common)
-- SHARE ROW EXCLUSIVE, Rarely used, 					Conflicts with most write locks
-- EXCLUSIVE,			VACUUM, 						Conflicts with reads and writes
-- ACCESS EXCLUSIVE, 	ALTER TABLE, 					DROP TABLE, Conflicts with everything


-----


-- Trigger

create table audit_log (id serial, tablename varchar(32), operation varchar(16));

create or replace function update_logs () returns trigger as $$
begin
insert into audit_log (tablename, operation) values(TG_TABLE_NAME,TG_OP );
return new;
end;
$$ language plpgsql;

create trigger trg_update_customer 
before update
on customer for each row execute function update_logs();

select * from customer;
select * from audit_log;
update customer set last_name = 'chow3' where customer_id = 1;


----------------------------


CREATE TABLE tab_audit_log (
    id SERIAL PRIMARY KEY,
    table_name TEXT,
    field_name TEXT,
    old_value TEXT,
    new_value TEXT,
    updated_date TIMESTAMP DEFAULT now()
  );

CREATE OR REPLACE FUNCTION func_update_audit_log()
  RETURNS TRIGGER AS
$$
DECLARE
  col_name text   := TG_ARGV[0]; 
  tab_name text   := TG_TABLE_NAME;
  o_value  text;
  n_value  text;
BEGIN
  o_value := row_to_json(OLD)->>col_name;
  n_value := row_to_json(NEW)->>col_name;

  IF o_value IS DISTINCT FROM n_value THEN
    INSERT INTO tab_audit_log(
      table_name, field_name, old_value, new_value, updated_date
    ) VALUES (
      tab_name, col_name, o_value, n_value, now()
    );
  END IF;

  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_audit_customer_email
  BEFORE UPDATE ON customer
  FOR EACH ROW
  EXECUTE FUNCTION func_update_audit_log('email');

select * from customer order by customer_id;
select * from tab_audit_log;
update customer set email = 'will@customer.org' where customer_id = 1;

