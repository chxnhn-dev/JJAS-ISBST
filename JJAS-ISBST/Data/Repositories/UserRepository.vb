Imports System.Data
Imports System.Data.SqlClient

Public Class UserRepository

    Public Function GetUsers(searchText As String) As DataTable
        Dim sql As String = "
            SELECT UserID,
                   Role,
                   FirstName,
                   LastName,
                   Username,
                   ContactNumber,
                   Address,
                   Email,
                   DateCreated AS DateUpdated,
                   IsActive
            FROM tbl_User
            WHERE (@search = ''
                OR FirstName LIKE @likeSearch
                OR LastName LIKE @likeSearch
                OR Username LIKE @likeSearch
                OR CONCAT(FirstName, ' ', LastName) LIKE @likeSearch
                OR (@search = 'active' AND IsActive = 1)
                OR (@search = 'inactive' AND IsActive = 0))
            ORDER BY IsActive ASC, DateCreated DESC;"

        Dim normalized As String = If(searchText, String.Empty).Trim().ToLowerInvariant()
        Dim likeSearch As String = If(String.IsNullOrWhiteSpace(searchText), "", "%" & searchText.Trim() & "%")

        Return Db.QueryDataTable(sql,
            New SqlParameter("@search", SqlDbType.NVarChar, 100) With {.Value = normalized},
            New SqlParameter("@likeSearch", SqlDbType.NVarChar, 150) With {.Value = likeSearch})
    End Function

    Public Function GetUserById(userId As Integer) As DataRow
        Dim sql As String = "
            SELECT UserID,
                   Role,
                   FirstName,
                   LastName,
                   ContactNumber,
                   Email,
                   Address,
                   Username,
                   IsActive
            FROM tbl_User
            WHERE UserID = @UserID;"

        Dim dt As DataTable = Db.QueryDataTable(sql, New SqlParameter("@UserID", SqlDbType.Int) With {.Value = userId})
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Public Function UsernameExists(username As String, Optional excludeUserId As Integer? = Nothing) As Boolean
        Dim sql As String = "
            SELECT COUNT(1)
            FROM tbl_User
            WHERE Username = @Username
              AND IsActive = 1;"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@Username", SqlDbType.NVarChar, 100) With {.Value = username.Trim()}
        }

        If excludeUserId.HasValue Then
            sql = "
                SELECT COUNT(1)
                FROM tbl_User
                WHERE Username = @Username
                  AND IsActive = 1
                  AND UserID <> @UserID;"
            parameters.Add(New SqlParameter("@UserID", SqlDbType.Int) With {.Value = excludeUserId.Value})
        End If

        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, parameters.ToArray())
        Return count > 0
    End Function

    Public Function EmailExists(email As String, Optional excludeUserId As Integer? = Nothing) As Boolean
        Dim sql As String = "
            SELECT COUNT(1)
            FROM tbl_User
            WHERE Email = @Email
              AND IsActive = 1;"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@Email", SqlDbType.NVarChar, 200) With {.Value = email.Trim()}
        }

        If excludeUserId.HasValue Then
            sql = "
                SELECT COUNT(1)
                FROM tbl_User
                WHERE Email = @Email
                  AND IsActive = 1
                  AND UserID <> @UserID;"
            parameters.Add(New SqlParameter("@UserID", SqlDbType.Int) With {.Value = excludeUserId.Value})
        End If

        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, parameters.ToArray())
        Return count > 0
    End Function

    Public Function ContactExists(contactNumber As String, Optional excludeUserId As Integer? = Nothing) As Boolean
        Dim sql As String = "
            SELECT COUNT(1)
            FROM tbl_User
            WHERE ContactNumber = @ContactNumber
              AND IsActive = 1;"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@ContactNumber", SqlDbType.NVarChar, 20) With {.Value = contactNumber.Trim()}
        }

        If excludeUserId.HasValue Then
            sql = "
                SELECT COUNT(1)
                FROM tbl_User
                WHERE ContactNumber = @ContactNumber
                  AND IsActive = 1
                  AND UserID <> @UserID;"
            parameters.Add(New SqlParameter("@UserID", SqlDbType.Int) With {.Value = excludeUserId.Value})
        End If

        Dim count As Integer = Db.ExecuteScalar(Of Integer)(sql, parameters.ToArray())
        Return count > 0
    End Function

    Public Function InsertUser(role As String,
                               firstName As String,
                               lastName As String,
                               contactNumber As String,
                               email As String,
                               address As String,
                               username As String,
                               passwordHash As String,
                               createdAt As DateTime) As Integer
        Dim sql As String = "
            INSERT INTO tbl_User
                (Role, FirstName, LastName, ContactNumber, Email, Address, Username, Password, DateCreated, IsActive)
            VALUES
                (@Role, @FirstName, @LastName, @ContactNumber, @Email, @Address, @Username, @Password, @DateCreated, 1);"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@Role", SqlDbType.NVarChar, 50) With {.Value = role.Trim()},
            New SqlParameter("@FirstName", SqlDbType.NVarChar, 100) With {.Value = firstName.Trim()},
            New SqlParameter("@LastName", SqlDbType.NVarChar, 100) With {.Value = lastName.Trim()},
            New SqlParameter("@ContactNumber", SqlDbType.NVarChar, 20) With {.Value = contactNumber.Trim()},
            New SqlParameter("@Email", SqlDbType.NVarChar, 200) With {.Value = email.Trim()},
            New SqlParameter("@Address", SqlDbType.NVarChar, -1) With {.Value = address.Trim()},
            New SqlParameter("@Username", SqlDbType.NVarChar, 100) With {.Value = username.Trim()},
            New SqlParameter("@Password", SqlDbType.NVarChar, 256) With {.Value = passwordHash},
            New SqlParameter("@DateCreated", SqlDbType.DateTime) With {.Value = createdAt})
    End Function

    Public Function UpdateUser(userId As Integer,
                               role As String,
                               firstName As String,
                               lastName As String,
                               contactNumber As String,
                               email As String,
                               address As String,
                               username As String,
                               updatedAt As DateTime) As Integer
        Dim sql As String = "
            UPDATE tbl_User
               SET Role = @Role,
                   FirstName = @FirstName,
                   LastName = @LastName,
                   ContactNumber = @ContactNumber,
                   Email = @Email,
                   Address = @Address,
                   Username = @Username,
                   DateCreated = @DateCreated
             WHERE UserID = @UserID;"

        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@Role", SqlDbType.NVarChar, 50) With {.Value = role.Trim()},
            New SqlParameter("@FirstName", SqlDbType.NVarChar, 100) With {.Value = firstName.Trim()},
            New SqlParameter("@LastName", SqlDbType.NVarChar, 100) With {.Value = lastName.Trim()},
            New SqlParameter("@ContactNumber", SqlDbType.NVarChar, 20) With {.Value = contactNumber.Trim()},
            New SqlParameter("@Email", SqlDbType.NVarChar, 200) With {.Value = email.Trim()},
            New SqlParameter("@Address", SqlDbType.NVarChar, -1) With {.Value = address.Trim()},
            New SqlParameter("@Username", SqlDbType.NVarChar, 100) With {.Value = username.Trim()},
            New SqlParameter("@DateCreated", SqlDbType.DateTime) With {.Value = updatedAt},
            New SqlParameter("@UserID", SqlDbType.Int) With {.Value = userId})
    End Function

    Public Function UpdatePassword(userId As Integer, passwordHash As String) As Integer
        Dim sql As String = "UPDATE tbl_User SET Password = @Password WHERE UserID = @UserID;"
        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@Password", SqlDbType.NVarChar, 256) With {.Value = passwordHash},
            New SqlParameter("@UserID", SqlDbType.Int) With {.Value = userId})
    End Function

    Public Function SetUserActiveStatus(userId As Integer, isActive As Boolean) As Integer
        Dim sql As String = "UPDATE tbl_User SET IsActive = @IsActive WHERE UserID = @UserID;"
        Return Db.ExecuteNonQuery(sql,
            New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = isActive},
            New SqlParameter("@UserID", SqlDbType.Int) With {.Value = userId})
    End Function

    Public Function DeleteUser(userId As Integer) As Integer
        Dim sql As String = "DELETE FROM tbl_User WHERE UserID = @UserID;"
        Return Db.ExecuteNonQuery(sql, New SqlParameter("@UserID", SqlDbType.Int) With {.Value = userId})
    End Function

End Class
