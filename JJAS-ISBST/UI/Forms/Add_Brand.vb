Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Add_Brand
    Dim SizeId As String

    Private Sub Add_Brand_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtbrand)
    End Sub

    Public Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        Dim exists As Boolean = False
        Dim sql As String = $"SELECT COUNT(*) FROM tbl_Brand WHERE LOWER({fieldName}) = LOWER(@Value)"
        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Value", fieldValue)
                conn.Open()
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                exists = (count > 0)
            End Using
        End Using
        Return exists
    End Function
    Private Sub txtbrand_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbrand.KeyPress
        ' Allow letters, digits, control keys, and space
        If Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) OrElse Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = False
            Exit Sub
        End If

        ' Allow only one special symbol (&, +, ', -)
        Dim allowedSymbols As String = "&'+-"
        If allowedSymbols.Contains(e.KeyChar) Then
            ' Check if the symbol already exists in the text
            If txtbrand.Text.IndexOfAny(allowedSymbols.ToCharArray()) <> -1 Then
                e.Handled = True ' Already has one special symbol
            Else
                e.Handled = False ' Allow it once
            End If
        Else
            e.Handled = True ' Block all other symbols
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim fieldName As String = txtBrand.Text.Trim

        If String.IsNullOrWhiteSpace(txtbrand.Text) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        txtbrand.Text = txtbrand.Text.Trim()

        If txtbrand.Text.Length < 2 Then
            MessageBox.Show("Brand name must be at least 2 characters long.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtbrand.Text.Contains("  ") Then
            MessageBox.Show("Brand name cannot contain multiple consecutive spaces.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("Brand", txtbrand.Text) Then
            MessageBox.Show("Brand already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If conn.State = ConnectionState.Closed Then conn.Open()

        If MessageBox.Show("Are you sure you want to add this brand?",
                       "Confirm Edit",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

            Try
                cmd = New SqlCommand("INSERT INTO tbl_Brand (Brand, DateCreated) 
                          VALUES (@Brand, @DateCreated)", conn)

                With cmd.Parameters
                    .AddWithValue("@Brand", txtbrand.Text.Trim())
                    .AddWithValue("@DateCreated", DateTime.Now)
                End With

                cmd.ExecuteNonQuery()
                conn.Close()

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Added Brand.")

                MsgBox("Added successfully!", MsgBoxStyle.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("An error occurred while adding brand: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

End Class