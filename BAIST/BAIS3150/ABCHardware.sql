
use ABCHardware


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
	[SalesNumber] [int] NOT NULL identity(1, 1),
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


ALTER TABLE [dbo].[SalesItems]  WITH CHECK ADD  CONSTRAINT [FK_SalesItems_SalesReceipts] FOREIGN KEY([SalesNumber])
REFERENCES [dbo].[SalesReceipts] ([SalesNumber])
ALTER TABLE [dbo].[SalesItems] CHECK CONSTRAINT [FK_SalesItems_SalesReceipts]


ALTER TABLE [dbo].[SalesReceipts]  WITH CHECK ADD  CONSTRAINT [FK_SalesReceipts_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
ALTER TABLE [dbo].[SalesReceipts] CHECK CONSTRAINT [FK_SalesReceipts_Customers]



