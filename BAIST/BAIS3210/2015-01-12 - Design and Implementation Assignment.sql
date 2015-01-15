

---------------
--drop tables--
---------------
if exists
(
	select [name]
	from sysobjects
	where [name] = 'Benefits'
)
drop table Benefits

if exists
(
	select [name]
	from sysobjects
	where [name] = 'Deductions'
)
drop table Deductions

if exists
(
	select [name]
	from sysobjects
	where [name] = 'Earnings'
)
drop table Earnings

if exists
(
	select [name]
	from sysobjects
	where [name] = 'PayrollStatement'
)
drop table PayrollStatement

if exists
(
	select [name]
	from sysobjects
	where [name] = 'Employee'
)
drop table Employee


-----------------
--create tables--
-----------------
create table Employee
(
	EmployeeNumber			int identity(10,10)		not null,
	EmployeeName			nvarchar(50)	not null,
	Department				nvarchar(50)	not null,
	Address1				nvarchar(50)	not null,
	Address2				nvarchar(50)	not null,
	Address3				nvarchar(50)	not null,
	City					nvarchar(50)	not null,
	Province				nvarchar(2)		not null,
	PostalCode				nvarchar(7)		not null,

	constraint PK_Employee_EmployeeNumber primary key clustered 
		(EmployeeNumber),
	constraint CK_Employee_PostalCode check
		(PostalCode like '[A-Z][0-9][A-Z] [0-9][A-Z][0-9]')
)

create table PayrollStatement
(
	StartDate				datetime		not null,
	EndDate					datetime		not null,
	EmployeeNumber			int				not null,
	NetPay					money			not null,

	constraint PK_PayrollStatement_EndDate_EmployeeNumber primary key clustered
		(EndDate, EmployeeNumber),
	constraint FK_PayrollStatement_EmployeeNumber foreign key (EmployeeNumber)
		references Employee (EmployeeNumber),
	constraint CK_PayrollStatement_StartDate_EndDate check
		(StartDate <= EndDate)
)

create table Earnings
(
	EndDate					datetime		not null,
	EmployeeNumber			int				not null,
	[Type]					nvarchar(20)	not null,
	CurrentAmount			money			not null,
	YTDAmount				money			not null,

	constraint PK_Earnings_EndDate_EmployeeNumber_Type primary key clustered
		(EndDate, EmployeeNumber, [Type]),
	constraint FK_Earnings_EndDate_EmployeeNumber foreign key (EndDate, EmployeeNumber)
		references PayrollStatement (EndDate, EmployeeNumber),
	constraint CK_Earnings_Type check
		([Type] in ('Regular', 'Overtime', 'Overtime2', 'Total'))
)

create table Deductions
(
	EndDate					datetime		not null,
	EmployeeNumber			int				not null,
	[Type]					nvarchar(20)	not null,
	CurrentAmount			money			not null,
	YTDAmount				money			not null,

	constraint PK_Deductions_EndDate_EmployeeNumber_Type primary key clustered
		(EndDate, EmployeeNumber, [Type]),
	constraint FK_Deductions_EndDate_EmployeeNumber foreign key (EndDate, EmployeeNumber)
		references PayrollStatement (EndDate, EmployeeNumber),
	constraint CK_Deductions_Type check
		([Type] in ('Income Tax', 'CPP', 'EI', 'Pension Plan', 'Alberta Health Care', 'Total'))
)

create table Benefits
(
	EndDate					datetime		not null,
	EmployeeNumber			int				not null,
	[Type]					nvarchar(20)	not null,
	CurrentAmount			money			not null,
	YTDAmount				money			not null,

	constraint PK_Benefits_EndDate_EmployeeNumber_Type primary key clustered
		(EndDate, EmployeeNumber, [Type]),
	constraint FK_Benefits_EndDate_EmployeeNumber foreign key (EndDate, EmployeeNumber)
		references PayrollStatement (EndDate, EmployeeNumber),
	constraint CK_Benefits_Type check
		([Type] in ('Pension Plan', 'Blue Cross', 'Dental', 'Life Insurance', 'Disability'))
)





