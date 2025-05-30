use pubs;

select * from product;

SELECT *
FROM product
WHERE JSON_VALUE(productSpec, '$.cpu') = 'i7';

CREATE OR ALTER PROCEDURE proc_filterProduct(
   @cpu NVARCHAR(16),
   @total INT OUTPUT
)
AS
BEGIN
   SELECT @total = COUNT(*)
   FROM product
   WHERE JSON_VALUE(productSpec, '$.cpu') = @cpu;
END;

drop proc proc_filterProduct;

DECLARE @tot INT;

EXEC proc_filterProduct
   @cpu = 'i7',
   @total = @tot output;

SELECT @tot AS total;

create table users
( id int primary key, name nvarchar(16), age int);

drop table users;

create proc proc_bulkInsert_using_csv
( @filepath nvarchar(32) ) as
begin
	declare @query NVARCHAR(MAX);
	set @query = 'bulk insert users from
	'''+@filepath+'''
	WITH (
       FIRSTROW = 2,
       FIELDTERMINATOR = '','',
       ROWTERMINATOR = ''\n''
   )'

	exec sp_executesql @query;
	BULK INSERT users
	FROM @filepath 
	WITH ( FIELDTERMINATOR = ',',
	   ROWTERMINATOR = '\n',     
	   FIRSTROW = 2
	)

end

drop proc proc_bulkInsert_using_csv

EXEC proc_bulkInsert_using_csv 
   @filepath = N'C:\GenSpark_Training\Day4(8-05-2025)\csv\Data.csv';

select * from users;

CREATE TABLE TaskLog (
   TaskName NVARCHAR(255),
   Status NVARCHAR(50),
   TaskCompletionDate DATETIME
);

CREATE PROCEDURE LogTaskStatus
   @TaskName NVARCHAR(255),  
   @Status NVARCHAR(50)      
AS
BEGIN
   INSERT INTO TaskLog (TaskName, Status, TaskCompletionDate)
   VALUES (@TaskName, @Status, GETDATE());

   PRINT 'Task status logged successfully.';
END;

EXEC LogTaskStatus 
	@TaskName= 'Task1',
	@Status ='Failed';

select * from TaskLog;

with cte_users as 
(
	select id, name 
	from users
)

select * from cte_users;

drop proc proc_pagination;

create or alter proc proc_pagination
(
	@page int, @size int
) as
begin
	select title, price, rowNumber from (select title, price, row_number() over(order by price) as rowNumber from titles) cte_pagination where rowNumber between (@page-1)*(@size+1) and @page*@size;
end

exec proc_pagination 
	@page = 2,
	@size = 10;

create or alter function func_addOne (@val int) returns int as
begin
	return @val + 1
end

select title, pub_id, dbo.func_addOne(pub_id) as next_pub_id from titles;

create function func_table_valued (@data int)
returns @result table(title varchar(64)) as
begin
	insert into @result select title from titles where pub_id = @data;
	return;
end

drop function func_table_valued;

select * from dbo.func_table_valued(1389);
