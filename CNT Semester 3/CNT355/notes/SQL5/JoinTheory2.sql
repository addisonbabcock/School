/* JoinTheory2.sql

Recall Joins can be used to retrieve data from 2 or more tables.  The result set produced 
depends upon the type of JOIN (several kinds exist) , but generally speaking
JOINS are used to gather data from multiple tables into one result set.

Examples: */

USE Pubs
PRINT 'PUBLISHERS TABLE:'
SELECT * from Publishers /* look at Publishers Table */
GO
PRINT 'PUBLISHER INFO TABLE:'
SELECT * from Pub_info /* look at Publishers Table */
GO

PRINT ''
PRINT 'INFO FROM BOTH TABLES FOR GIVEN pub_id values existing in both tables:'
SELECT *
FROM Publishers  /* Join between this table and.. */
	INNER JOIN Pub_info /* the Pub_info table */
	ON publishers.pub_id=pub_info.pub_id /* on condition that a given value in the pub_id
column is found in BOTH tables... intersection is the key concept to INNER JOINS */
go

/* next.. */
Use Northwind
SELECT * from Customers
ORDER by CustomerID
go
SELECT * from Orders
ORDER by CustomerID
go

/* to create a results table with columns from both tables where the value in the 
CustomerID column was the same: */
SELECT *
FROM Customers 
	JOIN Orders /* an INNER JOIN is default if not specified */
	ON (Customers.CustomerID=Orders.CustomerID)
go

/* generally speaking, it is best to declare the type of JOIN in your script, ie: */
/* an INNER JOIN between tables is the default if join type is not specified! */
SELECT *
FROM Customers 
	INNER JOIN Orders 
	ON (Customers.CustomerID=Orders.CustomerID) /* want all the rows from both these tables
	meeting the condition that a given CustomerID value exists in both tables */ 
go

/* next.. to find all the customers and suppliers located in the same City: */
PRINT 'Customers Table: '
PRINT ''
SELECT * from Customers
PRINT 'Suppliers Table: '
PRINT ''
SELECT * from Suppliers
go

PRINT ''
PRINT 'Result Set from JOIN: '
PRINT ''
SELECT Customers.CompanyName AS Customer, Suppliers.CompanyName AS Supplier, 
	Customers.City AS City
FROM Customers 
	INNER JOIN Suppliers /* getting data from Customers and Suppliers tables */
	ON Customers.City = Suppliers.City /* meeting condition that a given City value exists 
	in both tables */
go

/* next: */
USE Northwind
SELECT * FROM Orders
go

SELECT DISTINCT c.CustomerId,c.CompanyName,o.OrderId 
/* DISTINCT eliminates redundant ROWS in the virtual table we are creating.. where
a row corresponds to combinations of values from the various columns;  using distinct here
means no two rows in the result set will be exactly the same! */
FROM Customers c
	INNER JOIN Orders o
	ON c.CustomerId=o.CustomerId
GO

/* next.. can use logical operators with JOIN, ie: */
USE pubs
SELECT a.au_fname, a.au_lname, a.city as 'a.city',a.state as 'a.state',
	p.city as 'p.city',p.state as 'p.state', p.pub_name
FROM authors AS a 
	INNER JOIN publishers AS p
    ON a.city = p.city AND a.state = p.state /* 2 conditions determine a match */
ORDER BY a.au_lname ASC, a.au_fname ASC
go

/* INNER JOINS are often used between Primary key in one table, and a related Foreign key 
in another one.  Just because you can JOIN other eligible columns in 2 tables doesn't mean
you should; for example, it doesn't make sense to JOIN CustomerName with EmployeeAddress
even though these 2 columns might both be similar data types.  Whatever you do should
make sense. */

/* JOINS can be 'embedded' within JOINS.  Although you can solve many problems by linking just
2 tables, sometimes it is necessary to link 3 or more tables to get all the data required.

IT MAY BE PREFERABLE TO VIEW THE FOLLOWING RESULT SETS IN A GRID (see Query menu - set before
running the script).  Note that you can adjust the width of columns in the grid by sizing
at the top where the column identifiers are).  

Examples... */
/* the following is a JOIN example between 3 tables.. explained in stages: */
Use Pubs
/* 1. view tables and assign aliases */
PRINT 'Authors (alias = a):'
SELECT * FROM Authors a /* Authors is assigned an alias of 'a' for convenience */

