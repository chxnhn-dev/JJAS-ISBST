Imports System.Data

Public Class DeliveriesService
    Private ReadOnly _repo As New DeliveriesRepository()
    Private ReadOnly _supplierRepo As New SupplierRepository()

    Public Function GetSupplierLookup() As DataTable
        Return _supplierRepo.GetSupplierLookup()
    End Function

    Public Function GetPendingDeliveryProducts(searchText As String) As DataTable
        Return _repo.GetPendingDeliveryProducts(searchText)
    End Function

    Public Function GetDeliveryHeaderById(deliveryId As Integer) As DataRow
        Return _repo.GetDeliveryHeaderById(deliveryId)
    End Function

    Public Function GetDeliveryProductsByDeliveryId(deliveryId As Integer) As DataTable
        Return _repo.GetDeliveryProductsByDeliveryId(deliveryId)
    End Function

    Public Function SaveDelivery(mode As EntryFormMode,
                                 selectedId As Integer,
                                 supplierId As Integer,
                                 orderNumber As String,
                                 deliveryDate As DateTime,
                                 pendingItems As DataTable) As Integer
        Dim items As List(Of DeliveryLineItem) = MapDeliveryItems(pendingItems)

        If mode = EntryFormMode.EditExisting AndAlso selectedId > 0 Then
            Return _repo.UpdateDelivery(selectedId, supplierId, orderNumber, deliveryDate, items)
        End If

        Return _repo.InsertDelivery(supplierId, orderNumber, deliveryDate, items)
    End Function

    Public Sub DeleteDeliveryProduct(deliveryProductId As Integer)
        _repo.DeleteDeliveryProduct(deliveryProductId)
    End Sub

    Public Function PostDeliveryProduct(deliveryProductId As Integer) As PostDeliveryStatus
        Return _repo.PostDeliveryProduct(deliveryProductId)
    End Function

    Private Function MapDeliveryItems(pendingItems As DataTable) As List(Of DeliveryLineItem)
        Dim items As New List(Of DeliveryLineItem)()
        If pendingItems Is Nothing Then Return items

        For Each row As DataRow In pendingItems.Rows
            items.Add(New DeliveryLineItem With {
                .ProductID = Convert.ToInt32(row("ProductID")),
                .Quantity = Convert.ToInt32(row("Quantity")),
                .CostPrice = Convert.ToDecimal(row("CostPrice"))
            })
        Next

        Return items
    End Function
End Class
