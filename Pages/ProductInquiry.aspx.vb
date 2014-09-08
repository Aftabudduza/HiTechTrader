Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Pages_ProductInquiry
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
    Private strImageFilePath As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageFilePath")
    Private strImageURL As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageURL")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            fill_ddlStates()
            fill_ddlCountry()
            Session("ProductId") = Nothing
            Session("ProductName") = Nothing
            Session("Item") = Nothing
            Session("img") = Nothing
            Session("PODPrice") = Nothing
            Session("Price") = Nothing

            Try
                ProductId = CInt(Request.QueryString("Id").ToString())
            Catch ex As Exception
                ProductId = 0
            End Try
            If ProductId > 0 Then
                Session("ProductId") = ProductId
            End If
            If Not Session("ProductId") Is Nothing Then
                fill_MyProduct(CInt(Session("ProductId").ToString()))
            End If
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

            If Not UserDS.Tables(0).Rows(0)("MainImage").ToString() Is Nothing AndAlso UserDS.Tables(0).Rows(0)("MainImage").ToString() <> String.Empty Then
                strImg = UserDS.Tables(0).Rows(0)("MainImage").ToString()
                Dim imgArray() As String = Nothing
                Dim sStr As String = ""
                Dim sStrFile As String = ""
                If strImg.Length > 0 Then
                    If Not String.IsNullOrEmpty(strImageFilePath) Then
                        sStrFile = strImageFilePath & "\" & strImg
                    Else
                        sStrFile = "C:\inetpub\wwwroot\HitechTrader\ProductImages\Large\" & strImg
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
                    End If
                    If Not ds.Tables(0).Rows(0)("ProductName") Is Nothing AndAlso ds.Tables(0).Rows(0)("ProductName").ToString() <> "" Then
                        spanTitle.InnerHtml = ds.Tables(0).Rows(0)("ProductName").ToString()
                        Session("ProductName") = ds.Tables(0).Rows(0)("ProductName").ToString()
                    End If
                    If Not ds.Tables(0).Rows(0)("ItemNumber") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("ItemNumber").ToString()) > 0 Then
                        ItemNumb.InnerHtml = CInt(ds.Tables(0).Rows(0)("ItemNumber").ToString())
                        Session("Item") = CInt(ds.Tables(0).Rows(0)("ItemNumber").ToString())
                    End If
                    If Not ds.Tables(0).Rows(0)("ImageFileName") Is Nothing AndAlso ds.Tables(0).Rows(0)("ImageFileName").ToString() <> "" Then
                        Dim sb As String = ""
                        sb += "<div class=''><img src='" & ImageName(nProductID) & "' alt='" & ds.Tables(0).Rows(0)("ImageFileName") & "' Height='auto' class='thumbnailImage' id='GalMainImg' style='float: left;'></div>"
                        GallSub.InnerHtml = sb
                        Session("img") = ds.Tables(0).Rows(0)("ImageFileName")
                    End If
                    ReturnProduct.Attributes.Add("href", "ProductDetails.aspx?Id=" & CInt(Session("ProductId").ToString()) & "")
                    prodetails.Attributes.Add("href", "ProductDetails.aspx?Id=" & CInt(Session("ProductId").ToString()) & "")
                    If Not CDbl(ds.Tables(0).Rows(0)("Price").ToString()).ToString("0.00") > 0 Then
                        OptionAPrice.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)("Price").ToString()).ToString("0.00")
                        Session("Price") = ("$" & CDbl(ds.Tables(0).Rows(0)("Price").ToString()).ToString("0.00")).ToString()
                        pleasecall.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)("Price").ToString()).ToString("0.00")
                    Else
                        OptionAPrice.InnerHtml = "Please Call"
                        pleasecall.InnerHtml = "Please Call"
                    End If
                    If Not CDbl(ds.Tables(0).Rows(0)("LowestPrice").ToString()).ToString("0.00") > 0 Then
                        PODPrice.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)("LowestPrice").ToString()).ToString("0.00")
                        Session("PODPrice") = ("$" & CDbl(ds.Tables(0).Rows(0)("LowestPrice").ToString()).ToString("0.00")).ToString()
                    Else
                        PODPrice.InnerHtml = "$0.00"
                    End If
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub fill_ddlStates()
        Try
            Dim sSQl As String = "Select [State], StateName from refstates order by StateName"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlState.Items.Clear()
            ddlState.AppendDataBoundItems = True
            ddlState.Items.Add(New ListItem("-----------  Others  ----------", "-1"))
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.ddlState.Items.Add(New ListItem(dr("stateName"), dr("state")))
            Next
            ddlState.SelectedValue = "-1"
        Catch ex As Exception

        End Try

    End Sub
    Private Sub fill_ddlCountry()
        Try
            Dim sSQl As String = "Select [COUNTRY], COUNTRYNAME from REFCOUNTRIES order by COUNTRYNAME"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlcountry.Items.Clear()
            ddlcountry.AppendDataBoundItems = True
            ddlcountry.Items.Add(New ListItem("-----------  Others  ----------", "-1"))
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.ddlcountry.Items.Add(New ListItem(dr("COUNTRYNAME"), dr("COUNTRY")))
            Next
            ddlcountry.SelectedValue = "US"
        Catch ex As Exception

        End Try

    End Sub
    Public Sub SendEmail()
        Try
            Dim strMailServer As String = String.Empty
            Dim strMailUser As String = ""
            Dim strMailPassword As String = ""
            Dim strMailPort As String = System.Configuration.ConfigurationManager.AppSettings.Get("strMailPort")
            Dim isMailLive As String = System.Configuration.ConfigurationManager.AppSettings.Get("isMailLive")

            strMailUser = System.Configuration.ConfigurationManager.AppSettings.Get("strMailUser")
            strMailPassword = System.Configuration.ConfigurationManager.AppSettings.Get("strMailPassword")
            strMailServer = System.Configuration.ConfigurationManager.AppSettings.Get("strMailServer")

            If isMailLive = "true" Then
                objSmtpClient = New Net.Mail.SmtpClient(strMailServer, strMailPort)
                objSmtpClient.UseDefaultCredentials = False
                objSmtpClient.Credentials = New Net.NetworkCredential(strMailUser, strMailPassword)
            Else
                objSmtpClient = New Net.Mail.SmtpClient("smtp.gmail.com", 587)
                objSmtpClient.UseDefaultCredentials = False
                objSmtpClient.EnableSsl = True
                objSmtpClient.Credentials = New Net.NetworkCredential("adi.email.test@gmail.com", "adiadmin123")
            End If
            Dim from_address As String = ""
            Dim to_address As String = ""
            from_address = txtEmail.Text.ToString().Trim
            Try
                to_address = System.Configuration.ConfigurationManager.AppSettings.Get("toAddress")
            Catch ex As Exception
                to_address = "Sales@HiTechTrader.com"
            End Try

            objMailMessage = New Net.Mail.MailMessage
            objMailMessage.From = New Net.Mail.MailAddress(from_address)
            objMailMessage.To.Add(New Net.Mail.MailAddress(to_address))
            objMailMessage.Subject = "HiTech Trader Product Request"
            objMailMessage.IsBodyHtml = True
            objMailMessage.Body = Me.EmailHtml.ToString
            objSmtpClient.Send(objMailMessage)
            DisplayAlert("Thank you for your Inquiry. A sales representative will contact with you shortly.")

        Catch
            DisplayAlert("Sorry for the inconvenience. We can not process your request at this time.")
        Finally
            If (objSmtpClient Is Nothing) = False Then
                objSmtpClient = Nothing
            End If
            If (objMailMessage Is Nothing) = False Then
                objMailMessage.Dispose()
                objMailMessage = Nothing
            End If
        End Try
    End Sub
    Public Function EmailHtml() As System.Text.StringBuilder
        Dim sendUpdate As String = "NO"
        Dim emailbody As New System.Text.StringBuilder
        emailbody.Append("<h2>HiTechTrader Product Request</h2>")
        emailbody.Append("<table>")
        emailbody.Append("<tr><td colspan='2'></td></tr>")
        emailbody.Append("<tr><td colspan='2' style='text-align:center;font-size:14px;font-weight:bold;color:0000cc;'>Product Info.</td></tr>")
        emailbody.Append("<tr><td> Product Name </td><td> " & Session("ProductName").ToString().Trim() & "</td></tr>")
        emailbody.Append("<tr><td> Item No : </td><td> " & CInt(Session("Item").ToString().Trim()) & "</td></tr>")
        emailbody.Append("<tr><td colspan='2'></td></tr>")
        'emailbody.Append("<tr><td> Image </td><td><img src='http://192.82.249.221/HitechTrader/ProductImages/Large/" & Session("img") & "' alt='" & Session("img") & "' Height='130px' width='130px' style='float: left;'></td></tr>")
        emailbody.Append("<tr><td colspan='2' style='text-align:center;font-size:14px;font-weight:bold;color:0000cc;'>Buying Interest</td></tr>")
        If rdoIsInquery.SelectedIndex <> -1 Then
            emailbody.Append("<tr><td> Order/Inquiry </td><td> " & rdoIsInquery.SelectedValue.ToString().Trim() & "</td></tr>")
        End If
        If rdoPrice.Checked = True Then
            If Not Session("Price") Is Nothing Then
                emailbody.Append("<tr><td> Price: </td><td> " & Session("Price").ToString().Trim() & "</td></tr>")
            Else
                emailbody.Append("<tr><td> Price: </td><td> Please Call </td></tr>")
            End If

        End If
        If rdoPod.Checked = True Then
            If Not Session("PODPrice") Is Nothing Then
                emailbody.Append("<tr><td>POD Price: </td><td> " & Session("PODPrice").ToString().Trim() & "</td></tr>")
            Else
                emailbody.Append("<tr><td>POD Price: </td><td> $0.00 </td></tr>")
            End If
        End If
        emailbody.Append("<tr><td colspan='2'></td></tr>")
        emailbody.Append("<tr><td colspan='2' style='text-align:center;font-size:14px;font-weight:bold;color:0000cc;'>Payment Options</td></tr>")
        If chkcredit.Checked = True Then
            emailbody.Append("<tr><td>  Credit Card: </td><td> " & ddlcard.SelectedItem.Text.ToString().Trim() & "</td></tr>")
        End If
        If chkCheck.Checked = True Then
            emailbody.Append("<tr><td>  Check: </td><td> True</td></tr>")
        End If
        If chkPurchaseOrder.Checked = True Then
            emailbody.Append("<tr><td>  PO #: </td><td> " & txtPO.Text.ToString().Trim() & "</td></tr>")
        End If
        If chkOthers.Checked = True Then
            emailbody.Append("<tr><td> Other: </td><td> Other</td></tr>")
        End If
        emailbody.Append("<tr><td colspan='2'></td></tr>")
        emailbody.Append("<tr><td colspan='2' style='text-align:center;font-size:14px;font-weight:bold;color:0000cc;'>Contact Information </td></tr>")
        emailbody.Append("<tr><td> First Name: </td><td> " & txtName.Text.ToString().Trim() & "</td></tr>")
        emailbody.Append("<tr><td> Company Name: </td><td> " & txtCompany.Text.ToString().Trim() & "</td></tr>")
        emailbody.Append("<tr><td> Phone: </td><td> " & txtPhone.Text.ToString().Trim() & "</td></tr>")
        If Not String.IsNullOrEmpty(txtFax.Text.ToString) Then
            emailbody.Append("<tr><td> Fax: </td><td> " & txtFax.Text.ToString().Trim() & "</td></tr>")
        End If
        emailbody.Append("<tr><td> Email Address: </td><td> " & txtEmail.Text.ToString().Trim() & "</td></tr>")
        emailbody.Append("<tr><td> Addres 1: </td><td> " & txtAddress1.Text.ToString().Trim() & "</td></tr>")
        If Not String.IsNullOrEmpty(txtAdd2.Text.ToString) Then
            emailbody.Append("<tr><td> Address 2: </td><td> " & txtAdd2.Text.ToString().Trim() & "</td></tr>")
        End If
        emailbody.Append("<tr><td> City: </td><td> " & txtCity.Text.ToString().Trim() & "</td></tr>")
        emailbody.Append("<tr><td> State: </td><td> " & ddlState.SelectedItem.Text.ToString().Trim() & "</td></tr>")
        emailbody.Append("<tr><td> Country: </td><td> " & ddlcountry.SelectedItem.Text.ToString().Trim() & "</td></tr>")
        emailbody.Append("<tr><td> Zip Code: </td><td> " & txtZipCode.Text.ToString().Trim() & "</td></tr>")
        emailbody.Append("<tr><td> Message: </td><td> " & txtDetails.Text.ToString().Trim() & "</td></tr>")
        emailbody.Append("</table>")
        Return emailbody
    End Function
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Public Function Validate_Control() As String
        Try
            If rdoIsInquery.SelectedIndex = -1 Then
                errStr &= "Please select a Inquiry or Order Items" & Environment.NewLine
            End If
            If Len(txtName.Text.Trim()) <= 0 Then
                errStr &= "Please enter Your Name" & Environment.NewLine
            End If
            If Len(txtCompany.Text.Trim()) <= 0 Then
                errStr &= "Please enter Company Name" & Environment.NewLine
            End If
            If Len(txtEmail.Text.Trim()) <= 0 Then
                errStr &= "Please enter email address" & Environment.NewLine
            End If
            If Len(txtPhone.Text.Trim()) <= 0 Then
                errStr &= "Please enter Phone Number" & Environment.NewLine
            End If

            If Len(txtAddress1.Text.Trim()) <= 0 Then
                errStr &= "Please enter  address" & Environment.NewLine
            End If
            If ddlState.SelectedValue = "-1" Then
                errStr &= "Please select State" & Environment.NewLine
            End If
            If ddlcountry.SelectedValue = "-1" Then
                errStr &= "Please select Country" & Environment.NewLine
            End If
            If Len(txtEmail.Text.Trim()) <= 0 Then
                errStr &= "Please enter email address" & Environment.NewLine
            Else
                If Not ValidEmail(txtEmail.Text.ToString().Trim) Then
                    errStr &= "Invalid email address" & Environment.NewLine
                End If
            End If
            If Len(txtZipCode.Text.Trim()) <= 0 Then
                errStr &= "Please enter Zip Code" & Environment.NewLine
            End If
            If Len(txtDetails.Text.Trim()) <= 0 Then
                errStr &= "Please enter details message" & Environment.NewLine
            End If


            If chkPurchaseOrder.Checked = True Then
                If String.IsNullOrEmpty(txtPO.Text.ToString()) Then
                    errStr &= "Please enter PO No" & Environment.NewLine
                End If
            End If
        Catch ex As Exception

        End Try
        
        Return errStr
    End Function
    Public Function ValidEmail(ByVal value As String) As Boolean
        If (value Is Nothing) Then Return False
        Return reEmail.IsMatch(value)
    End Function
    Function MakeDBString(ByVal value As String) As String
        Return "'" & Replace(value, "'", "''") & "'"
    End Function
    Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSend.Click
        errStr = String.Empty
        Dim Obj As New OrderHistory(appGlobal.CONNECTIONSTRING)
        errStr = Validate_Control()
        Obj = Setdata(Obj)
        If errStr.Length <= 0 Then
            Try
                If Obj.InsertInquiry() > 0 Then

                End If
                SendEmail()
                ClearControls()
            Catch ex As Exception
                DisplayAlert(errStr)
            End Try
        End If
    End Sub
    Public Function Setdata(ByVal Obj As OrderHistory) As OrderHistory
        Try
            If txtName.Text.ToString().Trim.Length > 0 Then
                Obj.Name = txtName.Text.ToString().Trim
            Else
                Obj.Name = ""
            End If
            If txtCompany.Text.ToString().Trim.Length > 0 Then
                Obj.Company = txtCompany.Text.ToString().Trim
            Else
                Obj.Company = ""
            End If
            If txtEmail.Text.ToString().Trim.Length > 0 Then
                Obj.Email = txtEmail.Text.ToString().Trim
            Else
                Obj.Email = ""
            End If
            If txtAddress1.Text.ToString().Trim.Length > 0 Then
                Obj.Address = txtAddress1.Text.ToString().Trim
            Else
                Obj.Address = ""
            End If
            If txtAdd2.Text.ToString().Trim.Length > 0 Then
                Obj.Address1 = txtAdd2.Text.ToString().Trim
            Else
                Obj.Address1 = ""
            End If
            If txtCity.Text.ToString().Trim.Length > 0 Then
                Obj.City = txtCity.Text.ToString().Trim
            Else
                Obj.City = ""
            End If
            If ddlState.SelectedValue <> "-1" Then
                Obj.State = ddlState.SelectedValue
            Else
                Obj.State = "-1"
            End If
            If txtZipCode.Text.ToString().Trim.Length > 0 Then
                Obj.Zip = txtZipCode.Text.ToString().Trim
            Else
                Obj.Zip = ""
            End If
            If ddlcountry.SelectedValue <> "-1" Then
                Obj.Country = ddlcountry.SelectedValue
            Else
                Obj.Country = "-1"
            End If
            If txtPhone.Text.ToString().Trim.Length > 0 Then
                Obj.Phone = txtPhone.Text.ToString().Trim
            Else
                Obj.Phone = ""
            End If
            If txtFax.Text.ToString().Trim.Length > 0 Then
                Obj.Fax = txtFax.Text.ToString().Trim
            Else
                Obj.Fax = ""
            End If
            If txtDetails.Text.ToString().Trim.Length > 0 Then
                Obj.Message = txtDetails.Text.ToString().Trim
            Else
                Obj.Message = ""
            End If
            Obj.ItemsToSale = ""
            Obj.ItemToPurchase = ""
            Obj.IsNeedASAP = 0
            Obj.IsNeedFuture = 0
            If chkAdd.Checked Then
                Obj.IsAdd = 1
            Else
                Obj.IsAdd = 0
            End If
            If chkContact.Checked Then
                Obj.IsContact = 1
            Else
                Obj.IsContact = 0
            End If
            If chkCheck.Checked Then
                Obj.IsCheck = 1
            Else
                Obj.IsCheck = 0
            End If
            If chkOthers.Checked Then
                Obj.IsOther = 1
            Else
                Obj.IsOther = 0
            End If
            If txtPO.Text.ToString().Trim.Length > 0 Then
                Obj.PONumber = txtPO.Text.ToString().Trim
            Else
                Obj.PONumber = ""
            End If
            Obj.CardType = ddlcard.SelectedValue
            If rdoIsInquery.Items(0).Selected = True Then
                Obj.Type = "Order"
            Else
                Obj.Type = "Inquiry"
            End If
            If rdoPrice.Checked Then
                Obj.OptionPrice = "Option A"
                If Not Session("Price") Is Nothing Then
                    Obj.Price = CDbl(Session("Price").ToString())
                Else
                    Obj.Price = 0
                End If
            Else
                Obj.OptionPrice = "Option B"
                If Not Session("PODPrice") Is Nothing Then
                    Obj.Price = CDbl(Session("PODPrice").ToString())
                Else
                    Obj.Price = 0
                End If
            End If
            Obj.OrderDate = CDate(DateTime.UtcNow.ToString())
        Catch ex As Exception

        End Try

        Return Obj
    End Function
    Public Sub ClearControls()
        txtName.Text = ""
        txtCompany.Text = ""
        txtPhone.Text = ""
        txtCity.Text = ""
        txtEmail.Text = ""
        txtAdd2.Text = ""
        txtAddress1.Text = ""
        txtFax.Text = ""
        ddlState.SelectedValue = "-1"
        ddlcountry.SelectedValue = "-1"
        ddlcard.SelectedValue = "1"
        txtZipCode.Text = ""
        rdoIsInquery.SelectedIndex = -1
        rdoPod.Checked = False
        rdoPrice.Checked = False
        chkCheck.Checked = False
        chkcredit.Checked = False
        chkOthers.Checked = False
        chkPurchaseOrder.Checked = False

    End Sub
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        ClearControls()
    End Sub
End Class
