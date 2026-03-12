Imports System.Data
Imports System.Data.SqlClient

Public Class SupplierRepository

    Public Function GetActiveSuppliers(searchText As String) As DataTable
        Dim sql As String = "
            SELECT SupplierID,
                   COALESCE(NULLIF(LTRIM(RTRIM(SupplierName)), ''), Company) AS Company,
                   ContactNumber,
                   Address
            FROM tbl_supplier
            WHERE isactive = 1
              AND (@search = '' OR COALESCE(NULLIF(LTRIM(RTRIM(SupplierName)), ''), Company) LIKE @search)
            ORDER BY SupplierID DESC;"

        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")
        Return Db.QueryDataTable(sql, New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue})
    End Function

    Public Function GetActiveSuppliersPage(request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        safeRequest.PageSize = Math.Max(1, safeRequest.PageSize)
        safeRequest.PageIndex = Math.Max(1, safeRequest.PageIndex)

        Dim normalizedSearch As String = If(safeRequest.SearchText, String.Empty).Trim()
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(normalizedSearch), "", "%" & normalizedSearch & "%")

        Dim countSql As String = "
            SELECT COUNT(1)
            FROM tbl_supplier
            WHERE isactive = 1
              AND (@search = '' OR COALESCE(NULLIF(LTRIM(RTRIM(SupplierName)), ''), Company) LIKE @search);"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue}
        )

        Dim dataSql As String = "
            SELECT SupplierID,
                   COALESCE(NULLIF(LTRIM(RTRIM(SupplierName)), ''), Company) AS Company,
                   ContactNumber,
                   Address
            FROM tbl_supplier
            WHERE isactive = 1
              AND (@search = '' OR COALESCE(NULLIF(LTRIM(RTRIM(SupplierName)), ''), Company) LIKE @search)
            ORDER BY SupplierID DESC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;"

        Dim rows As DataTable = Db.QueryDataTable(
            dataSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue},
            New SqlParameter("@offset", SqlDbType.Int) With {.Value = safeRequest.Offset},
            New SqlParameter("@pageSize", SqlDbType.Int) With {.Value = safeRequest.PageSize}
        )

        Return PagedQueryResult.Create(rows, totalRecords, safeRequest)
    End Function

    Public Function GetSupplierLookup() As DataTable
        Dim sql As String = "
            SELECT SupplierID,
                   COALESCE(NULLIF(LTRIM(RTRIM(SupplierName)), ''), Company) AS Company
            FROM tbl_supplier
            WHERE isactive = 1
            ORDER BY COALESCE(NULLIF(LTRIM(RTRIM(SupplierName)), ''), Company);"
        Return Db.QueryDataTable(sql)
    End Function

    Public Function GetSupplierById(supplierId As Integer) As DataRow
        Dim sql As String = "
            SELECT SupplierID,
                   COALESCE(NULLIF(LTRIM(RTRIM(SupplierName)), ''), Company) AS Company,
                   ContactNumber,
                   Address,
                   AcceptsReturnRefund,
                   ReturnWindowDays
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
            WHERE LOWER(COALESCE(NULLIF(LTRIM(RTRIM(SupplierName)), ''), Company)) = LOWER(@SupplierName);"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@SupplierName", SqlDbType.NVarChar, 200) With {.Value = companyName.Trim()}
        }

        If excludeSupplierId.HasValue Then
            sql = "
                SELECT COUNT(1)
                FROM tbl_supplier
                WHERE LOWER(COALESCE(NULLIF(LTRIM(RTRIM(SupplierName)), ''), Company)) = LOWER(@SupplierName)
                  AND SupplierID <> @SupplierID;"
            parameters.Add(New SqlParameter("@SupplierID", SqlDbType.Int) With {.Value = excludeSupplierId.Value})
        End If

        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, parameters.ToArray())
        Return count > 0
    End Function

    Public Function InsertSupplier(supplierName As String,
                                   contactNumber As String,
                                   address As String,
                                   acceptsReturnRefund As Boolean,
                                   returnWindowDays As Integer?) As Integer
        Dim sql As String = "
            INSERT INTO tbl_supplier (SupplierName, Company, ContactNumber, Address, isactive, expiredate, dateCreated, AcceptsReturnRefund, ReturnWindowDays)
            VALUES (@SupplierName, @SupplierName, @ContactNumber, @Address, 1, NULL, GETDATE(), @AcceptsReturnRefund, @ReturnWindowDays);"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@SupplierName", SqlDbType.NVarChar, 200) With {.Value = supplierName.Trim()},
            New SqlParameter("@ContactNumber", SqlDbType.NVarChar, 20) With {.Value = contactNumber.Trim()},
            New SqlParameter("@Address", SqlDbType.NVarChar, -1) With {.Value = address.Trim()},
            New SqlParameter("@AcceptsReturnRefund", SqlDbType.Bit) With {.Value = acceptsReturnRefund},
            New SqlParameter("@ReturnWindowDays", SqlDbType.Int) With {.Value = If(returnWindowDays.HasValue, CType(returnWindowDays.Value, Object), DBNull.Value)})
    End Function

    Public Function UpdateSupplier(supplierId As Integer,
                                   supplierName As String,
                                   contactNumber As String,
                                   address As String,
                                   acceptsReturnRefund As Boolean,
                                   returnWindowDays As Integer?) As Integer
        Dim sql As String = "
            UPDATE tbl_supplier
               SET SupplierName = @SupplierName,
                   Company = @SupplierName,
                   ContactNumber = @ContactNumber,
                   Address = @Address,
                   AcceptsReturnRefund = @AcceptsReturnRefund,
                   ReturnWindowDays = @ReturnWindowDays
             WHERE SupplierID = @SupplierID;"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@SupplierName", SqlDbType.NVarChar, 200) With {.Value = supplierName.Trim()},
            New SqlParameter("@ContactNumber", SqlDbType.NVarChar, 20) With {.Value = contactNumber.Trim()},
            New SqlParameter("@Address", SqlDbType.NVarChar, -1) With {.Value = address.Trim()},
            New SqlParameter("@AcceptsReturnRefund", SqlDbType.Bit) With {.Value = acceptsReturnRefund},
            New SqlParameter("@ReturnWindowDays", SqlDbType.Int) With {.Value = If(returnWindowDays.HasValue, CType(returnWindowDays.Value, Object), DBNull.Value)},
            New SqlParameter("@SupplierID", SqlDbType.Int) With {.Value = supplierId})
    End Function

    Public Function DeactivateSupplier(supplierId As Integer) As Integer
        Dim sql As String = "UPDATE tbl_supplier SET isactive = 0 WHERE SupplierID = @SupplierID;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@SupplierID", SqlDbType.Int) With {.Value = supplierId})
    End Function

End Class
