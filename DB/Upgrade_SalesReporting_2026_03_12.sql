USE [JJAS-ISBST];
GO

IF OBJECT_ID('dbo.tbl_SalesTransaction', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[tbl_SalesTransaction](
        [TransactionID] [int] IDENTITY(1,1) NOT NULL,
        [TransactionNo] [nvarchar](50) NULL,
        [SaleDate] [datetime] NULL,
        [UserID] [int] NULL,
        [TotalItems] [int] NULL,
        [TotalDiscount] [decimal](18, 2) NULL,
        [TotalVAT] [decimal](18, 2) NULL,
        [TotalAmount] [decimal](18, 2) NULL,
        CONSTRAINT [PK_tbl_SalesTransaction] PRIMARY KEY CLUSTERED ([TransactionID] ASC)
    ) ON [PRIMARY]
END
GO

IF OBJECT_ID('dbo.tbl_ProductPriceHistory', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[tbl_ProductPriceHistory](
        [PriceHistoryID] [int] IDENTITY(1,1) NOT NULL,
        [ProductID] [int] NOT NULL,
        [OldCostPrice] [decimal](18, 2) NULL,
        [NewCostPrice] [decimal](18, 2) NULL,
        [OldSellingPrice] [decimal](18, 2) NOT NULL,
        [NewSellingPrice] [decimal](18, 2) NOT NULL,
        [ChangedByUserID] [int] NULL,
        [ChangeReason] [nvarchar](255) NULL,
        [DateChanged] [datetime] NOT NULL,
        CONSTRAINT [PK_tbl_ProductPriceHistory] PRIMARY KEY CLUSTERED ([PriceHistoryID] ASC)
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[tbl_ProductPriceHistory]
        ADD CONSTRAINT [DF_tbl_ProductPriceHistory_DateChanged] DEFAULT (GETDATE()) FOR [DateChanged]
END
GO

CREATE OR ALTER VIEW [dbo].[vw_SalesReport]
AS
SELECT
    TransactionNo,
    SaleDate,
    UserID,
    TotalItems,
    TotalDiscount,
    TotalVAT,
    TotalAmount
FROM dbo.tbl_SalesTransaction;
GO

CREATE OR ALTER VIEW [dbo].[vw_FinancialReport_Daily]
AS
SELECT
    CAST(SaleDate AS DATE) AS ReportDate,

    SUM(Quantity * SellingPrice) AS GrossSales,

    SUM(Quantity * CostPrice) AS TotalCost,

    SUM(Discount) AS TotalDiscount,

    SUM(VatAmount) AS TotalVAT,

    SUM(TotalAmount) AS NetSales,

    SUM((SellingPrice - CostPrice) * Quantity) AS Profit

FROM dbo.tbl_SalesHistory
GROUP BY CAST(SaleDate AS DATE);
GO

CREATE OR ALTER VIEW [dbo].[vw_FinancialReport_Monthly]
AS
SELECT
    YEAR(SaleDate) AS SalesYear,
    MONTH(SaleDate) AS SalesMonth,

    SUM(Quantity * SellingPrice) AS GrossSales,

    SUM(Quantity * CostPrice) AS TotalCost,

    SUM(Discount) AS TotalDiscount,

    SUM(VatAmount) AS TotalVAT,

    SUM(TotalAmount) AS NetSales,

    SUM((SellingPrice - CostPrice) * Quantity) AS Profit

FROM dbo.tbl_SalesHistory
GROUP BY
    YEAR(SaleDate),
    MONTH(SaleDate);
GO
