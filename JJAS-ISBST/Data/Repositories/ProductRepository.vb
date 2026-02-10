Imports System.Data
Imports System.Data.SqlClient

Public Class ProductRepository

    Public Function GetActiveProducts(searchText As String) As DataTable
        Dim sql As String = "
            SELECT p.ProductID,
                   p.BarcodeNumber,
                   p.Product,
                   c.Category,
                   s.Size,
                   b.Brand,
                   col.Color,
                   p.SellingPrice,
                   p.Description,
                   p.ImagePath,
                   b.BrandID,
                   c.CategoryID,
                   col.ColorID,
                   s.SizeID
            FROM tbl_Products p
            INNER JOIN tbl_Brand b ON p.BrandID = b.BrandID
            INNER JOIN tbl_Category c ON p.CategoryID = c.CategoryID
            INNER JOIN tbl_Color col ON p.ColorID = col.ColorID
            INNER JOIN tbl_Size s ON p.SizeID = s.SizeID
            WHERE p.IsActive = 1
            AND (@search = '' OR p.BarcodeNumber LIKE @search OR p.Product LIKE @search)
            ORDER BY p.DateCreated DESC;"

        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")
        Return Db.QueryDataTable(sql, New SqlParameter("@search", SqlDbType.NVarChar, 255) With {.Value = searchValue})
    End Function

    Public Function SoftDeleteProduct(productId As Integer) As Integer
        Dim sql As String = "UPDATE tbl_Products SET IsActive = 0 WHERE ProductID = @id;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@id", SqlDbType.Int) With {.Value = productId})
    End Function

    Public Function RestoreProduct(productId As Integer) As Integer
        Dim sql As String = "UPDATE tbl_Products SET IsActive = 1 WHERE ProductID = @id;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@id", SqlDbType.Int) With {.Value = productId})
    End Function

End Class
