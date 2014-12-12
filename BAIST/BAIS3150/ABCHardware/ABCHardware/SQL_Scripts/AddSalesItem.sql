




alter PROCEDURE AddSalesItem
	@SalesNumber	int,
	@ItemCode		nvarchar(6),
	@Quantity		int,
	@ItemTotal		money
AS
BEGIN
	if (@SalesNumber is null)
	begin
		raiserror ('AddSalesItem - @SalesNumber cannot be null', 16, 1)
		return 1
	end

	if (@ItemCode is null)
	begin
		raiserror ('AddSalesItem - @ItemCode cannot be null', 16, 1)
		return 1
	end

	if (@Quantity is null)
	begin
		raiserror ('AddSalesItem - @Quantity cannot be null', 16, 1)
		return 1
	end

	if (@ItemTotal is null)
	begin
		raiserror ('AddSalesItem - @ItemTotal cannot be null', 16, 1)
		return 1
	end

	insert into SalesItems (SalesNumber, ItemCode, Quantity, ItemTotal)
	values (@SalesNumber, @ItemCode, @Quantity, @ItemTotal)

	if (@@ERROR <> 0)
	begin
		raiserror('AddSalesItem - insert failed', 16, 1)
		return 1
	end

	return 0
END
GO
