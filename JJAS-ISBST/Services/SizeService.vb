Imports System.Data

Public Class SizeService
    Private ReadOnly _repo As New SizeRepository()

    Public Function GetSizes(searchText As String) As DataTable
        Return _repo.GetActiveSizes(searchText)
    End Function

    Public Function GetSizeById(sizeId As Integer) As DataRow
        Return _repo.GetSizeById(sizeId)
    End Function

    Public Function IsDuplicateName(sizeName As String, Optional excludeSizeId As Integer? = Nothing) As Boolean
        Return _repo.ExistsByName(sizeName, excludeSizeId)
    End Function

    Public Sub SaveSize(mode As EntryFormMode, selectedId As Integer, sizeName As String, description As String)
        Dim now As DateTime = DateTime.Now
        If mode = EntryFormMode.EditExisting AndAlso selectedId > 0 Then
            _repo.UpdateSize(selectedId, sizeName, description, now)
        Else
            _repo.InsertSize(sizeName, description, now)
        End If
    End Sub

    Public Function CanDelete(sizeId As Integer) As Boolean
        Return Not _repo.IsUsedByProducts(sizeId)
    End Function

    Public Sub DeleteSize(sizeId As Integer)
        _repo.DeleteSize(sizeId)
    End Sub
End Class
