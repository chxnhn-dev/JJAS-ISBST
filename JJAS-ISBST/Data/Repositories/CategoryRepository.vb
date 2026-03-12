Imports System.Data
Imports System.Data.SqlClient

Public Class CategoryRepository

    Public Function GetActiveCategories(searchText As String) As DataTable
        Dim sql As String = "
            SELECT CategoryID,
                   Category
            FROM tbl_Category
            WHERE IsActive = 1
              AND (@search = '' OR Category LIKE @search)
            ORDER BY Category;"

        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")
        Return Db.QueryDataTable(sql, New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue})
    End Function

    Public Function GetActiveCategoriesPage(request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        safeRequest.PageSize = Math.Max(1, safeRequest.PageSize)
        safeRequest.PageIndex = Math.Max(1, safeRequest.PageIndex)

        Dim normalizedSearch As String = If(safeRequest.SearchText, String.Empty).Trim()
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(normalizedSearch), "", "%" & normalizedSearch & "%")

        Dim countSql As String = "
            SELECT COUNT(1)
            FROM tbl_Category
            WHERE IsActive = 1
              AND (@search = '' OR Category LIKE @search);"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue}
        )

        Dim dataSql As String = "
            SELECT CategoryID,
                   Category
            FROM tbl_Category
            WHERE IsActive = 1
              AND (@search = '' OR Category LIKE @search)
            ORDER BY Category ASC, CategoryID ASC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;"

        Dim rows As DataTable = Db.QueryDataTable(
            dataSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue},
            New SqlParameter("@offset", SqlDbType.Int) With {.Value = safeRequest.Offset},
            New SqlParameter("@pageSize", SqlDbType.Int) With {.Value = safeRequest.PageSize}
        )

        Return PagedQueryResult.Create(rows, totalRecords, safeRequest)
    End Function

    Public Function GetCategoryById(categoryId As Integer) As DataRow
        Dim sql As String = "
            SELECT CategoryID,
                   Category
            FROM tbl_Category
            WHERE CategoryID = @CategoryID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@CategoryID", SqlDbType.Int) With {.Value = categoryId})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Public Function ExistsByName(categoryName As String, Optional excludeCategoryId As Integer? = Nothing) As Boolean
        Dim sql As String = "
            SELECT COUNT(1)
            FROM tbl_Category
            WHERE LOWER(Category) = LOWER(@Category);"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@Category", SqlDbType.NVarChar, 150) With {.Value = categoryName.Trim()}
        }

        If excludeCategoryId.HasValue Then
            sql = "
                SELECT COUNT(1)
                FROM tbl_Category
                WHERE LOWER(Category) = LOWER(@Category)
                  AND CategoryID <> @CategoryID;"
            parameters.Add(New SqlParameter("@CategoryID", SqlDbType.Int) With {.Value = excludeCategoryId.Value})
        End If

        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, parameters.ToArray())
        Return count > 0
    End Function

    Public Function InsertCategory(categoryName As String, createdAt As DateTime) As Integer
        Dim sql As String = "
            INSERT INTO tbl_Category (Category, DateCreated)
            VALUES (@Category, @DateCreated);"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@Category", SqlDbType.NVarChar, 150) With {.Value = categoryName.Trim()},
            New SqlParameter("@DateCreated", SqlDbType.DateTime) With {.Value = createdAt})
    End Function

    Public Function UpdateCategory(categoryId As Integer, categoryName As String, updatedAt As DateTime) As Integer
        Dim sql As String = "
            UPDATE tbl_Category
               SET Category = @Category,
                   DateCreated = @DateCreated
             WHERE CategoryID = @CategoryID;"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@Category", SqlDbType.NVarChar, 150) With {.Value = categoryName.Trim()},
            New SqlParameter("@DateCreated", SqlDbType.DateTime) With {.Value = updatedAt},
            New SqlParameter("@CategoryID", SqlDbType.Int) With {.Value = categoryId})
    End Function

    Public Function DeleteCategory(categoryId As Integer) As Integer
        Dim sql As String = "DELETE tbl_Category WHERE CategoryID = @CategoryID;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@CategoryID", SqlDbType.Int) With {.Value = categoryId})
    End Function

    Public Function IsUsedBySizes(categoryId As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(1) FROM tbl_Size WHERE CategoryID = @CategoryID;"
        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, New SqlParameter("@CategoryID", SqlDbType.Int) With {.Value = categoryId})
        Return count > 0
    End Function

    Public Function IsUsedByProducts(categoryId As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(1) FROM tbl_Products WHERE CategoryID = @CategoryID;"
        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, New SqlParameter("@CategoryID", SqlDbType.Int) With {.Value = categoryId})
        Return count > 0
    End Function

End Class
