-- proc to encrypt pass inp and return pass as output
-- proc to mask the given encrpt pass
-- proc that inserrt the data to new_customer table and calls this above 2 proc
-- proc to select the details of new_customer
-- proc to compare encry pass with encryp pass

/*
1. Create a stored procedure to encrypt a given text
Task: Write a stored procedure sp_encrypt_text that takes a plain text input (e.g., email or mobile number) and returns an encrypted version using PostgreSQL's pgcrypto extension.
Use pgp_sym_encrypt(text, key) from pgcrypto.

2. Create a stored procedure to compare two encrypted texts
Task: Write a procedure sp_compare_encrypted that takes two encrypted values and checks if they decrypt to the same plain text.
 
3. Create a stored procedure to partially mask a gi ven text
Task: Write a procedure sp_mask_text that:
Shows only the first 2 and last 2 characters of the input string
Masks the rest with *
E.g., input: 'john.doe@example.com' → output: 'jo***************om'
 
4. Create a procedure to insert into customer with encrypted email and masked name
Task:
Call sp_encrypt_text for email
Call sp_mask_text for first_name
Insert masked and encrypted values into the customer table
Use any valid address_id and store_id to satisfy FK constraints.
 
5. Create a procedure to fetch and display masked first_name and decrypted email for all customers
Task:
Write sp_read_customer_masked() that:
Loops through all rows
Decrypts email
Displays customer_id, masked first name, and decrypted email
*/

-- 1. Create a stored procedure to encrypt a given text
-- Task: Write a stored procedure sp_encrypt_text that takes a plain text input (e.g., email or mobile number) and returns an encrypted version using PostgreSQL's pgcrypto extension.

create extension if not exists pgcrypto;

create or replace function func_encrypt_text(p_input text, p_key text) 
returns bytea as $$
declare results bytea;
begin
	SELECT pgp_sym_encrypt(p_input, p_key) into results;
	raise notice '%',results;
	return results;
end; $$ language plpgsql;

-- do $$
-- declare a bytea; 
-- begin
-- a:= func_encrypt_text('chowdri','dummykey');
-- end;
-- $$ language plpgsql;

-- create or replace procedure proc_encrypt_text(
--     in p_input text,
--     in p_key text,
--     out p_result bytea
-- )
-- as $$
-- begin
--     select pgp_sym_encrypt(p_input, p_key) into p_result;
-- end;
-- $$ language plpgsql;


-- 2. Create a stored procedure to compare two encrypted texts
-- Task: Write a procedure sp_compare_encrypted that takes two encrypted values and checks if they decrypt to the same plain text.

create or replace function func_compare_encrypted(p_cipher1 bytea, p_cipher2 bytea, p_key text) 
returns boolean as $$
declare results boolean;
begin
	select pgp_sym_decrypt(p_cipher_1, p_key) = pgp_sym_decrypt(p_cipher_2, p_key) into results;
	return results;
end; $$ language plpgsql;

-- create or replace procedure proc_compare_encrypted(
--     in p_cipher1 bytea,
--     in p_cipher2 bytea,
--     in p_key text,
--     out p_result boolean
-- )
-- as $$
-- begin
--     select pgp_sym_decrypt(p_cipher1, p_key) = pgp_sym_decrypt(p_cipher2, p_key)
--     into p_result;
-- end;
-- $$ language plpgsql;

-- 3. Create a stored procedure to partially mask a gi ven text
-- Task: Write a procedure sp_mask_text that:

create or replace function func_mask_text(p_text text)
returns text as $$
declare
	n int := char_length(p_text);
begin
	if(n<=4) then	return p_text;
	end if;
	return substr(p_text, 1, 2) || repeat('*', n - 4) || substr(p_text, n - 1, 2); 
end; $$ language plpgsql;

select func_mask_text('chowdri');

-- create or replace procedure proc_mask_text(
--     in p_text text,
--     out p_result text
-- )
-- as $$
-- declare
--     n int := char_length(p_text);
-- begin
--     if n <= 4 then
--         p_result := p_text;
--     else
--         p_result := substr(p_text, 1, 2) || repeat('*', n - 4) || substr(p_text, n - 1, 2);
--     end if;
-- end;
-- $$ language plpgsql;


-- 4. Create a procedure to insert into customer with encrypted email and masked name

-- create table new_customer (id serial primary key, name text, email bytea);
-- drop table new_customer;
-- alter table customer alter column email text --> then encode and decode with base 64 to do things

create or replace procedure proc_insert_customer(
    p_name text,
    p_email text,
    p_key text
)
AS $$
declare
    masked_name text;
    enc_email bytea;
begin
    masked_name := func_mask_text(p_name);
    enc_email   := func_encrypt_text(p_email, p_key);

    insert into new_customer (name, email) values (masked_name, enc_email);
	-- insert statemnet for dvdrental db customer table given below,
	-- insert into customer (store_id, first_name, last_name, email, address_id, active, create_date)
    -- values (1,masked_name,p_lastname,enc_email,6, 1, CURRENT_DATE);
end;
$$ language plpgsql;

call proc_insert_customer('sakthivel', 'sakthivelradha@gmail.com','mySecret');

select * from new_customer;

-- 5. Create a procedure to fetch and display masked first_name and decrypted email for all customers
-- Task: Write sp_read_customer_masked()

create or replace procedure proc_select_customers (p_key text) as $$
declare customer_rec record;
	customer_cur cursor for
	select * from new_customer;
begin
	open customer_cur;
	fetch customer_cur into customer_rec;
	while found loop
		raise notice 'masked_name : %, decrypted_email : %', customer_rec.name, pgp_sym_decrypt(customer_rec.email,p_key);
		fetch customer_cur into customer_rec;
	end loop;
	close customer_cur;
end;
$$ language plpgsql;

CALL proc_select_customers('mySecret');