USE [JJAS-ISBST];
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_tbl_SalesHistory_SaleDate'
      AND object_id = OBJECT_ID('dbo.tbl_SalesHistory')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_tbl_SalesHistory_SaleDate]
    ON [dbo].[tbl_SalesHistory] ([SaleDate] ASC);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_tbl_SalesHistory_Name_SaleDate'
      AND object_id = OBJECT_ID('dbo.tbl_SalesHistory')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_tbl_SalesHistory_Name_SaleDate]
    ON [dbo].[tbl_SalesHistory] ([Name] ASC, [SaleDate] ASC)
    INCLUDE ([TransactionNo], [ProductName], [Quantity], [TotalAmount]);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_tbl_SalesHistory_SaleDate_TransactionNo'
      AND object_id = OBJECT_ID('dbo.tbl_SalesHistory')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_tbl_SalesHistory_SaleDate_TransactionNo]
    ON [dbo].[tbl_SalesHistory] ([SaleDate] ASC, [TransactionNo] ASC)
    INCLUDE (
        [Name],
        [ProductName],
        [BarcodeNumber],
        [Category],
        [Quantity],
        [SellingPrice],
        [CostPrice],
        [TotalAmount],
        [Discount],
        [VatAmount]
    );
END
GO
