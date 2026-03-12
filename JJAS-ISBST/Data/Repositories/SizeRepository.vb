Imports System.Data
Imports System.Data.SqlClient

Public Class SizeRepository

    Public Function GetActiveSizes(searchText As String) As DataTable
        Dim sql As String = "
            SELECT s.SizeID,
                   s.Size,
                   c.Category,
                   s.Description,
                   s.IsActive
            FROM dbo.tbl_Size s
            INNER JOIN dbo.tbl_Category c
                ON s.CategoryID = c.CategoryID
            WHERE s.IsActive = 1
              AND (@search = '' OR s.Size LIKE @search)
            ORDER BY c.Category, s.Size;"

        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")
        Return Db.QueryDataTable(sql, New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue})
    End Function

    Public Function GetActiveSizesPage(request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        safeRequest.PageSize = Math.Max(1, safeRequest.PageSize)
        safeRequest.PageIndex = Math.Max(1, safeRequest.PageIndex)

        Dim normalizedSearch As String = If(safeRequest.SearchText, String.Empty).Trim()
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(normalizedSearch), "", "%" & normalizedSearch & "%")

        Dim countSql As String = "
            SELECT COUNT(1)
            FROM dbo.tbl_Size s
            INNER JOIN dbo.tbl_Category c
                ON s.CategoryID = c.CategoryID
            WHERE s.IsActive = 1
              AND (@search = '' OR s.Size LIKE @search);"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue}
        )

        Dim dataSql As String = "
            SELECT s.SizeID,
                   s.Size,
                   c.Category,
                   s.Description,
                   s.IsActive
            FROM dbo.tbl_Size s
            INNER JOIN dbo.tbl_Category c
                ON s.CategoryID = c.CategoryID
            WHERE s.IsActive = 1
              AND (@search = '' OR s.Size LIKE @search)
            ORDER BY c.Category ASC, s.Size ASC, s.SizeID ASC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;"

        Dim rows As DataTable = Db.QueryDataTable(
            dataSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue},
            New SqlParameter("@offset", SqlDbType.Int) With {.Value = safeRequest.Offset},
            New SqlParameter("@pageSize", SqlDbType.Int) With {.Value = safeRequest.PageSize}
        )

        Return PagedQueryResult.Create(rows, totalRecords, safeRequest)
    End Function

    Public Function GetSizeById(sizeId As Integer) As DataRow
        Dim sql As String = "
            SELECT SizeID,
                   CategoryID,
                   Size,
                   Description
            FROM tbl_Size
            WHERE SizeID = @SizeID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@SizeID", SqlDbType.Int) With {.Value = sizeId})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Public Function ExistsByName(categoryId As Integer, sizeName As String, Optional excludeSizeId As Integer? = Nothing) As Boolean
        Dim sql As String = "
            SELECT COUNT(1)
            FROM tbl_Size
            WHERE CategoryID = @CategoryID
              AND LOWER(Size) = LOWER(@Size);"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@CategoryID", SqlDbType.Int) With {.Value = categoryId},
            New SqlParameter("@Size", SqlDbType.NVarChar, 150) With {.Value = sizeName.Trim()}
        }

        If excludeSizeId.HasValue Then
            sql = "
                SELECT COUNT(1)
                FROM tbl_Size
                WHERE CategoryID = @CategoryID
                  AND LOWER(Size) = LOWER(@Size)
                  AND SizeID <> @SizeID;"
            parameters.Add(New SqlParameter("@SizeID", SqlDbType.Int) With {.Value = excludeSizeId.Value})
        End If

        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, parameters.ToArray())
        Return count > 0
    End Function

    Public Function InsertSize(categoryId As Integer, sizeName As String, description As String, createdAt As DateTime) As Integer
        Dim sql As String = "
            INSERT INTO tbl_Size (CategoryID, Size, Description, DateCreated)
            VALUES (@CategoryID, @Size, @Description, @DateCreated);"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@CategoryID", SqlDbType.Int) With {.Value = categoryId},
            New SqlParameter("@Size", SqlDbType.NVarChar, 150) With {.Value = sizeName.Trim()},
            New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = If(description, String.Empty).Trim()},
            New SqlParameter("@DateCreated", SqlDbType.DateTime) With {.Value = createdAt})
    End Function

    Public Function UpdateSize(sizeId As Integer, categoryId As Integer, sizeName As String, description As String, updatedAt As DateTime) As Integer
        Dim sql As String = "
            UPDATE tbl_Size
               SET CategoryID = @CategoryID,
                   Size = @Size,
                   Description = @Description,
                   DateCreated = @DateCreated
             WHERE SizeID = @SizeID;"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@CategoryID", SqlDbType.Int) With {.Value = categoryId},
            New SqlParameter("@Size", SqlDbType.NVarChar, 150) With {.Value = sizeName.Trim()},
            New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = If(description, String.Empty).Trim()},
            New SqlParameter("@DateCreated", SqlDbType.DateTime) With {.Value = updatedAt},
            New SqlParameter("@SizeID", SqlDbType.Int) With {.Value = sizeId})
    End Function

    Public Function DeleteSize(sizeId As Integer) As Integer
        Dim sql As String = "DELETE tbl_Size WHERE SizeID = @SizeID;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@SizeID", SqlDbType.Int) With {.Value = sizeId})
    End Function

    Public Function IsUsedByProducts(sizeId As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(1) FROM tbl_Products WHERE SizeID = @SizeID;"
        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, New SqlParameter("@SizeID", SqlDbType.Int) With {.Value = sizeId})
        Return count > 0
    End Function

End Class
