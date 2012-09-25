/* Wolinski /04 */

use test --student should use some test database where tables & procedures can be created
go

-- Create test tables:
If EXISTS (select name from sysobjects
				where name='Clients')
	DROP TABLE Clients
go

If EXISTS (select name from sysobjects
				where name='UserLogins')
	DROP TABLE UserLogins
go

CREATE TABLE UserLogins (
	ClientId int IDENTITY (1, 1) NOT NULL
		Constraint PK_ClientId Primary Key nonclustered,
	LoginId varchar(40)
		CONSTRAINT Unique_LoginId UNIQUE NOT NULL,
	Password varchar(20) not null,
	IpAdr varchar(15) null,
	DateStamp smalldatetime not null, --date of record creation
	SecurityLock BIT NOT NULL
)
go

CREATE TABLE [dbo].[Clients] (
	[ClientId] [int] NOT NULL
		Constraint PK_ClientId2 Primary Key nonclustered
		Constraint FK_ClientId Foreign Key References UserLogins(ClientId),
	[FName] [varchar] (50)  NULL ,
	[LName] [varchar] (50)  NULL ,
	[Email] [varchar] (50)  NOT NULL
)
go

--Create stored procedure to record LoginId values; these must be unique!
--it is best to check for SP's before dropping (to avoid errors); drop SP if it exists
--before attempting creation

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'spCreateId')
   drop proc spCreateId
GO

/* spCreateId:  creates a new record in "UserLogins" table; a unique LoginId is
required for each user; a password is also established using this stored proc
before any detailed client data is gathered and stored.  

Entry is time-stamped & IP address is recorded  if it passed to this SP (optional parameter).
Inputs: see cols below
Outputs:  OUTPUT Para: Assigned ClientId (integer)
			RETURN para: 0 if ok; 1 if LoginId already exists; 2 if error */

CREATE PROC spCreateId
   @ClientId int OUTPUT, --created by DBMS; identity col
   @LoginID VARCHAR(40),
   @Password varchar (20),
   @IPAddress VARCHAR(15) = NULL,
   @SecurityLock BIT = 0
AS
SET NOCOUNT ON
If NOT EXISTS (Select LoginId From UserLogins
				Where LoginId=@LoginId)
BEGIN --LoginId does NOT already exist so create record:
	DECLARE   @DateStamp SMALLDATETIME --local variable; must begin with "@"

	--note GetDate() is a built-in function that we can call to get current date & time
	SELECT @DateStamp = GetDate() --assign a value to the variable (initializing)

	INSERT INTO UserLogins (LoginID,Password,IpAdr,DateStamp,SecurityLock)
	VALUES (@LoginID,@Password,@IPAddress,@DateStamp,@SecurityLock)

	-- Check for errors using the built-in function "@@ERROR"
	IF @@ERROR <> 0 --if @@ERROR returns 0 then the INSERT went ok
	BEGIN
		RETURN(2) --error (should never get this)
	/* note:  often the actual error code is sent back to the caller using
	RETURN @@ERROR

	If we do this we norally replace this entire IF structure with that statement, ie:

	INSERT INTO UserLogins (LoginID,Password,IpAdr,DateStamp,SecurityLock)
	VALUES (@LoginID,@Password,@IPAddress,@DateStamp,@SecurityLock)
	RETURN @@ERROR

	this is much simpler and more useful since we can act on specific error codes
	in our application (can store this in sys app log, mail it, etc.)

	these codes are well documented at Microsoft

	If a specific error code on encountering an error is not specified in the lab, exam, etc.
	then use this technique as it saves typing and is more useful in practice!
	*/

	END
	ELSE
	BEGIN
		--@@identity is another built-in function; this one retrieves the last
		--identity value created by SQL Server
   	SELECT @ClientId = @@identity --get the last value stored in the ClientId col
   	RETURN(0) --success
	END
END
ELSE
BEGIN --LoginId already exists; notify caller
   	RETURN(1) --can also be written as:  RETURN 1 
END
go


/* ************************************************************************** */
/* test spCreateId: 

Try creating a new record; display the output parameter value passed back from SP */

