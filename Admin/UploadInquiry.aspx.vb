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
                Dim sMessage As String = icnt & " Inquiry(s) Processed Out Of " & itotcnt & " With " & ierr & " Error(s)"
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

          
            Dim aSkips As ArrayList = Me.aSkipFields

            Dim iProdCategoryId As New ArrayList

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

                                If CStr(aHeader(x).Trim).ToUpper = "NAME" Then
                                    sbFields.Append("[NAME]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "EMAIL" Then
                                    sbFields.Append("[EMAIL]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "COMPANY" Then
                                    sbFields.Append("[COMPANY]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PHONE" Then
                                    sbFields.Append("[PHONE]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "FAX" Then
                                    sbFields.Append("[FAX]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "ADDRESS1" Then
                                    sbFields.Append("[ADDRESS]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "ADDRESS2" Then
                                    sbFields.Append("[ADDRESS1]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "CITY" Then
                                    sbFields.Append("[CITY]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "STATE" Then
                                    sbFields.Append("[STATE]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "ZIP" Then
                                    sbFields.Append("[ZIP]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "COUNTRY" Then
                                    sbFields.Append("[COUNTRY]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "MESSAGE" Then
                                    sbFields.Append("[MESSAGE]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "INQUIRYTYPE" Then
                                    sbFields.Append("[TYPE]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "CARDTYPE" Then
                                    sbFields.Append("[CARDTYPE]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PONUMBER" Then
                                    sbFields.Append("[PONUMBER]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "ITEMNO" Then
                                    sbFields.Append("[ITEMNUMBER]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "DATE" Then
                                    sbFields.Append("[Orderdate]")
                                    sbValues.Append("'" & CDate(aLine(x).ToString().Trim) & "'")
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

        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    Private Function aSkipFields() As ArrayList
        Dim a As New ArrayList
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
