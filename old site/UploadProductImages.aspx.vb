Imports System.IO
Imports System.Data
Partial Class Admin_UploadProductImages
    Inherits System.Web.UI.Page
    Public ds As DataSet = New DataSet()
    Dim dt As New DataTable
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
#Region "ProductImages Inload Methods"
    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        'inload the file
        Try
            If Me.uplProduct.FileName <> "" Then
                'read the file in
                Session("ProdID") = Nothing
                Dim filePath As String = Path.Combine(Request.PhysicalApplicationPath, "Files\Product\")

                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If

                Dim nFile As String = Path.Combine(filePath, "INLOAD_ProductImage_" & DateTime.Now.ToString("yyyyMMddhhmmss") & ".txt")

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

                Dim sMessage As String = icnt & " Product(s) Processed Out Of " & itotcnt & " With " & ierr & " Error(s)"
                lblMsg.Text = sMessage

                Try
                    If sProducts.Length > 0 Then
                        appGlobal.LogDataError("Successful In loading Products Images:" & System.Environment.NewLine & sProducts.ToString())
                    End If
                Catch ex As Exception

                End Try

                Try
                    If sFailedProducts.Length > 0 Then
                        appGlobal.LogDataError("Error In loading Products Images:" & System.Environment.NewLine & sFailedProducts.ToString())
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
            Dim sItemNumber As String = ""
            Dim sProductImage As String = ""
            Dim aSkips As ArrayList = Me.aSkipFields

            For i As Integer = 0 To aHeader.Count - 1
                If aHeader(i).Trim.ToString.Trim <> "" Then
                    If Not Actions.inArray(aSkips, CStr(aHeader(i).Trim).ToUpper) Then
                        If CStr(aHeader(i)).ToUpper = "LISTINGNO" Then
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
                    End If
                End If
            Next


            For i As Integer = 0 To aHeader.Count - 1
                If aHeader(i).Trim.ToString.Trim <> "" Then
                    If Not Actions.inArray(aSkips, CStr(aHeader(i).Trim).ToUpper) Then
                        If CStr(aHeader(i)).ToUpper = "PICNAME" Then
                            If aLine(i) <> "" And aLine(i).ToString.Trim.ToUpper <> "NULL" Then
                                sProductImage = aLine(i)
                            End If
                        End If
                    End If
                End If
            Next


            Session("ProdID") = sId

            If sProductImage.Trim.Length <= 0 Then
                lblMsg.Text = "No Product in Inload File"
                Return False
            End If

            If Not GetProductImageCrossRef(sProductImage.Trim, CInt(sId)) Then
                Return False
            Else
                Return True
            End If
          

        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    Private Function aSkipFields() As ArrayList
        Dim a As New ArrayList
        a.Add("ID")
        a.Add("WSM_ID")
        a.Add("ISDELETED")
        a.Add("PICCOUNT")
        Return a
    End Function
   
    Private Function GetProductImageCrossRef(ByVal sProductImage As String, ByVal nProductId As Integer) As Boolean
        Try
            Dim bSuccess As Boolean = True
            Dim sId As String = "0"
            Dim aImages As String() = sProductImage.Split(",")
            'clear old Category
            'add ALL new
            Try
                Dim sDel As String = "DELETE FROM ProductImageCrossRef WHERE ImageUrl = '" & sProductImage & "' and ProductId=" & nProductId
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
    
#End Region
#Region "Product Upload Methods"
    Public Sub GenerateFile()
        Try

            Dim fileName As String = String.Empty
            Dim str As String = "SELECT p.*  FROM ProductImageCrossRef p "


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
                wr.Write("ID")
                wr.Write(",")
                wr.Write("ListingNo")
                wr.Write(",")
                wr.Write("PicName")
                wr.Write(wr.NewLine)
                For Each dr As DataRow In dt.Rows
                    wr.Write(dr("ID").ToString())
                    wr.Write(",")
                    wr.Write(dr("ProductId").ToString())
                    wr.Write(",")
                    wr.Write(dr("ImageUrl").ToString())
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
