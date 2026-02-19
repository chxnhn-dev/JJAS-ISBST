Imports System.Data.SqlClient
Imports JJAS_ISBST.FrmLogin
Imports System.Text.RegularExpressions

Public Class Edit_Discount
    Public Property DiscountID As Integer
    Public Property DiscountName As String
    Public Property DiscountValue As Decimal
    Public Property Description As String

    Private Sub Edit_Discount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtDiscountName)
        BlockCopyPaste(txtDescription)
        BlockCopyPaste(txtDiscountValue)

        txtDiscountName.Text = DiscountName
        txtDiscountValue.Text = DiscountValue
        txtDescription.Text = Description
    End Sub

    ' Prevent special characters except % in DiscountName
    Private Sub txtDiscountName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDiscountName.KeyPress
        If Not (Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsWhiteSpace(e.KeyChar) OrElse e.KeyChar = "%"c OrElse e.KeyChar = ControlChars.Back) Then
            e.Handled = True
        End If
    End Sub

    ' Validate DiscountValue input
    Private Sub txtDiscountValue_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDiscountValue.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then
            e.Handled = False
        ElseIf e.KeyChar = "."c Then
            If txtDiscountValue.Text.Contains(".") Then
                e.Handled = True
            Else
                e.Handled = False
            End If
        Else
            e.Handled = True
        End If
    End Sub

    ' Check for duplicate discount names excluding current record
    Private Function IsDuplicateDiscount(name As String, id As Integer) As Boolean
        Dim sql As String = "
            SELECT CASE 
                WHEN EXISTS (
                    SELECT 1 FROM tbl_Discount WHERE LOWER(DiscountName) = LOWER(@Name) AND DiscountID <> @ID
                ) THEN 1 ELSE 0 END"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Name", name.Trim)
                cmd.Parameters.AddWithValue("@ID", id)
                conn.Open()
                Return Convert.ToBoolean(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

    Private Sub OnlyLetters_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDiscountName.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        ' Validate DiscountName
        If String.IsNullOrWhiteSpace(txtDiscountName.Text) Then
            MessageBox.Show("Please enter a discount name.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountName.Focus()
            Exit Sub
        End If

        If txtDiscountName.Text.Length < 2 Then
            MessageBox.Show("Discount name must be at least 2 characters long.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtDiscountName.Text.Contains("  ") Then
            MessageBox.Show("Discount name cannot contain multiple consecutive spaces.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Check for duplicates
        If IsDuplicateDiscount(txtDiscountName.Text, DiscountID) Then
            MessageBox.Show("Discount name already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountName.Focus()
            Exit Sub
        End If

        ' Validate DiscountValue
        Dim discountValue As Decimal
        If String.IsNullOrWhiteSpace(txtDiscountValue.Text) OrElse Not Decimal.TryParse(txtDiscountValue.Text, discountValue) Then
            MessageBox.Show("Please enter a valid discount value.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountValue.Focus()
            Exit Sub
        End If

        If discountValue < 0 Or discountValue > 100 Then
            MessageBox.Show("Percentage must be between 0 and 100.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountValue.Focus()
            Exit Sub
        End If

        ' Confirm update
        If MessageBox.Show("Are you sure you want to update this discount?", "Confirm Edit",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Dim sql As String = "
                    UPDATE tbl_Discount
                    SET DiscountName=@DiscountName,
                        DiscountValue=@DiscountValue,
                        Description=@Description,
                        DateUpdated=@DateUpdated
                    WHERE DiscountID=@DiscountID"

                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@DiscountName", txtDiscountName.Text.Trim)
                        cmd.Parameters.AddWithValue("@DiscountValue", discountValue)
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim)
                        cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now)
                        cmd.Parameters.AddWithValue("@DiscountID", DiscountID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Edited Discount.")
                MessageBox.Show("Updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("An error occurred while editing discount: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class
