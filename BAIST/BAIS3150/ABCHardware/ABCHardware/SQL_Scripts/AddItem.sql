


alter PROCEDURE AddItem
	@ItemCode			nvarchar(6),
	@ItemDescription	nvarchar(50),
	@UnitPrice			money,
	@Deleted			bit
AS
BEGIN

	if (@ItemCode is null)
	begin
		raiserror ('AddItem - @ItemCode cannot be null', 16, 1)
		return 1
	end

	if (@ItemDescription is null)
	begin
		raiserror ('AddItem - @ItemDescription cannot be null', 16, 1)
		return 1
	end

	if (@UnitPrice is null)
	begin
		raiserror ('AddItem - @UnitPrice cannot be null', 16, 1)
		return 1
	end

	if (@Deleted is null)
	begin
		raiserror ('AddItem - @Deleted cannot be null', 16, 1)
		return 1
	end

	insert into Items(ItemCode, Description, UnitPrice, Deleted) values
	(@ItemCode, @ItemDescription, @UnitPrice, @Deleted)

	if @@ERROR <> 0
	begin
		return 1
		raiserror('AddItem - insert failed', 16, 1)
	end

	return 0
END

