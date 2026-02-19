Imports System.Data

Public Class BrandService
    Private ReadOnly _repo As New BrandRepository()

    Public Function GetBrands(searchText As String) As DataTable
        Return _repo.GetActiveBrands(searchText)
    End Function

    Public Function GetBrandById(brandId As Integer) As DataRow
        Return _repo.GetBrandById(brandId)
    End Function

    Public Function IsDuplicateName(brandName As String, Optional excludeBrandId As Integer? = Nothing) As Boolean
        Return _repo.ExistsByName(brandName, excludeBrandId)
    End Function

    Public Sub SaveBrand(mode As EntryFormMode, selectedId As Integer, brandName As String)
        Dim now As DateTime = DateTime.Now
        If mode = EntryFormMode.EditExisting AndAlso selectedId > 0 Then
            _repo.UpdateBrand(selectedId, brandName, now)
        Else
            _repo.InsertBrand(brandName, now)
        End If
    End Sub

    Public Function CanDelete(brandId As Integer) As Boolean
        Return Not _repo.IsUsedByProducts(brandId)
    End Function

    Public Sub DeleteBrand(brandId As Integer)
        _repo.DeleteBrand(brandId)
    End Sub
End Class
