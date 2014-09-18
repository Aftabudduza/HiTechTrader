Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_AddNewManualItem
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Dim modifyCategory As Integer = 0
    Dim DeleteCategory As Integer = 0
    Dim sSQl As String = ""
    Dim ProductId As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IsSuperAdmin") = True Then
            If Not Session("Id") Is Nothing Then
                If Not Page.IsPostBack Then
                    Session("ProductId") = Nothing
                    Session("imgurl") = Nothing
                    Session("ItemNo") = Nothing
                    Session("Title") = Nothing
                    fill_ParentCategory()
                    GetItemNo()
                    Try
                        ProductId = CInt(Request.QueryString("PID").ToString())
                    Catch ex As Exception
                        ProductId = 0
                    End Try
                    If ProductId > 0 Then
                        Session("ProductId") = ProductId
                    End If
                    If Not Session("ProductId") Is Nothing Then
                        fill_Controls(CInt(Session("ProductId").ToString()))
                        btnAddItem.Text = "Update Manual Item"
                        FunctionTitle.InnerHtml = "Edit A Manual Record"
                        spanEdit.InnerHtml = ""
                        spanAddFile.InnerHtml = "<br /><a href='ManualProductImage.aspx?ProductId=" & CInt(Session("ProductId").ToString()) & "'>Add/Edit</a> &nbsp; &nbsp;&nbsp; the manual's pdf files."
                        spanUploadedFile.InnerHtml = getFiles(CInt(Session("ProductId").ToString()))

                    End If

                End If
            Else
                Response.Redirect("Login.aspx")
            End If
        Else
            Response.Redirect("Login.aspx")
        End If

    End Sub
    Private Sub fill_ParentCategory()
        Try
            Dim sSQl As String = "SELECT * FROM Category c WHERE c.CategoryParentId =0 AND c.IsMainorLabX = 0"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlCategory.Items.Clear()
            ddlCategory.AppendDataBoundItems = True
            ddlCategory.Items.Add(New ListItem("-------------  Select Parent Category  -------------", "-1"))
            For Each dr As DataRow In ds.Tables(0).Rows
                If CInt(dr("Id").ToString()) > 0 Then
                    Dim str As String = "SELECT * FROM Category c WHERE c.CategoryParentId =" & CInt(dr("Id").ToString())
                    Dim ds2 As DataSet = BRIClassLibrary.SQLData.generic_select(str, appGlobal.CONNECTIONSTRING)
                    Me.ddlCategory.Items.Add(New ListItem(dr("CategoryName"), dr("Id")))
                    If Not ds2 Is Nothing AndAlso ds2.Tables(0).Rows.Count > 0 Then
                        For Each dr2 As DataRow In ds2.Tables(0).Rows
                            If CInt(dr2("Id").ToString()) > 0 Then
                                Dim str3 As String = "SELECT * FROM Category c WHERE c.CategoryParentId =" & CInt(dr2("Id").ToString())
                                Dim ds3 As DataSet = BRIClassLibrary.SQLData.generic_select(str3, appGlobal.CONNECTIONSTRING)
                                Me.ddlCategory.Items.Add(New ListItem(" -->" & dr2("CategoryName"), dr2("Id")))
                                If Not ds3 Is Nothing AndAlso ds3.Tables(0).Rows.Count > 0 Then
                                    For Each dr3 As DataRow In ds3.Tables(0).Rows
                                        Me.ddlCategory.Items.Add(New ListItem(" " & " ---->" & dr3("CategoryName"), dr3("Id")))
                                    Next

                                End If
                            End If
                        Next
                    End If
                End If
            Next
            ddlCategory.SelectedValue = "-1"
        Catch ex As Exception

        End Try

    End Sub
    Public Function Setdata(ByVal Obj As ManualProduct) As ManualProduct
        Try
            If Not String.IsNullOrEmpty(txtMake.Text.ToString().Trim) AndAlso Not txtMake.Text.ToString().Trim = "" Then
                Dim nManufacturerID As Integer = GetManufacturer(txtMake.Text.ToString().Trim)
                Obj.ManufacturerId = nManufacturerID
                Obj.Make = txtMake.Text.ToString().Trim
            Else
                Obj.ManufacturerId = 0
                Obj.Make = ""
            End If
            If Not String.IsNullOrEmpty(txtModel.Text.ToString().Trim) AndAlso Not txtModel.Text.ToString().Trim = "" Then
                Obj.Model = txtModel.Text.ToString().Trim
            Else
                Obj.Model = ""
            End If
            If Not String.IsNullOrEmpty(txtTittle.Text.ToString().Trim) AndAlso Not txtTittle.Text.ToString().Trim = "" Then
                Obj.ProductName = txtTittle.Text.ToString().Trim
            Else
                Obj.ProductName = ""
            End If
            If Not String.IsNullOrEmpty(txtItemNo.Text.ToString().Trim) AndAlso Not txtItemNo.Text.ToString().Trim = "" Then
                Obj.ItemNumber = txtItemNo.Text.ToString().Trim
            Else
                Obj.ItemNumber = ""
            End If

            Obj.Barcode = ""
            Obj.BarcodeParent = ""
            Obj.AdminNotes = ""
            Obj.Price = 0.0
            Obj.LowestPrice = 0.0
            Obj.AuctionStart = 0.0
            Obj.CostofGoods = 0.0
            Obj.Qty = 1
            Obj.TotalPieces = 1
            Obj.Condition = ""
            Obj.Age = ""
            Obj.Weight = 0.0
            Obj.SellingPrice = 0.0
            Obj.QuantitySold = 0

            If Not String.IsNullOrEmpty(txtLocation.Text.ToString().Trim) Then
                Obj.Location = txtLocation.Text.ToString().Trim
            Else
                Obj.Location = ""
            End If


            If Not String.IsNullOrEmpty(txtDescription.Text.ToString().Trim) AndAlso Not txtDescription.Text.ToString().Trim = "" Then
                Obj.Description = txtDescription.Text.ToString().Trim
            Else
                Obj.Description = ""
            End If

            If Not String.IsNullOrEmpty(txtManualItemNo.Text.ToString().Trim) AndAlso Not txtManualItemNo.Text.ToString().Trim = "" Then
                Obj.ManualItemNo = txtManualItemNo.Text.ToString().Trim
            Else
                Obj.ManualItemNo = ""
            End If

            If Not String.IsNullOrEmpty(txtLink.Text.ToString().Trim) AndAlso Not txtLink.Text.ToString().Trim = "" Then
                Obj.Link = txtLink.Text.ToString().Trim
            Else
                Obj.Link = ""
            End If

            If ddlHistory.SelectedValue <> "-1" Then
                Obj.History = ddlHistory.SelectedValue.ToString()
            Else
                Obj.History = "-1"
            End If

            If ddlCategory.SelectedValue <> "-1" Then
                Obj.Category = CInt(ddlCategory.SelectedValue.ToString())
                Obj.ParentCategory = GetProductCategoryParent(CInt(ddlCategory.SelectedValue.ToString()))
            Else
                Obj.ParentCategory = 0
                Obj.Category = "-1"
            End If

            Obj.IsNotOnWeb = 0
            Obj.IsDoNotRelease = 0
            Obj.IsFeaturedItem = 0
            Obj.IsNewArrivalsPage = 0
            Obj.IsConsignmentItem = 0
            Obj.IsJustOfftheTruck = 0
            Obj.IsLabX = 0
            Obj.IsSold = 0
            Obj.VideoURL = ""
            Obj.IsIncludeinNewsletter = 0
            Obj.IsDeletePermanently = 0
            Obj.IsPaid = 0
            Obj.IsShipped = 0
            Obj.IsCompleted = 0
            Obj.ImageFileName = ""

            If chkHold.Checked = True Then
                Obj.IsHold = 1
            Else
                Obj.IsHold = 0
            End If
            If chkDelete.Checked = True Then
                Obj.IsDeleteItem = 1
            Else
                Obj.IsDeleteItem = 0
            End If

            If Not Session("ProductId") Is Nothing Then
                Obj.LastEdited = DateTime.UtcNow
                Obj.EditorID = CInt(Session("Id").ToString())
            Else
                Obj.CreatorID = CInt(Session("Id").ToString())
                Obj.DateCreated = DateTime.UtcNow
            End If
            
        Catch ex As Exception

        End Try

        Return Obj
    End Function
    Public Function Validate_Control() As String
        If Not String.IsNullOrEmpty(txtMake.Text.ToString().Trim) AndAlso Len(txtMake.ToString().Trim) <= 0 Then
            errStr &= "Please enter Make Name" & Environment.NewLine
        End If
        Return errStr
    End Function
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Protected Sub btnAddItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddItem.Click
        Dim checkInsert As Integer = 0
        errStr = String.Empty
        errStr = Validate_Control()
        If errStr.Length <= 0 Then
            Dim Obj As New ManualProduct(appGlobal.CONNECTIONSTRING)
            Obj = Setdata(Obj)
            Try
                If Not Session("ProductId") Is Nothing Then
                    Obj.Id = CInt(Session("ProductId").ToString())
                    If Obj.Update() Then
                        DisplayAlert("Your Item is successfully Updated !!!")
                    Else
                        DisplayAlert("Your Item is not Updated!")
                    End If
                Else
                    checkInsert = Obj.InsertIntoProduct()
                    If checkInsert > 0 Then
                        Session("ProductId") = checkInsert
                        DisplayAlert("Your Item is successfully created !!!")
                    Else
                        DisplayAlert("Item Insert Failed !!!")
                    End If
                End If
            Catch ex As Exception
            End Try
        Else
            DisplayAlert(errStr)
        End If
    End Sub
    Public Sub Clear_Controls()
        txtMake.Text = ""
        txtModel.Text = ""
        txtTittle.Text = ""
        txtItemNo.Text = ""
        txtLocation.Text = ""
        txtDescription.Text = ""
        txtManualItemNo.Text = ""
        ddlCategory.SelectedValue = "-1"
        chkDelete.Checked = False
        chkHold.Checked = False
        Session("ProductId") = Nothing
        headeritemright.Visible = False
        txtLink.Text = ""
    End Sub
    Private Sub fill_Controls(ByVal nProductID As Integer)
        If nProductID > 0 Then
            Try
                Dim sql As String = ""
                Dim ds As DataSet = Nothing
                sql = "SELECT * FROM ManualProduct p WHERE p.Id = " & nProductID
                If sql.Length > 0 Then
                    ds = BRIClassLibrary.SQLData.generic_select(sql, appGlobal.CONNECTIONSTRING)
                    If Not ds Is Nothing Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If Not ds.Tables(0).Rows(0)("Id") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("Id").ToString()) > 0 Then
                                Session("ProductId") = CInt(ds.Tables(0).Rows(0)("Id").ToString())
                            End If
                            If Not ds.Tables(0).Rows(0)("Make") Is Nothing AndAlso ds.Tables(0).Rows(0)("Make") <> "" Then
                                txtMake.Text = ds.Tables(0).Rows(0)("Make").ToString()
                            End If
                            If Not ds.Tables(0).Rows(0)("Model") Is Nothing AndAlso ds.Tables(0).Rows(0)("Model") <> "" Then
                                txtModel.Text = ds.Tables(0).Rows(0)("Model").ToString()
                            End If
                            If Not ds.Tables(0).Rows(0)("ProductName") Is Nothing AndAlso ds.Tables(0).Rows(0)("ProductName").ToString() <> "" Then
                                txtTittle.Text = ds.Tables(0).Rows(0)("ProductName").ToString()
                                Session("Title") = ds.Tables(0).Rows(0)("ProductName").ToString()
                            End If

                            If Not ds.Tables(0).Rows(0)("ItemNumber").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("ItemNumber").ToString() <> "" Then
                                txtItemNo.Text = ds.Tables(0).Rows(0)("ItemNumber").ToString()
                                Session("ItemNo") = ds.Tables(0).Rows(0)("ItemNumber").ToString()
                            End If

                            If Not ds.Tables(0).Rows(0)("Location") Is Nothing AndAlso ds.Tables(0).Rows(0)("Location").ToString() <> "" Then
                                txtLocation.Text = ds.Tables(0).Rows(0)("Location").ToString()
                            End If

                            If Not ds.Tables(0).Rows(0)("Description") Is Nothing AndAlso ds.Tables(0).Rows(0)("Description").ToString() <> "" Then
                                txtDescription.Text = HttpUtility.HtmlDecode(ds.Tables(0).Rows(0)("Description").ToString())
                            End If

                            If Not ds.Tables(0).Rows(0)("ManualItemNo") Is Nothing AndAlso ds.Tables(0).Rows(0)("ManualItemNo").ToString() <> "" Then
                                txtManualItemNo.Text = ds.Tables(0).Rows(0)("ManualItemNo").ToString()
                                '  hederitemright.InnerText = "Manual Item No :" & ds.Tables(0).Rows(0)("ManualItemNo").ToString()
                                '  hederitemright.Style.Add("background-color", "#ff33ff")
                                '  hederitemright.Style.Add("color", "#fff")
                                '  hederitemright.Style.Add("padding", "2px")
                            End If

                           


                            If Not ds.Tables(0).Rows(0)("IsHold") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsHold").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsHold")) > 0 Then
                                    chkHold.Checked = True
                                Else
                                    chkHold.Checked = False
                                End If
                            Else
                                chkHold.Checked = False
                            End If

                            If Not ds.Tables(0).Rows(0)("IsDeleteItem") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsDeleteItem").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsDeleteItem")) > 0 Then
                                    chkDelete.Checked = True
                                Else
                                    chkDelete.Checked = False
                                End If
                            Else
                                chkDelete.Checked = False
                            End If

                            Try
                                If Not ds.Tables(0).Rows(0)("LastEdited") Is Nothing AndAlso CDate(ds.Tables(0).Rows(0)("LastEdited")).ToString("MM/dd/yyyy") <> "12/31/1977" Then
                                    headeritemright.InnerHtml = "Entered :" & CDate(ds.Tables(0).Rows(0)("LastEdited")).ToString("yyyy-MM-dd hh:mm")
                                Else
                                    headeritemright.InnerHtml = "Entered :" & DateTime.UtcNow
                                End If
                            Catch ex As Exception
                                headeritemright.InnerHtml = "Entered :" & DateTime.UtcNow
                            End Try
                            '  Hits.InnerHtml = "Hits : 0"
                            FunctionTitle.InnerHtml = "Edit A Manual Record"
                            spanEdit.InnerHtml = ""
                            spanAddFile.InnerHtml = "<a style='font-weight:bold;' href='ManualProductImage.aspx?ProductId=" & CInt(Session("ProductId").ToString()) & "'>Add/Edit</a> the manual's pdf files."

                            If Not ds.Tables(0).Rows(0)("History") Is Nothing AndAlso ds.Tables(0).Rows(0)("History") <> "" Then
                                If ds.Tables(0).Rows(0)("History").ToString() <> "-1" Then
                                    ddlHistory.SelectedValue = ds.Tables(0).Rows(0)("History").ToString()
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("Link") Is Nothing AndAlso ds.Tables(0).Rows(0)("Link").ToString() <> "" Then
                                txtLink.Text = ds.Tables(0).Rows(0)("Link").ToString()
                            End If

                            If Not ds.Tables(0).Rows(0)("Category") Is Nothing AndAlso ds.Tables(0).Rows(0)("Category").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("Category").ToString()) > 0 Then
                                    ddlCategory.SelectedValue = CInt(ds.Tables(0).Rows(0)("Category").ToString())
                                End If
                            End If

                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub
    'Public Sub GetItemNo()
    '    Try
    '        If Session("ProductId") Is Nothing Then
    '            Dim itemno As String = ""
    '            Dim getItem As String = ""
    '            Dim FinalItemNo As String = ""
    '            Dim dtime As DateTime = DateTime.UtcNow
    '            itemno = dtime.ToString("yyyy-MM-dd")
    '            Dim Str As String = ""
    '            Dim ds As DataSet = Nothing
    '            Str = "SELECT p.ItemNumber FROM ManualProduct p WHERE CAST(CONVERT(varchar(8), p.DateCreated, 112) AS DATETIME)  = '" & itemno & "' AND id IN(SELECT max(id) FROM ManualProduct p2)"

    '            If Str.Length > 0 Then
    '                ds = BRIClassLibrary.SQLData.generic_select(Str, appGlobal.CONNECTIONSTRING)
    '                If Not ds Is Nothing Then
    '                    If ds.Tables(0).Rows.Count > 0 Then
    '                        getItem = ds.Tables(0).Rows(0)("ItemNumber").ToString().Trim
    '                        FinalItemNo = getItem.ToString().Substring(0, getItem.Length - 6)
    '                        If FinalItemNo.Contains("T") Then
    '                            If Session("IsThirdParty") = 3 Then
    '                                FinalItemNo = FinalItemNo.Remove(0, 1)
    '                                Dim NewItem As Integer = CInt(FinalItemNo) + 1
    '                                txtItemNo.Text = "T" & "" & NewItem.ToString("000") & "" & dtime.ToString("yyMMdd")
    '                            Else
    '                                If FinalItemNo.Contains("M") Then
    '                                    FinalItemNo = FinalItemNo.Remove(0, 2)
    '                                    Dim NewItem As Integer = CInt(FinalItemNo) + 1
    '                                    txtItemNo.Text = "M-" & NewItem.ToString("000") & "" & dtime.ToString("yyMMdd")
    '                                Else
    '                                    FinalItemNo = FinalItemNo.Remove(0, 1)
    '                                    Dim NewItem As Integer = CInt(FinalItemNo) + 1
    '                                    txtItemNo.Text = NewItem.ToString("000") & "" & dtime.ToString("yyMMdd")
    '                                End If

    '                            End If
    '                        Else
    '                            If FinalItemNo.Contains("M") Then
    '                                FinalItemNo = FinalItemNo.Remove(0, 2)
    '                                Dim NewItem As Integer = CInt(FinalItemNo) + 1
    '                                txtItemNo.Text = "M-" & NewItem.ToString("000") & "" & dtime.ToString("yyMMdd")
    '                            Else
    '                                FinalItemNo = FinalItemNo.Remove(0, 1)
    '                                Dim NewItem As Integer = CInt(FinalItemNo) + 1
    '                                txtItemNo.Text = NewItem.ToString("000") & "" & dtime.ToString("yyMMdd")
    '                            End If
    '                        End If

    '                    Else
    '                        If Session("IsThirdParty") = 3 Then
    '                            txtItemNo.Text = "T" & "001" & dtime.ToString("yyMMdd")
    '                        Else
    '                            txtItemNo.Text = "M-001" & dtime.ToString("yyMMdd")
    '                        End If

    '                    End If

    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub
    Public Sub GetItemNo()
        Try
            If Session("ProductId") Is Nothing Then
                Dim itemno As String = ""
                Dim getItem As String = ""
                Dim FinalItemNo As String = ""
                Dim dtime As DateTime = DateTime.UtcNow
                itemno = dtime.ToString("yyyyMMdd")
                Dim Str As String = ""
                Dim ds As DataSet = Nothing
                Str = " SELECT isnull(COUNT(*),0) + 1  AS Total FROM Product p WHERE CAST(CONVERT(varchar(8), p.DateCreated, 112) AS DATETIME) = '" & itemno & "'"
                If Str.Length > 0 Then
                    ds = BRIClassLibrary.SQLData.generic_select(Str, appGlobal.CONNECTIONSTRING)
                    If Not ds Is Nothing Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If Not ds.Tables(0).Rows(0)("Total").ToString() Is Nothing And ds.Tables(0).Rows(0)("Total").ToString() <> String.Empty Then
                                If CInt(ds.Tables(0).Rows(0)("Total").ToString()) > 0 Then
                                    If Session("IsThirdParty") = 3 Then
                                        txtItemNo.Text = "T" & ds.Tables(0).Rows(0)("Total").ToString().PadLeft(3, "0") & dtime.ToString("yyMMdd")
                                    Else
                                        txtItemNo.Text = "M-" & ds.Tables(0).Rows(0)("Total").ToString().PadLeft(3, "0") & dtime.ToString("yyMMdd")
                                    End If
                                End If

                            End If
                        Else
                            If Session("IsThirdParty") = 3 Then
                                txtItemNo.Text = "T" & "001" & dtime.ToString("yyMMdd")
                            Else
                                txtItemNo.Text = "M-001" & dtime.ToString("yyMMdd")
                            End If
                        End If
                    Else
                        If Session("IsThirdParty") = 3 Then
                            txtItemNo.Text = "T" & "001" & dtime.ToString("yyMMdd")
                        Else
                            txtItemNo.Text = "M-001" & dtime.ToString("yyMMdd")
                        End If
                    End If
                Else
                    If Session("IsThirdParty") = 3 Then
                        txtItemNo.Text = "T" & "001" & dtime.ToString("yyMMdd")
                    Else
                        txtItemNo.Text = "M-001" & dtime.ToString("yyMMdd")
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Function GetManufacturer(ByVal sManName As String) As Integer
        'test for the ManuFacturer name
        Dim sSQL As String = "select id from manufacturer where name='" & sManName.Replace("'", "''") & "'"
        Dim sManId As String = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        If sManId = "" Then
            'add it
            Dim sIns As String = "INSERT INTO MANUFACTURER ([NAME]) VALUES('" & sManName.Replace("'", "''") & "')"
            SQLData.generic_command(sIns, SQLData.ConnectionString)
            sManId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        End If
        If IsNumeric(sManId) Then
            Return CInt(sManId)
        Else
            Return 0
        End If
    End Function
    Private Function GetProductCategoryParent(ByVal nProductCategoryId As Integer) As Integer
        'test for the Product Category ID

        Dim sCatId As String = "0"
        If nProductCategoryId > 0 Then
            Dim sSQL As String = "select top 1 CategoryParentId from Category where Id=" & nProductCategoryId
            sCatId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        End If
        If IsNumeric(sCatId) Then
            Return CInt(sCatId)
        Else
            Return 0
        End If
    End Function
    Private Function getFiles(ByVal ProductId As Integer) As String
        Dim strFile As String = ""
        Try
            Dim str As String = ""
            Dim ds As DataSet = Nothing
            If Not Session("ProductId") Is Nothing Then
                str = "SELECT * FROM ManualProductImageCrossRef p where  p.ProductId =  " & CInt(Session("ProductId").ToString())
            End If
            If str.Length > 0 Then
                ds = SQLData.generic_select(str, appGlobal.CONNECTIONSTRING)
                If Not ds Is Nothing Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        strFile &= "<strong>Current Uploaded Files:</strong><br /><br />"
                        For Each dr As DataRow In ds.Tables(0).Rows
                            If Not dr("ImageUrl") Is Nothing And dr("ImageUrl").ToString() <> String.Empty Then
                                If Not dr("Name") Is Nothing And dr("Name").ToString() <> String.Empty Then
                                    strFile &= "<a target='_blank' style='font-wight:bold;' href='http://192.82.249.221/ProductImages/Large/Manual/" & dr("ImageUrl").ToString() & "'>" & dr("Name").ToString() & " </a><br />"
                                Else
                                    strFile &= "<a target='_blank' style='font-wight:bold;' href='http://192.82.249.221/ProductImages/Large/Manual/" & dr("ImageUrl").ToString() & "'> -- No Tile -- </a><br />"
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception

        End Try

        Return strFile

    End Function
End Class
