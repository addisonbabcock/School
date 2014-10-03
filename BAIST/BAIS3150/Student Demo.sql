


use ababcock1_BAIS3150_StudentsDemo



IF OBJECT_ID('dbo.Programs', 'U') IS NOT NULL
  DROP TABLE dbo.Programs

create table Programs
(
	ProgramCode		varchar (10)	PRIMARY KEY NOT NULL,
	Description		varchar (60)	NOT NULL
)



IF OBJECT_ID('dbo.Students', 'U') IS NOT NULL
  DROP TABLE dbo.Students

create table Students
(
	StudentID		varchar (10)	PRIMARY KEY NOT NULL,
	FirstName		varchar (25)	NOT NULL,
	LastName		varchar (25)	NOT NULL,
	Email			varchar (50)	NOT NULL,
	ProgramCode		varchar (10)	FOREIGN KEY REFERENCES Programs (ProgramCode) NOT NULL
)



create proc AddProgram
	@ProgramCode	varchar (10),
	@Description	varchar (60)
as
	declare @return_status int = 0

	if @ProgramCode is null
	or @Description is null
	begin
		raiserror ('AddProgram - parameters cannot be null', 16, 1)
		set @return_status = 1
	end else
	begin
		insert into Programs(ProgramCode, Description) values (@ProgramCode, @Description)

		if @@ERROR <> 0
		begin
			set @return_status = 1
			raiserror('AddProgram - insert failed', 16, 1)
		end
	end

	return @return_status

exec AddProgram 'BAIST', 'Bachelor of Applied Information Systems Technology'

exec AddProgram 'CNT', 'Computer Engineering Technology'

--exec AddProgram 'Fail', NULL



create proc AddStudent
	@StudentID		varchar (10),
	@FirstName		varchar (25),
	@LastName		varchar (25),
	@Email			varchar (50),
	@ProgramCode	varchar (10)
as
	declare @return_status int = 0

	if @StudentID is null
	or @FirstName is null
	or @LastName is null
	or @Email is null
	or @ProgramCode is null
	begin
		raiserror ('AddStudent - parameters cannot be null', 16, 1)
		set @return_status = 1
	end else
	begin
		insert into Students (StudentID, FirstName, LastName, Email, ProgramCode) 
		values (@StudentID, @FirstName, @LastName, @Email, @ProgramCode)

		if @@ERROR <> 0
		begin
			set @return_status = 1
			raiserror('AddStudent - insert failed', 16, 1)
		end
	end

	return @return_status


exec AddStudent '12345', 'Addison', 'Babcock', 'asdf@asdf.com', 'BAIST'

exec AddStudent '31245', 'Super', 'Man', 'super.man@asdf.com', 'CNT'



create proc GetStudent
	@StudentID		varchar (10)
as
	declare @return_status int = 0

	if @StudentID is null
	begin
		raiserror ('GetStudent - parameters cannot be null', 16, 1)
		set @return_status = 1
	end else
	begin
		select * from Students where @StudentID = Students.StudentID

		if @@ERROR <> 0
		begin
			set @return_status = 1
			raiserror('AddStudent - select failed', 16, 1)
		end
	end

	return @return_status
	
exec GetStudent '12345'

--exec GetStudent NULL

exec GetStudent '1231'



create proc UpdateStudent
	@StudentID		varchar (10),
	@FirstName		varchar (25),
	@LastName		varchar (25),
	@Email			varchar (50),
	@ProgramCode	varchar (10)
as
	declare @return_status int = 0

	if @StudentID is null
	or @FirstName is null
	or @LastName is null
	or @Email is null
	or @ProgramCode is null
	begin
		raiserror ('UpdateStudent - parameters cannot be null', 16, 1)
		set @return_status = 1
	end else
	begin
		update Students set 
			FirstName = @FirstName, 
			LastName = @LastName, 
			Email = @Email, 
			ProgramCode = @ProgramCode
		where StudentID = @StudentID

		if @@ERROR <> 0
		begin
			set @return_status = 1
			raiserror('UpdateStudent - insert failed', 16, 1)
		end
	end

	return @return_status

exec GetStudent '12345'
exec UpdateStudent '12345', 'Herp', 'Derp', 'Herp@Derp.com', 'BAIST'
exec GetStudent '12345'



create proc DeleteStudent
	@StudentID		varchar (10)
as
	declare @return_status int = 0

	if @StudentID is null
	begin
		raiserror ('DeleteStudent - parameters cannot be null', 16, 1)
		set @return_status = 1
	end else
	begin
		delete from Students
		where StudentID = @StudentID

		if @@ERROR <> 0
		begin
			set @return_status = 1
			raiserror('DeleteStudent - insert failed', 16, 1)
		end
	end

	return @return_status

exec AddStudent '11111', 'Temp', 'ToBeDeleted', 'Mistake@OhNoes.com', 'CNT'
exec GetStudent '11111'
exec DeleteStudent '11111'
exec GetStudent '11111'



create proc GetProgram
	@ProgramCode		varchar (10)
as
	declare @return_status int = 0

	if @ProgramCode is null
	begin
		raiserror ('GetProgram - parameters cannot be null', 16, 1)
		set @return_status = 1
	end else
	begin
		select * from Programs where @ProgramCode = ProgramCode

		if @@ERROR <> 0
		begin
			set @return_status = 1
			raiserror('GetProgram - select failed', 16, 1)
		end
	end

	return @return_status

exec GetProgram 'BAIST'
exec GetProgram 'CNT'
--exec GetProgram NULL



create proc GetStudents
	@ProgramCode		varchar (10)
as
	declare @return_status int = 0

	if @ProgramCode is null
	begin
		raiserror ('GetStudents - parameters cannot be null', 16, 1)
		set @return_status = 1
	end else
	begin
		select * from Students where @ProgramCode = Students.ProgramCode

		if @@ERROR <> 0
		begin
			set @return_status = 1
			raiserror('GetStudents - select failed', 16, 1)
		end
	end

	return @return_status

exec GetStudents 'BAIST'
exec GetStudents 'CNT'



