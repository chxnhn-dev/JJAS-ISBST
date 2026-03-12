Public Class AuditTrailService
    Private ReadOnly _repo As New AuditTrailRepository()

    Public Function GetAuditTrailPage(request As PagedQueryRequest) As PagedQueryResult
        Return _repo.GetAuditTrailPage(request)
    End Function
End Class
