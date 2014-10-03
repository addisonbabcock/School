

create proc ababcock1GetProductsByCategory
	@CategoryID		int
as
	declare @return_status int
	select @return_status = 0

	if @CategoryID is null
	begin
		select @return_status = 1
		raiserror ('GetProductsByCategory - parameters cannot be null.', 16, 1)
	end else begin
		select Products.ProductID, Products.ProductName, Suppliers.CompanyName as 'SupplierCompanyName', 
		Products.QuantityPerUnit, Products.UnitPrice, Products.UnitsInStock, Products.UnitsOnOrder, 
		Products.ReorderLevel, Products.Discontinued
			from Products 
			inner join Suppliers on Suppliers.SupplierID = Products.SupplierID
			where Products.CategoryID = @CategoryID

		if @@ERROR <> 0
		begin
			select @return_status = 1
			raiserror ('GetProductsByCategory - select error', 16, 1)
		end
	end

	return @return_status


exec ababcock1GetProductsByCategory null

exec ababcock1GetProductsByCategory '1'

exec ababcock1GetProductsByCategory '-123'

