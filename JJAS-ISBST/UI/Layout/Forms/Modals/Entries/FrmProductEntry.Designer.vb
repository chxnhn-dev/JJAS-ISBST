<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmProductEntry
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.pnlMainContainer = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.btnAdd = New Guna.UI2.WinForms.Guna2Button()
        Me.btnExit = New Guna.UI2.WinForms.Guna2Button()
        Me.btnBrowse = New Guna.UI2.WinForms.Guna2Button()
        Me.btnCancel = New Guna.UI2.WinForms.Guna2Button()
        Me.btnClear = New Guna.UI2.WinForms.Guna2Button()
        Me.PictureBox1 = New Guna.UI2.WinForms.Guna2PictureBox()
        Me.txtBarcodeNumber = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtProductName = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtCostPrice = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtSellingPrice = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtDescription = New Guna.UI2.WinForms.Guna2TextBox()
        Me.cbCategory = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.cbBrand = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.cbSize = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.cbColor = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.Guna2BorderlessForm1 = New Guna.UI2.WinForms.Guna2BorderlessForm(Me.components)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlMainContainer
        '
        Me.pnlMainContainer.Location = New System.Drawing.Point(0, 0)
        Me.pnlMainContainer.Name = "pnlMainContainer"
        Me.pnlMainContainer.Size = New System.Drawing.Size(200, 100)
        Me.pnlMainContainer.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(100, 23)
        Me.lblTitle.TabIndex = 0
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnAdd.ForeColor = System.Drawing.Color.White
        Me.btnAdd.Location = New System.Drawing.Point(0, 0)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(180, 45)
        Me.btnAdd.TabIndex = 0
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnExit.ForeColor = System.Drawing.Color.White
        Me.btnExit.Location = New System.Drawing.Point(0, 0)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(180, 45)
        Me.btnExit.TabIndex = 0
        '
        'btnBrowse
        '
        Me.btnBrowse.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnBrowse.ForeColor = System.Drawing.Color.White
        Me.btnBrowse.Location = New System.Drawing.Point(0, 0)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(180, 45)
        Me.btnBrowse.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnCancel.ForeColor = System.Drawing.Color.White
        Me.btnCancel.Location = New System.Drawing.Point(0, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(180, 45)
        Me.btnCancel.TabIndex = 0
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnClear.ForeColor = System.Drawing.Color.White
        Me.btnClear.Location = New System.Drawing.Point(0, 0)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(180, 45)
        Me.btnClear.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.ImageRotate = 0!
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(300, 200)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'txtBarcodeNumber
        '
        Me.txtBarcodeNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBarcodeNumber.DefaultText = ""
        Me.txtBarcodeNumber.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtBarcodeNumber.Location = New System.Drawing.Point(0, 0)
        Me.txtBarcodeNumber.Name = "txtBarcodeNumber"
        Me.txtBarcodeNumber.PlaceholderText = ""
        Me.txtBarcodeNumber.SelectedText = ""
        Me.txtBarcodeNumber.Size = New System.Drawing.Size(200, 36)
        Me.txtBarcodeNumber.TabIndex = 0
        '
        'txtProductName
        '
        Me.txtProductName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProductName.DefaultText = ""
        Me.txtProductName.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtProductName.Location = New System.Drawing.Point(0, 0)
        Me.txtProductName.Name = "txtProductName"
        Me.txtProductName.PlaceholderText = ""
        Me.txtProductName.SelectedText = ""
        Me.txtProductName.Size = New System.Drawing.Size(200, 36)
        Me.txtProductName.TabIndex = 0
        '
        'txtCostPrice
        '
        Me.txtCostPrice.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCostPrice.DefaultText = ""
        Me.txtCostPrice.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtCostPrice.Location = New System.Drawing.Point(0, 0)
        Me.txtCostPrice.Name = "txtCostPrice"
        Me.txtCostPrice.PlaceholderText = ""
        Me.txtCostPrice.SelectedText = ""
        Me.txtCostPrice.Size = New System.Drawing.Size(200, 36)
        Me.txtCostPrice.TabIndex = 0
        '
        'txtSellingPrice
        '
        Me.txtSellingPrice.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSellingPrice.DefaultText = ""
        Me.txtSellingPrice.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtSellingPrice.Location = New System.Drawing.Point(0, 0)
        Me.txtSellingPrice.Name = "txtSellingPrice"
        Me.txtSellingPrice.PlaceholderText = ""
        Me.txtSellingPrice.SelectedText = ""
        Me.txtSellingPrice.Size = New System.Drawing.Size(200, 36)
        Me.txtSellingPrice.TabIndex = 0
        '
        'txtDescription
        '
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.DefaultText = ""
        Me.txtDescription.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtDescription.Location = New System.Drawing.Point(0, 0)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.PlaceholderText = ""
        Me.txtDescription.SelectedText = ""
        Me.txtDescription.Size = New System.Drawing.Size(200, 36)
        Me.txtDescription.TabIndex = 0
        '
        'cbCategory
        '
        Me.cbCategory.BackColor = System.Drawing.Color.Transparent
        Me.cbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCategory.FocusedColor = System.Drawing.Color.Empty
        Me.cbCategory.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cbCategory.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(112, Byte), Integer))
        Me.cbCategory.ItemHeight = 30
        Me.cbCategory.Location = New System.Drawing.Point(0, 0)
        Me.cbCategory.Name = "cbCategory"
        Me.cbCategory.Size = New System.Drawing.Size(140, 36)
        Me.cbCategory.TabIndex = 0
        '
        'cbBrand
        '
        Me.cbBrand.BackColor = System.Drawing.Color.Transparent
        Me.cbBrand.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbBrand.FocusedColor = System.Drawing.Color.Empty
        Me.cbBrand.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cbBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(112, Byte), Integer))
        Me.cbBrand.ItemHeight = 30
        Me.cbBrand.Location = New System.Drawing.Point(0, 0)
        Me.cbBrand.Name = "cbBrand"
        Me.cbBrand.Size = New System.Drawing.Size(140, 36)
        Me.cbBrand.TabIndex = 0
        '
        'cbSize
        '
        Me.cbSize.BackColor = System.Drawing.Color.Transparent
        Me.cbSize.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cbSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSize.FocusedColor = System.Drawing.Color.Empty
        Me.cbSize.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cbSize.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(112, Byte), Integer))
        Me.cbSize.ItemHeight = 30
        Me.cbSize.Location = New System.Drawing.Point(0, 0)
        Me.cbSize.Name = "cbSize"
        Me.cbSize.Size = New System.Drawing.Size(140, 36)
        Me.cbSize.TabIndex = 0
        '
        'cbColor
        '
        Me.cbColor.BackColor = System.Drawing.Color.Transparent
        Me.cbColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbColor.FocusedColor = System.Drawing.Color.Empty
        Me.cbColor.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cbColor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(112, Byte), Integer))
        Me.cbColor.ItemHeight = 30
        Me.cbColor.Location = New System.Drawing.Point(0, 0)
        Me.cbColor.Name = "cbColor"
        Me.cbColor.Size = New System.Drawing.Size(140, 36)
        Me.cbColor.TabIndex = 0
        '
        'Guna2BorderlessForm1
        '
        Me.Guna2BorderlessForm1.AnimateWindow = True
        Me.Guna2BorderlessForm1.BorderRadius = 30
        Me.Guna2BorderlessForm1.ContainerControl = Me
        Me.Guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6R
        Me.Guna2BorderlessForm1.TransparentWhileDrag = True
        '
        'FrmProductEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(22, Byte), Integer), CType(CType(25, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1180, 760)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MinimumSize = New System.Drawing.Size(1180, 760)
        Me.Name = "FrmProductEntry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmProductEntry"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlMainContainer As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents btnAdd As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnExit As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnBrowse As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnCancel As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnClear As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents PictureBox1 As Guna.UI2.WinForms.Guna2PictureBox
    Friend WithEvents txtBarcodeNumber As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtProductName As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtCostPrice As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtSellingPrice As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtDescription As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents cbCategory As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents cbBrand As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents cbSize As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents cbColor As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents Guna2BorderlessForm1 As Guna.UI2.WinForms.Guna2BorderlessForm
End Class
