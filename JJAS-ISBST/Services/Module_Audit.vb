Imports System.Data.SqlClient

Module Module_Audit
    Public Sub LogActivity(userID As Integer, Name As String, username As String, role As String, action As String)
        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Dim sql As String = "INSERT INTO tbl_AuditTrail (UserID, Name, Username, Role, Action) VALUES (@UserID, @Name, @Username, @Role, @Action)"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@UserID", userID)
                    cmd.Parameters.AddWithValue("@name", Name)
                    cmd.Parameters.AddWithValue("@Username", username)
                    cmd.Parameters.AddWithValue("@Role", role)
                    cmd.Parameters.AddWithValue("@Action", action)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Audit trail error: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
End Module
