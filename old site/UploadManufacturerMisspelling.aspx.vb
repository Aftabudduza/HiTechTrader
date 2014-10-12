Imports System.IO
Imports System.Data
Partial Class Admin_UploadManufacturerMisspelling
    Inherits System.Web.UI.Page
    Public ds As DataSet = New DataSet()
    Dim dt As New DataTable
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
#Region "Manufacturer Misspelling Inload Methods"
    Protected Sub btnImportService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        'inload the file
        Try
            If Me.uplManufacturer.FileName <> "" Then
                'read the file in
                Dim filePath As String = Path.Combine(Request.PhysicalApplicationPath, "Files\Manufacturer\Misspelling\")

                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If

                Dim nFile As String = Path.Combine(filePath, "INLOAD_ManufacturerMisspelling_" & DateTime.Now.ToString("yyyyMMddhhmmss") & ".txt")

                Try
                    If System.IO.File.Exists(nFile) Then
                        System.IO.File.Delete(nFile)
                    End If
                Catch ex As Exception

                End Try
                uplManufacturer.SaveAs(nFile)

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
                Dim sMessage As String = icnt & " Manufacturer(s) Processed Out Of " & itotcnt & " With " & ierr & " Error(s)"
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
            Dim nManufacturerId As Integer = 0
            Dim sManufacturerName As String = ""
            Dim sManufacturerMisspelling As String = ""
            For i As Integer = 0 To aHeader.Count - 1
                If CStr(aHeader(i)).ToUpper = "MANUFACTURER" Then
                    sManufacturerName = aLine(i)
                End If
            Next
            If sManufacturerName.Length > 0 Then
                nManufacturerId = GetManufacturer(sManufacturerName)
            End If
            For i As Integer = 0 To aHeader.Count - 1
                If CStr(aHeader(i)).ToUpper = "MISSPELLING" Then
                    sManufacturerMisspelling = aLine(i).ToString().Trim
                End If
            Next
            If sManufacturerMisspelling = "" Then
                lblMsg.Text = "No Manufacturer in Inload File"
                Return False
            End If
            Dim sSQLTest As String = ""
            ds = Nothing
            'test if in system
            If nManufacturerId > 0 Then
                sSQLTest = "SELECT Id FROM ManufacturerMisspelling WHERE Misspelling='" & sManufacturerMisspelling.Trim & "' and ManufacturerId = " & nManufacturerId
            Else
                sSQLTest = "SELECT Id FROM ManufacturerMisspelling WHERE Misspelling='" & sManufacturerMisspelling.Trim & "'"
            End If

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
            Dim sUpsells As String = ""
            Dim sLocations As String = ""
            Dim sGroupItems As String = ""
            Dim sAccessoryItems As String = ""
            Dim sModels As String = ""
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
                    .Append("INSERT INTO ManufacturerMisspelling (")
                    For x As Integer = 0 To aHeader.Count - 1
                        If aHeader(x).Trim.ToString.Trim <> "" Then
                            If Not Actions.inArray(aSkips, CStr(aHeader(x).Trim).ToUpper) Then
                                If Not bFirst Then
                                    sbFields.Append(",")
                                    sbValues.Append(",")
                                Else
                                    bFirst = False
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "MANUFACTURER" Then
                                    sbFields.Append("[ManufacturerId]")
                                    sbValues.Append(nManufacturerId)
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "MISSPELLING" Then
                                    sbFields.Append("[Misspelling]")
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
                    .Append("UPDATE ManufacturerMisspelling SET ")
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
                                If CStr(aHeader(x).Trim).ToUpper = "MANUFACTURER" Then
                                    .Append("[ManufacturerId]=" & nManufacturerId)
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "MISSPELLING" Then
                                    .Append("[Misspelling]=" & "'" & CStr(aLine(x)).Trim & "'")
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
        a.Add("ID")
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
#End Region

#Region "Manufacturer Misspelling Upload Methods"
    Public Sub GenerateFile()
        Try

            Dim fileName As String = String.Empty
            Dim str As String = "SELECT m.[Name], ma.Misspelling   FROM Manufacturer m, ManufacturerMisspelling ma WHERE m.Id = ma.ManufacturerId ORDER BY m.[Name] asc"


            If str.Length > 0 Then
                dt = BRIClassLibrary.SQLData.generic_select(str, strConnection).Tables(0)
            End If

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim filePath = Path.Combine(Request.PhysicalApplicationPath, "Files\Manufacturer\Misspelling\")
                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If

                fileName = "Manufacturer_Export_" & DateTime.Now.ToString("yyyyMMddhhmmss") & ".csv"
                filePath = Path.Combine(filePath, fileName)
                Try
                    If System.IO.File.Exists(filePath) Then
                        System.IO.File.Delete(filePath)
                    End If
                Catch ex As Exception

                End Try

                Dim wr = New StreamWriter(filePath)
                wr.Write("Manufacturer")
                wr.Write(",")
                wr.Write("Misspelling")
                wr.Write(wr.NewLine)
                For Each dr As DataRow In dt.Rows
                    wr.Write(dr("Name").ToString())
                    wr.Write(",")
                    wr.Write(dr("Misspelling").ToString())
                    wr.Write(wr.NewLine)
                Next
                wr.Close()
                Session("ApplicationFile") = filePath
                Response.Redirect("../Files/Manufacturer/Misspelling/" & fileName)
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
