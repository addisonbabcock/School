if exists (select name from sysobjects where name = 'spArchiveBills')
	drop proc spArchiveBills

if exists (select name from sysobjects where name = 'ArchiveBills')
	drop table ArchiveBills

if exists (select name from sysobjects where name = 'ArchiveItemsOnBill')
	drop table ArchiveItemsOnBill

create table ArchiveBills
(
	BillNumber			int				not null,
	BilDate				smalldatetime	not null,
	ReservationNumber	int				null,
	Subtotal			smallmoney		not null,
	GST					smallmoney		not null,
	Total				smallmoney		not null,
	EmployeeId			int				not null,
	PaymentCode			char (1)		null,
--	ArchiveDate			smalldatetime	null
)

create table ArchiveItemsOnBill
(
	BillNumber			int				not null,
	ItemNumber			int				not null,
	Quantity			tinyint			not null,
	Price				smallmoney		not null,
	Amount				smallmoney		null
)

create proc spArchiveBills
	@year	int = null,
	@month	int = null
as
if	@year is null or
	@year > year (getdate ())
begin
	print 'Bad year value: ' + @year
	return 2
end
else
begin
	if	@month is null or
		@month < 1 or
		@month > 12
	begin
		print 'Bad month value: ' + @month
		return 3
	end
	else
	begin
		if not exists (	select	* 
						from	Bills 
						where	year (billdate) = @year and
								month (billdate) = @month )
		begin
			print 'Nothing to archive'
			return 4
		end
		else
		begin
			begin transaction
--insert archivebills
--select billnumber, /*getdate (),*/ billdate, reservationnumber,
--subtotal, gst, employeeid
--from bills
--where year (billdate) = @year and
--month (billdate) = @month
			select	*
			into	archivebills
			from	bills
			where	year (billdate) = @year and
					month (billdate) = @month
			if @@ERROR <> 0
			begin
				rollback transaction
				print 'Error saving Bills'
				return 1
			end
			else
			begin
--insert archiveitemonbill (billnumber, itemnumber, quantity, price)
--select billnumber, itemnumber, quantity, price
--from billnumber in (select billnumber from bills where year(billdate) = @year and month (billdate) = @month)
				select	*
				into	archiveitemonbill
				from	itemonbill
				where	billnumber in (	select	billnumber
										from	bills
										where	year (billdate) = @year and
												month (billdate) = @month)
				if @@error <> 0
				begin
					rollback transaction
					print 'Error saving ItemOnBill'
					return 1
				end
				else
				begin
					delete tableonbill
					where billnumber in (	select billnumber
											from bills
											where	year (billdate) = @year and
													month (billdate) = @month)
					if @@error <> 0
					begin
						rollback transaction
						print 'Error deleting old table data'
						return 1
					end
					else
					begin
						delete itemonbill
						where billnumber in (	select billnumber
												from bills
												where	year (billdate) = @year and
														month (billdate) = @month)
						if @@ERROR <> 0
						begin
							rollback transaction
							print 'Error deleting old item data'
							return 1
						end
						else
						begin
							delete from bills
							where	year (billdate) = @year and
									month (billdate) = @month
							if @@error <> 0
							begin
								rollback transaction
								print 'Error deleting old bills'
								return 1
							end
							else
							begin
								commit transaction
								print 'Bills archived'
								return 0
							end
						end
					end
				end
			end
		end
	end
end
