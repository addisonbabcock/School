/* CreateTable1.sql */
--illustrates fundamental concepts; no keys or indexes defined yet! */
--the data types used below should be discussed in class; see Books Online for 
--detailed information concerning data types; pay close attention to storage requirements!

use tablecreationpractice

--drop tables ONLY if they already exist; this can be done by checking the
--sysobjects table in the database!
IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Employees_Projects')
   drop table Employees_Projects
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Employees')
   drop table Employees
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Projects')
   drop table Projects
GO
 
-------------------------CREATE TABLES NOW: ------------------------------------

Create Table Employees(

	SocialInsuranceNumber	char(9) 	not null,
	LastName		varchar(100)	not null,
	FirstName		varchar(100)	not null,
	MiddleInitial		char(1)		null,
	BirthDate		smalldatetime	not null,
	Address			varchar(100)	null,
	Gender			char(1)		not null,
	Salary			smallmoney	not null,
	SupervisorSIN		char(9)		null
)

Create Table Employees_Projects(

	SocialInsuranceNumber	char(9) 	not null,
	ProjectNumber		char(2)		not null,
	WeeklyHrs		smallint	not null
)

Create Table Projects(

	ProjectNumber		char(2)		not null,
	ProjectName		varchar(100)	not null,
	ProjectLocation		varchar(100)	not null
)

/* CreateTable2.sql - adding PK definitions */

use tablecreationpractice

--drop tables ONLY if they already exist; this can be done by checking the
--sysobjects table in the database!
IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Employees_Projects')
   drop table Employees_Projects
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Employees')
   drop table Employees
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Projects')
   drop table Projects
GO
 
-------------------------CREATE TABLES NOW: ------------------------------------
--clustered indexes are created on the primary key columns; discussed later, these
--help to quickly locate information based on a PK value

Create Table Employees(

	SocialInsuranceNumber	char(9) 	not null
		Constraint PK_SocialInsuranceNumber primary key clustered,
	LastName		varchar(100)	not null,
	FirstName		varchar(100)	not null,
	MiddleInitial		char(1)		null,
	BirthDate		smalldatetime	not null,
	Address			varchar(100)	null,
	Gender			char(1)		not null,
	Salary			smallmoney	not null,
	SupervisorSIN		char(9)		null
)

--next table has a composite PK:
Create Table Employees_Projects(

	SocialInsuranceNumber	char(9) 	not null,
	ProjectNumber		char(2)		not null,
	
	Constraint PK_SocialInsuranceNumber_ProjectNumber 
		primary key clustered (SocialInsuranceNumber,ProjectNumber),

	WeeklyHrs		smallint	not null
)

Create Table Projects(

	ProjectNumber		char(2)		not null
		Constraint PK_ProjectNumber primary key clustered,
	ProjectName		varchar(100)	not null,
	ProjectLocation		varchar(100)	not null
)

/* CreateTable3.sql - illustrates concepts of:
Identity columns , Check Constraints, Foreign Keys and Calculated Columns */

use tablecreationpractice

--drop tables ONLY if they already exist; this can be done by checking the
--sysobjects table in the database!

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'PurchaseOrder')
   drop table PurchaseOrder
GO
--PurchaseOrder is a child table of Supplier (has a FK in it) so it MUST be dropped first.


IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Supplier')
   drop table Supplier
GO

----------------------Create Tables ---------------------------------

Create Table Supplier (

/* create SupplierId col, make it identity col and pri key: */
	SupplierId		int identity (1, 1)		not null
Constraint PK_Supplier Primary Key clustered,

/* create remaining cols with CHECK constraint on Phone col
ensuring that phone number format is correctly entered:  */
	Name			varchar(100)			not null,
	Phone			char(14)			not null
		Constraint CK_Phone Check 
	(Phone Like '([0-9][0-9][0-9]) [0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]')
)

----------------------------------------------------------------------
Create Table PurchaseOrder (

	OrderNumber		int identity (1,1)			not null
		Constraint PK_PurchaseOrder Primary Key clustered,
	OrderDate		smalldatetime			not null,
	DateReceived		smalldatetime			not null,

	SupplierId		int				not null
		Constraint FK_PurchaseOrderToSupplier references Supplier (SupplierId),

/* ensure SubTotal and GST are greater than 0:  */

	SubTotal		money				not null
		Constraint CK_SubTotalMustBePositive Check (Subtotal > 0),

	GST			money				not null
		Constraint CK_GSTMustBePositive Check (GST > 0),

/* define Total to be a calculated col:  */
	Total as Subtotal + GST,

	Constraint CK_DateReceivedMustBeOnOrAfterOrderDate 
		Check (DateReceived >= OrderDate)
)
