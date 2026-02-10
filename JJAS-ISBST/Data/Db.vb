Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' Small ADO.NET helper to reduce duplication and standardize safe DB calls.
''' Prefer repositories/services over writing SQL in Forms.
''' </summary>
Public Module Db

    Public Function QueryDataTable(sql As String, ParamArray parameters() As SqlParameter) As DataTable
        Dim dt As New DataTable()
        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                    cmd.Parameters.AddRange(parameters)
                End If
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function

    Public Function ExecuteNonQuery(sql As String, ParamArray parameters() As SqlParameter) As Integer
        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                    cmd.Parameters.AddRange(parameters)
                End If
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function ExecuteScalar(Of T)(sql As String, ParamArray parameters() As SqlParameter) As T
        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                    cmd.Parameters.AddRange(parameters)
                End If
                Dim obj = cmd.ExecuteScalar()
                If obj Is Nothing OrElse obj Is DBNull.Value Then
                    Return Nothing
                End If
                Return CType(Convert.ChangeType(obj, GetType(T)), T)
            End Using
        End Using
    End Function

End Module
