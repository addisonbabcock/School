

---------------
--drop tables--
---------------
if exists
(
	select [name]
	from sysobjects
	where [name] = 'SaleFact'
)
drop table SaleFact

if exists
(
	select [name]
	from sysobjects
	where [name] = 'CustomerDimension'
)
drop table CustomerDimension

if exists
(
	select [name]
	from sysobjects
	where [name] = 'EmployeeDimension'
)
drop table EmployeeDimension

if exists
(
	select [name]
	from sysobjects
	where [name] = 'ShipperDimension'
)
drop table ShipperDimension

if exists
(
	select [name]
	from sysobjects
	where [name] = 'ProductDimension'
)
drop table ProductDimension

if exists
(
	select [name]
	from sysobjects
	where [name] = 'TimeDimension'
)
drop table TimeDimension


-----------------
--create tables--
-----------------
create table TimeDimension
(
	TimeKey					int identity(1,1)	not null,
	TheDate					datetime,
	[DayOfWeek]				int,
	DayOfWeekName			nvarchar(30),
	[Month]					int,
	[MonthName]				nvarchar(30),
	[Year]					int,
	[Quarter]				int,
	[DayOfYear]				int,
	[Weekday]				nvarchar(3),
	[WeekOfYear]			int,


	constraint PK_TimeDimension_TimeKey primary key clustered 
		(TimeKey),
	constraint CK_TimeDimension_DayOfWeek check
		([DayOfWeek] >= 1 and [DayOfWeek] <= 7),
	constraint CK_TimeDimension_DayOfWeekName check
		(DayOfWeekName in ('Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday')),
	constraint CK_TimeDimension_Month check
		([Month] >= 1 and [Month] <= 12),
	constraint CK_TimeDimension_MonthName check
		([MonthName] in ('January', 'February', 'March', 'April', 'May', 'June',
		'July', 'August', 'September', 'October', 'November', 'December')),
	constraint CK_TimeDimension_Quarter check
		([Quarter] >= 1 and [Quarter] <= 4),
	constraint CK_TimeDimension_DayOfYear check
		([DayOfYear] >= 1 and [DayOfYear] <= 366),
	constraint CK_TimeDimension_Weekday check
		([Weekday] in ('No', 'Yes')),
	constraint CK_TimeDimension_WeekOfYear check
		([WeekOfYear] >= 1),
)


create table ProductDimension
(
	ProductKey				int identity(1,1)	not null,
	ProductName				nvarchar(40)		not null,
	ProductID				int					not null,
	SupplierName			nvarchar(40),
	CategoryName			nvarchar(15),
	ListUnitPrice			money,


	constraint PK_ProductDimension_ProductKey primary key clustered 
		(ProductKey),
)


create table ShipperDimension
(
	ShipperKey				int identity(1,1)	not null,
	ShipperName				nvarchar(40)		not null,
	ShipperID				int					not null,


	constraint PK_ShipperDimension_ShipperKey primary key clustered
		(ShipperKey),
)


create table EmployeeDimension
(
	EmployeeKey				int identity(1,1)	not null,
	EmployeeName			nvarchar(31)		not null,
	EmployeeID				int					not null,
	HireDate				datetime,


	constraint PK_EmployeeDimension_EmployeeKey primary key clustered
		(EmployeeKey),
)


create table CustomerDimension
(
	CustomerKey				int identity(1,1)	not null,
	CompanyName				nvarchar(40),
	CustomerID				nvarchar(5)			not null,
	ContactName				nvarchar(30),
	ContactTitle			nvarchar(30),
	[Address]				nvarchar(60),
	City					nvarchar(15),
	Region					nvarchar(15),
	PostalCode				nvarchar(10),
	Country					nvarchar(15),
	Phone					nvarchar(24),
	Fax						nvarchar(24),


	constraint PK_CustomerDimension_CustomerKey primary key clustered
		(CustomerKey),
)


create table SaleFact
(
	CustomerKey				int					not null,
	EmployeeKey				int					not null,
	ProductKey				int					not null,
	ShipperKey				int					not null,
	TimeKey					int					not null,
	LineItemQuantity		int					not null,
	LineItemDiscount		money				not null,
	LineItemFreight			money				not null,
	LineItemTotal			money				not null,


	constraint PK_SaleFact primary key nonclustered
		(CustomerKey, EmployeeKey, ProductKey, ShipperKey, TimeKey),
	constraint FK_SaleFact_CustomerKey foreign key
		(CustomerKey) references CustomerDimension (CustomerKey),
	constraint FK_SaleFact_EmployeeKey foreign key
		(EmployeeKey) references EmployeeDimension (EmployeeKey),
	constraint FK_SaleFact_ProductKey foreign key
		(ProductKey) references ProductDimension (ProductKey),
	constraint FK_SaleFact_ShipperKey foreign key
		(ShipperKey) references ShipperDimension (ShipperKey),
	constraint FK_SaleFact_TimeKey foreign key
		(TimeKey) references TimeDimension (TimeKey),
)


