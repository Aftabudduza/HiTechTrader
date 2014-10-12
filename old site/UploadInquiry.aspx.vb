Imports System.IO
Imports System.Data
Partial Class Admin_UploadInquiry
    Inherits System.Web.UI.Page
    Public ds As DataSet = New DataSet()
    Dim dt As New DataTable
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
#Region "Inquiry Inload Methods"
    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        'inload the file
        Try
            If Me.uplProduct.FileName <> "" Then
                'read the file in
                Dim filePath As String = Path.Combine(Request.PhysicalApplicationPath, "Files\Inquiry\")

                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If

                Dim nFile As String = Path.Combine(filePath, "INLOAD_Inquiry_" & DateTime.Now.ToString("yyyyMMddhhmmss") & ".txt")

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
                Dim sFailedProducts As String = String.Empty
                Dim sProducts As String = String.Empty

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
                            If Not Session("ProdID") Is Nothing Then
                                If sProducts.Length <= 0 Then
                                    sProducts = Session("ProdID").ToString()
                                Else
                                    sProducts &= "," & Session("ProdID").ToString()
                                End If
                            End If
                        Else
                            ierr += 1
                            If Not Session("ProdID") Is Nothing Then
                                If sFailedProducts.Length <= 0 Then
                                    sFailedProducts = Session("ProdID").ToString()
                                Else
                                    sFailedProducts &= "," & Session("ProdID").ToString()
                                End If
                            End If
                        End If
                        itotcnt += 1
                        'Loop
                    Next
                Catch ex As Exception
                Finally
                    'sr.Close()
                End Try
                Dim sMessage As String = icnt & " Inquiry(s) Processed Out Of " & itotcnt & " With " & ierr & " Error(s)"
                lblMsg.Text = sMessage
                Try
                    If sProducts.Length > 0 Then
                        appGlobal.LogDataError("Successful In loading Inquiries:" & System.Environment.NewLine & sProducts.ToString())
                    End If
                Catch ex As Exception

                End Try

                Try
                    If sFailedProducts.Length > 0 Then
                        appGlobal.LogDataError("Error In loading Inquiries:" & System.Environment.NewLine & sFailedProducts.ToString())
                    End If
                Catch ex As Exception

                End Try
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
            Dim sProdId As String = ""
            Dim sId As String = ""
          
            Dim aSkips As ArrayList = Me.aSkipFields

            Dim iProdCategoryId As New ArrayList

            For i As Integer = 0 To aHeader.Count - 1
                If CStr(aHeader(i)).ToUpper = "ID" Then
                    If aLine(i) <> "" And aLine(i).ToString.Trim.ToUpper <> "NULL" Then
                        If CInt(aLine(i).ToString.Trim) > 0 Then
                            sId = aLine(i).ToString.Trim
                        Else
                            sId = "0"
                        End If
                    Else
                        sId = "0"
                    End If
                End If
            Next

            Session("ProdID") = sId

            If CInt(sId.Trim) <= 0 Then
                lblMsg.Text = "No Product in Inload File"
                Return False
            End If

          
            ds = Nothing
            Dim sSQLTest As String = ""
            'test if in system

            sSQLTest = "SELECT Id FROM OrderHistory WHERE Id=" & CInt(sId.Trim)


            Try
                ds = SQLData.generic_select(sSQLTest, strConnection)
                If Not ds Is Nothing Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        sProdId = ds.Tables(0).Rows(0)(0).ToString()
                    End If
                End If
            Catch ex As Exception

            End Try

            If sProdId = "" Then
                Try
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
                        .Append("INSERT INTO OrderHistory (")
                        For x As Integer = 0 To aHeader.Count - 1
                            If aHeader(x).Trim.ToString.Trim <> "" Then
                                If Not Actions.inArray(aSkips, CStr(aHeader(x).Trim).ToUpper) Then
                                    If Not bFirst Then
                                        sbFields.Append(",")
                                        sbValues.Append(",")
                                    Else
                                        bFirst = False
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "ID" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            If CInt(aLine(x)) > 0 Then
                                                aLine(x) = CInt(aLine(x))
                                            Else
                                                aLine(x) = 0
                                            End If
                                        Else
                                            aLine(x) = 0
                                        End If

                                        sbFields.Append("[Id]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "NAME" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[NAME]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "EMAIL" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[EMAIL]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "COMPANY" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[COMPANY]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "PHONE" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[PHONE]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "FAX" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[FAX]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If

                                    If CStr(aHeader(x).Trim).ToUpper = "ADDRESS1" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[ADDRESS]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "ADDRESS2" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[ADDRESS1]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "CITY" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[CITY]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If

                                    If CStr(aHeader(x).Trim).ToUpper = "STATE" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[STATE]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If

                                    If CStr(aHeader(x).Trim).ToUpper = "ZIP" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[ZIP]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "COUNTRY" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[COUNTRY]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "MESSAGE" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[MESSAGE]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "INQUIRYTYPE" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[TYPE]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "PAYMENTTYPE" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[CARDTYPE]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "PAYMENT" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[PONUMBER]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "LISTINGNO" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            aLine(x) = aLine(x).ToString.Trim
                                        Else
                                            aLine(x) = ""
                                        End If
                                        sbFields.Append("[ITEMNUMBER]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "DATE" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            If IsDate(aLine(x).ToString().Trim) Then
                                                aLine(x) = CDate(aLine(x).ToString().Trim)
                                            Else
                                                aLine(x) = CDate(DateTime.UtcNow.ToString())
                                            End If
                                        Else
                                            aLine(x) = CDate(DateTime.UtcNow.ToString())
                                        End If
                                        sbFields.Append("[Orderdate]")
                                        sbValues.Append("'" & CDate(aLine(x).ToString().Trim) & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "ELIST" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            If CDbl(aLine(x)) > 0 Then
                                                aLine(x) = CDbl(aLine(x))
                                            Else
                                                aLine(x) = 0
                                            End If
                                        Else
                                            aLine(x) = 0
                                        End If
                                        sbFields.Append("[IsAdd]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If
                                    If CStr(aHeader(x).Trim).ToUpper = "NOCONTACT" Then
                                        If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                            If CDbl(aLine(x)) > 0 Then
                                                aLine(x) = CDbl(aLine(x))
                                            Else
                                                aLine(x) = 0
                                            End If
                                        Else
                                            aLine(x) = 0
                                        End If
                                        sbFields.Append("[IsContact]")
                                        sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                    End If

                                End If
                            End If
                            'test for no string ''
                        Next

                        sb.Append(sbFields.ToString & ") VALUES (" & sbValues.ToString & ");")

                    End With
                    System.Diagnostics.Debug.WriteLine(sb.ToString)

                    If Not SQLData.generic_command(sb.ToString, SQLData.ConnectionString) Then
                        Return False
                    End If

                Catch ex As Exception

                End Try

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
                    .Append("UPDATE OrderHistory SET ")
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
                                If CStr(aHeader(x).Trim).ToUpper = "ID" Then
                                    If aLine(x) <> "" And CInt(aLine(x)) > 0 Then
                                        aLine(x) = CInt(aLine(x))
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[Id]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "NAME" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[NAME]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "EMAIL" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[EMAIL]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "COMPANY" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[COMPANY]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PHONE" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[PHONE]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "FAX" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[FAX]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "ADDRESS1" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[ADDRESS]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "ADDRESS2" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[ADDRESS1]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "CITY" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[CITY]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "STATE" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[STATE]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "ZIP" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[ZIP]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "COUNTRY" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[COUNTRY]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "MESSAGE" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[MESSAGE]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "INQUIRYTYPE" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[TYPE]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PAYMENTTYPE" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[CARDTYPE]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PAYMENT" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[PONUMBER]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "LISTINGNO" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[ITEMNUMBER]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "DATE" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        If IsDate(aLine(x).ToString().Trim) Then
                                            aLine(x) = CDate(aLine(x).ToString().Trim)
                                        Else
                                            aLine(x) = CDate(DateTime.UtcNow.ToString())
                                        End If
                                    Else
                                        aLine(x) = CDate(DateTime.UtcNow.ToString())
                                    End If
                                    .Append("[Orderdate]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "ELIST" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[IsAdd]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "NOCONTACT" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        If CDbl(aLine(x)) > 0 Then
                                            aLine(x) = CDbl(aLine(x))
                                        Else
                                            aLine(x) = 0
                                        End If
                                    Else
                                        aLine(x) = 0
                                    End If
                                    .Append("[IsContact]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If

                            End If

                        End If 'test for no value
                    Next

                    .Append(" WHERE Id=" & sProdId)
                End With
                System.Diagnostics.Debug.WriteLine(sb.ToString)
                System.Diagnostics.Debug.WriteLine(sbAdd.ToString)

                If Not SQLData.generic_command(sb.ToString, SQLData.ConnectionString) Then
                    Return False
                End If

            End If

            

        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    Private Function aSkipFields() As ArrayList
        Dim a As New ArrayList
        a.Add("STATUSID")
        Return a
    End Function
   
#End Region
#Region "Inquiry Upload Methods"
    Public Sub GenerateFile()
        Try

            Dim fileName As String = String.Empty
            Dim str As String = "SELECT p.*   FROM OrderHistory p ORDER BY p.Name asc "


            If str.Length > 0 Then
                dt = BRIClassLibrary.SQLData.generic_select(str, strConnection).Tables(0)
            End If

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim filePath = Path.Combine(Request.PhysicalApplicationPath, "Files\Inquiry\")
                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If

                fileName = "Inquiry_Export_" & DateTime.Now.ToString("yyyyMMddhhmmss") & ".csv"
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
                wr.Write("Name")
                wr.Write(",")
                wr.Write("Company")
                wr.Write(",")
                wr.Write("Email")
                wr.Write(",")
                wr.Write("Address")
                wr.Write(",")
                wr.Write("Address 1")
                wr.Write(",")
                wr.Write("City")
                wr.Write(",")
                wr.Write("State")
                wr.Write(",")
                wr.Write("Zip")
                wr.Write(",")
                wr.Write("Country")
                wr.Write(",")
                wr.Write("Phone")
                wr.Write(",")
                wr.Write("Fax")
                wr.Write(",")
                wr.Write("Message")
                wr.Write(",")
                wr.Write("Inquiry Type")
                wr.Write(",")
                wr.Write("Card Type")
                wr.Write(",")
                wr.Write("PO Number")
                wr.Write(",")
                wr.Write("Date")
                wr.Write(wr.NewLine)
                For Each dr As DataRow In dt.Rows
                    wr.Write(dr("ItemNumber").ToString())
                    wr.Write(",")
                    wr.Write(dr("Name").ToString())
                    wr.Write(",")
                    wr.Write(dr("Company").ToString())
                    wr.Write(",")
                    wr.Write(dr("Email").ToString())
                    wr.Write(",")
                    wr.Write(dr("Address").ToString())
                    wr.Write(",")
                    wr.Write(dr("Address1").ToString())
                    wr.Write(",")
                    wr.Write(dr("City").ToString())
                    wr.Write(",")
                    wr.Write(dr("State").ToString())
                    wr.Write(",")
                    wr.Write(dr("Zip").ToString())
                    wr.Write(",")
                    wr.Write(dr("Country").ToString())
                    wr.Write(",")
                    wr.Write(dr("Phone").ToString())
                    wr.Write(",")
                    wr.Write(dr("Fax").ToString())
                    wr.Write(",")
                    wr.Write(dr("Message").ToString())
                    wr.Write(",")
                    wr.Write(dr("CardType").ToString())
                    wr.Write(",")
                    wr.Write(dr("PONumber").ToString())
                    wr.Write(",")
                    wr.Write(dr("OrderDate").ToString())
                    wr.Write(wr.NewLine)
                Next
                wr.Close()
                Session("ApplicationFile") = filePath
                Response.Redirect("../Files/Inquiry/" & fileName)
            Else
                DisplayAlert("You Have Nothing to Export")
            End If
        Catch ex As Exception
            DisplayAlert("Operation Not Proceed")
        End Try
    End Sub
   
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
