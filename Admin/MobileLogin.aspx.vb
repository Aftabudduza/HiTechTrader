Imports System.Data
Partial Class Admin_MobileLogin
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Session("Id") = Nothing
        End If
    End Sub
    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Try
            ValidateUser()

        Catch ex As Exception
            DisplayAlert(ex.Message)
        End Try
    End Sub
    Private Sub ValidateUser()
        If Not [String].IsNullOrEmpty(txtEmail.Text.Trim()) AndAlso Not [String].IsNullOrEmpty(txtPassword.Text.Trim()) Then
            Dim objUserInfo As New UserInfo(appGlobal.CONNECTIONSTRING)
            Try
                Dim dt As DataTable = objUserInfo.GetRows("*", "Email ='" & txtEmail.Text.ToString().Trim() & "'  and Password = '" & appGlobal.base64Encode(txtPassword.Text.ToString().Trim) & "'")

                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    If Not dt.Rows(0)("Id").ToString() Is Nothing AndAlso CInt(dt.Rows(0)("Id").ToString()) > 0 Then
                        Session("Id") = CInt(dt.Rows(0)("Id").ToString())
                    End If
                    If Not dt.Rows(0)("IsSuperAdmin").ToString() Is Nothing AndAlso dt.Rows(0)("IsSuperAdmin").ToString() <> "" Then
                        If dt.Rows(0)("IsSuperAdmin") = True Then
                            Session("IsSuperAdmin") = dt.Rows(0)("IsSuperAdmin").ToString()
                        Else
                            Session("IsSuperAdmin") = "False"
                        End If
                    End If
                    If Not dt.Rows(0)("UserPermission").ToString() Is Nothing AndAlso dt.Rows(0)("UserPermission").ToString() <> String.Empty Then
                        If CInt(dt.Rows(0)("UserPermission").ToString()) > 0 Then
                            If dt.Rows(0)("UserPermission") = 1 Then
                                Session("IsSiteAdmin") = CInt(dt.Rows(0)("UserPermission").ToString())
                            ElseIf dt.Rows(0)("UserPermission") = 2 Then
                                Session("IsAdmin") = CInt(dt.Rows(0)("UserPermission").ToString())
                            ElseIf dt.Rows(0)("UserPermission") = 3 Then
                                Session("IsThirdParty") = CInt(dt.Rows(0)("UserPermission").ToString())
                            End If
                        End If
                    End If
                    If Not dt.Rows(0)("Email").ToString() Is Nothing AndAlso dt.Rows(0)("Email").ToString() <> "" Then
                        Session("Email") = dt.Rows(0)("Email").ToString()
                    End If

                    If Not dt.Rows(0)("UserName").ToString() Is Nothing AndAlso dt.Rows(0)("UserName").ToString() <> "" Then
                        Session("UserName") = dt.Rows(0)("UserName").ToString()
                    End If

                    Try
                        Dim sIns As String = "INSERT INTO UserLog ([UserId] ,[Username],[LoginTime]) value (" & CInt(dt.Rows(0)("Id").ToString()) & ",'" & dt.Rows(0)("UserName").ToString() & "','" & CDate(DateTime.UtcNow.ToString()) & "')"
                        SQLData.generic_command(sIns, SQLData.ConnectionString)
                    Catch ex As Exception

                    End Try


                    'HttpContext.Current.Cache.Add(objDataTable.Rows(0)("Email").ToString(), "", Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.NotRemovable, _
                    ' Nothing)
                    'Dim sLastPage As String = System.Web.Security.FormsAuthentication.GetRedirectUrl(HttpContext.Current.User.Identity.Name, False)
                    'System.Web.Security.FormsAuthentication.RedirectFromLoginPage(objDataTable.Rows(0)("Email").ToString(), True)
                    Response.Redirect("../Admin/MobileHome.aspx")

                Else
                    DisplayAlert("Wrong credentials!")
                End If
            Catch ex As Exception
                DisplayAlert(ex.Message)
            End Try
        Else
            DisplayAlert("User name and password can not be empty!")
        End If
    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
End Class
