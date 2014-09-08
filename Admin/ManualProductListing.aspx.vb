Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Imports System.Data.SqlClient
Partial Class Admin_ManualProductListing
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Dim sSQl As String = ""
    Dim ProductCategoryId As Integer = 0
    Dim ProductStateId As Integer = 0
    Dim nUserID As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Public str As String = String.Empty
    Public strWhere As String = String.Empty
    Public sOrderBy As String = String.Empty
    Public strNoteList As String = String.Empty
    Public sSearch As String = String.Empty
    Public sParent As String = String.Empty
    Public sql As String = String.Empty
    Private strImageFilePath As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageFilePath")
    Private strImageURL As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageURL")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("Id") Is Nothing Then
            nUserID = CInt(Session("Id").ToString())
            Try
                sSearch = Request.QueryString("sc_search").ToString()
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
                    Session("pagerSQL") = " SELECT p.*, c.Id CatId, c.CategoryName, Assoc=ISNULL((SELECT COUNT(*) FROM ManualProduct mp WHERE mp.ItemNumber = p.ManualItemNo),0), Files=ISNULL((SELECT COUNT(*) FROM ManualProductImageCrossRef mpicr WHERE mpicr.ProductId = p.Id),0), Parent = (SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = p.ParentCategory)  FROM ManualProduct  p, Category c WHERE p.Category = c.Id AND   c.IsMainorLabX = 0 and p.IsDeleteItem <> 1 AND p.CreatorID =  " & CInt(Session("Id").ToString()) & "  and (p.Category = " & ProductCategoryId & "  or p.ParentCategory = " & ProductCategoryId & " )"
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If

                    Try
                        lblSearch.InnerHtml = ShowCategoryById(ProductCategoryId)
                    Catch ex As Exception
                        lblSearch.InnerHtml = ""
                    End Try
                Else
                    If sSearch.Length > 0 Then
                        strWhere = " and  (c.CategoryName like '%" & sSearch.Trim & "%'  or p.ItemNumber like '%" & sSearch.Trim & "%' or  p.ProductName like '%" & sSearch.Trim & "%'  or  p.Make like '%" & sSearch.Trim & "%'  or  p.Model like '%" & sSearch.Trim & "%'  or  p.Description like '%" & sSearch.Trim & "%'  or  p.Location like '%" & sSearch.Trim & "%')"
                    End If
                    If ProductStateId = 0 Then
                        Session("pagerSQL") = " SELECT p.*, c.Id CatId, c.CategoryName, Assoc=ISNULL((SELECT COUNT(*) FROM ManualProduct mp WHERE mp.ItemNumber = p.ManualItemNo),0), Files=ISNULL((SELECT COUNT(*) FROM ManualProductImageCrossRef mpicr WHERE mpicr.ProductId = p.Id),0), Parent = (SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = p.ParentCategory)  FROM ManualProduct  p, Category c WHERE p.Category = c.Id AND   c.IsMainorLabX = 0 and p.IsDeleteItem <> 1 AND p.CreatorID =  " & CInt(Session("Id").ToString()) & strWhere.Trim
                        If Not Session("strOrder") Is Nothing Then
                            sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                            SqlDataSource1.SelectCommand = sSQl
                        Else
                            SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                        End If
                    ElseIf ProductStateId = 1 Then
                        Session("pagerSQL") = " SELECT p.*, c.Id CatId, c.CategoryName, Assoc=ISNULL((SELECT COUNT(*) FROM ManualProduct mp WHERE mp.ItemNumber = p.ManualItemNo),0), Files=ISNULL((SELECT COUNT(*) FROM ManualProductImageCrossRef mpicr WHERE mpicr.ProductId = p.Id),0), Parent = (SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = p.ParentCategory)  FROM ManualProduct  p, Category c WHERE p.Category = c.Id AND   c.IsMainorLabX = 0 and p.IsDeleteItem <> 1 AND p.CreatorID =  " & CInt(Session("Id").ToString()) & strWhere.Trim
                        If Not Session("strOrder") Is Nothing Then
                            sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                            SqlDataSource1.SelectCommand = sSQl
                        Else
                            SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                        End If
                    ElseIf ProductStateId = 2 Then
                        Session("pagerSQL") = " SELECT p.*, c.Id CatId, c.CategoryName, Assoc=ISNULL((SELECT COUNT(*) FROM ManualProduct mp WHERE mp.ItemNumber = p.ManualItemNo),0), Files=ISNULL((SELECT COUNT(*) FROM ManualProductImageCrossRef mpicr WHERE mpicr.ProductId = p.Id),0), Parent = (SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = p.ParentCategory)  FROM ManualProduct  p, Category c WHERE p.Category = c.Id AND   c.IsMainorLabX = 0 and p.IsDeleteItem <> 1 and p.IsHold = 1  AND p.CreatorID =  " & CInt(Session("Id").ToString()) & strWhere.Trim
                        If Not Session("strOrder") Is Nothing Then
                            sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                            SqlDataSource1.SelectCommand = sSQl
                        Else
                            SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                        End If
                    ElseIf ProductStateId = 3 Then
                        Session("pagerSQL") = " SELECT p.*, c.Id CatId, c.CategoryName, Assoc=ISNULL((SELECT COUNT(*) FROM ManualProduct mp WHERE mp.ItemNumber = p.ManualItemNo),0), Files=ISNULL((SELECT COUNT(*) FROM ManualProductImageCrossRef mpicr WHERE mpicr.ProductId = p.Id),0), Parent = (SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = p.ParentCategory)  FROM ManualProduct  p, Category c WHERE p.Category = c.Id AND   c.IsMainorLabX = 0 and p.IsDeleteItem = 1  AND p.CreatorID =  " & CInt(Session("Id").ToString()) & strWhere.Trim
                        If Not Session("strOrder") Is Nothing Then
                            sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                            SqlDataSource1.SelectCommand = sSQl
                        Else
                            SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                        End If
                    End If

                    If sSearch.Length > 0 Then
                        lblCategory.InnerHtml = "Search Inventory - Results "
                        lblSearch.InnerHtml = "<strong>Search : </strong> " & sSearch.Trim
                        spanHelp.InnerHtml = "<a href='../Pages/Help.aspx'>Search Help</a>"
                    End If
                End If
            Else
                If Not Session("pagerSQL") Is Nothing Then
                    If Not Session("strOrder") Is Nothing Then
                        sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                        SqlDataSource1.SelectCommand = sSQl
                    Else
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                    End If
                End If
            End If
        Else
            Response.Redirect("Login.aspx")
        End If

    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
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
                    sOrderBy = " order by p.ItemNumber asc "
                End If
            Else
                Session("strOrderValue") = 1
                sOrderBy = " order by p.ItemNumber asc "
            End If

            Session("strOrder") = sOrderBy.ToString()

            If Not Session("pagerSQL") Is Nothing Then
                sSQl = Session("pagerSQL") & sOrderBy.ToString()
                SqlDataSource1.SelectCommand = sSQl
                ' Pager.PageSize = ddlPageSize.SelectedValue
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
                    sOrderBy = " order by p.Make desc "
                Else
                    Session("strOrderValue") = 1
                    sOrderBy = " order by  p.Make asc "
                End If
            Else
                Session("strOrderValue") = 1
                sOrderBy = " order by  p.Make asc "
            End If

            Session("strOrder") = sOrderBy.ToString()

            If Not Session("pagerSQL") Is Nothing Then
                sSQl = Session("pagerSQL") & sOrderBy.ToString()
                SqlDataSource1.SelectCommand = sSQl
                '  Pager.PageSize = ddlPageSize.SelectedValue
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
                    sOrderBy = " order by p.Model desc "
                Else
                    Session("strOrderValue") = 1
                    sOrderBy = " order by p.Model asc "
                End If
            Else
                Session("strOrderValue") = 1
                sOrderBy = " order by  p.Model asc "
            End If

            Session("strOrder") = sOrderBy.ToString()

            If Not Session("pagerSQL") Is Nothing Then
                sSQl = Session("pagerSQL") & sOrderBy.ToString()
                SqlDataSource1.SelectCommand = sSQl
                ' Pager.PageSize = ddlPageSize.SelectedValue
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function ShowDescription(ByVal Id As Integer, Optional ByVal sDesc As String = "") As String
        Dim strDesc As String = String.Empty
        Try
            If Not sDesc Is Nothing Then
                If sDesc.Length > 200 Then
                    strDesc = sDesc.Trim & " ... " & "<a title='" & sDesc.Trim & "' href='../Admin/AddNewManualItem.aspx?PID=" & Id & "'>More Info</a>"
                Else
                    strDesc = sDesc.Trim
                End If
            End If

        Catch ex As Exception
            Return strDesc
        End Try

        Return strDesc
    End Function
    'Public Function ShowCategoryName(ByVal Id As Integer) As String
    '    Dim str As String = String.Empty
    '    Dim sGPTitle As String = String.Empty
    '    Dim sPTitle As String = String.Empty
    '    Dim sTitle As String = String.Empty
    '    Dim UserDS As New DataSet
    '    Dim sStrNew As String = ""

    '    Try
    '        sStrNew = "SELECT c.Id, c.CategoryName, ParentId= isnull((SELECT Id FROM Category c2 WHERE c2.Id= c.CategoryParentId),0), Parent= (SELECT CategoryName FROM Category c2 WHERE c2.Id= c.CategoryParentId), GrandParent= (SELECT  CategoryName FROM Category c3 WHERE c3.Id = (SELECT CategoryParentId FROM Category c2 WHERE c2.Id= c.CategoryParentId)), GrandParentId= isnull((SELECT  Id FROM Category c3 WHERE c3.Id = (SELECT CategoryParentId FROM Category c2 WHERE c2.Id= c.CategoryParentId)),0) fROM Category c WHERE c.Id = " & Id
    '        UserDS = SQLData.generic_select(sStrNew, strConnection)

    '        If Not UserDS Is Nothing Then
    '            If UserDS.Tables(0).Rows.Count > 0 Then
    '                If Not UserDS.Tables(0).Rows(0)("GrandParent").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("GrandParent").ToString() <> String.Empty Then
    '                    sGPTitle = UserDS.Tables(0).Rows(0)("GrandParent").ToString().Trim
    '                End If
    '                If Not UserDS.Tables(0).Rows(0)("Parent").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("Parent").ToString() <> String.Empty Then
    '                    sPTitle = UserDS.Tables(0).Rows(0)("Parent").ToString().Trim
    '                End If
    '                If Not UserDS.Tables(0).Rows(0)("CategoryName").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("CategoryName").ToString() <> String.Empty Then
    '                    sTitle = UserDS.Tables(0).Rows(0)("CategoryName").ToString().Trim
    '                End If
    '            End If
    '        End If
    '        If sGPTitle.Length > 0 Then
    '            If sPTitle.Length > 0 Then
    '                If sTitle.Length > 0 Then
    '                    str = "Category: " & sGPTitle
    '                    str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;" & sPTitle
    '                    str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;>" & sTitle
    '                Else
    '                    str = "Category: " & sGPTitle
    '                    str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;" & sPTitle
    '                End If
    '            Else
    '                If sTitle.Length > 0 Then
    '                    str = "Category: " & sGPTitle
    '                    str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;>" & sTitle
    '                Else
    '                    str = "Category: " & sGPTitle
    '                End If
    '            End If
    '        Else
    '            If sPTitle.Length > 0 Then
    '                If sTitle.Length > 0 Then
    '                    str = "Category: " & sPTitle
    '                    str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;" & sTitle
    '                Else
    '                    str = "Category: " & sPTitle
    '                End If
    '            Else
    '                str = "Category: " & sTitle
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Return str
    '    End Try

    '    Return str
    'End Function

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
                        str = "Category: <a title='" & sGPTitle & "' href='../Admin/ManualProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("GrandParentId").ToString() & "'>" & sGPTitle & "</a> "
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<a title='" & sPTitle & "' href='../Admin/ManualProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("ParentId").ToString() & "'>" & sPTitle & "</a> "
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<a title='" & sTitle & "' href='../Admin/ManualProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("Id").ToString() & "'>" & sTitle & "</a> "
                    Else
                        str = "Category: <a title='" & sGPTitle & "' href='../Admin/ManualProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("GrandParentId").ToString() & "'>" & sGPTitle & "</a> "
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<a title='" & sPTitle & "' href='../Admin/ManualProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("ParentId").ToString() & "'>" & sPTitle & "</a> "
                    End If
                Else
                    If sTitle.Length > 0 Then
                        str = "Category: <a title='" & sGPTitle & "' href='../Admin/ManualProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("GrandParentId").ToString() & "'>" & sGPTitle & "</a> "
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<a title='" & sTitle & "' href='../Admin/ManualProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("Id").ToString() & "'>" & sTitle & "</a> "
                    Else
                        str = "Category: <a title='" & sGPTitle & "' href='../Admin/ManualProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("GrandParentId").ToString() & "'>" & sGPTitle & "</a> "
                    End If
                End If
            Else
                If sPTitle.Length > 0 Then
                    If sTitle.Length > 0 Then
                        str = "Category: <a title='" & sPTitle & "' href='../Admin/ManualProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("ParentId").ToString() & "'>" & sPTitle & "</a> "
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<a title='" & sTitle & "' href='../Admin/ManualProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("Id").ToString() & "'>" & sTitle & "</a> "
                    Else
                        str = "Category: <a title='" & sPTitle & "' href='../Admin/ManualProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("ParentId").ToString() & "'>" & sPTitle & "</a> "
                    End If
                Else
                    str = "Category: <a title='" & sTitle & "' href='../Admin/ManualProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("Id").ToString() & "'>" & sTitle & "</a> "
                End If
            End If

        Catch ex As Exception
            Return str
        End Try

        Return str
    End Function
    Public Function ShowCategoryById(ByVal Id As Integer) As String
        Dim UserDS As New DataSet
        Dim sStrNew As String = ""
        Dim sTitle As String = String.Empty

        Try
            sStrNew = "SELECT c.Id, c.CategoryName, ParentId= (SELECT Id FROM Category c2 WHERE c2.Id= c.CategoryParentId), Parent= (SELECT CategoryName FROM Category c2 WHERE c2.Id= c.CategoryParentId), GrandParent= (SELECT  CategoryName FROM Category c3 WHERE c3.Id = (SELECT CategoryParentId FROM Category c2 WHERE c2.Id= c.CategoryParentId)), GrandParentId= (SELECT  Id FROM Category c3 WHERE c3.Id = (SELECT CategoryParentId FROM Category c2 WHERE c2.Id= c.CategoryParentId)) fROM Category c WHERE c.Id =  " & Id
            UserDS = SQLData.generic_select(sStrNew, strConnection)

            If Not UserDS Is Nothing Then
                If UserDS.Tables(0).Rows.Count > 0 Then
                    If Not UserDS.Tables(0).Rows(0)("GrandParent").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("GrandParent").ToString() <> String.Empty Then
                        If Not UserDS.Tables(0).Rows(0)("Parent").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("Parent").ToString() <> String.Empty Then
                            If Not UserDS.Tables(0).Rows(0)("CategoryName").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("CategoryName").ToString() <> String.Empty Then
                                sTitle = " <h1 class='pagetitle'><a href='../Admin/InventoryList.aspx'>Inventory List</a>&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("GrandParent").ToString() & "&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("Parent").ToString() & "&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("CategoryName").ToString() & "</h1>"
                            Else
                                sTitle = "<h1 class='pagetitle'><a href='../Admin/InventoryList.aspx'>Inventory List</a>&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("GrandParent").ToString() & "&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("Parent").ToString() & "</h1>"
                            End If
                        Else
                            If Not UserDS.Tables(0).Rows(0)("CategoryName").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("CategoryName").ToString() <> String.Empty Then
                                sTitle = " <h1 class='pagetitle'><a href='../Admin/InventoryList.aspx'>Inventory List</a>&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("GrandParent").ToString() & "&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("CategoryName").ToString() & "</h1>"
                            Else
                                sTitle = "<h1 class='pagetitle'><a href='../Admin/InventoryList.aspx'>Inventory List</a>&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("GrandParent").ToString() & "</h1>"
                            End If
                        End If
                    Else
                        If Not UserDS.Tables(0).Rows(0)("Parent").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("Parent").ToString() <> String.Empty Then
                            If Not UserDS.Tables(0).Rows(0)("CategoryName").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("CategoryName").ToString() <> String.Empty Then
                                sTitle = " <h1 class='pagetitle'><a href='../Admin/InventoryList.aspx'>Inventory List</a>&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("Parent").ToString() & "&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("CategoryName").ToString() & "</h1>"
                            Else
                                sTitle = " <h1 class='pagetitle'><a href='../Admin/InventoryList.aspx'>Inventory List</a>&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("Parent").ToString() & "</h1>"
                            End If
                        Else
                            If Not UserDS.Tables(0).Rows(0)("CategoryName").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("CategoryName").ToString() <> String.Empty Then
                                sTitle = " <h1 class='pagetitle'><a href='../Admin/InventoryList.aspx'>Inventory List</a>&nbsp;&nbsp;>>&nbsp;&nbsp;" & UserDS.Tables(0).Rows(0)("CategoryName").ToString() & "</h1>"
                            Else
                                sTitle = ""
                            End If
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

    Protected Sub btnShow25_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShow25.Click
        Try
            If Not Session("pagerSQL") Is Nothing Then
                If Not Session("strOrder") Is Nothing Then
                    sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                    SqlDataSource1.SelectCommand = sSQl
                Else
                    SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                End If
                Pager.PageSize = 25
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnShow50_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShow50.Click
        Try
            If Not Session("pagerSQL") Is Nothing Then
                If Not Session("strOrder") Is Nothing Then
                    sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                    SqlDataSource1.SelectCommand = sSQl
                Else
                    SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                End If
                Pager.PageSize = 50
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnShow75_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShow75.Click
        Try
            If Not Session("pagerSQL") Is Nothing Then
                If Not Session("strOrder") Is Nothing Then
                    sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                    SqlDataSource1.SelectCommand = sSQl
                Else
                    SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                End If
                Pager.PageSize = 75
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnShow100_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShow100.Click
        Try
            If Not Session("pagerSQL") Is Nothing Then
                If Not Session("strOrder") Is Nothing Then
                    sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                    SqlDataSource1.SelectCommand = sSQl
                Else
                    SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                End If
                Pager.PageSize = 100
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Try
            If Not Session("pagerSQL") Is Nothing Then
                If Not Session("strOrder") Is Nothing Then
                    sSQl = Session("pagerSQL") & Session("strOrder").ToString()
                    SqlDataSource1.SelectCommand = sSQl
                Else
                    SqlDataSource1.SelectCommand = Session("pagerSQL") & " order by  p.ProductName asc "
                End If
                Pager.PageSize = 100000
            End If
        Catch ex As Exception

        End Try
    End Sub
    
End Class
