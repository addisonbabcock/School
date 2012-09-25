/* Having1.sql

HAVING is used to filter groups UNLIKE WHERE which is used to filter rows.

Note that when grouping data, if it is possible to use a WHERE clause to
filter rows before grouping data, then YOU SHOULD DO SO! 

Books Online has detailed info ---> 
HAVING clause, Filtering Rows AND Overview sections.

example code: */

use Northwind
go

select * from [Order Details]
order by orderid

--the above query clearly shows the Order Details table to hold the details concerning
--what products go with a particular order, how many, and at what price

--2 take a look at a subset of the columns now and re-format the query so we only
--concern ourselves mostly with the financial details:
SELECT OrdD1.OrderID AS OrderID,
       OrdD1.Quantity AS "Quantity of Units",
       OrdD1.UnitPrice 'Unit Price'
FROM [Order Details] AS OrdD1
order by OrdD1.OrderID
go

/* 3 now restrict (filter) the results to zero-in on orders which INCLUDE items 
priced over $100 to help identify our most important customers (note in your 
result set you may also see unitprice values under $100 because these rows accompany
any OrderId values with unitprice values exceeding 100): */

SELECT OrdD1.OrderID AS OrderID,
       OrdD1.Quantity AS "Quantity of Units",
       OrdD1.UnitPrice 'Unit Price'
FROM [Order Details] AS OrdD1
WHERE OrdD1.OrderID in (SELECT DISTINCT OrdD2.OrderID
                        FROM [Order Details] AS OrdD2
                        WHERE OrdD2.UnitPrice > $100)
order by OrdD1.OrderID
go

/*note the Order Details table has been given 2 aliases above.  Trying to run
the same query with only 1 alias will also work (below), but using 2 aliases 
helps to separate the subquery code from the rest of the query */

SELECT OrdD1.OrderID AS OrderID,
       OrdD1.Quantity AS "Quantity of Units",
       OrdD1.UnitPrice 'Unit Price'
FROM [Order Details] AS OrdD1
WHERE OrdD1.OrderID in (SELECT DISTINCT OrdD1.OrderID
                        FROM [Order Details] AS OrdD1
                        WHERE OrdD1.UnitPrice > $100)
order by OrdD1.OrderID
go

--4 Next, we can group the rows based on OrderId value and apply aggregate functions
--to the grouped data:

SELECT OrdD1.OrderID AS OrderID,
       SUM(OrdD1.Quantity) AS "Units Sold",
       SUM(OrdD1.UnitPrice * OrdD1.Quantity) AS Revenue
FROM [Order Details] AS OrdD1
WHERE OrdD1.OrderID in (SELECT DISTINCT OrdD2.OrderID
                        FROM [Order Details] AS OrdD2
                        WHERE OrdD2.UnitPrice > $100)
GROUP BY OrdD1.OrderID
order by OrdD1.OrderID

--5 Next, we can use HAVING to filter the groups so only those with total quantities
--exceeding 100 are gathered into our result set:

SELECT OrdD1.OrderID AS OrderID,
       SUM(OrdD1.Quantity) AS "Units Sold",
       SUM(OrdD1.UnitPrice * OrdD1.Quantity) AS Revenue
FROM [Order Details] AS OrdD1
WHERE OrdD1.OrderID in (SELECT DISTINCT OrdD2.OrderID
                        FROM [Order Details] AS OrdD2
                        WHERE OrdD2.UnitPrice > $100)
GROUP BY OrdD1.OrderID
HAVING SUM(OrdD1.Quantity) > 100
order by OrdD1.OrderID

----------------------------------------------------------------------------------------
--more examples...this time use Pubs:

USE Pubs
go

SELECT * FROM TitleAuthor /* look at this table... */
order by title_id
go
--note multiple author id values can be associated with a given title id value
--to zero in on this aspect...

SELECT Title_id,COUNT(Title_id) AS 'Number of Authors Associated With Title'
FROM TitleAuthor /* show how many times a given Title_id appears in the table */
GROUP BY Title_id
go

--to restrict to only titles written by multiple authors:
SELECT Title_id,COUNT(Title_id) AS 'Number of Authors Associated With Title'
FROM TitleAuthor /* show how many times a given Title_id appears in the table */
GROUP BY Title_id
HAVING count(title_id)>1 /* provided it occurs more than once */
go

---NEXT EXAMPLE-------------------------------------------------
Use Northwind --NORTHWIND for this one..
go

