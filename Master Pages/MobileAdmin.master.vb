Imports System.Data
Imports BRIClassLibrary
Partial Class Master_Pages_MobileAdmin
    Inherits System.Web.UI.MasterPage
    Private dt As DataTable = Nothing
    Private ds As DataSet = New DataSet()
    Private dt2 As DataTable = New DataTable()
    Public strConnection As String = appGlobal.CONNECTIONSTRING
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        currentdatetime.InnerHtml = DateTime.Now.ToString("MMMM dd" & ", " & " yyyy hh:mm:ss tt")
        curentyear.InnerHtml = DateTime.Now.ToString("yyyy")
        If Not Session("Id") Is Nothing Then
            logout.InnerHtml = "<a href='MobileLogout.aspx'>Log Out</a>"
            If Not Session("UserName") Is Nothing Then
                UserName.InnerHtml = Session("UserName").ToString()
            End If
        Else
            logout.InnerHtml = "<a href='MobileLogin.aspx'>Log In</a>"
        End If
        If Not IsPostBack Then
            'FillLeftMenuAdmin()
        End If
    End Sub
End Class

