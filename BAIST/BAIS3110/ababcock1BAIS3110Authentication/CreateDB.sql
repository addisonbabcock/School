USE master
GO
-- create a database for the security information
IF EXISTS (SELECT * FROM master..sysdatabases WHERE name = 'UserAccounts')
DROP DATABASE UserAccounts
GO
CREATE DATABASE UserAccounts
GO
USE UserAccounts
GO

CREATE TABLE [Users] (
[UserEmail] [varchar] (255) NOT NULL ,
[PasswordHash] [varchar] (40) NOT NULL ,
[salt] [varchar] (10) NOT NULL,
CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
(
[UserName]
) ON [PRIMARY]
) ON [PRIMARY]
GO

-- create stored procedure to register user details
CREATE PROCEDURE RegisterUser
@userName varchar(255),
@passwordHash varchar(40),
@salt varchar(10)
AS
INSERT INTO Users VALUES(@userName, @passwordHash, @salt)
GO

-- create stored procedure to retrieve user details
CREATE PROCEDURE LookupUser
@userName varchar(255)
AS
SELECT PasswordHash, salt
FROM Users
WHERE UserName = @userName
GO

--exec sp_grantlogin [WB206-03\ASPNET]
-- Add a database login for the UserAccounts database for the ASPNET account
--exec sp_grantdbaccess [WB206-03\ASPNET]
-- Grant execute permissions to the LookupUser and RegisterUser stored procs
--grant execute on LookupUser to [WB206-03\ASPNET]
--grant execute on RegisterUser to [WB206-03\ASPNET]

