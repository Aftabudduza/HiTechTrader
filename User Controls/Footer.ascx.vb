Imports System.Data
Imports System.IO
Imports BRIClassLibrary
Imports System.Globalization
Imports System.Data.SqlClient
Partial Class User_Controls_Footer
    Inherits System.Web.UI.UserControl
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        spanYear.InnerHtml = DateTime.UtcNow.Year.ToString()
        spanFooterLeft.InnerHtml = appGlobal.GetCMS_Message("FooterLeft", "Footer Left Logo")
        spanFooterRight.InnerHtml = appGlobal.GetCMS_Message("FooterRight", "Footer Right Logo")
        If Not Page.IsPostBack Then
            FooterMenu()
        End If
    End Sub
    Public Sub FooterMenu()
        Try
            Dim html As String = ""
            Dim dt As DataTable = Nothing
            Dim str As String = "SELECT * FROM CMSPageRef cr WHERE cr.IsFooter=1 OR cr.IsLeftMenu= cr.IsFooter AND cr.Live='Y' ORDER BY cr.FooterMenuOrder ASC"
            If str.Length > 0 Then
                dt = BRIClassLibrary.SQLData.generic_select(str, appGlobal.CONNECTIONSTRING).Tables(0)
                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            If Not dr Is Nothing Then
                                html &= "<li class='footerlicss'><a  href='../Pages/CMSCommonPage.aspx?PN=" & dr("CMSPage") & " &PT=" & dr("CMSTitle") & "' >" & dr("CMSTitle") & "</a></li>"
                            End If
                        Next
                        FooterManueCMS.InnerHtml = html.ToString()
                    End If
                End If
            End If


        Catch ex As Exception

        End Try
    End Sub
End Class
