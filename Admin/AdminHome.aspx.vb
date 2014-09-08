Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Imports System.Data.SqlClient
Partial Class Admin_AdminHome
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Public str As String = String.Empty
    Public ds As DataSet = Nothing
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("Id") Is Nothing Then
            nUserID = CInt(Session("Id").ToString())
            If Not Page.IsPostBack Then
                fill_HomeHit()
                fill_HomeItemValue()
                fill_HomeItemValueOnHold()
                showTotalProduct()
            End If
        Else
            Response.Redirect("Login.aspx")
        End If
    End Sub
    Public Sub fill_HomeHit()
        Try
            str = " SELECT Isnull(COUNT(p.Id),0) Total FROM ProductHit P WHERE p.IsHome = 1 "
            ds = SQLData.generic_select(str, strConnection)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    lblHomeHitcount.InnerHtml = ds.Tables(0).Rows(0)(0).ToString()
                End If
            End If
        Catch ex As Exception

        End Try
        
    End Sub
    Public Sub fill_HomeItemValue()
        Try
            ' str = " SELECT ISNULL(COUNT(p.Id),0) TotalItem, ISNULL(SUM(p.Price),0) TotalValue FROM Product p WHERE (p.IsNotOnWeb = 0 OR p.IsNotOnWeb IS NULL OR p.IsDoNotRelease IS null OR p.IsDoNotRelease = 0) AND p.IsLabX = 0 AND p.IsDeleteItem <> 1 "

            str = " SELECT ISNULL(sum(p.Qty-p.QuantitySold),0) TotalItem, ISNULL(sum((p.Qty-p.QuantitySold)*p.Price),0) TotalValue, ISNULL(SUM((p.Qty-p.QuantitySold)*p.LowestPrice),0) TotalLowestValue, ISNULL(SUM((p.Qty-p.QuantitySold)*p.CostofGoods),0) TotalCostOfGoods FROM Product p WHERE (p.IsNotOnWeb = 0 OR p.IsNotOnWeb IS NULL OR p.IsDoNotRelease IS null OR p.IsDoNotRelease = 0) AND p.IsLabX = 0 AND p.IsDeleteItem <> 1 "
            ds = SQLData.generic_select(str, strConnection)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If Not ds.Tables(0).Rows(0)(0).ToString() Is Nothing And CInt(ds.Tables(0).Rows(0)(0).ToString()) > 0 Then
                        lblTotalItem.InnerHtml = ds.Tables(0).Rows(0)(0).ToString()
                    Else
                        lblTotalItem.InnerHtml = "$0.00"
                    End If
                    If Not ds.Tables(0).Rows(0)(1).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(0).ToString()) > 0 Then
                        lblTotalItemValue.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(1).ToString()).ToString("#,##0.00")
                    Else
                        lblTotalItemValue.InnerHtml = "$0.00"
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
       
    End Sub
    Public Sub fill_HomeItemValueOnHold()
        Try
            '  str = " SELECT ISNULL(COUNT(p.Id),0) TotalItem, ISNULL(SUM(p.Price),0) TotalValue FROM Product p WHERE (p.IsNotOnWeb = 1  OR p.IsDoNotRelease = 1) AND p.IsLabX = 0 AND p.IsDeleteItem <> 1 "
            str = " SELECT ISNULL(sum(p.Qty-p.QuantitySold),0) TotalItem, ISNULL(sum((p.Qty-p.QuantitySold)*p.Price),0) TotalValue, ISNULL(SUM((p.Qty-p.QuantitySold)*p.LowestPrice),0) TotalLowestValue, ISNULL(SUM((p.Qty-p.QuantitySold)*p.CostofGoods),0) TotalCostOfGoods FROM Product p WHERE (p.IsNotOnWeb = 1  OR p.IsDoNotRelease = 1) AND p.IsLabX = 0 AND p.IsDeleteItem <> 1 "

            ds = SQLData.generic_select(str, strConnection)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If Not ds.Tables(0).Rows(0)(1).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(0).ToString()) > 0 Then
                        lblTotalItemValueOnHold.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(1).ToString()).ToString("#,##0.00")
                    Else
                        lblTotalItemValueOnHold.InnerHtml = "0"
                    End If
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            Dim strSearch As String = ""
            Dim sId As String = ""
            strSearch = txtSearch.Text.ToString().Trim
            If strSearch.Length > 0 Then
                Try
                    'add it
                    Dim sIns As String = "INSERT INTO SearchHistory ([SearchTerm],[CreatedDate]) VALUES('" & strSearch.Trim & "','" & CDate(DateTime.UtcNow) & "')"
                    SQLData.generic_command(sIns, SQLData.ConnectionString)
                Catch ex As Exception

                End Try

                If rdoEquipmentList.SelectedIndex <> -1 Then
                    If rdoEquipmentList.Items(0).Selected = True Then
                        Response.Redirect("../Admin/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                    If rdoEquipmentList.Items(1).Selected = True Then
                        Response.Redirect("../Admin/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                    If rdoEquipmentList.Items(2).Selected = True Then
                        Response.Redirect("../Admin/ProductListing.aspx?sc_state=3&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                    If rdoEquipmentList.Items(3).Selected = True Then
                        Response.Redirect("../Admin/ProductListing.aspx?sc_state=4&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                    If rdoEquipmentList.Items(4).Selected = True Then
                        Response.Redirect("../Admin/ProductListing.aspx?sc_state=5&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                    If rdoEquipmentList.Items(5).Selected = True Then
                        Response.Redirect("../Admin/ProductListing.aspx?sc_state=2&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                    If rdoEquipmentList.Items(6).Selected = True Then
                        Response.Redirect("../Admin/ProductListing.aspx?sc_state=7&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                    If rdoEquipmentList.Items(7).Selected = True Then
                        Response.Redirect("../Admin/ProductListing.aspx?sc_state=8&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                    If rdoEquipmentList.Items(8).Selected = True Then
                        Response.Redirect("../Admin/ProductListing.aspx?sc_state=6&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                    If rdoEquipmentList.Items(9).Selected = True Then
                        Response.Redirect("../Admin/ProductListing.aspx?sc_state=11&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                End If
            Else
                DisplayAlert("Please Enter Text to Search")
            End If
           
        Catch ex As Exception

        End Try
    End Sub
    Public Function ShowTotal(ByVal sql As String) As String
        Dim UserDS As New DataSet
        Dim strTotal As String = String.Empty
        Try
            UserDS = SQLData.generic_select(sql, strConnection)
            If Not UserDS Is Nothing Then
                If UserDS.Tables(0).Rows.Count > 0 Then
                    strTotal = "(" & CInt(UserDS.Tables(0).Rows.Count) & ")"
                    Return strTotal
                Else
                    Return strTotal
                End If
            Else
                Return strTotal
            End If
        Catch ex As Exception

        End Try

        Return strTotal
    End Function
    Public Sub showTotalProduct()
        Try
            Dim strQuery As String = String.Empty

            strQuery = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsLabX,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 "
            lblTotal.Text = ShowTotal(strQuery)

            strQuery = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsLabX,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsNewArrivalsPage = 1  "
            lblNew.Text = ShowTotal(strQuery)

            strQuery = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsLabX,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsConsignmentItem = 1  "
            lblConsignment.Text = ShowTotal(strQuery)

            strQuery = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsLabX,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsJustOfftheTruck = 1  "
            lblOffTheTrack.Text = ShowTotal(strQuery)

            strQuery = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsLabX,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsLabX = 1  "
            lblThirdParty.Text = ShowTotal(strQuery)

            strQuery = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsLabX,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsDoNotRelease = 1  "
            lblDoNotRelease.Text = ShowTotal(strQuery)

            strQuery = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsLabX,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsFeaturedItem = 1  "
            lblFeatured.Text = ShowTotal(strQuery)

            strQuery = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsLabX,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsSold = 1  "
            lblSold.Text = ShowTotal(strQuery)

            strQuery = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsLabX,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsDeleteItem = 1  "
            lblDelete.Text = ShowTotal(strQuery)

            strQuery = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsLabX,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsSold = 1  "
            lblArchiveSold.Text = ShowTotal(strQuery)

            strQuery = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsLabX,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsDeletePermanently = 1  "
            lblArchiveDelete.Text = ShowTotal(strQuery)


        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnManual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnManual.Click
        Try
            Dim strSearch As String = ""
            Dim sId As String = ""
            strSearch = txtManufactSearch.Text.ToString().Trim
            If strSearch.Length > 0 Then
                If rdoManufactSearch.SelectedIndex <> -1 Then
                    If rdoManufactSearch.Items(0).Selected = True Then
                        Response.Redirect("../Admin/ManualProductListing.aspx?sc_state=0&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                    If rdoManufactSearch.Items(1).Selected = True Then
                        Response.Redirect("../Admin/ManualProductListing.aspx?sc_state=1&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                    If rdoManufactSearch.Items(2).Selected = True Then
                        Response.Redirect("../Admin/ManualProductListing.aspx?sc_state=2&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                    If rdoManufactSearch.Items(3).Selected = True Then
                        Response.Redirect("../Admin/ManualProductListing.aspx?sc_state=3&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
                    End If
                End If
            Else
                DisplayAlert("Please Enter Text to Search")
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
