
Partial Class Admin_MobileLogout
    Inherits System.Web.UI.Page
    Public m_objCN As Data.IDbConnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Abandon()
        Session.Clear()
        Response.Redirect("MobileLogin.aspx")
    End Sub
End Class
