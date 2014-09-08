Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Imports System.Web.Services

Partial Class Admin_MobileSearchResult
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
    Dim ds As DataSet = Nothing
    Dim dt As DataTable = Nothing
    Dim SearchText As String = ""
    Dim SearchType As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private strImageFilePath As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageFilePath")
    Private strImageURL As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageURL")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IsSuperAdmin") = True Then
            If Not Session("Id") Is Nothing Then
                If Not Page.IsPostBack Then
                    Session("sSearch") = Nothing
                    Session("nSearchType") = Nothing
                    Session("Location") = Nothing
                    Session("Barcode") = Nothing
                    Session("IsNotOnWeb") = Nothing
                    Session("IsConsignmentItem") = Nothing
                    'fill_ParentCategory()
                    'GetItemNo()
                    Try
                        SearchText = Request.QueryString("Search").ToString()
                    Catch ex As Exception
                        SearchText = ""
                    End Try
                    If Not String.IsNullOrEmpty(SearchText.ToString()) And SearchText.ToString() <> "" Then
                        Session("sSearch") = SearchText
                    End If

                    Try
                        SearchType = CInt(Request.QueryString("SearchType").ToString())
                    Catch ex As Exception
                        SearchType = 0
                    End Try
                    If SearchType > 0 Then
                        Session("nSearchType") = SearchType
                    End If
                    If Not (Session("sSearch")) Is Nothing And Not (Session("nSearchType")) Is Nothing Then
                        fill_Controls(Session("sSearch").ToString(), CInt(Session("nSearchType").ToString()))
                        '  btnAddItem.Text = "Update Item"
                        ' spanEdit.InnerHtml = "<a href='ProductCopy.aspx?CopyImage=0&CopyID=" & CInt(Session("ProductId").ToString()) & "'>Copy Data to New Item</a>  :  <a href='ProductCopy.aspx?CopyImage=1&CopyID=" & CInt(Session("ProductId").ToString()) & "'>Copy Data & Image to New Item</a>"
                    End If

                End If
            Else
                Response.Redirect("MobileLogin.aspx")
            End If
        Else
            Response.Redirect("MobileLogin.aspx")
        End If

    End Sub
    Public Function ImageName(ByVal nId As Integer) As String
        Dim sImageName As String = ""
        Try
            Dim strSQL As String = ""
            Dim UserDS As New DataSet
            Dim strImg As String = ""
            strSQL = "select * from Product where Id=" & nId
            UserDS = SQLData.generic_select(strSQL, strConnection)
            If Not UserDS.Tables(0).Rows(0)("ImageFileName").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("ImageFileName").ToString() <> String.Empty Then
                strImg = UserDS.Tables(0).Rows(0)("ImageFileName").ToString()
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

        Catch ex As Exception
            sImageName = strImageURL & "/not_found_image.jpg"
        End Try
        Return sImageName
    End Function
    Public Sub fill_Controls(ByVal sSearch As String, ByVal nSearchType As Integer)
        If nSearchType > 0 And sSearch.Length > 0 Then
            Try
                Dim nCount As Integer = 0
                Dim EditSearch As String = ""
                Dim dtTitle As DataTable = Nothing
                Dim dtPrice As DataTable = Nothing
                Dim dtBarcode As DataTable = Nothing
                Dim dtLocation As DataTable = Nothing
                Dim html As String = ""
                If nSearchType = 1 Then
                    Try
                        sSQl &= "SELECT * FROM Product p WHERE "
                        sSQl &= "p.ItemNumber='" & sSearch.ToString() & "'"
                        dt = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                        If Not dt Is Nothing And dt.Rows.Count > 0 Then
                            For Each dr As DataRow In dt.Rows
                                If Not dr("Location") Is Nothing Then
                                    Session("Location") = dr("Location")
                                End If
                                If Not dr("Barcode") Is Nothing Then
                                    Session("Barcode") = dr("Barcode")
                                End If
                                If dr("IsNotOnWeb").ToString() <> "" AndAlso Not dr("IsNotOnWeb").ToString() Is Nothing Then
                                    If CInt(dr("IsNotOnWeb").ToString()) > 0 Then
                                        Session("IsNotOnWeb") = 1
                                    Else
                                        Session("IsNotOnWeb") = 0
                                    End If
                                Else
                                    Session("IsNotOnWeb") = 0
                                End If
                                If CInt(dr("IsConsignmentItem")) > 0 Then
                                    Session("IsConsignmentItem") = 1
                                Else
                                    Session("IsConsignmentItem") = 0
                                End If
                                html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                If Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                    html &= "<tr><td class='ItemRowName' style='color:green'>ItemNo:</td><td>" & dr("ItemNumber") & "</td></tr>"
                                End If
                                If Not Session("Location") Is Nothing AndAlso Session("Barcode") Is Nothing Then
                                    html &= "<tr><td class='ItemRowName' style='color:blue'>ItemNo:</td><td>" & dr("ItemNumber") & "</td></tr>"
                                End If
                                If Not Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                    html &= "<tr><td class='ItemRowName' style='color:black'>ItemNo:</td><td>" & dr("ItemNumber") & "</td></tr>"
                                End If

                                html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & dr("Id") & "'>" & dr("ProductName") & "</a></td></tr>"
                                html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & dr("Barcode") & "&SearchType=" & 1 & "'>" & dr("Barcode") & "</a></td> </tr>"
                                'html &= "<tr> <td class='ItemRowName'>Barcode</td> <td>" & dr("Barcode") & "</td> </tr>   "
                                html &= "<tr> <td class='ItemRowName'>Location:</td> <td><a href='MobileSearchResult.aspx?Search=" & dr("Location") & "&SearchType=" & 1 & "'>" & dr("Location") & "</a></td></tr>"
                                'html &= " <tr><td class='ItemRowName'><img src='' alt='' /></td><td>Price  :$<input type='text' id='txtPrice' name='' /><br /> POD™Price:$<input type='text' id='txtPodPrice' name='' /><br /> <input type='button' id='btnGo' name='Go' /></td></tr>"
                                html &= "<tr>  <td colspan='2'><img width='100' src='" & ImageName(CInt(dr("Id").ToString())) & "' alt='" & dr("ImageFileName") & "' /></td> </tr> "
                                If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 1 Then
                                    html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web <br/> On Consignment</td></tr>"
                                End If
                                If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 0 Then
                                    html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web </td></tr>"
                                End If
                                If Session("IsNotOnWeb") = 0 AndAlso Session("IsConsignmentItem") = 1 Then
                                    html &= "<tr><td colspan='2' style='background:yellow;'>Status : On Consignment</td></tr>"
                                End If
                                html &= " </table>"
                                nCount = nCount + 1
                            Next
                        Else
                            sSQl &= " OR p.ProductName='" & sSearch.ToString() & "'"
                            dtTitle = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                            If Not dtTitle Is Nothing And dtTitle.Rows.Count > 0 Then
                                For Each drTitle As DataRow In dtTitle.Rows
                                    If Not drTitle("Location") Is Nothing Then
                                        Session("Location") = drTitle("Location")
                                    End If
                                    If Not drTitle("Barcode") Is Nothing Then
                                        Session("Barcode") = drTitle("Barcode")
                                    End If
                                    If drTitle("IsNotOnWeb").ToString() <> "" AndAlso Not drTitle("IsNotOnWeb").ToString() Is Nothing Then
                                        If CInt(drTitle("IsNotOnWeb").ToString()) > 0 Then
                                            Session("IsNotOnWeb") = 1
                                        Else
                                            Session("IsNotOnWeb") = 0
                                        End If
                                    Else
                                        Session("IsNotOnWeb") = 0
                                    End If
                                    If CInt(drTitle("IsConsignmentItem")) > 0 Then
                                        Session("IsConsignmentItem") = 1
                                    Else
                                        Session("IsConsignmentItem") = 0
                                    End If
                                    html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                    If Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                        html &= "<tr><td class='ItemRowName' style='color:green'>ItemNo:</td><td>" & drTitle("ItemNumber") & "</td></tr>"
                                    End If
                                    If Not Session("Location") Is Nothing AndAlso Session("Barcode") Is Nothing Then
                                        html &= "<tr><td class='ItemRowName' style='color:blue'>ItemNo:</td><td>" & drTitle("ItemNumber") & "</td></tr>"
                                    End If
                                    If Not Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                        html &= "<tr><td class='ItemRowName' style='color:black'>ItemNo:</td><td>" & drTitle("ItemNumber") & "</td></tr>"
                                    End If
                                    html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & drTitle("Id") & "'>" & drTitle("ProductName") & "</a></td></tr>"
                                    html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & drTitle("Barcode") & "&SearchType=" & 1 & "'>" & drTitle("Barcode") & "</a></td> </tr>"
                                    html &= "<tr> <td class='ItemRowName'>Location:</td> <td><a href='MobileSearchResult.aspx?Search=" & drTitle("Location") & "&SearchType=" & 1 & "'>" & drTitle("Location") & "</a></td></tr>"
                                    'html &= " <tr><td class='ItemRowName'><img src='' alt='' /></td><td>Price  :$<input type='text' id='txtPrice' name='' /><br /> POD™Price:$<input type='text' id='txtPodPrice' name='' /><br /> <input type='button' id='btnGo' name='Go' /></td></tr>"
                                    html &= "<tr>  <td colspan='2'><img width='100' src='" & ImageName(CInt(drTitle("Id").ToString())) & "' alt='" & drTitle("ImageFileName") & "' /></td> </tr> "
                                    If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 1 Then
                                        html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web <br/> On Consignment</td></tr>"
                                    End If
                                    If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 0 Then
                                        html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web </td></tr>"
                                    End If
                                    If Session("IsNotOnWeb") = 0 AndAlso Session("IsConsignmentItem") = 1 Then
                                        html &= "<tr><td colspan='2' style='background:yellow;'>Status : On Consignment</td></tr>"
                                    End If
                                    html &= " </table>"
                                    nCount = nCount + 1
                                Next

                            Else

                                EditSearch = sSearch.ToString().Substring(sSearch.Length - 4)
                                sSQl &= " OR p.Location LIKE'%" & EditSearch.ToString() & "'"
                                dtLocation = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                                If Not dtLocation Is Nothing And dtLocation.Rows.Count > 0 Then
                                    For Each drLocation As DataRow In dtLocation.Rows
                                        If Not drLocation("Location") Is Nothing Then
                                            Session("Location") = drLocation("Location")
                                        End If
                                        If Not drLocation("Barcode") Is Nothing Then
                                            Session("Barcode") = drLocation("Barcode")
                                        End If
                                        If drLocation("IsNotOnWeb").ToString() <> "" AndAlso Not drLocation("IsNotOnWeb").ToString() Is Nothing Then
                                            If CInt(drLocation("IsNotOnWeb").ToString()) > 0 Then
                                                Session("IsNotOnWeb") = 1
                                            Else
                                                Session("IsNotOnWeb") = 0
                                            End If
                                        Else
                                            Session("IsNotOnWeb") = 0
                                        End If
                                        If CInt(drLocation("IsConsignmentItem")) > 0 Then
                                            Session("IsConsignmentItem") = 1
                                        Else
                                            Session("IsConsignmentItem") = 0
                                        End If
                                        html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                        If Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                            html &= "<tr><td class='ItemRowName' style='color:green'>ItemNo:</td><td>" & drLocation("ItemNumber") & "</td></tr>"
                                        End If
                                        If Not Session("Location") Is Nothing AndAlso Session("Barcode") Is Nothing Then
                                            html &= "<tr><td class='ItemRowName' style='color:blue'>ItemNo:</td><td>" & drLocation("ItemNumber") & "</td></tr>"
                                        End If
                                        If Not Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                            html &= "<tr><td class='ItemRowName' style='color:black'>ItemNo:</td><td>" & drLocation("ItemNumber") & "</td></tr>"
                                        End If
                                        html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & drLocation("Id") & "'>" & drLocation("ProductName") & "</a></td></tr>"
                                        html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & drLocation("Barcode") & "&SearchType=" & 1 & "'>" & drLocation("Barcode") & "</a></td> </tr>"
                                        html &= "<tr> <td class='ItemRowName'>Location:</td> <td><a href='MobileSearchResult.aspx?Search=" & drLocation("Location") & "&SearchType=" & 1 & "'>" & drLocation("Location") & "</a></td></tr>"
                                        'html &= " <tr><td class='ItemRowName'><img src='' alt='' /></td><td>Price  :$<input type='text' id='txtPrice' name='' /><br /> POD™Price:$<input type='text' id='txtPodPrice' name='' /><br /> <input type='button' id='btnGo' name='Go' /></td></tr>"
                                        html &= "<tr>  <td colspan='2'><img width='100' src='" & ImageName(CInt(drLocation("Id").ToString())) & "' alt='" & drLocation("ImageFileName") & "' /></td> </tr> "
                                        If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 1 Then
                                            html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web <br/> On Consignment</td></tr>"
                                        End If
                                        If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 0 Then
                                            html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web </td></tr>"
                                        End If
                                        If Session("IsNotOnWeb") = 0 AndAlso Session("IsConsignmentItem") = 1 Then
                                            html &= "<tr><td colspan='2' style='background:yellow;'>Status : On Consignment</td></tr>"
                                        End If
                                        html &= " </table>"
                                        nCount = nCount + 1
                                    Next

                                Else
                                    sSQl &= " OR p.Barcode='" & sSearch.ToString() & "' OR p.BarcodeParent='" & sSearch.ToString() & "'"
                                    dtBarcode = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                                    If Not dtBarcode Is Nothing And dtBarcode.Rows.Count > 0 Then
                                        For Each drBarcode As DataRow In dtBarcode.Rows
                                            If Not drBarcode("Location") Is Nothing Then
                                                Session("Location") = drBarcode("Location")
                                            End If
                                            If Not drBarcode("Barcode") Is Nothing Then
                                                Session("Barcode") = drBarcode("Barcode")
                                            End If
                                            If drBarcode("IsNotOnWeb").ToString() <> "" AndAlso Not drBarcode("IsNotOnWeb").ToString() Is Nothing Then
                                                If CInt(drBarcode("IsNotOnWeb").ToString()) > 0 Then
                                                    Session("IsNotOnWeb") = 1
                                                Else
                                                    Session("IsNotOnWeb") = 0
                                                End If
                                            Else
                                                Session("IsNotOnWeb") = 0
                                            End If
                                            If CInt(drBarcode("IsConsignmentItem")) > 0 Then
                                                Session("IsConsignmentItem") = 1
                                            Else
                                                Session("IsConsignmentItem") = 0
                                            End If
                                            html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                            If Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                                html &= "<tr><td class='ItemRowName' style='color:green'>ItemNo:</td><td>" & drBarcode("ItemNumber") & "</td></tr>"
                                            End If
                                            If Not Session("Location") Is Nothing AndAlso Session("Barcode") Is Nothing Then
                                                html &= "<tr><td class='ItemRowName' style='color:blue'>ItemNo:</td><td>" & drBarcode("ItemNumber") & "</td></tr>"
                                            End If
                                            If Not Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                                html &= "<tr><td class='ItemRowName' style='color:black'>ItemNo:</td><td>" & drBarcode("ItemNumber") & "</td></tr>"
                                            End If
                                            html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & drBarcode("Id") & "'>" & drBarcode("ProductName") & "</a></td></tr>"
                                            html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & drBarcode("Barcode") & "&SearchType=" & 1 & "'>" & drBarcode("Barcode") & "</a></td> </tr>"
                                            html &= "<tr> <td class='ItemRowName'>Location:</td> <td><a href='MobileSearchResult.aspx?Search=" & drBarcode("Location") & "&SearchType=" & 1 & "'>" & drBarcode("Location") & "</a></td></tr>"
                                            'html &= " <tr><td class='ItemRowName'><img src='' alt='' /></td><td>Price  :$<input type='text' id='txtPrice' name='' /><br /> POD™Price:$<input type='text' id='txtPodPrice' name='' /><br /> <input type='button' id='btnGo' name='Go' /></td></tr>"
                                            html &= "<tr>  <td colspan='2'><img width='100' src='" & ImageName(CInt(drBarcode("Id").ToString())) & "' alt='" & drBarcode("ImageFileName") & "' /></td> </tr> "
                                            If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 1 Then
                                                html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web <br/> On Consignment</td></tr>"
                                            End If
                                            If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 0 Then
                                                html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web </td></tr>"
                                            End If
                                            If Session("IsNotOnWeb") = 0 AndAlso Session("IsConsignmentItem") = 1 Then
                                                html &= "<tr><td colspan='2' style='background:yellow;'>Status : On Consignment</td></tr>"
                                            End If
                                            html &= " </table>"
                                            nCount = nCount + 1
                                        Next
                                    Else
                                        nCount = 0
                                        DisplayAlert("Result Not Found!!!!")

                                    End If
                                End If

                            End If
                        End If
                        Count.InnerHtml = " " & nCount
                        containDiv.InnerHtml = html

                    Catch ex As Exception

                    End Try
                ElseIf nSearchType = 2 Then
                    Try
                        sSQl &= "SELECT * FROM Product p WHERE "
                        sSQl &= "p.ItemNumber='" & sSearch.ToString() & "'"
                        dt = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                        If Not dt Is Nothing And dt.Rows.Count > 0 Then
                            For Each dr As DataRow In dt.Rows
                                If Not dr("Location") Is Nothing Then
                                    Session("Location") = dr("Location")
                                End If
                                If Not dr("Barcode") Is Nothing Then
                                    Session("Barcode") = dr("Barcode")
                                End If
                                If dr("IsNotOnWeb").ToString() <> "" AndAlso Not dr("IsNotOnWeb").ToString() Is Nothing Then
                                    If CInt(dr("IsNotOnWeb").ToString()) > 0 Then
                                        Session("IsNotOnWeb") = 1
                                    Else
                                        Session("IsNotOnWeb") = 0
                                    End If
                                Else
                                    Session("IsNotOnWeb") = 0
                                End If
                                If CInt(dr("IsConsignmentItem")) > 0 Then
                                    Session("IsConsignmentItem") = 1
                                Else
                                    Session("IsConsignmentItem") = 0
                                End If
                                html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                If Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                    html &= "<tr><td class='ItemRowName' style='color:green'>ItemNo:</td><td>" & dr("ItemNumber") & "</td></tr>"
                                End If
                                If Not Session("Location") Is Nothing AndAlso Session("Barcode") Is Nothing Then
                                    html &= "<tr><td class='ItemRowName' style='color:blue'>ItemNo:</td><td>" & dr("ItemNumber") & "</td></tr>"
                                End If
                                If Not Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                    html &= "<tr><td class='ItemRowName' style='color:black'>ItemNo:</td><td>" & dr("ItemNumber") & "</td></tr>"
                                End If
                                html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & dr("Id") & "'>" & dr("ProductName") & "</a></td></tr>"
                                html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & dr("Barcode") & "&SearchType=" & 2 & "'>" & dr("Barcode") & "</a></td> </tr>"
                                'html &= "<tr> <td class='ItemRowName'>Location:</td> <td>" & dr("Location") & "</td></tr>"
                                html &= " <tr><td ><img width='100' src='" & ImageName(CInt(dr("Id").ToString())) & "' alt='" & dr("ImageFileName") & "' /></td><td>Price  :$<input type='text' id='txtPrice' value='" & (CDbl((dr("Price").ToString())).ToString("0.00")) & "' name='" & dr("Price") & "' /><br /> POD™Price:$<input type='text' id='txtPodPrice' value='" & (CDbl(dr("LowestPrice").ToString()).ToString("0.00")) & "' name='" & dr("LowestPrice") & "' /><br /> <a style='margin-top:10px; float:left;' href='MobileAddItem.aspx?PID=" & dr("Id") & "'>Edit</a></td></tr>"
                                ' html &= "<tr>  <td colspan='2'><img width='100' src='../ProductImages/Large/" & (dr("ImageFileName")) & "' alt='" & dr("ImageFileName") & "' /></td> </tr> 
                                If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 1 Then
                                    html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web <br/> On Consignment</td></tr>"
                                End If
                                If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 0 Then
                                    html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web </td></tr>"
                                End If
                                If Session("IsNotOnWeb") = 0 AndAlso Session("IsConsignmentItem") = 1 Then
                                    html &= "<tr><td colspan='2' style='background:yellow;'>Status : On Consignment</td></tr>"
                                End If
                                html &= " </table>"
                                nCount = nCount + 1
                            Next
                        Else
                            sSQl &= " OR p.ProductName='" & sSearch.ToString() & "'"
                            dtTitle = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                            If Not dtTitle Is Nothing And dtTitle.Rows.Count > 0 Then
                                For Each drTitle As DataRow In dtTitle.Rows
                                    If Not drTitle("Location") Is Nothing Then
                                        Session("Location") = drTitle("Location")
                                    End If
                                    If Not drTitle("Barcode") Is Nothing Then
                                        Session("Barcode") = drTitle("Barcode")
                                    End If
                                    If drTitle("IsNotOnWeb").ToString() <> "" AndAlso Not drTitle("IsNotOnWeb").ToString() Is Nothing Then
                                        If CInt(drTitle("IsNotOnWeb").ToString()) > 0 Then
                                            Session("IsNotOnWeb") = 1
                                        Else
                                            Session("IsNotOnWeb") = 0
                                        End If
                                    Else
                                        Session("IsNotOnWeb") = 0
                                    End If
                                    If CInt(drTitle("IsConsignmentItem")) > 0 Then
                                        Session("IsConsignmentItem") = 1
                                    Else
                                        Session("IsConsignmentItem") = 0
                                    End If
                                    html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                    If Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                        html &= "<tr><td class='ItemRowName' style='color:green'>ItemNo:</td><td>" & drTitle("ItemNumber") & "</td></tr>"
                                    End If
                                    If Not Session("Location") Is Nothing AndAlso Session("Barcode") Is Nothing Then
                                        html &= "<tr><td class='ItemRowName' style='color:blue'>ItemNo:</td><td>" & drTitle("ItemNumber") & "</td></tr>"
                                    End If
                                    If Not Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                        html &= "<tr><td class='ItemRowName' style='color:black'>ItemNo:</td><td>" & drTitle("ItemNumber") & "</td></tr>"
                                    End If
                                    html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & drTitle("Id") & "'>" & drTitle("ProductName") & "</a></td></tr>"
                                    html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & drTitle("Barcode") & "&SearchType=" & 2 & "'>" & drTitle("Barcode") & "</a></td> </tr>"
                                    'html &= "<tr> <td class='ItemRowName'>Location:</td> <td>" & dr("Location") & "</td></tr>"
                                    html &= " <tr><td ><img width='100' src='" & ImageName(CInt(drTitle("Id").ToString())) & "' alt='" & drTitle("ImageFileName") & "' /></td><td>Price  :$<input type='text' id='txtPrice' value='" & (CDbl((drTitle("Price").ToString())).ToString("0.00")) & "' name='" & drTitle("Price") & "' /><br /> POD™Price:$<input type='text' id='txtPodPrice' value='" & (CDbl(drTitle("LowestPrice").ToString()).ToString("0.00")) & "' name='" & drTitle("LowestPrice") & "' /><br /> <a style='margin-top:10px; float:left;' href='MobileAddItem.aspx?PID=" & drTitle("Id") & "'>Edit</a></td></tr>"
                                    If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 1 Then
                                        html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web <br/> On Consignment</td></tr>"
                                    End If
                                    If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 0 Then
                                        html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web </td></tr>"
                                    End If
                                    If Session("IsNotOnWeb") = 0 AndAlso Session("IsConsignmentItem") = 1 Then
                                        html &= "<tr><td colspan='2' style='background:yellow;'>Status : On Consignment</td></tr>"
                                    End If
                                    html &= " </table>"
                                    nCount = nCount + 1
                                Next

                                'Else
                                '    EditSearch = sSearch.ToString().Substring(sSearch.Length - 4)
                                '    sSQl &= " OR p.Location LIKE'%" & EditSearch.ToString() & "'"
                                '    dtLocation = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                                '    If Not dtLocation Is Nothing And dtLocation.Rows.Count > 0 Then
                                '        For Each drLocation As DataRow In dtLocation.Rows
                                '            html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                '            html &= "<tr><td class='ItemRowName'>ItemNo:</td><td>" & drLocation("ItemNumber") & "</td></tr>"
                                '            html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & drLocation("Id") & "'>" & drLocation("ProductName") & "</a></td></tr>"
                                '            html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & drLocation("Barcode") & "&SearchType=" & 2 & "'>" & drLocation("Barcode") & "</a></td> </tr>"
                                '            'html &= "<tr> <td class='ItemRowName'>Location:</td> <td>" & dr("Location") & "</td></tr>"
                                '            html &= " <tr><td ><img width='100' src='../ProductImages/Large/" & (drLocation("ImageFileName")) & "' alt='" & drLocation("ImageFileName") & "' /></td><td>Price  :$<input type='text' id='txtPrice' value='" & (CDbl((drLocation("Price").ToString())).ToString("0.00")) & "' name='" & drLocation("Price") & "' /><br /> POD™Price:$<input type='text' id='txtPodPrice' value='" & (CDbl(drLocation("LowestPrice").ToString()).ToString("0.00")) & "' name='" & drLocation("LowestPrice") & "' /><br /> <a style='margin-top:10px; float:left;' href='MobileAddItem.aspx?PID=" & drLocation("Id") & "'>Edit</a></td></tr>"
                                '            html &= " </table>"
                                '            nCount = nCount + 1
                                '        Next

                            Else
                                sSQl &= " OR p.Barcode='" & sSearch.ToString() & "' OR p.BarcodeParent='" & sSearch.ToString() & "'"
                                dtBarcode = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                                If Not dtBarcode Is Nothing And dtBarcode.Rows.Count > 0 Then
                                    For Each drBarcode As DataRow In dtBarcode.Rows
                                        If Not drBarcode("Location") Is Nothing Then
                                            Session("Location") = drBarcode("Location")
                                        End If
                                        If Not drBarcode("Barcode") Is Nothing Then
                                            Session("Barcode") = drBarcode("Barcode")
                                        End If
                                        If drBarcode("IsNotOnWeb").ToString() <> "" AndAlso Not drBarcode("IsNotOnWeb").ToString() Is Nothing Then
                                            If CInt(drBarcode("IsNotOnWeb").ToString()) > 0 Then
                                                Session("IsNotOnWeb") = 1
                                            Else
                                                Session("IsNotOnWeb") = 0
                                            End If
                                        Else
                                            Session("IsNotOnWeb") = 0
                                        End If
                                        If CInt(drBarcode("IsConsignmentItem")) > 0 Then
                                            Session("IsConsignmentItem") = 1
                                        Else
                                            Session("IsConsignmentItem") = 0
                                        End If
                                        html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                        If Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                            html &= "<tr><td class='ItemRowName' style='color:green'>ItemNo:</td><td>" & drBarcode("ItemNumber") & "</td></tr>"
                                        End If
                                        If Not Session("Location") Is Nothing AndAlso Session("Barcode") Is Nothing Then
                                            html &= "<tr><td class='ItemRowName' style='color:blue'>ItemNo:</td><td>" & drBarcode("ItemNumber") & "</td></tr>"
                                        End If
                                        If Not Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                            html &= "<tr><td class='ItemRowName' style='color:black'>ItemNo:</td><td>" & drBarcode("ItemNumber") & "</td></tr>"
                                        End If
                                        html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & drBarcode("Id") & "'>" & drBarcode("ProductName") & "</a></td></tr>"
                                        html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & drBarcode("Barcode") & "&SearchType=" & 2 & "'>" & drBarcode("Barcode") & "</a></td> </tr>"
                                        'html &= "<tr> <td class='ItemRowName'>Location:</td> <td>" & dr("Location") & "</td></tr>"
                                        html &= " <tr><td ><img width='100' src='" & ImageName(CInt(drBarcode("Id").ToString())) & "' alt='" & drBarcode("ImageFileName") & "' /></td><td>Price  :$<input type='text' id='txtPrice' value='" & (CDbl((drBarcode("Price").ToString())).ToString("0.00")) & "' name='" & drBarcode("Price") & "' /><br /> POD™Price:$<input type='text' id='txtPodPrice' value='" & (CDbl(drBarcode("LowestPrice").ToString()).ToString("0.00")) & "' name='" & drBarcode("LowestPrice") & "' /><br /> <a style='margin-top:10px; float:left;' href='MobileAddItem.aspx?PID=" & drBarcode("Id") & "'>Edit</a></td></tr>"
                                        If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 1 Then
                                            html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web <br/> On Consignment</td></tr>"
                                        End If
                                        If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 0 Then
                                            html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web </td></tr>"
                                        End If
                                        If Session("IsNotOnWeb") = 0 AndAlso Session("IsConsignmentItem") = 1 Then
                                            html &= "<tr><td colspan='2' style='background:yellow;'>Status : On Consignment</td></tr>"
                                        End If
                                        html &= " </table>"
                                        nCount = nCount + 1
                                    Next
                                Else
                                    sSQl &= " OR p.Price='" & sSearch.ToString() & "'"
                                    dtPrice = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                                    If Not dtPrice Is Nothing And dtPrice.Rows.Count > 0 Then
                                        
                                        For Each drPrice As DataRow In dtPrice.Rows
                                            If Not drPrice("Location") Is Nothing Then
                                                Session("Location") = drPrice("Location")
                                            End If
                                            If Not drPrice("Barcode") Is Nothing Then
                                                Session("Barcode") = drPrice("Barcode")
                                            End If
                                            If drPrice("IsNotOnWeb").ToString() <> "" AndAlso Not drPrice("IsNotOnWeb").ToString() Is Nothing Then
                                                If CInt(drPrice("IsNotOnWeb").ToString()) > 0 Then
                                                    Session("IsNotOnWeb") = 1
                                                Else
                                                    Session("IsNotOnWeb") = 0
                                                End If
                                            Else
                                                Session("IsNotOnWeb") = 0
                                            End If
                                            If CInt(drPrice("IsConsignmentItem")) > 0 Then
                                                Session("IsConsignmentItem") = 1
                                            Else
                                                Session("IsConsignmentItem") = 0
                                            End If
                                            html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                            If Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                                html &= "<tr><td class='ItemRowName' style='color:green'>ItemNo:</td><td>" & drPrice("ItemNumber") & "</td></tr>"
                                            End If
                                            If Not Session("Location") Is Nothing AndAlso Session("Barcode") Is Nothing Then
                                                html &= "<tr><td class='ItemRowName' style='color:blue'>ItemNo:</td><td>" & drPrice("ItemNumber") & "</td></tr>"
                                            End If
                                            If Not Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                                html &= "<tr><td class='ItemRowName' style='color:black'>ItemNo:</td><td>" & drPrice("ItemNumber") & "</td></tr>"
                                            End If
                                            html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & drPrice("Id") & "'>" & drPrice("ProductName") & "</a></td></tr>"
                                            html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & drPrice("Barcode") & "&SearchType=" & 2 & "'>" & drPrice("Barcode") & "</a></td> </tr>"
                                            'html &= "<tr> <td class='ItemRowName'>Location:</td> <td>" & dr("Location") & "</td></tr>"
                                            html &= " <tr><td ><img width='100' src='" & ImageName(CInt(drPrice("Id").ToString())) & "' alt='" & drPrice("ImageFileName") & "' /></td><td>Price  :$<input type='text' id='txtPrice' value='" & (CDbl((drPrice("Price").ToString())).ToString("0.00")) & "' name='" & drPrice("Price") & "' /><br /> POD™Price:$<input type='text' id='txtPodPrice' value='" & (CDbl(drPrice("LowestPrice").ToString()).ToString("0.00")) & "' name='" & drPrice("LowestPrice") & "' /><br /> <a style='margin-top:10px; float:left;' href='MobileAddItem.aspx?PID=" & drPrice("Id") & "'>Edit</a></td></tr>"
                                            If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 1 Then
                                                html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web <br/> On Consignment</td></tr>"
                                            End If
                                            If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 0 Then
                                                html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web </td></tr>"
                                            End If
                                            If Session("IsNotOnWeb") = 0 AndAlso Session("IsConsignmentItem") = 1 Then
                                                html &= "<tr><td colspan='2' style='background:yellow;'>Status : On Consignment</td></tr>"
                                            End If
                                            html &= " </table>"
                                            nCount = nCount + 1
                                        Next
                                    Else
                                        nCount = 0
                                        DisplayAlert("Result Not Found!!!!")
                                    End If
                                End If
                                'End If

                            End If
                        End If
                        Count.InnerHtml = " " & nCount
                        containDiv.InnerHtml = html

                    Catch ex As Exception

                    End Try
                Else
                    Try
                        sSQl &= "SELECT * FROM Product p WHERE "
                        sSQl &= "p.ItemNumber='" & sSearch.ToString() & "'"
                        dt = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                        If Not dt Is Nothing And dt.Rows.Count > 0 Then
                            
                            For Each dr As DataRow In dt.Rows
                                If Not dr("Location") Is Nothing Then
                                    Session("Location") = dr("Location")
                                End If
                                If Not dr("Barcode") Is Nothing Then
                                    Session("Barcode") = dr("Barcode")
                                End If
                                If dr("IsNotOnWeb").ToString() <> "" AndAlso Not dr("IsNotOnWeb").ToString() Is Nothing Then
                                    If CInt(dr("IsNotOnWeb").ToString()) > 0 Then
                                        Session("IsNotOnWeb") = 1
                                    Else
                                        Session("IsNotOnWeb") = 0
                                    End If
                                Else
                                    Session("IsNotOnWeb") = 0
                                End If
                                If CInt(dr("IsConsignmentItem")) > 0 Then
                                    Session("IsConsignmentItem") = 1
                                Else
                                    Session("IsConsignmentItem") = 0
                                End If
                                html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                If Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                    html &= "<tr><td class='ItemRowName' style='color:green'>ItemNo:</td><td>" & dr("ItemNumber") & "</td></tr>"
                                End If
                                If Not Session("Location") Is Nothing AndAlso Session("Barcode") Is Nothing Then
                                    html &= "<tr><td class='ItemRowName' style='color:blue'>ItemNo:</td><td>" & dr("ItemNumber") & "</td></tr>"
                                End If
                                If Not Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                    html &= "<tr><td class='ItemRowName' style='color:black'>ItemNo:</td><td>" & dr("ItemNumber") & "</td></tr>"
                                End If
                                html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & dr("Id") & "'>" & dr("ProductName") & "</a></td></tr>"
                                html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & dr("Barcode") & "&SearchType=" & 3 & "'>" & dr("Barcode") & "</a></td> </tr>"
                                html &= "<tr> <td class='ItemRowName'>Location:</td> <td><a href='MobileSearchResult.aspx?Search=" & dr("Location") & "&SearchType=" & 3 & "'>" & dr("Location") & "</a><a style='margin-left:25px;' href='MobileAddItem.aspx?PID=" & dr("Id") & "'>Edit</a></td></tr>"
                                'html &= " <tr><td class='ItemRowName'><img src='' alt='' /></td><td>Price  :$<input type='text' id='txtPrice' name='' /><br /> POD™Price:$<input type='text' id='txtPodPrice' name='' /><br /> <input type='button' id='btnGo' name='Go' /></td></tr>"
                                html &= "<tr>  <td colspan='2'><img width='100' src='" & ImageName(CInt(dr("Id").ToString())) & "' alt='" & dr("ImageFileName") & "' /></td> </tr> "
                                If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 1 Then
                                    html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web <br/> On Consignment</td></tr>"
                                End If
                                If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 0 Then
                                    html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web </td></tr>"
                                End If
                                If Session("IsNotOnWeb") = 0 AndAlso Session("IsConsignmentItem") = 1 Then
                                    html &= "<tr><td colspan='2' style='background:yellow;'>Status : On Consignment</td></tr>"
                                End If
                                html &= " </table>"
                                nCount = nCount + 1
                            Next
                        Else
                            sSQl &= " OR p.ProductName='" & sSearch.ToString() & "'"
                            dtTitle = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                            If Not dtTitle Is Nothing And dtTitle.Rows.Count > 0 Then
                                For Each drTitle As DataRow In dtTitle.Rows
                                    If Not drTitle("Location") Is Nothing Then
                                        Session("Location") = drTitle("Location")
                                    End If
                                    If Not drTitle("Barcode") Is Nothing Then
                                        Session("Barcode") = drTitle("Barcode")
                                    End If
                                    If drTitle("IsNotOnWeb").ToString() <> "" AndAlso Not drTitle("IsNotOnWeb").ToString() Is Nothing Then
                                        If CInt(drTitle("IsNotOnWeb").ToString()) > 0 Then
                                            Session("IsNotOnWeb") = 1
                                        Else
                                            Session("IsNotOnWeb") = 0
                                        End If
                                    Else
                                        Session("IsNotOnWeb") = 0
                                    End If
                                    If CInt(drTitle("IsConsignmentItem")) > 0 Then
                                        Session("IsConsignmentItem") = 1
                                    Else
                                        Session("IsConsignmentItem") = 0
                                    End If
                                    html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                    If Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                        html &= "<tr><td class='ItemRowName' style='color:green'>ItemNo:</td><td>" & drTitle("ItemNumber") & "</td></tr>"
                                    End If
                                    If Not Session("Location") Is Nothing AndAlso Session("Barcode") Is Nothing Then
                                        html &= "<tr><td class='ItemRowName' style='color:blue'>ItemNo:</td><td>" & drTitle("ItemNumber") & "</td></tr>"
                                    End If
                                    If Not Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                        html &= "<tr><td class='ItemRowName' style='color:black'>ItemNo:</td><td>" & drTitle("ItemNumber") & "</td></tr>"
                                    End If
                                    html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & drTitle("Id") & "'>" & drTitle("ProductName") & "</a></td></tr>"
                                    html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & drTitle("Barcode") & "&SearchType=" & 3 & "'>" & drTitle("Barcode") & "</a></td> </tr>"
                                    html &= "<tr> <td class='ItemRowName'>Location:</td> <td><a href='MobileSearchResult.aspx?Search=" & drTitle("Location") & "&SearchType=" & 3 & "'>" & drTitle("Location") & "</a><a style='margin-left:25px;' href='MobileAddItem.aspx?PID=" & drTitle("Id") & "'>Edit</a></td></tr>"
                                    'html &= " <tr><td class='ItemRowName'><img src='' alt='' /></td><td>Price  :$<input type='text' id='txtPrice' name='' /><br /> POD™Price:$<input type='text' id='txtPodPrice' name='' /><br /> <input type='button' id='btnGo' name='Go' /></td></tr>"
                                    html &= "<tr> <td colspan='2'><img width='100' src='" & ImageName(CInt(drTitle("Id").ToString())) & "' alt='" & drTitle("ImageFileName") & "' /></td> </tr> "
                                    If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 1 Then
                                        html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web <br/> On Consignment</td></tr>"
                                    End If
                                    If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 0 Then
                                        html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web </td></tr>"
                                    End If
                                    If Session("IsNotOnWeb") = 0 AndAlso Session("IsConsignmentItem") = 1 Then
                                        html &= "<tr><td colspan='2' style='background:yellow;'>Status : On Consignment</td></tr>"
                                    End If
                                    html &= " </table>"
                                    nCount = nCount + 1
                                Next

                            Else

                                EditSearch = sSearch.ToString().Substring(sSearch.Length - 4)
                                sSQl &= " OR p.Location LIKE'%" & EditSearch.ToString() & "'"
                                dtLocation = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                                If Not dtLocation Is Nothing And dtLocation.Rows.Count > 0 Then
                                    For Each drLocation As DataRow In dtLocation.Rows
                                        If Not drLocation("Location") Is Nothing Then
                                            Session("Location") = drLocation("Location")
                                        End If
                                        If Not drLocation("Barcode") Is Nothing Then
                                            Session("Barcode") = drLocation("Barcode")
                                        End If
                                        If drLocation("IsNotOnWeb").ToString() <> "" AndAlso Not drLocation("IsNotOnWeb").ToString() Is Nothing Then
                                            If CInt(drLocation("IsNotOnWeb").ToString()) > 0 Then
                                                Session("IsNotOnWeb") = 1
                                            Else
                                                Session("IsNotOnWeb") = 0
                                            End If
                                        Else
                                            Session("IsNotOnWeb") = 0
                                        End If
                                        If CInt(drLocation("IsConsignmentItem")) > 0 Then
                                            Session("IsConsignmentItem") = 1
                                        Else
                                            Session("IsConsignmentItem") = 0
                                        End If
                                        html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                        If Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                            html &= "<tr><td class='ItemRowName' style='color:green'>ItemNo:</td><td>" & drLocation("ItemNumber") & "</td></tr>"
                                        End If
                                        If Not Session("Location") Is Nothing AndAlso Session("Barcode") Is Nothing Then
                                            html &= "<tr><td class='ItemRowName' style='color:blue'>ItemNo:</td><td>" & drLocation("ItemNumber") & "</td></tr>"
                                        End If
                                        If Not Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                            html &= "<tr><td class='ItemRowName' style='color:black'>ItemNo:</td><td>" & drLocation("ItemNumber") & "</td></tr>"
                                        End If
                                        html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & drLocation("Id") & "'>" & drLocation("ProductName") & "</a></td></tr>"
                                        html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & drLocation("Barcode") & "&SearchType=" & 3 & "'>" & drLocation("Barcode") & "</a></td> </tr>"
                                        html &= "<tr> <td class='ItemRowName'>Location:</td> <td><a href='MobileSearchResult.aspx?Search=" & drLocation("Location") & "&SearchType=" & 3 & "'>" & drLocation("Location") & "</a><a style='margin-left:25px;' href='MobileAddItem.aspx?PID=" & drLocation("Id") & "'>Edit</a></td></tr>"
                                        'html &= " <tr><td class='ItemRowName'><img src='' alt='' /></td><td>Price  :$<input type='text' id='txtPrice' name='' /><br /> POD™Price:$<input type='text' id='txtPodPrice' name='' /><br /> <input type='button' id='btnGo' name='Go' /></td></tr>"
                                        html &= "<tr> <td colspan='2'><img width='100' src='" & ImageName(CInt(drLocation("Id").ToString())) & "' alt='" & drLocation("ImageFileName") & "' /></td> </tr> "
                                        If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 1 Then
                                            html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web <br/> On Consignment</td></tr>"
                                        End If
                                        If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 0 Then
                                            html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web </td></tr>"
                                        End If
                                        If Session("IsNotOnWeb") = 0 AndAlso Session("IsConsignmentItem") = 1 Then
                                            html &= "<tr><td colspan='2' style='background:yellow;'>Status : On Consignment</td></tr>"
                                        End If
                                        html &= " </table>"
                                        nCount = nCount + 1
                                    Next

                                Else
                                    sSQl &= " OR p.Barcode='" & sSearch.ToString() & "' OR p.BarcodeParent='" & sSearch.ToString() & "'"
                                    dtBarcode = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING).Tables(0)
                                    If Not dtBarcode Is Nothing And dtBarcode.Rows.Count > 0 Then
                                        For Each drBarcode As DataRow In dtBarcode.Rows
                                            If Not drBarcode("Location") Is Nothing Then
                                                Session("Location") = drBarcode("Location")
                                            End If
                                            If Not drBarcode("Barcode") Is Nothing Then
                                                Session("Barcode") = drBarcode("Barcode")
                                            End If
                                            If drBarcode("IsNotOnWeb").ToString() <> "" AndAlso Not drBarcode("IsNotOnWeb").ToString() Is Nothing Then
                                                If CInt(drBarcode("IsNotOnWeb").ToString()) > 0 Then
                                                    Session("IsNotOnWeb") = 1
                                                Else
                                                    Session("IsNotOnWeb") = 0
                                                End If
                                            Else
                                                Session("IsNotOnWeb") = 0
                                            End If
                                            If CInt(drBarcode("IsConsignmentItem")) > 0 Then
                                                Session("IsConsignmentItem") = 1
                                            Else
                                                Session("IsConsignmentItem") = 0
                                            End If
                                            html &= "<table class='container_MobileSearch' width='98%;' cellspacing='5' cellpadding='5'>"
                                            If Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                                html &= "<tr><td class='ItemRowName' style='color:green'>ItemNo:</td><td>" & drBarcode("ItemNumber") & "</td></tr>"
                                            End If
                                            If Not Session("Location") Is Nothing AndAlso Session("Barcode") Is Nothing Then
                                                html &= "<tr><td class='ItemRowName' style='color:blue'>ItemNo:</td><td>" & drBarcode("ItemNumber") & "</td></tr>"
                                            End If
                                            If Not Session("Location") Is Nothing AndAlso Not Session("Barcode") Is Nothing Then
                                                html &= "<tr><td class='ItemRowName' style='color:black'>ItemNo:</td><td>" & drBarcode("ItemNumber") & "</td></tr>"
                                            End If
                                            html &= "<tr><td class='ItemRowName'>Title:</td><td><a href='MobileAddItem.aspx?PID=" & drBarcode("Id") & "'>" & drBarcode("ProductName") & "</a></td></tr>"
                                            html &= "<tr> <td class='ItemRowName'>Barcode</td> <td><a href='MobileSearchResult.aspx?Search=" & drBarcode("Barcode") & "&SearchType=" & 3 & "'>" & drBarcode("Barcode") & "</a></td> </tr>"
                                            html &= "<tr> <td class='ItemRowName'>Location:</td> <td><a href='MobileSearchResult.aspx?Search=" & drBarcode("Location") & "&SearchType=" & 3 & "'>" & drBarcode("Location") & "</a><a style='margin-left:25px;' href='MobileAddItem.aspx?PID=" & drBarcode("Id") & "'>Edit</a></td></tr>"
                                            'html &= " <tr><td class='ItemRowName'><img src='' alt='' /></td><td>Price  :$<input type='text' id='txtPrice' name='' /><br /> POD™Price:$<input type='text' id='txtPodPrice' name='' /><br /> <input type='button' id='btnGo' name='Go' /></td></tr>"
                                            html &= "<tr> <td colspan='2'><img width='100' src='" & ImageName(CInt(drBarcode("Id").ToString())) & "' alt='" & drBarcode("ImageFileName") & "' /></td> </tr> "
                                            If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 1 Then
                                                html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web <br/> On Consignment</td></tr>"
                                            End If
                                            If Session("IsNotOnWeb") = 1 AndAlso Session("IsConsignmentItem") = 0 Then
                                                html &= "<tr><td colspan='2' style='background:yellow;'>Status :  Not on Web </td></tr>"
                                            End If
                                            If Session("IsNotOnWeb") = 0 AndAlso Session("IsConsignmentItem") = 1 Then
                                                html &= "<tr><td colspan='2' style='background:yellow;'>Status : On Consignment</td></tr>"
                                            End If
                                            html &= " </table>"
                                            nCount = nCount + 1
                                        Next
                                    Else
                                        nCount = 0
                                        DisplayAlert("Result Not Found!!!!")

                                    End If
                                End If

                            End If
                        End If
                        Count.InnerHtml = nCount
                        containDiv.InnerHtml = html

                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
End Class
