Imports System.Data
Imports System.Data.SqlClient

Public Class CashierDashboardSummary
    Public Property TransactionsToday As Integer
    Public Property ItemsSoldToday As Integer
    Public Property SalesToday As Decimal
    Public Property ProfitToday As Decimal
End Class

Public Class StaffDashboardSummary
    Public Property TotalProducts As Integer
    Public Property PendingDeliveries As Integer
    Public Property ReturnsCount As Integer
    Public Property TransactionsToday As Integer
End Class

Public Class TransactionRepository

    Public Function GetCashierDashboardSummary(cashierUserId As Integer) As CashierDashboardSummary
        If cashierUserId <= 0 Then
            Return New CashierDashboardSummary()
        End If

        Dim summarySql As String = "
            SELECT
                (SELECT COUNT(1)
                 FROM tbl_SalesTransaction st
                 WHERE st.UserID = @cashierUserId
                   AND CAST(st.SaleDate AS date) = CAST(GETDATE() AS date)) AS TransactionsToday,
                (SELECT ISNULL(SUM(ISNULL(st.TotalItems, 0)), 0)
                 FROM tbl_SalesTransaction st
                 WHERE st.UserID = @cashierUserId
                   AND CAST(st.SaleDate AS date) = CAST(GETDATE() AS date)) AS ItemsSoldToday,
                (SELECT ISNULL(SUM(ISNULL(st.TotalAmount, 0)), 0)
                 FROM tbl_SalesTransaction st
                 WHERE st.UserID = @cashierUserId
                   AND CAST(st.SaleDate AS date) = CAST(GETDATE() AS date)) AS SalesToday,
                (SELECT ISNULL(SUM((ISNULL(sh.SellingPrice, 0) - ISNULL(sh.CostPrice, 0)) * ISNULL(sh.Quantity, 0)), 0)
                 FROM tbl_SalesHistory sh
                 WHERE sh.UserID = @cashierUserId
                   AND CAST(sh.SaleDate AS date) = CAST(GETDATE() AS date)) AS ProfitToday;"

        Dim summaryTable As DataTable = Db.QueryDataTable(
            summarySql,
            New SqlParameter("@cashierUserId", SqlDbType.Int) With {.Value = cashierUserId}
        )

        If summaryTable.Rows.Count = 0 Then
            Return New CashierDashboardSummary()
        End If

        Dim summaryRow As DataRow = summaryTable.Rows(0)
        Return New CashierDashboardSummary() With {
            .TransactionsToday = ToInt32Safe(summaryRow("TransactionsToday")),
            .ItemsSoldToday = ToInt32Safe(summaryRow("ItemsSoldToday")),
            .SalesToday = ToDecimalSafe(summaryRow("SalesToday")),
            .ProfitToday = ToDecimalSafe(summaryRow("ProfitToday"))
        }
    End Function

    Public Function GetRecentCashierTransactions(cashierUserId As Integer, maxRows As Integer) As DataTable
        If cashierUserId <= 0 Then
            Return New DataTable()
        End If

        Dim safeMaxRows As Integer = Math.Max(1, Math.Min(maxRows, 500))
        Dim query As String = "
            SELECT TOP (@maxRows)
                   st.TransactionNo AS [Transaction No],
                   st.SaleDate AS [Sale Date],
                   ISNULL(st.TotalItems, 0) AS [Items],
                   ISNULL(st.TotalAmount, 0) AS [Total Amount]
            FROM tbl_SalesTransaction st
            WHERE st.UserID = @cashierUserId
            ORDER BY st.SaleDate DESC, st.TransactionID DESC;"

        Return Db.QueryDataTable(
            query,
            New SqlParameter("@cashierUserId", SqlDbType.Int) With {.Value = cashierUserId},
            New SqlParameter("@maxRows", SqlDbType.Int) With {.Value = safeMaxRows}
        )
    End Function

    Public Function GetCashierStockMovement(cashierUserId As Integer, maxRows As Integer) As DataTable
        If cashierUserId <= 0 Then
            Return New DataTable()
        End If

        Dim safeMaxRows As Integer = Math.Max(1, Math.Min(maxRows, 500))
        Dim query As String = "
            SELECT TOP (@maxRows)
                   sh.SaleDate AS [Sale Date],
                   sh.TransactionNo AS [Transaction No],
                   sh.ProductName AS [Product],
                   sh.BarcodeNumber AS [Barcode],
                   ISNULL(sh.Quantity, 0) AS [Deducted Qty],
                   ISNULL(stock.CurrentStock, 0) AS [Current Stock]
            FROM tbl_SalesHistory sh
            LEFT JOIN (
                SELECT dp.ProductID,
                       SUM(ISNULL(dp.Quantity, 0)) AS CurrentStock
                FROM tbl_Delivery_Products dp
                WHERE dp.Status = 'Posted'
                GROUP BY dp.ProductID
            ) stock
                ON stock.ProductID = sh.ProductID
            WHERE sh.UserID = @cashierUserId
            ORDER BY sh.SaleDate DESC, sh.SaleID DESC;"

        Return Db.QueryDataTable(
            query,
            New SqlParameter("@cashierUserId", SqlDbType.Int) With {.Value = cashierUserId},
            New SqlParameter("@maxRows", SqlDbType.Int) With {.Value = safeMaxRows}
        )
    End Function

    Public Function GetStaffDashboardSummary() As StaffDashboardSummary
        Dim summarySql As String = "
            SELECT
                (SELECT COUNT(1) FROM tbl_Products WHERE ISNULL(IsActive, 1) = 1) AS TotalProducts,
                (
                    SELECT COUNT(DISTINCT dp.DeliveryID)
                    FROM tbl_Delivery_Products dp
                    WHERE ISNULL(dp.Status, 'Pending') = 'Pending'
                ) AS PendingDeliveries,
                (SELECT COUNT(1) FROM tbl_Supplier_Returns) AS ReturnsCount,
                (
                    SELECT COUNT(1)
                    FROM tbl_SalesTransaction st
                    WHERE CAST(st.SaleDate AS date) = CAST(GETDATE() AS date)
                ) AS TransactionsToday;"

        Dim summaryTable As DataTable = Db.QueryDataTable(summarySql)
        If summaryTable.Rows.Count = 0 Then
            Return New StaffDashboardSummary()
        End If

        Dim row As DataRow = summaryTable.Rows(0)
        Return New StaffDashboardSummary() With {
            .TotalProducts = ToInt32Safe(row("TotalProducts")),
            .PendingDeliveries = ToInt32Safe(row("PendingDeliveries")),
            .ReturnsCount = ToInt32Safe(row("ReturnsCount")),
            .TransactionsToday = ToInt32Safe(row("TransactionsToday"))
        }
    End Function

    Public Function GetLowStockItems(maxRows As Integer, Optional stockThreshold As Integer = 10) As DataTable
        Dim safeMaxRows As Integer = Math.Max(1, Math.Min(maxRows, 500))
        Dim safeThreshold As Integer = Math.Max(0, stockThreshold)

        Dim query As String = "
            SELECT TOP (@maxRows)
                   p.ImagePath AS [ImagePath],
                   p.Product AS [Product],
                   p.BarcodeNumber AS [Barcode],
                   SUM(ISNULL(dp.Quantity, 0)) AS [Stock],
                   MAX(ISNULL(d.OrderNumber, '')) AS [Order No],
                   MAX(ISNULL(dp.DateUpdated, d.DeliveryDate)) AS [Updated]
            FROM tbl_Delivery_Products dp
            INNER JOIN tbl_Products p ON p.ProductID = dp.ProductID
            LEFT JOIN tbl_Deliveries d ON d.DeliveryID = dp.DeliveryID
            WHERE ISNULL(dp.Status, '') = 'Posted'
              AND ISNULL(dp.Quantity, 0) >= 0
              AND ISNULL(p.IsActive, 1) = 1
            GROUP BY p.ProductID, p.ImagePath, p.Product, p.BarcodeNumber
            HAVING SUM(ISNULL(dp.Quantity, 0)) <= @stockThreshold
            ORDER BY SUM(ISNULL(dp.Quantity, 0)) ASC, p.Product ASC;"

        Return Db.QueryDataTable(
            query,
            New SqlParameter("@maxRows", SqlDbType.Int) With {.Value = safeMaxRows},
            New SqlParameter("@stockThreshold", SqlDbType.Int) With {.Value = safeThreshold}
        )
    End Function

    Public Function GetStockMovement(maxRows As Integer) As DataTable
        Dim safeMaxRows As Integer = Math.Max(1, Math.Min(maxRows, 500))

        Dim query As String = "
            SELECT TOP (@maxRows)
                   p.ImagePath AS [ImagePath],
                   sh.SaleDate AS [Sale Date],
                   sh.Name AS [Cashier],
                   sh.TransactionNo AS [Transaction No],
                   sh.ProductName AS [Product],
                   sh.BarcodeNumber AS [Barcode],
                   ISNULL(sh.Quantity, 0) AS [Deducted Qty],
                   ISNULL(stock.CurrentStock, 0) AS [Current Stock]
            FROM tbl_SalesHistory sh
            LEFT JOIN (
                SELECT dp.ProductID,
                       SUM(ISNULL(dp.Quantity, 0)) AS CurrentStock
                FROM tbl_Delivery_Products dp
                WHERE ISNULL(dp.Status, '') = 'Posted'
                GROUP BY dp.ProductID
            ) stock
                ON stock.ProductID = sh.ProductID
            LEFT JOIN tbl_Products p
                ON p.ProductID = sh.ProductID
            ORDER BY sh.SaleDate DESC, sh.SaleID DESC;"

        Return Db.QueryDataTable(
            query,
            New SqlParameter("@maxRows", SqlDbType.Int) With {.Value = safeMaxRows}
        )
    End Function

    Public Function GetSalesHistoryPage(request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        safeRequest.PageSize = Math.Max(1, safeRequest.PageSize)
        safeRequest.PageIndex = Math.Max(1, safeRequest.PageIndex)

        Dim normalizedSearch As String = If(safeRequest.SearchText, String.Empty).Trim()
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(normalizedSearch), "", "%" & normalizedSearch & "%")

        Dim dateFromValue As Object = If(safeRequest.DateFrom.HasValue, CType(safeRequest.DateFrom.Value.Date, Object), DBNull.Value)
        Dim dateToValue As Object = If(safeRequest.DateTo.HasValue, CType(safeRequest.DateTo.Value.Date, Object), DBNull.Value)

        Dim countSql As String = "
            SELECT COUNT(1)
            FROM tbl_SalesHistory
            WHERE (@search = '' OR ProductName LIKE @search OR BarcodeNumber LIKE @search OR TransactionNo LIKE @search OR Name LIKE @search)
              AND (@dateFrom IS NULL OR CAST(SaleDate AS date) >= @dateFrom)
              AND (@dateTo IS NULL OR CAST(SaleDate AS date) <= @dateTo);"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 255) With {.Value = searchValue},
            New SqlParameter("@dateFrom", SqlDbType.Date) With {.Value = dateFromValue},
            New SqlParameter("@dateTo", SqlDbType.Date) With {.Value = dateToValue}
        )

        Dim dataSql As String = "
            SELECT SaleID,
                   Name AS Cashier,
                   ProductName,
                   BarcodeNumber,
                   Quantity,
                   SellingPrice,
                   TotalAmount,
                   Discount,
                   VatRate,
                   VatAmount,
                   Vatable,
                   TransactionNo,
                   SaleDate
            FROM tbl_SalesHistory
            WHERE (@search = '' OR ProductName LIKE @search OR BarcodeNumber LIKE @search OR TransactionNo LIKE @search OR Name LIKE @search)
              AND (@dateFrom IS NULL OR CAST(SaleDate AS date) >= @dateFrom)
              AND (@dateTo IS NULL OR CAST(SaleDate AS date) <= @dateTo)
            ORDER BY SaleDate DESC, SaleID DESC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;"

        Dim rows As DataTable = Db.QueryDataTable(
            dataSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 255) With {.Value = searchValue},
            New SqlParameter("@dateFrom", SqlDbType.Date) With {.Value = dateFromValue},
            New SqlParameter("@dateTo", SqlDbType.Date) With {.Value = dateToValue},
            New SqlParameter("@offset", SqlDbType.Int) With {.Value = safeRequest.Offset},
            New SqlParameter("@pageSize", SqlDbType.Int) With {.Value = safeRequest.PageSize}
        )

        Return PagedQueryResult.Create(rows, totalRecords, safeRequest)
    End Function

    Private Shared Function ToInt32Safe(value As Object) As Integer
        If value Is Nothing OrElse value Is DBNull.Value Then
            Return 0
        End If

        Dim parsedInteger As Integer
        If Integer.TryParse(value.ToString(), parsedInteger) Then
            Return parsedInteger
        End If

        Dim parsedDecimal As Decimal
        If Decimal.TryParse(value.ToString(), parsedDecimal) Then
            Return CInt(Math.Truncate(parsedDecimal))
        End If

        Return 0
    End Function

    Private Shared Function ToDecimalSafe(value As Object) As Decimal
        If value Is Nothing OrElse value Is DBNull.Value Then
            Return 0D
        End If

        Dim parsedDecimal As Decimal
        If Decimal.TryParse(value.ToString(), parsedDecimal) Then
            Return parsedDecimal
        End If

        Return 0D
    End Function
End Class
