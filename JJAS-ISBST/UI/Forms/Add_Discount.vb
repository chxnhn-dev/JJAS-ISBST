Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Add_Discount
    Private Sub Add_Discount_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub txtContatNumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDiscountValue.KeyPress

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
    Private Sub txtDiscountName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDiscountName.KeyPress
        ' Allow letters, digits, space, backspace, and %
        If Not (Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsWhiteSpace(e.KeyChar) OrElse e.KeyChar = "-"c OrElse e.KeyChar = ControlChars.Back) Then
            e.Handled = True
        End If
    End Sub

    Private Function IsDuplicateDiscount(name As String) As Boolean
        Dim sql As String = "SELECT CASE WHEN EXISTS (
                                SELECT 1 FROM tbl_Discount WHERE LOWER(DiscountName) = LOWER(@Name)
                             ) THEN 1 ELSE 0 END"
        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Name", name.Trim)
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
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        If String.IsNullOrWhiteSpace(txtDiscountValue.Text) Then
            MessageBox.Show("Please enter a discount value.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountValue.Focus()
            Exit Sub
        End If

        Dim discountValue As Decimal

        If IsDuplicateDiscount(txtDiscountName.Text) Then
            MessageBox.Show("This discount name already exists.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountName.Focus()
            Exit Sub
        End If

        If txtDiscountName.Text.Length < 2 Then
            MessageBox.Show("Brand name must be at least 2 characters long.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtDiscountName.Text.Contains("  ") Then
            MessageBox.Show("Brand name cannot contain multiple consecutive spaces.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not Decimal.TryParse(txtDiscountValue.Text, discountValue) Then
            MessageBox.Show("Please enter a valid number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountValue.Focus()
            Exit Sub
        End If

        ' Extra validation for Percentage
        If txtDiscountValue.Text.Trim = (discountValue < 0 Or discountValue > 100) Then
            MessageBox.Show("Percentage must be between 0 and 100.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountValue.Focus()
            Exit Sub
        End If

        If MessageBox.Show("Are you sure you want to add this discount?",
                   "Confirm Edit",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

            Try
                If conn.State = ConnectionState.Closed Then conn.Open()

                Dim query As String = "INSERT INTO tbl_Discount (DiscountName, DiscountValue, Description, DateUpdated) 
                                   VALUES (@DiscountName, @DiscountValue, @Description, @DateUpdated)"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@DiscountName", txtDiscountName.Text.Trim)
                    cmd.Parameters.AddWithValue("@DiscountValue", discountValue)
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim)
                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now)

                    cmd.ExecuteNonQuery()
                End Using
                conn.Close()

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Added discount.")

                MessageBox.Show("Added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As Exception
                MessageBox.Show("Error while saving: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class