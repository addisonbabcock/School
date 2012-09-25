if exists (select name from sysobjects where name = 'spQ2')
	drop proc spQ2

create proc spQ2
	@ResNum int = null
as

if @resnum is null
begin
	print 'No Reservation Number was supplied'
	return 2
end
else
begin
	if not exists (	select	*
					from	reservations 
					where	reservationnumber = @resnum and
							reservationstatus = 'R')
	begin
		print 'Reservation not found'
		return 3
	end
	else
	begin
		begin tran
		update	reservations
		set		reservationstatus = 'C'
		where	reservationnumber = @resnum
		if @@ERROR <> 0
		begin
			rollback tran
			print 'Reservation could not be cancelled'
			return 1
		end
		else
		begin
			delete	tablesforreservation
			where	reservationnumber = @resnum

			if @@error <> 0
			begin
				rollback tran
				print 'Error removing tables'
				return 1
			end
			else
			begin
				commit tran
				return 0
			end
		end
	end
end

exec restaurant.sqQ2 6

--22/22