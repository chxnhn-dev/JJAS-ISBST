Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms

' Professional session handling:
' - IsActive stays as "account enabled/disabled"
' - Current login state is tracked in dbo.tbl_AppSession (created by DB script in /DB)
Public Class SessionService

    Private Const SESSION_TABLE As String = "dbo.tbl_AppSession"

    Public Shared Function CreateSession(principalType As String, principalId As Integer, username As String, fullName As String, role As String) As Guid
        Dim sessionId As Guid = Guid.NewGuid()

        ' Best-effort DB session. If the table doesn't exist yet, we still allow login (in-memory only),
        ' but you should run the provided SQL script to enable full session tracking.
        Try
            Dim sql As String = $"
                -- End any existing active sessions for this principal (single-session policy)
                UPDATE {SESSION_TABLE}
                   SET EndedAt = SYSDATETIME(),
                       EndReason = 'Kicked'
                 WHERE PrincipalType = @PrincipalType
                   AND PrincipalID = @PrincipalID
                   AND EndedAt IS NULL
                   AND ExpiresAt > SYSDATETIME();

                INSERT INTO {SESSION_TABLE}
                    (SessionID, PrincipalType, PrincipalID, Username, FullName, Role, CreatedAt, LastSeenAt, ExpiresAt, MachineName, AppVersion)
                VALUES
                    (@SessionID, @PrincipalType, @PrincipalID, @Username, @FullName, @Role, SYSDATETIME(), SYSDATETIME(), DATEADD(HOUR, 12, SYSDATETIME()), @MachineName, @AppVersion);
            "

            Db.ExecuteNonQuery(sql,
                New SqlParameter("@SessionID", SqlDbType.UniqueIdentifier) With {.Value = sessionId},
                New SqlParameter("@PrincipalType", SqlDbType.NVarChar, 20) With {.Value = principalType},
                New SqlParameter("@PrincipalID", SqlDbType.Int) With {.Value = principalId},
                New SqlParameter("@Username", SqlDbType.NVarChar, 100) With {.Value = username},
                New SqlParameter("@FullName", SqlDbType.NVarChar, 200) With {.Value = fullName},
                New SqlParameter("@Role", SqlDbType.NVarChar, 50) With {.Value = role},
                New SqlParameter("@MachineName", SqlDbType.NVarChar, 128) With {.Value = Environment.MachineName},
                New SqlParameter("@AppVersion", SqlDbType.NVarChar, 50) With {.Value = Application.ProductVersion}
            )
        Catch
            ' swallow: app can still run without session table (but deactivation checks won't work)
        End Try

        SessionContext.SessionID = sessionId
        SessionContext.PrincipalType = principalType
        SessionContext.PrincipalID = principalId
        SessionContext.Username = username
        SessionContext.FullName = fullName
        SessionContext.Role = role

        Return sessionId
    End Function

    Public Shared Sub EndCurrentSession(Optional reason As String = "Logout")
        If SessionContext.SessionID = Guid.Empty Then
            SessionContext.Clear()
            Return
        End If

        Try
            Dim sql As String = $"
                UPDATE {SESSION_TABLE}
                   SET EndedAt = SYSDATETIME(),
                       EndReason = @Reason
                 WHERE SessionID = @SessionID
                   AND EndedAt IS NULL;
            "
            Db.ExecuteNonQuery(sql,
                New SqlParameter("@Reason", SqlDbType.NVarChar, 50) With {.Value = reason},
                New SqlParameter("@SessionID", SqlDbType.UniqueIdentifier) With {.Value = SessionContext.SessionID}
            )
        Catch
            ' ignore
        End Try

        SessionContext.Clear()
    End Sub

    Public Shared Function Heartbeat() As Boolean
        If SessionContext.SessionID = Guid.Empty Then Return False

        Try
            Dim sql As String = $"
                UPDATE {SESSION_TABLE}
                   SET LastSeenAt = SYSDATETIME()
                 WHERE SessionID = @SessionID
                   AND EndedAt IS NULL
                   AND ExpiresAt > SYSDATETIME();

                SELECT CASE WHEN EXISTS(
                    SELECT 1 FROM {SESSION_TABLE}
                     WHERE SessionID = @SessionID
                       AND EndedAt IS NULL
                       AND ExpiresAt > SYSDATETIME()
                ) THEN 1 ELSE 0 END;
            "
            Dim isValid As Integer = Db.ExecuteScalar(Of Integer)(sql,
                New SqlParameter("@SessionID", SqlDbType.UniqueIdentifier) With {.Value = SessionContext.SessionID}
            )
            Return isValid = 1
        Catch
            ' If session table doesn't exist, treat as valid (in-memory only)
            Return True
        End Try
    End Function

    Public Shared Function IsUserLoggedIn(userId As Integer) As Boolean
        Try
            Dim sql As String = $"
                SELECT CASE WHEN EXISTS(
                    SELECT 1 FROM {SESSION_TABLE}
                     WHERE PrincipalType = 'user'
                       AND PrincipalID = @UserID
                       AND EndedAt IS NULL
                       AND ExpiresAt > SYSDATETIME()
                ) THEN 1 ELSE 0 END;
            "
            Dim val As Integer = Db.ExecuteScalar(Of Integer)(sql,
                New SqlParameter("@UserID", SqlDbType.Int) With {.Value = userId}
            )
            Return val = 1
        Catch
            Return False
        End Try
    End Function

End Class
