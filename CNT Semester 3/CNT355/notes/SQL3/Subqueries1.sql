/* Subqueries1.sql - Basic SUBQUERY EXAMPLES 

NOTE: further info on subqueries can be found in Books Online..
Index\Subqueries\Subquery Fundamentals

A subquery is a SELECT query nested inside a SELECT, INSERT, UPDATE, or DELETE statement, 
or inside another subquery. A subquery can be used anywhere an expression is allowed.

examples:  */

USE Pubs
PRINT 'Sales Table'
SELECT * FROM Sales /* take a look at Sales table */
go

PRINT 'Select results for subquery in next batch:'
SELECT Stor_Id FROM Stores /* first look at this result set */
	WHERE Stor_Name='BookBeat' /* as this select will be used for subquery below */
go

PRINT 'Now results using subquery:'
SELECT Ord_date 'Order Date',Qty 'Quantity' /* start of the outer query (also called
						outer select */
FROM Sales
WHERE Stor_Id=( 
	SELECT Stor_Id FROM Stores /* this inner query (also called outer select)
					returns value(s) to the outer query */
	WHERE Stor_Name='BookBeat' /* this example demonstrates the idea of 'nesting' */
	)
go

