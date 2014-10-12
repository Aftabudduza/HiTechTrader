Imports System.IO
Imports System.Data
Partial Class Admin_UploadProduct
    Inherits System.Web.UI.Page
    Public ds As DataSet = New DataSet()
    Dim dt As New DataTable
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
#Region "Product Inload Methods"
    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        'inload the file
        Try
            If Me.uplProduct.FileName <> "" Then
                'read the file in
                Dim filePath As String = Path.Combine(Request.PhysicalApplicationPath, "Files\Product\")

                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If

                Dim nFile As String = Path.Combine(filePath, "INLOAD_Product_" & DateTime.Now.ToString("yyyyMMddhhmmss") & ".txt")

                Try
                    If System.IO.File.Exists(nFile) Then
                        System.IO.File.Delete(nFile)
                    End If
                Catch ex As Exception

                End Try
                uplProduct.SaveAs(nFile)

                'process the file
                'Dim sr As StreamReader = Nothing
                Dim icnt As Integer = 0
                Dim ierr As Integer = 0
                Dim itotcnt As Integer = 0
                Try
                    Dim aLines As System.Collections.Generic.List(Of String) = Actions.Get_FileBytesToString(nFile)
                    'sr = New StreamReader(sWorkingFile)
                    Dim sLine As String = ""

                    sLine = aLines(0) 'sr.ReadLine
                    Dim aHeader As New ArrayList
                    aHeader = BRIClassLibrary.StringMethods.Parse_CSV_Line(sLine, ",", """")
                    'anatech

                    For i As Integer = 1 To aLines.Count - 1
                        'Do While sr.Peek <> -1
                        sLine = aLines(i) 'sr.ReadLine
                        If ProcessLine(sLine, aHeader) Then
                            icnt += 1
                        Else
                            ierr += 1
                        End If
                        itotcnt += 1
                        'Loop
                    Next
                Catch ex As Exception
                Finally
                    'sr.Close()
                End Try
                Dim sMessage As String = icnt & " Product(s) Processed Out Of " & itotcnt & " With " & ierr & " Error(s)"
                lblMsg.Text = sMessage

            Else
                lblMsg.Text = "You Must Select file To Import."
            End If
        Catch ex As Exception
            lblMsg.Text = "Error Inloading:" & ex.Message
            appGlobal.LogDataError("Error Inloading:" & ex.Message)
        End Try
    End Sub
    Private Function ProcessLine(ByVal sLine As String, ByVal aHeader As ArrayList) As Boolean
        Try
            Dim aLine As New ArrayList
            aLine = BRIClassLibrary.StringMethods.Parse_CSV_Line(sLine, ",", """")

            Dim sItemNumber As String = ""
            Dim sCategoryName As String = ""
            Dim sParentCategoryName As String = ""
            Dim nParentCategoryID As Integer = 0
            Dim nCategoryID As Integer = 0
            Dim sManufacturerName As String = ""
            Dim iManufacturerId As Integer = 0
            Dim sProductImage As String = ""
            Dim sAge As String = ""
            Dim sCondition As String = ""

            For i As Integer = 0 To aHeader.Count - 1
                If CStr(aHeader(i)).ToUpper = "ITEMNUMBER" Then
                    sItemNumber = aLine(i)
                End If
            Next
            For i As Integer = 0 To aHeader.Count - 1
                If CStr(aHeader(i)).ToUpper = "CATEGORYNAME" Then
                    sCategoryName = aLine(i)
                End If
            Next
            For i As Integer = 0 To aHeader.Count - 1
                If CStr(aHeader(i)).ToUpper = "PARENTCATEGORY" Then
                    sParentCategoryName = aLine(i)
                End If
            Next
            For i As Integer = 0 To aHeader.Count - 1
                If CStr(aHeader(i)).ToUpper = "MAKE" Then
                    sManufacturerName = aLine(i)
                End If
            Next

            For i As Integer = 0 To aHeader.Count - 1
                If CStr(aHeader(i)).ToUpper = "PRODUCTIMAGE" Then
                    sProductImage = aLine(i)
                End If
            Next

            For i As Integer = 0 To aHeader.Count - 1
                If CStr(aHeader(i)).ToUpper = "AGE" Then
                    sAge = aLine(i)
                End If
            Next

            For i As Integer = 0 To aHeader.Count - 1
                If CStr(aHeader(i)).ToUpper = "CONDITION" Then
                    sCondition = aLine(i)
                End If
            Next

            If sAge.Length > 0 Then
                Dim nAge As Integer = GetAgeCondition(sAge, 3)
            End If

            If sCondition.Length > 0 Then
                Dim nCondition As Integer = GetAgeCondition(sCondition, 3)
            End If

            If sItemNumber = "" Then
                lblMsg.Text = "No Product in Inload File"
                Return False
            End If

            If sParentCategoryName.Length > 0 Then
                nParentCategoryID = GetParent(sParentCategoryName.Trim)
            End If

            If nParentCategoryID > 0 Then
                If sCategoryName.Length > 0 Then
                    nCategoryID = GetProductCategory(sCategoryName.Trim, nParentCategoryID)
                End If
            End If

            If sManufacturerName.Length > 0 Then
                iManufacturerId = GetManufacturer(sManufacturerName.Trim)
            End If

            ds = Nothing
            Dim sSQLTest As String = ""
            'test if in system

            sSQLTest = "SELECT Id FROM Product WHERE ItemNumber='" & sItemNumber.Trim & "'"

            Dim sProdId As String = ""
            Try
                ds = SQLData.generic_select(sSQLTest, strConnection)
                If Not ds Is Nothing Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        sProdId = ds.Tables(0).Rows(0)(0).ToString()
                    End If
                End If
            Catch ex As Exception

            End Try

            Dim aSkips As ArrayList = Me.aSkipFields

            Dim iProdCategoryId As New ArrayList

            Dim sPartNumbers As String = ""
            Dim sHead As String = ""
            If sProdId = "" Then
                '************************************************************
                'INSERT!!!!!!!!!!!!!!!!!!!!!!
                '------------------------------------------------------------
                'if not add
                Dim sb As New System.Text.StringBuilder
                Dim sbFields As New System.Text.StringBuilder
                Dim sbValues As New System.Text.StringBuilder
                Dim sbAddFields As New System.Text.StringBuilder
                Dim sbAddValues As New System.Text.StringBuilder
                Dim bFirst As Boolean = True
                Dim bFirstAdd As Boolean = True
                Dim bHitPriceBreak As Boolean = False
                Dim bHasBriefDescription As Boolean = False
                Dim sDescription As String = ""

                Dim sAddTable As String = ""

                With sb
                    .Append("INSERT INTO Product (")
                    For x As Integer = 0 To aHeader.Count - 1
                        If aHeader(x).Trim.ToString.Trim <> "" Then
                            If Not Actions.inArray(aSkips, CStr(aHeader(x).Trim).ToUpper) Then
                                If Not bFirst Then
                                    sbFields.Append(",")
                                    sbValues.Append(",")
                                Else
                                    bFirst = False
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "ITEMNUMBER" Then
                                    sbFields.Append("[ItemNumber]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "TITLE" Then
                                    sbFields.Append("[ProductName]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "MAKE" Then
                                    sbFields.Append("[ManufacturerId],")
                                    sbValues.Append("'" & iManufacturerId & "',")
                                    sbFields.Append("[Make]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "MODEL" Then
                                    sbFields.Append("[Model]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "DESCRIPTION" Then
                                    sbFields.Append("[Description]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "CONDITION" Then
                                    sbFields.Append("[Condition]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "AGE" Then
                                    sbFields.Append("[Age]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "WEIGHT" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[Weight]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PRICE" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[Price]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "QTY" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[Qty],[TotalPieces]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "','" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "QTYSOLD" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[QuantitySold]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "LOWESTPRICE" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[LowestPrice]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "COSTOFGOODS" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[CostofGoods]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "AUCTIONSTART" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[AuctionStart]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "SELLINGPRICE" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[SellingPrice]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "CATEGORYNAME" Then
                                    sbFields.Append("[Category]")
                                    sbValues.Append("'" & nCategoryID & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PARENTCATEGORY" Then
                                    sbFields.Append("[ParentCategory]")
                                    sbValues.Append("'" & nParentCategoryID & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "LOCATION" Then
                                    sbFields.Append("[Location]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "BARCODE" Then
                                    sbFields.Append("[Barcode]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "BARCODEPARENT" Then
                                    sbFields.Append("[BarcodeParent]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "HOLD" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[IsHold]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "SOLD" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[IsSold]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "SPECIALS" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[IsSpecial]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "NEW" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[IsNewArrivalsPage]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "FEATURED" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[IsFeaturedItem]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "OFFTRUCK" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[IsJustOfftheTruck]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "CONSIGNMENT" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[IsConsignmentItem]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "DELETED" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[IsDeleteItem]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "LABXAD" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[IsLabX]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "MAINPIC" Then
                                    sbFields.Append("[ImageFileName]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PRODUCTIMAGE" Then
                                    sbFields.Append("[ThumbFileName]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "NOWATERMARKIMAGE" Then
                                    sbFields.Append("[Watermarkimg]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If

                            End If

                        End If
                        'test for no string ''
                    Next
                    If Not Session("Id") Is Nothing Then
                        sbFields.Append(",[CreatorID]")
                        sbValues.Append(",'" & CInt(Session("Id").ToString()) & "'")
                        sbFields.Append(",[DateCreated]")
                        sbValues.Append(",'" & CDate(DateTime.UtcNow.ToString()) & "'")
                    End If


                    sb.Append(sbFields.ToString & ") VALUES (" & sbValues.ToString & ");")


                End With
                System.Diagnostics.Debug.WriteLine(sb.ToString)

                If Not SQLData.generic_command(sb.ToString, SQLData.ConnectionString) Then
                    Return False
                End If
                sProdId = SQLData.generic_scalar(sSQLTest, SQLData.ConnectionString)



            Else
                '*******************************************
                'UPDATE
                '------------------------------------------
                'else update
                Dim sb As New System.Text.StringBuilder
                Dim sbAdd As New System.Text.StringBuilder
                Dim sbAddFields As New System.Text.StringBuilder
                Dim sbAddValues As New System.Text.StringBuilder

                '****************************************

                With sb
                    .Append("UPDATE Product SET ")
                    Dim bFirst As Boolean = True
                    Dim bFirstAdd As Boolean = True
                    Dim bHitPriceBreak As Boolean = False
                    Dim bHasBriefDescription As Boolean = False
                    Dim sDescription As String = ""

                    For x As Integer = 0 To aHeader.Count - 1
                        If aHeader(x).Trim.ToString.Trim <> "" Then
                            'System.Diagnostics.Debug.WriteLine(CStr(aHeader(x).Trim))
                            'fill in the data                        

                            If Not Actions.inArray(aSkips, CStr(aHeader(x).Trim).ToUpper) Then
                                'Use this to test for Brief description
                                If Not bFirst Then
                                    .Append(",")
                                Else
                                    bFirst = False
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "ITEMNUMBER" Then
                                    .Append("[ItemNumber]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "TITLE" Then
                                    .Append("[ProductName]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "MAKE" Then
                                    .Append("[ManufacturerId]=" & "'" & iManufacturerId & "', [Make]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "MODEL" Then
                                    .Append("[Model]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "DESCRIPTION" Then
                                    .Append("[Description]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "CONDITION" Then
                                    .Append("[Condition]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "AGE" Then
                                    .Append("[Age]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "WEIGHT" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[Weight]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PRICE" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[Price]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "QTY" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[Qty]=" & "'" & CStr(aLine(x)).Trim & "', [TotalPieces]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "QTYSOLD" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[QuantitySold]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "LOWESTPRICE" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[LowestPrice]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "COSTOFGOODS" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[CostofGoods]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "AUCTIONSTART" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[AuctionStart]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "SELLINGPRICE" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[SellingPrice]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "CATEGORYNAME" Then
                                    .Append("[Category]=" & "'" & nCategoryID & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PARENTCATEGORY" Then
                                    .Append("[ParentCategory]=" & "'" & nParentCategoryID & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "LOCATION" Then
                                    .Append("[Location]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "BARCODE" Then
                                    .Append("[Barcode]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "BARCODEPARENT" Then
                                    .Append("[BarcodeParent]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "HOLD" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[IsHold]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "SOLD" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[IsSold]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "SPECIALS" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[IsSpecial]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "NEW" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[IsNewArrivalsPage]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "FEATURED" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[IsFeaturedItem]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "OFFTRUCK" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[IsJustOfftheTruck]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "CONSIGNMENT" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[IsConsignmentItem]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "DELETED" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[IsDeleteItem]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "LABXAD" Then
                                    If aLine(x) <> "" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[IsLabX]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "MAINPIC" Then
                                    .Append("[ImageFileName]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PRODUCTIMAGE" Then
                                    .Append("[ThumbFileName]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "NOWATERMARKIMAGE" Then
                                    .Append("[Watermarkimg]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                            End If

                        End If 'test for no value
                    Next

                    If Not Session("Id") Is Nothing Then
                        .Append(",[EditorID]=" & "'" & CInt(Session("Id").ToString()) & "'")
                        .Append(",[LastEdited]=" & "'" & CDate(DateTime.UtcNow.ToString()) & "'")
                    End If

                    .Append(" WHERE Id=" & sProdId)
                End With
                System.Diagnostics.Debug.WriteLine(sb.ToString)
                System.Diagnostics.Debug.WriteLine(sbAdd.ToString)

                If Not SQLData.generic_command(sb.ToString, SQLData.ConnectionString) Then
                    Return False
                End If
            End If


            'PRODUCT Category CROSS REFERENCE
            If Not GetProductCategoryCrossRef(sCategoryName, nParentCategoryID, sProdId) Then
                Throw New Exception("Error generating Category Cross Ref")
            End If

            'PRODUCT Image CROSS REFERENCE
            If Not GetProductImageCrossRef(sProductImage, sProdId) Then
                Throw New Exception("Error generating Images")
            End If

        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    Private Function aSkipFields() As ArrayList
        Dim a As New ArrayList
        a.Add("CATEGORYNO")
        Return a
    End Function
    Private Function GetManufacturer(ByVal sManName As String) As Integer
        'test for the ManuFacturer name
        Dim sSQL As String = "select id from manufacturer where name='" & sManName.Trim & "'"
        Dim sManId As String = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        If sManId = "" Then
            'add it
            Dim sIns As String = "INSERT INTO MANUFACTURER ([NAME]) VALUES('" & sManName.Trim & "')"
            SQLData.generic_command(sIns, SQLData.ConnectionString)
            sManId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        End If
        If IsNumeric(sManId) Then
            Return CInt(sManId)
        Else
            Return 0
        End If
    End Function
    Private Function GetParent(ByVal sName As String) As Integer
        'test for the ManuFacturer name
        Dim sSQL As String = "select id from Category where CategoryName='" & sName.Trim & "'"
        Dim sId As String = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        If sId = "" Then
            'add it
            Dim sIns As String = "INSERT INTO Category ([CategoryName], [CategoryParentId]) VALUES('" & sName.Trim & "','0')"
            SQLData.generic_command(sIns, SQLData.ConnectionString)
            sId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        End If
        If IsNumeric(sId) Then
            Return CInt(sId)
        Else
            Return 0
        End If
    End Function
    Private Function GetProductCategory(ByVal sProductCategory As String, ByVal nParentId As Integer) As Integer
        'test for the Product Category ID
        'sProdCat is split by ; groupings
        Dim aCat As String() = sProductCategory.Split(",")
        Dim sCatId As String = "0"
        For Each sProdCat As String In aCat
            sProdCat = sProdCat.Trim
            If sProdCat.Trim <> "" Then
                Dim sSQL As String = "select id from Category where [CategoryName]='" & sProdCat.Trim.Trim & "' and CategoryParentId=" & nParentId
                sCatId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
                If sCatId = "" Then
                    'add it
                    Dim sIns As String = "INSERT INTO Category ([CategoryName], CategoryParentId) VALUES('" & sProdCat.Trim & "','" & nParentId & "')"
                    SQLData.generic_command(sIns, SQLData.ConnectionString)
                    sCatId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
                End If
            End If
        Next
        If IsNumeric(sCatId) Then
            Return CInt(sCatId)
        Else
            Return 0
        End If
    End Function
    Private Function GetProductImageCrossRef(ByVal sProductImage As String, ByVal nProductId As Integer) As Boolean
        Try
            Dim bSuccess As Boolean = True
            Dim sId As String = "0"
            Dim aImages As String() = sProductImage.Split(",")
            'clear old Category
            'add ALL new
            Try
                Dim sDel As String = "DELETE FROM ProductImageCrossRef WHERE ProductId=" & nProductId
                SQLData.generic_command(sDel, SQLData.ConnectionString)
            Catch ex As Exception

            End Try


            If sProductImage.Trim = "" Then
                Return True
            ElseIf sProductImage.Trim = "None Available" Then
                Return True
            ElseIf sProductImage.Trim = "#N/A" Or sProductImage.Trim = "N/A" Then
                Return True
            End If

            If aImages.Length > 0 Then
                For Each sProdImg As String In aImages
                    sProdImg = sProdImg.Trim
                    If Not String.IsNullOrEmpty(sProdImg) Then
                        Dim sSQL As String = "select id from ProductImageCrossRef where [ImageUrl]='" & sProdImg.Trim.Trim & "' and ProductId=" & nProductId
                        sId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
                        If sId = "" Then
                            'add it
                            Dim sIns As String = "INSERT INTO ProductImageCrossRef ([ProductId], ImageUrl) VALUES('" & nProductId & "','" & sProdImg.Trim & "')"
                            SQLData.generic_command(sIns, SQLData.ConnectionString)
                            sId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
                        End If
                    End If
                Next
            End If

            Return bSuccess

        Catch ex As Exception

        End Try

    End Function
    Private Function GetProductCategoryCrossRef(ByVal sProductCategory As String, ByVal nParentId As Integer, ByVal nProductId As Integer) As Boolean
        Try
            Dim bSuccess As Boolean = True
            Dim sId As String = "0"
            Dim aCategories As String() = sProductCategory.Split(",")
            Dim nCategoryID As Integer = 0
            'clear old Category
            'add ALL new
            Try
                Dim sDel As String = "DELETE FROM PRODUCTCATEGORYCROSSREF WHERE ProductId=" & nProductId
                SQLData.generic_command(sDel, SQLData.ConnectionString)
            Catch ex As Exception

            End Try


            If sProductCategory.Trim = "" Then
                Return True
            ElseIf sProductCategory.Trim = "None Available" Then
                Return True
            ElseIf sProductCategory.Trim = "#N/A" Or sProductCategory.Trim = "N/A" Then
                Return True
            End If

            If aCategories.Length > 0 Then
                For Each sProdCat As String In aCategories
                    sProdCat = sProdCat.Trim
                    If Not String.IsNullOrEmpty(sProdCat) Then
                        nCategoryID = GetProductCategory(sProdCat.Trim, nParentId)
                        If nCategoryID > 0 Then
                            Dim sSQL As String = "SELECT ID FROM PRODUCTCATEGORYCROSSREF WHERE  PRODUCTID=" & nProductId & " AND PRODUCTCATEGORYID=" & nCategoryID
                            sId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
                            If sId = "" Then
                                'add it
                                Dim sIns As String = "INSERT INTO PRODUCTCATEGORYCROSSREF ([PRODUCTID], PRODUCTCATEGORYID) VALUES('" & nProductId & "','" & nCategoryID & "')"
                                SQLData.generic_command(sIns, SQLData.ConnectionString)
                                sId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
                            End If
                        End If

                    End If
                Next
            End If

            Return bSuccess

        Catch ex As Exception

        End Try

    End Function
    Private Function GetAgeCondition(ByVal sName As String, ByVal nType As Integer) As Integer
        Dim sSQL As String = "select Id from AdminSystemData where name='" & sName.Trim & "' and Type = " & nType
        Dim sId As String = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        If sId = "" Then
            'add it
            Dim sIns As String = "INSERT INTO AdminSystemData ([NAME], [Type]) VALUES('" & sName.Trim & "','" & nType & "')"
            SQLData.generic_command(sIns, SQLData.ConnectionString)
            sId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        End If
        If IsNumeric(sId) Then
            Return CInt(sId)
        Else
            Return 0
        End If
    End Function
#End Region
#Region "Product Upload Methods"
    Public Sub GenerateFile()
        Try

            Dim fileName As String = String.Empty
            '  Dim str As String = "SELECT p.*, c.CategoryName ParentCategoryName   FROM Product p, Category c WHERE p.ParentCategory = c.Id ORDER BY p.ItemNumber asc "
            Dim str As String = "SELECT p.*, isnull(c.CategoryName,'') ParentCategoryName  FROM Product p  LEFT join Category c on p.ParentCategory = c.Id  ORDER BY p.ItemNumber asc  "


            If str.Length > 0 Then
                dt = BRIClassLibrary.SQLData.generic_select(str, strConnection).Tables(0)
            End If

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim filePath = Path.Combine(Request.PhysicalApplicationPath, "Files\Product\")
                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If

                fileName = "Product_Export_" & DateTime.Now.ToString("yyyyMMddhhmmss") & ".csv"
                filePath = Path.Combine(filePath, fileName)
                Try
                    If System.IO.File.Exists(filePath) Then
                        System.IO.File.Delete(filePath)
                    End If
                Catch ex As Exception

                End Try

                Dim wr = New StreamWriter(filePath)
                wr.Write("Itemnumber")
                wr.Write(",")
                wr.Write("Title")
                wr.Write(",")
                wr.Write("Make")
                wr.Write(",")
                wr.Write("Model")
                wr.Write(",")
                wr.Write("Description")
                wr.Write(",")
                wr.Write("Admin Notes")
                wr.Write(",")
                wr.Write("Condition")
                wr.Write(",")
                wr.Write("Age")
                wr.Write(",")
                wr.Write("Weight")
                wr.Write(",")
                wr.Write("Price")
                wr.Write(",")
                wr.Write("Qty")
                wr.Write(",")
                wr.Write("Qty Sold")
                wr.Write(",")
                wr.Write("POD Price")
                wr.Write(",")
                wr.Write("Cost of Goods")
                wr.Write(",")
                wr.Write("Auction Start")
                wr.Write(",")
                wr.Write("Selling Price")
                wr.Write(",")
                wr.Write("Category Name")
                wr.Write(",")
                wr.Write("Parent Category")
                wr.Write(",")
                wr.Write("Location")
                wr.Write(",")
                wr.Write("Barcode")
                wr.Write(",")
                wr.Write("Barcode Parent")
                wr.Write(",")
                wr.Write("Hold")
                wr.Write(",")
                wr.Write("Sold")
                wr.Write(",")
                wr.Write("Specials")
                wr.Write(",")
                wr.Write("New")
                wr.Write(",")
                wr.Write("Featured")
                wr.Write(",")
                wr.Write("Offtruck")
                wr.Write(",")
                wr.Write("Consignment")
                wr.Write(",")
                wr.Write("Deleted")
                wr.Write(",")
                wr.Write("LabX Ad")
                wr.Write(",")
                wr.Write("Main Pic")
                wr.Write(",")
                wr.Write("product_image")
                wr.Write(",")
                wr.Write("No Watermark Image")
                wr.Write(wr.NewLine)
                For Each dr As DataRow In dt.Rows
                    wr.Write(dr("ItemNumber").ToString())
                    wr.Write(",")
                    wr.Write(dr("ProductName").ToString().Replace(",", " "))
                    wr.Write(",")
                    wr.Write(dr("Make").ToString())
                    wr.Write(",")
                    wr.Write(dr("Model").ToString())
                    wr.Write(",")
                    Dim sDesc As String = HttpUtility.HtmlDecode(dr("Description").ToString())
                    If sDesc.Length > 0 Then
                        sDesc = sDesc.Replace(",", " ")
                    End If
                    wr.Write(sDesc.Trim)
                    wr.Write(",")
                    Dim sNotes As String = HttpUtility.HtmlDecode(dr("AdminNotes").ToString())
                    If sNotes.Length > 0 Then
                        sNotes = sNotes.Replace(",", " ")
                    End If
                    wr.Write(sDesc.Trim)
                    wr.Write(",")
                    wr.Write(dr("Condition").ToString())
                    wr.Write(",")
                    wr.Write(dr("Age").ToString())
                    wr.Write(",")
                    wr.Write(dr("Weight").ToString())
                    wr.Write(",")
                    wr.Write(dr("Price").ToString())
                    wr.Write(",")
                    wr.Write(dr("Qty").ToString())
                    wr.Write(",")
                    wr.Write(dr("QuantitySold").ToString())
                    wr.Write(",")
                    wr.Write(dr("LowestPrice").ToString())
                    wr.Write(",")
                    wr.Write(dr("CostofGoods").ToString())
                    wr.Write(",")
                    wr.Write(dr("AuctionStart").ToString())
                    wr.Write(",")
                    wr.Write(dr("SellingPrice").ToString())
                    wr.Write(",")
                    wr.Write(Create_Category(CInt(dr("Id").ToString())))
                    wr.Write(",")
                    wr.Write(dr("ParentCategoryName").ToString())
                    wr.Write(",")
                    wr.Write(dr("Location").ToString())
                    wr.Write(",")
                    wr.Write(dr("Barcode").ToString())
                    wr.Write(",")
                    wr.Write(dr("BarcodeParent").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsHold").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsSold").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsSpecial").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsNewArrivalsPage").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsFeaturedItem").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsJustOfftheTruck").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsConsignmentItem").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsDeleteItem").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsLabX").ToString())
                    wr.Write(",")
                    wr.Write(dr("ImageFileName").ToString())
                    wr.Write(",")
                    wr.Write(Create_Images(CInt(dr("Id").ToString())))
                    wr.Write(",")
                    wr.Write(dr("Watermarkimg").ToString())
                    wr.Write(wr.NewLine)
                Next
                wr.Close()
                Session("ApplicationFile") = filePath
                Response.Redirect("../Files/Product/" & fileName)
            Else
                DisplayAlert("You Have Nothing to Export")
            End If
        Catch ex As Exception
            DisplayAlert("Operation Not Proceed")
        End Try
    End Sub
    Private Function Create_Category(ByVal nProductId As Integer) As String
        Try
            'create the Models
            Dim sSQL As String = " SELECT C.CategoryName  FROM ProductCategoryCrossRef pccr, Category c WHERE pccr.ProductCategoryId = c.Id  and pccr.ProductId = " & nProductId
            Dim ds As DataSet = SQLData.generic_select(sSQL, SQLData.ConnectionString)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim sCategories As String = ""
                    For Each dr As DataRow In ds.Tables(0).Rows
                        Dim sCatName As String = String.Empty
                        If Not dr("CategoryName") Is Nothing Then
                            If Not String.IsNullOrEmpty(dr("CategoryName").ToString().Trim()) Then
                                sCatName = dr("CategoryName").ToString().Trim()
                                If sCatName.Length > 0 Then
                                    sCatName = sCatName.Trim
                                End If
                            End If
                        End If

                        If sCatName.Trim <> "" Then
                            If sCategories.Trim <> "" Then
                                sCategories &= ";"
                            End If
                            sCategories &= sCatName.Trim
                        End If
                    Next
                    Return sCategories
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        Catch ex As Exception

        End Try

        Return ""

    End Function
    Private Function Create_Images(ByVal nProductId As Integer) As String
        Try
            'create the Models
            Dim sSQL As String = " SELECT picr.ImageUrl FROM ProductImageCrossRef picr  WHERE picr.ProductId  =  " & nProductId
            Dim ds As DataSet = SQLData.generic_select(sSQL, SQLData.ConnectionString)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim sImages As String = ""
                    For Each dr As DataRow In ds.Tables(0).Rows
                        Dim sImageName As String = String.Empty
                        If Not dr("ImageUrl") Is Nothing Then
                            If Not String.IsNullOrEmpty(dr("ImageUrl").ToString().Trim()) Then
                                sImageName = dr("ImageUrl").ToString().Trim()
                                If sImageName.Length > 0 Then
                                    sImageName = sImageName.Trim
                                End If
                            End If
                        End If

                        If sImageName.Trim <> "" Then
                            If sImages.Trim <> "" Then
                                sImages &= ";"
                            End If
                            sImages &= sImageName.Trim
                        End If
                    Next
                    Return sImages
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        Catch ex As Exception

        End Try

        Return ""

    End Function
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            GenerateFile()
        Catch ex As Exception

        End Try
    End Sub
#End Region
End Class
