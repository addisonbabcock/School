/* JoinTheory3.sql

There are differences between the script in the text
and the script below but the general theme is the same.

The following batches should be executed ONE AT A TIME */

use Northwind
go


/*  Note that you can use an information schema view to
gather useful info about databases and database objects...
(go to books online.. Index.. Information Schema Views.. bookmark this page as it can
be quite useful in the future).

for example, one way to find out what the PK is in any table: */

select * from INFORMATION_SCHEMA.KEY_COLUMN_USAGE
go

-- note constraint PK_Employees specifies EmployeeId col is PK for Employees table

--next, have a quick look at data in Employees:
SELECT EmployeeId, LastName from Employees
go

--for practice, we will be creating some new tables in your default database
--first, drop them for convenience as a developer:

--------------------------------------------------------------------
use ? --***CHANGE THIS TO specify YOUR assigned database
go

--drop tables prior to attempting creation:

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Reviews')
Drop TABLE Reviews
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'ProductsCopy')
Drop TABLE ProductsCopy
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Reviewers')
Drop TABLE Reviewers
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'EmployeesCopy')
Drop TABLE EmployeesCopy
GO

--create a populated table which contains a copy of data in Northwind..Employees:
SELECT *
INTO EmployeesCopy
FROM Northwind..Employees
go

--take a look to verify:
SELECT * FROM EmployeesCopy
go

--NOTE regarding SELECT INTO statement... keys are NOT copied!  can see this
--as there is NO result for the EmployeesCopy table produced by executing:
select table_name 'Table Name',
	constraint_name 'Constraint name',
	column_name 'Column Name'
from INFORMATION_SCHEMA.KEY_COLUMN_USAGE
order by Table_Name
go

--lets designate the EmployeeID column our new table to be PK:
ALTER Table EmployeesCopy
	ADD CONSTRAINT PK_EmpId Primary Key Clustered (EmployeeID)
go

--to verify PK has now been defined, see entry for EmployeesCopy table:
select table_name 'Table Name',
	constraint_name 'Constraint name',
	column_name 'Column Name'
from INFORMATION_SCHEMA.KEY_COLUMN_USAGE
order by Table_Name
go

--now create a new table with 1:1 relationship to EmployeesCopy:
CREATE TABLE Reviewers (
	EmployeeId int not null --this will identify a reviewer
		Constraint PK_EmployeeId1 Primary Key
		Constraint FK_ReviewersToEmployees Foreign Key References EmployeesCopy(EmployeeId),
--end of EmployeeId col definition .. can see this was marked with the comma.. now next one:

	ReviewerAlias varchar(20) not null, --required info cause defined as NOT NULL
	ReviewDate smalldatetime not null
) --end of table definition
GO

/* small note on the significance of the GO statement: 
(from "GO" in books online)...
The current batch of statements is composed of all statements entered since the last GO.
The statements in the batch are compiled into a single "execution plan" by the DBMS.*/

/*Now we will insert some data for practice; this is done using INSERT - in books online, 
index, INSERT (described) .. BOOKMARK if not already done!  */

INSERT INTO Reviewers
(EmployeeId,ReviewerAlias,ReviewDate) --column list for cols we are inserting values into
VALUES
(1,'Wolinski',GetDate())
go

--note if you try to do this again, you will get an error due to PK constraint
--which only allows a given value in the EmployeeId col to occur once.. go ahead,
--run the following batch to see the error message for yourself:
INSERT INTO Reviewers
(EmployeeId,ReviewerAlias,ReviewDate) --column list for cols we are inserting values into
VALUES
(1,'Wolinski',GetDate())
go

--next to see the FK constraint in action, let's try to insert an EmployeeId
--value that does NOT exist in the parent table (this should also produce an error msg):
INSERT INTO Reviewers
(EmployeeId,ReviewerAlias,ReviewDate) --column list for cols we are inserting values into
VALUES
(888,'Wolinski',GetDate())
go

--Next, let's populate the Reviewers table with more test data for practice:
INSERT INTO Reviewers
(EmployeeId,ReviewerAlias,ReviewDate) --column list for cols we are inserting values into
VALUES
(2,'Vanselow',GetDate())
go
INSERT INTO Reviewers
(EmployeeId,ReviewerAlias,ReviewDate) --column list for cols we are inserting values into
VALUES
(3,'Silver',GetDate())
go
INSERT INTO Reviewers
(EmployeeId,ReviewerAlias,ReviewDate) --column list for cols we are inserting values into
VALUES
(4,'Walker',GetDate())
go

--look at the data so far:
SELECT * from Reviewers
go

--now create a copy of the data found in Northwind..Products:
SELECT * INTO ProductsCopy
FROM Northwind..Products
go

--take a look at our new table:
SELECT * FROM ProductsCopy
go

--now define a PK in new table:
ALTER Table ProductsCopy
	ADD CONSTRAINT PK_ProductId Primary Key Clustered (ProductId)
go

--next create a table of Reviews that can be used to store details concerning
--a particular product review that was done by one of the reviewers:
CREATE TABLE Reviews (
	ReviewId	int	not null  --a unique value for identifying product a review
		CONSTRAINT PK_ReviewId Primary Key clustered,

	EmployeeId int not null  --of the employee who did the product review
		CONSTRAINT FK_EmployeeId2 Foreign Key References Reviewers(EmployeeId),

	ProductId int not null  --identifies the product that was reviewed
		CONSTRAINT FK_ProductId Foreign Key References ProductsCopy(ProductId),

	Date smalldatetime not null, --for date when review was done
	ReviewComments varchar(200) --this should be enough room for a review!
)
go

--take a look at the products that could be reviewed by a reviewer again:
select * from ProductsCopy
go

--now insert some test data into Reviews:
INSERT INTO Reviews
(ReviewId,EmployeeId,ProductId,Date,ReviewComments)
VALUES
(1,1,1,GetDate(),'Great Product!  Tastes like chicken but much cheaper.')
GO

INSERT INTO Reviews
(ReviewId,EmployeeId,ProductId,Date,ReviewComments)
VALUES
(2,1,2,GetDate(),'I would not eat this garbage.')
GO

INSERT INTO Reviews
(ReviewId,EmployeeId,ProductId,Date,ReviewComments)
VALUES
(3,2,3,GetDate(),'Disgusting taste and so thick I bent my titanium fork getting it out of the can.')
GO

-----------------------------------------------------------------------------------
/* now we are in a position to JOIN some data gathered from the two populated tables
we have created */

--take a peek at the tables:
select * from Reviewers
select * from Reviews
go

--to see the reviews done by EmployeeId = 1:
Select * From Reviewers R1
	INNER JOIN Reviews R2
	ON R1.EmployeeId=R2.EmployeeId  --build a result set from both tables
WHERE R1.EmployeeId=1 --apply filter to only see records we are interested in
go

--NOTE That in the above query, the total width of the result set is based on all columns
--pulled from BOTH tables.  We can limit the cols in the result set easily, ie:
Select R1.EmployeeId,R1.ReviewerAlias,R2.ProductId 'ProductId of Product Reviewed',
		R2.Date 'Date of Review'
From Reviewers R1
	INNER JOIN Reviews R2
	ON R1.EmployeeId=R2.EmployeeId  --build a result set from both tables
WHERE R1.EmployeeId=1 --apply filter to only see records we are interested in
go

-------------------------------------------------------------------------------
/* CONTINUE with your own queries based on the material in pp. 229 - 239 of the text
now. */