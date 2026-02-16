Imports System

' Holds the current in-memory session state for the running application.
' This is NOT stored in tbl_User; the DB keeps sessions in tbl_AppSession.
Public Module SessionContext
    Public Property SessionID As Guid = Guid.Empty
    Public Property PrincipalType As String = ""   ' "admin" | "user"
    Public Property PrincipalID As Integer = 0     ' AdminID or UserID
    Public Property Username As String = ""
    Public Property FullName As String = ""
    Public Property Role As String = ""            ' admin/cashier/staff...

    Public Sub Clear()
        SessionID = Guid.Empty
        PrincipalType = ""
        PrincipalID = 0
        Username = ""
        FullName = ""
        Role = ""
    End Sub

    Public ReadOnly Property IsLoggedIn As Boolean
        Get
            Return SessionID <> Guid.Empty AndAlso PrincipalID > 0
        End Get
    End Property
End Module
