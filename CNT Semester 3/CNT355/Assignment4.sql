use cnt3k_a4

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'ParentGuardian')
   drop table ParentGuardian
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'EmergencyContact')
   drop table EmergencyContact
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Address')
   drop table Address
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Students')
   drop table Students
GO
--+2

create table Students 
(
	StudentId		int				not null,
	Password		varchar (20)	not null,
	DateStamp		smalldatetime	not null,
	IPAddress		varchar	(15)		null,
	Surname			varchar (50)	not null,
	MiddleName		varchar (50)		null,
	FirstName		varchar (50)	not null,
	Gender			char (1)		not null,
	
	Constraint PK_StudentId primary key clustered 
		(StudentId)
)
--+2

create table ParentGuardian
(
	StudentId		int				not null,
	GuardianType	varchar (50)	not null,
	LName			varchar (50)	not null,
	FName			varchar (50)	not null,
	HomePh			varchar (20)	not null,
	WorkPh			varchar (20)		null,
	MobilePh		varchar (20)		null,
	
	constraint PK_StudentId_GuardianType primary key clustered
		(StudentId, GuardianType),
	constraint FK_ParentGuardianToStudents foreign key
		(StudentId) references Students (StudentId)
)
--+2

create table EmergencyContact
(
	StudentId		int				not null,
	ContactPriority	tinyint			not null,
	LastName		varchar (50)	not null,
	FirstName		varchar (50)	not null,
	HomePh			varchar (20)	not null,
	WorkPh			varchar (20)		null,
	MobilePh		varchar (20)		null,
	
	constraint PK_StudentId_ContactPriority primary key clustered
		(StudentId, ContactPriority),
	constraint FK_EmergencyContactToStudent foreign key
		(StudentId) references Students (StudentId)
)
--+2

create table Address
(
	StudentId		int				not null,
	AdrType			varchar (20)	not null,
	ApartmentOrBox	varchar (10)		null,
	StreetAddress	varchar (50)	not null,
	City			varchar (20)	not null,
	ProvinceOrState	varchar (20)	not null,
	Country			varchar (20)	not null,
	PostalCode		varchar (20)	not null,
	
	constraint PK_StudentId_AdrType primary key clustered
		(StudentId, AdrType),
	constraint FK_AddressToStudent foreign key
		(StudentId) references Students (StudentId)
)
--+2

--10/10