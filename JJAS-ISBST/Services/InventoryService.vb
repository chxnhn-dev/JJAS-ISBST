Public Class InventoryService
    Private ReadOnly _repo As New InventoryRepository()

    Public Function GetPostedInventoryPage(request As PagedQueryRequest) As PagedQueryResult
        Return _repo.GetPostedInventoryPage(request)
    End Function
End Class
