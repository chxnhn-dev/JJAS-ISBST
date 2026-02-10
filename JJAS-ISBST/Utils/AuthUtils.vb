Imports System.Security.Cryptography

Public Module AuthUtils
    Public ReadOnly DefaultIterations As Integer = 200_000
    Private ReadOnly SaltSize As Integer = 32
    Private ReadOnly HashSize As Integer = 32

    Public Function CreatePasswordHash(password As String, Optional iterations As Integer = 0) As (hash As Byte(), salt As Byte(), iterationsUsed As Integer)
        If iterations <= 0 Then iterations = DefaultIterations
        Dim salt(SaltSize - 1) As Byte
        Using rng = RandomNumberGenerator.Create()
            rng.GetBytes(salt)
        End Using

        Dim hash As Byte()
        Try
            Dim r As New Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256)
            hash = r.GetBytes(HashSize)
        Catch ex As MissingMethodException
            Dim r As New Rfc2898DeriveBytes(password, salt, iterations) ' fallback to HMAC-SHA1
            hash = r.GetBytes(HashSize)
        End Try

        Return (hash, salt, iterations)
    End Function

    Public Function VerifyPassword(password As String, storedHash As Byte(), storedSalt As Byte(), iterations As Integer) As Boolean
        If iterations <= 0 Then iterations = DefaultIterations
        Dim computed As Byte()
        Try
            Dim r As New Rfc2898DeriveBytes(password, storedSalt, iterations, HashAlgorithmName.SHA256)
            computed = r.GetBytes(storedHash.Length)
        Catch ex As MissingMethodException
            Dim r As New Rfc2898DeriveBytes(password, storedSalt, iterations)
            computed = r.GetBytes(storedHash.Length)
        End Try
        Return FixedTimeEquals(storedHash, computed)
    End Function

    Private Function FixedTimeEquals(a As Byte(), b As Byte()) As Boolean
        If a Is Nothing OrElse b Is Nothing Then Return False
        If a.Length <> b.Length Then Return False
        Dim diff As Integer = 0
        For i = 0 To a.Length - 1
            diff = diff Or (a(i) Xor b(i))
        Next
        Return diff = 0
    End Function
End Module