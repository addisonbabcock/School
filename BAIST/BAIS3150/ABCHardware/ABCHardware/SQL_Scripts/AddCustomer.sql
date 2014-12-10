



create proc AddCustomer
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
		raiserror ('AddCustomer - @CustomerId cannot be null', 16, 1)
		return 1
	end

	if (@CustomerName is null)
	begin
		raiserror ('AddCustomer - @CustomerName cannot be null', 16, 1)
		return 1
	end

	if (@CustomerAddress is null)
	begin
		raiserror ('AddCustomer - @CustomerAddress cannot be null', 16, 1)
		return 1
	end

	if (@CustomerCity is null)
	begin
		raiserror ('AddCustomer - @CustomerCity cannot be null', 16, 1)
		return 1
	end

	if (@CustomerProvince is null)
	begin
		raiserror ('AddCustomer - @CustomerProvince cannot be null', 16, 1)
		return 1
	end

	if (@CustomerPC is null)
	begin
		raiserror ('AddCustomer - @CustomerPC cannot be null', 16, 1)
		return 1
	end

	if (@Deleted is null)
	begin
		raiserror ('AddCustomer - @Deleted cannot be null', 16, 1)
		return 1
	end

	insert into Customers (CustomerId, CustomerName, CustomerAddress, CustomerCity, CustomerProvince, CustomerPC, Deleted)
	values (@CustomerId, @CustomerName, @CustomerAddress, @CustomerCity, @CustomerProvince, @CustomerPC, @Deleted)
	
	if @@ERROR <> 0
	begin
		return 1
		raiserror('AddCustomer - insert failed', 16, 1)
	end

	return 0
end