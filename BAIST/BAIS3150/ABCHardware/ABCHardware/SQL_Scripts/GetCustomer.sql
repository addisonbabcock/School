


create PROCEDURE GetCustomer
	@CustomerId			int
AS
BEGIN

	if (@CustomerId is null)
	begin
		raiserror ('GetCustomer - @CustomerId cannot be null', 16, 1)
		return 1
	end

	select * from Customers where Customers.CustomerId = @CustomerId

	if @@ERROR <> 0
	begin
		return 1
		raiserror('GetCustomer - insert failed', 16, 1)
	end

	return 0
END

