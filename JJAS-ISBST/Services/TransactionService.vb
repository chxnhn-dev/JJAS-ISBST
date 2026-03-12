Imports System.Data

Public Class TransactionService
    Private ReadOnly _repo As New TransactionRepository()

    Public Function GetCashierDashboardSummary(cashierUserId As Integer) As CashierDashboardSummary
        Return _repo.GetCashierDashboardSummary(cashierUserId)
    End Function

    Public Function GetRecentCashierTransactions(cashierUserId As Integer, maxRows As Integer) As DataTable
        Return _repo.GetRecentCashierTransactions(cashierUserId, maxRows)
    End Function

    Public Function GetCashierStockMovement(cashierUserId As Integer, maxRows As Integer) As DataTable
        Return _repo.GetCashierStockMovement(cashierUserId, maxRows)
    End Function

    Public Function GetStaffDashboardSummary() As StaffDashboardSummary
        Return _repo.GetStaffDashboardSummary()
    End Function

    Public Function GetLowStockItems(maxRows As Integer, Optional stockThreshold As Integer = 10) As DataTable
        Return _repo.GetLowStockItems(maxRows, stockThreshold)
    End Function

    Public Function GetStockMovement(maxRows As Integer) As DataTable
        Return _repo.GetStockMovement(maxRows)
    End Function

    Public Function GetSalesHistoryPage(request As PagedQueryRequest) As PagedQueryResult
        Return _repo.GetSalesHistoryPage(request)
    End Function
End Class
