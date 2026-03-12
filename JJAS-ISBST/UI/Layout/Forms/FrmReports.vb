Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports Guna.UI2.WinForms

Public Class FrmReports
    Private Enum ReportType
        Sales
        Financial
        Cashier
        Delivery
        Returns
    End Enum

    Private Class FilterCriteria
        Public Property WhereClause As String
        Public Property Parameters As List(Of SqlParameter)
        Public Property CashierFilter As String
    End Class

    Private Class ReportTotals
        Public Property TotalQtySold As Decimal
        Public Property GrossSales As Decimal
        Public Property TotalDiscount As Decimal
        Public Property TotalVat As Decimal
        Public Property NetSales As Decimal
        Public Property TotalCost As Decimal
        Public Property EstimatedProfit As Decimal
        Public Property RecordCount As Integer
        Public Property TotalItemCount As Decimal
        Public Property TotalQuantity As Decimal
        Public Property TotalReturnedQuantity As Decimal
        Public Property AggregateCost As Decimal
    End Class

    Private Class PrintColumn
        Public Property FieldName As String
        Public Property Header As String
        Public Property AlignRight As Boolean
        Public Property FormatKind As String
    End Class

    Private ReadOnly _activeFillColor As Color = Color.FromArgb(75, 78, 75)
    Private ReadOnly _inactiveFillColor As Color = Color.FromArgb(50, 52, 50)
    Private ReadOnly _tabPanelColor As Color = Color.FromArgb(30, 32, 30)
    Private ReadOnly _activeSidebarFillColor As Color = Color.FromArgb(30, 32, 30)
    Private ReadOnly _inactiveSidebarFillColor As Color = Color.Black

    Private ReadOnly _printDocument As New PrintDocument()
    Private Const _printHeaderStoreName As String = "JJOMS APPAREL"
    Private ReadOnly _tabAnimationTimer As New Timer() With {.Interval = 15}
    Private _tabAnimationStartUtc As DateTime
    Private Const _tabAnimationDurationMs As Double = 180.0
    Private _tabFromSalesColor As Color
    Private _tabFromFinancialColor As Color
    Private _tabFromCashierColor As Color
    Private _tabFromDeliveryColor As Color
    Private _tabFromReturnsColor As Color
    Private _tabToSalesColor As Color
    Private _tabToFinancialColor As Color
    Private _tabToCashierColor As Color
    Private _tabToDeliveryColor As Color
    Private _tabToReturnsColor As Color

    Private _activeReport As ReportType = ReportType.Sales
    Private _isLoadingDateFilters As Boolean
    Private _isLoadingCashierFilter As Boolean
    Private _allRows As New DataTable()
    Private _currentTotals As New ReportTotals()
    Private _totalsLabel As Guna2HtmlLabel
    Private _currentPage As Integer = 1
    Private Const _pageSize As Integer = 50
    Private _deliveryCostColumnAvailable As Boolean = True

    Private _printRows As DataTable
    Private _printColumns As List(Of PrintColumn)
    Private _printRowIndex As Integer
    Private _printPageNumber As Integer = 1

    Private Sub FrmReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AddHandler _printDocument.PrintPage, AddressOf PrintDocument_PrintPage
        AddHandler _tabAnimationTimer.Tick, AddressOf TabAnimationTimer_Tick

        ApplySidebarUserPanel()
        InitializeDateFilters()
        LoadCashierNames()
        ConfigureSidebarPermissions()
        ConfigureReportTabPermissions()
        SwitchSidebarNavigation(btnReports)
        Dim defaultReport As ReportType
        If IsCashierUser() Then
            defaultReport = ReportType.Cashier
        ElseIf IsStaffUser() Then
            defaultReport = ReportType.Delivery
        Else
            defaultReport = ReportType.Sales
        End If
        SetActiveReport(defaultReport, loadData:=False)
        LoadCurrentReport()
    End Sub

    Private Sub EnsureTotalsLabel()
        If _totalsLabel IsNot Nothing Then Return

        _totalsLabel = New Guna2HtmlLabel()
        _totalsLabel.BackColor = Color.Transparent
        _totalsLabel.ForeColor = Color.White
        _totalsLabel.Font = New Font("Arial", 9.0F, FontStyle.Bold)
        _totalsLabel.Location = New Point(520, 24)
        _totalsLabel.Size = New Size(875, 44)
        _totalsLabel.Text = "Total Qty Sold: 0 | Gross Sales: 0.00 | Total Discount: 0.00 | Total VAT: 0.00 | Net Sales: 0.00"

        Guna2Panel4.Controls.Add(_totalsLabel)
        _totalsLabel.BringToFront()
    End Sub

    Private Sub InitializeDateFilters()
        _isLoadingDateFilters = True
        Try
            dtpFromDate.Value = Date.Today
            dtpTodate.Value = Date.Today
        Catch ex As Exception
            MessageBox.Show("Failed to load date filters: " & ex.Message,
                            "Reports",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
        Finally
            _isLoadingDateFilters = False
        End Try
    End Sub

    Private Sub LoadCashierNames()
        _isLoadingCashierFilter = True
        Try
            If IsCashierUser() Then
                Dim cashierItems As New DataTable()
                cashierItems.Columns.Add("DisplayText", GetType(String))
                cashierItems.Columns.Add("ValueText", GetType(String))

                Dim loggedCashier As String = ResolveLoggedCashierName()
                If loggedCashier.Length = 0 Then
                    cashierItems.Rows.Add("Cashier", String.Empty)
                Else
                    Dim userIdValue As String = If(FrmLogin.CurrentUser.UserID > 0, FrmLogin.CurrentUser.UserID.ToString(), String.Empty)
                    cashierItems.Rows.Add(loggedCashier, userIdValue)
                End If

                cbCashierName.DataSource = cashierItems
                cbCashierName.DisplayMember = "DisplayText"
                cbCashierName.ValueMember = "ValueText"
                cbCashierName.SelectedIndex = 0
                Return
            End If

            Dim items As New DataTable()
            items.Columns.Add("DisplayText", GetType(String))
            items.Columns.Add("ValueText", GetType(String))
            items.Rows.Add("All Cashiers", String.Empty)

            Dim cashierQuery As String = BuildCashierNamesQuery()
            Dim dtCashiers As DataTable = Db.QueryDataTable(cashierQuery)
            Dim seen As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

            For Each row As DataRow In dtCashiers.Rows
                Dim cashierName As String = Convert.ToString(row("CashierName")).Trim()
                If cashierName.Length = 0 Then Continue For
                If seen.Contains(cashierName) Then Continue For

                Dim cashierIdValue As String = Convert.ToString(row("UserID")).Trim()
                items.Rows.Add(cashierName, cashierIdValue)
                seen.Add(cashierName)
            Next

            cbCashierName.DataSource = items
            cbCashierName.DisplayMember = "DisplayText"
            cbCashierName.ValueMember = "ValueText"
            cbCashierName.SelectedIndex = 0
        Catch ex As Exception
            cbCashierName.DataSource = Nothing
            cbCashierName.Items.Clear()
            cbCashierName.Items.Add("All Cashiers")
            cbCashierName.SelectedIndex = 0

            MessageBox.Show("Failed to load cashier names: " & ex.Message,
                            "Reports",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
        Finally
            _isLoadingCashierFilter = False
        End Try
    End Sub

    Private Function BuildCashierNamesQuery() As String
        Dim displayExpr As String = GetUserDisplayExpression("u")
        Return "SELECT DISTINCT CashierName, UserID " &
               "FROM (" &
               "    SELECT u.UserID, " & displayExpr & " AS CashierName " &
               "    FROM dbo.tbl_User u " &
               "    WHERE LOWER(ISNULL(u.Role, '')) = 'cashier'" &
               ") AS cashierList " &
               "WHERE LTRIM(RTRIM(ISNULL(CashierName, ''))) <> '' " &
               "ORDER BY CashierName;"
    End Function

    Private Function GetUserDisplayExpression(userAlias As String) As String
        Dim safeAlias As String = If(String.IsNullOrWhiteSpace(userAlias), "u", userAlias.Trim())
        Dim hasFullName As Boolean = HasTableColumn("dbo.tbl_User", "FullName")
        Dim hasName As Boolean = HasTableColumn("dbo.tbl_User", "Name")

        Dim baseExpr As String
        If hasFullName Then
            baseExpr = $"LTRIM(RTRIM(ISNULL({safeAlias}.FullName, '')))"
        ElseIf hasName Then
            baseExpr = $"LTRIM(RTRIM(ISNULL({safeAlias}.[Name], '')))"
        Else
            baseExpr = $"LTRIM(RTRIM(COALESCE({safeAlias}.FirstName, '') + " &
                       $"CASE WHEN ISNULL({safeAlias}.FirstName, '') <> '' AND ISNULL({safeAlias}.LastName, '') <> '' THEN ' ' ELSE '' END + " &
                       $"COALESCE({safeAlias}.LastName, '')))"
        End If

        Return $"COALESCE(NULLIF({baseExpr}, ''), LTRIM(RTRIM(ISNULL({safeAlias}.Username, ''))))"
    End Function

    Public Sub LoadCurrentReport()
        Select Case _activeReport
            Case ReportType.Sales
                LoadSalesReport()
            Case ReportType.Financial
                LoadFinancialReport()
            Case ReportType.Cashier
                LoadCashierReport()
            Case ReportType.Delivery
                LoadDeliveryReport()
            Case ReportType.Returns
                LoadReturnsReport()
        End Select
    End Sub

    Public Sub LoadSalesReport()
        Dim cashierExpr As String = GetUserDisplayExpression("u")
        Dim searchCols As String() = {"st.TransactionNo", cashierExpr}
        Dim criteria As FilterCriteria = BuildFilterCriteria(searchCols,
                                                             dateColumn:="st.SaleDate",
                                                             cashierIdColumn:="st.UserID",
                                                             cashierNameColumn:=cashierExpr)

        Dim sql As String =
            "SELECT " &
            "    st.SaleDate, " &
            "    st.TransactionNo, " &
            "    " & cashierExpr & " AS Cashier, " &
            "    st.TotalItems, " &
            "    st.TotalDiscount, " &
            "    st.TotalVAT, " &
            "    st.TotalAmount " &
            "FROM dbo.tbl_SalesTransaction st " &
            "LEFT JOIN dbo.tbl_User u ON st.UserID = u.UserID " &
            "WHERE " & criteria.WhereClause & " " &
            "ORDER BY st.SaleDate DESC, st.TransactionID DESC;"

        BindReport(sql, criteria, searchCols)
        ApplySalesGridFormatting()
    End Sub

    Public Sub LoadFinancialReport()
        Dim useMonthly As Boolean
        Dim searchCols As String() = Nothing
        Dim criteria As FilterCriteria = BuildFinancialCriteria(useMonthly, searchCols)

        Dim sql As String
        If useMonthly Then
            sql =
                "SELECT " &
                "    DATEFROMPARTS(SalesYear, SalesMonth, 1) AS ReportDate, " &
                "    GrossSales, " &
                "    TotalCost, " &
                "    TotalDiscount, " &
                "    TotalVAT, " &
                "    NetSales, " &
                "    Profit " &
                "FROM dbo.vw_FinancialReport_Monthly " &
                "WHERE " & criteria.WhereClause & " " &
                "ORDER BY SalesYear DESC, SalesMonth DESC;"
        Else
            sql =
                "SELECT " &
                "    CAST(ReportDate AS datetime) AS ReportDate, " &
                "    GrossSales, " &
                "    TotalCost, " &
                "    TotalDiscount, " &
                "    TotalVAT, " &
                "    NetSales, " &
                "    Profit " &
                "FROM dbo.vw_FinancialReport_Daily " &
                "WHERE " & criteria.WhereClause & " " &
                "ORDER BY ReportDate DESC;"
        End If

        BindReport(sql, criteria, searchCols)
        ApplyFinancialGridFormatting()
    End Sub

    Public Sub LoadCashierReport()
        Dim cashierExpr As String = GetUserDisplayExpression("u")
        Dim searchCols As String() = {cashierExpr, "st.TransactionNo"}
        Dim criteria As FilterCriteria = BuildFilterCriteria(searchCols,
                                                             includeCashierFilter:=True,
                                                             dateColumn:="st.SaleDate",
                                                             cashierIdColumn:="st.UserID",
                                                             cashierNameColumn:=cashierExpr)

        Dim sql As String =
            "SELECT " &
            "    " & cashierExpr & " AS [Cashier], " &
            "    st.TransactionNo AS [Transaction No], " &
            "    st.TotalAmount AS [Total Amount] " &
            "FROM dbo.tbl_SalesTransaction st " &
            "LEFT JOIN dbo.tbl_User u ON st.UserID = u.UserID " &
            "WHERE " & criteria.WhereClause & " " &
            "ORDER BY st.SaleDate DESC, st.TransactionID DESC;"

        BindReport(sql, criteria, searchCols, includeCashierFilter:=True)
        ApplyCashierGridFormatting()
    End Sub

    Public Sub LoadDeliveryReport()
        Dim searchCols As String() = {
            "ISNULL(d.DeliveryNumber, '')",
            "ISNULL(d.OrderNumber, '')",
            "COALESCE(NULLIF(LTRIM(RTRIM(s.SupplierName)), ''), s.Company)",
            "ISNULL(dp.Status, 'Pending')"
        }
        Dim criteria As FilterCriteria = BuildFilterCriteria(searchCols, dateColumn:="d.DeliveryDate")
        _deliveryCostColumnAvailable = HasTableColumn("dbo.tbl_Delivery_Products", "CostPrice")

        Dim totalCostExpression As String =
            If(_deliveryCostColumnAvailable,
               "SUM(ISNULL(dp.Quantity, 0) * ISNULL(dp.CostPrice, 0)) AS TotalCost",
               "CAST(0 AS decimal(18, 2)) AS TotalCost")

        Dim sql As String =
            "SELECT " &
            "    d.DeliveryDate, " &
            "    ISNULL(d.DeliveryNumber, '') AS DeliveryNo, " &
            "    ISNULL(d.OrderNumber, '') AS OrderNo, " &
            "    COALESCE(NULLIF(LTRIM(RTRIM(s.SupplierName)), ''), s.Company) AS Supplier, " &
            "    COUNT(dp.DeliveryProductID) AS ItemCount, " &
            "    SUM(ISNULL(dp.Quantity, 0)) AS TotalQty, " &
            "    CASE " &
            "        WHEN SUM(CASE WHEN ISNULL(dp.Status, 'Pending') = 'Pending' THEN 1 ELSE 0 END) = 0 " &
            "             AND SUM(CASE WHEN ISNULL(dp.Status, '') = 'Posted' THEN 1 ELSE 0 END) > 0 " &
            "            THEN 'Posted' " &
            "        WHEN SUM(CASE WHEN ISNULL(dp.Status, '') = 'Posted' THEN 1 ELSE 0 END) > 0 " &
            "            THEN 'Partially Posted' " &
            "        ELSE 'Pending' " &
            "    END AS Status, " &
            "    " & totalCostExpression & " " &
            "FROM dbo.tbl_Deliveries d " &
            "INNER JOIN dbo.tbl_Supplier s ON d.SupplierID = s.SupplierID " &
            "INNER JOIN dbo.tbl_Delivery_Products dp ON dp.DeliveryID = d.DeliveryID " &
            "WHERE " & criteria.WhereClause & " " &
            "GROUP BY d.DeliveryID, d.DeliveryDate, d.DeliveryNumber, d.OrderNumber, " &
            "         COALESCE(NULLIF(LTRIM(RTRIM(s.SupplierName)), ''), s.Company) " &
            "ORDER BY d.DeliveryDate DESC, d.DeliveryID DESC;"

        BindReport(sql, criteria, searchCols)
        ApplyDeliveryGridFormatting()
    End Sub

    Public Sub LoadReturnsReport()
        Dim searchCols As String() = {
            "ISNULL(sr.ReturnNumber, '')",
            "ISNULL(d.DeliveryNumber, '')",
            "ISNULL(d.OrderNumber, '')",
            "COALESCE(NULLIF(LTRIM(RTRIM(s.SupplierName)), ''), s.Company)",
            "ISNULL(sr.ReturnType, '')",
            "ISNULL(sr.Resolution, '')",
            "ISNULL(sr.Status, '')",
            "ISNULL(sr.Notes, '')"
        }
        Dim criteria As FilterCriteria = BuildFilterCriteria(searchCols, dateColumn:="sr.ReturnDate")

        Dim sql As String =
            "SELECT " &
            "    ISNULL(sr.ReturnNumber, '') AS ReturnNo, " &
            "    COALESCE(NULLIF(LTRIM(RTRIM(s.SupplierName)), ''), s.Company) AS Supplier, " &
            "    ISNULL(d.DeliveryNumber, '') AS DeliveryNo, " &
            "    sr.ReturnDate, " &
            "    ISNULL(sr.ReturnType, '') AS ReturnType, " &
            "    ISNULL(sr.Resolution, '') AS Resolution, " &
            "    ISNULL(itemTotals.ItemCount, 0) AS ItemCount, " &
            "    ISNULL(itemTotals.TotalReturnedQty, 0) AS TotalReturnedQty, " &
            "    ISNULL(sr.Status, '') AS Status, " &
            "    ISNULL(sr.Notes, '') AS Notes " &
            "FROM dbo.tbl_Supplier_Returns sr " &
            "INNER JOIN dbo.tbl_Deliveries d ON sr.DeliveryID = d.DeliveryID " &
            "INNER JOIN dbo.tbl_Supplier s ON sr.SupplierID = s.SupplierID " &
            "OUTER APPLY ( " &
            "    SELECT COUNT(1) AS ItemCount, " &
            "           SUM(ISNULL(sri.QtyReturned, 0)) AS TotalReturnedQty " &
            "    FROM dbo.tbl_Supplier_Return_Items sri " &
            "    WHERE sri.ReturnID = sr.ReturnID " &
            ") itemTotals " &
            "WHERE " & criteria.WhereClause & " " &
            "ORDER BY sr.ReturnDate DESC, sr.ReturnID DESC;"

        BindReport(sql, criteria, searchCols)
        ApplyReturnsGridFormatting()
    End Sub

    Private Sub BindReport(sql As String,
                           criteria As FilterCriteria,
                           searchColumns As IEnumerable(Of String),
                           Optional includeCashierFilter As Boolean = False)
        Try
            _allRows = Db.QueryDataTable(sql, criteria.Parameters.ToArray())
            _currentPage = 1
            ApplyCurrentPageData()
            _currentTotals = GetReportTotals(searchColumns, includeCashierFilter)
            UpdateTotalsDisplay()
            DGVtable.ClearSelection()
        Catch ex As Exception
            DGVtable.DataSource = Nothing
            _allRows = New DataTable()
            _currentTotals = New ReportTotals()
            _currentPage = 1
            lblPage.Text = "Page 1 of 1"
            btnPreviousPage.Enabled = False
            btnNextPage.Enabled = False
            UpdateTotalsDisplay()
            MessageBox.Show("Failed to load report: " & ex.Message,
                            "Reports",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ApplyCurrentPageData()
        Dim source As DataTable = If(_allRows, New DataTable())
        Dim totalRows As Integer = source.Rows.Count
        Dim totalPages As Integer = Math.Max(1, CInt(Math.Ceiling(totalRows / CDbl(_pageSize))))

        If _currentPage < 1 Then _currentPage = 1
        If _currentPage > totalPages Then _currentPage = totalPages

        Dim pageTable As DataTable = source.Clone()
        If totalRows > 0 Then
            Dim startIndex As Integer = (_currentPage - 1) * _pageSize
            Dim endIndex As Integer = Math.Min(startIndex + _pageSize, totalRows)

            For i As Integer = startIndex To endIndex - 1
                pageTable.ImportRow(source.Rows(i))
            Next
        End If

        DGVtable.DataSource = pageTable
        lblPage.Text = $"Page {_currentPage} of {totalPages}"
        btnPreviousPage.Enabled = (_currentPage > 1)
        btnNextPage.Enabled = (_currentPage < totalPages)
    End Sub

    Private Function BuildFilterCriteria(searchColumns As IEnumerable(Of String),
                                         Optional includeCashierFilter As Boolean = False,
                                         Optional dateColumn As String = "SaleDate",
                                         Optional cashierIdColumn As String = "",
                                         Optional cashierNameColumn As String = "Name") As FilterCriteria
        Dim parts As New List(Of String)()
        Dim parameters As New List(Of SqlParameter)()

        Dim dateFrom As DateTime
        Dim dateTo As DateTime
        GetSelectedDateRange(dateFrom, dateTo)

        Dim safeDateColumn As String = If(String.IsNullOrWhiteSpace(dateColumn), "SaleDate", dateColumn)
        parts.Add(safeDateColumn & " >= @DateFrom")
        parts.Add(safeDateColumn & " < DATEADD(DAY, 1, @DateTo)")

        parameters.Add(New SqlParameter("@DateFrom", SqlDbType.DateTime2) With {.Value = dateFrom.Date})
        parameters.Add(New SqlParameter("@DateTo", SqlDbType.DateTime2) With {.Value = dateTo.Date})

        Dim search As String = txtSearch.Text.Trim()
        If search.Length > 0 Then
            Dim predicates As New List(Of String)()
            For Each col As String In searchColumns
                predicates.Add(col & " LIKE @Search")
            Next
            If predicates.Count > 0 Then
                parts.Add("(" & String.Join(" OR ", predicates) & ")")
                parameters.Add(New SqlParameter("@Search", SqlDbType.NVarChar, 255) With {.Value = "%" & search & "%"})
            End If
        End If

        Dim cashierFilter As String = String.Empty
        Dim loggedCashierName As String = ResolveLoggedCashierName()
        Dim loggedCashierId As Integer = FrmLogin.CurrentUser.UserID
        If loggedCashierId <= 0 Then
            loggedCashierId = SessionContext.PrincipalID
        End If

        If IsCashierUser() Then
            If loggedCashierId > 0 AndAlso Not String.IsNullOrWhiteSpace(cashierIdColumn) Then
                parts.Add($"{cashierIdColumn} = @CashierUserID")
                parameters.Add(New SqlParameter("@CashierUserID", SqlDbType.Int) With {.Value = loggedCashierId})
                cashierFilter = loggedCashierName
            ElseIf loggedCashierName.Length > 0 AndAlso Not String.IsNullOrWhiteSpace(cashierNameColumn) Then
                parts.Add($"{cashierNameColumn} = @LoggedCashierName")
                parameters.Add(New SqlParameter("@LoggedCashierName", SqlDbType.VarChar, 100) With {.Value = loggedCashierName})
                cashierFilter = loggedCashierName
            End If
        ElseIf includeCashierFilter Then
            Dim selectedCashierId As Integer = GetSelectedCashierId()
            If selectedCashierId > 0 AndAlso Not String.IsNullOrWhiteSpace(cashierIdColumn) Then
                parts.Add($"{cashierIdColumn} = @CashierUserID")
                parameters.Add(New SqlParameter("@CashierUserID", SqlDbType.Int) With {.Value = selectedCashierId})
                cashierFilter = GetSelectedCashierName()
            Else
                cashierFilter = GetSelectedCashierName()
                If cashierFilter.Length > 0 AndAlso Not String.IsNullOrWhiteSpace(cashierNameColumn) Then
                    parts.Add($"{cashierNameColumn} = @CashierName")
                    parameters.Add(New SqlParameter("@CashierName", SqlDbType.VarChar, 100) With {.Value = cashierFilter})
                End If
            End If
        End If

        Return New FilterCriteria With {
            .WhereClause = String.Join(" AND ", parts),
            .Parameters = parameters,
            .CashierFilter = cashierFilter
        }
    End Function

    Private Function BuildFinancialCriteria(ByRef useMonthly As Boolean, ByRef searchColumns As String()) As FilterCriteria
        Dim parts As New List(Of String)()
        Dim parameters As New List(Of SqlParameter)()

        Dim dateFrom As DateTime
        Dim dateTo As DateTime
        GetSelectedDateRange(dateFrom, dateTo)

        useMonthly = ShouldUseMonthlyFinancialView(dateFrom, dateTo)

        Dim dateExpr As String
        If useMonthly Then
            dateExpr = "DATEFROMPARTS(SalesYear, SalesMonth, 1)"
            searchColumns = {
                $"CONVERT(NVARCHAR(7), {dateExpr}, 120)",
                $"DATENAME(MONTH, {dateExpr})",
                "CAST(SalesYear AS NVARCHAR(4))"
            }
        Else
            dateExpr = "ReportDate"
            searchColumns = {
                $"CONVERT(NVARCHAR(10), {dateExpr}, 120)"
            }
        End If

        If useMonthly Then
            Dim monthFrom As New DateTime(dateFrom.Year, dateFrom.Month, 1)
            Dim monthTo As New DateTime(dateTo.Year, dateTo.Month, 1)
            parts.Add(dateExpr & " >= @DateFrom")
            parts.Add(dateExpr & " < DATEADD(MONTH, 1, @DateTo)")

            parameters.Add(New SqlParameter("@DateFrom", SqlDbType.DateTime2) With {.Value = monthFrom})
            parameters.Add(New SqlParameter("@DateTo", SqlDbType.DateTime2) With {.Value = monthTo})
        Else
            parts.Add(dateExpr & " >= @DateFrom")
            parts.Add(dateExpr & " < DATEADD(DAY, 1, @DateTo)")

            parameters.Add(New SqlParameter("@DateFrom", SqlDbType.DateTime2) With {.Value = dateFrom.Date})
            parameters.Add(New SqlParameter("@DateTo", SqlDbType.DateTime2) With {.Value = dateTo.Date})
        End If

        Dim search As String = txtSearch.Text.Trim()
        If search.Length > 0 AndAlso searchColumns IsNot Nothing AndAlso searchColumns.Length > 0 Then
            Dim predicates As New List(Of String)()
            For Each col As String In searchColumns
                predicates.Add(col & " LIKE @Search")
            Next
            parts.Add("(" & String.Join(" OR ", predicates) & ")")
            parameters.Add(New SqlParameter("@Search", SqlDbType.NVarChar, 255) With {.Value = "%" & search & "%"})
        End If

        Return New FilterCriteria With {
            .WhereClause = String.Join(" AND ", parts),
            .Parameters = parameters
        }
    End Function

    Private Function ShouldUseMonthlyFinancialView(dateFrom As DateTime, dateTo As DateTime) As Boolean
        If dateFrom.Year <> dateTo.Year OrElse dateFrom.Month <> dateTo.Month Then
            Return True
        End If

        Dim spanDays As Double = (dateTo.Date - dateFrom.Date).TotalDays
        Return spanDays > 31
    End Function

    Private Function GetReportTotals(searchColumns As IEnumerable(Of String),
                                     Optional includeCashierFilter As Boolean = False) As ReportTotals
        Select Case _activeReport
            Case ReportType.Delivery
                Return GetDeliveryReportTotalsFromRows()
            Case ReportType.Returns
                Return GetReturnsReportTotalsFromRows()
            Case ReportType.Financial
                Return GetFinancialReportTotals()
        End Select

        Dim cashierExpr As String = GetUserDisplayExpression("u")
        Dim criteria As FilterCriteria = BuildFilterCriteria(searchColumns,
                                                             includeCashierFilter:=includeCashierFilter,
                                                             dateColumn:="st.SaleDate",
                                                             cashierIdColumn:="st.UserID",
                                                             cashierNameColumn:=cashierExpr)
        Dim totals As New ReportTotals()

        Dim sql As String =
            "SELECT " &
            "    ISNULL(SUM(ISNULL(st.TotalItems, 0)), 0) AS TotalQtySold, " &
            "    ISNULL(SUM(ISNULL(st.TotalAmount, 0) + ISNULL(st.TotalDiscount, 0)), 0) AS GrossSales, " &
            "    ISNULL(SUM(ISNULL(st.TotalDiscount, 0)), 0) AS TotalDiscount, " &
            "    ISNULL(SUM(ISNULL(st.TotalVAT, 0)), 0) AS TotalVat, " &
            "    ISNULL(SUM(ISNULL(st.TotalAmount, 0)), 0) AS NetSales " &
            "FROM dbo.tbl_SalesTransaction st " &
            "LEFT JOIN dbo.tbl_User u ON st.UserID = u.UserID " &
            "WHERE " & criteria.WhereClause & ";"

        Dim dt As DataTable = Db.QueryDataTable(sql, criteria.Parameters.ToArray())
        If dt.Rows.Count = 0 Then Return totals

        Dim row As DataRow = dt.Rows(0)
        totals.TotalQtySold = ToDecimal(row("TotalQtySold"))
        totals.GrossSales = ToDecimal(row("GrossSales"))
        totals.TotalDiscount = ToDecimal(row("TotalDiscount"))
        totals.TotalVat = ToDecimal(row("TotalVat"))
        totals.NetSales = ToDecimal(row("NetSales"))
        Return totals
    End Function

    Private Function GetFinancialReportTotals() As ReportTotals
        Dim totals As New ReportTotals()

        Dim useMonthly As Boolean
        Dim searchCols As String() = Nothing
        Dim criteria As FilterCriteria = BuildFinancialCriteria(useMonthly, searchCols)

        Dim totalsSql As String
        If useMonthly Then
            totalsSql =
                "SELECT " &
                "    ISNULL(SUM(GrossSales), 0) AS GrossSales, " &
                "    ISNULL(SUM(TotalCost), 0) AS TotalCost, " &
                "    ISNULL(SUM(TotalDiscount), 0) AS TotalDiscount, " &
                "    ISNULL(SUM(TotalVAT), 0) AS TotalVat, " &
                "    ISNULL(SUM(NetSales), 0) AS NetSales, " &
                "    ISNULL(SUM(Profit), 0) AS EstimatedProfit " &
                "FROM dbo.vw_FinancialReport_Monthly " &
                "WHERE " & criteria.WhereClause & ";"
        Else
            totalsSql =
                "SELECT " &
                "    ISNULL(SUM(GrossSales), 0) AS GrossSales, " &
                "    ISNULL(SUM(TotalCost), 0) AS TotalCost, " &
                "    ISNULL(SUM(TotalDiscount), 0) AS TotalDiscount, " &
                "    ISNULL(SUM(TotalVAT), 0) AS TotalVat, " &
                "    ISNULL(SUM(NetSales), 0) AS NetSales, " &
                "    ISNULL(SUM(Profit), 0) AS EstimatedProfit " &
                "FROM dbo.vw_FinancialReport_Daily " &
                "WHERE " & criteria.WhereClause & ";"
        End If

        Dim dt As DataTable = Db.QueryDataTable(totalsSql, criteria.Parameters.ToArray())
        If dt.Rows.Count > 0 Then
            Dim row As DataRow = dt.Rows(0)
            totals.GrossSales = ToDecimal(row("GrossSales"))
            totals.TotalCost = ToDecimal(row("TotalCost"))
            totals.TotalDiscount = ToDecimal(row("TotalDiscount"))
            totals.TotalVat = ToDecimal(row("TotalVat"))
            totals.NetSales = ToDecimal(row("NetSales"))
            totals.EstimatedProfit = ToDecimal(row("EstimatedProfit"))
        End If

        Dim dateFrom As DateTime
        Dim dateTo As DateTime
        GetSelectedDateRange(dateFrom, dateTo)

        Dim qtySql As String =
            "SELECT ISNULL(SUM(ISNULL(TotalItems, 0)), 0) " &
            "FROM dbo.tbl_SalesTransaction " &
            "WHERE SaleDate >= @DateFrom AND SaleDate < DATEADD(DAY, 1, @DateTo);"

        Dim qtyValue As Decimal = Db.ExecuteScalar(Of Decimal)(
            qtySql,
            New SqlParameter("@DateFrom", SqlDbType.DateTime2) With {.Value = dateFrom.Date},
            New SqlParameter("@DateTo", SqlDbType.DateTime2) With {.Value = dateTo.Date}
        )
        totals.TotalQtySold = qtyValue

        Return totals
    End Function

    Private Function GetDeliveryReportTotalsFromRows() As ReportTotals
        Dim totals As New ReportTotals()
        Dim rows As DataTable = If(_allRows, New DataTable())
        totals.RecordCount = rows.Rows.Count

        If rows.Rows.Count = 0 Then Return totals

        For Each row As DataRow In rows.Rows
            totals.TotalItemCount += ToDecimal(row("ItemCount"))
            totals.TotalQuantity += ToDecimal(row("TotalQty"))
            totals.AggregateCost += ToDecimal(row("TotalCost"))
        Next

        Return totals
    End Function

    Private Function GetReturnsReportTotalsFromRows() As ReportTotals
        Dim totals As New ReportTotals()
        Dim rows As DataTable = If(_allRows, New DataTable())
        totals.RecordCount = rows.Rows.Count

        If rows.Rows.Count = 0 Then Return totals

        For Each row As DataRow In rows.Rows
            totals.TotalItemCount += ToDecimal(row("ItemCount"))
            totals.TotalReturnedQuantity += ToDecimal(row("TotalReturnedQty"))
        Next

        Return totals
    End Function

    Private Sub UpdateTotalsDisplay()
        If _totalsLabel Is Nothing Then Return

        _totalsLabel.Text = BuildTotalsText()
    End Sub

    Private Function BuildTotalsText() As String
        Dim text As String
        Select Case _activeReport
            Case ReportType.Delivery
                text = "Total Deliveries: " & _currentTotals.RecordCount.ToString("N0") &
                       " | Total Items: " & _currentTotals.TotalItemCount.ToString("N0") &
                       " | Total Qty: " & _currentTotals.TotalQuantity.ToString("N0")
                If _deliveryCostColumnAvailable Then
                    text &= " | Total Cost: " & _currentTotals.AggregateCost.ToString("N2")
                End If

            Case ReportType.Returns
                text = "Total Returns: " & _currentTotals.RecordCount.ToString("N0") &
                       " | Total Items: " & _currentTotals.TotalItemCount.ToString("N0") &
                       " | Total Returned Qty: " & _currentTotals.TotalReturnedQuantity.ToString("N0")

            Case Else
                text = "Total Qty Sold: " & _currentTotals.TotalQtySold.ToString("N0") &
                       " | Gross Sales: " & _currentTotals.GrossSales.ToString("N2") &
                       " | Total Discount: " & _currentTotals.TotalDiscount.ToString("N2") &
                       " | Total VAT: " & _currentTotals.TotalVat.ToString("N2") &
                       " | Net Sales: " & _currentTotals.NetSales.ToString("N2")

                If _activeReport = ReportType.Financial Then
                    text &= " | Total Cost: " & _currentTotals.TotalCost.ToString("N2") &
                            " | Estimated Profit: " & _currentTotals.EstimatedProfit.ToString("N2")
                End If
        End Select

        Return text
    End Function

    Private Function ToDecimal(value As Object) As Decimal
        If value Is Nothing OrElse value Is DBNull.Value Then Return 0D
        Dim parsed As Decimal
        If Decimal.TryParse(Convert.ToString(value), parsed) Then
            Return parsed
        End If
        Return 0D
    End Function

    Private Function HasTableColumn(tableName As String, columnName As String) As Boolean
        If String.IsNullOrWhiteSpace(tableName) OrElse String.IsNullOrWhiteSpace(columnName) Then
            Return False
        End If

        Try
            Dim sql As String =
                "SELECT COUNT(1) " &
                "FROM sys.columns " &
                "WHERE object_id = OBJECT_ID(@TableName) " &
                "  AND name = @ColumnName;"

            Dim count As Integer = Db.ExecuteScalar(Of Integer)(
                sql,
                New SqlParameter("@TableName", SqlDbType.NVarChar, 256) With {.Value = tableName.Trim()},
                New SqlParameter("@ColumnName", SqlDbType.NVarChar, 128) With {.Value = columnName.Trim()})

            Return count > 0
        Catch
            Return False
        End Try
    End Function

    Private Sub ApplySalesGridFormatting()
        If Not GridHelpers.IsGridReady(DGVtable) Then Return
        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        ApplyStandardGridLayout(DGVtable)

        GridHelpers.ApplyColumnSetup(DGVtable, "SaleDate", Sub(col)
                                                               col.HeaderText = "Sale Date"
                                                               col.DefaultCellStyle.Format = "MM/dd/yyyy hh:mm tt"
                                                           End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "TransactionNo", Sub(col) col.HeaderText = "Transaction No")
        GridHelpers.ApplyColumnSetup(DGVtable, "TotalItems", Sub(col)
                                                                col.HeaderText = "Total Items"
                                                                col.DefaultCellStyle.Format = "N0"
                                                                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                            End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "TotalDiscount", Sub(col)
                                                                   col.HeaderText = "Total Discount"
                                                                   col.DefaultCellStyle.Format = "N2"
                                                                   col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                               End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "TotalVAT", Sub(col)
                                                              col.HeaderText = "Total VAT"
                                                              col.DefaultCellStyle.Format = "N2"
                                                              col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                          End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "TotalAmount", Sub(col)
                                                                 col.HeaderText = "Total Amount"
                                                                 col.DefaultCellStyle.Format = "N2"
                                                                 col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                             End Sub)
    End Sub

    Private Sub ApplyFinancialGridFormatting()
        If Not GridHelpers.IsGridReady(DGVtable) Then Return
        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        ApplyStandardGridLayout(DGVtable)

        GridHelpers.ApplyColumnSetup(DGVtable, "ReportDate", Sub(col)
                                                                 col.HeaderText = "Report Date"
                                                                 col.DefaultCellStyle.Format = "MM/dd/yyyy"
                                                             End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "GrossSales", Sub(col)
                                                                col.HeaderText = "Gross Sales"
                                                                col.DefaultCellStyle.Format = "N2"
                                                                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                            End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "TotalCost", Sub(col)
                                                               col.HeaderText = "Total Cost"
                                                               col.DefaultCellStyle.Format = "N2"
                                                               col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                           End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "TotalDiscount", Sub(col)
                                                                   col.HeaderText = "Total Discount"
                                                                   col.DefaultCellStyle.Format = "N2"
                                                                   col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                               End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "TotalVAT", Sub(col)
                                                              col.HeaderText = "Total VAT"
                                                              col.DefaultCellStyle.Format = "N2"
                                                              col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                          End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "NetSales", Sub(col)
                                                              col.HeaderText = "Net Sales"
                                                              col.DefaultCellStyle.Format = "N2"
                                                              col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                          End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "Profit", Sub(col)
                                                            col.HeaderText = "Profit"
                                                            col.DefaultCellStyle.Format = "N2"
                                                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                        End Sub)
    End Sub

    Private Sub ApplyCashierGridFormatting()
        If Not GridHelpers.IsGridReady(DGVtable) Then Return
        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        ApplyStandardGridLayout(DGVtable)

        GridHelpers.ApplyColumnSetup(DGVtable, "Cashier", Sub(col) col.HeaderText = "Cashier")
        GridHelpers.ApplyColumnSetup(DGVtable, "Transaction No", Sub(col) col.HeaderText = "Transaction No")
        GridHelpers.ApplyColumnSetup(DGVtable, "Total Amount", Sub(col)
                                                                   col.HeaderText = "Total Amount"
                                                                   col.DefaultCellStyle.Format = "N2"
                                                                   col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                               End Sub)
    End Sub

    Private Sub ApplyDeliveryGridFormatting()
        If Not GridHelpers.IsGridReady(DGVtable) Then Return
        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        ApplyStandardGridLayout(DGVtable)

        Dim columnActions As New Dictionary(Of String, Action(Of DataGridViewColumn)) From {
            {"DeliveryDate", Sub(col)
                                 col.HeaderText = "Delivery Date"
                                 col.DefaultCellStyle.Format = "MM/dd/yyyy"
                             End Sub},
            {"DeliveryNo", Sub(col) col.HeaderText = "Delivery No."},
            {"OrderNo", Sub(col) col.HeaderText = "Order No."},
            {"Supplier", Sub(col) col.HeaderText = "Supplier"},
            {"ItemCount", Sub(col)
                              col.HeaderText = "Item Count"
                              col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                          End Sub},
            {"TotalQty", Sub(col)
                             col.HeaderText = "Total Qty"
                             col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                         End Sub},
            {"Status", Sub(col) col.HeaderText = "Status"},
            {"TotalCost", Sub(col)
                              col.HeaderText = "Total Cost"
                              col.DefaultCellStyle.Format = "N2"
                              col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                              col.Visible = _deliveryCostColumnAvailable
                          End Sub}
        }
        GridHelpers.ApplyColumnSetup(DGVtable, columnActions)
    End Sub

    Private Sub ApplyReturnsGridFormatting()
        If Not GridHelpers.IsGridReady(DGVtable) Then Return
        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        ApplyStandardGridLayout(DGVtable)

        Dim columnActions As New Dictionary(Of String, Action(Of DataGridViewColumn)) From {
            {"ReturnNo", Sub(col) col.HeaderText = "Return No."},
            {"Supplier", Sub(col) col.HeaderText = "Supplier"},
            {"DeliveryNo", Sub(col) col.HeaderText = "Delivery No."},
            {"ReturnDate", Sub(col)
                               col.HeaderText = "Return Date"
                               col.DefaultCellStyle.Format = "MM/dd/yyyy"
                           End Sub},
            {"ReturnType", Sub(col) col.HeaderText = "Return Type"},
            {"Resolution", Sub(col) col.HeaderText = "Resolution"},
            {"ItemCount", Sub(col)
                              col.HeaderText = "Item Count"
                              col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                          End Sub},
            {"TotalReturnedQty", Sub(col)
                                     col.HeaderText = "Total Returned Qty"
                                     col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                 End Sub},
            {"Status", Sub(col) col.HeaderText = "Status"},
            {"Notes", Sub(col) col.HeaderText = "Notes"}
        }
        GridHelpers.ApplyColumnSetup(DGVtable, columnActions)
    End Sub

    Private Sub ConfigureSidebarPermissions()
        btnFileMaintenance.Text = "File Maintenance"

        If IsCashierUser() Then
            btnFileMaintenance.Visible = True
            btnFileMaintenance.Text = "Discount"
            btnDelivery.Visible = False
            btnReturns.Visible = False
            btnAuditTrail.Visible = False

            ApplyCashierSidebarNavigationOrder(Guna2Panel2, btnHome, btnPos, btnInventory, btnFileMaintenance, btnTransaction, btnReports, btnLogout)
            Return
        End If

        If IsStaffUser() Then
            btnPos.Visible = False
            btnAuditTrail.Visible = False

            ApplyStaffSidebarNavigationOrder(Guna2Panel2, btnHome, btnFileMaintenance, btnDelivery, btnReturns, btnInventory, btnTransaction, btnReports, btnLogout)
        End If
    End Sub

    Private Sub ApplySidebarUserPanel()
        lblFirstname.Text = ResolveSidebarFirstName()
        lblUserLevel.Text = ResolveSidebarRoleDisplay()
    End Sub

    Private Sub ConfigureReportTabPermissions()
        If IsCashierUser() Then
            btnSalesReport.Visible = False
            pnlUser.Visible = False
            btnFinancialReports.Visible = False
            pnlCategory.Visible = False
            btnCashierReport.Visible = True
            pnlSize.Visible = True
            btnDeliveryReportTab.Visible = False
            pnlDeliveryReportTab.Visible = False
            btnReturnsReportTab.Visible = False
            pnlReturnsReportTab.Visible = False
            _activeReport = ReportType.Cashier
            Return
        End If

        If IsStaffUser() Then
            btnSalesReport.Visible = False
            pnlUser.Visible = False
            btnFinancialReports.Visible = False
            pnlCategory.Visible = False
            btnCashierReport.Visible = False
            pnlSize.Visible = False
            btnDeliveryReportTab.Visible = True
            pnlDeliveryReportTab.Visible = True
            btnReturnsReportTab.Visible = True
            pnlReturnsReportTab.Visible = True

            If _activeReport <> ReportType.Delivery AndAlso _activeReport <> ReportType.Returns Then
                _activeReport = ReportType.Delivery
            End If

            Return
        End If

        btnSalesReport.Visible = True
        pnlUser.Visible = True
        btnFinancialReports.Visible = True
        pnlCategory.Visible = True
        btnCashierReport.Visible = True
        pnlSize.Visible = True

        Dim canUseExtendedReportTabs As Boolean = CanAccessExtendedReports()

        btnDeliveryReportTab.Visible = canUseExtendedReportTabs
        pnlDeliveryReportTab.Visible = canUseExtendedReportTabs
        btnReturnsReportTab.Visible = canUseExtendedReportTabs
        pnlReturnsReportTab.Visible = canUseExtendedReportTabs

        If Not canUseExtendedReportTabs AndAlso
           (_activeReport = ReportType.Delivery OrElse _activeReport = ReportType.Returns) Then
            _activeReport = ReportType.Sales
        End If
    End Sub

    Private Function IsAdminUser() As Boolean
        Return String.Equals(FrmLogin.CurrentUser.Role, "admin", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Function IsStaffUser() As Boolean
        Return String.Equals(FrmLogin.CurrentUser.Role, "staff", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Function IsCashierUser() As Boolean
        Return String.Equals(FrmLogin.CurrentUser.Role, "cashier", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Function CanAccessExtendedReports() As Boolean
        Return IsAdminUser() OrElse IsStaffUser()
    End Function

    Private Sub SwitchSidebarNavigation(activeButton As Guna2Button, Optional nextForm As Form = Nothing)
        SetActiveSidebarButton(activeButton)
        If nextForm Is Nothing Then Return
        OpenForm(nextForm)
    End Sub

    Private Sub SetActiveSidebarButton(activeButton As Guna2Button)
        For Each btn As Guna2Button In GetSidebarNavigationButtons()
            If btn Is Nothing Then Continue For

            Dim isActive As Boolean = (btn Is activeButton)
            btn.FillColor = If(isActive, _activeSidebarFillColor, _inactiveSidebarFillColor)
            btn.HoverState.FillColor = _activeSidebarFillColor
        Next
    End Sub

    Private Iterator Function GetSidebarNavigationButtons() As IEnumerable(Of Guna2Button)
        Yield btnHome
        Yield btnFileMaintenance
        Yield btnDelivery
        Yield btnReturns
        Yield btnInventory
        Yield btnPos
        Yield btnTransaction
        Yield btnAuditTrail
        Yield btnReports
    End Function

    Private Sub OpenForm(nextForm As Form)
        If nextForm Is Nothing Then Return
        nextForm.Show()
        Me.Hide()
    End Sub

    Private Sub SetActiveReport(report As ReportType, Optional loadData As Boolean = True)
        If IsCashierUser() Then
            report = ReportType.Cashier
        ElseIf IsStaffUser() AndAlso report <> ReportType.Delivery AndAlso report <> ReportType.Returns Then
            report = ReportType.Delivery
        End If

        If (report = ReportType.Delivery OrElse report = ReportType.Returns) AndAlso Not CanAccessExtendedReports() Then
            report = ReportType.Sales
        End If

        Dim previousReport As ReportType = _activeReport
        _activeReport = report

        If previousReport = report Then
            ApplyTabButtonColors(report)
        Else
            StartTabButtonAnimation(report)
        End If

        ApplyTabPanelColors()

        Select Case report
            Case ReportType.Sales
                txtSearch.PlaceholderText = "Transaction / Cashier"
            Case ReportType.Financial
                txtSearch.PlaceholderText = "Date / Period"
            Case ReportType.Cashier
                txtSearch.PlaceholderText = "Cashier / Transaction No"
            Case ReportType.Delivery
                txtSearch.PlaceholderText = "Delivery No / Order No / Supplier / Status"
            Case ReportType.Returns
                txtSearch.PlaceholderText = "Return No / Delivery No / Supplier / Type / Resolution / Status / Notes"
        End Select

        Dim showCashierFilter As Boolean = (report = ReportType.Cashier) AndAlso Not IsCashierUser()
        cbCashierName.Visible = showCashierFilter
        cbCashierName.Location = New Point(499, 52)

        If loadData Then
            LoadCurrentReport()
        End If
    End Sub

    Private Sub ApplyTabPanelColors()
        pnlUser.FillColor = _tabPanelColor
        pnlCategory.FillColor = _tabPanelColor
        pnlSize.FillColor = _tabPanelColor
        pnlDeliveryReportTab.FillColor = _tabPanelColor
        pnlReturnsReportTab.FillColor = _tabPanelColor
    End Sub

    Private Sub ApplyTabButtonColors(activeReport As ReportType)
        btnSalesReport.FillColor = GetTabTargetColor(ReportType.Sales, activeReport)
        btnFinancialReports.FillColor = GetTabTargetColor(ReportType.Financial, activeReport)
        btnCashierReport.FillColor = GetTabTargetColor(ReportType.Cashier, activeReport)
        btnDeliveryReportTab.FillColor = GetTabTargetColor(ReportType.Delivery, activeReport)
        btnReturnsReportTab.FillColor = GetTabTargetColor(ReportType.Returns, activeReport)
    End Sub

    Private Sub StartTabButtonAnimation(activeReport As ReportType)
        _tabFromSalesColor = btnSalesReport.FillColor
        _tabFromFinancialColor = btnFinancialReports.FillColor
        _tabFromCashierColor = btnCashierReport.FillColor
        _tabFromDeliveryColor = btnDeliveryReportTab.FillColor
        _tabFromReturnsColor = btnReturnsReportTab.FillColor

        _tabToSalesColor = GetTabTargetColor(ReportType.Sales, activeReport)
        _tabToFinancialColor = GetTabTargetColor(ReportType.Financial, activeReport)
        _tabToCashierColor = GetTabTargetColor(ReportType.Cashier, activeReport)
        _tabToDeliveryColor = GetTabTargetColor(ReportType.Delivery, activeReport)
        _tabToReturnsColor = GetTabTargetColor(ReportType.Returns, activeReport)

        If _tabAnimationTimer.Enabled Then
            _tabAnimationTimer.Stop()
        End If

        _tabAnimationStartUtc = DateTime.UtcNow
        _tabAnimationTimer.Start()
    End Sub

    Private Function GetTabTargetColor(buttonReport As ReportType, activeReport As ReportType) As Color
        Return If(buttonReport = activeReport, _activeFillColor, _inactiveFillColor)
    End Function

    Private Sub TabAnimationTimer_Tick(sender As Object, e As EventArgs)
        Dim elapsedMs As Double = (DateTime.UtcNow - _tabAnimationStartUtc).TotalMilliseconds
        Dim progress As Double = elapsedMs / _tabAnimationDurationMs

        If progress >= 1.0 Then
            _tabAnimationTimer.Stop()
            btnSalesReport.FillColor = _tabToSalesColor
            btnFinancialReports.FillColor = _tabToFinancialColor
            btnCashierReport.FillColor = _tabToCashierColor
            btnDeliveryReportTab.FillColor = _tabToDeliveryColor
            btnReturnsReportTab.FillColor = _tabToReturnsColor
            Return
        End If

        btnSalesReport.FillColor = InterpolateColor(_tabFromSalesColor, _tabToSalesColor, progress)
        btnFinancialReports.FillColor = InterpolateColor(_tabFromFinancialColor, _tabToFinancialColor, progress)
        btnCashierReport.FillColor = InterpolateColor(_tabFromCashierColor, _tabToCashierColor, progress)
        btnDeliveryReportTab.FillColor = InterpolateColor(_tabFromDeliveryColor, _tabToDeliveryColor, progress)
        btnReturnsReportTab.FillColor = InterpolateColor(_tabFromReturnsColor, _tabToReturnsColor, progress)
    End Sub

    Private Function InterpolateColor(fromColor As Color, toColor As Color, progress As Double) As Color
        Dim p As Double = progress
        If Double.IsNaN(p) OrElse Double.IsInfinity(p) Then
            p = 1.0
        End If
        p = Math.Max(0.0, Math.Min(1.0, p))

        Dim a As Integer = InterpolateChannel(CInt(fromColor.A), CInt(toColor.A), p)
        Dim r As Integer = InterpolateChannel(CInt(fromColor.R), CInt(toColor.R), p)
        Dim g As Integer = InterpolateChannel(CInt(fromColor.G), CInt(toColor.G), p)
        Dim b As Integer = InterpolateChannel(CInt(fromColor.B), CInt(toColor.B), p)

        Return Color.FromArgb(a, r, g, b)
    End Function

    Private Function InterpolateChannel(fromChannel As Integer, toChannel As Integer, progress As Double) As Integer
        Dim value As Double = CDbl(fromChannel) + (CDbl(toChannel) - CDbl(fromChannel)) * progress
        If value < 0.0 Then Return 0
        If value > 255.0 Then Return 255
        Return CInt(Math.Round(value, MidpointRounding.AwayFromZero))
    End Function

    Private Sub ApplyActiveReportGridFormatting()
        Select Case _activeReport
            Case ReportType.Sales
                ApplySalesGridFormatting()
            Case ReportType.Financial
                ApplyFinancialGridFormatting()
            Case ReportType.Cashier
                ApplyCashierGridFormatting()
            Case ReportType.Delivery
                ApplyDeliveryGridFormatting()
            Case ReportType.Returns
                ApplyReturnsGridFormatting()
        End Select
    End Sub

    Private Sub GetSelectedDateRange(ByRef dateFrom As DateTime, ByRef dateTo As DateTime)
        dateFrom = ReadDateFilter(dtpFromDate, Date.Today)
        dateTo = ReadDateFilter(dtpTodate, Date.Today)
        If dateFrom > dateTo Then
            Dim temp As DateTime = dateFrom
            dateFrom = dateTo
            dateTo = temp
        End If
    End Sub

    Private Function ReadDateFilter(picker As Guna2DateTimePicker, fallbackDate As DateTime) As DateTime
        If picker Is Nothing Then Return fallbackDate.Date
        Return picker.Value.Date
    End Function

    Private Function GetSelectedCashierId() As Integer
        If cbCashierName Is Nothing Then Return 0

        Dim selectedValue As String = Convert.ToString(cbCashierName.SelectedValue).Trim()
        If selectedValue.Equals("All Cashiers", StringComparison.OrdinalIgnoreCase) Then
            Return 0
        End If

        Dim parsedId As Integer
        If Integer.TryParse(selectedValue, parsedId) Then
            Return parsedId
        End If

        Return 0
    End Function

    Private Function GetSelectedCashierName() As String
        If cbCashierName Is Nothing Then Return String.Empty

        Dim selectedText As String = Convert.ToString(cbCashierName.Text).Trim()

        If selectedText.Equals("All Cashiers", StringComparison.OrdinalIgnoreCase) Then
            Return String.Empty
        End If

        If selectedText.Length > 0 Then Return selectedText
        Return String.Empty
    End Function

    Private Function GetPrintColumnsForCurrentReport() As List(Of PrintColumn)
        Select Case _activeReport
            Case ReportType.Sales
                Return New List(Of PrintColumn) From {
                    New PrintColumn With {.FieldName = "SaleDate", .Header = "SaleDate", .FormatKind = "date"},
                    New PrintColumn With {.FieldName = "TransactionNo", .Header = "TransactionNo"},
                    New PrintColumn With {.FieldName = "Cashier", .Header = "Cashier"},
                    New PrintColumn With {.FieldName = "TotalItems", .Header = "Items", .AlignRight = True, .FormatKind = "int"},
                    New PrintColumn With {.FieldName = "TotalDiscount", .Header = "Discount", .AlignRight = True, .FormatKind = "money"},
                    New PrintColumn With {.FieldName = "TotalVAT", .Header = "VAT", .AlignRight = True, .FormatKind = "money"},
                    New PrintColumn With {.FieldName = "TotalAmount", .Header = "TotalAmount", .AlignRight = True, .FormatKind = "money"}
                }

            Case ReportType.Financial
                Return New List(Of PrintColumn) From {
                    New PrintColumn With {.FieldName = "ReportDate", .Header = "ReportDate", .FormatKind = "date"},
                    New PrintColumn With {.FieldName = "GrossSales", .Header = "GrossSales", .AlignRight = True, .FormatKind = "money"},
                    New PrintColumn With {.FieldName = "TotalCost", .Header = "TotalCost", .AlignRight = True, .FormatKind = "money"},
                    New PrintColumn With {.FieldName = "TotalDiscount", .Header = "Discount", .AlignRight = True, .FormatKind = "money"},
                    New PrintColumn With {.FieldName = "TotalVAT", .Header = "VAT", .AlignRight = True, .FormatKind = "money"},
                    New PrintColumn With {.FieldName = "NetSales", .Header = "NetSales", .AlignRight = True, .FormatKind = "money"},
                    New PrintColumn With {.FieldName = "Profit", .Header = "Profit", .AlignRight = True, .FormatKind = "money"}
                }

            Case ReportType.Cashier
                Return New List(Of PrintColumn) From {
                    New PrintColumn With {.FieldName = "Cashier", .Header = "Cashier"},
                    New PrintColumn With {.FieldName = "Transaction No", .Header = "Transaction No"},
                    New PrintColumn With {.FieldName = "Total Amount", .Header = "Total Amount", .AlignRight = True, .FormatKind = "money"}
                }

            Case ReportType.Delivery
                Dim cols As New List(Of PrintColumn) From {
                    New PrintColumn With {.FieldName = "DeliveryDate", .Header = "DeliveryDate", .FormatKind = "date"},
                    New PrintColumn With {.FieldName = "DeliveryNo", .Header = "DeliveryNo"},
                    New PrintColumn With {.FieldName = "OrderNo", .Header = "OrderNo"},
                    New PrintColumn With {.FieldName = "Supplier", .Header = "Supplier"},
                    New PrintColumn With {.FieldName = "ItemCount", .Header = "Items", .AlignRight = True, .FormatKind = "int"},
                    New PrintColumn With {.FieldName = "TotalQty", .Header = "TotalQty", .AlignRight = True, .FormatKind = "int"},
                    New PrintColumn With {.FieldName = "Status", .Header = "Status"}
                }

                If _deliveryCostColumnAvailable Then
                    cols.Add(New PrintColumn With {.FieldName = "TotalCost", .Header = "TotalCost", .AlignRight = True, .FormatKind = "money"})
                End If
                Return cols

            Case Else
                Return New List(Of PrintColumn) From {
                    New PrintColumn With {.FieldName = "ReturnNo", .Header = "ReturnNo"},
                    New PrintColumn With {.FieldName = "Supplier", .Header = "Supplier"},
                    New PrintColumn With {.FieldName = "DeliveryNo", .Header = "DeliveryNo"},
                    New PrintColumn With {.FieldName = "ReturnDate", .Header = "ReturnDate", .FormatKind = "date"},
                    New PrintColumn With {.FieldName = "ReturnType", .Header = "ReturnType"},
                    New PrintColumn With {.FieldName = "Resolution", .Header = "Resolution"},
                    New PrintColumn With {.FieldName = "ItemCount", .Header = "Items", .AlignRight = True, .FormatKind = "int"},
                    New PrintColumn With {.FieldName = "TotalReturnedQty", .Header = "ReturnedQty", .AlignRight = True, .FormatKind = "int"},
                    New PrintColumn With {.FieldName = "Status", .Header = "Status"},
                    New PrintColumn With {.FieldName = "Notes", .Header = "Notes"}
                }
        End Select
    End Function

    Private Function GetCurrentReportTitle() As String
        Select Case _activeReport
            Case ReportType.Sales
                Return "Sales Report"
            Case ReportType.Financial
                Return "Financial Report"
            Case ReportType.Cashier
                Return "Cashier Report"
            Case ReportType.Delivery
                Return "Delivery Report"
            Case Else
                Return "Returns Report"
        End Select
    End Function

    Private Function GetHeaderSummaryLines() As List(Of Tuple(Of String, String, Boolean))
        Dim lines As New List(Of Tuple(Of String, String, Boolean))()

        Select Case _activeReport
            Case ReportType.Delivery
                lines.Add(Tuple.Create("TOTAL ITEMS", _currentTotals.TotalItemCount.ToString("N0"), False))
                lines.Add(Tuple.Create("TOTAL QTY", _currentTotals.TotalQuantity.ToString("N0"), False))
                Dim overallValue As String = If(_deliveryCostColumnAvailable,
                                                _currentTotals.AggregateCost.ToString("N2"),
                                                _currentTotals.TotalQuantity.ToString("N0"))
                lines.Add(Tuple.Create("OVERALL TOTAL", overallValue, False))

            Case ReportType.Returns
                lines.Add(Tuple.Create("TOTAL RETURNS", _currentTotals.RecordCount.ToString("N0"), False))
                lines.Add(Tuple.Create("TOTAL RETURN QTY", _currentTotals.TotalReturnedQuantity.ToString("N0"), False))
                lines.Add(Tuple.Create("OVERALL TOTAL", _currentTotals.TotalReturnedQuantity.ToString("N0"), False))

            Case Else
                lines.Add(Tuple.Create("TOTAL SALES", _currentTotals.GrossSales.ToString("N2"), False))
                lines.Add(Tuple.Create("TOTAL RETURN", 0D.ToString("N2"), False))
                lines.Add(Tuple.Create("OVERALL TOTAL", _currentTotals.NetSales.ToString("N2"), True))
        End Select

        Return lines
    End Function

    Private Function GetGridFooterTotal() As Tuple(Of String, String)
        Select Case _activeReport
            Case ReportType.Delivery
                Dim value As String = If(_deliveryCostColumnAvailable,
                                         _currentTotals.AggregateCost.ToString("N2"),
                                         _currentTotals.TotalQuantity.ToString("N0"))
                Return Tuple.Create("TOTAL :", value)

            Case ReportType.Returns
                Return Tuple.Create("TOTAL :", _currentTotals.TotalReturnedQuantity.ToString("N0"))

            Case Else
                Return Tuple.Create("TOTAL :", _currentTotals.NetSales.ToString("N2"))
        End Select
    End Function

    Private Function FormatPrintCellValue(row As DataRow, col As PrintColumn) As String
        If row Is Nothing OrElse col Is Nothing OrElse Not row.Table.Columns.Contains(col.FieldName) Then Return String.Empty

        Dim value As Object = row(col.FieldName)
        If value Is Nothing OrElse value Is DBNull.Value Then Return String.Empty

        Select Case col.FormatKind
            Case "date"
                Dim d As DateTime
                If DateTime.TryParse(Convert.ToString(value), d) Then Return d.ToString("MM/dd/yyyy")
            Case "int"
                Return ToDecimal(value).ToString("N0")
            Case "money"
                Return ToDecimal(value).ToString("N2")
        End Select

        Return Convert.ToString(value)
    End Function

    Private Sub PrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs)
        If _printRows Is Nothing OrElse _printColumns Is Nothing OrElse _printColumns.Count = 0 Then
            e.HasMorePages = False
            Return
        End If

        Dim fontBrand As New Font("Arial Black", 18, FontStyle.Bold)
        Dim fontTitle As New Font("Segoe UI", 12, FontStyle.Bold)
        Dim fontHeader As New Font("Segoe UI", 9, FontStyle.Bold)
        Dim fontBody As New Font("Segoe UI", 9, FontStyle.Regular)
        Dim fontMeta As New Font("Segoe UI", 9, FontStyle.Regular)

        Dim y As Integer = e.MarginBounds.Top
        Dim left As Integer = e.MarginBounds.Left
        Dim rowHeight As Integer = 24
        Dim colWidth As Integer = CInt(Math.Floor(e.MarginBounds.Width / Math.Max(1, _printColumns.Count)))

        Dim storeName As String = _printHeaderStoreName
        Dim storeSize As SizeF = e.Graphics.MeasureString(storeName, fontBrand)
        e.Graphics.DrawString(storeName, fontBrand, Brushes.Black, left + (e.MarginBounds.Width - storeSize.Width) \ 2, y)
        y += CInt(Math.Ceiling(storeSize.Height)) + 6

        Dim dFrom As DateTime
        Dim dTo As DateTime
        GetSelectedDateRange(dFrom, dTo)

        Dim summaryLines As List(Of Tuple(Of String, String, Boolean)) = GetHeaderSummaryLines()
        Dim summaryLineHeight As Integer = 16
        Dim rightBlockWidth As Integer = CInt(Math.Floor(e.MarginBounds.Width * 0.4))
        Dim rightBlockX As Integer = e.MarginBounds.Right - rightBlockWidth
        Dim labelWidth As Integer = CInt(Math.Floor(rightBlockWidth * 0.65))
        Dim valueWidth As Integer = rightBlockWidth - labelWidth
        Dim sfLeft As New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}
        Dim sfRight As New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}

        Dim leftBlockX As Integer = left
        Dim leftBlockY As Integer = y
        Dim dateLabelGap As Integer = 6
        Dim fromLabel As String = "FROM :"
        Dim toLabel As String = "TO :"
        Dim fromLabelSize As SizeF = e.Graphics.MeasureString(fromLabel, fontHeader)
        Dim toLabelSize As SizeF = e.Graphics.MeasureString(toLabel, fontHeader)
        Dim dateValueX As Integer = leftBlockX + CInt(Math.Max(fromLabelSize.Width, toLabelSize.Width)) + dateLabelGap

        e.Graphics.DrawString(fromLabel, fontHeader, Brushes.Black, leftBlockX, leftBlockY)
        e.Graphics.DrawString(dFrom.ToString("MMM dd, yyyy"), fontMeta, Brushes.Black, dateValueX, leftBlockY)
        leftBlockY += summaryLineHeight

        e.Graphics.DrawString(toLabel, fontHeader, Brushes.Black, leftBlockX, leftBlockY)
        e.Graphics.DrawString(dTo.ToString("MMM dd, yyyy"), fontMeta, Brushes.Black, dateValueX, leftBlockY)

        Dim rightBlockY As Integer = y
        Dim underlineAfterIndex As Integer = If(summaryLines.Count >= 3, 1, -1)
        For i As Integer = 0 To summaryLines.Count - 1
            Dim line As Tuple(Of String, String, Boolean) = summaryLines(i)
            e.Graphics.DrawString(line.Item1 & " :", fontHeader, Brushes.Black, New RectangleF(rightBlockX, rightBlockY, labelWidth, summaryLineHeight), sfLeft)
            Dim valueBrush As Brush = If(line.Item3, Brushes.Red, Brushes.Black)
            e.Graphics.DrawString(line.Item2, fontHeader, valueBrush, New RectangleF(rightBlockX + labelWidth, rightBlockY, valueWidth, summaryLineHeight), sfRight)
            rightBlockY += summaryLineHeight

            If i = underlineAfterIndex Then
                Dim lineY As Integer = rightBlockY - 2
                e.Graphics.DrawLine(Pens.Black, rightBlockX + labelWidth, lineY, rightBlockX + rightBlockWidth, lineY)
                rightBlockY += 2
            End If
        Next

        y = Math.Max(leftBlockY + summaryLineHeight, rightBlockY) + 10

        Dim title As String = GetCurrentReportTitle().ToUpperInvariant()
        Dim titleSize As SizeF = e.Graphics.MeasureString(title, fontTitle)
        e.Graphics.DrawString(title, fontTitle, Brushes.Black, left + (e.MarginBounds.Width - titleSize.Width) \ 2, y)
        y += CInt(Math.Ceiling(titleSize.Height)) + 8

        Dim x As Integer = left
        For i As Integer = 0 To _printColumns.Count - 1
            Dim width As Integer = If(i = _printColumns.Count - 1, e.MarginBounds.Right - x, colWidth)
            e.Graphics.FillRectangle(Brushes.LightGray, x, y, width, rowHeight)
            e.Graphics.DrawRectangle(Pens.Black, x, y, width, rowHeight)
            e.Graphics.DrawString(_printColumns(i).Header, fontHeader, Brushes.Black, New RectangleF(x + 2, y + 4, width - 4, rowHeight - 4))
            x += width
        Next
        y += rowHeight

        Dim maxRows As Integer = Math.Max(1, CInt(Math.Floor((e.MarginBounds.Bottom - y - 50) / CDbl(rowHeight))))
        Dim rowsPrinted As Integer = 0

        While _printRowIndex < _printRows.Rows.Count AndAlso rowsPrinted < maxRows
            x = left
            Dim row As DataRow = _printRows.Rows(_printRowIndex)

            For i As Integer = 0 To _printColumns.Count - 1
                Dim width As Integer = If(i = _printColumns.Count - 1, e.MarginBounds.Right - x, colWidth)
                e.Graphics.DrawRectangle(Pens.Black, x, y, width, rowHeight)

                Dim valueText As String = FormatPrintCellValue(row, _printColumns(i))
                Dim sf As New StringFormat With {
                    .Alignment = If(_printColumns(i).AlignRight, StringAlignment.Far, StringAlignment.Near),
                    .LineAlignment = StringAlignment.Center
                }
                e.Graphics.DrawString(valueText, fontBody, Brushes.Black, New RectangleF(x + 2, y + 2, width - 4, rowHeight - 4), sf)
                x += width
            Next

            y += rowHeight
            _printRowIndex += 1
            rowsPrinted += 1
        End While

        If _printRowIndex >= _printRows.Rows.Count Then
            Dim footerTotal As Tuple(Of String, String) = GetGridFooterTotal()
            Dim footerText As String = footerTotal.Item1
            Dim footerValue As String = footerTotal.Item2
            Dim footerY As Integer = y + 6
            If footerY + rowHeight <= e.MarginBounds.Bottom Then
                e.Graphics.DrawString(footerText, fontHeader, Brushes.Black, left + 4, footerY)
                e.Graphics.DrawString(footerValue, fontHeader, Brushes.Black, e.MarginBounds.Right - 4, footerY, sfRight)
                y = footerY + rowHeight
            End If
        End If

        Dim pageText As String = "Page " & _printPageNumber.ToString()
        Dim pageSize As SizeF = e.Graphics.MeasureString(pageText, fontMeta)
        e.Graphics.DrawString(pageText, fontMeta, Brushes.Black, left + (e.MarginBounds.Width - pageSize.Width) \ 2, e.MarginBounds.Bottom + 24)

        If _printRowIndex < _printRows.Rows.Count Then
            _printPageNumber += 1
            e.HasMorePages = True
            Return
        End If

        e.HasMorePages = False
        _printRowIndex = 0
        _printPageNumber = 1
    End Sub

    Private Sub btnSalesReport_Click(sender As Object, e As EventArgs) Handles btnSalesReport.Click
        SetActiveReport(ReportType.Sales)
    End Sub

    Private Sub btnFinancialReports_Click(sender As Object, e As EventArgs) Handles btnFinancialReports.Click
        SetActiveReport(ReportType.Financial)
    End Sub

    Private Sub btnCashierReport_Click(sender As Object, e As EventArgs) Handles btnCashierReport.Click
        SetActiveReport(ReportType.Cashier)
    End Sub

    Private Sub btnDeliveryReportTab_Click(sender As Object, e As EventArgs) Handles btnDeliveryReportTab.Click
        SetActiveReport(ReportType.Delivery)
    End Sub

    Private Sub btnReturnsReportTab_Click(sender As Object, e As EventArgs) Handles btnReturnsReportTab.Click
        SetActiveReport(ReportType.Returns)
    End Sub

    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        If IsCashierUser() Then
            SwitchSidebarNavigation(btnHome, New FrmDashboardCashier())
            Return
        End If

        If IsStaffUser() Then
            SwitchSidebarNavigation(btnHome, New FrmDashboardStaff())
            Return
        End If

        SwitchSidebarNavigation(btnHome, New frmHome())
    End Sub

    Private Sub btnFileMaintenance_Click(sender As Object, e As EventArgs) Handles btnFileMaintenance.Click
        If IsCashierUser() Then
            SwitchSidebarNavigation(btnFileMaintenance, New FrmDiscountCashier())
        ElseIf IsStaffUser() Then
            SwitchSidebarNavigation(btnFileMaintenance, New FileMaintenance.Category())
        Else
            SwitchSidebarNavigation(btnFileMaintenance, New FileMaintenance.User())
        End If
    End Sub

    Private Sub btnDelivery_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
        SwitchSidebarNavigation(btnDelivery, New DeliveriesModuleForm())
    End Sub

    Private Sub btnReturns_Click(sender As Object, e As EventArgs) Handles btnReturns.Click
        SwitchSidebarNavigation(btnReturns, New SupplierReturnsModuleForm())
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        SwitchSidebarNavigation(btnInventory, New InventoryModuleForm())
    End Sub

    Private Sub btnPos_Click(sender As Object, e As EventArgs) Handles btnPos.Click
        SwitchSidebarNavigation(btnPos, New FrmPOS())
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        SwitchSidebarNavigation(btnTransaction, New TransactionsModuleForm())
    End Sub

    Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
        SwitchSidebarNavigation(btnAuditTrail, New AuditTrailModuleForm())
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        SwitchSidebarNavigation(btnReports)
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to log out?",
                                                      "Logout",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question)
        If confirm <> DialogResult.Yes Then Return

        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                SessionService.EndCurrentSession("Logout")
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging out: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        LogActivity(FrmLogin.CurrentUser.UserID,
                    FrmLogin.CurrentUser.FullName,
                    FrmLogin.CurrentUser.Username,
                    FrmLogin.CurrentUser.Role,
                    "User Logged Out.")

        FrmLogin.CurrentUser.UserID = 0
        FrmLogin.CurrentUser.Username = ""
        FrmLogin.CurrentUser.Role = ""
        FrmLogin.CurrentUser.FullName = ""

        Me.Hide()
        Dim loginForm As New FrmLogin()
        loginForm.Show()
    End Sub

    Private Sub btnPreviousPage_Click(sender As Object, e As EventArgs) Handles btnPreviousPage.Click
        If _currentPage <= 1 Then Return
        _currentPage -= 1
        ApplyCurrentPageData()
        ApplyActiveReportGridFormatting()
        DGVtable.ClearSelection()
    End Sub

    Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        Dim totalRows As Integer = If(_allRows, New DataTable()).Rows.Count
        Dim totalPages As Integer = Math.Max(1, CInt(Math.Ceiling(totalRows / CDbl(_pageSize))))
        If _currentPage >= totalPages Then Return

        _currentPage += 1
        ApplyCurrentPageData()
        ApplyActiveReportGridFormatting()
        DGVtable.ClearSelection()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadCurrentReport()
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dtpFilters_ValueChanged(sender As Object, e As EventArgs) Handles dtpFromDate.ValueChanged, dtpTodate.ValueChanged
        If _isLoadingDateFilters Then Return
        LoadCurrentReport()
    End Sub

    Private Sub cbCashierName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbCashierName.SelectedIndexChanged
        If _isLoadingCashierFilter Then Return
        If _activeReport <> ReportType.Cashier Then Return
        LoadCashierReport()
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        If _allRows Is Nothing OrElse _allRows.Rows.Count = 0 Then
            MessageBox.Show("No data to print.",
                            "Reports",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Return
        End If

        _printRows = _allRows.Copy()
        _printColumns = GetPrintColumnsForCurrentReport()
        _printRowIndex = 0
        _printPageNumber = 1

        Using printDlg As New PrintDialog()
            printDlg.Document = _printDocument
            If printDlg.ShowDialog() <> DialogResult.OK Then Return
            _printDocument.PrinterSettings = printDlg.PrinterSettings

            Using preview As New PrintPreviewDialog()
                preview.Document = _printDocument
                preview.ShowDialog()
            End Using
        End Using
    End Sub

    Private Function ResolveLoggedCashierName() As String
        Dim cashierName As String = If(FrmLogin.CurrentUser.FullName, String.Empty).Trim()
        If cashierName.Length > 0 Then Return cashierName

        cashierName = If(SessionContext.FullName, String.Empty).Trim()
        If cashierName.Length > 0 Then Return cashierName

        cashierName = If(FrmLogin.CurrentUser.Username, String.Empty).Trim()
        If cashierName.Length > 0 Then Return cashierName

        Return If(SessionContext.Username, String.Empty).Trim()
    End Function
End Class
