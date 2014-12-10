


alter PROCEDURE GetItem
	@ItemCode			nvarchar(6)
AS
BEGIN

	if (@ItemCode is null)
	begin
		raiserror ('GetItem - @ItemCode cannot be null', 16, 1)
		return 1
	end

	select * from Items where Items.ItemCode = @ItemCode

	if @@ERROR <> 0
	begin
		return 1
		raiserror('GetItem - insert failed', 16, 1)
	end

	return 0
END

