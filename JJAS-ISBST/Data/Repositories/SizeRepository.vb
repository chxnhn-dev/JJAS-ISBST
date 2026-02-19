Imports System.Data
Imports System.Data.SqlClient

Public Class SizeRepository

    Public Function GetActiveSizes(searchText As String) As DataTable
        Dim sql As String = "
            SELECT SizeID,
                   Size,
                   Description
            FROM tbl_Size
            WHERE IsActive = 1
              AND (@search = '' OR Size LIKE @search)
            ORDER BY Size;"

        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")
        Return Db.QueryDataTable(sql, New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue})
    End Function

    Public Function GetSizeById(sizeId As Integer) As DataRow
        Dim sql As String = "
            SELECT SizeID,
                   Size,
                   Description
            FROM tbl_Size
            WHERE SizeID = @SizeID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@SizeID", SqlDbType.Int) With {.Value = sizeId})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Public Function ExistsByName(sizeName As String, Optional excludeSizeId As Integer? = Nothing) As Boolean
        Dim sql As String = "
            SELECT COUNT(1)
            FROM tbl_Size
            WHERE LOWER(Size) = LOWER(@Size);"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@Size", SqlDbType.NVarChar, 150) With {.Value = sizeName.Trim()}
        }

        If excludeSizeId.HasValue Then
            sql = "
                SELECT COUNT(1)
                FROM tbl_Size
                WHERE LOWER(Size) = LOWER(@Size)
                  AND SizeID <> @SizeID;"
            parameters.Add(New SqlParameter("@SizeID", SqlDbType.Int) With {.Value = excludeSizeId.Value})
        End If

        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, parameters.ToArray())
        Return count > 0
    End Function

    Public Function InsertSize(sizeName As String, description As String, createdAt As DateTime) As Integer
        Dim sql As String = "
            INSERT INTO tbl_Size (Size, Description, DateCreated)
            VALUES (@Size, @Description, @DateCreated);"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@Size", SqlDbType.NVarChar, 150) With {.Value = sizeName.Trim()},
            New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = If(description, String.Empty).Trim()},
            New SqlParameter("@DateCreated", SqlDbType.DateTime) With {.Value = createdAt})
    End Function

    Public Function UpdateSize(sizeId As Integer, sizeName As String, description As String, updatedAt As DateTime) As Integer
        Dim sql As String = "
            UPDATE tbl_Size
               SET Size = @Size,
                   Description = @Description,
                   DateCreated = @DateCreated
             WHERE SizeID = @SizeID;"

        Return Db.ExecuteNonQuery(sql,
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
