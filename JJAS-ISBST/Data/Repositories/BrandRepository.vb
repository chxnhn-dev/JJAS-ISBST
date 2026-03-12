Imports System.Data
Imports System.Data.SqlClient

Public Class BrandRepository

    Public Function GetActiveBrands(searchText As String) As DataTable
        Dim sql As String = "
            SELECT BrandID,
                   Brand
            FROM tbl_Brand
            WHERE IsActive = 1
              AND (@search = '' OR Brand LIKE @search)
            ORDER BY Brand;"

        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")
        Return Db.QueryDataTable(sql, New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue})
    End Function

    Public Function GetActiveBrandsPage(request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        safeRequest.PageSize = Math.Max(1, safeRequest.PageSize)
        safeRequest.PageIndex = Math.Max(1, safeRequest.PageIndex)

        Dim normalizedSearch As String = If(safeRequest.SearchText, String.Empty).Trim()
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(normalizedSearch), "", "%" & normalizedSearch & "%")

        Dim countSql As String = "
            SELECT COUNT(1)
            FROM tbl_Brand
            WHERE IsActive = 1
              AND (@search = '' OR Brand LIKE @search);"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue}
        )

        Dim dataSql As String = "
            SELECT BrandID,
                   Brand
            FROM tbl_Brand
            WHERE IsActive = 1
              AND (@search = '' OR Brand LIKE @search)
            ORDER BY Brand ASC, BrandID ASC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;"

        Dim rows As DataTable = Db.QueryDataTable(
            dataSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue},
            New SqlParameter("@offset", SqlDbType.Int) With {.Value = safeRequest.Offset},
            New SqlParameter("@pageSize", SqlDbType.Int) With {.Value = safeRequest.PageSize}
        )

        Return PagedQueryResult.Create(rows, totalRecords, safeRequest)
    End Function

    Public Function GetBrandById(brandId As Integer) As DataRow
        Dim sql As String = "
            SELECT BrandID,
                   Brand
            FROM tbl_Brand
            WHERE BrandID = @BrandID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@BrandID", SqlDbType.Int) With {.Value = brandId})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Public Function ExistsByName(brandName As String, Optional excludeBrandId As Integer? = Nothing) As Boolean
        Dim sql As String = "
            SELECT COUNT(1)
            FROM tbl_Brand
            WHERE LOWER(Brand) = LOWER(@Brand);"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@Brand", SqlDbType.NVarChar, 150) With {.Value = brandName.Trim()}
        }

        If excludeBrandId.HasValue Then
            sql = "
                SELECT COUNT(1)
                FROM tbl_Brand
                WHERE LOWER(Brand) = LOWER(@Brand)
                  AND BrandID <> @BrandID;"
            parameters.Add(New SqlParameter("@BrandID", SqlDbType.Int) With {.Value = excludeBrandId.Value})
        End If

        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, parameters.ToArray())
        Return count > 0
    End Function

    Public Function InsertBrand(brandName As String, createdAt As DateTime) As Integer
        Dim sql As String = "
            INSERT INTO tbl_Brand (Brand, DateCreated)
            VALUES (@Brand, @DateCreated);"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@Brand", SqlDbType.NVarChar, 150) With {.Value = brandName.Trim()},
            New SqlParameter("@DateCreated", SqlDbType.DateTime) With {.Value = createdAt})
    End Function

    Public Function UpdateBrand(brandId As Integer, brandName As String, updatedAt As DateTime) As Integer
        Dim sql As String = "
            UPDATE tbl_Brand
               SET Brand = @Brand,
                   DateCreated = @DateCreated
             WHERE BrandID = @BrandID;"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@Brand", SqlDbType.NVarChar, 150) With {.Value = brandName.Trim()},
            New SqlParameter("@DateCreated", SqlDbType.DateTime) With {.Value = updatedAt},
            New SqlParameter("@BrandID", SqlDbType.Int) With {.Value = brandId})
    End Function

    Public Function DeleteBrand(brandId As Integer) As Integer
        Dim sql As String = "DELETE tbl_Brand WHERE BrandID = @BrandID;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@BrandID", SqlDbType.Int) With {.Value = brandId})
    End Function

    Public Function IsUsedByProducts(brandId As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(1) FROM tbl_Products WHERE BrandID = @BrandID;"
        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, New SqlParameter("@BrandID", SqlDbType.Int) With {.Value = brandId})
        Return count > 0
    End Function

End Class
