
use ababcock1_BAIS3150_JobsDemo

alter proc GetAllJobs
as

	declare @return_status int = 0

	select JobCode, JobClass, HourlyRate
	from Jobs

	if @@ERROR <> 0
	begin
		set @return_status = 1
		raiserror('GetAllJobs - Select error in Jobs table', 16, 1)
	end

	return @return_status



alter proc GetEmployeesWithJob
	@JobCode as int
as
	declare @return_status int = 0

	if @JobCode is null
	begin
		raiserror ('GetEmployeesWithJob - parameter null - JobCode', 16, 1)
		set @return_status = 1
	end	else
	begin

		select Employees.EmployeeNumber, Employees.EmployeeName, Jobs.JobClass, Jobs.HourlyRate
		from Jobs
		inner join Employees on Employees.JobCode = Jobs.JobCode
		where Employees.JobCode = @JobCode

		if @@ERROR <> 0
		begin
			set @return_status = 1
			raiserror('GetEmployeesWithJob - select error', 16, 1)
	end

	return @return_status

exec GetEmployeesWithJob 3
exec GetEmployeesWithJob null


alter proc AddJob
	@JobCode as int,
	@JobClass as varchar(50),
	@HourlyRate as money
as
	declare @return_status int = 0

	if @JobCode is null
	or @JobClass is null
	or @HourlyRate is null
	begin
		raiserror ('AddJob - parameters cannot be null', 16, 1)
		set @return_status = 1
	end else
	begin
		insert into Jobs (JobCode, JobClass, HourlyRate) values (@JobCode, @JobClass, @HourlyRate)

		if @@ERROR <> 0
		begin
			set @return_status = 1
			raiserror('AddJob - insert failed', 16, 1)
		end
	end

	return @return_status


exec AddJob 4, 'Classifier', 543