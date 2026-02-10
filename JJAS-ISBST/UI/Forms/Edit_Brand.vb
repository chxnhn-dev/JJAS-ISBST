Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Edit_Brand
    Public Property BrandID As Integer
    Public Property Brand As String
    Public Property Description As String
    Private Sub Edit_Size_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtbrand)

        txtbrand.Text = Brand
    End Sub

    Private Function IsDuplicate(fieldName As String, value As String, currentBrandId As Integer) As Boolean
        Dim sql As String = "
        IF EXISTS (SELECT 1 FROM tbl_Brand 
                   WHERE LOWER(" & fieldName & ") = LOWER(@Value) 
                   AND BrandID <> @BrandID)
           SELECT 1 ELSE SELECT 0"
        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Value", value)
                cmd.Parameters.AddWithValue("@BrandID", currentBrandId)
                conn.Open()
                Return Convert.ToBoolean(cmd.ExecuteScalar())
            End Using
        End Using
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

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click

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

        If IsDuplicate("Brand", txtbrand.Text, BrandID) Then
            MessageBox.Show("Brand name already exists.", "Duplicate",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If MessageBox.Show("Are you sure you want to update this brand?",
                   "Confirm Edit",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Dim sql As String = "UPDATE tbl_Brand SET " &
                                "Brand=@Brand, DateCreated=@DateCreated  " &
                                "WHERE BrandID=@BrandID"

                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Brand", txtbrand.Text.Trim)
                        cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now)
                        cmd.Parameters.AddWithValue("@BrandID", BrandID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Edited Brand.")
                MessageBox.Show("Updated successfully.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("An error occurred while editing brand: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class