------------------
--drop procedures--
------------------
if (object_id ('AddPayrollStatement', 'P') is not null)
	drop proc AddPayrollStatement
if (object_id ('GetPayrollStatement', 'P') is not null)
	drop proc GetPayrollStatement
go

---------------------
--create procedures--
---------------------
create proc AddPayrollStatement
	@StartDate						datetime, 
	@EndDate						datetime, 
	@EmployeeName					nvarchar(50), 
	@Department						nvarchar(50), 
	@EmployeeNumber					int,
	@Address						nvarchar(50), 
	@City							nvarchar(50), 
	@Province						nvarchar(50), 
	@PostalCode						nvarchar(50), 
	@CurrentRegularEarnings			money, 
	@YTDRegularEarnings				money, 
	@CurrentOvertimeEarnings		money, 
	@YTDOvertimeEarnings			money, 
	@CurrentOvertime2Earnings		money, 
	@YTDOvertime2Earnings			money, 
	@CurrentTotalEarnings			money, 
	@YTDTotalEarnings				money,
	@CurrentIncomeTax				money, 
	@YTDIncomeTax					money, 
	@CurrentCPP						money, 
	@YTDCPP							money, 
	@CurrentEI						money, 
	@YTDEI							money, 
	@CurrentPensionPlanDeduction	money, 
	@YTDPensionPlanDeduction		money, 
	@CurrentAlbertaHealthCare		money, 
	@YTDAlbertaHealthCare			money, 
	@CurrentTotalDeductions			money, 
	@YTDTotalDeductions				money, 
	@NetPay							money,
	@CurrentPensionPlanBenefit		money, 
	@YTDPensionPlanBenefit			money, 
	@CurrentBlueCross				money, 
	@YTDBlueCross					money, 
	@CurrentDental					money, 
	@YTDDental						money, 
	@CurrentLifeInsurance			money, 
	@YTDLifeInsurance				money, 
	@CurrentDisability				money, 
	@YTDDisability					money
