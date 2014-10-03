

create proc ababcock1GetCustomersByCountry
	@Country		nvarchar(15)
as
	declare @return_status int
	select @return_status = 0

	if @Country is null
	begin
		select @return_status = 1
		raiserror ('GetCustomersByCountry - parameters cannot be null.', 16, 1)
	end else begin
		select Customers.CustomerID, Customers.CompanyName, Customers.ContactName, 
			Customers.ContactTitle, Customers.Address, Customers.City, Customers.Region,
			Customers.PostalCode, Customers.Phone, Customers.Fax
			from Customers where Customers.Country = @Country

		if @@ERROR <> 0
		begin
			select @return_status = 1
			raiserror ('GetCustomersByCountry - select error', 16, 1)
		end
	end

	return @return_status


exec ababcock1GetCustomersByCountry null

exec ababcock1GetCustomersByCountry 'Canada'

exec ababcock1GetCustomersByCountry 'Notexistastan'



