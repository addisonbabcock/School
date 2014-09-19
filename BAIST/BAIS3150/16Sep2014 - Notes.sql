


use ababcock1_BAIS3150_Implementation_Assignment


create table Job
(
	JobCode			int not null primary key identity(1,1),
	JobClass		nvarchar(100) not null,
	HourlyRate		smallmoney not null,
)

insert into Job (JobClass, HourlyRate)
values ('Electrical Engineer', 84.50)

select * from Job


create table Employee
(
	EmployeeNumber		int not null primary key identity (100, 1),
	EmployeeName		nvarchar(100) not null,
	JobCode				int references Job(JobCode)						--FK as a column constraint
)

insert into Employee (EmployeeName, JobCode)
values ('Super Man', 1)

select Employee.EmployeeName, Employee.EmployeeNumber, Job.JobClass, Job.HourlyRate
from Job
inner join Employee on Job.JobCode = Employee.JobCode