PRINT 'TitleAuthor (ta):' /* once you have declared an alias, you must use that in any
references to the object */
SELECT * FROM TitleAuthor ta

PRINT 'Titles (t):'
SELECT * FROM Titles t
go

/* 2. the big picture for this example.. TitleAuthor can be viewed as a 'linking table' 
between Authors and Titles, ie:

Authors   1-----------N  TitleAuthor  N------------1  Titles */

/* 3.  next, pull data from the first 2 tables creating a 'virtual' table from
rows of both tables that contain Au_Id values that exist in both tables. */
USE Pubs
PRINT 'Authors-TitleAuthor JOIN Results:'
Select a.Au_Lname + ', ' + a.Au_Fname 'Authors-Name',a.Au_Id 'Authors.Au_Id',
	ta.Au_Id 'TitleAuthor.Au_Id',ta.title_id 'TitleAuthor.Title_Id'
From Authors a 
	INNER JOIN TitleAuthor ta 
	ON a.Au_Id = ta.Au_Id
GO

/* 4. Now create another virtual table joining the virtual table created above to Titles.  By
doing this we have gathered useful data from Authors and Titles together in one spot */
USE Pubs
PRINT 'Authors-TitleAuthor-Titles JOIN Results:'
Select a.Au_Lname + ', ' + a.Au_Fname 'Authors-Name',a.Au_Id 'Authors.Au_Id',
ta.Au_Id 'TitleAuthor.Au_Id',ta.Title_Id 'TitleAuthor.Title_Id',
t.Title_Id 'Titles.Title_Id',t.Title 'Titles.Title',t.Type 'Titles.Type'
From Authors a 
	INNER JOIN TitleAuthor ta 
	ON a.Au_Id = ta.Au_Id
		INNER JOIN Titles t
		ON ta.Title_Id=t.Title_Id
GO

/* another embedded JOIN example... suggestion:  it may also be helpful to look at the tables 
involved, perhaps sketching them (virtual tables too) to help understand this! Explanation
via script follows this code (below): */

USE Pubs
Print' '
Print'Authors with Royalty >= 14, via join'
print' '
Select distinct Au_Lname + ', ' + Au_Fname 'Authors-Name'
From Authors a 
	INNER JOIN TitleAuthor ta /* note use of aliases to reduce typing! */
	ON a.Au_Id = ta.Au_Id
		INNER join Titles t 
		ON ta.Title_Id = t.Title_Id
			INNER JOIN Roysched rs 
			ON t.Title_Id = rs.Title_Id
Where rs.Royalty >= 24
Go

/* EXPLANATION for the above script...use same approach as in previous example to
see how this works, ie:  */
/* 1st virtual table.. */
USE Pubs
Select distinct Au_Lname + ', ' + Au_Fname 'a-Name',a.au_id 'a.Au_Id',
	ta.au_id 'ta.Au_Id'
From Authors a 
	INNER JOIN TitleAuthor ta 
	ON a.Au_Id = ta.Au_Id /* result of this query = 1st virtual table */
Go

/* next, JOIN 1st virtual table with Titles (t):  */
USE Pubs
Select distinct Au_Lname + ', ' + Au_Fname 'a-Name',a.au_id 'a.Au_Id',
ta.au_id 'ta.Au_Id',ta.title_id 'ta.Title_Id',
t.Title_Id 't.Title_Id'
From Authors a 
	INNER JOIN TitleAuthor ta 
	ON a.Au_Id = ta.Au_Id
		INNER join Titles t
		ON ta.Title_Id = t.Title_Id /* result of this query = 2nd virtual table */
Go

/* next, JOIN 2nd virtual table with Roysched (rs):  */
USE Pubs
Select distinct Au_Lname + ', ' + Au_Fname 'a-Name',a.au_id 'a.Au_Id',
ta.au_id 'ta.Au_Id',ta.title_id 'ta.Title_Id',
t.Title_Id 't.Title_Id',
rs.title_id 'rs.Title_Id',rs.royalty 'rs.Royalty' /* can see Royalty value in Roysched now */
From Authors a 
	INNER JOIN TitleAuthor ta 
	ON a.Au_Id = ta.Au_Id
		INNER join Titles t
		ON ta.Title_Id = t.Title_Id /* 2nd virtual table is JOINED with.. */
			INNER JOIN Roysched rs /* Roysched table; match based on title_id values */
			ON t.Title_Id = rs.Title_Id
