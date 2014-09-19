
use ababcock1_BAIS3150_Implementation_Assignment


--Create tables--

CREATE TABLE [dbo].[Customers](
	[CustomerId] [int] NOT NULL,
	[CustomerName] [nvarchar](50) NOT NULL,
	[CustomerAddress] [nvarchar](50) NOT NULL,
	[CustomerCity] [nvarchar](50) NOT NULL,
	[CustomerProvince] [nvarchar](50) NOT NULL,
	[CustomerPC] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY
(
	[CustomerId] ASC
)
) ON [PRIMARY]



CREATE TABLE [dbo].[Items](
	[ItemCode] [nvarchar](6) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[UnitPrice] [money] NOT NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[ItemCode] ASC
)
) ON [PRIMARY]



CREATE TABLE [dbo].[SalesItems](
	[SalesNumber] [int] NOT NULL,
	[ItemCode] [nvarchar](6) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ItemTotal] [money] NOT NULL,
 CONSTRAINT [PK_SalesItems] PRIMARY KEY CLUSTERED 
(
	[SalesNumber] ASC,
	[ItemCode] ASC
)
) ON [PRIMARY]



CREATE TABLE [dbo].[SalesReceipts](
	[SalesNumber] [int] NOT NULL,
	[SaleDate] [date] NOT NULL,
	[SalesPerson] [nvarchar](50) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Subtotal] [money] NOT NULL,
	[GST] [money] NOT NULL,
	[Total] [money] NOT NULL,
 CONSTRAINT [PK_SalesReceipt] PRIMARY KEY CLUSTERED 
(
	[SalesNumber] ASC
)
) ON [PRIMARY]



--Add FK constraints--
ALTER TABLE [dbo].[SalesItems]  WITH CHECK ADD  CONSTRAINT [FK_SalesItems_Items] FOREIGN KEY([ItemCode])
REFERENCES [dbo].[Items] ([ItemCode])
ALTER TABLE [dbo].[SalesItems] CHECK CONSTRAINT [FK_SalesItems_Items]


ALTER TABLE [dbo].[SalesItems]  WITH CHECK ADD  CONSTRAINT [FK_SalesItems_SalesReceipt] FOREIGN KEY([SalesNumber])
REFERENCES [dbo].[SalesReceipt] ([SalesNumber])
ALTER TABLE [dbo].[SalesItems] CHECK CONSTRAINT [FK_SalesItems_SalesReceipt]


ALTER TABLE [dbo].[SalesReceipt]  WITH CHECK ADD  CONSTRAINT [FK_SalesReceipt_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
ALTER TABLE [dbo].[SalesReceipt] CHECK CONSTRAINT [FK_SalesReceipt_Customers]



--Add sample items--
insert into Items (ItemCode, Description, UnitPrice)
values ('I12847', 'Vice Grip 1/2 Inch', 10.00)

insert into Items (ItemCode, Description, UnitPrice)
values ('N22475', 'Claw Hammer', 15.00)

insert into Items (ItemCode, Description, UnitPrice)
values ('P77455', 'Torque Wrench', 75.00)

insert into Items (ItemCode, Description, UnitPrice)
values ('Q12345', 'Box of nails', 50.00)



--Add sample sales--


--First--
insert into Customers (CustomerId, CustomerName, CustomerAddress, CustomerCity, CustomerProvince, CustomerPC)
values (1, 'John Smith', '12345 67 Street', 'Edmonton', 'Alberta', 'T6T 6T6')

insert into SalesReceipts (SalesNumber, SaleDate, SalesPerson, CustomerId, Subtotal, GST, Total)
values (123456789, '16Jan2004', 'Jenny Brooks', 1, 115.00, 8.05, 123.05)

insert into SalesItems (SalesNumber, ItemCode, Quantity, ItemTotal)
values (123456789, 'I12847', 1, 10.00)

insert into SalesItems (SalesNumber, ItemCode, Quantity, ItemTotal)
values (123456789, 'N22475', 2, 30.00)

insert into SalesItems (SalesNumber, ItemCode, Quantity, ItemTotal)
values (123456789, 'P77455', 1, 75.00)



--Second--
insert into Customers (CustomerId, CustomerName, CustomerAddress, CustomerCity, CustomerProvince, CustomerPC)
values (2, 'Super Man', '321 Any Street', 'Edmonton', 'Alberta', 'A1A 1A1')

insert into SalesReceipts (SalesNumber, SaleDate, SalesPerson, CustomerId, Subtotal, GST, Total)
values (15, '18Sep2014', 'Addison Babcock', 2, 65.00, 4.55, 69.55)

insert into SalesItems (SalesNumber, ItemCode, Quantity, ItemTotal)
values (15, 'Q12345', 1, 50.00)

insert into SalesItems (SalesNumber, ItemCode, Quantity, ItemTotal)
values (15, 'N22475', 1, 15.00)



--Test SP_GetSale
exec SP_GetSale 15

exec SP_GetSale 123456789

exec SP_GetSale null

