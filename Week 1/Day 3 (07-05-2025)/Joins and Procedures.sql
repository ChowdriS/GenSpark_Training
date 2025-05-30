use pubs;

-- Print the publisher's name, book name and the order date of the  books
-- SELECT pub_name Publisher_Name, title Book_Name, ord_date Order_Date
-- FROM publishers p JOIN titles t ON p.pub_id = t.pub_id
-- JOIN sales s ON s.title_id = t.title_id
-- order by 3 desc


-- Print the publisher name and the first book sale date for all the publishers
with JoinedTable As (
	select a.pub_id, pub_name, title, title_id from publishers a join titles t 
	on a.pub_id = t.pub_id
)

select pub_name, title, sale_date from 
( 
	select pub_name, title, ord_date sale_date, 
	row_number() over (partition by j.pub_id order by ord_date) AS sale_rank 
	from JoinedTable j join sales s
	on j.title_id = s.title_id
) final_table
where sale_rank=1;


print the bookname and the store address of the sale
select * from 
(
	select t.title,s.stor_id from 
	titles t join sales s on t.title_id = s.title_id 
) as j
join stores st on j.stor_id = st.stor_id;

use pubs;
select * from titles;

create procedure proc_sample (
	@title_id int,
	@title varchar(32) output ) as
begin
	select top 1 @title = pub_name from publishers where pub_id = @title_id
end;

drop procedure proc_sample;

declare @title_out varchar(32);

exec proc_sample 
	@title_id = 1389,
	@title=@title_out output;

select @title_out;


select top 1 pub_name from publishers where pub_id = 1389;

CREATE TABLE Product (
   id INT IDENTITY(1,1) PRIMARY KEY,
   productSpec VARCHAR(MAX)
);

drop proc InsertProduct

CREATE PROCEDURE InsertProduct
   @productSpec NVARCHAR(MAX)
AS
BEGIN
   INSERT INTO Product (productSpec)
   VALUES (@productSpec);
END;
GO

CREATE PROCEDURE UpdateProductSpec
   @id INT,
   @key VARCHAR(50),
   @newValue VARCHAR(100)
AS
BEGIN
   UPDATE Product
   SET productSpec = JSON_MODIFY(productSpec, '$.' + @key, @newValue)
   WHERE id = @id;
END;
GO

EXEC InsertProduct 
    @productSpec = '{"brand": "Dell", "ram": "16GB", "cpu": "i7"}';


EXEC UpdateProductSpec 
@id = 1, 
@key = 'ram', 
@newValue = '32GB';


select * from product;


drop table product;

DECLARE @jsondataa NVARCHAR(MAX) = '
[
 {"id": 1, "brand": "Dell", "ram": "16GB", "cpu": "i7"},
 {"id": 2, "brand": "HP", "ram": "8GB", "cpu": "i5"},
 {"id": 3, "brand": "Lenovo", "ram": "32GB", "cpu": "i9"},
 {"id": 4, "brand": "Asus", "ram": "16GB", "cpu": "i7"}
]';



create table product (
	id int primary key,
	brand nvarchar(32),
	ram nvarchar(32),
	cpu nvarchar(32)
)

create proc proc_bulkInsert (
	@jsondata nvarchar(MAX)
) as
begin
	insert into product (id,brand,ram,cpu) 
	select id,brand,ram,cpu from openjson(@jsondata)
	with (
		id int,brand nvarchar(32), ram nvarchar(32), cpu nvarchar(32)
	)
end

exec proc_bulkInsert ;

delete from product;

select * from product;

drop table product;

CREATE TABLE Product (
   id INT IDENTITY(1,1) PRIMARY KEY,
   productSpec VARCHAR(MAX)
);

drop proc InsertProduct

CREATE PROCEDURE InsertProduct
   @productSpec NVARCHAR(MAX)
AS
BEGIN
   INSERT INTO Product (productSpec)
   VALUES (@productSpec);
END;
GO

EXEC InsertProduct 
   @productSpec = '{"brand": "Dell", "ram": "16GB", "cpu": "i7"}';

select * from product 
where
try_cast(JSON_VALUE(productSpec, '$.cpu') as nvarchar(16)) = 'i7';

create procedure proc_GetBrandId (
	@item_id int
) as
begin
	select brand from product where id = @item_id;
end

exec proc_GetBrandId 2;


create or alter procedure GetPostsbyUserid (@user_id int)
as 
begin
select * from Posts where user_id = @user_id
end
 
exec GetPostsbyUserid 1