Imports System.Data
Imports System.Collections.Generic

Public Class DeliveriesService
    Private ReadOnly _repo As New DeliveriesRepository()
    Private ReadOnly _supplierRepo As New SupplierRepository()

    Public Function GetSupplierLookup() As DataTable
        Return _supplierRepo.GetSupplierLookup()
    End Function

    Public Function GetPendingDeliveryProducts(searchText As String) As DataTable
        Return _repo.GetPendingDeliveryProducts(searchText)
    End Function

    Public Function GetPendingDeliveryProducts(searchText As String, pageNumber As Integer, pageSize As Integer) As DeliveryPagedResult
        Return _repo.GetPendingDeliveryProducts(searchText, pageNumber, pageSize)
    End Function

    Public Function GetDeliveryHeaderById(deliveryId As Integer) As DataRow
        Return _repo.GetDeliveryHeaderById(deliveryId)
    End Function

    Public Function DeliveryNumberExists(deliveryNumber As String, Optional excludeDeliveryId As Integer? = Nothing) As Boolean
        Return _repo.DeliveryNumberExists(deliveryNumber, excludeDeliveryId)
    End Function

    Public Function GetDeliveryProductsByDeliveryId(deliveryId As Integer) As DataTable
        Return _repo.GetDeliveryProductsByDeliveryId(deliveryId)
    End Function

    Public Function GetSupplierReturnContextByDeliveryId(deliveryId As Integer) As DataRow
        Return _repo.GetSupplierReturnContextByDeliveryId(deliveryId)
    End Function

    Public Function GetPendingSupplierReturnDeliveryLookup() As DataTable
        Return _repo.GetPendingSupplierReturnDeliveryLookup()
    End Function

    Public Function GetSupplierReturnItemsByDeliveryId(deliveryId As Integer) As DataTable
        Return _repo.GetSupplierReturnItemsByDeliveryId(deliveryId)
    End Function

    Public Function GetSupplierReturnsPage(request As PagedQueryRequest) As PagedQueryResult
        Return _repo.GetSupplierReturnsPage(request)
    End Function

    Public Sub DeleteDelivery(deliveryId As Integer)
        _repo.DeleteDelivery(deliveryId)
    End Sub

    Public Function SaveDelivery(mode As EntryFormMode,
                                 selectedId As Integer,
                                 supplierId As Integer,
                                 deliveryNumber As String,
                                 orderNumber As String,
                                 deliveryDate As DateTime,
                                 pendingItems As DataTable) As Integer
        Dim normalizedDeliveryNumber As String = If(deliveryNumber, String.Empty).Trim()
        If String.IsNullOrWhiteSpace(normalizedDeliveryNumber) Then
            Throw New ArgumentException("Delivery Number cannot be empty.")
        End If

        Dim excludeDeliveryId As Integer? = Nothing
        If mode = EntryFormMode.EditExisting AndAlso selectedId > 0 Then
            excludeDeliveryId = selectedId
        End If

        If _repo.DeliveryNumberExists(normalizedDeliveryNumber, excludeDeliveryId) Then
            Throw New InvalidOperationException("Delivery Number already exists.")
        End If

        Dim items As List(Of DeliveryLineItem) = MapDeliveryItems(pendingItems)

        If mode = EntryFormMode.EditExisting AndAlso selectedId > 0 Then
            Return _repo.UpdateDelivery(selectedId, supplierId, normalizedDeliveryNumber, orderNumber, deliveryDate, items)
        End If

        Return _repo.InsertDelivery(supplierId, normalizedDeliveryNumber, orderNumber, deliveryDate, items)
    End Function

    Public Sub DeleteDeliveryProduct(deliveryProductId As Integer)
        _repo.DeleteDeliveryProduct(deliveryProductId)
    End Sub

    Public Function PostDeliveryBatch(deliveryId As Integer) As Integer
        Dim pendingIds As List(Of Integer) = _repo.GetPendingDeliveryProductIdsByDeliveryId(deliveryId)
        Dim postedCount As Integer = 0

        For Each deliveryProductId As Integer In pendingIds
            Dim status As PostDeliveryStatus = _repo.PostDeliveryProduct(deliveryProductId)
            If status = PostDeliveryStatus.Success Then
                postedCount += 1
            End If
        Next

        Return postedCount
    End Function

    Public Function PostDeliveryProduct(deliveryProductId As Integer) As PostDeliveryStatus
        Return _repo.PostDeliveryProduct(deliveryProductId)
    End Function

    Public Function SaveSupplierReturn(deliveryId As Integer,
                                       returnDate As DateTime,
                                       returnType As String,
                                       resolution As String,
                                       notes As String,
                                       items As IEnumerable(Of SupplierReturnLineItem)) As SupplierReturnSaveResult
        Return _repo.SaveSupplierReturn(deliveryId, returnDate, returnType, resolution, notes, items)
    End Function

    Private Function MapDeliveryItems(pendingItems As DataTable) As List(Of DeliveryLineItem)
        Dim items As New List(Of DeliveryLineItem)()
        If pendingItems Is Nothing Then Return items

        For Each row As DataRow In pendingItems.Rows
            If row Is Nothing OrElse row.RowState = DataRowState.Deleted Then Continue For

            If pendingItems.Columns.Contains("Status") Then
                Dim statusText As String = Convert.ToString(row("Status")).Trim()
                If String.Equals(statusText, "Posted", StringComparison.OrdinalIgnoreCase) Then
                    Continue For
                End If
            End If

            items.Add(New DeliveryLineItem With {
                .ProductID = Convert.ToInt32(row("ProductID")),
                .Quantity = Convert.ToInt32(row("Quantity"))
            })
        Next

        Return items
    End Function
End Class
