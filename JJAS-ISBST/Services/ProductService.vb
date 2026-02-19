Imports System.Data

Public Class ProductService
    Private ReadOnly _repo As New ProductRepository()

    Public Function GetActiveProducts(searchText As String) As DataTable
        Return _repo.GetActiveProducts(searchText)
    End Function

    Public Function GetProductById(productId As Integer) As DataRow
        Return _repo.GetProductById(productId)
    End Function

    Public Function FindFirstActiveProductByBarcode(searchText As String) As DataRow
        Return _repo.FindFirstActiveProductByBarcode(searchText)
    End Function
End Class
