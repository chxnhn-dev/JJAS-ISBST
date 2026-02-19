Imports System.Data

Public Class SupplierService
    Private ReadOnly _repo As New SupplierRepository()

    Public Function GetSuppliers(searchText As String) As DataTable
        Return _repo.GetActiveSuppliers(searchText)
    End Function

    Public Function GetSupplierLookup() As DataTable
        Return _repo.GetSupplierLookup()
    End Function

    Public Function GetSupplierById(supplierId As Integer) As DataRow
        Return _repo.GetSupplierById(supplierId)
    End Function

    Public Function CompanyExists(companyName As String, Optional excludeSupplierId As Integer? = Nothing) As Boolean
        Return _repo.CompanyExists(companyName, excludeSupplierId)
    End Function

    Public Sub SaveSupplier(mode As EntryFormMode, selectedId As Integer, company As String, contactNumber As String, address As String)
        If mode = EntryFormMode.EditExisting AndAlso selectedId > 0 Then
            _repo.UpdateSupplier(selectedId, company, contactNumber, address)
        Else
            _repo.InsertSupplier(company, contactNumber, address)
        End If
    End Sub

    Public Sub DeactivateSupplier(supplierId As Integer)
        _repo.DeactivateSupplier(supplierId)
    End Sub
End Class
