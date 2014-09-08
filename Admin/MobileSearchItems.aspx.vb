Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_MobileSearchItems
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
    Dim ProductId As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IsSuperAdmin") = True Then
            If Not Session("Id") Is Nothing Then
                If Not Page.IsPostBack Then
                    Session("SearchText") = Nothing
                    txtSearchtext.Text = ""
                End If
            Else
                Response.Redirect("MobileLogin.aspx")
            End If
        Else
            Response.Redirect("MobileLogin.aspx")
        End If

    End Sub
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            If Not String.IsNullOrEmpty(txtSearchtext.Text.ToString()) And txtSearchtext.Text.ToString() <> "" Then
                Session("SearchText") = txtSearchtext.Text.ToString()
                If Not Session("SearchText").ToString() Is Nothing Then
                    If rdoSearchType.SelectedIndex <> -1 Then
                        Response.Redirect("MobileSearchResult.aspx?Search=" & txtSearchtext.Text.ToString() & "&SearchType=" & CInt(rdoSearchType.SelectedValue.ToString()))
                    End If

                Else
                    DisplayAlert("Please Enter Your Text")
                End If
            Else
                DisplayAlert("Please Enter Your Text")

            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
End Class
