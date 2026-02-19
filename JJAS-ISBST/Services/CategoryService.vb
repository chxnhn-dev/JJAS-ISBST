Imports System.Data

Public Class CategoryService
    Private ReadOnly _repo As New CategoryRepository()

    Public Function GetCategories(searchText As String) As DataTable
        Return _repo.GetActiveCategories(searchText)
    End Function

    Public Function GetCategoryById(categoryId As Integer) As DataRow
        Return _repo.GetCategoryById(categoryId)
    End Function

    Public Function IsDuplicateName(categoryName As String, Optional excludeCategoryId As Integer? = Nothing) As Boolean
        Return _repo.ExistsByName(categoryName, excludeCategoryId)
    End Function

    Public Sub SaveCategory(mode As EntryFormMode, selectedId As Integer, categoryName As String)
        Dim now As DateTime = DateTime.Now
        If mode = EntryFormMode.EditExisting AndAlso selectedId > 0 Then
            _repo.UpdateCategory(selectedId, categoryName, now)
        Else
            _repo.InsertCategory(categoryName, now)
        End If
    End Sub

    Public Function CanDelete(categoryId As Integer) As Boolean
        Return Not _repo.IsUsedByProducts(categoryId)
    End Function

    Public Sub DeleteCategory(categoryId As Integer)
        _repo.DeleteCategory(categoryId)
    End Sub
End Class
