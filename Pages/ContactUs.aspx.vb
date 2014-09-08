Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Pages_ContactUs
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
            objMailMessage.Subject = "HiTech Trader Contact Request"
            objMailMessage.IsBodyHtml = True
            objMailMessage.Body = Me.EmailHtml.ToString
            objSmtpClient.Send(objMailMessage)
            DisplayAlert("Thank you for your request. A sales representative will contact with you shortly.")

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
        emailbody.Append("<h2>Hi Tech Trader Contact Request</h2>")
        emailbody.Append("<table>")
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
        emailbody.Append("<tr><td> Message: </td><td> " & txtMessage.Text.ToString().Trim() & "</td></tr>")
        emailbody.Append("<tr><td> Items To Purchase: </td><td> " & txtPurchase.Text.ToString().Trim() & "</td></tr>")
        emailbody.Append("<tr><td> Items For Sale: </td><td> " & txtSale.Text.ToString().Trim() & "</td></tr>")
        emailbody.Append("</table>")
        Return emailbody
    End Function
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Public Function Validate_Control() As String
        Try
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

            If txtMessage.Text.ToString().Trim.Length > 0 Then
                Obj.Message = txtMessage.Text.ToString().Trim
            Else
                Obj.Message = ""
            End If

            If txtSale.Text.ToString().Trim.Length > 0 Then
                Obj.ItemsToSale = txtSale.Text.ToString().Trim
            Else
                Obj.ItemsToSale = ""
            End If

            If txtPurchase.Text.ToString().Trim.Length > 0 Then
                Obj.ItemToPurchase = txtPurchase.Text.ToString().Trim
            Else
                Obj.ItemToPurchase = ""
            End If
          
            If chkNeedASAP.Checked Then
                Obj.IsNeedASAP = 1
            Else
                Obj.IsNeedASAP = 0
            End If

            If chkFuture.Checked Then
                Obj.IsNeedFuture = 1
            Else
                Obj.IsNeedFuture = 0
            End If

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

            Obj.IsCheck = 0
            Obj.IsOther = 0
            Obj.PONumber = ""
            Obj.CardType = ""
            Obj.Type = "Contact"
            Obj.OptionPrice = ""
            Obj.Price = 0
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
        txtZipCode.Text = ""
    End Sub
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        ClearControls()
    End Sub

End Class
