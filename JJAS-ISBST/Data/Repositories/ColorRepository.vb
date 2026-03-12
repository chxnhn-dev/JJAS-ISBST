Imports System.Data
Imports System.Data.SqlClient

Public Class ColorRepository

    Public Function GetActiveColors(searchText As String) As DataTable
        Dim sql As String = "
            SELECT ColorID,
                   Color
            FROM tbl_Color
            WHERE IsActive = 1
              AND (@search = '' OR Color LIKE @search)
            ORDER BY Color;"

        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")
        Return Db.QueryDataTable(sql, New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue})
    End Function

    Public Function GetActiveColorsPage(request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        safeRequest.PageSize = Math.Max(1, safeRequest.PageSize)
        safeRequest.PageIndex = Math.Max(1, safeRequest.PageIndex)

        Dim normalizedSearch As String = If(safeRequest.SearchText, String.Empty).Trim()
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(normalizedSearch), "", "%" & normalizedSearch & "%")

        Dim countSql As String = "
            SELECT COUNT(1)
            FROM tbl_Color
            WHERE IsActive = 1
              AND (@search = '' OR Color LIKE @search);"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue}
        )

        Dim dataSql As String = "
            SELECT ColorID,
                   Color
            FROM tbl_Color
            WHERE IsActive = 1
              AND (@search = '' OR Color LIKE @search)
            ORDER BY Color ASC, ColorID ASC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;"

        Dim rows As DataTable = Db.QueryDataTable(
            dataSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue},
            New SqlParameter("@offset", SqlDbType.Int) With {.Value = safeRequest.Offset},
            New SqlParameter("@pageSize", SqlDbType.Int) With {.Value = safeRequest.PageSize}
        )

        Return PagedQueryResult.Create(rows, totalRecords, safeRequest)
    End Function

    Public Function GetColorById(colorId As Integer) As DataRow
        Dim sql As String = "
            SELECT ColorID,
                   Color
            FROM tbl_Color
            WHERE ColorID = @ColorID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@ColorID", SqlDbType.Int) With {.Value = colorId})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Public Function ExistsByName(colorName As String, Optional excludeColorId As Integer? = Nothing) As Boolean
        Dim sql As String = "
            SELECT COUNT(1)
            FROM tbl_Color
            WHERE LOWER(Color) = LOWER(@Color);"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@Color", SqlDbType.NVarChar, 150) With {.Value = colorName.Trim()}
        }

        If excludeColorId.HasValue Then
            sql = "
                SELECT COUNT(1)
                FROM tbl_Color
                WHERE LOWER(Color) = LOWER(@Color)
                  AND ColorID <> @ColorID;"
            parameters.Add(New SqlParameter("@ColorID", SqlDbType.Int) With {.Value = excludeColorId.Value})
        End If

        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, parameters.ToArray())
        Return count > 0
    End Function

    Public Function InsertColor(colorName As String, createdAt As DateTime) As Integer
        Dim sql As String = "
            INSERT INTO tbl_Color (Color, DateCreated)
            VALUES (@Color, @DateCreated);"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@Color", SqlDbType.NVarChar, 150) With {.Value = colorName.Trim()},
            New SqlParameter("@DateCreated", SqlDbType.DateTime) With {.Value = createdAt})
    End Function

    Public Function UpdateColor(colorId As Integer, colorName As String, updatedAt As DateTime) As Integer
        Dim sql As String = "
            UPDATE tbl_Color
               SET Color = @Color,
                   DateCreated = @DateCreated
             WHERE ColorID = @ColorID;"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@Color", SqlDbType.NVarChar, 150) With {.Value = colorName.Trim()},
            New SqlParameter("@DateCreated", SqlDbType.DateTime) With {.Value = updatedAt},
            New SqlParameter("@ColorID", SqlDbType.Int) With {.Value = colorId})
    End Function

    Public Function DeleteColor(colorId As Integer) As Integer
        Dim sql As String = "DELETE tbl_Color WHERE ColorID = @ColorID;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@ColorID", SqlDbType.Int) With {.Value = colorId})
    End Function

    Public Function IsUsedByProducts(colorId As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(1) FROM tbl_Products WHERE ColorID = @ColorID;"
        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, New SqlParameter("@ColorID", SqlDbType.Int) With {.Value = colorId})
        Return count > 0
    End Function

End Class
