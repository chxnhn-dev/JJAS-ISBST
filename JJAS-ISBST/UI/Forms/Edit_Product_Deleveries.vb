Imports System.Data.SqlClient

Public Class Edit_Product_Deleveries
    Public Property selectedProductID As Integer

    Public Property SelectedQuantity As Integer
    Public Property SelectedCostPrice As String
    Dim selectedid As Integer = -1

    Private Sub Edit_Product_Deleveries_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        selectedid = selectedProductID
        txtCostPrice.Text = SelectedCostPrice
        txtQuantity.Text = SelectedQuantity
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        ' 1. Validate fields
        If String.IsNullOrWhiteSpace(txtCostPrice.Text) OrElse
           String.IsNullOrWhiteSpace(txtQuantity.Text) Then
            MessageBox.Show("Please fill in all required fields.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        SelectedQuantity = Integer.Parse(txtQuantity.Text)
        SelectedCostPrice = Decimal.Parse(txtCostPrice.Text)

        If MessageBox.Show("Are you sure you want to update this product quantiy/costprice?",
                   "Confirm Edit",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then



            Try

                Using conn As SqlConnection = DataAccess.GetConnection()
                    Using cmd As New SqlCommand("SELECT SellingPrice FROM tbl_Products WHERE ProductID=@ProductID", conn)
                        cmd.Parameters.AddWithValue("@ProductID", selectedid)
                        conn.Open()
                        Dim dbSellingPrice As Object = cmd.ExecuteScalar()
                        If dbSellingPrice IsNot Nothing AndAlso SelectedCostPrice >= CDec(dbSellingPrice) Then
                            MessageBox.Show($"Cost Price ({SelectedCostPrice:C}) cannot be higher than Selling Price ({CDec(dbSellingPrice):C}).",
                                    "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Exit Sub
                        End If
                    End Using
                End Using

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
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class