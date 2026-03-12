Imports System.Data

Public Class DiscountService
    Private ReadOnly _repo As New DiscountRepository()

    Public Function GetDiscounts(searchText As String) As DataTable
        Return _repo.GetActiveDiscounts(searchText)
    End Function

    Public Function GetDiscountsPage(request As PagedQueryRequest) As PagedQueryResult
        Return _repo.GetActiveDiscountsPage(request)
    End Function

    Public Function GetDiscountById(discountId As Integer) As DataRow
        Return _repo.GetDiscountById(discountId)
    End Function

    Public Function IsDuplicateName(discountName As String, Optional excludeDiscountId As Integer? = Nothing) As Boolean
        Return _repo.ExistsByName(discountName, excludeDiscountId)
    End Function

    Public Sub SaveDiscount(mode As EntryFormMode, selectedId As Integer, discountName As String, discountValue As Decimal, description As String)
        Dim now As DateTime = DateTime.Now
        If mode = EntryFormMode.EditExisting AndAlso selectedId > 0 Then
            _repo.UpdateDiscount(selectedId, discountName, discountValue, description, now)
        Else
            _repo.InsertDiscount(discountName, discountValue, description, now)
        End If
    End Sub

    Public Sub DeleteDiscount(discountId As Integer)
        _repo.DeleteDiscount(discountId)
    End Sub
End Class
