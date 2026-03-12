Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Public Enum PostDeliveryStatus
    Success = 0
    AlreadyPosted = 1
End Enum

Public Class DeliveryLineItem
    Public Property ProductID As Integer
    Public Property Quantity As Integer
End Class

Public Class DeliveryPagedResult
    Public Property Items As DataTable
    Public Property TotalRecords As Integer
End Class

Public Class SupplierReturnLineItem
    Public Property DeliveryProductID As Integer
    Public Property ProductID As Integer
    Public Property Quantity As Integer
End Class

Public Class SupplierReturnAppliedItem
    Public Property DeliveryProductID As Integer
    Public Property QuantityReturned As Integer
End Class

Public Class SupplierReturnSaveResult
    Public Property ReturnID As Integer
    Public Property ReturnNumber As String
    Public Property AppliedItems As List(Of SupplierReturnAppliedItem)
End Class

Public Class DeliveriesRepository

    Public Function GetPendingDeliveryProducts(searchText As String) As DataTable
        Dim sql As String = "
            SELECT d.DeliveryNumber,
                   d.OrderNumber,
                   d.DeliveryDate,
                   s.Company AS CompanyName,
                   COUNT(dp.DeliveryProductID) AS ItemCount,
                   SUM(ISNULL(dp.Quantity, 0)) AS TotalQty,
                   CASE
                       WHEN SUM(CASE WHEN ISNULL(dp.Status, 'Pending') = 'Pending' THEN 1 ELSE 0 END) = 0
                            AND SUM(CASE WHEN ISNULL(dp.Status, '') = 'Posted' THEN 1 ELSE 0 END) > 0
                           THEN 'Posted'
                       WHEN SUM(CASE WHEN ISNULL(dp.Status, '') = 'Posted' THEN 1 ELSE 0 END) > 0
                           THEN 'Partially Posted'
                       ELSE 'Pending'
                   END AS Status,
                   d.DeliveryID,
                   SUM(CASE WHEN ISNULL(dp.Status, 'Pending') = 'Pending' THEN 1 ELSE 0 END) AS PendingItemCount,
                   SUM(CASE WHEN ISNULL(dp.Status, '') = 'Posted' THEN 1 ELSE 0 END) AS PostedItemCount,
                   MAX(CASE WHEN p.ImagePath IS NOT NULL AND LTRIM(RTRIM(p.ImagePath)) <> '' THEN p.ImagePath END) AS ImagePath
            FROM tbl_deliveries d
            INNER JOIN tbl_supplier s ON d.SupplierID = s.SupplierID
            INNER JOIN tbl_delivery_products dp ON dp.DeliveryID = d.DeliveryID
            INNER JOIN tbl_products p ON dp.ProductID = p.ProductID
            WHERE (@search = ''
                   OR ISNULL(d.DeliveryNumber, '') LIKE @search
                   OR d.OrderNumber LIKE @search
                   OR s.Company LIKE @search
                   OR p.BarcodeNumber LIKE @search
                   OR p.Product LIKE @search)
            GROUP BY d.DeliveryID, d.DeliveryNumber, d.OrderNumber, d.DeliveryDate, s.Company, d.DateCreated
            ORDER BY d.DateCreated DESC, d.DeliveryID DESC;"

        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")
        Return Db.QueryDataTable(sql, New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue})
    End Function

    Public Function GetPendingDeliveryProducts(searchText As String, pageNumber As Integer, pageSize As Integer) As DeliveryPagedResult
        Dim safePageNumber As Integer = Math.Max(1, pageNumber)
        Dim safePageSize As Integer = Math.Max(1, pageSize)
        Dim startRow As Integer = ((safePageNumber - 1) * safePageSize) + 1
        Dim endRow As Integer = startRow + safePageSize - 1
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")

        Dim countSql As String = "
            SELECT COUNT(DISTINCT d.DeliveryID)
            FROM tbl_deliveries d
            INNER JOIN tbl_supplier s ON d.SupplierID = s.SupplierID
            INNER JOIN tbl_delivery_products dp ON dp.DeliveryID = d.DeliveryID
            INNER JOIN tbl_products p ON dp.ProductID = p.ProductID
            WHERE (@search = ''
                   OR ISNULL(d.DeliveryNumber, '') LIKE @search
                   OR d.OrderNumber LIKE @search
                   OR s.Company LIKE @search
                   OR p.BarcodeNumber LIKE @search
                   OR p.Product LIKE @search);"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue}
        )

        Dim dataSql As String = "
            WITH DeliveryRows AS (
                SELECT d.DeliveryID,
                       d.DeliveryNumber,
                       d.OrderNumber,
                       d.DeliveryDate,
                       s.Company AS CompanyName,
                       COUNT(dp.DeliveryProductID) AS ItemCount,
                       SUM(ISNULL(dp.Quantity, 0)) AS TotalQty,
                       SUM(CASE WHEN ISNULL(dp.Status, 'Pending') = 'Pending' THEN 1 ELSE 0 END) AS PendingItemCount,
                       SUM(CASE WHEN ISNULL(dp.Status, '') = 'Posted' THEN 1 ELSE 0 END) AS PostedItemCount,
                       CASE
                           WHEN SUM(CASE WHEN ISNULL(dp.Status, 'Pending') = 'Pending' THEN 1 ELSE 0 END) = 0
                                AND SUM(CASE WHEN ISNULL(dp.Status, '') = 'Posted' THEN 1 ELSE 0 END) > 0
                               THEN 'Posted'
                           WHEN SUM(CASE WHEN ISNULL(dp.Status, '') = 'Posted' THEN 1 ELSE 0 END) > 0
                               THEN 'Partially Posted'
                           ELSE 'Pending'
                       END AS Status,
                       MAX(CASE WHEN p.ImagePath IS NOT NULL AND LTRIM(RTRIM(p.ImagePath)) <> '' THEN p.ImagePath END) AS ImagePath,
                       ROW_NUMBER() OVER (ORDER BY d.DateCreated DESC, d.DeliveryID DESC) AS RowNumber
                FROM tbl_deliveries d
                INNER JOIN tbl_supplier s ON d.SupplierID = s.SupplierID
                INNER JOIN tbl_delivery_products dp ON dp.DeliveryID = d.DeliveryID
                INNER JOIN tbl_products p ON dp.ProductID = p.ProductID
                WHERE (@search = ''
                       OR ISNULL(d.DeliveryNumber, '') LIKE @search
                       OR d.OrderNumber LIKE @search
                       OR s.Company LIKE @search
                       OR p.BarcodeNumber LIKE @search
                       OR p.Product LIKE @search)
                GROUP BY d.DeliveryID, d.DeliveryNumber, d.OrderNumber, d.DeliveryDate, s.Company, d.DateCreated
            )
            SELECT DeliveryNumber,
                   OrderNumber,
                   DeliveryDate,
                   CompanyName,
                   ItemCount,
                   TotalQty,
                   Status,
                   DeliveryID,
                   PendingItemCount,
                   PostedItemCount,
                   ImagePath
            FROM DeliveryRows
            WHERE RowNumber BETWEEN @startRow AND @endRow
            ORDER BY RowNumber;"

        Dim items As DataTable = Db.QueryDataTable(
            dataSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 150) With {.Value = searchValue},
            New SqlParameter("@startRow", SqlDbType.Int) With {.Value = startRow},
            New SqlParameter("@endRow", SqlDbType.Int) With {.Value = endRow}
        )

        Return New DeliveryPagedResult With {
            .Items = items,
            .TotalRecords = totalRecords
        }
    End Function

    Public Function GetDeliveryHeaderById(deliveryId As Integer) As DataRow
        Dim sql As String = "
            SELECT DeliveryID,
                   DeliveryNumber,
                   OrderNumber,
                   DeliveryDate,
                   SupplierID
            FROM tbl_Deliveries
            WHERE DeliveryID = @DeliveryID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Public Function DeliveryNumberExists(deliveryNumber As String, Optional excludeDeliveryId As Integer? = Nothing) As Boolean
        Dim normalizedDeliveryNumber As String = If(deliveryNumber, String.Empty).Trim()
        If normalizedDeliveryNumber.Length = 0 Then
            Return False
        End If

        Dim sql As String = "
            SELECT COUNT(1)
            FROM tbl_Deliveries
            WHERE UPPER(LTRIM(RTRIM(ISNULL(DeliveryNumber, '')))) = UPPER(@DeliveryNumber)
              AND (@ExcludeDeliveryID IS NULL OR DeliveryID <> @ExcludeDeliveryID);"

        Dim matchCount As Integer = Db.ExecuteScalar(Of Integer)(
            sql,
            New SqlParameter("@DeliveryNumber", SqlDbType.NVarChar, 50) With {.Value = normalizedDeliveryNumber},
            New SqlParameter("@ExcludeDeliveryID", SqlDbType.Int) With {
                .Value = If(excludeDeliveryId.HasValue, CType(excludeDeliveryId.Value, Object), DBNull.Value)
            }
        )

        Return matchCount > 0
    End Function

    Public Function GetDeliveryProductsByDeliveryId(deliveryId As Integer) As DataTable
        Dim sql As String = "
            SELECT dp.DeliveryProductID,
                   dp.ProductID,
                   p.BarcodeNumber,
                   p.Product,
                   dp.Quantity,
                   ISNULL(dp.ReturnedQty, 0) AS ReturnedQty,
                   ISNULL(dp.Status, 'Pending') AS Status,
                   ISNULL(p.CostPrice, 0) AS CostPrice,
                   p.ImagePath
            FROM tbl_Delivery_Products dp
            INNER JOIN tbl_Products p ON dp.ProductID = p.ProductID
            WHERE dp.DeliveryID = @DeliveryID
            ORDER BY CASE WHEN ISNULL(dp.Status, 'Pending') = 'Posted' THEN 0 ELSE 1 END,
                     dp.DeliveryProductID;"

        Return Db.QueryDataTable(sql, New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
    End Function

    Public Function GetSupplierReturnContextByDeliveryId(deliveryId As Integer) As DataRow
        Dim sql As String = "
            SELECT d.DeliveryID,
                   d.SupplierID,
                   COALESCE(NULLIF(LTRIM(RTRIM(s.SupplierName)), ''), s.Company) AS SupplierName,
                   ISNULL(d.DeliveryNumber, '') AS DeliveryNumber,
                   ISNULL(d.OrderNumber, '') AS OrderNumber,
                   d.DeliveryDate,
                   ISNULL(s.AcceptsReturnRefund, 0) AS AcceptsReturnRefund,
                   s.ReturnWindowDays,
                   ISNULL(lineStats.PendingItemCount, 0) AS PendingItemCount,
                   ISNULL(lineStats.PostedItemCount, 0) AS PostedItemCount,
                   CASE
                       WHEN ISNULL(lineStats.PostedItemCount, 0) > 0 THEN 'Posted'
                       WHEN ISNULL(lineStats.PendingItemCount, 0) > 0 THEN 'Pending'
                       ELSE 'Pending'
                   END AS DeliveryStatus
            FROM tbl_Deliveries d
            INNER JOIN tbl_Supplier s ON d.SupplierID = s.SupplierID
            OUTER APPLY (
                SELECT SUM(CASE WHEN ISNULL(dp.Status, 'Pending') = 'Pending' THEN 1 ELSE 0 END) AS PendingItemCount,
                       SUM(CASE WHEN ISNULL(dp.Status, 'Pending') = 'Posted' THEN 1 ELSE 0 END) AS PostedItemCount
                FROM tbl_Delivery_Products dp
                WHERE dp.DeliveryID = d.DeliveryID
            ) lineStats
            WHERE d.DeliveryID = @DeliveryID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Public Function GetPendingSupplierReturnDeliveryLookup() As DataTable
        Dim sql As String = "
            SELECT d.DeliveryID,
                   COALESCE(NULLIF(LTRIM(RTRIM(s.SupplierName)), ''), s.Company) AS SupplierName,
                   ISNULL(d.DeliveryNumber, '') AS DeliveryNumber,
                   ISNULL(d.OrderNumber, '') AS OrderNumber,
                   d.DeliveryDate,
                   ISNULL(lineStats.PendingItemCount, 0) AS PendingItemCount,
                   ISNULL(lineStats.PostedItemCount, 0) AS PostedItemCount,
                   'Pending' AS DeliveryStatus,
                   (
                       CASE
                           WHEN NULLIF(LTRIM(RTRIM(ISNULL(d.DeliveryNumber, ''))), '') IS NULL THEN 'NO-DLV'
                           ELSE LTRIM(RTRIM(d.DeliveryNumber))
                       END
                       + ' | ' +
                       CASE
                           WHEN NULLIF(LTRIM(RTRIM(ISNULL(d.OrderNumber, ''))), '') IS NULL THEN 'NO-ORDER'
                           ELSE LTRIM(RTRIM(d.OrderNumber))
                       END
                       + ' | ' +
                       COALESCE(NULLIF(LTRIM(RTRIM(s.SupplierName)), ''), s.Company)
                       + ' | ' +
                       CONVERT(varchar(10), d.DeliveryDate, 101)
                   ) AS DisplayText
            FROM tbl_Deliveries d
            INNER JOIN tbl_Supplier s ON d.SupplierID = s.SupplierID
            OUTER APPLY (
                SELECT SUM(CASE WHEN ISNULL(dp.Status, 'Pending') = 'Pending' THEN 1 ELSE 0 END) AS PendingItemCount,
                       SUM(CASE WHEN ISNULL(dp.Status, 'Pending') = 'Posted' THEN 1 ELSE 0 END) AS PostedItemCount
                FROM tbl_Delivery_Products dp
                WHERE dp.DeliveryID = d.DeliveryID
            ) lineStats
            WHERE ISNULL(lineStats.PendingItemCount, 0) > 0
              AND ISNULL(lineStats.PostedItemCount, 0) = 0
            ORDER BY d.DateCreated DESC, d.DeliveryID DESC;"

        Return Db.QueryDataTable(sql)
    End Function

    Public Function GetSupplierReturnItemsByDeliveryId(deliveryId As Integer) As DataTable
        Dim sql As String = "
            SELECT dp.DeliveryProductID,
                   dp.ProductID,
                   p.Product,
                   p.BarcodeNumber,
                   ISNULL(dp.Quantity, 0) + ISNULL(dp.ReturnedQty, 0) AS DeliveredQty,
                   ISNULL(dp.ReturnedQty, 0) AS ReturnedQty,
                   (ISNULL(dp.Quantity, 0) + ISNULL(dp.ReturnedQty, 0)) - ISNULL(dp.ReturnedQty, 0) AS MaxReturnable,
                   ISNULL(dp.Status, 'Pending') AS Status,
                   CAST(0 AS int) AS ReturnQty
            FROM tbl_Delivery_Products dp
            INNER JOIN tbl_Products p ON dp.ProductID = p.ProductID
            WHERE dp.DeliveryID = @DeliveryID
              AND ISNULL(dp.Status, 'Pending') = 'Pending'
            ORDER BY p.Product, dp.DeliveryProductID;"

        Return Db.QueryDataTable(sql, New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
    End Function

    Public Function GetSupplierReturnsPage(request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        safeRequest.PageSize = Math.Max(1, safeRequest.PageSize)
        safeRequest.PageIndex = Math.Max(1, safeRequest.PageIndex)

        Dim normalizedSearch As String = If(safeRequest.SearchText, String.Empty).Trim()
        Dim searchValue As String = If(String.IsNullOrWhiteSpace(normalizedSearch), "", "%" & normalizedSearch & "%")

        Dim searchPredicate As String =
            "(@search = '' " &
            " OR sr.ReturnNumber LIKE @search" &
            " OR ISNULL(d.DeliveryNumber, '') LIKE @search" &
            " OR ISNULL(d.OrderNumber, '') LIKE @search" &
            " OR COALESCE(NULLIF(LTRIM(RTRIM(s.SupplierName)), ''), s.Company) LIKE @search" &
            " OR s.Company LIKE @search" &
            " OR EXISTS (" &
            "       SELECT 1" &
            "       FROM tbl_Supplier_Return_Items sriSearch" &
            "       INNER JOIN tbl_Products pSearch ON sriSearch.ProductID = pSearch.ProductID" &
            "       WHERE sriSearch.ReturnID = sr.ReturnID" &
            "         AND (pSearch.Product LIKE @search OR pSearch.BarcodeNumber LIKE @search)" &
            "   )" &
            " )"

        Dim countSql As String = "
            SELECT COUNT(1)
            FROM tbl_Supplier_Returns sr
            INNER JOIN tbl_Deliveries d ON sr.DeliveryID = d.DeliveryID
            INNER JOIN tbl_Supplier s ON sr.SupplierID = s.SupplierID
            WHERE " & searchPredicate & ";"

        Dim totalRecords As Integer = Db.ExecuteScalar(Of Integer)(
            countSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 255) With {.Value = searchValue}
        )

        Dim dataSql As String = "
            SELECT sr.ReturnID,
                   sr.ReturnNumber,
                   COALESCE(NULLIF(LTRIM(RTRIM(s.SupplierName)), ''), s.Company) AS SupplierName,
                   ISNULL(d.DeliveryNumber, '') AS DeliveryNumber,
                   ISNULL(d.OrderNumber, '') AS OrderNumber,
                   d.DeliveryDate,
                   sr.ReturnDate,
                   sr.ReturnType,
                   sr.Resolution,
                   sr.Status,
                   ISNULL(sr.Notes, '') AS Notes,
                   ISNULL(itemTotals.ItemCount, 0) AS ItemCount,
                   ISNULL(itemTotals.TotalReturnedQty, 0) AS TotalReturnedQty
            FROM tbl_Supplier_Returns sr
            INNER JOIN tbl_Deliveries d ON sr.DeliveryID = d.DeliveryID
            INNER JOIN tbl_Supplier s ON sr.SupplierID = s.SupplierID
            OUTER APPLY (
                SELECT COUNT(1) AS ItemCount,
                       SUM(ISNULL(sri.QtyReturned, 0)) AS TotalReturnedQty
                FROM tbl_Supplier_Return_Items sri
                WHERE sri.ReturnID = sr.ReturnID
            ) itemTotals
            WHERE " & searchPredicate & "
            ORDER BY sr.ReturnDate DESC, sr.ReturnID DESC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;"

        Dim rows As DataTable = Db.QueryDataTable(
            dataSql,
            New SqlParameter("@search", SqlDbType.NVarChar, 255) With {.Value = searchValue},
            New SqlParameter("@offset", SqlDbType.Int) With {.Value = safeRequest.Offset},
            New SqlParameter("@pageSize", SqlDbType.Int) With {.Value = safeRequest.PageSize}
        )

        Return PagedQueryResult.Create(rows, totalRecords, safeRequest)
    End Function

    Public Function GetPendingDeliveryProductIdsByDeliveryId(deliveryId As Integer) As List(Of Integer)
        Dim sql As String = "
            SELECT DeliveryProductID
            FROM tbl_Delivery_Products
            WHERE DeliveryID = @DeliveryID
              AND ISNULL(Status, 'Pending') = 'Pending'
            ORDER BY DeliveryProductID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
        Dim ids As New List(Of Integer)()

        For Each row As DataRow In dt.Rows
            If row Is Nothing OrElse row.IsNull("DeliveryProductID") Then Continue For
            ids.Add(Convert.ToInt32(row("DeliveryProductID")))
        Next

        Return ids
    End Function

    Public Function InsertDelivery(supplierId As Integer,
                                   deliveryNumber As String,
                                   orderNumber As String,
                                   deliveryDate As DateTime,
                                   items As IEnumerable(Of DeliveryLineItem)) As Integer
        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction()
                Try
                    Dim newDeliveryId As Integer

                    Dim insertDeliverySql As String =
                        "INSERT INTO tbl_deliveries (SupplierID, DeliveryNumber, OrderNumber, DeliveryDate, DateCreated, Status) " &
                        "VALUES (@SupplierID, @DeliveryNumber, @OrderNumber, @DeliveryDate, GETDATE(), 'Pending'); SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(insertDeliverySql, conn, tran)
                        cmd.Parameters.Add(New SqlParameter("@SupplierID", SqlDbType.Int) With {.Value = supplierId})
                        cmd.Parameters.Add(New SqlParameter("@DeliveryNumber", SqlDbType.NVarChar, 50) With {.Value = deliveryNumber.Trim()})
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
                                   deliveryNumber As String,
                                   orderNumber As String,
                                   deliveryDate As DateTime,
                                   items As IEnumerable(Of DeliveryLineItem)) As Integer
        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction()
                Try
                    Dim updateDeliverySql As String =
                        "UPDATE tbl_deliveries " &
                        "SET SupplierID = @SupplierID, DeliveryNumber = @DeliveryNumber, OrderNumber = @OrderNumber, DeliveryDate = @DeliveryDate " &
                        "WHERE DeliveryID = @DeliveryID;"

                    Using cmd As New SqlCommand(updateDeliverySql, conn, tran)
                        cmd.Parameters.Add(New SqlParameter("@SupplierID", SqlDbType.Int) With {.Value = supplierId})
                        cmd.Parameters.Add(New SqlParameter("@DeliveryNumber", SqlDbType.NVarChar, 50) With {.Value = deliveryNumber.Trim()})
                        cmd.Parameters.Add(New SqlParameter("@OrderNumber", SqlDbType.NVarChar, 50) With {.Value = orderNumber.Trim()})
                        cmd.Parameters.Add(New SqlParameter("@DeliveryDate", SqlDbType.Date) With {.Value = deliveryDate.Date})
                        cmd.Parameters.Add(New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
                        cmd.ExecuteNonQuery()
                    End Using

                    ' Preserve posted rows as immutable history; replace only current pending rows.
                    Using deleteProductsCmd As New SqlCommand("DELETE FROM tbl_Delivery_Products WHERE DeliveryID = @DeliveryID AND ISNULL(Status, 'Pending') = 'Pending';", conn, tran)
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
            "SELECT @DeliveryID, @ProductID, ISNULL(p.CostPrice, 0), @Quantity, GETDATE(), 'Pending' " &
            "FROM dbo.tbl_Products p " &
            "WHERE p.ProductID = @ProductID;"

        For Each item As DeliveryLineItem In items
            Using cmd As New SqlCommand(insertProductSql, conn, tran)
                cmd.Parameters.Add(New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
                cmd.Parameters.Add(New SqlParameter("@ProductID", SqlDbType.Int) With {.Value = item.ProductID})
                cmd.Parameters.Add(New SqlParameter("@Quantity", SqlDbType.Int) With {.Value = item.Quantity})
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                If rowsAffected = 0 Then
                    Throw New InvalidOperationException("Unable to save delivery item because the product record was not found.")
                End If
            End Using
        Next
    End Sub

    Public Function DeleteDeliveryProduct(deliveryProductId As Integer) As Integer
        Dim sql As String = "DELETE FROM tbl_delivery_products WHERE DeliveryProductID = @DeliveryProductID;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@DeliveryProductID", SqlDbType.Int) With {.Value = deliveryProductId})
    End Function

    Public Function DeleteDelivery(deliveryId As Integer) As Integer
        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction()
                Try
                    Using hasPostedCmd As New SqlCommand(
                        "SELECT COUNT(1) FROM tbl_Delivery_Products WHERE DeliveryID = @DeliveryID AND ISNULL(Status, '') = 'Posted';",
                        conn,
                        tran)
                        hasPostedCmd.Parameters.Add(New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
                        Dim postedCount As Integer = Convert.ToInt32(hasPostedCmd.ExecuteScalar())
                        If postedCount > 0 Then
                            Throw New InvalidOperationException("Cannot delete a delivery batch that already contains posted items.")
                        End If
                    End Using

                    Using deleteDetailsCmd As New SqlCommand("DELETE FROM tbl_Delivery_Products WHERE DeliveryID = @DeliveryID;", conn, tran)
                        deleteDetailsCmd.Parameters.Add(New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
                        deleteDetailsCmd.ExecuteNonQuery()
                    End Using

                    Dim affected As Integer
                    Using deleteHeaderCmd As New SqlCommand("DELETE FROM tbl_Deliveries WHERE DeliveryID = @DeliveryID;", conn, tran)
                        deleteHeaderCmd.Parameters.Add(New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
                        affected = deleteHeaderCmd.ExecuteNonQuery()
                    End Using

                    tran.Commit()
                    Return affected
                Catch
                    tran.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Public Function PostDeliveryProduct(deliveryProductId As Integer) As PostDeliveryStatus
        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction(IsolationLevel.Serializable)
                Try
                    Dim productId As Integer
                    Dim quantityToAdd As Integer
                    Dim currentProductCost As Decimal = 0D
                    Dim hasCurrentProductCost As Boolean = False
                    Dim oldQty As Integer = 0

                    Dim getPendingSql As String =
                        "SELECT ProductID, Quantity " &
                        "FROM tbl_delivery_products " &
                        "WITH (UPDLOCK, ROWLOCK) " &
                        "WHERE DeliveryProductID = @DeliveryProductID AND Status = 'Pending';"

                    Using getCmd As New SqlCommand(getPendingSql, conn, tran)
                        getCmd.Parameters.Add(New SqlParameter("@DeliveryProductID", SqlDbType.Int) With {.Value = deliveryProductId})
                        Using rdr As SqlDataReader = getCmd.ExecuteReader()
                            If Not rdr.Read() Then
                                tran.Rollback()
                                Return PostDeliveryStatus.AlreadyPosted
                            End If

                            productId = Convert.ToInt32(rdr("ProductID"))
                            quantityToAdd = If(IsDBNull(rdr("Quantity")), 0, Convert.ToInt32(rdr("Quantity")))
                        End Using
                    End Using

                    If quantityToAdd <= 0 Then
                        Throw New InvalidOperationException("Delivery quantity must be greater than zero before posting.")
                    End If

                    ' True on-hand in this system = posted delivery qty - sold qty.
                    Using getOnHandQtyCmd As New SqlCommand(
                        "SELECT " &
                        "    ISNULL((SELECT SUM(Quantity) " &
                        "            FROM tbl_Delivery_Products WITH (UPDLOCK, HOLDLOCK) " &
                        "            WHERE ProductID = @ProductID AND Status = 'Posted'), 0) " &
                        "    - " &
                        "    ISNULL((SELECT SUM(Quantity) " &
                        "            FROM tbl_SalesHistory WITH (UPDLOCK, HOLDLOCK) " &
                        "            WHERE ProductID = @ProductID), 0);",
                        conn,
                        tran)

                        getOnHandQtyCmd.Parameters.Add(New SqlParameter("@ProductID", SqlDbType.Int) With {.Value = productId})
                        Dim onHandObj As Object = getOnHandQtyCmd.ExecuteScalar()
                        oldQty = If(onHandObj Is Nothing OrElse IsDBNull(onHandObj), 0, Convert.ToInt32(onHandObj))
                    End Using

                    Using getProductCostCmd As New SqlCommand("SELECT CostPrice FROM dbo.tbl_products WITH (UPDLOCK, ROWLOCK) WHERE ProductID = @ProductID;", conn, tran)
                        getProductCostCmd.Parameters.Add(New SqlParameter("@ProductID", SqlDbType.Int) With {.Value = productId})
                        Dim productCostObj As Object = getProductCostCmd.ExecuteScalar()
                        If productCostObj IsNot Nothing AndAlso Not IsDBNull(productCostObj) Then
                            currentProductCost = Convert.ToDecimal(productCostObj)
                            hasCurrentProductCost = True
                        End If
                    End Using

                    Dim deliveryCost As Decimal = If(hasCurrentProductCost, currentProductCost, 0D)
                    Dim oldCost As Decimal = If(hasCurrentProductCost, currentProductCost, deliveryCost)
                    Dim totalQty As Integer = oldQty + quantityToAdd
                    Dim weightedCost As Decimal

                    If totalQty > 0 Then
                        weightedCost = Math.Round(((oldQty * oldCost) + (quantityToAdd * deliveryCost)) / totalQty, 2)
                    ElseIf oldQty > 0 Then
                        weightedCost = oldCost
                    Else
                        weightedCost = deliveryCost
                    End If

                    Using updateProductCostCmd As New SqlCommand("UPDATE dbo.tbl_products SET CostPrice = @NewWeightedCost WHERE ProductID = @ProductID;", conn, tran)
                        Dim newWeightedCostParam As New SqlParameter("@NewWeightedCost", SqlDbType.Decimal)
                        newWeightedCostParam.Precision = 18
                        newWeightedCostParam.Scale = 2
                        newWeightedCostParam.Value = weightedCost
                        updateProductCostCmd.Parameters.Add(newWeightedCostParam)
                        updateProductCostCmd.Parameters.Add(New SqlParameter("@ProductID", SqlDbType.Int) With {.Value = productId})

                        Dim updatedRows As Integer = updateProductCostCmd.ExecuteNonQuery()
                        If updatedRows = 0 Then
                            Throw New InvalidOperationException("Product not found while updating cost price.")
                        End If
                    End Using

                    ' Keep delivery rows immutable history: mark pending row as posted, do not merge into another row.
                    Using postCmd As New SqlCommand("UPDATE tbl_delivery_products SET Status = 'Posted', DateUpdated = GETDATE() WHERE DeliveryProductID = @DeliveryProductID AND Status = 'Pending';", conn, tran)
                        postCmd.Parameters.Add(New SqlParameter("@DeliveryProductID", SqlDbType.Int) With {.Value = deliveryProductId})
                        Dim rowsAffected As Integer = postCmd.ExecuteNonQuery()
                        If rowsAffected = 0 Then
                            tran.Rollback()
                            Return PostDeliveryStatus.AlreadyPosted
                        End If
                    End Using

                    tran.Commit()
                    Return PostDeliveryStatus.Success
                Catch
                    tran.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Public Function SaveSupplierReturn(deliveryId As Integer,
                                       returnDate As DateTime,
                                       returnType As String,
                                       resolution As String,
                                       notes As String,
                                       items As IEnumerable(Of SupplierReturnLineItem)) As SupplierReturnSaveResult
        Dim normalizedReturnType As String = If(returnType, String.Empty).Trim()
        Dim normalizedResolution As String = If(resolution, String.Empty).Trim()
        Dim normalizedNotes As String = If(notes, String.Empty).Trim()
        Dim normalizedItems As New List(Of SupplierReturnLineItem)()

        If items IsNot Nothing Then
            For Each item As SupplierReturnLineItem In items
                If item Is Nothing Then Continue For
                normalizedItems.Add(item)
            Next
        End If

        If deliveryId <= 0 Then
            Throw New InvalidOperationException("A saved delivery is required before creating a supplier return.")
        End If
        If String.IsNullOrWhiteSpace(normalizedReturnType) Then
            Throw New InvalidOperationException("Return Type is required.")
        End If
        If String.IsNullOrWhiteSpace(normalizedResolution) Then
            Throw New InvalidOperationException("Resolution is required.")
        End If
        If normalizedItems.Count = 0 Then
            Throw New InvalidOperationException("Add at least one return item.")
        End If

        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Using tran As SqlTransaction = conn.BeginTransaction(IsolationLevel.Serializable)
                Try
                    Dim supplierId As Integer
                    Dim deliveryDate As DateTime
                    Dim acceptsReturnRefund As Boolean
                    Dim returnWindowDays As Integer? = Nothing
                    Dim pendingItemCount As Integer = 0
                    Dim postedItemCount As Integer = 0

                    Dim headerSql As String = "
                        SELECT d.SupplierID,
                               d.DeliveryDate,
                               ISNULL(s.AcceptsReturnRefund, 0) AS AcceptsReturnRefund,
                               s.ReturnWindowDays,
                               ISNULL(lineStats.PendingItemCount, 0) AS PendingItemCount,
                               ISNULL(lineStats.PostedItemCount, 0) AS PostedItemCount
                        FROM tbl_Deliveries d WITH (UPDLOCK, HOLDLOCK)
                        INNER JOIN tbl_Supplier s WITH (UPDLOCK, HOLDLOCK) ON d.SupplierID = s.SupplierID
                        OUTER APPLY (
                            SELECT SUM(CASE WHEN ISNULL(dp.Status, 'Pending') = 'Pending' THEN 1 ELSE 0 END) AS PendingItemCount,
                                   SUM(CASE WHEN ISNULL(dp.Status, 'Pending') = 'Posted' THEN 1 ELSE 0 END) AS PostedItemCount
                            FROM tbl_Delivery_Products dp
                            WHERE dp.DeliveryID = d.DeliveryID
                        ) lineStats
                        WHERE d.DeliveryID = @DeliveryID;"

                    Using headerCmd As New SqlCommand(headerSql, conn, tran)
                        headerCmd.Parameters.Add(New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
                        Using rdr As SqlDataReader = headerCmd.ExecuteReader()
                            If Not rdr.Read() Then
                                Throw New InvalidOperationException("Selected delivery record was not found.")
                            End If

                            supplierId = Convert.ToInt32(rdr("SupplierID"))
                            deliveryDate = Convert.ToDateTime(rdr("DeliveryDate")).Date
                            acceptsReturnRefund = Convert.ToBoolean(rdr("AcceptsReturnRefund"))
                            If Not IsDBNull(rdr("ReturnWindowDays")) Then
                                returnWindowDays = Convert.ToInt32(rdr("ReturnWindowDays"))
                            End If
                            pendingItemCount = Convert.ToInt32(rdr("PendingItemCount"))
                            postedItemCount = Convert.ToInt32(rdr("PostedItemCount"))
                        End Using
                    End Using

                    If postedItemCount > 0 OrElse pendingItemCount <= 0 Then
                        Throw New InvalidOperationException("Cannot return/refund Posted deliveries.")
                    End If

                    If Not acceptsReturnRefund Then
                        Throw New InvalidOperationException("This supplier does not accept return/refund requests.")
                    End If

                    If returnWindowDays.HasValue Then
                        Dim expiryDate As DateTime = deliveryDate.AddDays(returnWindowDays.Value)
                        If DateTime.Today.Date > expiryDate.Date Then
                            Throw New InvalidOperationException($"The supplier return window expired on {expiryDate:MMMM d, yyyy}.")
                        End If
                    End If

                    Dim sanitizedItems As New List(Of SupplierReturnLineItem)()
                    For Each item As SupplierReturnLineItem In normalizedItems
                        If item.DeliveryProductID <= 0 OrElse item.ProductID <= 0 Then Continue For
                        If item.Quantity <= 0 Then Continue For
                        sanitizedItems.Add(item)
                    Next

                    If sanitizedItems.Count = 0 Then
                        Throw New InvalidOperationException("Return quantity must be greater than 0.")
                    End If

                    Dim returnNumber As String = GenerateSupplierReturnNumber(conn, tran)
                    Dim returnId As Integer

                    Dim insertHeaderSql As String =
                        "INSERT INTO tbl_Supplier_Returns (ReturnNumber, SupplierID, DeliveryID, ReturnDate, ReturnType, Resolution, Notes, Status, DateCreated) " &
                        "VALUES (@ReturnNumber, @SupplierID, @DeliveryID, @ReturnDate, @ReturnType, @Resolution, @Notes, 'Completed', GETDATE()); " &
                        "SELECT SCOPE_IDENTITY();"

                    Using insertHeaderCmd As New SqlCommand(insertHeaderSql, conn, tran)
                        insertHeaderCmd.Parameters.Add(New SqlParameter("@ReturnNumber", SqlDbType.VarChar, 50) With {.Value = returnNumber})
                        insertHeaderCmd.Parameters.Add(New SqlParameter("@SupplierID", SqlDbType.Int) With {.Value = supplierId})
                        insertHeaderCmd.Parameters.Add(New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
                        insertHeaderCmd.Parameters.Add(New SqlParameter("@ReturnDate", SqlDbType.DateTime) With {.Value = returnDate})
                        insertHeaderCmd.Parameters.Add(New SqlParameter("@ReturnType", SqlDbType.NVarChar, 30) With {.Value = normalizedReturnType})
                        insertHeaderCmd.Parameters.Add(New SqlParameter("@Resolution", SqlDbType.NVarChar, 30) With {.Value = normalizedResolution})
                        insertHeaderCmd.Parameters.Add(New SqlParameter("@Notes", SqlDbType.NVarChar, 250) With {.Value = If(normalizedNotes.Length > 0, CType(normalizedNotes, Object), DBNull.Value)})
                        returnId = Convert.ToInt32(insertHeaderCmd.ExecuteScalar())
                    End Using

                    Dim appliedItems As New List(Of SupplierReturnAppliedItem)()

                    For Each item As SupplierReturnLineItem In sanitizedItems
                        Dim dbProductId As Integer
                        Dim currentQty As Integer
                        Dim currentReturnedQty As Integer
                        Dim productName As String = "Selected product"
                        Dim statusText As String

                        Dim getLineSql As String = "
                            SELECT dp.ProductID,
                                   p.Product,
                                   ISNULL(dp.Quantity, 0) AS Quantity,
                                   ISNULL(dp.ReturnedQty, 0) AS ReturnedQty,
                                   ISNULL(dp.Status, 'Pending') AS Status
                            FROM tbl_Delivery_Products dp WITH (UPDLOCK, HOLDLOCK, ROWLOCK)
                            INNER JOIN tbl_Products p ON dp.ProductID = p.ProductID
                            WHERE dp.DeliveryProductID = @DeliveryProductID
                              AND dp.DeliveryID = @DeliveryID;"

                        Using getLineCmd As New SqlCommand(getLineSql, conn, tran)
                            getLineCmd.Parameters.Add(New SqlParameter("@DeliveryProductID", SqlDbType.Int) With {.Value = item.DeliveryProductID})
                            getLineCmd.Parameters.Add(New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
                            Using rdr As SqlDataReader = getLineCmd.ExecuteReader()
                                If Not rdr.Read() Then
                                    Throw New InvalidOperationException("One of the selected delivery products no longer exists.")
                                End If

                                dbProductId = Convert.ToInt32(rdr("ProductID"))
                                productName = Convert.ToString(rdr("Product")).Trim()
                                currentQty = Convert.ToInt32(rdr("Quantity"))
                                currentReturnedQty = Convert.ToInt32(rdr("ReturnedQty"))
                                statusText = Convert.ToString(rdr("Status")).Trim()
                            End Using
                        End Using

                        If Not String.Equals(statusText, "Pending", StringComparison.OrdinalIgnoreCase) Then
                            Throw New InvalidOperationException("Cannot return/refund Posted deliveries.")
                        End If

                        If item.ProductID <> dbProductId Then
                            Throw New InvalidOperationException($"Product mismatch detected for {productName}.")
                        End If

                        If item.Quantity <= 0 Then
                            Throw New InvalidOperationException($"Return quantity must be greater than 0 for {productName}.")
                        End If

                        If item.Quantity > currentQty Then
                            Throw New InvalidOperationException($"{productName} exceeds the remaining returnable quantity ({currentQty}).")
                        End If

                        Dim updateLineSql As String =
                            "UPDATE tbl_Delivery_Products " &
                            "SET ReturnedQty = ReturnedQty + @ReturnQty, " &
                            "    Quantity = Quantity - @ReturnQty, " &
                            "    DateUpdated = GETDATE() " &
                            "WHERE DeliveryProductID = @DeliveryProductID " &
                            "  AND DeliveryID = @DeliveryID " &
                            "  AND ISNULL(Status, 'Pending') = 'Pending' " &
                            "  AND ISNULL(Quantity, 0) >= @ReturnQty;"

                        Using updateLineCmd As New SqlCommand(updateLineSql, conn, tran)
                            updateLineCmd.Parameters.Add(New SqlParameter("@ReturnQty", SqlDbType.Int) With {.Value = item.Quantity})
                            updateLineCmd.Parameters.Add(New SqlParameter("@DeliveryProductID", SqlDbType.Int) With {.Value = item.DeliveryProductID})
                            updateLineCmd.Parameters.Add(New SqlParameter("@DeliveryID", SqlDbType.Int) With {.Value = deliveryId})
                            Dim rowsAffected As Integer = updateLineCmd.ExecuteNonQuery()
                            If rowsAffected = 0 Then
                                Dim maxReturnable As Integer = Math.Max(0, currentQty)
                                Throw New InvalidOperationException($"{productName} exceeds the remaining returnable quantity ({maxReturnable}).")
                            End If
                        End Using

                        Dim insertItemSql As String =
                            "INSERT INTO tbl_Supplier_Return_Items (ReturnID, DeliveryProductID, ProductID, QtyReturned, Reason) " &
                            "VALUES (@ReturnID, @DeliveryProductID, @ProductID, @QtyReturned, @Reason);"

                        Using insertItemCmd As New SqlCommand(insertItemSql, conn, tran)
                            insertItemCmd.Parameters.Add(New SqlParameter("@ReturnID", SqlDbType.Int) With {.Value = returnId})
                            insertItemCmd.Parameters.Add(New SqlParameter("@DeliveryProductID", SqlDbType.Int) With {.Value = item.DeliveryProductID})
                            insertItemCmd.Parameters.Add(New SqlParameter("@ProductID", SqlDbType.Int) With {.Value = dbProductId})
                            insertItemCmd.Parameters.Add(New SqlParameter("@QtyReturned", SqlDbType.Int) With {.Value = item.Quantity})
                            insertItemCmd.Parameters.Add(New SqlParameter("@Reason", SqlDbType.NVarChar, 100) With {.Value = normalizedReturnType})
                            insertItemCmd.ExecuteNonQuery()
                        End Using

                        appliedItems.Add(New SupplierReturnAppliedItem With {
                            .DeliveryProductID = item.DeliveryProductID,
                            .QuantityReturned = item.Quantity
                        })
                    Next

                    tran.Commit()

                    Return New SupplierReturnSaveResult With {
                        .ReturnID = returnId,
                        .ReturnNumber = returnNumber,
                        .AppliedItems = appliedItems
                    }
                Catch
                    tran.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Private Function GenerateSupplierReturnNumber(conn As SqlConnection, tran As SqlTransaction) As String
        Dim nextSequence As Integer

        Using cmd As New SqlCommand("SELECT ISNULL(MAX(ReturnID), 0) + 1 FROM tbl_Supplier_Returns WITH (UPDLOCK, HOLDLOCK);", conn, tran)
            nextSequence = Convert.ToInt32(cmd.ExecuteScalar())
        End Using

        Return $"SR-{nextSequence:000000}"
    End Function

End Class
