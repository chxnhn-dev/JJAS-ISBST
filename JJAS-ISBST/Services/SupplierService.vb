Imports System.Data

Public Class SupplierService
    Private ReadOnly _repo As New SupplierRepository()

    Public Function GetSuppliers(searchText As String) As DataTable
        Return _repo.GetActiveSuppliers(searchText)
    End Function

    Public Function GetSuppliersPage(request As PagedQueryRequest) As PagedQueryResult
        Return _repo.GetActiveSuppliersPage(request)
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

    Public Sub SaveSupplier(mode As EntryFormMode,
                            selectedId As Integer,
                            supplierName As String,
                            contactNumber As String,
                            address As String,
                            acceptsReturnRefund As Boolean,
                            returnWindowDays As Integer?)
        If mode = EntryFormMode.EditExisting AndAlso selectedId > 0 Then
            _repo.UpdateSupplier(selectedId, supplierName, contactNumber, address, acceptsReturnRefund, returnWindowDays)
        Else
            _repo.InsertSupplier(supplierName, contactNumber, address, acceptsReturnRefund, returnWindowDays)
        End If
    End Sub

    Public Sub DeactivateSupplier(supplierId As Integer)
        _repo.DeactivateSupplier(supplierId)
    End Sub
End Class
