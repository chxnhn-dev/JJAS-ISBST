Imports System.Data

Public Class ColorService
    Private ReadOnly _repo As New ColorRepository()

    Public Function GetColors(searchText As String) As DataTable
        Return _repo.GetActiveColors(searchText)
    End Function

    Public Function GetColorsPage(request As PagedQueryRequest) As PagedQueryResult
        Return _repo.GetActiveColorsPage(request)
    End Function

    Public Function GetColorById(colorId As Integer) As DataRow
        Return _repo.GetColorById(colorId)
    End Function

    Public Function IsDuplicateName(colorName As String, Optional excludeColorId As Integer? = Nothing) As Boolean
        Return _repo.ExistsByName(colorName, excludeColorId)
    End Function

    Public Sub SaveColor(mode As EntryFormMode, selectedId As Integer, colorName As String)
        Dim now As DateTime = DateTime.Now
        If mode = EntryFormMode.EditExisting AndAlso selectedId > 0 Then
            _repo.UpdateColor(selectedId, colorName, now)
        Else
            _repo.InsertColor(colorName, now)
        End If
    End Sub

    Public Function CanDelete(colorId As Integer) As Boolean
        Return Not _repo.IsUsedByProducts(colorId)
    End Function

    Public Sub DeleteColor(colorId As Integer)
        _repo.DeleteColor(colorId)
    End Sub
End Class
