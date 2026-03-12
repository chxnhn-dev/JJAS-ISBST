Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Public Module ProductEditRules
    Public Const StructuralFieldsLockedMessage As String = "This product is already used in inventory or transactions. Structural details cannot be modified."
End Module

Public Class ProductUsageInfo
    Public Property UsedInInventory As Boolean
    Public Property UsedInSalesHistory As Boolean
    Public Property UsedInDeliveryProducts As Boolean
    Public Property UsedInReturn As Boolean

    Public ReadOnly Property HasReferences As Boolean
        Get
            Return UsedInInventory OrElse UsedInSalesHistory OrElse UsedInDeliveryProducts OrElse UsedInReturn
        End Get
    End Property
End Class

Public Class ProductSaveRequest
    Public Property BarcodeNumber As String
    Public Property ProductName As String
    Public Property CostPrice As Decimal
    Public Property SellingPrice As Decimal
    Public Property Description As String
    Public Property BrandID As Integer
    Public Property CategoryID As Integer
    Public Property ColorID As Integer
    Public Property SizeID As Integer
    Public Property ImagePath As String
    Public Property IsActive As Boolean = True
    Public Property ChangedByUserID As Integer
    Public Property ChangeReason As String
End Class

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
                   ISNULL(p.CostPrice, 0) AS CostPrice,
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

    Public Function GetActiveProductsPage(request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        safeRequest.PageSize = Math.Max(1, safeRequest.PageSize)
        safeRequest.PageIndex = Math.Max(1, safeRequest.PageIndex)

        Dim normalizedSearch As String = If(safeRequest.SearchText, String.Empty).Trim()
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(normalizedSearch), "", "%" & normalizedSearch & "%")

        Dim countSql As String = "
            SELECT COUNT(1)
            FROM tbl_Products p
            WHERE p.IsActive = 1
              AND (@search = '' OR p.BarcodeNumber LIKE @search OR p.Product LIKE @search);"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 255) With {.Value = searchValue}
        )

        Dim dataSql As String = "
            SELECT p.ProductID,
                   p.BarcodeNumber,
                   p.Product,
                   c.Category,
                   s.Size,
                   b.Brand,
                   col.Color,
                   ISNULL(p.CostPrice, 0) AS CostPrice,
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
            ORDER BY p.DateCreated DESC, p.ProductID DESC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;"

        Dim rows As DataTable = Db.QueryDataTable(
            dataSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 255) With {.Value = searchValue},
            New SqlParameter("@offset", SqlDbType.Int) With {.Value = safeRequest.Offset},
            New SqlParameter("@pageSize", SqlDbType.Int) With {.Value = safeRequest.PageSize}
        )

        Return PagedQueryResult.Create(rows, totalRecords, safeRequest)
    End Function

    Public Function SoftDeleteProduct(productId As Integer) As Integer
        Dim sql As String = "UPDATE tbl_Products SET IsActive = 0 WHERE ProductID = @id;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@id", SqlDbType.Int) With {.Value = productId})
    End Function

    Public Function RestoreProduct(productId As Integer) As Integer
        Dim sql As String = "UPDATE tbl_Products SET IsActive = 1 WHERE ProductID = @id;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@id", SqlDbType.Int) With {.Value = productId})
    End Function

    Public Function GetProductById(productId As Integer) As DataRow
        Dim sql As String = "
            SELECT ProductID,
                   Product,
                   BarcodeNumber,
                   ISNULL(CostPrice, 0) AS CostPrice,
                   SellingPrice,
                   Description,
                   BrandID,
                   CategoryID,
                   ColorID,
                   SizeID,
                   ImagePath,
                   IsActive
            FROM tbl_Products
            WHERE ProductID = @ProductID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@ProductID", SqlDbType.Int) With {.Value = productId})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Public Function GetProductUsageInfo(productId As Integer) As ProductUsageInfo
        Return New ProductUsageInfo With {
            .UsedInInventory = TableContainsProductId("tbl_Inventory", productId),
            .UsedInSalesHistory = TableContainsProductId("tbl_SalesHistory", productId),
            .UsedInDeliveryProducts = TableContainsProductId("tbl_Delivery_Products", productId),
            .UsedInReturn = TableContainsProductId("tbl_Return", productId) OrElse
                          TableContainsProductId("tbl_Supplier_Return_Items", productId)
        }
    End Function

    Public Function InsertProduct(request As ProductSaveRequest) As Integer
        If request Is Nothing Then Throw New ArgumentNullException(NameOf(request))

        Dim sql As String = "
            INSERT INTO tbl_Products (
                BarcodeNumber,
                Product,
                CostPrice,
                SellingPrice,
                Description,
                BrandID,
                CategoryID,
                ColorID,
                SizeID,
                ImagePath,
                IsActive,
                DateCreated
            )
            VALUES (
                @BarcodeNumber,
                @Product,
                @CostPrice,
                @SellingPrice,
                @Description,
                @BrandID,
                @CategoryID,
                @ColorID,
                @SizeID,
                @ImagePath,
                @IsActive,
                @DateCreated
            );"

        Return Db.ExecuteNonQuery(sql, CreateSaveParameters(request))
    End Function

    Public Function UpdateProduct(productId As Integer, request As ProductSaveRequest) As Integer
        If request Is Nothing Then Throw New ArgumentNullException(NameOf(request))

        Dim currentRow As DataRow = GetProductById(productId)
        If currentRow Is Nothing Then
            Throw New InvalidOperationException("Selected product record was not found.")
        End If

        Dim usageInfo As ProductUsageInfo = GetProductUsageInfo(productId)
        If usageInfo.HasReferences AndAlso HasProtectedFieldChanges(currentRow, request) Then
            Throw New InvalidOperationException(StructuralFieldsLockedMessage)
        End If

        Dim sql As String = "
            UPDATE tbl_Products
               SET BarcodeNumber = @BarcodeNumber,
                   Product = @Product,
                   CostPrice = @CostPrice,
                   SellingPrice = @SellingPrice,
                   Description = @Description,
                   BrandID = @BrandID,
                   CategoryID = @CategoryID,
                   ColorID = @ColorID,
                   SizeID = @SizeID,
                   ImagePath = @ImagePath,
                   IsActive = @IsActive,
                   DateCreated = @DateCreated
             WHERE ProductID = @ProductID;"

        Dim parameters As New List(Of SqlParameter)(CreateSaveParameters(request)) From {
            New SqlParameter("@ProductID", SqlDbType.Int) With {.Value = productId}
        }

        Dim oldCostPrice As Decimal = ReadDecimal(currentRow, "CostPrice")
        Dim oldSellingPrice As Decimal = ReadDecimal(currentRow, "SellingPrice")
        Dim newCostPrice As Decimal = Decimal.Round(request.CostPrice, 2, MidpointRounding.AwayFromZero)
        Dim newSellingPrice As Decimal = Decimal.Round(request.SellingPrice, 2, MidpointRounding.AwayFromZero)

        Dim priceChanged As Boolean = oldCostPrice <> newCostPrice OrElse oldSellingPrice <> newSellingPrice
        Dim rowsAffected As Integer = 0

        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction()
                Try
                    If priceChanged Then
                        Dim insertSql As String = "
                            INSERT INTO tbl_ProductPriceHistory (
                                ProductID,
                                OldCostPrice,
                                NewCostPrice,
                                OldSellingPrice,
                                NewSellingPrice,
                                ChangedByUserID,
                                ChangeReason,
                                DateChanged
                            )
                            VALUES (
                                @ProductID,
                                @OldCostPrice,
                                @NewCostPrice,
                                @OldSellingPrice,
                                @NewSellingPrice,
                                @ChangedByUserID,
                                @ChangeReason,
                                GETDATE()
                            );"

                        Using cmdInsert As New SqlCommand(insertSql, conn, tran)
                            cmdInsert.Parameters.Add(New SqlParameter("@ProductID", SqlDbType.Int) With {.Value = productId})
                            cmdInsert.Parameters.Add(New SqlParameter("@OldCostPrice", SqlDbType.Decimal) With {.Precision = 18, .Scale = 2, .Value = oldCostPrice})
                            cmdInsert.Parameters.Add(New SqlParameter("@NewCostPrice", SqlDbType.Decimal) With {.Precision = 18, .Scale = 2, .Value = newCostPrice})
                            cmdInsert.Parameters.Add(New SqlParameter("@OldSellingPrice", SqlDbType.Decimal) With {.Precision = 18, .Scale = 2, .Value = oldSellingPrice})
                            cmdInsert.Parameters.Add(New SqlParameter("@NewSellingPrice", SqlDbType.Decimal) With {.Precision = 18, .Scale = 2, .Value = newSellingPrice})

                            Dim changedByValue As Object = If(request.ChangedByUserID > 0, CType(request.ChangedByUserID, Object), DBNull.Value)
                            cmdInsert.Parameters.Add(New SqlParameter("@ChangedByUserID", SqlDbType.Int) With {.Value = changedByValue})

                            Dim reasonValue As Object = If(String.IsNullOrWhiteSpace(request.ChangeReason), DBNull.Value, request.ChangeReason.Trim())
                            cmdInsert.Parameters.Add(New SqlParameter("@ChangeReason", SqlDbType.NVarChar, 255) With {.Value = reasonValue})

                            cmdInsert.ExecuteNonQuery()
                        End Using
                    End If

                    Using cmdUpdate As New SqlCommand(sql, conn, tran)
                        cmdUpdate.Parameters.AddRange(parameters.ToArray())
                        rowsAffected = cmdUpdate.ExecuteNonQuery()
                    End Using

                    tran.Commit()
                Catch
                    Try
                        tran.Rollback()
                    Catch
                    End Try
                    Throw
                End Try
            End Using
        End Using

        Return rowsAffected
    End Function

    Public Function FindFirstActiveProductByBarcode(searchText As String) As DataRow
        Dim sql As String = "
            SELECT TOP 1 ProductID,
                         Product,
                         BarcodeNumber,
                         ISNULL(CostPrice, 0) AS CostPrice,
                         SellingPrice,
                         ImagePath
            FROM tbl_Products
            WHERE IsActive = 1
              AND BarcodeNumber LIKE @search
            ORDER BY ProductID;"

        Dim searchValue As String = "%" & If(searchText, String.Empty).Trim() & "%"
        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Private Function CreateSaveParameters(request As ProductSaveRequest) As SqlParameter()
        Dim costPriceParam As New SqlParameter("@CostPrice", SqlDbType.Decimal) With {
            .Precision = 18,
            .Scale = 2,
            .Value = request.CostPrice
        }

        Dim sellingPriceParam As New SqlParameter("@SellingPrice", SqlDbType.Decimal) With {
            .Precision = 18,
            .Scale = 2,
            .Value = request.SellingPrice
        }

        Return {
            New SqlParameter("@BarcodeNumber", SqlDbType.NVarChar, 50) With {.Value = NormalizeText(request.BarcodeNumber)},
            New SqlParameter("@Product", SqlDbType.NVarChar, 50) With {.Value = NormalizeText(request.ProductName)},
            costPriceParam,
            sellingPriceParam,
            New SqlParameter("@Description", SqlDbType.NVarChar, 200) With {.Value = NormalizeText(request.Description)},
            New SqlParameter("@BrandID", SqlDbType.Int) With {.Value = request.BrandID},
            New SqlParameter("@CategoryID", SqlDbType.Int) With {.Value = request.CategoryID},
            New SqlParameter("@ColorID", SqlDbType.Int) With {.Value = request.ColorID},
            New SqlParameter("@SizeID", SqlDbType.Int) With {.Value = request.SizeID},
            New SqlParameter("@ImagePath", SqlDbType.NVarChar, 500) With {.Value = If(String.IsNullOrWhiteSpace(request.ImagePath), CType(DBNull.Value, Object), request.ImagePath.Trim())},
            New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = request.IsActive},
            New SqlParameter("@DateCreated", SqlDbType.DateTime) With {.Value = DateTime.Now}
        }
    End Function

    Private Function HasProtectedFieldChanges(currentRow As DataRow, request As ProductSaveRequest) As Boolean
        If currentRow Is Nothing Then
            Return False
        End If

        Return Not String.Equals(ReadString(currentRow, "BarcodeNumber"), NormalizeText(request.BarcodeNumber), StringComparison.Ordinal) OrElse
               ReadDecimal(currentRow, "CostPrice") <> Decimal.Round(request.CostPrice, 2, MidpointRounding.AwayFromZero) OrElse
               Not String.Equals(ReadString(currentRow, "Description"), NormalizeText(request.Description), StringComparison.Ordinal) OrElse
               ReadInt(currentRow, "BrandID") <> request.BrandID OrElse
               ReadInt(currentRow, "CategoryID") <> request.CategoryID OrElse
               ReadInt(currentRow, "ColorID") <> request.ColorID OrElse
               ReadInt(currentRow, "SizeID") <> request.SizeID
    End Function

    Private Function TableContainsProductId(tableName As String, productId As Integer) As Boolean
        Dim qualifiedTableName As String = Nothing
        If Not TryGetQualifiedProductTableName(tableName, qualifiedTableName) Then
            Return False
        End If

        Dim sql As String = $"SELECT COUNT(1) FROM {qualifiedTableName} WHERE ProductID = @ProductID;"
        Dim count As Integer = Db.ExecuteScalar(Of Integer)(
            sql,
            New SqlParameter("@ProductID", SqlDbType.Int) With {.Value = productId}
        )
        Return count > 0
    End Function

    Private Function TryGetQualifiedProductTableName(tableName As String, ByRef qualifiedTableName As String) As Boolean
        qualifiedTableName = Nothing

        Dim sql As String = "
            SELECT TOP 1 TABLE_SCHEMA
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = @TableName
              AND COLUMN_NAME = 'ProductID';"

        Dim schemaName As String = Db.ExecuteScalar(Of String)(
            sql,
            New SqlParameter("@TableName", SqlDbType.NVarChar, 128) With {.Value = tableName}
        )

        If String.IsNullOrWhiteSpace(schemaName) Then
            Return False
        End If

        qualifiedTableName = $"[{EscapeSqlIdentifier(schemaName)}].[{EscapeSqlIdentifier(tableName)}]"
        Return True
    End Function

    Private Function EscapeSqlIdentifier(identifier As String) As String
        Return If(identifier, String.Empty).Replace("]", "]]")
    End Function

    Private Function NormalizeText(value As String) As String
        Return If(value, String.Empty).Trim()
    End Function

    Private Function ReadString(row As DataRow, columnName As String) As String
        If row Is Nothing OrElse row.Table Is Nothing OrElse Not row.Table.Columns.Contains(columnName) OrElse row.IsNull(columnName) Then
            Return String.Empty
        End If

        Return Convert.ToString(row(columnName)).Trim()
    End Function

    Private Function ReadDecimal(row As DataRow, columnName As String) As Decimal
        If row Is Nothing OrElse row.Table Is Nothing OrElse Not row.Table.Columns.Contains(columnName) OrElse row.IsNull(columnName) Then
            Return 0D
        End If

        Return Decimal.Round(Convert.ToDecimal(row(columnName)), 2, MidpointRounding.AwayFromZero)
    End Function

    Private Function ReadInt(row As DataRow, columnName As String) As Integer
        If row Is Nothing OrElse row.Table Is Nothing OrElse Not row.Table.Columns.Contains(columnName) OrElse row.IsNull(columnName) Then
            Return 0
        End If

        Return Convert.ToInt32(row(columnName))
    End Function

End Class
