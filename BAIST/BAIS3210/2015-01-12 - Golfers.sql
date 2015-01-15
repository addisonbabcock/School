


use BAIS3210_Random_DB

if exists
(
	select [name]
	from sysobjects
	where [name] = 'TournamentPerformance'
)
drop table TournamentPerformance

if exists
(
	select [name]
	from sysobjects
	where [name] = 'Tournament'
)
drop table Tournament

if exists
(
	select [name]
	from sysobjects
	where [name] = 'GolfCourses'
)
drop table GolfCourses

if exists
(
	select [name]
	from sysobjects
	where [name] = 'Golfers'
)
drop table Golfers



create table Golfers
(
	GolferCode			int				not null
		constraint PK_Golfers_GolferCode primary key clustered,
	FirstName			nvarchar(30),
	LastName			nvarchar(30),
	[Address]			nvarchar(50),
	City				nvarchar(25),
	Province			nchar(2),
	PostalCode			nchar(7)
		constraint CK_Golfers_PostalCode check (PostalCode like '[A-Z][0-9][A-Z] [0-9][A-Z][0-9]'),
	Phone				nvarchar(20),
	Email				nvarchar(100),
	Handicap			tinyint
)

create table GolfCourses
(
	GolfCourseCode		int identity(10,10) not null,
	Name				nvarchar(100),
	DateConstructed		datetime,
	DifficultyRating	nvarchar(15),
	Par					tinyint,

	constraint PK_GolfCourses_GolfCourseCode primary key clustered (GolfCourseCode),
	constraint CK_GolfCourses_Par check (Par between 70 and 74)
)

create table Tournaments
(
	TournamentCode		int not null,
	Name				nvarchar(100),
	Sponsor				nvarchar(50),
	StartDate			datetime,
	EndDate				datetime,
	GolfCourseCode		int,
	[Type]				nvarchar(10)
)

alter table Tournaments
add constraint PK_Tournaments_TournamentCode primary key clustered (TournamentCode),
	constraint FK_Tournaments_GolfCourseCode foreign key (GolfCourseCode) references GolfCourses(GolfCourseCode),
	constraint CK_Tournaments_EndDate check (EndDate > StartDate)

create table TournamentPerformance
(
	GolferCode			int			not null,
	TournamentCode		int			not null,
	Day1Score			int,
	Day2Score			int,
	Day3Score			int,
	Day4Score			int,
	FinalPlacing		int,

	constraint PK_TournamentPerformance_GolferCode_TournamentCode
		primary key clustered (GolferCode, TournamentCode),
	constraint FK_TournamentPerformance_GolferCode
		foreign key (GolferCode) references Golfers(GolferCode),
	constraint FK_TournamentPerformance_TournamentCode
		foreign key (TournamentCode) references Tournaments(TournamentCode)
)






