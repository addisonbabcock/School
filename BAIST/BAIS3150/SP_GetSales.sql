

CREATE PROCEDURE SP_GetSale
	-- Add the parameters for the stored procedure here
	@SalesNumber as int
AS
	SET NOCOUNT ON

	if @SalesNumber is null
	begin
		RAISERROR (15600,-1,-1, 'SP_GetSale')
		return 1
	end

	select 
		SalesReceipts.SalesNumber as 'Sales Number',
		SalesReceipts.SaleDate as 'Sale Date',
		SalesReceipts.SalesPerson as 'Salesperson',
		Customers.CustomerName as 'Customer Name',
		Customers.CustomerAddress as 'Customer Address',
		Customers.CustomerCity as 'Customer City',
		Customers.CustomerProvince as 'Customer Province',
		Customers.CustomerPC as 'Customer Province',
		Items.ItemCode as 'Item Code',
		Items.Description as 'Description',
		Items.UnitPrice as 'Unit Price',
		SalesItems.Quantity as 'Quantity',
		SalesItems.ItemTotal as 'Item Total',
		SalesReceipts.Subtotal as 'Sub Total',
		SalesReceipts.GST as 'GST',
		SalesReceipts.Total as 'Sale Total'
	from SalesReceipts
	inner join Customers on Customers.CustomerId = SalesReceipts.CustomerId
	inner join SalesItems on SalesItems.SalesNumber = SalesReceipts.SalesNumber
	inner join Items on Items.ItemCode = SalesItems.ItemCode
	where SalesReceipts.SalesNumber = @SalesNumber
	order by SalesItems.ItemCode asc

	return 0
