/* JoinTheory1.sql */


/*   Execute the following batches for practice noting carefully the data in the original
tables and the data in the result set returned by the query */

--set Query Analyzer for Results in TEXT

use Pubs
go

--first, look at Sales table:
SELECT	Stor_Id 				'Store Id', 
	Title_Id 				'Title Id', 
	Ord_Num 				'Order Number', 
	Convert(char(9), Ord_Date, 6) 	'Order date'  --See Books Online - CONVERT
FROM Sales
go

/* now... to get data from more than 1 table, a JOIN can be used, for example,
let's say we wanted to also know the Store Name and Address associated with
with an order: */
SELECT	S.Stor_Id 				'Store Id',
	St.Stor_Name 'Store Name', --NEW
	St.Stor_Address 'Store Address', --NEW
	S.Title_Id 				'Title Id', 
	S.Ord_Num 				'Order Number', 
	Convert(char(9), S.Ord_Date, 6) 	'Order date'  --See Books Online - CONVERT
FROM Sales S --here is where alias of S is assigned to Sales
INNER JOIN Stores St ON
	(S.Stor_Id=St.Stor_Id) --this specifies the condition of the join ie, the 
--same Stor_Id value is used to put data from a row in one table together with
--data from a row in another table
go

--confused?  look very closely at the data in the original tables and then run the Join above
--till you can see what is happening

/* Next, let's try this..

Write a select statement to retrieve the store id, store name, title id, order number, 
and order date for all books.  As we already know, the store related data is in the 
STORES table and the sales related data is in the SALES table.  

Display the order date in "dd Mon yy" format (can do this with CONVERT function).

now one step at a time..

when using the JOIN operation in a select you must tell the server 
what table to retrieve each attribute from whenever an attribute name
is coded.

 			Syntax:	tablename.attributename

			i.e.		SELECT	Sales.Stor_Id
						
		
-	there are two ways to code the JOIN operation itself:
i)	the ANSI method (coded as part of the FROM clause)

syntax:	FROM tablename1 INNER JOIN tablename2 
			ON join condition	
 
ii) the Transact SQL style:

syntax:	Select... etc.
			FROM	tablename1, tablename2
			WHERE tablename1.joinkeyattributename =tablename2.joinkeyattributename 	

..so, using the ANSI style JOIN we can do something like this: */

SELECT	Sales.Stor_Id 				'Store Id',
		Stores.Stor_Name 				'Store Name',
		Sales.Title_Id				'Title Id',
		Sales.Ord_Num				'Order Number',
		Convert(char(9), Sales.Ord_Date, 6)	'Order Date'
FROM	Sales Inner Join Stores 
	ON Sales.Stor_Id = Stores.Stor_Id
go

/*..next, using the Transact SQL style JOIN we can do this:*/

SELECT	Sales.Stor_Id 				'Store Id',
		Stores.Stor_Name 				'Store Name',
		Sales.Title_Id				'Title Id',
		Sales.Ord_Num				'Order Number',
		Convert(char(9), Sales.Ord_Date, 6)	'Order Date'
		FROM	Sales, Stores 
		WHERE	Sales.Stor_Id  =  Stores.Stor_Id
go

--you should see identical results produced from the 2 queries above; if a certain
--style is NOT specified in your labs, assns, etc., then you are free to use whichever
--you prefer although ANSI is generally a better choice to make your code more portable
--between database mgmt systems
 
/* we are not restricted to gathering data from only 2 tables!  We can nest 
joins multiple times, and you can think of this as having the effect of creating a 
'wider' result set (which you can think of as a table)
 
To retrieve data from 3 tables in a single SELECT statement you must:

i)	for the ANSI style join use 2 inner joins, each with there own join condition,
on the FROM clause.. for example:

i.e.	SELECT.... col list here (using qualified names, ie Table.Column)
		FROM	Students Inner Join Marks 
		ON Students.Student_Id = Marks.Student_Id   
						 Inner Join Courses 
						 on Courses.Course_Id = Marks.Course_Id

ii)	for the Transact SQL style join, you use a compound Boolean condition using 
AND on the WHERE clause and include all three table names on the From clause like this:

	SELECT.... col list here (using qualified names, ie Table.Column) 
	FROM	Students, Courses, Marks
	WHERE	(Students.Student_Id = Marks.Student_Id)   and
					(Courses.Course_Id = Marks.Course_Id)


-----------EXERCISES TO DO IN CLASS------------------------------------------

NOW HERE ARE SOME EXERCISES FOR YOU TO DO NEXT (use Pubs database). 


1.	Write a select statement that will retrieve the employee id, the employee 
name (in the format ‘Wilson, John B.’), the hire date and the job description
for any employees hired in 1994.  Sort the data in ascending hire date order. 

2.	Write a select statement that will retrieve the employee id, the id of publisher
the employee works for, as well as the city and state that the publisher resides in.  
Sort the data by city within state.

3.	Write a select statement to retrieve the author's name (in the format
'John Wilson') the title id, the advance, the year to date sales, and the
royaltyper for any authors residing in California (abbreviated as 'CA').



	



