--transactions:
--begin transaction
--	marks initial state of data
--<do stuff here>
if @@ERROR
	--rollback transaction
	--	used when an error is detected, causes data to be restored to the state
	--	it was in when begin tran was marked
else
	--commit transaction
	--	used when all the steps of the transaction have completed

--www.bctechnology.com