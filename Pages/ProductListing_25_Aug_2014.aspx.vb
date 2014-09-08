Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Imports System.Data.SqlClient
Partial Class Pages_ProductListing
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Dim sSQl As String = ""
    Dim ProductCategoryId As Integer = 0
    Dim ProductStateId As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Public str As String = String.Empty
    Public strWhere As String = String.Empty
    Public sOrderBy As String = String.Empty
    Public strNoteList As String = String.Empty
    Public sSearch As String = String.Empty
    Public sql As String = String.Empty
    Private strImageFilePath As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageFilePath")
    Private strImageURL As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageURL")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            sSearch = CInt(Request.QueryString("sc_search").ToString())
        Catch ex As Exception
            sSearch = ""
        End Try
        Try
            ProductCategoryId = CInt(Request.QueryString("sc_cat").ToString())
        Catch ex As Exception
            ProductCategoryId = 0
        End Try

        Try
            ProductStateId = CInt(Request.QueryString("sc_state").ToString())
        Catch ex As Exception
            ProductStateId = 0
        End Try
        If Not Page.IsPostBack Then
            Session("sSQL") = Nothing
            Session("IsASC") = Nothing
            Session("pagerSQL") = Nothing
            Session("sStr") = Nothing
            Session("sCat") = Nothing
            Session("strOrderValue") = Nothing
            Session("strOrder") = Nothing
            Session("strSearch") = Nothing

            If ProductCategoryId > 0 Then
                Session("sCat") = ProductCategoryId
                Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0  and (c.CategoryParentId = " & ProductCategoryId & " OR c.Id = " & ProductCategoryId & ")"
                If Not Session("strOrder") Is Nothing Then
                    sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                    SqlDataSource1.SelectCommand = sSQl
                Else
                    SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                End If
                Pager.PageSize = ddlPageSize.SelectedValue

                Try
                    lblCategory.InnerHtml = ShowCategoryById(ProductCategoryId)
                Catch ex As Exception
                    lblCategory.InnerHtml = ""
                End Try
            Else
                If sSearch.Length > 0 Then
                    strWhere = " and  (c.CategoryName like '%" & sSearch.Trim & "%'  or p.ItemNumber like '%" & sSearch.Trim & "%' or  p.ProductName like '%" & sSearch.Trim & "%'  or  p.Make like '%" & sSearch.Trim & "%'  or  p.Model like '%" & sSearch.Trim & "%'  or  p.Description like '%" & sSearch.Trim & "%'  or  p.Location like '%" & sSearch.Trim & "%'  or  p.Barcode like '%" & sSearch.Trim & "%'  or  p.BarcodeParent like '%" & sSearch.Trim & "%' or  p.AdminNotes like '%" & sSearch.Trim & "%')"
                End If
                If ProductStateId = 0 Then
                    lblCategory.InnerHtml = ""
                    Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 " & strWhere.Trim
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If
                    Pager.PageSize = ddlPageSize.SelectedValue
                ElseIf ProductStateId = 1 Then
                    lblCategory.InnerHtml = "New Arrivals"
                    Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsNewArrivalsPage = 1  " & strWhere.Trim
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If
                    Pager.PageSize = ddlPageSize.SelectedValue
                ElseIf ProductStateId = 2 Then
                    lblCategory.InnerHtml = "On Consignment"
                    Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsConsignmentItem = 1  " & strWhere.Trim
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If
                    Pager.PageSize = ddlPageSize.SelectedValue
                ElseIf ProductStateId = 3 Then
                    lblCategory.InnerHtml = "Just Off the Truck"
                    Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsJustOfftheTruck = 1  " & strWhere.Trim
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If
                    Pager.PageSize = ddlPageSize.SelectedValue
                ElseIf ProductStateId = 4 Then
                    lblCategory.InnerHtml = "Not On Web"
                    Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsLabX = 1  " & strWhere.Trim
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If
                    Pager.PageSize = ddlPageSize.SelectedValue
                ElseIf ProductStateId = 5 Then
                    lblCategory.InnerHtml = "Marked Do Not Release"
                    Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsDoNotRelease = 1  " & strWhere.Trim
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If
                    Pager.PageSize = ddlPageSize.SelectedValue
                ElseIf ProductStateId = 6 Then
                    lblCategory.InnerHtml = "Marked Featured Items"
                    Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsFeaturedItem = 1  " & strWhere.Trim
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If
                    Pager.PageSize = ddlPageSize.SelectedValue
                ElseIf ProductStateId = 7 Then
                    lblCategory.InnerHtml = "Recently Sold"
                    Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsSold = 1  " & strWhere.Trim
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If
                    Pager.PageSize = ddlPageSize.SelectedValue
                ElseIf ProductStateId = 8 Then
                    lblCategory.InnerHtml = "Deleted Items"
                    Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsDeleteItem = 1  " & strWhere.Trim
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If
                    Pager.PageSize = ddlPageSize.SelectedValue
                ElseIf ProductStateId = 9 Then
                    lblCategory.InnerHtml = "Archived Sold"
                    Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsSold = 1  " & strWhere.Trim
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If
                    Pager.PageSize = ddlPageSize.SelectedValue
                ElseIf ProductStateId = 10 Then
                    lblCategory.InnerHtml = "Archived Deleted"
                    Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsDeletePermanently = 1  " & strWhere.Trim
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If
                    Pager.PageSize = ddlPageSize.SelectedValue
                ElseIf ProductStateId = 11 Then
                    lblCategory.InnerHtml = "Specials"
                    Session("pagerSQL") = " SELECT p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'') FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and p.IsSpecial = 1  " & strWhere.Trim
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by p.ProductName asc "
                    End If
                    Pager.PageSize = ddlPageSize.SelectedValue
                End If
                If sSearch.Length > 0 Then
                    lblCategory.InnerHtml = "<h1 class='pagetitle'>Search Inventory - Results</h1> <br /><br /> <strong>Search term:</strong> " & sSearch.Trim
                    spanHelp.InnerHtml = "<a href='../Pages/Help.aspx'>Search Help</a>"
                End If
            End If
        Else
            If Not Session("pagerSQL") Is Nothing Then
                If Not Session("strOrder") Is Nothing Then
                    sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                    SqlDataSource1.SelectCommand = sSQl
                Else
                    SqlDataSource1.SelectCommand = Session("pagerSQL") & " p.ProductName asc "
                End If
                Pager.PageSize = ddlPageSize.SelectedValue
            End If
        End If
    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub

    Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        Try
            SqlDataSource1.SelectCommand = Session("pagerSQL")
            Pager.PageSize = ddlPageSize.SelectedValue
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub InventoryItems_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles InventoryItems.ItemCommand

    End Sub

    Public Function ImageName(ByVal nId As Integer) As String
        Dim sImageName As String = ""
        Try
            Dim strSQL As String = ""
            Dim UserDS As New DataSet
            Dim strImg As String = ""
            strSQL = "select * from Product where Id=" & nId
            UserDS = SQLData.generic_select(strSQL, strConnection)
            If Not UserDS Is Nothing And UserDS.Tables(0).Rows.Count > 0 Then
                If Not UserDS.Tables(0).Rows(0)("MainImage").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("MainImage").ToString() <> String.Empty Then
                    strImg = UserDS.Tables(0).Rows(0)("MainImage").ToString()
                    Dim imgArray() As String = Nothing
                    Dim sStr As String = ""
                    Dim sStrFile As String = ""
                    If strImg.Length > 0 Then
                        If Not String.IsNullOrEmpty(strImageFilePath) Then
                            sStrFile = strImageFilePath & "\" & strImg
                        Else
                            sStrFile = "C:\inetpub\wwwroot\sysadmin\products\Images\" & strImg
                        End If
                        If Not String.IsNullOrEmpty(strImageURL) Then
                            sStr = strImageURL & "/" & strImg
                        Else
                            sStr = strImageURL & "/not_found_image.jpg"
                        End If

                        If System.IO.File.Exists(sStrFile) Then
                            sImageName = sStr
                        Else
                            sImageName = strImageURL & "/not_found_image.jpg"
                        End If
                    Else
                        sImageName = strImageURL & "/not_found_image.jpg"
                    End If
                Else
                    sImageName = strImageURL & "/not_found_image.jpg"
                End If
            End If
        Catch ex As Exception
            sImageName = strImageURL & "/not_found_image.jpg"
        End Try
        Return sImageName
    End Function
    Public Function IsExists(ByVal nUserId As Integer, ByVal nLoanId As Integer) As Boolean
        Dim UserDS As New DataSet
        Dim sStrNew As String = ""
        Try
            sStrNew = "select * from NoteOrder where  UserID  = " & nUserId & "  and LoanID = " & nLoanId
            UserDS = SQLData.generic_select(sStrNew, strConnection)

            If Not UserDS Is Nothing Then
                If UserDS.Tables(0).Rows.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception

        End Try

        Return True
    End Function


    Public Function NoteColor(Optional ByVal nId As Integer = 0) As String
        Dim sSpan As String = ""
        If nId > 0 Then
            If IsExists(CInt(Session("ID").ToString()), nId) Then
                sSpan = "<span style='float: right; font-weight:bold; font-size:30px; margin-top:5px; color: white;'>*</span>"
            End If

        End If
        Return sSpan
    End Function


    Protected Sub btnBarcode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBarcode.Click
        sOrderBy = String.Empty
        sSQl = String.Empty

        Try
            If Not Session("strOrderValue") Is Nothing Then
                If Session("strOrderValue").ToString() = "1" Then
                    Session("strOrderValue") = 2
                    sOrderBy = " order by  p.Barcode desc "
                Else
                    Session("strOrderValue") = 1
                    sOrderBy = " order by  p.Barcode asc "
                End If
            Else
                Session("strOrderValue") = 1
                sOrderBy = " order by  p.Barcode asc "
            End If

            Session("strOrder") = sOrderBy.ToString()

            If Not Session("pagerSQL") Is Nothing Then
                sSQl = Session("pagerSQL") & sOrderBy.ToString()
                SqlDataSource1.SelectCommand = sSQl
                Pager.PageSize = ddlPageSize.SelectedValue
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDate.Click
        sOrderBy = String.Empty
        sSQl = String.Empty

        Try
            If Not Session("strOrderValue") Is Nothing Then
                If Session("strOrderValue").ToString() = "1" Then
                    Session("strOrderValue") = 2
                    sOrderBy = " order by  p.DateCreated desc "
                Else
                    Session("strOrderValue") = 1
                    sOrderBy = " order by  p.DateCreated asc "
                End If
            Else
                Session("strOrderValue") = 1
                sOrderBy = " order by  p.DateCreated asc "
            End If

            Session("strOrder") = sOrderBy.ToString()

            If Not Session("pagerSQL") Is Nothing Then
                sSQl = Session("pagerSQL") & sOrderBy.ToString()
                SqlDataSource1.SelectCommand = sSQl
                Pager.PageSize = ddlPageSize.SelectedValue
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnItemNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnItemNo.Click
        sOrderBy = String.Empty
        sSQl = String.Empty

        Try
            If Not Session("strOrderValue") Is Nothing Then
                If Session("strOrderValue").ToString() = "1" Then
                    Session("strOrderValue") = 2
                    sOrderBy = " order by  p.ItemNumber desc "
                Else
                    Session("strOrderValue") = 1
                    sOrderBy = " order by  p.ItemNumber asc "
                End If
            Else
                Session("strOrderValue") = 1
                sOrderBy = " order by  p.ItemNumber asc "
            End If

            Session("strOrder") = sOrderBy.ToString()

            If Not Session("pagerSQL") Is Nothing Then
                sSQl = Session("pagerSQL") & sOrderBy.ToString()
                SqlDataSource1.SelectCommand = sSQl
                Pager.PageSize = ddlPageSize.SelectedValue
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnLocation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocation.Click
        sOrderBy = String.Empty
        sSQl = String.Empty

        Try
            If Not Session("strOrderValue") Is Nothing Then
                If Session("strOrderValue").ToString() = "1" Then
                    Session("strOrderValue") = 2
                    sOrderBy = " order by  p.Location desc "
                Else
                    Session("strOrderValue") = 1
                    sOrderBy = " order by  p.Location asc "
                End If
            Else
                Session("strOrderValue") = 1
                sOrderBy = " order by  p.Location asc "
            End If

            Session("strOrder") = sOrderBy.ToString()

            If Not Session("pagerSQL") Is Nothing Then
                sSQl = Session("pagerSQL") & sOrderBy.ToString()
                SqlDataSource1.SelectCommand = sSQl
                Pager.PageSize = ddlPageSize.SelectedValue
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnManufacturer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnManufacturer.Click
        sOrderBy = String.Empty
        sSQl = String.Empty

        Try
            If Not Session("strOrderValue") Is Nothing Then
                If Session("strOrderValue").ToString() = "1" Then
                    Session("strOrderValue") = 2
                    sOrderBy = " order by  p.Make desc "
                Else
                    Session("strOrderValue") = 1
                    sOrderBy = " order by p.Make asc "
                End If
            Else
                Session("strOrderValue") = 1
                sOrderBy = " order by  p.Make asc "
            End If

            Session("strOrder") = sOrderBy.ToString()

            If Not Session("pagerSQL") Is Nothing Then
                sSQl = Session("pagerSQL") & sOrderBy.ToString()
                SqlDataSource1.SelectCommand = sSQl
                Pager.PageSize = ddlPageSize.SelectedValue
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnModel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModel.Click
        sOrderBy = String.Empty
        sSQl = String.Empty

        Try
            If Not Session("strOrderValue") Is Nothing Then
                If Session("strOrderValue").ToString() = "1" Then
                    Session("strOrderValue") = 2
                    sOrderBy = " order by  p.Model desc "
                Else
                    Session("strOrderValue") = 1
                    sOrderBy = " order by  p.Model asc "
                End If
            Else
                Session("strOrderValue") = 1
                sOrderBy = " order by  p.Model asc "
            End If

            Session("strOrder") = sOrderBy.ToString()

            If Not Session("pagerSQL") Is Nothing Then
                sSQl = Session("pagerSQL") & sOrderBy.ToString()
                SqlDataSource1.SelectCommand = sSQl
                Pager.PageSize = ddlPageSize.SelectedValue
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function ShowDescription(ByVal Id As Integer, Optional ByVal sDesc As String = "") As String
        Dim strDesc As String = String.Empty
        Try
            If Not sDesc Is Nothing Then
                If sDesc.Length > 200 Then
                    strDesc = sDesc.Trim & " ... " & "<a title='" & sDesc.Trim & "' href='ProductDetails.aspx?Id=" & Id & "'>More Info</a>"
                Else
                    strDesc = sDesc.Trim
                End If
            End If

        Catch ex As Exception
            Return strDesc
        End Try

        Return strDesc
    End Function

    Public Function ShowVideo(ByVal Id As Integer, ByVal sTitle As String, Optional ByVal sVideoName As String = "") As String
        Dim strVideo As String = String.Empty
        Try
            If Not sVideoName Is Nothing Then
                If sVideoName.Length > 0 Then
                    'strVideo = "<div style='vertical-align: middle; width:80px; margin-left:70px;'><a title='Video for: " & sTitle.Trim & "' href='ProductDetails.aspx?Id=" & Id & "'><img width='20' height='20' border='0' align='left' style='padding-right: 0.2em' alt='See a Video of the Item' src='../ProductImages/Large/videoicon.gif'>See Video</a></div>"
                    strVideo = "<div style='vertical-align: middle; width:80px; margin-left:70px;'><a title='Video for: " & sTitle.Trim & "' href='" & sVideoName.Trim & "' target='_blank'><img width='20' height='20' border='0' align='left' style='padding-right: 0.2em' alt='See a Video of the Item' src='../ProductImages/Large/videoicon.gif'>See Video</a></div>"

                End If
            End If
            If strVideo.Length > 0 Then
                Return strVideo
            End If
        Catch ex As Exception
            Return ""
        End Try

        Return ""
    End Function
    Public Function ShowCategory(ByVal Id As Integer, ByVal sTitle As String, ByVal pId As Integer, ByVal sPTitle As String) As String
        Dim str As String = String.Empty

        Try
            If sPTitle.Length > 0 Then
                If sTitle.Length > 0 Then
                    str = "Category: <a title='" & sPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & pId & "'>" & sPTitle & "</a> "
                    str &= " >> <a title='" & sTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & Id & "'>" & sTitle & "</a> "
                Else
                    str = "Category: <a title='" & sPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & pId & "'>" & sPTitle & "</a> "
                End If
            End If
        Catch ex As Exception
            Return str
        End Try

        Return str
    End Function
    'Public Function ShowCategoryById(ByVal Id As Integer) As String
    '    Dim UserDS As New DataSet
    '    Dim sStrNew As String = ""
    '    Dim sTitle As String = String.Empty

    '    Try
    '        sStrNew = "select * from Category where Id = " & Id
    '        UserDS = SQLData.generic_select(sStrNew, strConnection)

    '        If Not UserDS Is Nothing Then
    '            If UserDS.Tables(0).Rows.Count > 0 Then
    '                If Not UserDS.Tables(0).Rows(0)("CategoryName").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("CategoryName").ToString() <> String.Empty Then
    '                    sTitle = " <h1 class='pagetitle'><a href='../Pages/InventoryList.aspx'>Inventory List</a> >> " & UserDS.Tables(0).Rows(0)("CategoryName").ToString() & "</h1>"
    '                Else
    '                    sTitle = ""
    '                End If
    '            Else
    '                sTitle = ""
    '            End If
    '        Else
    '            sTitle = ""
    '        End If
    '    Catch ex As Exception
    '        sTitle = ""
    '    End Try


    '    Return sTitle
    'End Function
    Public Function ShowCategoryById(ByVal Id As Integer) As String
        Dim UserDS As New DataSet
        Dim sStrNew As String = ""
        Dim sTitle As String = String.Empty

        Try
            sStrNew = "SELECT c.CategoryName, Parent= (SELECT CategoryName FROM Category c2 WHERE c2.Id= c.CategoryParentId) FROM Category c WHERE c.Id =  " & Id
            UserDS = SQLData.generic_select(sStrNew, strConnection)

            If Not UserDS Is Nothing Then
                If UserDS.Tables(0).Rows.Count > 0 Then
                    If Not UserDS.Tables(0).Rows(0)("Parent").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("Parent").ToString() <> String.Empty Then
                        If Not UserDS.Tables(0).Rows(0)("CategoryName").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("CategoryName").ToString() <> String.Empty Then
                            sTitle = " <h1 class='pagetitle'><a href='../Admin/InventoryList.aspx'>Inventory List</a>&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("Parent").ToString() & "&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("CategoryName").ToString() & "</h1>"
                        Else
                            sTitle = ""
                        End If
                    Else
                        If Not UserDS.Tables(0).Rows(0)("CategoryName").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("CategoryName").ToString() <> String.Empty Then
                            sTitle = " <h1 class='pagetitle'><a href='../Admin/InventoryList.aspx'>Inventory List</a>&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("CategoryName").ToString() & "</h1>"
                        Else
                            sTitle = ""
                        End If
                    End If


                Else
                    sTitle = ""
                End If
            Else
                sTitle = ""
            End If
        Catch ex As Exception
            sTitle = ""
        End Try


        Return sTitle
    End Function
    Public Function ShowPrice(Optional ByVal Price As Double = 0) As String
        Dim strPrice As String = String.Empty
        Try
            If Price > 0 Then
                strPrice = "<span class='ListingListPrice'>Price: " & Price.ToString("#,##0.00") & "</span>&nbsp;&nbsp;&nbsp;<span class='ListingPODPrice'> POD<sup>&trade;</sup> Price:</span><span style='font-weight: normal;' class='ListingListPrice'>" & Price.ToString("#,##0.00") & "</span>"
            Else
                strPrice = "<span class='ListingListPrice'>Price: Please Call</span>"
            End If

        Catch ex As Exception
            Return strPrice
        End Try

        Return strPrice
    End Function
    Public Function ShowBarCode(ByVal Id As Integer, ByVal Barcode As String, Optional ByVal BarcodeParent As String = "") As String
        Dim strBarcode As String = String.Empty
        Try
            If Not Barcode Is Nothing And Barcode.Length > 0 Then
                If Not BarcodeParent Is Nothing And BarcodeParent.Length > 0 Then
                    strBarcode = Barcode.Trim & " child of " & BarcodeParent.Trim
                Else
                    strBarcode = Barcode.Trim
                End If
            Else
                strBarcode = ""
            End If

        Catch ex As Exception
            Return strBarcode
        End Try

        Return strBarcode
    End Function
    Public Function ShowNotOnWeb(Optional ByVal nIsNotOnWeb As Integer = 0) As String
        Dim str As String = String.Empty
        Try
            If nIsNotOnWeb = 1 Then
                str = " Not On Web "
            Else
                str = ""
            End If

        Catch ex As Exception
            Return str
        End Try

        Return str
    End Function
    Public Function ShowConsignment(Optional ByVal nIsConsignment As Integer = 0) As String
        Dim str As String = String.Empty
        Try
            If nIsConsignment = 1 Then
                str = " On Consignment"
            Else
                str = ""
            End If

        Catch ex As Exception
            Return str
        End Try

        Return str
    End Function
    Public Function ShowColor(ByVal Location As String, Optional ByVal Barcode As String = "") As String
        Dim strColor As String = String.Empty
        Dim bLocation As Boolean = False
        Dim bBarcode As Boolean = False

        Try
            If Not Location Is Nothing And Location.Length > 0 Then
                bLocation = True
            End If
            If Not Barcode Is Nothing And Barcode.Length > 0 Then
                bBarcode = True
            End If
        Catch ex As Exception

        End Try
        If bLocation = True And bBarcode = True Then
            strColor = "color:#000;"
        ElseIf bLocation = True And bBarcode = False Then
            strColor = "background-color: #9f9fff; color: #000066;"
        ElseIf bLocation = False And bBarcode = True Then
            strColor = "background-color: #9fff9f; color: #006600;"
        ElseIf bLocation = False And bBarcode = False Then
            strColor = "background-color: #ff9f9f; color: #990000;"
        End If
        Return strColor
    End Function
End Class
