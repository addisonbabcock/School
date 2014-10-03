

create proc ababcock1GetCategory
	@CategoryID		int
as
	declare @return_status int
	select @return_status = 0

	if @CategoryID is null
	begin
		select @return_status = 1
		raiserror ('GetCategory - parameters cannot be null.', 16, 1)
	end else begin
		select Categories.CategoryID, Categories.CategoryName, Categories.Description
			from Categories where Categories.CategoryID = @CategoryID

		if @@ERROR <> 0
		begin
			select @return_status = 1
			raiserror ('GetCategory - select error', 16, 1)
		end
	end

	return @return_status


exec ababcock1GetCategory null

exec ababcock1GetCategory '1'

exec ababcock1GetCategory '-123'

