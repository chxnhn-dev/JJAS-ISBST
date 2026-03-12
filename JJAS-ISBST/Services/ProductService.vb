Imports System.Data

Public Class ProductService
    Private ReadOnly _repo As New ProductRepository()

    Public Function GetActiveProducts(searchText As String) As DataTable
        Return _repo.GetActiveProducts(searchText)
    End Function

    Public Function GetActiveProductsPage(request As PagedQueryRequest) As PagedQueryResult
        Return _repo.GetActiveProductsPage(request)
    End Function

    Public Function GetProductById(productId As Integer) As DataRow
        Return _repo.GetProductById(productId)
    End Function

    Public Function GetProductUsageInfo(productId As Integer) As ProductUsageInfo
        Return _repo.GetProductUsageInfo(productId)
    End Function

    Public Sub SaveProduct(mode As EntryFormMode, selectedId As Integer, request As ProductSaveRequest)
        If mode = EntryFormMode.EditExisting AndAlso selectedId > 0 Then
            _repo.UpdateProduct(selectedId, request)
        Else
            _repo.InsertProduct(request)
        End If
    End Sub

    Public Function FindFirstActiveProductByBarcode(searchText As String) As DataRow
        Return _repo.FindFirstActiveProductByBarcode(searchText)
    End Function
End Class
