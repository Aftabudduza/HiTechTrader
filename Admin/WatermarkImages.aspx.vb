Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_WatermarkImages
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
    Dim ItemId As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IsSuperAdmin") = True Then
            If Not Session("Id") Is Nothing Then
                If Not Page.IsPostBack Then
                    Session("ProductId") = Nothing
                    Try
                        ItemId = CInt(Request.QueryString("Itemno").ToString())
                    Catch ex As Exception
                        ItemId = 0
                    End Try
                    If ItemId > 0 Then
                        Session("ProductId") = ItemId
                    End If
                    If Not (Session("ProductId")) Is Nothing Then
                        fill_Controls(CInt(Session("ProductId").ToString()))
                    End If

                End If
            Else
                Response.Redirect("Login.aspx")
            End If
        Else
            Response.Redirect("Login.aspx")
        End If

    End Sub
    Public Sub fill_Controls(ByVal Itemno As Integer)
        If Not String.IsNullOrEmpty(Itemno.ToString()) AndAlso Not Itemno.ToString() Is Nothing Then
            Dim str As String = ""
            Dim ds As DataSet = Nothing
            Dim html As String = ""
            str = "SELECT p.ImageFileName FROM Product p WHERE p.Id =" & CInt(Itemno.ToString())
            If str.Length > 0 Then
                ds = BRIClassLibrary.SQLData.generic_select(str, appGlobal.CONNECTIONSTRING)
                If Not ds Is Nothing Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If Not ds.Tables(0).Rows(0)("ImageFileName") Is Nothing And ds.Tables(0).Rows(0)("ImageFileName").ToString() <> String.Empty Then
                            html &= "<img src='../ProductImages/Large/Images/" & ds.Tables(0).Rows(0)("ImageFileName").ToString() & "' alt='" & ds.Tables(0).Rows(0)("ImageFileName").ToString() & "' >"
                            imgdiv.InnerHtml = html
                        End If
                    Else
                        DisplayAlert("You Haven't Upload Any Water Mark Image yet!!!!!")
                    End If
                Else
                    DisplayAlert("You Haven't Upload Any Water Mark Image yet!!!!!")
                End If
            End If
        End If

    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
End Class
