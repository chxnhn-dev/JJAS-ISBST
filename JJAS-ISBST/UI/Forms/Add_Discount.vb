Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Add_Discount
    Public Property DiscountID As Integer?

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return DiscountID.HasValue
        End Get
    End Property

    Private Sub Add_Discount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigureMode()
        BlockCopyPaste(txtDiscountName)
        BlockCopyPaste(txtDiscountValue)
        BlockCopyPaste(txtDescription)

        If IsEditMode Then
            LoadDiscountForEdit()
        End If
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Discount"
            btnAdd.Text = "Update"
        Else
            Me.Text = "Add Discount"
            btnAdd.Text = "Save"
        End If
    End Sub

    Private Sub LoadDiscountForEdit()
        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand("SELECT DiscountName, DiscountValue, Description FROM tbl_Discount WHERE DiscountID = @DiscountID", connection)
                command.Parameters.AddWithValue("@DiscountID", DiscountID.Value)
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        txtDiscountName.Text = reader("DiscountName").ToString()
                        txtDiscountValue.Text = Convert.ToDecimal(reader("DiscountValue")).ToString("0.##")
                        txtDescription.Text = reader("Description").ToString()
                    Else
                        MessageBox.Show("Selected discount record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Me.DialogResult = DialogResult.Cancel
                        Me.Close()
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Sub txtDiscountValue_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDiscountValue.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then
            e.Handled = False
        ElseIf e.KeyChar = "."c Then
            e.Handled = txtDiscountValue.Text.Contains(".")
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtDiscountName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDiscountName.KeyPress
        If Not (Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsWhiteSpace(e.KeyChar) OrElse e.KeyChar = "-"c OrElse e.KeyChar = ControlChars.Back) Then
            e.Handled = True
        End If
    End Sub

    Private Function IsDuplicateDiscount(name As String) As Boolean
        Dim sql As String = "SELECT CASE WHEN EXISTS (SELECT 1 FROM tbl_Discount WHERE LOWER(DiscountName) = LOWER(@Name)"

        If IsEditMode Then
            sql &= " AND DiscountID <> @DiscountID"
        End If

        sql &= ") THEN 1 ELSE 0 END"

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand(sql, connection)
                command.Parameters.AddWithValue("@Name", name.Trim())

                If IsEditMode Then
                    command.Parameters.AddWithValue("@DiscountID", DiscountID.Value)
                End If

                connection.Open()
                Return Convert.ToBoolean(command.ExecuteScalar())
            End Using
        End Using
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim discountName As String = txtDiscountName.Text.Trim()
        Dim description As String = txtDescription.Text.Trim()

        If String.IsNullOrWhiteSpace(discountName) Then
            MessageBox.Show("Please enter a discount name.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountName.Focus()
            Exit Sub
        End If

        If discountName.Length < 2 Then
            MessageBox.Show("Discount name must be at least 2 characters long.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If discountName.Contains("  ") Then
            MessageBox.Show("Discount name cannot contain multiple consecutive spaces.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicateDiscount(discountName) Then
            MessageBox.Show("This discount name already exists.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountName.Focus()
            Exit Sub
        End If

        Dim discountValue As Decimal
        If String.IsNullOrWhiteSpace(txtDiscountValue.Text) OrElse Not Decimal.TryParse(txtDiscountValue.Text, discountValue) Then
            MessageBox.Show("Please enter a valid discount value.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountValue.Focus()
            Exit Sub
        End If

        If discountValue < 0D OrElse discountValue > 100D Then
            MessageBox.Show("Percentage must be between 0 and 100.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountValue.Focus()
            Exit Sub
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this discount?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            Using connection As SqlConnection = DataAccess.GetConnection()
                connection.Open()

                Dim sql As String
                If IsEditMode Then
                    sql = "UPDATE tbl_Discount SET DiscountName=@DiscountName, DiscountValue=@DiscountValue, Description=@Description, DateUpdated=@DateUpdated WHERE DiscountID=@DiscountID"
                Else
                    sql = "INSERT INTO tbl_Discount (DiscountName, DiscountValue, Description, DateUpdated) VALUES (@DiscountName, @DiscountValue, @Description, @DateUpdated)"
                End If

                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddWithValue("@DiscountName", discountName)
                    command.Parameters.AddWithValue("@DiscountValue", discountValue)
                    command.Parameters.AddWithValue("@Description", description)
                    command.Parameters.AddWithValue("@DateUpdated", DateTime.Now)

                    If IsEditMode Then
                        command.Parameters.AddWithValue("@DiscountID", DiscountID.Value)
                    End If

                    command.ExecuteNonQuery()
                End Using
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Discount.", "Added discount."))
            MessageBox.Show(If(IsEditMode, "Updated successfully.", "Added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error while saving discount: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
