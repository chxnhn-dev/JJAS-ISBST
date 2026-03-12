Public Class VatService
    Private ReadOnly _repo As New VatRepository()

    Public Function GetVatPage(request As PagedQueryRequest) As PagedQueryResult
        Return _repo.GetVatPage(request)
    End Function
End Class
