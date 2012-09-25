/* SQL Practice Exam 3 Questions


------------------------------------------------
ASSIGN COLUMN ALIASES FOR ALL RETURNED COLUMNS.
------------------------------------------------


*/

use Practice --create this; xx=your class number.  Should be 
--current database for SQL below!
go

/* 1. Draw source data from Northwind for this question.

Write a stored procedure called "spGetSwitzerlandInfo" to retrieve the following 
information concerning orders placed by Customers from Switzerland 
(value in Country col of Customers table = Switzerland):

Full name of employee who took the order (formatted as Doe, John); Name of the Company that
placed the order; Date order was placed; City that customer is from in Switzerland.

Assign aliases to columns and tables as usual, and use ANSI style JOINS only; NO subqueries
and NO SQL style joins! 

Your stored procedure must have 2 input parameters called "@RequiredDate1"
and "@RequiredDate2" (see RequiredDate col in Orders to determine data type).

Code the stored procedure to retrieve information concerning orders with a required date
between @RequiredDate1 and @RequiredDate2 inclusive.

Organize your results by customer city, and don't forget to drop the procedure
if it exists prior to attempting creation as usual.

Also, include a block of code to drop the procedure prior to creation if it exists, and
also an error check after the SELECT; send a return code of 1 if an error
is detected; 0 if no error.

*/

/* PLACE YOUR SQL HERE !!!!!!!!!!!!!!!!!!!!!!!!!! */

use northwind

if exists (select name from sysobjects where name = 'spGetSwitzerlandInfo')
	drop proc spGetSwitzerlandInfo

create proc spGetSwitzerlandInfo 
	@RequiredDate1	datetime = null,
	@RequiredDate2	datetime = null
as
	set nocount on
	select	employees.lastname + ', ' + employees.firstname 'Employee Name',
			customers.companyname 'Company Name',
			orders.orderdate 'Order Date',
			customers.city 'Customer Location'
	from	customers
			join	orders
			on		orders.customerid = customers.customerid
					join	employees
					on		employees.employeeid = orders.employeeid
	where	orders.requireddate between @requireddate1 and @requireddate2 and
			customers.country like 'Switzerland'
	order by 4

	if @@ERROR <> 0
		return 1
	return 0
go

/* To PRODUCE A RESULT SET FOR QUESTION 1 (should be 2 ROWS), EXECUTE THIS QUERY:
EXEC spGetSwitzerlandInfo 
	@RequiredDate1 = '6/1/1996',
	@RequiredDate2 = '8/15/1996'
*/

/* ----------------------------------------
2. Source data comes from Northwind.

Create a view called vwGetOrderInfo that will return the following information
concerning orders and employees:

Full name of employee formatted as 'John Doe';
approximate age of employee AT THE TIME THEY were hired (hint: find the difference 
in years between hire date and birthdate...don't worry about months and days here; only
concern yourself with difference in "year" parts of the dates); 
also return the number of orders taken by the employee.

Further, limit your results to ONLY employees that were hired when they were younger
than 30, and order your results by the age of employee at time of hire.

Include a block of code to drop the view if it exists prior to attempting creation.

NOTES:  ASSUME EMPLOYEE NAMES TO BE UNIQUE; also, note that a
FUNCTION EXISTS TO FIND the DIFFERENCE BETWEEN DATES!
*/

use northwind

if exists (select name from sysobjects where name = 'vwGetOrderInfo')
	drop view vwGetOrderInfo

create view vwGetOrderInfo
as
	select	top 100 percent
			e.firstname + ' ' + e.lastname 'Employee Name',
			datediff (yy, e.birthdate, e.hiredate) 'Age at time of hire',
			count (o.employeeid) 'Number of orders'
	from	employees e
			join	orders o
			on		e.employeeid = o.employeeid
	where	datediff (yy, e.birthdate, e.hiredate) < 30
	group by e.firstname, e.lastname, datediff (yy, e.birthdate, e.hiredate)
	order by 2
	
/* to produce a result set for Q2 (should be 2 ROWS), execute the following SELECT:
select * from vwGetOrderInfo
*/


