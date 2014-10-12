Imports System.Data
Imports BRIClassLibrary
Partial Class User_Controls_Header
    Inherits System.Web.UI.UserControl
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        spanHeaderTopRight.InnerHtml = appGlobal.GetCMS_Message("HeaderTopRight", "Header Top Right CMS")
        If Not Page.IsPostBack Then
            LeftMenu()
        End If
    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            Dim strSearch As String = ""
            Dim sId As String = ""
            strSearch = txtSearch.Text.ToString().Trim
            If strSearch.Length > 0 Then
                Try
                    'add it
                    Dim sIns As String = "INSERT INTO SearchHistory ([SearchTerm],[CreatedDate]) VALUES('" & strSearch.Trim & "','" & CDate(DateTime.UtcNow) & "')"
                    SQLData.generic_command(sIns, SQLData.ConnectionString)
                Catch ex As Exception

                End Try

                Response.Redirect("../Pages/ProductListing.aspx?sc_state=0&sc_page=0&sc_cat=0&sc_search=" & strSearch.Trim)
            Else
                DisplayAlert("Please Enter Text to Search")
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub LeftMenu()
        Try
            Dim html As String = ""
            Dim dt As DataTable = Nothing
            Dim str As String = "SELECT * FROM CMSPageRef cr WHERE cr.IsLeftMenu=1 OR cr.IsLeftMenu= cr.IsFooter AND cr.Live='Y' ORDER BY cr.LeftMenuOrder ASC"
            If str.Length > 0 Then
                dt = BRIClassLibrary.SQLData.generic_select(str, appGlobal.CONNECTIONSTRING).Tables(0)
                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            If Not dr Is Nothing Then
                                html &= "<li class='footerlicss'><a  href='../Pages/CMSCommonPage.aspx?PN=" & dr("CMSPage") & " &PT=" & dr("CMSTitle") & "' >" & dr("CMSTitle") & "</a></li>"
                            End If
                        Next
                        HeaderLeftMenuCMS.InnerHtml = html.ToString()
                    End If
                End If
            End If


        Catch ex As Exception

        End Try
    End Sub
End Class
