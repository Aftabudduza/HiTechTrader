Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_InventoryList
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Dim CategoryID As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("Id") Is Nothing Then
            If Not Page.IsPostBack Then
                fill_MyCategories()
            End If
        Else
            Response.Redirect("Login.aspx")
        End If
    End Sub
    'Private Sub fill_MyCategories()
    '    Dim htmlStr As String = ""
    '    Dim query As String = ""
    '    Try
    '        query = "SELECT DISTINCT c.Id, MAX(c.CategoryName) NAME, COUNT(p.Id) total   FROM Product p, Category c WHERE c.Id = p.ParentCategory AND c.IsMainorLabX = 0 GROUP BY C.Id"
    '        Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(query, appGlobal.CONNECTIONSTRING)
    '        If Not ds Is Nothing And ds.Tables(0).Rows.Count > 0 Then
    '            Dim i As Integer = 0
    '            Dim m As Integer = 0
    '            Dim j As Integer = 0
    '            Dim k As Integer = 0
    '            Dim tbl As New System.Text.StringBuilder
    '            Dim SubQuary As String = ""
    '            tbl.Append("<table  width='100%;' class='ListCategory' style=' color: #000000;float:left; cursor: pointer;font: 13px/20px Calibri; padding-left: 9px;'><tr><td class='ListCategoryColumn'>")
    '            If ds.Tables(0).Rows.Count > 0 Then
    '                k = CInt(ds.Tables(0).Rows.Count)
    '                m = CDbl(k / 4).ToString("00")
    '                m = Math.Round(m)
    '                For Each dr As DataRow In ds.Tables(0).Rows
    '                    If i Mod 4 = 0 And i > 3 Then
    '                        tbl.Append("</tr>")
    '                    Else
    '                        If j > m Then
    '                            tbl.Append("<td></td><td class='ListCategoryColumn'><div><h3 class='MainCategory'><a href='../Admin/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=" & dr("Id").ToString() & "' class='h3a'>" & dr("NAME").ToString() & "&nbsp;&nbsp;" & "(" & dr("total") & ")" & "</a></h3><ul class='ListCategory'>")
    '                            j = 0
    '                        Else
    '                            tbl.Append("<div><h3 class='MainCategory'><a href='../Admin/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=" & dr("Id").ToString() & "' class='h3a'>" & dr("NAME") & "&nbsp;&nbsp;" & "(" & dr("total") & ")" & "</a></h3><ul class='ListCategory'>")
    '                        End If
    '                    End If
    '                    If CInt(ds.Tables(0).Rows(0)("Id")) > 0 Then
    '                        SubQuary = "SELECT DISTINCT c.Id, MAX(c.CategoryName) NAME,  COUNT(pccr.ProductId) Total FROM ProductCategoryCrossRef pccr, Category c WHERE  pccr.ProductCategoryId = c.Id AND c.IsMainorLabX = 0 AND c.CategoryParentId=" & CInt(dr("Id")) & " GROUP BY c.Id ORDER BY max(c.CategoryName) ASC"
    '                        Dim ds2 As DataSet = BRIClassLibrary.SQLData.generic_select(SubQuary, appGlobal.CONNECTIONSTRING)
    '                        If Not ds2 Is Nothing And ds2.Tables(0).Rows.Count > 0 Then
    '                            For Each dr2 As DataRow In ds2.Tables(0).Rows
    '                                tbl.Append("<li><a href='../Admin/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=" & dr2("Id").ToString() & "'>" & dr2("NAME") & "(" & dr2("Total") & ")" & "</a></li>")
    '                            Next
    '                        End If
    '                        tbl.Append("</ul></div><br/>")
    '                        j = j + 1
    '                    End If
    '                    If j > m Then
    '                        tbl.Append("</td>")
    '                        i = i + 1
    '                    End If


    '                Next
    '            End If
    '            tbl.Append("</table>")
    '            CategoryList.InnerHtml = tbl.ToString()
    '        Else
    '            CategoryList.InnerHtml = " "
    '        End If

    '    Catch ex As Exception
    '    End Try
    'End Sub
    Private Sub fill_MyCategories()
        Dim htmlStr As String = ""
        Dim query As String = ""
        Try
            ' query = "SELECT DISTINCT c.Id, MAX(c.CategoryName) NAME, COUNT(p.Id) total   FROM Product p, Category c WHERE c.Id = p.ParentCategory AND c.IsMainorLabX = 0 GROUP BY C.Id"
            query = " SELECT DISTINCT  c4.Id, max(c4.CategoryName) Name, Total =ISNULL((SELECT COUNT(p.Id) Total FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0  and (c.CategoryParentId = c4.Id OR c.Id = c4.Id OR c.CategoryParentId IN (SELECT c2.Id  FROM Category c2 WHERE c2.CategoryParentId IN (SELECT c3.CategoryParentId  FROM Category c3 WHERE c3.CategoryParentId =c4.Id)))),0)  FROM Category c4 WHERE c4.CategoryParentId = 0 AND c4.IsMainorLabX = 0  GROUP BY c4.Id HAVING ISNULL((SELECT COUNT(p.Id) Total FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0  and (c.CategoryParentId = c4.Id OR c.Id = c4.Id OR c.CategoryParentId IN (SELECT c2.Id  FROM Category c2 WHERE c2.CategoryParentId IN (SELECT c3.CategoryParentId  FROM Category c3 WHERE c3.CategoryParentId =c4.Id)))),0) > 0  ORDER BY max(c4.CategoryName) ASC  "
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(query, appGlobal.CONNECTIONSTRING)
            If Not ds Is Nothing And ds.Tables(0).Rows.Count > 0 Then
                Dim i As Integer = 0
                Dim m As Integer = 0
                Dim j As Integer = 0
                Dim k As Integer = 0
                Dim tbl As New System.Text.StringBuilder
                Dim SubQuery As String = ""
                Dim ChildQuery As String = ""

                tbl.Append("<table  width='100%;' class='ListCategory' style=' color: #000000;float:left; cursor: pointer;font: 13px/20px Calibri; padding-left: 9px;'><tr><td class='ListCategoryColumn'>")
                If ds.Tables(0).Rows.Count > 0 Then
                    k = CInt(ds.Tables(0).Rows.Count)
                    m = CDbl(k / 4).ToString("00")
                    m = Math.Round(m)
                    For Each dr As DataRow In ds.Tables(0).Rows
                        If i Mod 4 = 0 And i > 3 Then
                            tbl.Append("</tr>")
                        Else
                            If j > m Then
                                tbl.Append("<td></td><td class='ListCategoryColumn'><div><h3 class='MainCategory'><a href='../Admin/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=" & dr("Id").ToString() & "' class='h3a'>" & dr("NAME").ToString() & "&nbsp;&nbsp;" & "(" & dr("total") & ")" & "</a></h3><ul class='ListCategory'>")
                                j = 0
                            Else
                                tbl.Append("<div><h3 class='MainCategory'><a href='../Admin/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=" & dr("Id").ToString() & "' class='h3a'>" & dr("NAME") & "&nbsp;&nbsp;" & "(" & dr("total") & ")" & "</a></h3><ul class='ListCategory'>")
                            End If
                        End If
                        If Not dr("Id") Is Nothing And dr("Id").ToString() <> String.Empty Then
                            If CInt(dr("Id").ToString()) > 0 Then
                                SubQuery = " SELECT DISTINCT c2.Id, MAX(c2.CategoryName) Name, Total = ISNULL((SELECT COUNT(p.Id) Total FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0  and (c.CategoryParentId = c2.Id OR c.Id = c2.Id)),0)  FROM Category c2 WHERE c2.CategoryParentId = " & CInt(dr("Id").ToString()) & " GROUP BY c2.Id  HAVING ISNULL((SELECT COUNT(p.Id) Total FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0  and (c.CategoryParentId = c2.Id OR c.Id = c2.Id)),0) > 0 ORDER BY MAX(c2.CategoryName) ASC  "
                                Dim ds2 As DataSet = BRIClassLibrary.SQLData.generic_select(SubQuery, appGlobal.CONNECTIONSTRING)
                                If Not ds2 Is Nothing And ds2.Tables(0).Rows.Count > 0 Then
                                    For Each dr2 As DataRow In ds2.Tables(0).Rows
                                        If Not dr2("Id") Is Nothing And dr2("Id").ToString() <> String.Empty Then
                                            If CInt(dr2("Id").ToString()) > 0 Then
                                                ChildQuery = " SELECT DISTINCT c2.Id, MAX(c2.CategoryName) Name, Total = ISNULL((SELECT COUNT(p.Id) Total FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0  and  c.Id = c2.Id),0)  FROM Category c2 WHERE c2.CategoryParentId = " & CInt(dr2("Id").ToString()) & " GROUP BY c2.Id  HAVING ISNULL((SELECT COUNT(p.Id) Total FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0  and  c.Id = c2.Id),0) > 0 ORDER BY MAX(c2.CategoryName) ASC   "
                                                Dim ds3 As DataSet = BRIClassLibrary.SQLData.generic_select(ChildQuery, appGlobal.CONNECTIONSTRING)
                                                If Not ds3 Is Nothing And ds3.Tables(0).Rows.Count > 0 Then
                                                    tbl.Append("<li><a href='../Admin/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=" & dr2("Id").ToString() & "'>" & dr2("NAME") & "(" & dr2("Total") & ")" & "</a><ul class='ListCategory'>")
                                                    For Each dr3 As DataRow In ds3.Tables(0).Rows
                                                        If Not dr3("Id") Is Nothing And dr3("Id").ToString() <> String.Empty Then
                                                            If CInt(dr3("Id").ToString()) > 0 Then
                                                                tbl.Append("<li><a href='../Admin/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=" & dr3("Id").ToString() & "'>" & dr3("NAME") & "(" & dr3("Total") & ")" & "</a></li>")
                                                            End If
                                                        End If
                                                    Next
                                                    tbl.Append("</ul></li>")
                                                Else
                                                    tbl.Append("<li><a href='../Admin/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=" & dr2("Id").ToString() & "'>" & dr2("NAME") & "(" & dr2("Total") & ")" & "</a></li>")
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                                tbl.Append("</ul></div><br/>")
                                j = j + 1
                            End If
                        End If

                        If j > m Then
                            tbl.Append("</td>")
                            i = i + 1
                        End If


                    Next
                End If
                tbl.Append("</table>")
                CategoryList.InnerHtml = tbl.ToString()
            Else
                CategoryList.InnerHtml = " "
            End If

        Catch ex As Exception
        End Try
    End Sub
End Class
