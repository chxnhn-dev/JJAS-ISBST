Imports System.Data
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text

Public Class AuthService

    Public Class LoginResult
        Public Property Success As Boolean
        Public Property UserId As Integer
        Public Property Username As String
        Public Property FullName As String
        Public Property Role As String
        Public Property ErrorMessage As String
    End Class

    Private Function HashPasswordSha256(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
            Return BitConverter.ToString(bytes).Replace("-", "").ToLower()
        End Using
    End Function

    Public Function FrmLogin(username As String, password As String) As LoginResult
        Dim result As New LoginResult() With {.Success = False}

        username = If(username, "").Trim()
        password = If(password, "").Trim()

        If username = "" OrElse password = "" Then
            result.ErrorMessage = "Please enter both Username and Password."
            Return result
        End If

        ' 1) Admin login (PBKDF2)
        Try
            Dim adminSql As String = "
                SELECT AdminID, Username, FullName, PasswordHash, PasswordSalt, Iterations
                FROM Admins
                WHERE Username = @Username;"

            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Using cmd As New SqlCommand(adminSql, conn)
                    cmd.Parameters.Add(New SqlParameter("@Username", SqlDbType.NVarChar, 50) With {.Value = username})
                    Using rdr = cmd.ExecuteReader()
                        If rdr.Read() Then
                            Dim storedHash = CType(rdr("PasswordHash"), Byte())
                            Dim storedSalt = CType(rdr("PasswordSalt"), Byte())
                            Dim iterations = Convert.ToInt32(rdr("Iterations"))
                            Dim fullName = rdr("FullName").ToString()

                            If AuthUtils.VerifyPassword(password, storedHash, storedSalt, iterations) Then
                                result.Success = True
                                result.UserId = Convert.ToInt32(rdr("AdminID"))
                                result.Username = username
                                result.FullName = fullName
                                result.Role = "admin"
                                Return result
                            Else
                                result.ErrorMessage = "Invalid Admin password."
                                Return result
                            End If
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            result.ErrorMessage = "Error checking admin account: " & ex.Message
            Return result
        End Try

        ' 2) Regular user login (tbl_User) using SHA-256 legacy hashing
        Try
            Dim hashedPassword As String = HashPasswordSha256(password)

            Dim sql As String = "
                SELECT UserID, Username, Role, FirstName, LastName
                FROM tbl_User 
                WHERE Username = @username AND Password = @password AND IsActive = 1;"

            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.Add(New SqlParameter("@username", SqlDbType.NVarChar, 50) With {.Value = username})
                    cmd.Parameters.Add(New SqlParameter("@password", SqlDbType.NVarChar, 256) With {.Value = hashedPassword})

                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        If rdr.Read() Then
                            result.Success = True
                            result.UserId = Convert.ToInt32(rdr("UserID"))
                            result.Username = rdr("Username").ToString()
                            result.Role = rdr("Role").ToString().ToLower()

                            Dim firstName As String = rdr("FirstName").ToString()
                            Dim lastName As String = rdr("LastName").ToString()
                            result.FullName = ($"{firstName} {lastName}").Trim()
                        Else
                            result.ErrorMessage = "FrmLogin Failed. Invalid username or password."
                        End If
                    End Using
                End Using
            End Using

            Return result

        Catch ex As Exception
            result.ErrorMessage = "Error connecting to database: " & ex.Message
            Return result
        End Try

    End Function

    Public Sub LogoutUser(userId As Integer)
        ' Kept for backward compatibility with existing UI.
        ' Proper logout uses SessionService.EndCurrentSession().
        Try
            SessionService.EndCurrentSession("Logout")
        Catch
            ' ignore
        End Try
    End Sub

End Class
