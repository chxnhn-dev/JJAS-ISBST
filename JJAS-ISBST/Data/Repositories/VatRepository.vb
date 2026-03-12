Imports System.Data
Imports System.Data.SqlClient

Public Class VatRepository

    Public Function GetVatPage(request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        safeRequest.PageSize = Math.Max(1, safeRequest.PageSize)
        safeRequest.PageIndex = Math.Max(1, safeRequest.PageIndex)

        Dim normalizedSearch As String = If(safeRequest.SearchText, String.Empty).Trim()
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(normalizedSearch), "", "%" & normalizedSearch & "%")

        Dim countSql As String = "
            SELECT COUNT(1)
            FROM tbl_Vat
            WHERE (@search = '' OR CONVERT(varchar(20), Vat_Rate) LIKE @search);"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 100) With {.Value = searchValue}
        )

        Dim dataSql As String = "
            SELECT VatID,
                   Vat_Rate,
                   DateUpdated
            FROM tbl_Vat
            WHERE (@search = '' OR CONVERT(varchar(20), Vat_Rate) LIKE @search)
            ORDER BY DateUpdated DESC, VatID DESC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;"

        Dim rows As DataTable = Db.QueryDataTable(
            dataSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 100) With {.Value = searchValue},
            New SqlParameter("@offset", SqlDbType.Int) With {.Value = safeRequest.Offset},
            New SqlParameter("@pageSize", SqlDbType.Int) With {.Value = safeRequest.PageSize}
        )

        Return PagedQueryResult.Create(rows, totalRecords, safeRequest)
    End Function
End Class
