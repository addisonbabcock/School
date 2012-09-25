use CNT3K_L4

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Invoices_Items')
   drop table Invoices_Items
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Invoices')
   drop table Invoices
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Customers')
   drop table Customers
GO

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'Items')
   drop table Items
GO
--+2

create table Items
(
	ItemId			int identity (1, 1)	not null,
	Description		varchar (100)		not null,
	CurrentPrice	smallmoney			not null,

	constraint PK_ItemId primary key clustered
		(ItemId),
	constraint CK_CurrentPrice check (CurrentPrice > 0),
)

create table Customers
(
	CustomerId		int identity (1, 1)	not null,
	LastName		varchar (100)		not null,
	FirstName		varchar (100)		not null,
	Phone			char (14)			null,

	constraint PK_CustomerId primary key clustered
		(CustomerId),
	constraint CK_Phone check (Phone like 
		'([0-9][0-9][0-9])-[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]'),
)
--+2

create table Invoices
(
	InvoiceId		int identity (1, 1)	not null,
	InvoiceDate		smalldatetime		not null,
	CustomerId		int					not null,
	Subtotal		money				not null,
	GST				tinyint				not null,
	Total as Subtotal + Subtotal * GST / 100,

	constraint PK_InvoiceId primary key clustered
		(InvoiceId),
	constraint FK_InvoicesToCustomers foreign key
		(CustomerId) references Customers (CustomerId),
	constraint CK_InvoiceDate check (
		InvoiceDate >= GetDate ()),
	constraint CK_Subtotal check (Subtotal > 0),
	constraint CK_GST check (GST >= 7),
)
--+2

create table Invoices_Items
(
	InvoiceId		int					not null,
	ItemId			int					not null,
	Quantity		smallint			not null,
	PriceChargedPerItem
					smallmoney			not null,
	AmountChargedForItems as Quantity * PriceChargedPerItem,

	constraint PK_InvoiceId_ItemId primary key clustered
		(InvoiceId, ItemId),
	constraint FK_Invoices_ItemsToInvoices foreign key
		(InvoiceId) references Invoices (InvoiceId),
	constraint FK_Invoices_ItemsToItems foreign key
		(ItemId) references Items (ItemId),
	constraint CK_Quantity check (Quantity > 0),
	constraint CK_PriceChargedPerItem check (PriceChargedPerItem > 0),
)
--+2

--10/10