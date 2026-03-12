Imports System.Data
Imports System.Security.Cryptography
Imports System.Text

Public Class UserService
    Private ReadOnly _repo As New UserRepository()

    Public Function GetUsers(searchText As String) As DataTable
        Return _repo.GetUsers(searchText)
    End Function

    Public Function GetUsersPage(request As PagedQueryRequest) As PagedQueryResult
        Return _repo.GetUsersPage(request)
    End Function

    Public Function GetUserById(userId As Integer) As DataRow
        Return _repo.GetUserById(userId)
    End Function

    Public Function UsernameExists(username As String, Optional excludeUserId As Integer? = Nothing) As Boolean
        Return _repo.UsernameExists(username, excludeUserId)
    End Function

    Public Function EmailExists(email As String, Optional excludeUserId As Integer? = Nothing) As Boolean
        Return _repo.EmailExists(email, excludeUserId)
    End Function

    Public Function ContactExists(contactNumber As String, Optional excludeUserId As Integer? = Nothing) As Boolean
        Return _repo.ContactExists(contactNumber, excludeUserId)
    End Function

    Public Sub SaveUser(mode As EntryFormMode,
                        selectedId As Integer,
                        role As String,
                        firstName As String,
                        lastName As String,
                        contactNumber As String,
                        email As String,
                        address As String,
                        username As String,
                        Optional plainPassword As String = Nothing)
        Dim now As DateTime = DateTime.Now
        Dim hasPassword As Boolean = Not String.IsNullOrWhiteSpace(plainPassword)

        If mode = EntryFormMode.EditExisting AndAlso selectedId > 0 Then
            _repo.UpdateUser(selectedId, role, firstName, lastName, contactNumber, email, address, username, now)
            If hasPassword Then
                _repo.UpdatePassword(selectedId, HashPassword(plainPassword))
            End If
        Else
            _repo.InsertUser(role, firstName, lastName, contactNumber, email, address, username, HashPassword(plainPassword), now)
        End If
    End Sub

    Public Sub SetUserActiveStatus(userId As Integer, isActive As Boolean)
        _repo.SetUserActiveStatus(userId, isActive)
    End Sub

    Public Sub DeleteUser(userId As Integer)
        _repo.DeleteUser(userId)
    End Sub

    Private Function HashPassword(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
            Return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant()
        End Using
    End Function
End Class
