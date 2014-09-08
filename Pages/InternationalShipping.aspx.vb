Imports System.Data

Partial Class Pages_InternationalShipping
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CMSContent.InnerHtml = appGlobal.GetCMS_Message("InternationalShipping", "International Shipping CMS")
        SetSEO("InternationalShipping")
    End Sub
    Public Sub SetSEO(ByVal pageRef As String)
        Dim meta As New HtmlMeta()
        Dim meta2 As New HtmlMeta()
        Dim strSQL As String
        Dim objDS As DataSet
        Try
            strSQL = "select * from CMSPageRef where CMSPage = '" & pageRef & "' and WebsiteID = '" & appGlobal.WebsiteID & "' and live = 'Y'"
            objDS = BRIClassLibrary.SQLData.generic_select(strSQL, appGlobal.CONNECTIONSTRING)

            If objDS.Tables(0).Rows.Count > 0 Then
                Page.Header.Title = objDS.Tables(0).Rows(0).Item("MetaTitle")

                meta.Attributes.Add("name", "keywords")
                meta.Attributes.Add("content", objDS.Tables(0).Rows(0).Item("MetaKeywords"))

                Page.Header.Controls.Add(meta)

                meta2.Attributes.Add("name", "description")
                meta2.Attributes.Add("content", objDS.Tables(0).Rows(0).Item("MetaDescription"))

                Page.Header.Controls.Add(meta2)

            End If
        Catch ex As Exception

        End Try

        objDS = Nothing
    End Sub
End Class