DECLARE    @ClientId int --must delcare variables within a batch before you can use it
DECLARE	@ReturnCode int

--look carefully at the syntax below:
EXEC @ReturnCode=spCreateId
	@ClientId  OUTPUT, --created by DBMS; identity col; SP returns this value to caller
							--if a new record was created
   @LoginID = 'JackSmith',
   @Password = 'hello',
   @IPAddress = '122.34.55.111'
 
SELECT 'Assigned ClientId is: '= CAST(@ClientId as varchar(20)), --note that @ClientId remains valid throughout
--the current batch
	'Return Code'=CAST(@ReturnCode as char(1))
go

--take a peek at the table:
Select * from UserLogins
go

--next, try to create a record using the same LoginId value...note we should see a 
--Return Code of 1 coming back from SP and NULL for ClientId
DECLARE    @ClientId int --must delcare variables within a batch before you can use it
DECLARE	@ReturnCode int

EXEC @ReturnCode=spCreateId
	@ClientId  OUTPUT, --created by DBMS; identity col; SP returns this value to caller
							--if a new record was created
   @LoginID = 'JackSmith',
   @Password = 'hello',
   @IPAddress = '122.34.55.111'
 
SELECT 'Assigned ClientId is: '=@ClientId, --note that @ClientId remains valid throughout
--the current batch
	'Return Code'=@ReturnCode
go

/* ************************************************************************** */
/* Next, write a stored procedure called  spCreateClient to store new records in 
the Clients table.  Send return code of 1 if error detected, 0 if all OK: */

--drop if exists: 
IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'spCreateClient')
   drop proc spCreateClient
GO

/* ************************************************************************** */
/* spCreateClient:  creates a new record in "Clients" table

Inputs: see cols below
Outputs:  RETURN para: 1 if error; 0 if ok; */

CREATE PROC spCreateClient
   @ClientID int,
   @FName varchar (50) = NULL, --optional parameter in this example
   @LName varchar (50) = NULL, --optional
   @Email varchar (50)
AS
SET NOCOUNT ON
INSERT INTO Clients	
   (ClientId,FName,LName,Email)
VALUES 			
   (@ClientID,@FName,@LName,@Email)
-- Check for errors:
IF @@ERROR <> 0 
BEGIN
    RETURN(1) --error
END
ELSE
BEGIN
   RETURN(0) --success (no errors detected)
END
go

/* ************************************************************************** */
/* test spCreateClient: 

Try creating a new record; display the output parameter value passed back from SP */

DECLARE	@ReturnCode int

--call SP passing the input parameters:
EXEC @ReturnCode=spCreateClient
   @ClientID=1,
   @FName='Mr.',
   @LName='Pew',
   @Email='me@somewhere.com'
 
SELECT 'Return Code'=@ReturnCode
go

--take a peek at the table to verify record creation:
Select * from Clients
go

--NEXT ************* Try creating another record with same ClientId (this should
--produce an error - Primary Key cannot be duplicated - Return code should be 1

--PUT RESULTS IN TEXT to clearly see both the error msg AND the Return Code:

DECLARE	@ReturnCode int

--call SP passing the input parameters:
EXEC @ReturnCode=spCreateClient
   @ClientID=1,
   @FName='Mr.',
   @LName='Pew',
   @Email='me@somewhere.com'
 
SELECT 'Return Code'=@ReturnCode
go

--**************************************************************
--NEXT... a demonstration of optional parameters; first clear the Clients table:
DELETE Clients
go

--now call the SP but this time, do NOT supply values for first and last names:
DECLARE	@ReturnCode int

--call SP passing the input parameters:
EXEC @ReturnCode=spCreateClient
   @ClientID=1,
   @Email='me@somewhere.com'
 
SELECT 'Return Code'=@ReturnCode
go

--take a peek at the table:
SELECT * From Clients --should see NULL for the name cols
go

/* Moving on... now create a SP IN YOUR DB to retrieve ALL records from Northwind..Products.
We will make this progressively more complex until INPUT, OUTPUT, Return Code, and
Result Set concepts are demonstrated */

CREATE PROC spGetProducts
AS
SELECT * From Northwind..Products
RETURN --note that no specific return code has been specified!
go

--execute SP and take a look at the return code:
DECLARE	@ReturnCode int
EXEC @ReturnCode=spGetProducts --this produces a result set
SELECT 'Return Code'=@ReturnCode --this displays the return code
go

--can see the above example has sent a result set AND a return code to caller

--************************************************************
--NEXT... lets add an input parameter so info for a specific product can be 
--retrieved (based on ProductId).  We will have to re-create the SP:

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'spGetProducts')
   drop proc spGetProducts
