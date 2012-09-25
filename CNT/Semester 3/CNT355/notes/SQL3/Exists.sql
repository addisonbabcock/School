/* Exists.sql

When a subquery is introduced in combination with the keyword EXISTS, it functions as an 
existence test. Retrieving data using this technique as oposed to other ways of coding
a solution can offer performance improvements because the inner query does not actually
return any rows. */

--example:

USE Northwind

SELECT CategoryName
FROM Categories
WHERE EXISTS (SELECT NULL) 
go

/* In the simple example above, "SELECT NULL" always returns a 1 row result set (ie, NULL)
therefore the expression "EXISTS (SELECT NULL)" always evaluates to TRUE.  

Think of the above query as operating this way:  
rows are retrieved one-by-one from the Categories table (in this case consisting of 
values from only one column).  For each row being examined,
the WHERE clause is checked and if the condition in the where clause evaluates to true, 
the row being examined will become part of the returned result set. */

------------------------------------------------------------------------

/* if we want to yield results that are listed alphabetically, we can simply 
use an ORDER BY clause, ie: */

SELECT CategoryName
FROM Categories
WHERE EXISTS (SELECT NULL)
ORDER BY CategoryName ASC --ASC can be omitted as this is the default!
GO

------------------------------------------------------------------------
/* More than 1 condition can be specified in a subquery used with EXISTS, ie: */

Use Pubs

SELECT DISTINCT pub_name
FROM publishers
WHERE EXISTS
   (SELECT *
   FROM titles
   WHERE pub_id = publishers.pub_id
   AND type = 'business')

--IN THE ABOVE QUERY, THINK OF "publishers.pub_id" AS REPRESENTING THE pub_id VALUE
--PRESENT IN THE CURRENT ROW BEING EXAMINED IN THE PUBLISHERS TABLE!

--again, think of the above query as examining rows one at a time from the publishers
--table; whether or not the selected row is included in the result set depends upon
--whether or not the existence test evaluates to TRUE

----------------------------------------------------------------------
-- another way to get the same result set as the above query produced is to use 
--the IN clause, ie:

USE pubs

SELECT distinct pub_name
FROM publishers
WHERE pub_id IN
   (SELECT pub_id
   FROM titles
   WHERE type = 'business')
   
/* The disadvantage of this method is that performance is generally slower than 
what is achieved using the EXISTS technique */

-------------------------------------------------------
/* next are two queries to find authors who live in the same city as a publisher. 
Note that both queries return the same information. */

USE pubs
SELECT au_lname, au_fname
FROM authors
WHERE exists
   (SELECT *
   FROM publishers
   WHERE authors.city = publishers.city)
GO

-- this can also be done using = ANY

USE pubs
SELECT au_lname, au_fname
FROM authors
WHERE city = ANY --similar to using IN .. see next query!
   (SELECT city
   FROM publishers)
GO

USE pubs
SELECT au_lname, au_fname
FROM authors
WHERE city IN
   (SELECT city
   FROM publishers)
GO


/* This example shows queries to find titles of books published by any publisher 
located in a city that begins with the letter B. */

USE pubs

SELECT title
FROM titles
WHERE EXISTS
   (SELECT *
   FROM publishers
   WHERE pub_id = titles.pub_id
   AND city LIKE 'B%')
GO

-- Or, using IN:

USE pubs
SELECT title
FROM titles
WHERE pub_id IN
   (SELECT pub_id
   FROM publishers
   WHERE city LIKE 'B%')
GO


---------------------------------------------------------------
/*NOT EXISTS works the opposite as EXISTS. The WHERE clause in NOT EXISTS is satisfied 
if no rows are returned by the subquery. This example finds the names of publishers 
who do not publish business books. */

USE pubs
SELECT pub_name
FROM publishers
WHERE NOT EXISTS
   (SELECT *
   FROM titles
   WHERE pub_id = publishers.pub_id
   AND type = 'business')
ORDER BY pub_name
GO

