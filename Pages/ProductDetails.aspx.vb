Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Pages_ProductDetails
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nProductID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Dim ProductId As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Session("ProductId") = Nothing
            Try
                ProductId = CInt(Request.QueryString("Id").ToString())
            Catch ex As Exception
                ProductId = 0
            End Try
            If ProductId > 0 Then
                Session("ProductId") = ProductId
                ImgL.InnerHtml = "<a title='30 day warranty' href='../Pages/ProductDetails.aspx?Id=" & ProductId & "'><img alt='30 day warranty' src='../App_Themes/Hitech/images/30daywarrenty_ty.gif'  class='warrenty_img' /> </a>"
            End If
            If Not Session("ProductId") Is Nothing Then
                fill_MyProduct(CInt(Session("ProductId").ToString()))
                FillVideo_ExcellLink(CInt(Session("ProductId").ToString()))
            Else
                Response.Redirect("~/Default.aspx")
            End If
        End If

    End Sub
    Public Sub fill_MyProduct(ByVal nProductID As Integer)
        If nProductID > 0 Then
            Try
                Dim str As String = ""
                Dim ds As DataSet = Nothing
                str = "SELECT * FROM Product p WHERE p.Id=" & nProductID
                ds = BRIClassLibrary.SQLData.generic_select(str, appGlobal.CONNECTIONSTRING)
                If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                    If Not ds.Tables(0).Rows(0)("Id") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("Id").ToString()) > 0 Then
                        Session("ProductId") = CInt(ds.Tables(0).Rows(0)("Id").ToString())
                        GetSubImage(CInt(Session("ProductId").ToString()))
                    End If
                    If Not ds.Tables(0).Rows(0)("ProductName") Is Nothing AndAlso ds.Tables(0).Rows(0)("ProductName").ToString() <> "" Then
                        Tittle.InnerHtml = ds.Tables(0).Rows(0)("ProductName").ToString()
                    End If
                    If Not ds.Tables(0).Rows(0)("ItemNumber") Is Nothing AndAlso ds.Tables(0).Rows(0)("ItemNumber").ToString() <> "" Then
                        ItemNumb.InnerHtml = ds.Tables(0).Rows(0)("ItemNumber").ToString()
                    End If
                    If Not ds.Tables(0).Rows(0)("Description") Is Nothing AndAlso ds.Tables(0).Rows(0)("Description").ToString() <> "" Then
                        Desc.InnerHtml = HttpUtility.HtmlDecode(ds.Tables(0).Rows(0)("Description").ToString())

                    End If
                    If Not ds.Tables(0).Rows(0)("Condition") Is Nothing AndAlso ds.Tables(0).Rows(0)("Condition").ToString() <> "" Then
                        Condition.InnerHtml = ds.Tables(0).Rows(0)("Condition").ToString()
                    End If
                    If Not ds.Tables(0).Rows(0)("Age") Is Nothing AndAlso ds.Tables(0).Rows(0)("Age").ToString() <> "" Then
                        Age.InnerHtml = ds.Tables(0).Rows(0)("Age").ToString()
                    End If

                    If Not ds.Tables(0).Rows(0)("Category") Is Nothing AndAlso ds.Tables(0).Rows(0)("Category").ToString() <> "" Then
                        If CInt(ds.Tables(0).Rows(0)("Category").ToString()) > 0 Then
                            CategoryName.InnerHtml = ShowCategoryName(CInt(ds.Tables(0).Rows(0)("Category").ToString()))
                        End If
                    End If

                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    'Public Sub GetSubImage(ByVal nproId As Integer)
    '    If nproId > 0 Then
    '        Try
    '            Dim html As String = ""
    '            Dim ds As DataSet = Nothing
    '            Dim str As String = "SELECT p.Id,p.ImageFileName FROM Product p WHERE p.Id=" & nproId
    '            ds = BRIClassLibrary.SQLData.generic_select(str, appGlobal.CONNECTIONSTRING)
    '            If Not ds Is Nothing Then
    '                If ds.Tables(0).Rows.Count > 0 Then
    '                    html = "<img src='../ProductImages/Large/" & ds.Tables(0).Rows(0)("ImageFileName") & "' alt='" & ds.Tables(0).Rows(0)("ImageFileName") & "' Height='auto' class='GalleryImage' id='GalMainImg' style='float: left;max-width: 720px;max-height: 600px;padding: 20px;'>"
    '                End If
    '            End If
    '            GalleryImg.InnerHtml = html
    '            Dim str2 As String = ""
    '            Dim ds2 As DataSet = Nothing
    '            Dim sb As String = ""
    '            str2 = "SELECT picr.ImageUrl FROM Product p,ProductImageCrossRef picr WHERE p.Id = picr.ProductId and p.Id=" & nproId
    '            ds2 = BRIClassLibrary.SQLData.generic_select(str2, appGlobal.CONNECTIONSTRING)
    '            If Not ds2 Is Nothing Then
    '                If ds2.Tables(0).Rows.Count > 0 Then
    '                    sb += "<div class='thumbnailGalleryFirstC3'><a hre=''><img src='../ProductImages/Large/" & ds.Tables(0).Rows(0)("ImageFileName") & "' alt='" & ds.Tables(0).Rows(0)("ImageFileName") & "' Height='auto' class='thumbnailImage' id='GalMainImg' style='float: left;padding: 13px;'></a></div>"
    '                    For Each dr As DataRow In ds2.Tables(0).Rows
    '                        If Not ds2.Tables(0).Rows(0)("ImageUrl") Is Nothing Then
    '                            sb += "<div class='thumbnailGallery'><a href='#'><img src='../ProductImages/Large/" & ds2.Tables(0).Rows(0)("ImageUrl") & "' alt='" & ds2.Tables(0).Rows(0)("ImageUrl") & "' Height='auto' class='thumbnailImage' id='GalMainImg' style='float: left;padding: 13px;'></a></div>"
    '                        End If
    '                    Next
    '                    GallSub.InnerHtml = sb
    '                End If
    '            End If

    '        Catch ex As Exception

    '        End Try
    '    End If
    'End Sub
    Public Sub GetSubImage(ByVal nproId As Integer)
        If nproId > 0 Then
            Try
                Dim imgname As FileInfo = Nothing
                Dim html As String = ""
                Dim ds As DataSet = Nothing
                Dim str As String = "SELECT p.Id,p.ImageFileName FROM Product p WHERE p.Id=" & nproId
                ds = BRIClassLibrary.SQLData.generic_select(str, appGlobal.CONNECTIONSTRING)
                If Not ds Is Nothing Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        html = "<img width='720' src='../ProductImages/Large/" & ds.Tables(0).Rows(0)("ImageFileName") & "' alt='" & ds.Tables(0).Rows(0)("ImageFileName") & "' Height='auto' class='GalleryImage' id='GalMainImg' style='float: left;max-width: 720px;max-height: 600px;padding: 20px;'>"
                    End If
                End If
                GalleryImg.InnerHtml = html
                Dim str2 As String = ""
                Dim ds2 As DataSet = Nothing
                Dim sb As String = ""
                Dim fileExt As String = ""
                str2 = "SELECT picr.ImageUrl FROM Product p,ProductImageCrossRef picr WHERE p.Id = picr.ProductId and p.Id=" & nproId
                ds2 = BRIClassLibrary.SQLData.generic_select(str2, appGlobal.CONNECTIONSTRING)
                If Not ds2 Is Nothing Then
                    If ds2.Tables(0).Rows.Count > 0 Then
                        sb += "<li><a href='../ProductImages/Large/" & ds.Tables(0).Rows(0)("ImageFileName") & "'><img src='../ProductImages/Large/" & ds.Tables(0).Rows(0)("ImageFileName") & "' alt='" & ds.Tables(0).Rows(0)("ImageFileName") & "' Height='auto' class='thumbnailImage' id='GalMainImg' style='float: left;padding: 13px;'></a></li>"
                        For Each dr As DataRow In ds2.Tables(0).Rows
                            If Not dr("ImageUrl") Is Nothing And dr("ImageUrl").ToString() <> String.Empty Then
                                Dim filename As String = dr("ImageUrl")
                                If filename.Contains(".jpg") Or filename.Contains(".jpeg") Or filename.Contains(".gif") Or filename.Contains(".png") Or filename.Contains(".bmp") Then
                                    sb += "<li><a href='../ProductImages/Large/" & filename & "'><img src='../ProductImages/Large/" & filename & "' alt='" & filename & "' Height='auto' class='thumbnailImage' id='GalMainImg' style='float: left;padding: 13px;'></a></li>"
                                End If

                            End If
                        Next
                        GallSub.InnerHtml = sb
                    End If
                End If

            Catch ex As Exception

            End Try
        End If
    End Sub
    'Public Sub Category(ByVal nCatId As Integer)
    '    Try
    '        Dim html As String = ""
    '        Dim sSQl As String = "SELECT * FROM Category c WHERE c.id =" & nCatId
    '        Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
    '        For Each dr As DataRow In ds.Tables(0).Rows
    '            If CInt(dr("CategoryParentId").ToString()) > 0 Then
    '                Dim str As String = "SELECT * FROM Category c WHERE c.Id =" & CInt(dr("CategoryParentId").ToString())
    '                Dim ds2 As DataSet = BRIClassLibrary.SQLData.generic_select(str, appGlobal.CONNECTIONSTRING)
    '                For Each dr2 As DataRow In ds2.Tables(0).Rows
    '                    If CInt(dr2("CategoryParentId").ToString()) > 0 Then
    '                        Dim str3 As String = "SELECT * FROM Category c WHERE c.Id =" & CInt(dr2("CategoryParentId").ToString())
    '                        Dim ds3 As DataSet = BRIClassLibrary.SQLData.generic_select(str3, appGlobal.CONNECTIONSTRING)
    '                        For Each dr3 As DataRow In ds3.Tables(0).Rows
    '                            html += "<a href='ProductListing.aspx?sc_cat=" & dr3("Id") & "&" & "sc_ProductId=" & CInt(Session("ProductId").ToString()) & ">" & dr3("CategoryName") & "</a>" & "<span style='color:#000;'> >> </span>" & "<a href='ProductListing.aspx?sc_cat=" & dr2("Id") & "&" & "sc_ProductId=" & CInt(Session("ProductId").ToString()) & ">" & dr2("CategoryName") & "</a>" & "<span style='color:#000;'> >> </span>" & "<a href='ProductListing.aspx?sc_cat=" & dr("Id") & "&" & "sc_ProductId=" & CInt(Session("ProductId").ToString()) & ">" & dr("CategoryName") & "</a>"
    '                        Next
    '                    Else
    '                        html += "<a href='ProductListing.aspx?sc_cat=" & dr2("Id") & "&" & "sc_ProductId=" & CInt(Session("ProductId").ToString()) & "'>" & dr2("CategoryName") & "</a>" & " <span style='color:#000;'> >> </span> " & "<a href='ProductListing.aspx?sc_cat=" & dr("Id") & "&" & "sc_ProductId=" & CInt(Session("ProductId").ToString()) & "'>" & dr("CategoryName") & "</a>"
    '                    End If
    '                Next
    '            Else
    '                html += "<a href='ProductListing.aspx?sc_cat=" & dr("Id") & "&" & "sc_ProductId=" & CInt(Session("ProductId").ToString()) & ">" & dr("CategoryName") & "</a>"
    '            End If
    '        Next
    '        Cat.Text = html
    '    Catch ex As Exception

    '    End Try
    'End Sub
    Protected Sub btnInquery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInquery.Click
        Try
            If Not Session("ProductId").ToString() Is Nothing Then
                Response.Redirect("ProductInquiry.aspx?Id=" & CInt(Session("ProductId").ToString()))
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
                        str = "<a href='InventoryList_UI.aspx'>InventoryList </a>&nbsp;>>&nbsp;&nbsp;<a title='" & sGPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("GrandParentId").ToString() & "'>" & sGPTitle & "</a> "
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<a title='" & sPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("ParentId").ToString() & "'>" & sPTitle & "</a> "
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<a title='" & sTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("Id").ToString() & "'>" & sTitle & "</a> "
                    Else
                        str = "<a href='InventoryList_UI.aspx'>InventoryList </a> &nbsp;>>&nbsp;&nbsp;<a title='" & sGPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("GrandParentId").ToString() & "'>" & sGPTitle & "</a> "
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<a title='" & sPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("ParentId").ToString() & "'>" & sPTitle & "</a> "
                    End If
                Else
                    If sTitle.Length > 0 Then
                        str = "<a href='InventoryList_UI.aspx'>InventoryList </a> &nbsp;>>&nbsp;&nbsp;<a title='" & sGPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("GrandParentId").ToString() & "'>" & sGPTitle & "</a> "
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<a title='" & sTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("Id").ToString() & "'>" & sTitle & "</a> "
                    Else
                        str = "<a href='InventoryList_UI.aspx'>InventoryList </a> &nbsp;>>&nbsp;&nbsp;<a title='" & sGPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("GrandParentId").ToString() & "'>" & sGPTitle & "</a> "
                    End If
                End If
            Else
                If sPTitle.Length > 0 Then
                    If sTitle.Length > 0 Then
                        str = "<a href='InventoryList_UI.aspx'>InventoryList </a> &nbsp;>>&nbsp;&nbsp;<a title='" & sPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("ParentId").ToString() & "'>" & sPTitle & "</a> "
                        str &= "&nbsp;&nbsp;>>&nbsp;&nbsp;<a title='" & sTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("Id").ToString() & "'>" & sTitle & "</a> "
                    Else
                        str = "<a href='InventoryList_UI.aspx'>InventoryList </a> &nbsp;>>&nbsp;&nbsp;<a title='" & sPTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("ParentId").ToString() & "'>" & sPTitle & "</a> "
                    End If
                Else
                    str = "<a href='InventoryList_UI.aspx'>InventoryList </a> &nbsp;>>&nbsp;&nbsp;<a title='" & sTitle & "' href='../Pages/ProductListing.aspx?sc_state=1&sc_id=1&sc_cat=" & UserDS.Tables(0).Rows(0)("Id").ToString() & "'>" & sTitle & "</a> "
                End If
            End If

        Catch ex As Exception
            Return str
        End Try

        Return str
    End Function
    Protected Sub btnrepeatInquery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnrepeatInquery.Click
        Try
            btnInquery_Click(sender, e)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub FillVideo_ExcellLink(ByVal nId As Integer)
        Try
            If nId > 0 Then
                Try
                    Dim str As String = ""
                    Dim ds As DataSet = Nothing
                    str = "SELECT p.VideoURL FROM Product p WHERE p.Id=" & CInt(nId.ToString())
                    If str.Length > 0 Then
                        ds = BRIClassLibrary.SQLData.generic_select(str, appGlobal.CONNECTIONSTRING)
                        If Not ds Is Nothing Then
                            If ds.Tables(0).Rows.Count > 0 Then
                                If Not ds.Tables(0).Rows(0)("VideoURL") Is Nothing AndAlso ds.Tables(0).Rows(0)("VideoURL").ToString() <> String.Empty Then
                                    VideoLink.InnerHtml = "<h3 class='ProductHilite' style='background-color: #ffff00'>Video Presentation:</h3> &nbsp;See a <a href='" & ds.Tables(0).Rows(0)("VideoURL").ToString() & "' target='_blank'>video</a> of this item."
                                End If

                            End If
                        End If
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim sql As String = ""
                    Dim dsExcell As DataSet = Nothing
                    Dim sImgName As String = ""
                    sql = "SELECT picr.ImageUrl FROM ProductImageCrossRef picr WHERE picr.ProductId=" & CInt(nId.ToString())
                    If sql.Length > 0 Then
                        dsExcell = BRIClassLibrary.SQLData.generic_select(sql, appGlobal.CONNECTIONSTRING)
                        If Not dsExcell Is Nothing Then
                            If dsExcell.Tables(0).Rows.Count > 0 Then
                                If Not dsExcell.Tables(0).Rows(0)("ImageUrl") Is Nothing AndAlso dsExcell.Tables(0).Rows(0)("ImageUrl").ToString() <> String.Empty Then
                                    For Each dr As DataRow In dsExcell.Tables(0).Rows
                                        sImgName = dr("ImageUrl").ToString()
                                        If sImgName.Contains(".xlsx") Or sImgName.Contains(".xls") Or sImgName.Contains(".doc") Or sImgName.Contains(".docx") Then
                                            ExcellLink.InnerHtml = "<a href='../ProductImages/Large/" & sImgName.ToString() & "' target='_blank' ><img src='../App_Themes/Hitech/images/Ex.png' alt='Excel Image'/></a>"
                                        ElseIf sImgName.Contains(".pdf") Then
                                            PdfLink.InnerHtml = "<a href='../ProductImages/Large/" & sImgName.ToString() & "' target='_blank' ><img src='../App_Themes/Hitech/images/pdf.png' alt='Pdf Image'/></a>"
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    End If
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
