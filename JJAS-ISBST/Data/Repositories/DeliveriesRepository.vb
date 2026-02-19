Imports System.Data
Imports System.Data.SqlClient

Public Enum PostDeliveryStatus
    Success = 0
    AlreadyPosted = 1
End Enum

Public Class DeliveryLineItem
    Public Property ProductID As Integer
    Public Property Quantity As Integer
    Public Property CostPrice As Decimal
End Class

Public Class DeliveriesRepository

    Public Function GetPendingDeliveryProducts(searchText As String) As DataTable
        Dim sql As String = "
            SELECT dp.DeliveryProductID,
                   d.DeliveryID,
                   d.OrderNumber,
                   d.DeliveryDate,
                   s.Company AS CompanyName,
                   p.Product AS ProductName,
                   p.BarcodeNumber,
                   dp.Quantity,
                   dp.CostPrice,
                   dp.Status,
                   p.SellingPrice,
                   p.ImagePath
            FROM tbl_delivery_products dp
            INNER JOIN tbl_deliveries d ON dp.DeliveryID = d.DeliveryID
            INNER JOIN tbl_supplier s ON d.SupplierID = s.SupplierID
            INNER JOIN tbl_products p ON dp.ProductID = p.ProductID
            WHERE dp.Status = 'Pending'
              AND (@search = '' OR p.BarcodeNumber LIKE @search OR p.Product LIKE @search)
            ORDER BY p.DateCreated DESC;"

        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")
        Return Db.QueryDataTable(sql, New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue})
    End Function

    Public Function GetDeliveryHeaderById(deliveryId As Integer) As DataRow
        Dim sql As String = "
            SELECT DeliveryID,
                   OrderNumber,
                   DeliveryDate,
                   SupplierID
            FROM tbl_Deliveries
            WHERE DeliveryID = @DeliveryID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Public Function GetDeliveryProductsByDeliveryId(deliveryId As Integer) As DataTable
        Dim sql As String = "
            SELECT dp.DeliveryProductID,
                   dp.ProductID,
                   p.BarcodeNumber,
                   p.Product,
                   dp.Quantity,
                   dp.CostPrice,
                   p.SellingPrice,
                   p.ImagePath
            FROM tbl_Delivery_Products dp
            INNER JOIN tbl_Products p ON dp.ProductID = p.ProductID
            WHERE dp.DeliveryID = @DeliveryID;"

        Return Db.QueryDataTable(sql, New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
    End Function

    Public Function InsertDelivery(supplierId As Integer,
                                   orderNumber As String,
                                   deliveryDate As DateTime,
                                   items As IEnumerable(Of DeliveryLineItem)) As Integer
        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction()
                Try
                    Dim newDeliveryId As Integer

                    Dim insertDeliverySql As String =
                        "INSERT INTO tbl_deliveries (SupplierID, OrderNumber, DeliveryDate, DateCreated, Status) " &
                        "VALUES (@SupplierID, @OrderNumber, @DeliveryDate, GETDATE(), 'Pending'); SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(insertDeliverySql, conn, tran)
                        cmd.Parameters.Add(New SqlParameter("@SupplierID", SqlDbType.Int) With {.Value = supplierId})
                        cmd.Parameters.Add(New SqlParameter("@OrderNumber", SqlDbType.NVarChar, 50) With {.Value = orderNumber.Trim()})
                        cmd.Parameters.Add(New SqlParameter("@DeliveryDate", SqlDbType.Date) With {.Value = deliveryDate.Date})
                        newDeliveryId = Convert.ToInt32(cmd.ExecuteScalar())
                    End Using

                    InsertDeliveryItems(conn, tran, newDeliveryId, items)
                    tran.Commit()
                    Return newDeliveryId
                Catch
                    tran.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Public Function UpdateDelivery(deliveryId As Integer,
                                   supplierId As Integer,
                                   orderNumber As String,
                                   deliveryDate As DateTime,
                                   items As IEnumerable(Of DeliveryLineItem)) As Integer
        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction()
                Try
                    Dim updateDeliverySql As String =
                        "UPDATE tbl_deliveries " &
                        "SET SupplierID = @SupplierID, OrderNumber = @OrderNumber, DeliveryDate = @DeliveryDate " &
                        "WHERE DeliveryID = @DeliveryID;"

                    Using cmd As New SqlCommand(updateDeliverySql, conn, tran)
                        cmd.Parameters.Add(New SqlParameter("@SupplierID", SqlDbType.Int) With {.Value = supplierId})
                        cmd.Parameters.Add(New SqlParameter("@OrderNumber", SqlDbType.NVarChar, 50) With {.Value = orderNumber.Trim()})
                        cmd.Parameters.Add(New SqlParameter("@DeliveryDate", SqlDbType.Date) With {.Value = deliveryDate.Date})
                        cmd.Parameters.Add(New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
                        cmd.ExecuteNonQuery()
                    End Using

                    Using deleteProductsCmd As New SqlCommand("DELETE FROM tbl_Delivery_Products WHERE DeliveryID = @DeliveryID;", conn, tran)
                        deleteProductsCmd.Parameters.Add(New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
                        deleteProductsCmd.ExecuteNonQuery()
                    End Using

                    InsertDeliveryItems(conn, tran, deliveryId, items)
                    tran.Commit()
                    Return deliveryId
                Catch
                    tran.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Private Sub InsertDeliveryItems(conn As SqlConnection,
                                    tran As SqlTransaction,
                                    deliveryId As Integer,
                                    items As IEnumerable(Of DeliveryLineItem))
        Dim insertProductSql As String =
            "INSERT INTO tbl_Delivery_Products (DeliveryID, ProductID, CostPrice, Quantity, DateUpdated, Status) " &
            "VALUES (@DeliveryID, @ProductID, @CostPrice, @Quantity, GETDATE(), 'Pending');"

        For Each item As DeliveryLineItem In items
            Using cmd As New SqlCommand(insertProductSql, conn, tran)
                cmd.Parameters.Add(New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
                cmd.Parameters.Add(New SqlParameter("@ProductID", SqlDbType.Int) With {.Value = item.ProductID})

                Dim costPriceParam As New SqlParameter("@CostPrice", SqlDbType.Decimal) With {.Value = item.CostPrice}
                costPriceParam.Precision = 18
                costPriceParam.Scale = 2
                cmd.Parameters.Add(costPriceParam)

                cmd.Parameters.Add(New SqlParameter("@Quantity", SqlDbType.Int) With {.Value = item.Quantity})
                cmd.ExecuteNonQuery()
            End Using
        Next
    End Sub

    Public Function DeleteDeliveryProduct(deliveryProductId As Integer) As Integer
        Dim sql As String = "DELETE FROM tbl_delivery_products WHERE DeliveryProductID = @DeliveryProductID;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@DeliveryProductID", SqlDbType.Int) With {.Value = deliveryProductId})
    End Function

    Public Function PostDeliveryProduct(deliveryProductId As Integer) As PostDeliveryStatus
        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction()
                Try
                    Dim productId As Integer
                    Dim quantityToAdd As Integer
                    Dim newCostPrice As Decimal

                    Dim getPendingSql As String =
                        "SELECT ProductID, Quantity, CostPrice " &
                        "FROM tbl_delivery_products " &
                        "WHERE DeliveryProductID = @DeliveryProductID AND Status = 'Pending';"

                    Using getCmd As New SqlCommand(getPendingSql, conn, tran)
                        getCmd.Parameters.Add(New SqlParameter("@DeliveryProductID", SqlDbType.Int) With {.Value = deliveryProductId})
                        Using rdr As SqlDataReader = getCmd.ExecuteReader()
                            If Not rdr.Read() Then
                                tran.Rollback()
                                Return PostDeliveryStatus.AlreadyPosted
                            End If

                            productId = Convert.ToInt32(rdr("ProductID"))
                            quantityToAdd = Convert.ToInt32(rdr("Quantity"))
                            newCostPrice = Convert.ToDecimal(rdr("CostPrice"))
                        End Using
                    End Using

                    Dim postedDeliveryProductId As Object
                    Using checkCmd As New SqlCommand("SELECT TOP 1 DeliveryProductID FROM tbl_delivery_products WHERE ProductID = @ProductID AND Status = 'Posted';", conn, tran)
                        checkCmd.Parameters.Add(New SqlParameter("@ProductID", SqlDbType.Int) With {.Value = productId})
                        postedDeliveryProductId = checkCmd.ExecuteScalar()
                    End Using

                    If postedDeliveryProductId IsNot Nothing AndAlso Not IsDBNull(postedDeliveryProductId) Then
                        Dim postedId As Integer = Convert.ToInt32(postedDeliveryProductId)
                        Dim oldQty As Integer = 0
                        Dim oldCostPrice As Decimal = 0D

                        Using getPostedCmd As New SqlCommand("SELECT Quantity, CostPrice FROM tbl_delivery_products WHERE DeliveryProductID = @PostedID;", conn, tran)
                            getPostedCmd.Parameters.Add(New SqlParameter("@PostedID", SqlDbType.Int) With {.Value = postedId})
                            Using rdr As SqlDataReader = getPostedCmd.ExecuteReader()
                                If rdr.Read() Then
                                    oldQty = Convert.ToInt32(rdr("Quantity"))
                                    oldCostPrice = Convert.ToDecimal(rdr("CostPrice"))
                                End If
                            End Using
                        End Using

                        Dim totalQty As Integer = oldQty + quantityToAdd
                        Dim weightedCost As Decimal = If(totalQty > 0,
                            Math.Round(((oldCostPrice * oldQty) + (newCostPrice * quantityToAdd)) / totalQty, 2),
                            newCostPrice)

                        Using updatePostedCmd As New SqlCommand("UPDATE tbl_delivery_products SET Quantity = @TotalQty, CostPrice = @WeightedCost, DateUpdated = GETDATE() WHERE DeliveryProductID = @PostedID;", conn, tran)
                            updatePostedCmd.Parameters.Add(New SqlParameter("@TotalQty", SqlDbType.Int) With {.Value = totalQty})

                            Dim weightedCostParam As New SqlParameter("@WeightedCost", SqlDbType.Decimal) With {.Value = weightedCost}
                            weightedCostParam.Precision = 18
                            weightedCostParam.Scale = 2
                            updatePostedCmd.Parameters.Add(weightedCostParam)

                            updatePostedCmd.Parameters.Add(New SqlParameter("@PostedID", SqlDbType.Int) With {.Value = postedId})
                            updatePostedCmd.ExecuteNonQuery()
                        End Using

                        Using deletePendingCmd As New SqlCommand("DELETE FROM tbl_delivery_products WHERE DeliveryProductID = @PendingID;", conn, tran)
                            deletePendingCmd.Parameters.Add(New SqlParameter("@PendingID", SqlDbType.Int) With {.Value = deliveryProductId})
                            deletePendingCmd.ExecuteNonQuery()
                        End Using
                    Else
                        Using postCmd As New SqlCommand("UPDATE tbl_delivery_products SET Status = 'Posted', DateUpdated = GETDATE() WHERE DeliveryProductID = @DeliveryProductID AND Status = 'Pending';", conn, tran)
                            postCmd.Parameters.Add(New SqlParameter("@DeliveryProductID", SqlDbType.Int) With {.Value = deliveryProductId})
                            Dim rowsAffected As Integer = postCmd.ExecuteNonQuery()
                            If rowsAffected = 0 Then
                                tran.Rollback()
                                Return PostDeliveryStatus.AlreadyPosted
                            End If
                        End Using
                    End If

                    tran.Commit()
                    Return PostDeliveryStatus.Success
                Catch
                    tran.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

End Class