as
begin try
	begin transaction

	if @EmployeeNumber is null 
		or (select @EmployeeNumber from Employee where Employee.EmployeeNumber = @EmployeeNumber) is null
	begin
		insert into Employee (EmployeeName, Department, Address1, Address2, Address3, City, Province, PostalCode)
		values (@EmployeeName, @Department, @Address, '', '', @City, @Province, @PostalCode)

		set @EmployeeNumber = @@IDENTITY
	end	else begin
		update Employee set
			EmployeeName = @EmployeeName,
			Department = @Department,
			Address1 = @Address,
			City = @City,
			Province = @Province,
			PostalCode = @PostalCode
		where Employee.EmployeeNumber = @EmployeeNumber
	end

	insert into PayrollStatement (StartDate, EndDate, EmployeeNumber, NetPay)
	values (@StartDate, @EndDate, @EmployeeNumber, @NetPay)

	insert into Earnings (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Regular', @CurrentRegularEarnings, @YTDRegularEarnings)
	insert into Earnings (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Overtime', @CurrentOvertimeEarnings, @YTDOvertimeEarnings)
	insert into Earnings (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Overtime2', @CurrentOvertime2Earnings, @YTDOvertime2Earnings)
	insert into Earnings (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Total', @CurrentTotalEarnings, @YTDTotalEarnings)

	insert into Deductions (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Income Tax', @CurrentIncomeTax, @YTDIncomeTax)
	insert into Deductions (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'CPP', @CurrentCPP, @YTDCPP)
	insert into Deductions (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'EI', @CurrentEI, @YTDEI)
	insert into Deductions (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Pension Plan', @CurrentPensionPlanDeduction, @YTDPensionPlanDeduction)
	insert into Deductions (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Alberta Health Care', @CurrentAlbertaHealthCare, @YTDAlbertaHealthCare)
	insert into Deductions (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Total', @CurrentTotalDeductions, @YTDTotalDeductions)

	insert into Benefits (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Pension Plan', @CurrentPensionPlanBenefit, @YTDPensionPlanBenefit)
	insert into Benefits (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Blue Cross', @CurrentBlueCross, @YTDBlueCross)
	insert into Benefits (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Dental', @CurrentDental, @YTDDental)
	insert into Benefits (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Life Insurance', @CurrentLifeInsurance, @YTDLifeInsurance)
	insert into Benefits (EndDate, EmployeeNumber, Type, CurrentAmount, YTDAmount)
	values (@EndDate, @EmployeeNumber, 'Disability', @CurrentDisability, @YTDDisability)

	commit transaction
	return 0
end try
begin catch
	rollback transaction
	return 1
end catch
go


create proc GetPayrollStatement
	@EndDate						datetime,
	@EmployeeNumber					int
as
	select
		PayrollStatement.StartDate			as 'Start Date',
		PayrollStatement.EndDate			as 'End Date',
		Employee.EmployeeName				as 'Employee Name',
		Employee.Department					as 'Start Date',
		Employee.EmployeeNumber				as 'Employee Number',
		Employee.Address1					as 'Address',
		Employee.City						as 'City',
		Employee.Province					as 'Province',
		Employee.PostalCode					as 'Postal Code',

		RegularEarnings.CurrentAmount		as 'Earnings Regular Current',
		RegularEarnings.YTDAmount			as 'Earnings Regular YTD',
		OvertimeEarnings.CurrentAmount		as 'Earnings Overtime * 1.5 Current',
		OvertimeEarnings.YTDAmount			as 'Earnings Overtime * 1.5 YTD',
		Overtime2Earnings.CurrentAmount		as 'Earnings Overtime * 2 Current',
		Overtime2Earnings.YTDAmount			as 'Earnings Overtime * 2 YTD',

		IncomeTaxDeductions.CurrentAmount	as 'Deductions Income Tax Current',
		IncomeTaxDeductions.YTDAmount		as 'Deductions Income Tax YTD',
		CPPDeductions.CurrentAmount			as 'Deductions CPP Current',
		CPPDeductions.YTDAmount				as 'Deductions CPP YTD',
		EIDeductions.CurrentAmount			as 'Deductions EI Current',
		EIDeductions.YTDAmount				as 'Deductions EI YTD',
		PensionPlanDeductions.CurrentAmount	as 'Deductions Pension Plan Current',
		PensionPlanDeductions.YTDAmount		as 'Deductions Pension Plan YTD',
		AlbertaHealthCareDeductions.CurrentAmount	as 'Deductions Alberta Health Care Current',
		AlbertaHealthCareDeductions.YTDAmount		as 'Deductions Alberta Health Care YTD',
		TotalDeductions.CurrentAmount		as 'Deductions Total Current',
		TotalDeductions.YTDAmount			as 'Deductions Total YTD',

		PensionPlanBenefits.CurrentAmount	as 'Benefits Pension Plan Current',
		PensionPlanBenefits.YTDAmount		as 'Benefits Pension Plan YTD',
		BlueCrossBenefits.CurrentAmount		as 'Benefits Blue Cross Current',
		BlueCrossBenefits.YTDAmount			as 'Benefits Blue Cross YTD',
		DentalBenefits.CurrentAmount		as 'Benefits Dental Current',
		DentalBenefits.YTDAmount			as 'Benefits Dental YTD',
		LifeInsuranceBenefits.CurrentAmount	as 'Benefits Life Insurance Current',
		LifeInsuranceBenefits.YTDAmount		as 'Benefits Life Insurance YTD',
		DisabilityBenefits.CurrentAmount	as 'Benefits Disability Current',
		DisabilityBenefits.YTDAmount		as 'Benefits Disability YTD'

	from PayrollStatement
	inner join Employee on Employee.EmployeeNumber = PayrollStatement.EmployeeNumber
	inner join Earnings as RegularEarnings 
		on RegularEarnings.EndDate = PayrollStatement.EndDate 
		and RegularEarnings.EmployeeNumber = PayrollStatement.EmployeeNumber
		and RegularEarnings.Type = 'Regular'
	inner join Earnings as OvertimeEarnings 
		on OvertimeEarnings.EndDate = PayrollStatement.EndDate 
		and OvertimeEarnings.EmployeeNumber = PayrollStatement.EmployeeNumber
		and OvertimeEarnings.Type = 'Overtime'
	inner join Earnings as Overtime2Earnings 
		on Overtime2Earnings.EndDate = PayrollStatement.EndDate 
		and Overtime2Earnings.EmployeeNumber = PayrollStatement.EmployeeNumber
		and Overtime2Earnings.Type = 'Overtime2'
	inner join Earnings as TotalEarnings 
		on TotalEarnings.EndDate = PayrollStatement.EndDate 
		and TotalEarnings.EmployeeNumber = PayrollStatement.EmployeeNumber
		and TotalEarnings.Type = 'Total'
	inner join Deductions as IncomeTaxDeductions 
		on IncomeTaxDeductions.EndDate = PayrollStatement.EndDate 
		and IncomeTaxDeductions.EmployeeNumber = PayrollStatement.EmployeeNumber
		and IncomeTaxDeductions.Type = 'Income Tax'
	inner join Deductions as CPPDeductions 
		on CPPDeductions.EndDate = PayrollStatement.EndDate 
		and CPPDeductions.EmployeeNumber = PayrollStatement.EmployeeNumber
		and CPPDeductions.Type = 'CPP'
	inner join Deductions as EIDeductions 
		on EIDeductions.EndDate = PayrollStatement.EndDate 
		and EIDeductions.EmployeeNumber = PayrollStatement.EmployeeNumber
		and EIDeductions.Type = 'EI'
	inner join Deductions as PensionPlanDeductions 
		on PensionPlanDeductions.EndDate = PayrollStatement.EndDate 
		and PensionPlanDeductions.EmployeeNumber = PayrollStatement.EmployeeNumber
		and PensionPlanDeductions.Type = 'Pension Plan'
	inner join Deductions as AlbertaHealthCareDeductions 
		on AlbertaHealthCareDeductions.EndDate = PayrollStatement.EndDate 
		and AlbertaHealthCareDeductions.EmployeeNumber = PayrollStatement.EmployeeNumber
		and AlbertaHealthCareDeductions.Type = 'Alberta Health Care'
	inner join Deductions as TotalDeductions 
		on TotalDeductions.EndDate = PayrollStatement.EndDate 
		and TotalDeductions.EmployeeNumber = PayrollStatement.EmployeeNumber
		and TotalDeductions.Type = 'Total'
	inner join Benefits as PensionPlanBenefits 
		on PensionPlanBenefits.EndDate = PayrollStatement.EndDate 
		and PensionPlanBenefits.EmployeeNumber = PayrollStatement.EmployeeNumber
		and PensionPlanBenefits.Type = 'Pension Plan'
	inner join Benefits as BlueCrossBenefits 
		on BlueCrossBenefits.EndDate = PayrollStatement.EndDate 
		and BlueCrossBenefits.EmployeeNumber = PayrollStatement.EmployeeNumber
		and BlueCrossBenefits.Type = 'Blue Cross'
	inner join Benefits as DentalBenefits 
		on DentalBenefits.EndDate = PayrollStatement.EndDate 
		and DentalBenefits.EmployeeNumber = PayrollStatement.EmployeeNumber
		and DentalBenefits.Type = 'Dental'
	inner join Benefits as LifeInsuranceBenefits 
		on LifeInsuranceBenefits.EndDate = PayrollStatement.EndDate 
		and LifeInsuranceBenefits.EmployeeNumber = PayrollStatement.EmployeeNumber
		and LifeInsuranceBenefits.Type = 'Life Insurance'
	inner join Benefits as DisabilityBenefits 
		on DisabilityBenefits.EndDate = PayrollStatement.EndDate 
		and DisabilityBenefits.EmployeeNumber = PayrollStatement.EmployeeNumber
		and DisabilityBenefits.Type = 'Disability'
	where PayrollStatement.EmployeeNumber = @EmployeeNumber
		and PayrollStatement.EndDate = @EndDate
go