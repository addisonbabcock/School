



create proc UpdateCustomer
	@CustomerId			int,
	@CustomerName		nvarchar(50),
	@CustomerAddress	nvarchar(50),
	@CustomerCity		nvarchar(50),
	@CustomerProvince	nvarchar(50),
	@CustomerPC			nvarchar(10),
	@Deleted			bit
as
begin
	
	if (@CustomerId is null)
	begin
		raiserror ('UpdateCustomer - @CustomerId cannot be null', 16, 1)
		return 1
	end

	if (@CustomerName is null)
	begin
		raiserror ('UpdateCustomer - @CustomerName cannot be null', 16, 1)
		return 1
	end

	if (@CustomerAddress is null)
	begin
		raiserror ('UpdateCustomer - @CustomerAddress cannot be null', 16, 1)
		return 1
	end

	if (@CustomerCity is null)
	begin
		raiserror ('UpdateCustomer - @CustomerCity cannot be null', 16, 1)
		return 1
	end

	if (@CustomerProvince is null)
	begin
		raiserror ('UpdateCustomer - @CustomerProvince cannot be null', 16, 1)
		return 1
	end

	if (@CustomerPC is null)
	begin
		raiserror ('UpdateCustomer - @CustomerPC cannot be null', 16, 1)
		return 1
	end

	if (@Deleted is null)
	begin
		raiserror ('UpdateCustomer - @Deleted cannot be null', 16, 1)
		return 1
	end

	update Customers set 
		CustomerName = @CustomerName, 
		CustomerAddress = @CustomerAddress, 
		CustomerCity = @CustomerCity, 
		CustomerProvince = @CustomerProvince, 
		CustomerPC = @CustomerPC, 
		Deleted = @Deleted
	where CustomerId = @CustomerId
	
	if @@ERROR <> 0
	begin
		return 1
		raiserror('UpdateCustomer - update failed', 16, 1)
	end

	return 0
end