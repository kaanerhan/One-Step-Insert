# One-Step-Insert



CREATE TABLE [dbo].[OrderDetails](
	[Id] [varchar](60) NOT NULL,
	[OrderId] [varchar](60) NOT NULL,
	[ProductCode] [nvarchar](50) NOT NULL,
	[ProductName] [nvarchar](50) NOT NULL,
	[Discount] [decimal](18, 4) NULL,
	[Price] [decimal](18, 4) NULL,
	[Amount] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Order](
	[Id] [varchar](60) NOT NULL,
	[OrderNumber] [nvarchar](50) NOT NULL,
	[TotalPrice] [decimal](18, 4) NULL,
	[TotalDiscount] [decimal](18, 4) NULL,
	[CustomerFirstName] [nvarchar](50) NULL,
	[CustomerLastName] [nvarchar](50) NULL,
	[CargoTrackingNumber] [nvarchar](50) NULL,
	[OrderDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


