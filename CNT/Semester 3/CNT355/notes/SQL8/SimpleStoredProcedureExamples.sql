/* SimpleStoredProcedureExamples.sql */

/* 1. Internal Stored Procedure example:  Execute the following batch with YOUR database 
selected in the drop down box (Query analyzer).. this will create stored procedure 
"spJobsReport" in YOUR database */

CREATE PROCEDURE spJobsReport as /* UP stands for "user procedure" */

/* statements making up stored procedure go in this section */
	SELECT * FROM Jobs --in this case, only 1 statement to keep it simple!
Return


/* 2. now create a table to use for practice.. */
CREATE TABLE jobs
(
    job_id  	smallint IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    job_desc   	varchar(50)     NOT NULL,
    min_lvl 	tinyint NOT NULL CHECK (min_lvl >= 10),
    max_lvl 	tinyint NOT NULL CHECK (max_lvl <= 250)
)
go

/* 3. now to use the stored procedure, execute it: */
exec spJobsReport
go

/* 4. External Stored Procedure example:  the following SP will reside in your user database
(it was created there) but will access data in another database */
CREATE PROCEDURE UP_AuthorsReport AS
	SELECT	Pubs.dbo.Authors.Au_Lname as LastName,
		Pubs.dbo.Authors.Au_Fname as FirstName
	FROM	Pubs.dbo.Authors
RETURN

/* 5.  to test the SP, execute it:  */
exec UP_AuthorsReport
go

/* another External SP example */

Use db_n --in this case, the stored procedure will be created in the "db_n" database
CREATE PROC spShippers AS
	SELECT * FROM Northwind.dbo.Shippers
RETURN
go --run the batch

Exec spShippers --execute the stored procedure created above
go






