/* Select5.sql */

--------------------------------------------------
--TOP keyword is used to limit the number of rows returned

--example query
use Northwind
SELECT CategoryId,ProductName,ProductSales
FROM [Sales By Category]
ORDER BY ProductSales
go

--now, to return only the first 5 rows from the last result set:
SELECT TOP 5 CategoryId,ProductName,ProductSales
FROM [Sales By Category]
ORDER BY ProductSales
go

--to organize results according to ProductSales col values such that the largest 5
--values appear:
SELECT TOP 5 CategoryId,ProductName,ProductSales
FROM [Sales By Category]
ORDER BY ProductSales DESC --descending instead of default ascending
go

--------------------------------------------------
--next...consider a duplicate result situation and how WITH TIES can be used
--first, look at entire result set returned from:
SELECT OrderId,Quantity
FROM [Order Details]
ORDER BY Quantity DESC
go

--now return only the first 3 rows:
SELECT TOP 3 OrderId,Quantity
FROM [Order Details]
ORDER BY Quantity DESC
go

/*using WITH TIES causs query to output rows until either the specified number of rows
has been reached, or allthe results sharing the same value as the last specified record 
have been returned, ie: try the following query: */

SELECT TOP 3 WITH TIES OrderId,Quantity
FROM [Order Details]
ORDER BY Quantity DESC
go

--------------------------------------------------
--see PERCENT in Books Online
--example:
--first, look at the result set from the following (note how many rows are returned):
SELECT CategoryId,ProductName,ProductSales
FROM [Sales By Category]
ORDER BY ProductSales DESC --descending instead of default ascending
go

--now to return one-tenth (or nearly one-tenth if rowcount is not divisible by 10) of the rows:
SELECT TOP 10 PERCENT CategoryId,ProductName,ProductSales
FROM [Sales By Category]
ORDER BY ProductSales DESC --descending instead of default ascending
go

-------------------------------------------------
/*The DISTINCT keyword eliminates duplicate rows from the results of a SELECT statement.
See DISTINCT KEYWORD in books online. */
--first, see the result set produced from:
SELECT CategoryId,CategoryName,ProductName,ProductSales
FROM [Sales By Category]
go

--next, look at the CategoryName col only to produce many duplicates in the result set:
SELECT CategoryName
FROM [Sales By Category]
go

--using DISTINCT can eliminate duplicate rows (in this case, the rows are only 1 col wide):
SELECT DISTINCT CategoryName
FROM [Sales By Category]
go

-------------------------------------------------
--NESTED SELECT statements (subqueries)
--examples:
--first consider this result set:
SELECT CustomerId,CompanyName FROM Customers
go

--next, list all the different CustomerId values in the Orders table for rows with value 
--of USA in the ShipCountry col:
SELECT DISTINCT CustomerId FROM Orders
WHERE ShipCountry='USA'
go
--note this query has returned a set of results that can now be used in combination
--with the first query, ie:

SELECT CustomerId,CompanyName FROM Customers
WHERE CustomerId IN (
							SELECT DISTINCT CustomerId FROM Orders
							WHERE ShipCountry='USA'
							)
go
--the only rows in the result set (based on data retrieved from the Customers table)
--are those that contain CustomerId values found in rows of the Orders table having
--the value 'USA' in the ShipCountry col

--the 2nd query is called the inner select (or a subquery)

--the IN keyword above determines if a given value matches any value in the subquery 

-------------------------------------------------------------------------
--Subqueries with NOT IN
/*Subqueries introduced with the keyword NOT IN also return a list of zero or more 
values. This query finds the names of the publishers who have not published business 
books.*/

USE pubs
SELECT pub_name
FROM publishers
WHERE pub_id NOT IN
   (SELECT pub_id
   FROM titles
   WHERE type = 'business')
go

-------------------------------------------------------------------------
--Subqueries with EXISTS
/*When a subquery is introduced with the keyword EXISTS, it functions as an 
existence test. The WHERE clause of the outer query tests for the existence of 
rows returned by the subquery. The subquery does not actually produce any data; 
it returns a value of TRUE or FALSE.

A subquery introduced with EXISTS has the following syntax:

WHERE [NOT] EXISTS (subquery)

This example query finds the names of all publishers who publish business books: */

USE pubs
SELECT pub_name
FROM publishers
WHERE EXISTS
   (SELECT *
   FROM titles
   WHERE pub_id = publishers.pub_id
      AND type = 'business')
go
--note the fully qualified name above: publishers.pub_id
--this is a way of specifying table.column

------------------------------------------------------------------
--another example:
use Northwind
SELECT CompanyName FROM Customers
WHERE EXISTS (
				SELECT DISTINCT CustomerId FROM Orders
				WHERE ShipCountry='USA' AND Orders.CustomerId = Customers.CustomerId
				)
go

--When choosing between IN and EXISTS, use EXISTS if possible as it will yield a higher
--performance solution

----------------------------------------------------------------

--See script example file "Exists.sql" for more examples and explanations!








