/* Output Parameter basics... */

--example:
drop proc sp_ChangeParams
go

create proc sp_ChangeParams
	@num1 tinyint OUTPUT, --define 2 output parameters
	@num2 tinyint OUTPUT
AS
	SET @num1 = @num1 * 2 --assign values
	SET @num2 = 0
go

--now to use the SP:
DECLARE @num1 tinyint --must declare variables to use them
DECLARE @num2 tinyint
SET @num1 = 10 --assign test values

SET @num2 = 20
PRINT 'Test values before calling procedure:'
select @num1 '@num1',@num2 '@num2'

exec sp_ChangeParams @num1 OUTPUT,@num2 OUTPUT --call SP passing parameters
PRINT 'Test values after calling procedure:'
select @num1 '@num1',@num2 '@num2'
go

--if you forget to use the keyword OUTPUT in the procedure call, the
--new values will NOT be passed back to the caller by the procedure, ie, try it:
DECLARE @num1 tinyint --must declare variables to use them
DECLARE @num2 tinyint
SET @num1 = 10 --assign test values
SET @num2 = 20
PRINT 'Test values before calling procedure:'
select @num1 '@num1',@num2 '@num2'
exec sp_ChangeParams @num1,@num2 --call SP passing parameters

PRINT 'Test values after calling procedure:'
select @num1 '@num1',@num2 '@num2'
go

-- running the above query clearly demonstrates the parameters are treated as INPUT
--parameters even though in the stored procedure definition they are marked OUTPUT

