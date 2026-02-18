Imports System.Data.SqlClient

Namespace FileMaintenance
    Public Class Vat
        Inherits FileMaintenanceBaseForm

        Protected Overrides ReadOnly Property CurrentMaintenanceTab As MaintenanceTab
            Get
                Return MaintenanceTab.VatTab
            End Get
        End Property

        Protected Overrides ReadOnly Property SearchCaption As String
            Get
                Return "Search VAT:"
            End Get
        End Property

        Protected Overrides ReadOnly Property SearchPlaceholder As String
            Get
                Return "12"
            End Get
        End Property

        Protected Overrides ReadOnly Property UseEditPrimaryAction As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Overrides Sub HandlePrimaryAction()
            UpdateVat()
        End Sub

        Protected Overrides Sub LoadTableData(searchText As String)
            Dim dt As New DataTable()
            Dim sql As String = "
                SELECT VatID,
                       Vat_Rate,
                       DateUpdated
                FROM tbl_Vat
                WHERE (@search = '' OR CONVERT(varchar(20), Vat_Rate) LIKE @search)
                ORDER BY DateUpdated DESC"

            Using conn As SqlConnection = DataAccess.GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@search", "%" & searchText.Trim() & "%")
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            DGVtable.DataSource = dt
            If DGVtable.Columns.Contains("VatID") Then
                DGVtable.Columns("VatID").Visible = False
            End If

            DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            ApplyDefaultGridLayout()
        End Sub

        Private Sub UpdateVat()
            If DGVtable.Rows.Count = 0 Then
                MessageBox.Show("No VAT record available to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim targetRow As DataGridViewRow = Nothing
            If DGVtable.CurrentRow IsNot Nothing AndAlso Not DGVtable.CurrentRow.IsNewRow Then
                targetRow = DGVtable.CurrentRow
            Else
                targetRow = DGVtable.Rows(0)
            End If

            Dim vatId As Integer = Convert.ToInt32(targetRow.Cells("VatID").Value)
            Dim vatRate As Decimal = Convert.ToDecimal(targetRow.Cells("Vat_Rate").Value)

            Dim entryForm As New FrmVATEntry With {
                .VatID = vatId,
                .VatRate = vatRate
            }

            If entryForm.ShowDialog() = DialogResult.OK Then
                LogActivity(FrmLogin.CurrentUser.UserID, FrmLogin.CurrentUser.FullName, FrmLogin.CurrentUser.Username, FrmLogin.CurrentUser.Role, "VAT updated.")
                ReloadData()
            End If
        End Sub

        Private Sub DGVtable_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellDoubleClick
            If e.RowIndex < 0 Then Exit Sub
            UpdateVat()
        End Sub
    End Class
End Namespace
