/* GroupBy1.sql - Group By  */

/* See Books Online for detailed info ---> Group By, Overview...

GROUP BY is used to gather together rows that have the same value in one or
more rows.  Once the rows have been "grouped" together in memory, aggregate
functions are often used to produce a single resulting value for "grouped" values
in one or more columns.

The optional HAVING clause can be used to filter "grouped" results.  

--------------------------------------------------------------------------------
Note If the ORDER BY clause is not specified, the results returned using the GROUP BY 
clause are not in any particular order. It is recommended that you always use 
the ORDER BY clause to specify a particular ordering of the data. 
-------------------------------------------------------------------------------- 

Examples  */
USE Pubs
/* look at a sample table:  */
SELECT * from titleauthor
order by title_id
go

SELECT Title_Id, Count(title_id) 'Count of Title_id instances'/* produce a count for each group */
FROM titleauthor
GROUP BY title_id /* groups in this case, will be specific values in the title_id col */
HAVING count(title_id)>1 /* when >1 row exists with a given value in title_id col */
GO

/* ----------------------------------next example..------------------------------------- */
USE Northwind
SELECT * from Products /* look at the table */
order by CategoryId,ProductId
go

--can group by Categories and apply aggregate functions to the groups of data:
SELECT CategoryID, AVG(UnitPrice) 'Average UnitPrice For CategoryId'
FROM Products
GROUP BY CategoryID /* display average Unit Price for each Category */
go

/* ----------------------------------next example..------------------------------------- */
USE Northwind
SELECT * from Customers /* look at this table */
order by Region
go

SELECT Region,Count(City) /* count the number of rows with City values for each region */
FROM Customers  /* for example, look at the "OR" region; */
GROUP BY Region /* there are 4 values in the City col */
go

/* you can also group based on combinations of values from columns; for example...  */
/* first look at the Products table...  */
USE Northwind
SELECT * from Products
go

--now look at the groups based on SupplierId,CategoryId combinations:
SELECT SupplierID,CategoryID, 'Average Price'=AVG(UnitPrice),
'Summary of Units in Stock'=SUM(UnitsInStock)
FROM Products
GROUP BY SupplierID,CategoryID
go

/* this yields a result set that is hard to read because it is not well ordered!  
Let's fix this by adding an ORDER BY clause to the result set...  */
SELECT SupplierID,CategoryID, 'Average Price'=AVG(UnitPrice),
'Summary of Units in Stock'=SUM(UnitsInStock)
FROM Products
GROUP BY SupplierID,CategoryID
ORDER BY SupplierID,CategoryID /* order by CategoryId within SupplierId
--note: ORDER BY if used, must appear after GROUP BY */
go

/* the ALL keyword can affect results when a WHERE clause is involved.  To 
demonstrate this by example, first look at a few cols from 
the Products table: */
USE Northwind
SELECT CategoryId,UnitPrice,UnitsInStock from Products
WHERE UnitsInStock>100
go

--note that some CategoryId values appeared in more than 1 row of the result set.

/* now look at the Average Unit Price for each Category ID: */
SELECT CategoryID,'Average Unit Price in Category'=AVG(UnitPrice)
FROM Products
WHERE UnitsInStock>100
GROUP BY CategoryID

/* now with ALL..  */
SELECT CategoryID, 'Average Unit Price in Category'=AVG(UnitPrice)
FROM Products
WHERE UnitsInStock>100
GROUP BY ALL CategoryID /* using the ALL keyword means list ALL of the CategoryId
values found in the table; values that are not amongst those meeting the
condition in the WHERE clause return a NULL result for the average */
go












  




