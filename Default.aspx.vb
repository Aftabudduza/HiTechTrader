Imports System.Data

Partial Class _Default
    Inherits System.Web.UI.Page
    Public m_objCN As Data.IDbConnection
    Public errStr As String
    Dim userId As Integer = 0
    Dim PromotionID As Integer = 0
    Dim ds As New DataSet
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private strConnection As String = appGlobal.CONNECTIONSTRING()
    Dim sSQl As String = String.Empty
    Private strImageFilePath As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageFilePath")
    Private strImageURL As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageURL")
    Public Function ImageName(ByVal nId As Integer) As String
        Dim sImageName As String = ""
        Try
            Dim strSQL As String = ""
            Dim UserDS As New DataSet
            Dim strImg As String = ""
            strSQL = "select * from Product where Id=" & nId
            UserDS = SQLData.generic_select(strSQL, strConnection)
            If Not UserDS Is Nothing And UserDS.Tables(0).Rows.Count > 0 Then
                If Not UserDS.Tables(0).Rows(0)("ImageFileName").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("ImageFileName").ToString() <> String.Empty Then
                    strImg = UserDS.Tables(0).Rows(0)("ImageFileName").ToString()
                    Dim imgArray() As String = Nothing
                    Dim sStr As String = ""
                    Dim sStrFile As String = ""
                    If strImg.Length > 0 Then
                        If Not String.IsNullOrEmpty(strImageFilePath) Then
                            sStrFile = strImageFilePath & "\Images\" & strImg
                        Else
                            sStrFile = "C:\inetpub\wwwroot\ProductImages\Large\Images\" & strImg
                        End If
                        If Not String.IsNullOrEmpty(strImageURL) Then
                            sStr = strImageURL & "/Images/" & strImg
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            getFeaturedProducts()
            Try
                'add it
                Dim sIns As String = "INSERT INTO ProductHit ([IsHome],[CreatedDate]) VALUES('1','" & CDate(DateTime.UtcNow) & "')"
                SQLData.generic_command(sIns, SQLData.ConnectionString)
            Catch ex As Exception

            End Try
        End If
    End Sub
    Public Sub getFeaturedProducts()
        Try
            If sSQl.Length = 0 Then
                sSQl = " SELECT *, c.CategoryName  FROM Product p, Category c WHERE p.Category = c.Id AND  p.IsFeaturedItem = 1 "
            End If
            Dim dt As DataTable = BRIClassLibrary.SQLData.generic_select(sSQl, strConnection).Tables(0)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                Dim i As Integer = 0
                Dim m As Integer = 0
                Dim sb As New System.Text.StringBuilder
                sb.Append("<ul>")
                If dt.Rows.Count > 0 Then
                    Dim count As Integer = dt.Rows.Count
                    For Each dr As DataRow In dt.Rows
                        'If i Mod 3 = 0 And i > 2 Then
                        '    sb.Append("<li>")
                        '    m = 1
                        'Else
                        '    m = m + 1
                        'End If
                        Dim nPrice As Double = 0
                        If Not dr("Price") Is Nothing AndAlso CDbl(dr("Price").ToString()) > 0 Then
                            nPrice = CDbl(dr("Price").ToString())
                        End If
                        Dim strCoupon As String = ""
                        If Not dr(2) Is Nothing AndAlso dr(2).ToString() <> String.Empty Then
                            'sb.Append("<li><div class='FeaturedDisplay'><div class='CatDisplay'> <a title='" & dr("CategoryName").ToString() & "' href='http://192.82.249.221/Pages/ProductListing.aspx?sc_state=1&sc_cat=" & dr("Category").ToString() & "&sc_id=1'>" & dr("CategoryName").ToString() & "</a></div> <span> <a title='" & dr(2).ToString() & "' href='../Pages/ProductDetails.aspx?Id=" & dr(0).ToString() & "'>" & dr(2).ToString() & "</a></span> <a title='' href='../Pages/ProductDetails.aspx?Id=" & dr(0).ToString() & "'>  <img alt='" & dr(2).ToString() & "' width='130' vspace='5' hspace='5' height='115' border='0' src='../ProductImages/Large/Images/" & dr("ImageFileName").ToString() & "' /></a> <span>$" & nPrice.ToString("#.00") & "</span></div></li>")
                            sb.Append("<li><div class='FeaturedDisplay'><div class='CatDisplay'> <a title='" & dr("CategoryName").ToString() & "' href='http://192.82.249.221/Pages/ProductListing.aspx?sc_state=1&sc_cat=" & dr("Category").ToString() & "&sc_id=1'>" & dr("CategoryName").ToString() & "</a></div> <div style='text-align:center; width:100%; height:40px;'> <a title='" & dr(2).ToString() & "' href='../Pages/ProductDetails.aspx?Id=" & dr(0).ToString() & "'>" & dr(2).ToString() & "</a></div> <div style='height:130px; overflow:hidden; padding-bottom:10px;'><a title='' href='../Pages/ProductDetails.aspx?Id=" & dr(0).ToString() & "'>  <img alt='" & dr(2).ToString() & "' width='130' height='115' vspace='5' hspace='5' border='0' src='" & ImageName(CInt(dr(0).ToString())) & "' /></a></div> <span>$" & nPrice.ToString("#.00") & "</span></div></li>")
                        End If
                        'If m Mod 3 = 0 And m > 2 And i > 2 Then
                        '    sb.Append("</li>")
                        'End If
                        i = i + 1
                    Next
                End If
                sb.Append("</ul>")
                spanFeatured.InnerHtml = sb.ToString()
            End If
        Catch ex As Exception
        End Try
    End Sub
    
End Class
