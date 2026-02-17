Imports System.Data.SqlClient
<<<<<<< HEAD
Imports JJAS_ISBST
=======
Imports JJAS_ISBST.Login
>>>>>>> 66ac34f75a7f9e5bea91a346824fcee990f61aba

Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.


    Partial Friend Class MyApplication
        Private Sub MyApplication_Shutdown(sender As Object, e As EventArgs) Handles Me.Shutdown
            Try
<<<<<<< HEAD
                SessionService.EndCurrentSession("Shutdown")
            Catch ex As Exception
                MessageBox.Show("Error ending session: " & ex.Message)
=======
                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    Using cmd As New SqlCommand("UPDATE tbl_User SET IsLoggedIn = 0 WHERE UserID = @UserID", conn)
                        cmd.Parameters.AddWithValue("@UserID", CurrentUser.UserID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error logging out current user: " & ex.Message)
>>>>>>> 66ac34f75a7f9e5bea91a346824fcee990f61aba
            End Try
        End Sub

    End Class
End Namespace
