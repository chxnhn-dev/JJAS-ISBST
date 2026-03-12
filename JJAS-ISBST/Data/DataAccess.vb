Imports System.Configuration
Imports System.Data.SqlClient

Public Module DataAccess
    ' NOTE:
    ' We keep the existing settings-based connection string name so the project
    ' continues to run without code changes on your side.
    ' This refactor simply makes failures clearer (instead of a vague NullReference).
    Private ReadOnly _connectionString As String = GetConnectionStringSafe()

    Private Function GetConnectionStringSafe() As String
        Dim cs = ConfigurationManager.ConnectionStrings("JJAS_ISBST.My.MySettings.JJAS_ISBSTConnectionString")
        If cs Is Nothing OrElse String.IsNullOrWhiteSpace(cs.ConnectionString) Then
            Throw New ConfigurationErrorsException(
                "Missing connection string 'JJAS_ISBST.My.MySettings.JJAS_ISBSTConnectionString' in app.config. " &
                "Open the project settings (Settings.settings) or App.config and ensure the connection string exists.")
        End If
        Return cs.ConnectionString
    End Function

    Public Function GetConnection() As SqlConnection
        Return New SqlConnection(_connectionString)
    End Function
End Module