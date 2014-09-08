
Partial Class User_Controls_Footer
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        spanYear.InnerHtml = DateTime.UtcNow.Year.ToString()
        spanFooterLeft.InnerHtml = appGlobal.GetCMS_Message("FooterLeft", "Footer Left Logo")
        spanFooterRight.InnerHtml = appGlobal.GetCMS_Message("FooterRight", "Footer Right Logo")
    End Sub
End Class
