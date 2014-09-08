Imports System.Data
Imports BRIClassLibrary
Partial Class Master_Pages_Home
    Inherits System.Web.UI.MasterPage
    Private dt As DataTable = Nothing
    Private ds As DataSet = New DataSet()
    Private dt2 As DataTable = New DataTable()
    Public strConnection As String = appGlobal.CONNECTIONSTRING
    Public errStr As String = String.Empty
    Dim sSQl As String = ""
    Dim ProductCategoryId As Integer = 0
    Dim ProductStateId As Integer = 0
    Public str As String = String.Empty
    Public strWhere As String = String.Empty
    Public sOrderBy As String = String.Empty
    Public strNoteList As String = String.Empty
    Public sSearch As String = String.Empty
    Public sql As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            spanLeftTopLogo.InnerHtml = appGlobal.GetCMS_Message("LeftTopLogo", "Left Top Logo")
            spanLeftInnerLogo.InnerHtml = appGlobal.GetCMS_Message("LeftInnerLogo", "Left Inner Logo")

        Catch ex As Exception

        End Try
       
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
            FillLeftMenu()
        End If
        If ProductCategoryId > 0 Then
            spanCatTitle.InnerHtml = ShowCategoryName(ProductCategoryId)
            spanAllCat.Visible = True
        Else
            spanCatTitle.InnerHtml = ""
            spanAllCat.Visible = False
        End If
    End Sub
    Public Sub FillLeftMenu()
        Dim strSQL As String = ""
        Dim strChildSQL As String = ""
        Dim strChild2SQL As String = ""
        Dim objDS As DataSet = Nothing
        Dim objDS2 As DataSet = Nothing
        Dim pcIDs As String = ""
        Dim ds As DataSet = Nothing
        Dim dsChild As DataSet = Nothing
        Dim dsChild2 As DataSet = Nothing
        Dim strTemp As String = ""
        Dim html As String = ""
        Try
            strSQL = " SELECT  c.*  FROM   Category c WHERE c.CategoryParentId = 0   ORDER BY c.CategoryName ASC  "
            ds = BRIClassLibrary.SQLData.generic_select(strSQL, strConnection)
            If Not ds Is Nothing And ds.Tables(0).Rows.Count > 0 Then
                html += "<li class='active'><a href='../Pages/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=0' title='Inventory List: All Categories'><span>Categories</span></a></li>"
                For Each dr As DataRow In ds.Tables(0).Rows
                    html += " <li class='has-sub'><a href='../Pages/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=" & CInt(dr("Id").ToString()) & "'><span>" & dr("CategoryName") & "</span></a>"
                    If CInt(dr("Id").ToString()) > 0 Then
                        strChildSQL = "SELECT * FROM Category c WHERE c.CategoryParentId =" & CInt(dr("Id").ToString())
                        dsChild = BRIClassLibrary.SQLData.generic_select(strChildSQL, strConnection)
                        If Not dsChild Is Nothing And dsChild.Tables(0).Rows.Count > 0 Then
                            html += "<ul>"
                            For Each dr2 As DataRow In dsChild.Tables(0).Rows
                                If CInt(dr2("Id").ToString()) > 0 Then
                                    strChild2SQL = "SELECT * FROM Category c WHERE c.CategoryParentId =" & CInt(dr2("Id").ToString())
                                    dsChild2 = BRIClassLibrary.SQLData.generic_select(strChild2SQL, strConnection)
                                    If Not dsChild2 Is Nothing And dsChild2.Tables(0).Rows.Count > 0 Then
                                        html += "<li class='has-sub'><a href='../Pages/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=" & CInt(dr2("Id").ToString()) & "'><span>" & dr2("CategoryName") & "</span></a><ul style='display:block;'>"
                                        For Each dr3 As DataRow In dsChild2.Tables(0).Rows
                                            html += " <li><a href='../Pages/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=" & CInt(dr3("Id").ToString()) & "'><span>" & dr3("CategoryName") & "</span></a></li>"
                                        Next
                                        html += "</ul></li>"
                                    Else
                                        html += " <li><a href='../Pages/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=" & CInt(dr2("Id").ToString()) & "'><span>" & dr2("CategoryName") & "</span></a></li>"
                                    End If
                                End If
                            Next
                            html += "</ul>"
                        End If
                    End If
                    html += "</li>"
                Next
            End If
            leftmenu.InnerHtml = html

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
        Dim nGPTotal As Integer = 0
        Dim nPTotal As Integer = 0
        Dim nTotal As Integer = 0

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
                        str = "<ul><li><a title='" & sGPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("GrandParentId").ToString() & "'>" & sGPTitle & "</a> "
                        str &= "<ul><li><a title='" & sPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("ParentId").ToString() & "'>" & sPTitle & "</a> "
                        str &= "<ul><li><a style='font-weight:bold;' title='" & sTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("Id").ToString() & "'>" & sTitle & "</a></li></ul> </li></ul></li>"
                    Else
                        str = "<ul><li><a title='" & sGPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("GrandParentId").ToString() & "'>" & sGPTitle & "</a> "
                        str &= "<ul><li><a style='font-weight:bold;' title='" & sPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("ParentId").ToString() & "'>" & sPTitle & "</a></li></ul> "
                    End If
                Else
                    If sTitle.Length > 0 Then
                        str = "<ul><li><a title='" & sGPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("GrandParentId").ToString() & "'>" & sGPTitle & "</a>"
                        str &= "<ul><li><a style='font-weight:bold;' title='" & sTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("Id").ToString() & "'>" & sTitle & "</a></li></ul></li></ul> "
                    Else
                        str = "<ul><li><a style='font-weight:bold;' title='" & sGPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("GrandParentId").ToString() & "'>" & sGPTitle & "</b></a> </li></ul>"
                    End If
                End If
            Else
                If sPTitle.Length > 0 Then
                    If sTitle.Length > 0 Then
                        str = "<ul><li><a title='" & sPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("ParentId").ToString() & "'>" & sPTitle & "</a>"
                        str &= "<ul><li><a style='font-weight:bold;' title='" & sTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("Id").ToString() & "'>" & sTitle & "</a> </li></ul></li></ul>"
                    Else
                        str = "<ul><li><a style='font-weight:bold;' title='" & sPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("ParentId").ToString() & "'>" & sPTitle & "</a> </li></ul>"
                    End If
                Else
                    str = "<ul><li><a style='font-weight:bold;' title='" & sTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("Id").ToString() & "'>" & sTitle & "</a></li></ul>"
                End If
            End If

        Catch ex As Exception
            Return str
        End Try

        Return str
    End Function
End Class

