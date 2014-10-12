Imports System.IO
Imports System.Data
Partial Class Admin_UploadCategory
    Inherits System.Web.UI.Page
    Public ds As DataSet = New DataSet()
    Dim dt As New DataTable
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
#Region "Category Inload Methods"
    Protected Sub btnImportService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        'inload the file
        Try
            If Me.uplCategory.FileName <> "" Then
                'read the file in
                Dim filePath As String = Path.Combine(Request.PhysicalApplicationPath, "Files\Category\")

                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If

                Dim nFile As String = Path.Combine(filePath, "INLOAD_Category_" & DateTime.Now.ToString("yyyyMMddhhmmss") & ".txt")

                Try
                    If System.IO.File.Exists(nFile) Then
                        System.IO.File.Delete(nFile)
                    End If
                Catch ex As Exception

                End Try
                uplCategory.SaveAs(nFile)

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
                Dim sMessage As String = icnt & " Categories(s) Processed Out Of " & itotcnt & " With " & ierr & " Error(s)"
                lblMsg.Text = sMessage
                Try
                    If sProducts.Length > 0 Then
                        appGlobal.LogDataError("Successful In loading Categories:" & System.Environment.NewLine & sProducts.ToString())
                    End If
                Catch ex As Exception

                End Try

                Try
                    If sFailedProducts.Length > 0 Then
                        appGlobal.LogDataError("Error In loading Categories:" & System.Environment.NewLine & sFailedProducts.ToString())
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

            Dim sName As String = ""
            Dim sParent As String = ""
            Dim nId As Integer = 0
            Dim nParent As Integer = 0

            For i As Integer = 0 To aHeader.Count - 1
                If CStr(aHeader(i)).ToUpper = "ID" Then
                    If aLine(i) <> "" And aLine(i).ToString.Trim.ToUpper <> "NULL" Then
                        If CInt(aLine(i).ToString.Trim) > 0 Then
                            nId = CInt(aLine(i).ToString().Trim)
                        Else
                            nId = 0
                        End If
                    Else
                        nId = 0
                    End If

                End If
            Next

            For i As Integer = 0 To aHeader.Count - 1
                If CStr(aHeader(i)).ToUpper = "PARENT" Then
                    If aLine(i) <> "" And aLine(i).ToString.Trim.ToUpper <> "NULL" Then
                        If CInt(aLine(i).ToString.Trim) > 0 Then
                            nParent = CInt(aLine(i).ToString().Trim)
                        Else
                            nParent = 0
                        End If
                    Else
                        nParent = 0
                    End If
                End If
            Next
            Session("ProdID") = nId

            If nId <= 0 Then
                lblMsg.Text = "No Category in Inload File"
                Return False
            End If
           
            ds = Nothing
            Dim sSQLTest As String = ""
            'test if in system

            sSQLTest = "SELECT Id FROM Category WHERE Id='" & nId & "' and CategoryParentId = " & nParent

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
                    .Append("INSERT INTO Category (")
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
                                    If aLine(x) <> "" And CInt(aLine(x)) > 0 Then
                                        aLine(x) = CInt(aLine(x))
                                    Else
                                        aLine(x) = 0
                                    End If
                                    sbFields.Append("[Id]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "NAME" Then
                                    sbFields.Append("[CategoryName]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PARENT" Then
                                    sbFields.Append("[CategoryParentId]")
                                    sbValues.Append("'" & nParent & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "METADESC" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    sbFields.Append("[MetaDescription]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "METAKEYWORDS" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    sbFields.Append("[MetaKeywords]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "LEVEL" Then
                                    If CDbl(aLine(x)) > 0 Then
                                        aLine(x) = CDbl(aLine(x))
                                    Else
                                        aLine(x) = 0
                                    End If
                                    'fill in the data
                                    sbFields.Append("[Catlevel]")
                                    sbValues.Append("'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "ISACTIVE" Then
                                    If aLine(x) <> "" And CDbl(aLine(x)) > 0 Then
                                        aLine(x) = CDbl(aLine(x))
                                    Else
                                        aLine(x) = 0
                                    End If
                                    'fill in the data
                                    sbFields.Append("[IsActive]")
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
                    .Append("UPDATE Category SET ")
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
                                    .Append("[CategoryName]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "PARENT" Then
                                    .Append("[CategoryParentId]=" & "'" & nParent & "'")
                                End If


                                If CStr(aHeader(x).Trim).ToUpper = "LEVEL" Then
                                    If CDbl(aLine(x)) > 0 Then
                                        aLine(x) = CDbl(aLine(x))
                                    Else
                                        aLine(x) = 0
                                    End If
                                    'fill in the data
                                    .Append("[Catlevel]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If

                                If CStr(aHeader(x).Trim).ToUpper = "ISACTIVE" Then
                                    If aLine(x) <> "" And CDbl(aLine(x)) > 0 Then
                                        aLine(x) = CDbl(aLine(x))
                                    Else
                                        aLine(x) = 0
                                    End If
                                    'fill in the data
                                    .Append("[IsActive]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "METADESC" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[MetaDescription]=" & "'" & CStr(aLine(x)).Trim & "'")
                                End If
                                If CStr(aHeader(x).Trim).ToUpper = "METAKEYWORDS" Then
                                    If aLine(x) <> "" And aLine(x).ToString.Trim.ToUpper <> "NULL" Then
                                        aLine(x) = aLine(x).ToString.Trim
                                    Else
                                        aLine(x) = ""
                                    End If
                                    .Append("[MetaKeywords]=" & "'" & CStr(aLine(x)).Trim & "'")
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
        a.Add("CATEGORYNO")
        a.Add("LINEAGE")
        a.Add("UCATEGORYNAME")
        a.Add("NOCHILDREN")
        Return a
    End Function
    Private Function GetParent(ByVal sName As String) As Integer
        'test for the ManuFacturer name
        Dim sSQL As String = "select id from Category where CategoryName='" & sName.Trim & "'"
        Dim sId As String = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        If sId = "" Then
            'add it
            Dim sIns As String = "INSERT INTO Category ([CategoryName]) VALUES('" & sName.Trim & "')"
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
#Region "Category Upload Methods"
    Public Sub GenerateFile()
        Try

            Dim fileName As String = String.Empty
            Dim str As String = "SELECT A.*, ISNULL(B.Total,0) NoChildren FROM (SELECT c.* FROM Category c WHERE c.IsLabX = 0 OR c.IsLabX IS NULL) A LEFT JOIN (SELECT DISTINCT CategoryParentId, ISNULL(COUNT(Id),0) Total FROM category WHERE CategoryParentId >0 AND  IsLabX = 0 OR IsLabX IS NULL GROUP BY CategoryParentId) B ON A.Id = B.CategoryParentId ORDER BY A.Id asc "


            If str.Length > 0 Then
                dt = BRIClassLibrary.SQLData.generic_select(str, strConnection).Tables(0)
            End If

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim filePath = Path.Combine(Request.PhysicalApplicationPath, "Files\Category\")
                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If

                fileName = "Category_Export_" & DateTime.Now.ToString("yyyyMMddhhmmss") & ".csv"
                filePath = Path.Combine(filePath, fileName)
                Try
                    If System.IO.File.Exists(filePath) Then
                        System.IO.File.Delete(filePath)
                    End If
                Catch ex As Exception

                End Try

                Dim wr = New StreamWriter(filePath)
                wr.Write("Id")
                wr.Write(",")
                wr.Write("Parent")
                wr.Write(",")
                wr.Write("Name")
                wr.Write(",")
                wr.Write("Lineage")
                wr.Write(",")
                wr.Write("Level")
                wr.Write(",")
                wr.Write("IsActive")
                wr.Write(",")
                wr.Write("Meta Desc")
                wr.Write(",")
                wr.Write("Meta Keywords")
                wr.Write(wr.NewLine)
                For Each dr As DataRow In dt.Rows
                    wr.Write(dr("Id").ToString())
                    wr.Write(",")
                    wr.Write(dr("CategoryParentId").ToString())
                    wr.Write(",")
                    wr.Write(dr("CategoryName").ToString())
                    wr.Write(",")
                    If Not dr("CategoryParentId").ToString() Is Nothing And CInt(dr("CategoryParentId").ToString()) > 0 Then
                        wr.Write("/" & dr("CategoryParentId").ToString() & "/" & dr("Id").ToString() & "/")
                    Else
                        wr.Write("/" & dr("Id").ToString() & "/")
                    End If
                    wr.Write(",")
                    wr.Write(dr("Catlevel").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsActive").ToString())
                    wr.Write(",")
                    wr.Write(dr("MetaDescription").ToString())
                    wr.Write(",")
                    wr.Write(dr("MetaKeywords").ToString())
                    wr.Write(wr.NewLine)
                Next
                wr.Close()
                Session("ApplicationFile") = filePath
                Response.Redirect("../Files/Category/" & fileName)
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
