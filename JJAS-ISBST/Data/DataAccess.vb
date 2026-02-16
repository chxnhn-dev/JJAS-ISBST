Imports System.Configuration
Imports System.Data.SqlClient

Public Module DataAccess
    Private ReadOnly _connectionString As String = ConfigurationManager.ConnectionStrings("JJAS_ISBST.My.MySettings.JJAS_ISBSTConnectionString").ConnectionString

    Public Function GetConnection() As SqlConnection
        Return New SqlConnection(_connectionString)
    End Function
End Module