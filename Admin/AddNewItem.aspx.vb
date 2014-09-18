Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_AddNewItem
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
                    Session("waterfilename") = Nothing
                    Session("waterimgurl") = Nothing
                    Session("filename") = Nothing
                    Session("ItemNo") = Nothing
                    Session("Title") = Nothing
                    fill_ParentCategory()
                    GetItemNo()
                    ddl_Condition()
                    ddl_Age()
                    Try
                        ProductId = CInt(Request.QueryString("PID").ToString())
                    Catch ex As Exception
                        ProductId = 0
                    End Try
                    If ProductId > 0 Then
                        Session("ProductId") = ProductId
                        lblProductView.InnerHtml = "<a href='../Pages/ProductDetails.aspx?Id=" & Session("ProductId") & "' target='_blank'>Customer View</a> &nbsp; of Item"
                    End If
                    If Not (Session("ProductId")) Is Nothing Then
                        fill_Controls(CInt(Session("ProductId").ToString()))
                        btnAddItem.Text = "Update Item"
                        spanEdit.InnerHtml = "<a href='ProductCopy.aspx?CopyImage=0&CopyID=" & CInt(Session("ProductId").ToString()) & "'>Copy Data to New Item</a>  :  <a href='ProductCopy.aspx?CopyImage=1&CopyID=" & CInt(Session("ProductId").ToString()) & "'>Copy Data & Image to New Item</a>"
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
    Public Function Setdata(ByVal Obj As Product) As Product
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

            If Not String.IsNullOrEmpty(txtBarcodeNo.Text.ToString().Trim) AndAlso Not txtBarcodeNo.Text.ToString().Trim = "" Then
                Obj.Barcode = txtBarcodeNo.Text.ToString().Trim
            Else
                Obj.Barcode = ""
            End If
            If Not String.IsNullOrEmpty(txtBarcodeParentNo.Text.ToString().Trim) AndAlso Not txtBarcodeParentNo.Text.ToString().Trim = "" Then
                Obj.BarcodeParent = txtBarcodeParentNo.Text.ToString().Trim
            Else
                Obj.BarcodeParent = ""
            End If

            If ddlRow.SelectedValue <> "" AndAlso ddlShelf.SelectedValue <> "" AndAlso ddlSection.SelectedValue <> "" Then
                Obj.Location = ddlRow.SelectedValue.ToString() & ddlShelf.SelectedValue.ToString() & ddlSection.SelectedValue.ToString()
            Else
                Obj.Location = ""
            End If


            If Not String.IsNullOrEmpty(txtDescription.Text.ToString().Trim) AndAlso Not txtDescription.Text.ToString().Trim = "" Then
                Obj.Description = txtDescription.Text.ToString().Trim
            Else
                Obj.Description = ""
            End If
            
            If Not String.IsNullOrEmpty(txtAdminDescription.Text.ToString().Trim) AndAlso Not txtAdminDescription.Text.ToString().Trim = "" Then
                Obj.AdminNotes = txtAdminDescription.Text.ToString().Trim
            Else
                Obj.AdminNotes = ""
            End If
            If IsNumeric(txtPrice.Text.ToString()) Then
                Obj.Price = CDbl(txtPrice.Text.ToString())
            Else
                Obj.Price = 0.0

            End If
            If IsNumeric(txtLowestPrice.Text.ToString()) Then
                Obj.LowestPrice = CDbl(txtLowestPrice.Text.ToString())
            Else
                Obj.LowestPrice = 0.0
            End If
            If IsNumeric(txtAuctionStart.Text.ToString()) Then
                Obj.AuctionStart = CDbl(txtAuctionStart.Text.ToString())
            Else
                Obj.AuctionStart = 0.0
            End If
            If IsNumeric(txtCostofGoods.Text.ToString()) Then
                Obj.CostofGoods = CDbl(txtCostofGoods.Text.ToString())
            Else
                Obj.CostofGoods = 0.0
            End If

            If Not String.IsNullOrEmpty(txtManualItemNo.Text.ToString().Trim) AndAlso Not txtManualItemNo.Text.ToString().Trim = "" Then
                Obj.ManualItemNo = txtManualItemNo.Text.ToString().Trim
            Else
                Obj.ManualItemNo = ""
            End If
            If txtQuantity.Text.ToString() <> "" Then
                If CInt(txtQuantity.Text.ToString()) > 0 Then
                    Obj.Qty = CInt(txtQuantity.Text.ToString().Trim)
                Else
                    Obj.Qty = 0
                End If
            Else
                Obj.Qty = 0
            End If
            If txtTotalPieces.Text.ToString() <> "" Then
                If CInt(txtTotalPieces.Text.ToString()) > 0 Then
                    Obj.TotalPieces = CInt(txtTotalPieces.Text.ToString().Trim)
                Else
                    Obj.TotalPieces = 0
                End If
            Else
                Obj.TotalPieces = 0
            End If
           

            If ddlCondition.SelectedValue <> "" Then
                Obj.Condition = ddlCondition.SelectedValue.ToString().Trim
            Else
                Obj.Condition = ""
            End If
            If ddlAge.SelectedValue <> "" Then
                Obj.Age = ddlAge.SelectedValue.ToString().Trim
            Else
                Obj.Age = ""
            End If
            If IsNumeric(txtWeight.Text.ToString()) Then
                Obj.Weight = CDbl(txtWeight.Text.ToString())
            Else
                Obj.Weight = 0.0
            End If
            
            If ddlCategory.SelectedValue <> "-1" Then
                Obj.Category = CInt(ddlCategory.SelectedValue.ToString())
                Obj.ParentCategory = GetProductCategoryParent(CInt(ddlCategory.SelectedValue.ToString()))
            Else
                Obj.ParentCategory = 0
                Obj.Category = "-1"
            End If

            If chkItem.SelectedValue <> "" Then
                If chkItem.Items(0).Selected = True Then
                    Obj.IsNotOnWeb = 1
                Else
                    Obj.IsNotOnWeb = 0
                End If

                If chkItem.Items(1).Selected = True Then
                    Obj.IsDoNotRelease = 1
                Else
                    Obj.IsDoNotRelease = 0
                End If

                If chkItem.Items(2).Selected = True Then
                    Obj.IsFeaturedItem = 1
                Else
                    Obj.IsFeaturedItem = 0
                End If

                If chkItem.Items(3).Selected = True Then
                    Obj.IsNewArrivalsPage = 1
                Else
                    Obj.IsNewArrivalsPage = 0
                End If

                If chkItem.Items(4).Selected = True Then
                    Obj.IsConsignmentItem = 1
                Else
                    Obj.IsConsignmentItem = 0
                End If

                If chkItem.Items(5).Selected = True Then
                    Obj.IsJustOfftheTruck = 1
                Else
                    Obj.IsJustOfftheTruck = 0
                End If

                If chkItem.Items(6).Selected = True Then
                    Obj.IsSold = 1
                Else
                    Obj.IsSold = 0
                End If

                If chkItem.Items(7).Selected = True Then
                    Obj.IsDeleteItem = 1
                Else
                    Obj.IsDeleteItem = 0
                End If

                If chkItem.Items(8).Selected = True Then
                    Obj.IsLabX = 1
                Else
                    Obj.IsLabX = 0
                End If
            End If
            If Not String.IsNullOrEmpty(txtVideo.Text.ToString()) AndAlso txtVideo.Text.ToString() <> "" Then
                Obj.VideoURL = txtVideo.Text.ToString()
            Else
                Obj.VideoURL = ""
            End If

            If Not Session("ProductId") Is Nothing Then
                If chkadditionalLink.SelectedValue <> "" Then
                    If chkadditionalLink.Items(0).Selected = True Then
                        Obj.IsIncludeinNewsletter = 1
                    Else
                        Obj.IsIncludeinNewsletter = 0
                    End If

                    If chkadditionalLink.Items(1).Selected = True Then
                        Obj.IsDeletePermanently = 1
                    Else
                        Obj.IsDeletePermanently = 0
                    End If

                    If chkadditionalLink.Items(2).Selected = True Then
                        Obj.IsPaid = 1
                    Else
                        Obj.IsPaid = 0
                    End If

                    If chkadditionalLink.Items(3).Selected = True Then
                        Obj.IsShipped = 1
                    Else
                        Obj.IsShipped = 0
                    End If

                    If chkadditionalLink.Items(4).Selected = True Then
                        Obj.IsCompleted = 1
                    Else
                        Obj.IsCompleted = 0
                    End If
                End If

            End If

            If Not Session("ProductId") Is Nothing Then
                Obj.LastEdited = DateTime.UtcNow
                Obj.EditorID = CInt(Session("Id").ToString())
            Else
                Obj.CreatorID = CInt(Session("Id").ToString())
                Obj.DateCreated = DateTime.UtcNow
            End If
           

            If Not Session("ProductId") Is Nothing Then
                Try
                    If chkDelete.Checked = True Then
                        If Not ImageContainer Is Nothing Then
                            If Not Session("imgurl") Is Nothing Then
                                Obj.Id = CInt(Session("ProductId").ToString())
                                Obj.ImageFileName = ""
                                Obj.UpdateImage()

                                Dim filePath As String = Path.Combine(Request.PhysicalApplicationPath, "ProductImages/Large/")
                                Dim File As String = Path.Combine(filePath, Session("imgurl"))
                                Try
                                    If System.IO.File.Exists(File) Then
                                        System.IO.File.Delete(File)
                                    End If
                                Catch ex As Exception
                                End Try
                                Session("imgurl") = Nothing
                            End If
                        End If

                    End If
                Catch ex As Exception

                End Try
            End If

            If Not Session("filename") Is Nothing Then
                Obj.ImageFileName = Session("filename").ToString()
            Else
                If Not Session("imgurl") Is Nothing Then
                    Obj.ImageFileName = Session("imgurl")
                Else
                    Obj.ImageFileName = String.Empty
                End If
            End If

            If Not Session("waterfilename") Is Nothing Then
                Obj.Watermarkimg = Session("waterfilename").ToString()
            Else
                If Not Session("imgwaterurl") Is Nothing Then
                    Obj.Watermarkimg = Session("imgwaterurl")
                Else
                    Obj.Watermarkimg = String.Empty
                End If
            End If

        Catch ex As Exception

        End Try

        Return Obj
    End Function
    Public Function Validate_Control() As String
        If Not String.IsNullOrEmpty(txtMake.Text.ToString().Trim) AndAlso Len(txtMake.ToString().Trim) <= 0 Then
            errStr &= "Please enter Make Name" & Environment.NewLine
        End If

        If Not flName.FileName Is Nothing And flName.FileName <> String.Empty Then
            If flName.FileName.Length <= 50 Then
                Dim filelength As Integer = 0
                filelength = flName.PostedFile.ContentLength
                If filelength > 0 Then
                    filelength = filelength / (1024 * 1024)
                End If
                If filelength <= 1 Then
                    Dim filePath As String = Path.Combine(Request.PhysicalApplicationPath, "ProductImages/Large/Images/")
                    If Not Directory.Exists(filePath) Then
                        Directory.CreateDirectory(filePath)
                    End If
                    Dim File As String = Path.Combine(filePath, flName.FileName)
                    Dim fileExt As String = Path.GetExtension(flName.FileName).ToLower()
                    If fileExt = ".jpg" Or fileExt = ".jpeg" Or fileExt = ".gif" Or fileExt = ".png" Or fileExt = ".bmp" Then
                        Try
                            If System.IO.File.Exists(File) Then
                                System.IO.File.Delete(File)
                            End If
                        Catch ex As Exception
                        End Try
                        flName.SaveAs(File)
                        Session("fileName") = flName.FileName
                    Else
                        DisplayAlert("Only file with extension "".jpg"", "".jpeg"", "".gif"", "".png"", "".bmp"" are  allowed")
                    End If
                Else
                    DisplayAlert("File Size can not be more than 1 MB")
                End If
            Else
                DisplayAlert("File name can not be more than 50 Characters")
            End If
        End If

        If Not flWatermarkimg.FileName Is Nothing And flWatermarkimg.FileName <> String.Empty Then
            If flWatermarkimg.FileName.Length <= 50 Then
                Dim filelength As Integer = 0
                filelength = flWatermarkimg.PostedFile.ContentLength
                If filelength > 0 Then
                    filelength = filelength / (1024 * 1024)
                End If
                If filelength <= 1 Then
                    Dim filePath As String = ""
                    filePath = Path.Combine(Request.PhysicalApplicationPath, "ProductImages/Large/NoWaterMarkImage/")
                    If Not Directory.Exists(filePath) Then
                        Directory.CreateDirectory(filePath)
                    End If
                    Dim File As String = Path.Combine(filePath, flWatermarkimg.FileName)
                    Dim fileExt As String = Path.GetExtension(flWatermarkimg.FileName).ToLower()
                    If fileExt = ".jpg" Or fileExt = ".jpeg" Or fileExt = ".gif" Or fileExt = ".png" Or fileExt = ".bmp" Then
                        Try
                            If System.IO.File.Exists(File) Then
                                System.IO.File.Delete(File)
                            End If
                        Catch ex As Exception
                        End Try
                        flWatermarkimg.SaveAs(File)
                        Session("waterfilename") = flWatermarkimg.FileName
                    Else
                        DisplayAlert("Only file with extension "".jpg"", "".jpeg"", "".gif"", "".png"", "".bmp"" are  allowed")
                    End If
                Else
                    DisplayAlert("File Size can not be more than 1 MB")
                End If
            Else
                DisplayAlert("Watermark File name can not be more than 50 Characters")
            End If
        End If

        Return errStr
    End Function
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Public Sub ddl_Condition()
        Try
            Dim sSQl As String = "SELECT * FROM AdminSystemData asd WHERE asd.[Type]=2"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlCondition.AppendDataBoundItems = True
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.ddlCondition.Items.Add(New ListItem(dr("Name"), dr("Name")))
            Next
        Catch ex As Exception

        End Try

    End Sub
    Public Sub ddl_Age()
        Try

            Dim sSQl As String = "SELECT * FROM AdminSystemData asd WHERE asd.[Type]=3"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlAge.AppendDataBoundItems = True
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.ddlAge.Items.Add(New ListItem(dr("Name"), dr("Name")))
            Next
        Catch ex As Exception

        End Try

    End Sub
 
    Protected Sub btnAddItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddItem.Click
        Dim checkInsert As Integer = 0
        errStr = String.Empty
        errStr = Validate_Control()
        If errStr.Length <= 0 Then
            Dim Obj As New Product(appGlobal.CONNECTIONSTRING)
            Obj = Setdata(Obj)
            Try
                If Not Session("ProductId") Is Nothing Then
                    Obj.Id = CInt(Session("ProductId").ToString())
                    If Not GetProductCategoryCrossRef(CInt(ddlCategory.SelectedValue.Trim), CInt(Session("ProductId").ToString())) Then

                    End If
                    If Obj.Update() Then
                        Session("imgurl") = Nothing
                        Session("waterfilename") = Nothing
                        Session("waterimgurl") = Nothing
                        Session("filename") = Nothing
                        If Obj.ImageFileName <> "" Then
                            Get_Files(Obj.ImageFileName.Trim)
                        End If

                        Try
                            If Not Session("Id") Is Nothing Then
                                Try
                                    Dim sIns As String = "INSERT INTO ProductLog ([UserId] ,[Username],[ProductId],[ModifiedOn]) value (" & CInt(Session("Id").ToString()) & ",'" & Session("UserName").ToString() & "','" & CInt(Session("ProductId").ToString()) & "','" & CDate(DateTime.UtcNow.ToString()) & "')"
                                    SQLData.generic_command(sIns, SQLData.ConnectionString)
                                Catch ex As Exception

                                End Try
                            End If
                        Catch ex As Exception

                        End Try

                        DisplayAlert("Your Item is successfully Updated !!!")
                    Else
                        DisplayAlert("Your Item is not Updated!")
                    End If
                Else
                    checkInsert = Obj.InsertIntoProduct()
                    If checkInsert > 0 Then
                        Session("imgurl") = Nothing
                        Session("waterfilename") = Nothing
                        Session("waterimgurl") = Nothing
                        Session("filename") = Nothing
                        Session("ProductId") = checkInsert
                        If Not GetProductCategoryCrossRef(CInt(ddlCategory.SelectedValue.Trim), checkInsert) Then

                        End If
                        If Obj.ImageFileName <> "" Then
                            Get_Files(Obj.ImageFileName.Trim)
                        End If
                        Try
                            If Not Session("Id") Is Nothing Then
                                Try
                                    Dim sIns As String = "INSERT INTO ProductLog ([UserId] ,[Username],[ProductId],[CreatedOn]) value (" & CInt(Session("Id").ToString()) & ",'" & Session("UserName").ToString() & "','" & CInt(Session("ProductId").ToString()) & "','" & CDate(DateTime.UtcNow.ToString()) & "')"
                                    SQLData.generic_command(sIns, SQLData.ConnectionString)
                                Catch ex As Exception

                                End Try
                            End If
                        Catch ex As Exception

                        End Try

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
        txtBarcodeNo.Text = ""
        txtBarcodeParentNo.Text = ""
        txtLocation.Text = ""
        txtDescription.Text = ""
        txtAdminDescription.Text = ""
        txtPrice.Text = ""
        txtLowestPrice.Text = ""
        txtAuctionStart.Text = ""
        txtCostofGoods.Text = ""
        ' txtSellingPrice.Text = ""
        txtManualItemNo.Text = ""
        txtQuantity.Text = ""
        ' txtQuantitySold.Text = ""
        txtTotalPieces.Text = ""
        txtWeight.Text = ""
        ddlAge.SelectedValue = -1
        ddlCondition.SelectedValue = -1
        ddlCategory.SelectedValue = "-1"
        chkItem.Items.Clear()
        chkDelete.Checked = False
        txtVideo.Text = ""
        chkadditionalLink.Items.Clear()
        'Session("ProductId") = Nothing
        hederitemright.Visible = False


    End Sub

    Private Sub fill_Controls(ByVal nProductID As Integer)
        If nProductID > 0 Then
            Try
                Dim sql As String = ""
                Dim ds As DataSet = Nothing
                sql = "SELECT * FROM Product p WHERE p.Id = " & nProductID
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

                            If Not ds.Tables(0).Rows(0)("Barcode") Is Nothing AndAlso ds.Tables(0).Rows(0)("Barcode").ToString() <> "" Then
                                txtBarcodeNo.Text = ds.Tables(0).Rows(0)("Barcode").ToString()
                            End If
                            If Not ds.Tables(0).Rows(0)("BarcodeParent") Is Nothing AndAlso ds.Tables(0).Rows(0)("BarcodeParent").ToString() <> "" Then
                                txtBarcodeParentNo.Text = ds.Tables(0).Rows(0)("BarcodeParent").ToString()
                            End If
                            If Not ds.Tables(0).Rows(0)("Location") Is Nothing AndAlso ds.Tables(0).Rows(0)("Location").ToString() <> "" Then
                                txtLocation.Text = ds.Tables(0).Rows(0)("Location").ToString()
                            End If

                            If Not ds.Tables(0).Rows(0)("Description") Is Nothing AndAlso ds.Tables(0).Rows(0)("Description").ToString() <> "" Then
                                txtDescription.Text = HttpUtility.HtmlDecode(ds.Tables(0).Rows(0)("Description").ToString())
                            End If
                            If Not ds.Tables(0).Rows(0)("AdminNotes") Is Nothing AndAlso ds.Tables(0).Rows(0)("AdminNotes").ToString() <> "" Then
                                txtAdminDescription.Text = ds.Tables(0).Rows(0)("AdminNotes").ToString()
                            End If
                            If Not ds.Tables(0).Rows(0)("Price") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("Price")) > 0 Then
                                txtPrice.Text = CDbl(ds.Tables(0).Rows(0)("Price").ToString()).ToString("0.00")
                            End If
                            If Not ds.Tables(0).Rows(0)("LowestPrice") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("LowestPrice")) > 0 Then
                                txtLowestPrice.Text = CDbl(ds.Tables(0).Rows(0)("LowestPrice").ToString()).ToString("0.00")
                            End If
                            If Not ds.Tables(0).Rows(0)("AuctionStart") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("AuctionStart")) > 0 Then
                                txtAuctionStart.Text = CDbl(ds.Tables(0).Rows(0)("AuctionStart").ToString()).ToString("0.00")
                            End If
                            If Not ds.Tables(0).Rows(0)("CostofGoods") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("CostofGoods")) > 0 Then
                                txtCostofGoods.Text = CDbl(ds.Tables(0).Rows(0)("CostofGoods").ToString()).ToString("0.00")
                            End If
                            'If Not ds.Tables(0).Rows(0)("SellingPrice") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("SellingPrice")) > 0 Then
                            '    txtSellingPrice.Text = CDbl(ds.Tables(0).Rows(0)("SellingPrice").ToString()).ToString("0.00")
                            'End If
                            'If Not ds.Tables(0).Rows(0)("QuantitySold") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("QuantitySold")) > 0 Then
                            '    txtQuantitySold.Text = CInt(ds.Tables(0).Rows(0)("QuantitySold").ToString())
                            'Else
                            '    txtQuantitySold.Text = 0
                            'End If
                            If Not ds.Tables(0).Rows(0)("Qty") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("Qty")) > 0 Then
                                txtQuantity.Text = CInt(ds.Tables(0).Rows(0)("Qty").ToString())
                            Else
                                txtQuantity.Text = 0
                            End If

                            If Not ds.Tables(0).Rows(0)("TotalPieces") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("TotalPieces")) > 0 Then
                                txtTotalPieces.Text = CDbl(ds.Tables(0).Rows(0)("TotalPieces").ToString())
                            End If
                            If Not ds.Tables(0).Rows(0)("Condition") Is Nothing AndAlso ds.Tables(0).Rows(0)("Condition").ToString() <> "" Then
                                ddlCondition.SelectedValue = ds.Tables(0).Rows(0)("Condition").ToString()
                            End If

                            If Not ds.Tables(0).Rows(0)("Age") Is Nothing AndAlso ds.Tables(0).Rows(0)("Age").ToString() <> "" Then
                                ddlAge.SelectedValue = ds.Tables(0).Rows(0)("Age").ToString()
                            End If

                            If Not ds.Tables(0).Rows(0)("ManualItemNo") Is Nothing AndAlso ds.Tables(0).Rows(0)("ManualItemNo").ToString() <> "" Then
                                txtManualItemNo.Text = ds.Tables(0).Rows(0)("ManualItemNo").ToString()
                                hederitemright.InnerText = "Manual Item No :" & ds.Tables(0).Rows(0)("ManualItemNo").ToString()
                                hederitemright.Style.Add("background-color", "#ff33ff")
                                hederitemright.Style.Add("color", "#fff")
                                hederitemright.Style.Add("padding", "2px")
                            End If

                            If ds.Tables(0).Rows(0)("Weight") > 0 Then
                                txtWeight.Text = CDbl(ds.Tables(0).Rows(0)("Weight").ToString())
                            Else
                                txtWeight.Text = "0.00"
                            End If
                            If Not ds.Tables(0).Rows(0)("Category") Is Nothing AndAlso ds.Tables(0).Rows(0)("Category").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("Category").ToString()) > 0 Then
                                    ddlCategory.SelectedValue = CInt(ds.Tables(0).Rows(0)("Category").ToString())
                                    lblCategory.InnerHtml = ShowCategoryName(CInt(ds.Tables(0).Rows(0)("Category").ToString()))
                                End If
                            End If


                            If Not ds.Tables(0).Rows(0)("IsNotOnWeb") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsNotOnWeb").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsNotOnWeb")) > 0 Then
                                    chkItem.Items(0).Selected = True
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("IsDoNotRelease") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsDoNotRelease").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsDoNotRelease")) > 0 Then
                                    chkItem.Items(1).Selected = True
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("IsFeaturedItem") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsFeaturedItem").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsFeaturedItem")) > 0 Then
                                    chkItem.Items(2).Selected = True
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("IsNewArrivalsPage") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsNewArrivalsPage").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsNewArrivalsPage")) > 0 Then
                                    chkItem.Items(3).Selected = True
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("IsConsignmentItem") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsConsignmentItem").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsConsignmentItem")) > 0 Then
                                    chkItem.Items(4).Selected = True
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("IsJustOfftheTruck") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsJustOfftheTruck").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsJustOfftheTruck")) > 0 Then
                                    chkItem.Items(5).Selected = True
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("IsSold") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsSold").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsSold")) > 0 Then
                                    chkItem.Items(6).Selected = True
                                End If
                            End If

                            If Not ds.Tables(0).Rows(0)("IsDeleteItem") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsDeleteItem").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsDeleteItem")) > 0 Then
                                    chkItem.Items(7).Selected = True
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("IsLabX") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsLabX").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsLabX")) > 0 Then
                                    chkItem.Items(8).Selected = True
                                End If
                            End If

                            If Not ds.Tables(0).Rows(0)("IsIncludeinNewsletter") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsIncludeinNewsletter").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsIncludeinNewsletter")) > 0 Then
                                    chkadditionalLink.Items(0).Selected = True
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("IsDeletePermanently") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsDeletePermanently").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsDeletePermanently")) > 0 Then
                                    chkadditionalLink.Items(1).Selected = True
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("IsPaid") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsPaid").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsPaid")) > 0 Then
                                    chkadditionalLink.Items(2).Selected = True
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("IsShipped") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsShipped").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsShipped")) > 0 Then
                                    chkadditionalLink.Items(3).Selected = True
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("IsCompleted") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsCompleted").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsCompleted")) > 0 Then
                                    chkadditionalLink.Items(4).Selected = True
                                End If
                            End If

                            If Not ds.Tables(0).Rows(0)("ImageFileName") Is Nothing AndAlso ds.Tables(0).Rows(0)("ImageFileName").ToString() <> "" Then
                                Get_Files(ds.Tables(0).Rows(0)("ImageFileName").ToString())
                                Session("imgurl") = ds.Tables(0).Rows(0)("ImageFileName").ToString()
                            End If
                            If Not ds.Tables(0).Rows(0)("VideoURL") Is Nothing AndAlso ds.Tables(0).Rows(0)("VideoURL").ToString() <> "" Then
                                txtVideo.Text = ds.Tables(0).Rows(0)("VideoURL").ToString()
                            End If


                            Try
                                If Not ds.Tables(0).Rows(0)("LastEdited") Is Nothing AndAlso CDate(ds.Tables(0).Rows(0)("LastEdited")).ToString("MM/dd/yyyy") <> "12/31/1977" Then
                                    LastEdit.InnerHtml = "Last Edited :" & CDate(ds.Tables(0).Rows(0)("LastEdited"))
                                Else
                                    LastEdit.InnerHtml = "Last Edited :" & DateTime.UtcNow
                                End If
                            Catch ex As Exception
                                LastEdit.InnerHtml = "Last Edited :" & DateTime.UtcNow
                            End Try

                            If Not ds.Tables(0).Rows(0)("Watermarkimg") Is Nothing AndAlso ds.Tables(0).Rows(0)("Watermarkimg").ToString() <> "" Then
                                Session("waterimgurl") = ds.Tables(0).Rows(0)("Watermarkimg").ToString()
                            End If
                            Hits.InnerHtml = "Hits : 0"
                            FunctionTitle.InnerText = " Edit Inventory Item"
                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub
    Private Sub Get_Files(ByVal flname As String)
        Try
            'Dim str As String = Path.Combine(Request.PhysicalApplicationPath, "ProductImages/" & flname)
            Dim html As String = ""
            html = "<img src='../ProductImages/Large/Images/" & flname & "' alt='" & flname & "' Height='auto' style='float: left;max-width: 720px;padding: 20px;'>"
            ImageContainer.InnerHtml = html
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Location_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRow.SelectedIndexChanged, ddlShelf.SelectedIndexChanged, ddlSection.SelectedIndexChanged
        Dim row As String = ""
        Dim shelf As String = ""
        Dim selection As String = ""
        Try
            If ddlRow.SelectedValue <> "" Then
                row = ddlRow.SelectedValue.ToString()
            End If

            If ddlShelf.SelectedValue <> "" Then
                shelf = ddlShelf.SelectedValue.ToString()
            End If

            If ddlSection.SelectedValue <> "" Then
                selection = ddlSection.SelectedValue.ToString()
            End If

            If ddlRow.SelectedValue <> "" AndAlso ddlShelf.SelectedValue <> "" AndAlso ddlSection.SelectedValue <> "" Then
                txtLocation.Text = row & " " & shelf & "-" & selection
            End If
            
        Catch ex As Exception
        End Try
    End Sub

    'Public Sub GetItemNo()
    '    Try
    '        If Session("ProductId") Is Nothing Then
    '            Dim itemno As String = ""
    '            Dim getItem As String = ""
    '            Dim FinalItemNo As String = ""
    '            Dim dtime As DateTime = DateTime.UtcNow
    '            itemno = dtime.ToString("yyyymmdd")
    '            Dim Str As String = ""
    '            Dim ds As DataSet = Nothing
    '            Str = "SELECT p.ItemNumber FROM Product p WHERE CAST(CONVERT(varchar(8), p.DateCreated, 112) AS DATETIME)  = '" & itemno & "' AND id IN(SELECT max(id) FROM Product p2)"
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
    '                                FinalItemNo = FinalItemNo.Remove(0, 1)
    '                                Dim NewItem As Integer = CInt(FinalItemNo) + 1
    '                                txtItemNo.Text = "" & NewItem.ToString("000") & "" & dtime.ToString("yyMMdd")
    '                            End If
    '                        Else
    '                            txtItemNo.Text = "" & (FinalItemNo + 1).ToString("000") & "" & dtime.ToString("yyMMdd")
    '                        End If

    '                    Else
    '                        If Session("IsThirdParty") = 3 Then
    '                            txtItemNo.Text = "T" & "001" & dtime.ToString("yyMMdd")
    '                        Else
    '                            txtItemNo.Text = "001" & dtime.ToString("yyMMdd")
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
                                        txtItemNo.Text = ds.Tables(0).Rows(0)("Total").ToString().PadLeft(3, "0") & dtime.ToString("yyMMdd")
                                    End If
                                End If

                            End If
                        Else
                            If Session("IsThirdParty") = 3 Then
                                txtItemNo.Text = "T" & "001" & dtime.ToString("yyMMdd")
                            Else
                                txtItemNo.Text = "001" & dtime.ToString("yyMMdd")
                            End If
                        End If
                    Else
                        If Session("IsThirdParty") = 3 Then
                            txtItemNo.Text = "T" & "001" & dtime.ToString("yyMMdd")
                        Else
                            txtItemNo.Text = "001" & dtime.ToString("yyMMdd")
                        End If
                    End If
                Else
                    If Session("IsThirdParty") = 3 Then
                        txtItemNo.Text = "T" & "001" & dtime.ToString("yyMMdd")
                    Else
                        txtItemNo.Text = "001" & dtime.ToString("yyMMdd")
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub addimg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles addimg.Click
        Try
            Response.Redirect("ProductImage.aspx?ProductId=" & Session("ProductId").ToString())
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
    Private Function GetProductCategoryCrossRef(ByVal nProductCategoryId As Integer, ByVal nProductId As Integer) As Boolean
        Try
            Dim bSuccess As Boolean = True
            Dim sId As String = "0"
            Try
                Dim sDel As String = "DELETE FROM PRODUCTCATEGORYCROSSREF WHERE ProductId=" & nProductId
                SQLData.generic_command(sDel, SQLData.ConnectionString)
            Catch ex As Exception

            End Try


            If nProductCategoryId > 0 Then
                Dim sSQL As String = "SELECT ID FROM PRODUCTCATEGORYCROSSREF WHERE  PRODUCTID=" & nProductId & " AND PRODUCTCATEGORYID=" & nProductCategoryId
                sId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
                If sId = "" Then
                    'add it
                    Dim sIns As String = "INSERT INTO PRODUCTCATEGORYCROSSREF ([PRODUCTID], PRODUCTCATEGORYID) VALUES('" & nProductId & "','" & nProductCategoryId & "')"
                    SQLData.generic_command(sIns, SQLData.ConnectionString)
                    sId = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
                End If
            End If

            Return bSuccess

        Catch ex As Exception

        End Try

    End Function

    Protected Sub chkItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkItem.SelectedIndexChanged
        Try
            If chkItem.SelectedIndex <> -1 Then
                If chkItem.Items(8).Selected = True Then
                    chkItem.Items(0).Selected = True
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkNoWatermarkImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkNoWatermarkImage.Click
        Try
            If Not String.IsNullOrEmpty(txtItemNo.Text.ToString()) AndAlso Not txtItemNo.Text.ToString() Is Nothing Then
                Response.Redirect("NoWatermarkImages.aspx?Itemno=" & CInt(Session("ProductId").ToString()))
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub lnkWatermarkImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkWatermarkImage.Click
        Try
            If Not String.IsNullOrEmpty(txtItemNo.Text.ToString()) AndAlso Not txtItemNo.Text.ToString() Is Nothing Then
                Response.Redirect("WatermarkImages.aspx?Itemno=" & CInt(Session("ProductId").ToString()))
            End If
        Catch ex As Exception

        End Try
    End Sub


    Public Function ShowCategoryName(ByVal Id As Integer) As String
        Dim str As String = String.Empty
        Dim sGPTitle As String = String.Empty
        Dim sPTitle As String = String.Empty
        Dim sTitle As String = String.Empty
        Dim UserDS As New DataSet
        Dim sStrNew As String = ""

        Try
            sStrNew = "SELECT c.Id, c.CategoryName, ParentId= isnull((SELECT Id FROM Category c2 WHERE c2.Id= c.CategoryParentId),0), Parent= (SELECT CategoryName FROM Category c2 WHERE c2.Id= c.CategoryParentId), GrandParent= (SELECT  CategoryName FROM Category c3 WHERE c3.Id = (SELECT CategoryParentId FROM Category c2 WHERE c2.Id= c.CategoryParentId)), GrandParentId= isnull((SELECT  Id FROM Category c3 WHERE c3.Id = (SELECT CategoryParentId FROM Category c2 WHERE c2.Id= c.CategoryParentId)),0) fROM Category c WHERE c.Id = " & Id
            UserDS = SQLData.generic_select(sStrNew, strConnection)

            If Not UserDS Is Nothing Then
                If UserDS.Tables(0).Rows.Count > 0 Then
                    If Not UserDS.Tables(0).Rows(0)("GrandParent").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("GrandParent").ToString() <> String.Empty Then
                        sGPTitle = UserDS.Tables(0).Rows(0)("GrandParent").ToString().Trim
                    End If
                    If Not UserDS.Tables(0).Rows(0)("Parent").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("Parent").ToString() <> String.Empty Then
                        sPTitle = UserDS.Tables(0).Rows(0)("Parent").ToString().Trim
                    End If
                    If Not UserDS.Tables(0).Rows(0)("CategoryName").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("CategoryName").ToString() <> String.Empty Then
                        sTitle = UserDS.Tables(0).Rows(0)("CategoryName").ToString().Trim
                    End If
                End If
            End If
            If sGPTitle.Length > 0 Then
                If sPTitle.Length > 0 Then
                    If sTitle.Length > 0 Then
                        str = "Current Category:&nbsp;&nbsp;<span style='color:f33;'>" & sGPTitle & "</span>"
                        str &= "&nbsp;&nbsp;>> &nbsp;&nbsp;<span style='color:f33;'>" & sPTitle & "</span>"
                        str &= "&nbsp;&nbsp;>> &nbsp;&nbsp;<span style='color:f33;'>" & sTitle & "</span>"
                    Else
                        str = "Current Category:&nbsp;&nbsp;<span style='color:f33;'>" & sGPTitle & "</span>"
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<span style='color:f33;'>" & sPTitle & "</span>"
                    End If
                Else
                    If sTitle.Length > 0 Then
                        str = "Current Category:&nbsp;&nbsp;<span style='color:f33;'>" & sGPTitle & "</span>"
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<span style='color:f33;'>" & sTitle & "</span>"
                    Else
                        str = "Current Category:&nbsp;&nbsp;<span style='color:f33;'>" & sGPTitle & "</span>"
                    End If
                End If
            Else
                If sPTitle.Length > 0 Then
                    If sTitle.Length > 0 Then
                        str = "Current Category:&nbsp;&nbsp;<span style='color:f33;'>" & sPTitle & "</span>"
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<span style='color:f33;'>" & sTitle & "</span>"
                    Else
                        str = "Current Category:&nbsp;&nbsp;<span style='color:f33;'>" & sPTitle & "</span>"
                    End If
                Else
                    str = "Current Category:&nbsp;&nbsp;<span style='color:f33;'>" & sTitle & "</span>"
                End If
            End If

        Catch ex As Exception
            Return str
        End Try

        Return str
    End Function
End Class
