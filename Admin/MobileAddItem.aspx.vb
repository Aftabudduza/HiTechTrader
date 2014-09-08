Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_MobileAddItem
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
                    If Not (Session("ProductId")) Is Nothing Then
                        fill_Controls(CInt(Session("ProductId").ToString()))
                        btnAddItem.Text = "Update Item"
                        'spanEdit.InnerHtml = "<a href='ProductCopy.aspx?CopyImage=0&CopyID=" & CInt(Session("ProductId").ToString()) & "'>Copy Data to New Item</a>  :  <a href='ProductCopy.aspx?CopyImage=1&CopyID=" & CInt(Session("ProductId").ToString()) & "'>Copy Data & Image to New Item</a>"
                    End If

                End If
            Else
                Response.Redirect("MobileLogin.aspx")
            End If
        Else
            Response.Redirect("MobileLogin.aspx")
        End If

    End Sub
    Private Sub fill_ParentCategory()
        Try
            Dim sSQl As String = "SELECT * FROM Category c WHERE c.CategoryParentId =0 AND c.IsMainorLabX = 0"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlCategory.Items.Clear()
            ddlCategory.AppendDataBoundItems = True
            ddlCategory.Items.Add(New ListItem("--- Select Category ---", "-1"))
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
            If Not String.IsNullOrEmpty(txtLocation.Text.ToString().Trim) AndAlso Not txtLocation.Text.ToString().Trim = "" Then
                Obj.Location = txtLocation.Text.ToString().Trim
            Else
                Obj.Location = ""
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
           
            If IsNumeric(txtPrice.Text.ToString()) Then
                Obj.Price = CDbl(txtPrice.Text.ToString())
            Else
                Obj.Price = 0.0

            End If
            If IsNumeric(txtPODPrice.Text.ToString()) Then
                Obj.LowestPrice = CDbl(txtPODPrice.Text.ToString())
            Else
                Obj.LowestPrice = 0.0
            End If
           
            If ddlCategory.SelectedValue <> "-1" Then
                Obj.Category = CInt(ddlCategory.SelectedValue.ToString())
                Obj.ParentCategory = GetProductCategoryParent(CInt(ddlCategory.SelectedValue.ToString()))
            Else
                Obj.ParentCategory = 0
                Obj.Category = "-1"
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
            Dim Obj As New Product(appGlobal.CONNECTIONSTRING)
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
        txtBarcodeNo.Text = ""
        txtBarcodeParentNo.Text = ""
        txtLocation.Text = ""
        txtPrice.Text = ""
        ddlCategory.SelectedValue = "-1"
        Session("ProductId") = Nothing
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
                            If Not ds.Tables(0).Rows(0)("Price") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("Price")) > 0 Then
                                txtPrice.Text = CDbl(ds.Tables(0).Rows(0)("Price").ToString()).ToString("0.00")
                            Else
                                txtPrice.Text = 0.0
                            End If
                            If Not ds.Tables(0).Rows(0)("LowestPrice") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("LowestPrice")) > 0 Then
                                txtPODPrice.Text = CDbl(ds.Tables(0).Rows(0)("LowestPrice").ToString()).ToString("0.00")
                            Else
                                txtPODPrice.Text = 0.0
                            End If
                           
                            If CInt(ds.Tables(0).Rows(0)("Category").ToString()) > 0 Then
                                ddlCategory.SelectedValue = CInt(ds.Tables(0).Rows(0)("Category").ToString())
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
End Class
