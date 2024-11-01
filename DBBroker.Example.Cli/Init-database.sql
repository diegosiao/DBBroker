CREATE DATABASE DbBroker
GO

USE [DbBroker]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Person](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[Birthday] [date] NULL,
	[Salary] [money] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[UpdatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
