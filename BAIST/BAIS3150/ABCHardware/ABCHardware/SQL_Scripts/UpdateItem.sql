


alter PROCEDURE UpdateItem
	@ItemCode			nvarchar(6),
	@ItemDescription	nvarchar(50),
	@UnitPrice			money,
	@Deleted			bit
AS
BEGIN

	if (@ItemCode is null)
	begin
		raiserror ('UpdateItem - @ItemCode cannot be null', 16, 1)
		return 1
	end

	if (@ItemDescription is null)
	begin
		raiserror ('UpdateItem - @ItemDescription cannot be null', 16, 1)
		return 1
	end

	if (@UnitPrice is null)
	begin
		raiserror ('UpdateItem - @UnitPrice cannot be null', 16, 1)
		return 1
	end

	if (@Deleted is null)
	begin
		raiserror ('UpdateItem - @Deleted cannot be null', 16, 1)
		return 1
	end

	update Items set Description = @ItemDescription, UnitPrice = @UnitPrice, Deleted = @Deleted
	where ItemCode = @ItemCode

	if @@ERROR <> 0
	begin
		return 1
		raiserror('UpdateItem - update failed', 16, 1)
	end

	return 0
END