Go

/* lastly, add a WHERE clause to filter Royalties:  */
USE Pubs
Select distinct Au_Lname + ', ' + Au_Fname 'a-Name',a.au_id 'a.Au_Id',
ta.au_id 'ta.Au_Id',ta.title_id 'ta.Title_Id',
t.Title_Id 't.Title_Id',
rs.title_id 'rs.Title_Id',rs.royalty 'rs.Royalty' /* can see Royalty value in Roysched now */
From Authors a 
	INNER JOIN TitleAuthor ta 
	ON a.Au_Id = ta.Au_Id
		INNER join Titles t
		ON ta.Title_Id = t.Title_Id /* 2nd virtual table is JOINED with.. */
			INNER JOIN Roysched rs /* Roysched table; match based on title_id values */
			ON t.Title_Id = rs.Title_Id
Where rs.Royalty >= 24 /* filter to isolate rows we are interested in */
Go

/* *****************************OUTER JOINS***************************** 
While INNER JOINS are exclusive (ie, in that rows without a match are excluded), OUTER
JOINS are inclusive.  See Bks Online, Outer Joins, Using Outer Joins for detailed info.

examples: */

USE pubs
go

SELECT * from authors /* look at tables.. */
ORDER BY au_lname
go
SELECT * from publishers
go

/* The example below shows how to retrieve ALL rows from the left table 
(publishers in this case), and only matching rows (on city values) from the 
publishers table.  Note that NULLs will appear in the result set where values
do not exist in the second table for the specified match condition. */

SELECT a.au_lname + ' ' + a.au_fname AS [Author Name], a.city 'City Author Lives In',
	p.pub_name 'Publishers Name',p.city 'Publisher''s City' --how to display apostrophe
FROM authors a LEFT OUTER JOIN publishers p 
    ON a.city = p.city
ORDER BY a.au_lname ASC, a.au_fname ASC /* ordered by first name within last name */
go

/* Again, note... The LEFT OUTER JOIN above includes all rows in the authors table in the 
results, whether or not there is a match on the city column in the publishers table. 
Another way to look at this:  The keyword left specifies that the table on the left 
side of the 'LEFT' keyword is to be preserved, i.e., all of its rows are to survive the 
join operation. */

------------------------------------------------------
-- another example...

USE Pubs
Select * from Discounts /* first look at these tables */
Select * from Stores
go

/* next, a simple INNER JOIN: */

/* note that only one of the tables below contains DiscountType and Discount columns 
therefore the table does not need to be specified for these cols.  On the other hand,
the Stor_name col is present in both tables (so we need to be specific to avoid amiguity;
must use a qulaified name, ie: table.col format */
SELECT DiscountType,Discount,s.Stor_name 
FROM Discounts d
	INNER JOIN stores s /* result set shows Stor_id value(s) present in both tables */
	ON d.Stor_id=s.Stor_id
GO

----------------------------------------------------------------
/* next.. LEFT OUTER JOIN: */
/* result set for this query shows ALL row(s) present in Discounts
and only matching rows (based on Stor_id values) in Stores table: */

SELECT DiscountType,Discount,s.Stor_name 
FROM Discounts d
	LEFT OUTER JOIN stores s 
	ON d.Stor_id=s.Stor_id
GO
-------------------------------------------------------------------
/* next.. RIGHT OUTER JOIN; all rows in the second table are to be included in the results, 
regardless of whether there is matching data in the first table. */

SELECT DiscountType,Discount,s.Stor_name 
FROM Discounts d
	RIGHT OUTER JOIN stores s -- all data from stores will be in this result set
	ON d.Stor_id=s.Stor_id
GO

/* ****************FULL JOIN (FULL OUTER JOIN) *************************************** 
SEE Bks Online, JOINS, Full, before proceeding further. 

As stated in Bks Online, a full outer join returns all rows in both the left and right 
tables. Any time a row has no match in the other table, the select list columns from the 
other table contain null values. When there is a match between the tables, the entire result 
set rows contains data values from the base tables. Full JOINS are seldom used and
therefore they will not be emphasized here. */






