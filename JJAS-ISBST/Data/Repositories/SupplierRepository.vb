Imports System.Data
Imports System.Data.SqlClient

Public Class SupplierRepository

    Public Function GetActiveSuppliers(searchText As String) As DataTable
        Dim sql As String = "
            SELECT SupplierID,
                   Company,
                   ContactNumber,
                   Address
            FROM tbl_supplier
            WHERE isactive = 1
              AND (@search = '' OR Company LIKE @search)
            ORDER BY SupplierID DESC;"

        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")
        Return Db.QueryDataTable(sql, New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue})
    End Function

    Public Function GetSupplierLookup() As DataTable
        Dim sql As String = "
            SELECT SupplierID,
                   Company
            FROM tbl_supplier
            WHERE isactive = 1
            ORDER BY Company;"
        Return Db.QueryDataTable(sql)
    End Function

    Public Function GetSupplierById(supplierId As Integer) As DataRow
        Dim sql As String = "
            SELECT SupplierID,
                   Company,
                   ContactNumber,
                   Address
            FROM tbl_supplier
            WHERE SupplierID = @SupplierID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@SupplierID", SqlDbType.Int) With {.Value = supplierId})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Public Function CompanyExists(companyName As String, Optional excludeSupplierId As Integer? = Nothing) As Boolean
        Dim sql As String = "
            SELECT COUNT(1)
            FROM tbl_supplier
            WHERE LOWER(Company) = LOWER(@Company);"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@Company", SqlDbType.NVarChar, 200) With {.Value = companyName.Trim()}
        }

        If excludeSupplierId.HasValue Then
            sql = "
                SELECT COUNT(1)
                FROM tbl_supplier
                WHERE LOWER(Company) = LOWER(@Company)
                  AND SupplierID <> @SupplierID;"
            parameters.Add(New SqlParameter("@SupplierID", SqlDbType.Int) With {.Value = excludeSupplierId.Value})
        End If

        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, parameters.ToArray())
        Return count > 0
    End Function

    Public Function InsertSupplier(company As String, contactNumber As String, address As String) As Integer
        Dim sql As String = "
            INSERT INTO tbl_supplier (Company, ContactNumber, Address, isactive, expiredate, dateCreated)
            VALUES (@Company, @ContactNumber, @Address, 1, NULL, GETDATE());"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@Company", SqlDbType.NVarChar, 200) With {.Value = company.Trim()},
            New SqlParameter("@ContactNumber", SqlDbType.NVarChar, 20) With {.Value = contactNumber.Trim()},
            New SqlParameter("@Address", SqlDbType.NVarChar, -1) With {.Value = address.Trim()})
    End Function

    Public Function UpdateSupplier(supplierId As Integer, company As String, contactNumber As String, address As String) As Integer
        Dim sql As String = "
            UPDATE tbl_supplier
               SET Company = @Company,
                   ContactNumber = @ContactNumber,
                   Address = @Address
             WHERE SupplierID = @SupplierID;"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@Company", SqlDbType.NVarChar, 200) With {.Value = company.Trim()},
            New SqlParameter("@ContactNumber", SqlDbType.NVarChar, 20) With {.Value = contactNumber.Trim()},
            New SqlParameter("@Address", SqlDbType.NVarChar, -1) With {.Value = address.Trim()},
            New SqlParameter("@SupplierID", SqlDbType.Int) With {.Value = supplierId})
    End Function

    Public Function DeactivateSupplier(supplierId As Integer) As Integer
        Dim sql As String = "UPDATE tbl_supplier SET isactive = 0 WHERE SupplierID = @SupplierID;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@SupplierID", SqlDbType.Int) With {.Value = supplierId})
    End Function

End Class