/* 3. ----------------------------------------
Source data is Northwind database.  
PART 1:
Create a view called "vwSummarizeCatergoryRevenues"
to return summary information concerning product categories
and the revenue generated due to orders for products in those categories.  

Your view is to return the following columns(make sure you assign aliases!):

i) category name (these are in the Categories table); use column alias of "Category"
ii) Total revenue (under column alias of "Revenue") from all orders placed for 
the product category (assume CategoryName values to be unique; this fact is 
important for your grouping!)

Note that for any given order in the Order Details table, total revenue from that one order 
= ((UnitPrice * Quantity)*(1-discount))  where all these are taken from the 
Order Details table.

iii) third column = average for the order revenue values in the category. 

Order the results by the average value such that the largest value appears first
in your results.

Format the monetary values so there are 2 digits to the right of the decimal by
coverting to a destination data type of "decimal(10,2).

Also, include a block of code to drop the view prior to creation if it exists.
*/

/* PUT YOUR SQL FOR PART I HERE......................... */

use northwind

if exists (select name from sysobjects where name = 'vwSummarizeCatergoryRevenues')
	drop view vwSummarizeCatergoryRevenues

create view vwSummarizeCatergoryRevenues
as 
	select	top 100 percent
			c.categoryname 'Category',
			cast (sum ((od.UnitPrice * od.Quantity)*(1-od.discount)) as decimal (10,2)) 'Revenue',
			cast (avg ((od.UnitPrice * od.Quantity)*(1-od.discount)) as decimal (10,2)) 'Average'
	from	[order details] od
			join	products p
			on		p.productid = od.productid
					join	categories c
					on		c.categoryid = p.categoryid
	group by c.categoryname
	order by avg ((od.UnitPrice * od.Quantity)*(1-od.discount)) desc
go

/* PART 2:
Create a stored procedure called "spSummarizeCategoryRevenues"; include a block of code to 
drop the procedure prior to creation if it exists.

It should have 1 input parameter called @MinAverage of decimal(10,2) data type.

Procedure should return all columns from the view created in part 1 but ONLY rows
with an average value for orders within the category that is greater than the
value supplied via the input parameter.

Set the input parameter to a NULL default and if NO parameter is passed, terminate
execution and send back a return code of 2.

If any other errors are detected send back a return code of 1.

If NO errors are detected, send return code of 0 back.

*/

if exists (select name from sysobjects where name = 'spSummarizeCategoryRevenues')
	drop proc spSummarizeCategoryRevenues

create proc spSummarizeCategoryRevenues
	@MinAverage decimal (10, 2) = null
as
	if @MinAverage is null
	begin
		return 2
	end
	else
	begin
		select	* 
		from	vwSummarizeCatergoryRevenues
		where	Average > @MinAverage
		
		if @@ERROR <> 0
		begin
			return 1
		end
		else
		begin
			return 0
		end
	end
go

/* To produce a RESULT set for Q3, execute the following statement:
exec spSummarizeCategoryRevenues 500
*/


/* -----------------------------------------------------------------
4. TRANSACTION:
  
First, run the script below to create 2 new tables in YOUR database: */

Create Table Student  (
	StudentId int not null primary key clustered,
	LName varchar(50) not null,
	FName varchar(50) not null,
	HomePhone varchar(12) null
)
go

Create table Address(
	StudentId int not null
		Constraint FK_StId Foreign Key References Student(StudentId),
	AddressType varchar(10) not null,
		CONSTRAINT PK_StudentId_AddressType Primary Key 
			clustered (StudentId,AddressType), --composite primary key
	POBox varchar(10) null,
	StreetAdr varchar(50) not null,
	City  varchar(50) not null,
	ProvOrState varchar(50) not null,
	Country varchar(50) not null,
	PostalCode varchar(10) null
)
go


/* After you have created the tables, now code a transaction to:

i) Insert a new Student Record:  id=121333, Last Name=Doe, First Name=John, set home phone to NULL
ii)Insert a child Address record for this student:  type = Permanent, Box 111, 12133-567 St., Edmonton, AB., Canada, T5G2R1

Use PRINT statements to report failures or to report completion of all steps (2 here).

Note that a stored procedure is NOT required for this question so do NOT use one!  This also means there are 
NO return codes.

Part marks will apply for this question.  You do NOT need to produce a result set as your instructor will 
examine your SQL code.

*/

use practice
begin tran

insert into Student (StudentId, LName, FName)
values (121333, 'Doe', 'John')

if @@ERROR <> 0
begin
	rollback tran
	print 'Could not add student'
end
else
begin
	insert into Address (StudentId, AddressType, POBox, StreetAdr, City, ProvOrState, Country, PostalCode)
	values (121333, 'Permanent', 'Box 111', '12133-567 St.', 'Edmonton', 'AB.', 'Canada', 'T5G2R1')

	if @@ERROR <> 0
	begin
		rollback tran
		print 'Could not add student'
	end
	else
	begin
		commit tran
		print 'Success!'
	end
end