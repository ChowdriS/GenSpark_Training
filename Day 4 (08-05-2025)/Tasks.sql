use master;
--select * from Employees;

--1) List all orders with the customer name and the employee who handled the order.
select o.OrderID order_id,concat(e.FirstName,' ',e.LastName) Employee_name , c.ContactName
from Employees e 
join Orders o on e.EmployeeID = o.EmployeeID
join Customers c on c.CustomerID = o.CustomerID;

--2) Get a list of products along with their category and supplier name.

select p.ProductName ProductName, c.CategoryName CategoryName, s.CompanyName CompanyName
from Products p 
join Suppliers s on p.SupplierID = s.SupplierID
join Categories c on p.CategoryID =c.CategoryID;

--3) Show all orders and the products included in each order with quantity and unit price.

select o.OrderID, p.ProductName, p.UnitPrice, d.Quantity
from Orders o
join [Order Details] d on o.OrderID = d.OrderID
join Products p on d.ProductID = p.ProductID;

--4) List employees who report to other employees (manager-subordinate relationship).

select concat(firstname,'',LastName) as Name from Employees where ReportsTo is not null;
--or 
select concat(e.firstname,'',e.LastName) as EmployeeName, 
concat(m.firstname,'',m.LastName) as ManagerName
from Employees e
join Employees m 
on e.ReportsTo = m.EmployeeID;

--5) Display each customer and their total order count.

select c.ContactName, count(*) as OrdersCount
from Customers c 
join Orders o on c.CustomerID = o.CustomerID
group by c.contactName;


--6) Find the average unit price of products per category.

select CategoryName,avg(p.UnitPrice) avg_UnitPrice 
from Products p 
join Categories c on p.CategoryID = c.CategoryID 
group by c.CategoryName;

--7) List customers where the contact title starts with 'Owner'.

select ContactName from Customers where ContactName like 'Owner%';

--8) Show the top 5 most expensive products.

select top 5 ProductID,ProductName,UnitPrice from products order by UnitPrice desc;

--9) Return the total sales amount (quantity × unit price) per order.

select orderid,sum(unitprice*quantity) as tot_amt from [Order Details] 
group by OrderID;

--10) Create a stored procedure that returns all orders for a given customer ID.

create or alter proc proc_fetchOrders 
(@id nvarchar(16)) as
begin
	select orderid from orders where CustomerID = @id;
end

exec proc_fetchOrders @id = 'SUPRD'


--11) Write a stored procedure that inserts a new product.

create or alter proc proc_addProduct
(@jsondata nvarchar(max)) as
begin
	insert into Products
	select pname,SupplierID,CategoryID,QuantityPerUnit,unitprice,
	unitinstock,unitsonorder,reorderlevel,discontinued
	from openjson(@jsondata)
	with (
		pname nvarchar(40),SupplierID int ,CategoryID int,QuantityPerUnit nvarchar(20),
		unitprice money,unitinstock smallint ,unitsonorder smallint,
		reorderlevel smallint,discontinued bit
	)
end

exec proc_addProduct @jsondata = '{
    "pname": "Tea",
    "SupplierID": 1,
    "CategoryID": 1,
    "QuantityPerUnit": "10 boxes x 20 bags",
    "unitprice": 18.00,
    "unitinstock": 39,
    "unitsonorder": 0,
    "reorderlevel": 10,
    "discontinued": 0
}'
--select * from Products


--12) Create a stored procedure that returns total sales per employee.

create or alter proc proc_getTotalSales (@id int) as
begin
	select EmployeeID, sum(tot_amt) as tot_amt from (select orderid,sum(unitprice*quantity) as tot_amt from [Order Details] 
	group by OrderID) as d join orders o on d.OrderID = o.Orderid
	where o.EmployeeID = @id
	group by EmployeeID;
end

exec proc_getTotalSales @id = '3'


--13) Use a CTE to rank products by unit price within each category.

with cte_rankProduct as (
	select ProductName,UnitPrice,categoryid, rank() 
	over(partition by categoryid order by unitprice desc) 
	as CatrgoryWiseRank from Products
)

select * from cte_rankProduct;

--14) Create a CTE to calculate total revenue per product and filter 
--products with revenue > 10,000.

 select ProductID,revenue from 
(select ProductID,sum(quantity*UnitPrice) revenue
from [Order Details]
group by ProductID) as agg_table 
where revenue > 10000

--15) Use a CTE with recursion to display employee hierarchy.

with cte_hierarchy as (
	select EmployeeID, concat(firstname,' ',lastname) as fullname,
	ReportsTo as manager_id, 1 as level from Employees where ReportsTo is null

	union all

	select e.EmployeeID, concat(e.firstname,' ',e.lastname) as fullname,
	e.ReportsTo as manager_id, ch.level + 1 from Employees e 
	join cte_hierarchy ch on e.ReportsTo = ch.EmployeeID
)

select * from cte_hierarchy;