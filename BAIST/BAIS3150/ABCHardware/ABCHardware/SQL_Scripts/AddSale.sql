




alter PROCEDURE AddSale
	@SaleDate		date,
	@SalesPerson	nvarchar(50),
	@CustomerId		int,
	@Subtotal		money,
	@GST			money,
	@Total			money
AS
BEGIN
	if (@SaleDate is null)
	begin
		raiserror ('AddSale - @SaleDate cannot be null', 16, 1)
		return 1
	end

	if (@SalesPerson is null)
	begin
		raiserror ('AddSale - @SalesPerson cannot be null', 16, 1)
		return 1
	end

	if (@CustomerId is null)
	begin
		raiserror ('AddSale - @CustomerId cannot be null', 16, 1)
		return 1
	end

	if (@Subtotal is null)
	begin
		raiserror ('AddSale - @Subtotal cannot be null', 16, 1)
		return 1
	end

	if (@GST is null)
	begin
		raiserror ('AddSale - @GST cannot be null', 16, 1)
		return 1
	end

	if (@Total is null)
	begin
		raiserror ('AddSale - @Total cannot be null', 16, 1)
		return 1
	end

	insert into SalesReceipts (SaleDate, SalesPerson, CustomerId, Subtotal, GST, Total)
	values (@SaleDate, @SalesPerson, @CustomerId, @Subtotal, @GST, @Total)

	select SCOPE_IDENTITY()

	if (@@ERROR <> 0)
	begin
		raiserror('AddCustomer - insert failed', 16, 1)
		return 1
	end

	return 0
END
GO
