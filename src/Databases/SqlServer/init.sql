-- Create database
CREATE DATABASE DBBroker;
GO

-- Use the new database
USE [DBBroker]
GO

-- #TABLES
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[Email] [varchar](250) NULL,
	[Birthday] [date] NULL,
	[OrdersTotal] [int] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[ModifiedAt] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Addresses](
	[Id] [uniqueidentifier] NOT NULL,
	[Street] [varchar](250) NOT NULL,
	[City] [varchar](100) NOT NULL,
	[State] [varchar](50) NOT NULL,
	[PostalCode] [varchar](20) NOT NULL,
	[Country] [varchar](100) NOT NULL
 CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomersNotes](
	[Id] [uniqueidentifier] NOT NULL,
	[CustomerId] [uniqueidentifier] NULL,
	[StatusId] [int] NULL,
	[NoteContent] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CustomerNotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomersNotesStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [varchar](50) NULL,
 CONSTRAINT [PK_CustomersNotesStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [uniqueidentifier] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[BillingAddressId] [uniqueidentifier] NOT NULL,
	[ShippingAddressId] [uniqueidentifier] NOT NULL,
	[StatusId] [int] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[ModifiedAt] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdersNotes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[NoteContent] [varchar](1024) NOT NULL,
 CONSTRAINT [PK_OrderNotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdersProducts](
	[Id] [uniqueidentifier] NOT NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_OrderProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdersStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [varchar](50) NOT NULL,
 CONSTRAINT [PK_OrdersStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [uniqueidentifier] NOT NULL,
	[ProductName] [varchar](50) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promotions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[Expiration] [datetime] NOT NULL,
 CONSTRAINT [PK_Promotions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PromotionsEnrollments](
	[Id] [int] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[PromotionId] [int] NOT NULL,
 CONSTRAINT [PK_PromotionsEnrollments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- #CONSTRAINTS
ALTER TABLE [dbo].[Customers] WITH CHECK ADD CONSTRAINT FK_CustomersAddressId FOREIGN KEY([Id])
REFERENCES [dbo].[Addresses] ([Id])
GO
--
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customers]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_OrdersStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[OrdersStatus] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_OrdersStatus]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_BillingAddress] FOREIGN KEY([BillingAddressId])
REFERENCES [dbo].[Addresses] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_BillingAddress]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_ShippingAddress] FOREIGN KEY([ShippingAddressId])
REFERENCES [dbo].[Addresses] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_ShippingAddress]
GO
--
ALTER TABLE [dbo].[CustomersNotes]  WITH CHECK ADD  CONSTRAINT [FK_CustomerNotes_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
GO
ALTER TABLE [dbo].[CustomersNotes] CHECK CONSTRAINT [FK_CustomerNotes_Customers]
GO
ALTER TABLE [dbo].[CustomersNotes]  WITH CHECK ADD  CONSTRAINT [FK_CustomersNotes_CustomersNotesStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[CustomersNotesStatus] ([Id])
GO
ALTER TABLE [dbo].[CustomersNotes] CHECK CONSTRAINT [FK_CustomersNotes_CustomersNotesStatus]
GO
--
ALTER TABLE [dbo].[OrdersNotes]  WITH CHECK ADD  CONSTRAINT [FK_OrderNotes_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
GO
ALTER TABLE [dbo].[OrdersNotes] CHECK CONSTRAINT [FK_OrderNotes_Orders]
GO
--
ALTER TABLE [dbo].[OrdersProducts]  WITH CHECK ADD  CONSTRAINT [FK_OrderProducts_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
GO
ALTER TABLE [dbo].[OrdersProducts] CHECK CONSTRAINT [FK_OrderProducts_Orders]
GO
ALTER TABLE [dbo].[OrdersProducts]  WITH CHECK ADD  CONSTRAINT [FK_OrderProducts_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[OrdersProducts] CHECK CONSTRAINT [FK_OrderProducts_Products]
GO
--
ALTER TABLE [dbo].[PromotionsEnrollments]  WITH CHECK ADD  CONSTRAINT [FK_PromotionsEnrollments_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
GO
ALTER TABLE [dbo].[PromotionsEnrollments] CHECK CONSTRAINT [FK_PromotionsEnrollments_Customers]
GO
ALTER TABLE [dbo].[PromotionsEnrollments]  WITH CHECK ADD  CONSTRAINT [FK_PromotionsEnrollments_Promotions] FOREIGN KEY([PromotionId])
REFERENCES [dbo].[Promotions] ([Id])
GO
ALTER TABLE [dbo].[PromotionsEnrollments] CHECK CONSTRAINT [FK_PromotionsEnrollments_Promotions]
GO


-- #VIEWS
CREATE OR ALTER VIEW VwOrderSummary
AS
SELECT 	c.Name AS CustomerName, 
		o.StatusId AS OrderStatusId, 
		s.Status AS OrderStatus, 
		(SELECT COUNT(*) FROM OrdersProducts op WHERE op.OrderId = o.Id ) AS NumberOfItems
FROM Orders o
JOIN Customers c ON c.ID = o.CustomerId 
JOIN OrdersStatus s ON s.Id = o.StatusId;
GO

CREATE OR ALTER VIEW VwOrders
AS
SELECT 
		o.Id AS OrderId, 
		o.CreatedAt AS OrderCreation,
		o.CustomerId,
		c.Name AS CustomerName,
		p.Id AS ProductId,
		p.ProductName,
		p.Price
FROM Orders o
JOIN Customers c ON c.Id = o.CustomerId
JOIN OrdersStatus s ON s.Id = o.StatusId
JOIN OrdersProducts op ON op.OrderId = o.Id
JOIN Products p ON p.Id = op.ProductId;
GO
