USE [PleasantRustle]
GO
/****** Object:  Table [dbo].[Agents]    Script Date: 08.02.2024 18:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agents](
	[ID] [int] NOT NULL,
	[Type] [nvarchar](3) NULL,
	[Name] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Logo] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Priority] [int] NULL,
	[Director] [nvarchar](50) NULL,
	[TaxNumber] [nvarchar](10) NULL,
	[CodeRegistration] [nchar](9) NULL,
 CONSTRAINT [PK_Agents] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 08.02.2024 18:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Article] [int] NOT NULL,
	[AmountPeople] [int] NOT NULL,
	[ShopNumber] [int] NOT NULL,
	[MinimumPrice] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Realisation]    Script Date: 08.02.2024 18:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Realisation](
	[ProductID] [int] NOT NULL,
	[AgentID] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[Quantity] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Realisation]  WITH CHECK ADD  CONSTRAINT [FK_Realization_Agents] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agents] ([ID])
GO
ALTER TABLE [dbo].[Realisation] CHECK CONSTRAINT [FK_Realization_Agents]
GO
ALTER TABLE [dbo].[Realisation]  WITH CHECK ADD  CONSTRAINT [FK_Realization_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ID])
GO
ALTER TABLE [dbo].[Realisation] CHECK CONSTRAINT [FK_Realization_Products]
GO
