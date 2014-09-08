Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_UserManagement
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Dim modifyUser As Integer = 0
    Dim DeleteUser As Integer = 0
    Dim sSQl As String = ""
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IsSuperAdmin") = True Then
            If Not Session("Id") Is Nothing Then
                nUserID = CInt(Session("Id").ToString())
                If Not Page.IsPostBack Then
                    Session("Modify") = Nothing
                    Session("delete") = Nothing
                    Try
                        modifyUser = CInt(Request.QueryString("modi").ToString())
                    Catch ex As Exception
                        modifyUser = 0
                    End Try
                    If modifyUser > 0 Then
                        Session("Modify") = modifyUser
                        FunctionTitle.InnerHtml = "Modify a User"
                        btnEditUser.Text = "Edit User"
                    End If
                    Try
                        DeleteUser = CInt(Request.QueryString("Del").ToString())
                    Catch ex As Exception
                        DeleteUser = 0
                    End Try
                    If DeleteUser > 0 Then
                        Session("delete") = DeleteUser
                        FunctionTitle.InnerHtml = "Delete a User"
                        btnEditUser.Text = "Delete User"
                    End If

                    ModifiedUserList()
                End If
            Else
                Response.Redirect("Login.aspx")
            End If
        Else
            Response.Redirect("Login.aspx")
        End If

    End Sub
    Public Sub ModifiedUserList()
        Try
            ddlUserList.Items.Clear()
            If Not Session("Modify") Is Nothing Then
                sSQl = "SELECT * FROM UserInfo ui WHERE ui.IsSuperAdmin IS NULL "
            End If
            If Not Session("delete") Is Nothing Then
                sSQl = "SELECT * FROM UserInfo ui WHERE ui.IsSuperAdmin IS NULL and ui.[Status] <> 3"
            End If

            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlUserList.AppendDataBoundItems = True
            ddlUserList.Items.Add(New ListItem("--------------  Select User ------------", "-1"))
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.ddlUserList.Items.Add(New ListItem(dr("UserName"), dr("Id")))
            Next
            ddlUserList.SelectedValue = "-1"
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub btnEditUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditUser.Click
        Try
            If ddlUserList.SelectedValue <> "-1" Then
                If Not Session("Modify") Is Nothing Then
                    Response.Redirect("Registration.aspx?UId=" & CInt(ddlUserList.SelectedValue.ToString()))
                End If
                If Not Session("delete") Is Nothing Then
                    deleteUserfromdb(CInt(ddlUserList.SelectedValue.ToString()))
                End If

            Else
                DisplayAlert("Please Select User Name !!")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Private Sub deleteUserfromdb(ByVal nUserID As Integer)
        If nUserID > 0 Then
            Try
                Dim Obj As New UserInfo(appGlobal.CONNECTIONSTRING)
                Obj.Id = CInt(nUserID.ToString())
                Obj.Status = "3"
                If Obj.UpdateUserStatus() Then
                    ddlUserList.SelectedValue = "-1"
                    DisplayAlert("User Deleted Successfully !!")
                Else
                    DisplayAlert("User Deleted Faild !!")
                End If

            Catch ex As Exception

            End Try
        End If

    End Sub
End Class
