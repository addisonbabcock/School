use pubs

--Question 1
select au_lname, phone
from authors

--Question 2
select stor_name 'Store Name', city 'Store City', state 'State' 
from stores

--Question 3
select title_id 'Title Identification Number', price 'Price', advance 'Advance', royalty 'Royalty' 
from titles

--Question 4
select fname 'First Name', lname 'Last Name'
-- fname + ' ' + lname
-- lname + ', ' + lname
from employee
where hire_date >= 'Jan 01, 1991'
--where year (hire_date) > 1990
--where datepart (yy, hire_date) > 1990

use Northwind

--Question 5
SELECT RequiredDate 'Required Date', ShippedDate 'Shipped Date', EmployeeId 'Employee ID' 
FROM Orders
WHERE CustomerId = 'BERGS'

--Question 6
SELECT CompanyName 'Company Name', Address 'Address', City 'City', Region 'Region'
FROM Customers
WHERE CompanyName LIKE 'Fr%'
--the server is not case sensitive

--Question 7
SELECT OrderDate 'Ordered Date', ShippedDate 'Shipped Date', OrderId 'Order ID'
FROM Orders
WHERE OrderDate BETWEEN 'July 4, 1996' AND 'Sep 3, 1996'
ORDER BY OrderDate ASC

--Question 8
SELECT LastName 'Last Name', FirstName 'First Name'
FROM Employees
WHERE City = 'London' OR City = 'Seattle'
--where city in ('london','seattle')
ORDER BY LastName ASC, FirstName ASC

--Question 9
SELECT City 'City', CompanyName 'Company Name', Region 'Region'
FROM Suppliers
WHERE Region IS NULL --do not use "[bla bla bla] = null"
ORDER BY City ASC, CompanyName ASC

--Question 10
SELECT OrderId 'Order ID', (UnitPrice - Discount) * Quantity as 'Total Cost of Order'
--UnitPrice*Quantity - Discount*(UnitPrice*Quantity) 'Total Cost'
--(1 - discount) * (unitprice * quantity)
FROM "Order Details"