SELECT * /* take a look at the table */  
FROM Orders /* note several rows have the same value in EmployeeId col */
order by employeeid --easier to see one employee can be associated with multiple orders
go

--to see how many orders an employee is associated with:
SELECT EmployeeId,Count(*)AS Number_Of_Orders /* see B.O.- * means count rows here*/ 
FROM Orders
GROUP BY  EmployeeId /* group rows by EmployeeId values */
/* GROUP BY is applied after WHERE if it is present */
go

/* to filter so we can zero in on employees with more than 50 orders.. 
give them a raise! */
SELECT EmployeeId,Count(*)AS Number_Of_Orders
FROM Orders
GROUP BY  EmployeeId
HAVING Count(*)>50 --rowcount > 50 in a given group
go 

/* a small modification allows us to see the top performers first: */
SELECT EmployeeId,Count(*)AS Number_Of_Orders
FROM Orders
WHERE EmployeeId>3
GROUP BY  EmployeeId
HAVING Count(*)>50 /* and return if there are more than 50 rows for an EmployeeId value */
ORDER BY Count(*) DESC /* list from largest number of orders to smallest */
go

-------------------------------------------------------------
/* next example...  */
USE Pubs --PUBS again!
go

SELECT * 
FROM Titles /* take a look at table first */
order by pub_id --note a given publisher can be associated with many titles (books)
go

--now zero in on selected information (shows multiple rows with same pub_id values)
SELECT title_id,pub_id,Advance,Price
FROM Titles
ORDER BY Pub_Id
go

--to summarize the data into a more convenient form, group data based on pub_id values.
/* find the SUM of values in Advance col, and AVG of values in Price col for each
group: */
SELECT pub_id,SUM(Advance) AS 'Total Advance',AVG(Price) AS 'Average Price of Titles'
FROM Titles 
GROUP BY Pub_Id /* calculations are on each set of values corresponding to a Pub_Id value */
go

--to filter the data to only groups with total advance exceeding 30000:
SELECT pub_id,SUM(Advance) AS 'Total Advance',AVG(Price) AS 'Average Price of Titles'
FROM Titles
GROUP BY Pub_Id 
HAVING SUM(Advance)>30000
go

/*the HAVING clause acts on rows AFTER they have been grouped; WHERE is applied 
to each row before it becomes part of a group to see whether or not it can be included
in the group. */
SELECT pub_id,SUM(Advance) AS Sum_Of_Advance,AVG(Price) AS Price
FROM Titles /* find the SUM of values in Advance col, and AVG of values in Price col */
WHERE Price>=5 /* using only rows with a value >=5 in the Price col to make up groups */
GROUP BY Pub_Id /* doing calculations for each set of rows corresponding to a Pub_Id value */
HAVING SUM(Advance)>15000 /* now more filtering... only return results when SUM 
of values in Advance col > 15k */
go


/* another example.. */
USE Northwind
go

SELECT * FROM Employees /* look at table */
order by ReportsTo
go

--note that multiple employees can report to a manager

--to see how many employees a manager actually manages:
SELECT ReportsTo AS [Manager Id], Count(*) AS 'Number of Employees Managed' /* give aliases for nice col headings
and count number of rows that exist in each group. */ 
FROM Employees
GROUP BY ReportsTo
ORDER BY ReportsTo
go

/* to see results for everybody except EmployeeId of 5  (run the query above AND
the query below to see the difference! */
SELECT ReportsTo AS Manager, Count(*) AS 'Number of Employees Managed' 
FROM Employees
WHERE EmployeeId != 5 /* means select rows that do NOT have value of 5 in Employee col */
GROUP BY ReportsTo
go

/* we can use HAVING as usual to further filter our data.. for example, to see 
which managers look after more than 3 employees: */
SELECT ReportsTo AS Manager, Count(*) AS 'Number of Employees Managed' 
FROM Employees
GROUP BY ReportsTo
HAVING Count(*)>3
go

-------------------------------------------------------------------------
/* another example.. */
USE Northwind
go

SELECT *
FROM [Order Details] --get familiar with table
order by OrderId
go

SELECT OrderId,SUM(Quantity) AS Total_Quantity
FROM [Order Details] /* want to see OrderId values and Sum of values in Quantity col.. */
GROUP BY OrderId /* ..for groups of rows based on specific OrderId values.. */
HAVING SUM(Quantity)>300 /* ..and filter after the groups have been formed; 
we only want to see groups with the sum in Quantity col of all rows in the group 
exceeding 300 */
go



