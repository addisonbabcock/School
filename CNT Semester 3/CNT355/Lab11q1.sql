If EXISTS (select name from sysobjects
				where name='spQ1')
	DROP proc spQ1

create proc spQ1
as
declare @resnum int

begin transaction

insert reservations (ReservationDateTime, CustomerName, ContactName, Phone, NumberInParty, EventDescription, Reservationstatus)
values ('Dec 31, 2003 17:00', 'Arafat and Sharon', 'George Bush', '666-1287', 3, 'Peace Talks', 'R')

if @@ERROR <> 0
begin
	rollback transaction
	print 'Error inserting reservation'
	return 1
end

select @resnum = @@IDENTITY
insert tablesforreservation (reservationnumber, tablenumber)
values (@resnum, 21)

if @@ERROR <> 0
begin
	rollback transaction
	print 'Error reserving table 21'
	return 2
end

insert tablesforreservation (reservationnumber, tablenumber)
values (@resnum, 22)

if @@ERROR <> 0
begin
	rollback transaction
	print 'Error reserving table 22'
	return 3
end

commit transaction
print 'Success'
return 0

exec spQ1

--17/17