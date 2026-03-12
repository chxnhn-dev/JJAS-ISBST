Imports System.Data
Imports System.Data.SqlClient

Public Class InventoryRepository

    Public Function GetPostedInventoryPage(request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        safeRequest.PageSize = Math.Max(1, safeRequest.PageSize)
        safeRequest.PageIndex = Math.Max(1, safeRequest.PageIndex)

        Dim normalizedSearch As String = If(safeRequest.SearchText, String.Empty).Trim()
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(normalizedSearch), "", "%" & normalizedSearch & "%")

        Dim dateFromValue As Object = If(safeRequest.DateFrom.HasValue, CType(safeRequest.DateFrom.Value.Date, Object), DBNull.Value)
        Dim dateToValue As Object = If(safeRequest.DateTo.HasValue, CType(safeRequest.DateTo.Value.Date, Object), DBNull.Value)

        Dim countSql As String = "
            SELECT COUNT(1)
            FROM tbl_delivery_products dp
            INNER JOIN tbl_deliveries d ON dp.DeliveryID = d.DeliveryID
            INNER JOIN tbl_supplier s ON d.supplierid = s.supplierid
            INNER JOIN tbl_products p ON dp.ProductID = p.ProductID
            WHERE dp.Status = 'Posted'
              AND dp.Quantity >= 0
              AND (@search = '' OR p.BarcodeNumber LIKE @search OR p.Product LIKE @search OR d.OrderNumber LIKE @search)
              AND (@dateFrom IS NULL OR CAST(dp.DateUpdated AS date) >= @dateFrom)
              AND (@dateTo IS NULL OR CAST(dp.DateUpdated AS date) <= @dateTo);"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 255) With {.Value = searchValue},
            New SqlParameter("@dateFrom", SqlDbType.Date) With {.Value = dateFromValue},
            New SqlParameter("@dateTo", SqlDbType.Date) With {.Value = dateToValue}
        )

        Dim dataSql As String = "
            SELECT dp.DeliveryProductID,
                   d.DeliveryID,
                   d.OrderNumber,
                   p.BarcodeNumber,
                   p.Product AS ProductName,
                   dp.Quantity,
                   p.ImagePath,
                   dp.DateUpdated
            FROM tbl_delivery_products dp
            INNER JOIN tbl_deliveries d ON dp.DeliveryID = d.DeliveryID
            INNER JOIN tbl_supplier s ON d.supplierid = s.supplierid
            INNER JOIN tbl_products p ON dp.ProductID = p.ProductID
            WHERE dp.Status = 'Posted'
              AND dp.Quantity >= 0
              AND (@search = '' OR p.BarcodeNumber LIKE @search OR p.Product LIKE @search OR d.OrderNumber LIKE @search)
              AND (@dateFrom IS NULL OR CAST(dp.DateUpdated AS date) >= @dateFrom)
              AND (@dateTo IS NULL OR CAST(dp.DateUpdated AS date) <= @dateTo)
            ORDER BY d.DateCreated DESC, dp.DeliveryProductID DESC
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
End Class
