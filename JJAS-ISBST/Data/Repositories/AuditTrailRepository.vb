Imports System.Data
Imports System.Data.SqlClient

Public Class AuditTrailRepository

    Public Function GetAuditTrailPage(request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        safeRequest.PageSize = Math.Max(1, safeRequest.PageSize)
        safeRequest.PageIndex = Math.Max(1, safeRequest.PageIndex)

        Dim normalizedSearch As String = If(safeRequest.SearchText, String.Empty).Trim()
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(normalizedSearch), "", "%" & normalizedSearch & "%")

        Dim dateFromValue As Object = If(safeRequest.DateFrom.HasValue, CType(safeRequest.DateFrom.Value.Date, Object), DBNull.Value)
        Dim dateToValue As Object = If(safeRequest.DateTo.HasValue, CType(safeRequest.DateTo.Value.Date, Object), DBNull.Value)

        Dim countSql As String = "
            SELECT COUNT(1)
            FROM tbl_AuditTrail
            WHERE (@search = '' OR Name LIKE @search OR Username LIKE @search OR Role LIKE @search OR Action LIKE @search OR CONVERT(nvarchar(30), DateTimeStamp, 120) LIKE @search)
              AND (@dateFrom IS NULL OR CAST(DateTimeStamp AS date) >= @dateFrom)
              AND (@dateTo IS NULL OR CAST(DateTimeStamp AS date) <= @dateTo);"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 255) With {.Value = searchValue},
            New SqlParameter("@dateFrom", SqlDbType.Date) With {.Value = dateFromValue},
            New SqlParameter("@dateTo", SqlDbType.Date) With {.Value = dateToValue}
        )

        Dim dataSql As String = "
            SELECT AuditID,
                   UserID,
                   Name,
                   Username,
                   Role,
                   Action,
                   DatetimeStamp
            FROM tbl_AuditTrail
            WHERE (@search = '' OR Name LIKE @search OR Username LIKE @search OR Role LIKE @search OR Action LIKE @search OR CONVERT(nvarchar(30), DateTimeStamp, 120) LIKE @search)
              AND (@dateFrom IS NULL OR CAST(DateTimeStamp AS date) >= @dateFrom)
              AND (@dateTo IS NULL OR CAST(DateTimeStamp AS date) <= @dateTo)
            ORDER BY DateTimeStamp DESC, AuditID DESC
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
