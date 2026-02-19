Imports JJAS_ISBST.FrmLogin

Public Class FrmDeliveryEntry
    Private ReadOnly _service As New DeliveriesService()

    Private Const ColGridEdit As String = "colGridEdit"
    Private Const ColGridDelete As String = "colGridDelete"
    Private Const ColTempRowId As String = "TempRowID"

    Public Property Mode As EntryFormMode = EntryFormMode.AddNew
    Public Property SelectedId As Integer = -1

    Public Property deliveriesid As Integer
        Get
            Return SelectedId
        End Get
        Set(value As Integer)
            SelectedId = value
            If value > 0 Then
                Mode = EntryFormMode.EditExisting
            Else
                Mode = EntryFormMode.AddNew
            End If
        End Set
    End Property

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return Mode = EntryFormMode.EditExisting AndAlso SelectedId > 0
        End Get
    End Property

    Private dtPending As DataTable
    Private _isLoading As Boolean = False
    Private _nextTempRowId As Integer = 1

    Private Sub EnsureDtPending()
        If dtPending Is Nothing Then
            dtPending = New DataTable()
            dtPending.Columns.Add(ColTempRowId, GetType(Integer))
            dtPending.Columns.Add("ProductID", GetType(Integer))
            dtPending.Columns.Add("BarcodeNumber", GetType(String))
            dtPending.Columns.Add("Product", GetType(String))
            dtPending.Columns.Add("Quantity", GetType(Integer))
            dtPending.Columns.Add("CostPrice", GetType(Decimal))
            dtPending.Columns.Add("SellingPrice", GetType(Decimal))
            dtPending.Columns.Add("ImagePath", GetType(String))
            dtPending.Columns.Add("ProductImage", GetType(Image))
        End If
    End Sub

    Private Function GetNextTempRowId() As Integer
        Dim currentId As Integer = _nextTempRowId
        _nextTempRowId += 1
        Return currentId
    End Function

    Private Function LoadImageFromPath(imagePath As String) As Image
        If String.IsNullOrWhiteSpace(imagePath) OrElse Not IO.File.Exists(imagePath) Then
            Return Nothing
        End If

        Using tempImg As Image = Image.FromFile(imagePath)
            Return New Bitmap(tempImg)
        End Using
    End Function

    Private Sub EnsureActionColumns()
        If Not DGVdeliveries.Columns.Contains(ColGridEdit) Then
            Dim editCol As New DataGridViewButtonColumn() With {
                .Name = ColGridEdit,
                .HeaderText = "Edit",
                .Text = "Edit",
                .UseColumnTextForButtonValue = True,
                .Width = 100,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
            }
            DGVdeliveries.Columns.Add(editCol)
        End If

        If Not DGVdeliveries.Columns.Contains(ColGridDelete) Then
            Dim deleteCol As New DataGridViewButtonColumn() With {
                .Name = ColGridDelete,
                .HeaderText = "Delete",
                .Text = "Delete",
                .UseColumnTextForButtonValue = True,
                .Width = 100,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
            }
            DGVdeliveries.Columns.Add(deleteCol)
        End If
    End Sub

    Private Function FindPendingRowByTempId(tempRowId As Integer) As DataRow
        EnsureDtPending()
        Dim foundRows() As DataRow = dtPending.Select(ColTempRowId & " = " & tempRowId.ToString())
        If foundRows.Length = 0 Then Return Nothing
        Return foundRows(0)
    End Function

    Private Sub EditPendingRowByTempId(tempRowId As Integer)
        Dim rowToEdit As DataRow = FindPendingRowByTempId(tempRowId)
        If rowToEdit Is Nothing Then
            MessageBox.Show("Unable to find product in the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim productId As Integer = Convert.ToInt32(rowToEdit("ProductID"))
        Dim f As New FrmDeliveryProductEntry With {
            .ProductID = productId,
            .SelectedQuantity = Convert.ToInt32(rowToEdit("Quantity")),
            .SelectedCostPrice = Convert.ToDecimal(rowToEdit("CostPrice")),
            .SelectedSellingPrice = Convert.ToDecimal(rowToEdit("SellingPrice"))
        }

        If f.ShowDialog() <> DialogResult.OK Then Return

        Dim duplicateRows() As DataRow = dtPending.Select(
            "ProductID = " & f.selectedID.ToString() & " AND " & ColTempRowId & " <> " & tempRowId.ToString())

        If duplicateRows.Length > 0 Then
            duplicateRows(0)("Quantity") = Convert.ToInt32(duplicateRows(0)("Quantity")) + f.SelectedQuantity
            duplicateRows(0)("CostPrice") = f.SelectedCostPrice
            duplicateRows(0)("SellingPrice") = f.SelectedSellingPrice
            duplicateRows(0)("BarcodeNumber") = f.SelectedBarcode
            duplicateRows(0)("Product") = f.SelectedProductName
            duplicateRows(0)("ImagePath") = f.SelectedImagePath
            duplicateRows(0)("ProductImage") = LoadImageFromPath(f.SelectedImagePath)
            dtPending.Rows.Remove(rowToEdit)
            MessageBox.Show("Product merged with an existing row and updated.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            rowToEdit("ProductID") = f.selectedID
            rowToEdit("BarcodeNumber") = f.SelectedBarcode
            rowToEdit("Product") = f.SelectedProductName
            rowToEdit("Quantity") = f.SelectedQuantity
            rowToEdit("CostPrice") = f.SelectedCostPrice
            rowToEdit("SellingPrice") = f.SelectedSellingPrice
            rowToEdit("ImagePath") = f.SelectedImagePath
            rowToEdit("ProductImage") = LoadImageFromPath(f.SelectedImagePath)
        End If

        displayData()
        DGVdeliveries.ClearSelection()
    End Sub

    Private Sub DeletePendingRowByTempId(tempRowId As Integer)
        Dim rowToDelete As DataRow = FindPendingRowByTempId(tempRowId)
        If rowToDelete Is Nothing Then
            MessageBox.Show("Unable to find product in the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If MessageBox.Show("Are you sure you want to remove this product from the list?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        dtPending.Rows.Remove(rowToDelete)
        displayData()
        DGVdeliveries.ClearSelection()
    End Sub

    Private Sub displayData()
        EnsureDtPending()

        For Each row As DataRow In dtPending.Rows
            Dim path As String = If(row("ImagePath") IsNot Nothing, row("ImagePath").ToString(), String.Empty)
            If Not String.IsNullOrEmpty(path) AndAlso IO.File.Exists(path) Then
                Using tempImg As Image = Image.FromFile(path)
                    row("ProductImage") = New Bitmap(tempImg)
                End Using
            Else
                row("ProductImage") = Nothing
            End If
        Next

        DGVdeliveries.DataSource = dtPending
        EnsureActionColumns()

        If DGVdeliveries.Columns.Contains(ColTempRowId) Then DGVdeliveries.Columns(ColTempRowId).Visible = False
        If DGVdeliveries.Columns.Contains("ProductID") Then DGVdeliveries.Columns("ProductID").Visible = False
        If DGVdeliveries.Columns.Contains("ImagePath") Then DGVdeliveries.Columns("ImagePath").Visible = False

        If DGVdeliveries.Columns.Contains("ProductImage") Then
            DGVdeliveries.Columns("ProductImage").DisplayIndex = 0
            DirectCast(DGVdeliveries.Columns("ProductImage"), DataGridViewImageColumn).ImageLayout = DataGridViewImageCellLayout.Zoom
            DGVdeliveries.Columns("ProductImage").Width = 80
        End If

        DGVdeliveries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        For Each col As DataGridViewColumn In DGVdeliveries.Columns
            If col.Name = "ProductImage" OrElse col.Name = ColGridEdit OrElse col.Name = ColGridDelete Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Else
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
        Next

        ApplyStandardGridLayout(DGVdeliveries)
        DGVdeliveries.ClearSelection()
    End Sub

    Public Sub generateorderNumber()
        Dim rnd As New Random()
        Dim orderNumber As String = DateTime.Now.ToString("yyyyMMddHHmmss") & rnd.Next(10, 99).ToString()
        txtOrderNumber.Text = orderNumber
    End Sub

    Private Sub FrmDeliveryEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _isLoading = True
        Try
            LoadSuppliers()

            dtpShipDate.MaxDate = DateTime.Today

            dtPending = Nothing
            _nextTempRowId = 1
            EnsureDtPending()

            If IsEditMode Then
                Me.Text = "Edit Delivery"
                LoadDeliveryDetails(SelectedId)
            Else
                Me.Text = "Add Delivery"
                generateorderNumber()
            End If

            displayData()

            Dim tooltip As New ToolTip()
            tooltip.SetToolTip(cbCompany, "Select a supplier (company).")
            tooltip.SetToolTip(dtpShipDate, "Select the delivery date.")
            tooltip.SetToolTip(btnAdd, "Add a product to the delivery.")
            tooltip.SetToolTip(btnSave, "Save the delivery.")
        Finally
            _isLoading = False
        End Try
    End Sub

    Private Sub LoadSuppliers()
        Dim dt As DataTable = _service.GetSupplierLookup()

        Dim row As DataRow = dt.NewRow()
        row("SupplierID") = 0
        row("Company") = "-- Select Supplier --"
        dt.Rows.InsertAt(row, 0)

        cbCompany.DataSource = dt
        cbCompany.DisplayMember = "Company"
        cbCompany.ValueMember = "SupplierID"
        cbCompany.SelectedIndex = 0
    End Sub

    Private Sub DGVsize_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVdeliveries.DataBindingComplete
        ApplyStandardGridLayout(DGVdeliveries)
        DGVdeliveries.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
        DGVdeliveries.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
        DGVdeliveries.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub

    Public Sub LoadDeliveryDetails(deliveryId As Integer)
        EnsureDtPending()
        dtPending.Clear()
        _nextTempRowId = 1

        Dim header As DataRow = _service.GetDeliveryHeaderById(deliveryId)
        If header Is Nothing Then
            MessageBox.Show("Selected delivery record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not IsDBNull(header("OrderNumber")) Then txtOrderNumber.Text = header("OrderNumber").ToString()

        If Not IsDBNull(header("DeliveryDate")) Then
            Dim deliveryDate As DateTime = Convert.ToDateTime(header("DeliveryDate"))
            If deliveryDate < dtpShipDate.MinDate Then
                dtpShipDate.Value = dtpShipDate.MinDate
            ElseIf deliveryDate > dtpShipDate.MaxDate Then
                dtpShipDate.Value = dtpShipDate.MaxDate
            Else
                dtpShipDate.Value = deliveryDate
            End If
        End If

        If Not IsDBNull(header("SupplierID")) Then
            Dim supplierId As Integer = Convert.ToInt32(header("SupplierID"))
            Try
                If supplierId > 0 Then
                    _isLoading = True
                    cbCompany.SelectedValue = supplierId
                End If
            Catch
            Finally
                _isLoading = False
            End Try
        End If

        Dim products As DataTable = _service.GetDeliveryProductsByDeliveryId(deliveryId)
        dtPending.Clear()
        For Each r As DataRow In products.Rows
            Dim nr As DataRow = dtPending.NewRow()
            nr(ColTempRowId) = GetNextTempRowId()
            nr("ProductID") = r("ProductID")
            nr("BarcodeNumber") = r("BarcodeNumber")
            nr("Product") = r("Product")
            nr("Quantity") = r("Quantity")
            nr("CostPrice") = r("CostPrice")
            nr("SellingPrice") = r("SellingPrice")
            nr("ImagePath") = r("ImagePath")
            nr("ProductImage") = Nothing
            dtPending.Rows.Add(nr)
        Next

        For Each r As DataRow In dtPending.Rows
            Dim path As String = If(r("ImagePath") IsNot Nothing, r("ImagePath").ToString(), String.Empty)
            If Not String.IsNullOrEmpty(path) AndAlso IO.File.Exists(path) Then
                Using tempImg As Image = Image.FromFile(path)
                    r("ProductImage") = New Bitmap(tempImg)
                End Using
            Else
                r("ProductImage") = Nothing
            End If
        Next

        displayData()
    End Sub

    Private Sub cbCompany_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbCompany.SelectedIndexChanged
    End Sub

    Private Sub cbCompany_DropDown(sender As Object, e As EventArgs) Handles cbCompany.DropDown
        If cbCompany.Items.Count <= 1 Then
            _isLoading = True
            Try
                LoadSuppliers()
            Finally
                _isLoading = False
            End Try
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        EnsureDtPending()

        Dim f As New FrmDeliveryProductEntry
        If f.ShowDialog() = DialogResult.OK Then
            Dim imgPath As String = f.SelectedImagePath
            Dim productImage As Image = LoadImageFromPath(imgPath)

            Dim existingRows() As DataRow = dtPending.Select("ProductID = " & f.selectedID.ToString())
            If existingRows.Length > 0 Then
                existingRows(0)("Quantity") = Convert.ToInt32(existingRows(0)("Quantity")) + f.SelectedQuantity
                existingRows(0)("CostPrice") = f.SelectedCostPrice
                existingRows(0)("SellingPrice") = f.SelectedSellingPrice
                MessageBox.Show("Product already exists; quantity updated.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                dtPending.Rows.Add(GetNextTempRowId(),
                               f.selectedID,
                               f.SelectedBarcode,
                               f.SelectedProductName,
                               f.SelectedQuantity,
                               f.SelectedCostPrice,
                               f.SelectedSellingPrice,
                               imgPath,
                               productImage)
                MessageBox.Show("Product added to delivery list!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            displayData()
            DGVdeliveries.ClearSelection()
        End If
    End Sub

    Private Sub DGVdeliveries_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVdeliveries.CellContentClick
        If e.RowIndex < 0 Then Exit Sub
        If e.ColumnIndex < 0 Then Exit Sub

        Dim colName As String = DGVdeliveries.Columns(e.ColumnIndex).Name
        If colName <> ColGridEdit AndAlso colName <> ColGridDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVdeliveries.Rows(e.RowIndex)
        If Not DGVdeliveries.Columns.Contains(ColTempRowId) OrElse row.Cells(ColTempRowId).Value Is Nothing Then
            MessageBox.Show("Unable to identify selected row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim tempRowId As Integer = Convert.ToInt32(row.Cells(ColTempRowId).Value)
        If colName = ColGridEdit Then
            EditPendingRowByTempId(tempRowId)
        ElseIf colName = ColGridDelete Then
            DeletePendingRowByTempId(tempRowId)
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If cbCompany.SelectedIndex <= 0 OrElse cbCompany.SelectedValue Is Nothing OrElse Convert.ToInt32(cbCompany.SelectedValue) = 0 Then
            MessageBox.Show("Please select a supplier.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        EnsureDtPending()
        If dtPending.Rows.Count = 0 Then
            MessageBox.Show("Please add at least one product.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim actionText As String = If(IsEditMode, "update", "save")
        If MessageBox.Show($"Are you sure you want to {actionText} this delivery?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            _service.SaveDelivery(Mode, SelectedId, Convert.ToInt32(cbCompany.SelectedValue), txtOrderNumber.Text.Trim(), dtpShipDate.Value.Date, dtPending)

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Deliveries.", "Added Deliveries."))
            MessageBox.Show(If(IsEditMode, "Delivery updated successfully!", "Delivery saved successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error saving delivery: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