GO

--now re-create:
CREATE PROC spGetProducts
	@ProductId int
AS
SELECT * From Northwind..Products
WHERE ProductId=@ProductId
RETURN --note that no specific return code has been specified! 0 will be sent back.
go

--Now test this version of SP:
--execute SP and take a look at the result set AND return code:
DECLARE	@ReturnCode int
EXEC @ReturnCode=spGetProducts 
	@ProductId=5 --supply a value of 5 for the INPUT parameter

SELECT 'Return Code'=@ReturnCode --this displays the return code
go

--************************************************************
--NEXT... lets add an OUTPUT parameter into the example!

--To keep it simple, lets simply return tne Product Name using the output
--parameter (even though it will also appear in the result set!)
--We will have to re-create the SP again so drop first:

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'spGetProducts')
   drop proc spGetProducts
GO

--now re-create:
CREATE PROC spGetProducts
	@ProductName nvarchar(40) OUTPUT, --data types should be the same as seen in the table
	@ProductId int
AS
SELECT * From Northwind..Products --SELECT produces the result set sent back
WHERE ProductId=@ProductId

--next assign a value to the output parameter for passing back:
SET @ProductName=(SELECT ProductName From Northwind..Products
						WHERE ProductId=@ProductId)

RETURN(0) --terminate execution and send return code = 0 back to caller
go

--Now test this version of SP:
--execute SP and take a look at the result set, Output Parameter, AND return code:
DECLARE	@ReturnCode int
DECLARE	@OutputParameter nvarchar(40)

--execution of the SP produces the result set seen in the results pane of Query Analyzer
EXEC @ReturnCode=spGetProducts 
	@OutputParameter OUTPUT,
	@ProductId=5 --supply a value of 5 for the INPUT parameter

--the following code shows us return code and output parameter:
SELECT 'Return Code'=@ReturnCode, --this displays the return code
	@OutputParameter 'Output Parameter (Product Name)'
go

--************************************************************
--NEXT... you can easily send messages to the caller in place of 
--a set of results:

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'spShowProducts')
   drop proc spShowProducts
GO

CREATE PROC spShowProducts
	@ProductId int=NULL
AS
	If @ProductId IS NULL
	Begin
		PRINT 'ProductId missing' --send message to caller (as a result)
		RETURN(1) --send ret code of 1 back to caller
	End
	Else
	Begin
		SELECT * From Northwind..Products --SELECT produces the result set sent back
		WHERE ProductId=@ProductId
	End
go

--to test (PUT QUERY ANALYZER RESULTS IN TEXT):
--first, without supplying a ProductId:
DECLARE	@ReturnCode int
EXEC @ReturnCode=spShowProducts --should see message returned by SP

--And... the following code shows us return code which should be 1 here:
SELECT 'Return Code'=@ReturnCode --this displays the return code
go

--NOW TRY with a supplied ProductId:
DECLARE	@ReturnCode int
--execution of the SP produces the result set seen in the results pane of Query Analyzer
EXEC @ReturnCode=spShowProducts 
	@ProductId=5 --supply a value of 5 for the INPUT parameter
--the following code shows us return code and output parameter:
SELECT 'Return Code'=@ReturnCode --this displays the return code
go

/* By now you should have a pretty good grip on passing data back and forth!

One last tip... when using various parameters, keep the order in which you specify the
parameters when calling the SP the same as the order used in the stored procedure itself
to avoid problems.  */












