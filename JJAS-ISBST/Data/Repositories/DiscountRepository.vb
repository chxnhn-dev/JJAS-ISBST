Imports System.Data
Imports System.Data.SqlClient

Public Class DiscountRepository

    Public Function GetActiveDiscounts(searchText As String) As DataTable
        Dim sql As String = "
            SELECT DiscountID,
                   DiscountName,
                   DiscountValue,
                   Description
            FROM tbl_Discount
            WHERE IsActive = 1
              AND (@search = '' OR DiscountName LIKE @search)
            ORDER BY DiscountName;"

        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")
        Return Db.QueryDataTable(sql, New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue})
    End Function

    Public Function GetActiveDiscountsPage(request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        safeRequest.PageSize = Math.Max(1, safeRequest.PageSize)
        safeRequest.PageIndex = Math.Max(1, safeRequest.PageIndex)

        Dim normalizedSearch As String = If(safeRequest.SearchText, String.Empty).Trim()
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(normalizedSearch), "", "%" & normalizedSearch & "%")

        Dim countSql As String = "
            SELECT COUNT(1)
            FROM tbl_Discount
            WHERE IsActive = 1
              AND (@search = '' OR DiscountName LIKE @search);"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue}
        )

        Dim dataSql As String = "
            SELECT DiscountID,
                   DiscountName,
                   DiscountValue,
                   Description
            FROM tbl_Discount
            WHERE IsActive = 1
              AND (@search = '' OR DiscountName LIKE @search)
            ORDER BY DiscountName ASC, DiscountID ASC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;"

        Dim rows As DataTable = Db.QueryDataTable(
            dataSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue},
            New SqlParameter("@offset", SqlDbType.Int) With {.Value = safeRequest.Offset},
            New SqlParameter("@pageSize", SqlDbType.Int) With {.Value = safeRequest.PageSize}
        )

        Return PagedQueryResult.Create(rows, totalRecords, safeRequest)
    End Function

    Public Function GetDiscountById(discountId As Integer) As DataRow
        Dim sql As String = "
            SELECT DiscountID,
                   DiscountName,
                   DiscountValue,
                   Description
            FROM tbl_Discount
            WHERE DiscountID = @DiscountID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@DiscountID", SqlDbType.Int) With {.Value = discountId})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Public Function ExistsByName(discountName As String, Optional excludeDiscountId As Integer? = Nothing) As Boolean
        Dim sql As String = "
            SELECT COUNT(1)
            FROM tbl_Discount
            WHERE LOWER(DiscountName) = LOWER(@DiscountName);"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@DiscountName", SqlDbType.NVarChar, 150) With {.Value = discountName.Trim()}
        }

        If excludeDiscountId.HasValue Then
            sql = "
                SELECT COUNT(1)
                FROM tbl_Discount
                WHERE LOWER(DiscountName) = LOWER(@DiscountName)
                  AND DiscountID <> @DiscountID;"
            parameters.Add(New SqlParameter("@DiscountID", SqlDbType.Int) With {.Value = excludeDiscountId.Value})
        End If

        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, parameters.ToArray())
        Return count > 0
    End Function

    Public Function InsertDiscount(discountName As String, discountValue As Decimal, description As String, updatedAt As DateTime) As Integer
        Dim sql As String = "
            INSERT INTO tbl_Discount (DiscountName, DiscountValue, Description, DateUpdated)
            VALUES (@DiscountName, @DiscountValue, @Description, @DateUpdated);"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@DiscountName", SqlDbType.NVarChar, 150) With {.Value = discountName.Trim()},
            New SqlParameter("@DiscountValue", SqlDbType.Decimal) With {.Value = discountValue},
            New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = If(description, String.Empty).Trim()},
            New SqlParameter("@DateUpdated", SqlDbType.DateTime) With {.Value = updatedAt})
    End Function

    Public Function UpdateDiscount(discountId As Integer, discountName As String, discountValue As Decimal, description As String, updatedAt As DateTime) As Integer
        Dim sql As String = "
            UPDATE tbl_Discount
               SET DiscountName = @DiscountName,
                   DiscountValue = @DiscountValue,
                   Description = @Description,
                   DateUpdated = @DateUpdated
             WHERE DiscountID = @DiscountID;"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@DiscountName", SqlDbType.NVarChar, 150) With {.Value = discountName.Trim()},
            New SqlParameter("@DiscountValue", SqlDbType.Decimal) With {.Value = discountValue},
            New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = If(description, String.Empty).Trim()},
            New SqlParameter("@DateUpdated", SqlDbType.DateTime) With {.Value = updatedAt},
            New SqlParameter("@DiscountID", SqlDbType.Int) With {.Value = discountId})
    End Function

    Public Function DeleteDiscount(discountId As Integer) As Integer
        Dim sql As String = "DELETE tbl_Discount WHERE DiscountID = @DiscountID;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@DiscountID", SqlDbType.Int) With {.Value = discountId})
    End Function

End Class
