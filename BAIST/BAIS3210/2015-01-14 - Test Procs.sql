DECLARE	@return_value int

EXEC	@return_value = [dbo].[AddPayrollStatement]
		@StartDate = N'2015-02-01',
		@EndDate = N'2015-02-28',
		@EmployeeName = N'Addison Babcock',
		@Department = N'IT',
		@EmployeeNumber = 10,
		@Address = N'123 Any Street',
		@City = N'Edmonton',
		@Province = N'AB',
		@PostalCode = N'A1A 1A1',
		@CurrentRegularEarnings = 1,
		@YTDRegularEarnings = 2,
		@CurrentOvertimeEarnings = 3,
		@YTDOvertimeEarnings = 4,
		@CurrentOvertime2Earnings = 5,
		@YTDOvertime2Earnings = 6,
		@CurrentTotalEarnings = 7,
		@YTDTotalEarnings = 8,
		@CurrentIncomeTax = 9,
		@YTDIncomeTax = 0,
		@CurrentCPP = 1,
		@YTDCPP = 2,
		@CurrentEI = 3,
		@YTDEI = 4,
		@CurrentPensionPlanDeduction = 5,
		@YTDPensionPlanDeduction = 6,
		@CurrentAlbertaHealthCare = 7,
		@YTDAlbertaHealthCare = 8,
		@CurrentTotalDeductions = 9,
		@YTDTotalDeductions = 0,
		@NetPay = 1,
		@CurrentPensionPlanBenefit = 2,
		@YTDPensionPlanBenefit = 3,
		@CurrentBlueCross = 4,
		@YTDBlueCross = 5,
		@CurrentDental = 6,
		@YTDDental = 7,
		@CurrentLifeInsurance = 8,
		@YTDLifeInsurance = 9,
		@CurrentDisability = 0,
		@YTDDisability = 1

SELECT	'Return Value' = @return_value

GO

EXEC	[dbo].[GetPayrollStatement]
		@EndDate = '2015-02-28', @EmployeeNumber = 10


		
select * from Employee

select * from PayrollStatement

select * from Earnings

select * from Deductions

select * from Benefits
