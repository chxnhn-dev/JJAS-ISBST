Imports System.Data.SqlClient

' NOTE:
' This module originally contained a hard-coded connection string and shared ADO.NET objects.
' It is kept for backward compatibility with existing Forms.
' Prefer using DataAccess.GetConnection() and repository/service classes for new code.
Module Module1

    ''' <summary>
    ''' Backward-compatible helper. Prefer DataAccess.GetConnection().
    ''' </summary>
    Public Function connect() As SqlConnection
        Return DataAccess.GetConnection()
    End Function

    ' Legacy shared objects (avoid in new code)
    Public conn As SqlConnection = connect()
    Public cmd As New SqlCommand
    Public da As New SqlDataAdapter
    Public dt As New DataTable

    ''' <summary>
    ''' Legacy grid reloader. Prefer repositories + data binding.
    ''' </summary>
    Public Sub reload(ByVal sql As String, ByVal DTG As Object)
        dt = New DataTable()

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        With cmd
            .Connection = conn
            .CommandText = sql
        End With

        da.SelectCommand = cmd
        da.Fill(dt)
        DTG.datasource = dt

        If conn.State = ConnectionState.Open Then
            conn.Close()
        End If

        da.Dispose()
    End Sub

End Module
