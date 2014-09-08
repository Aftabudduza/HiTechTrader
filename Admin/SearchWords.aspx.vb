Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Imports System.Data.SqlClient
Partial Class Admin_SearchWords
    Inherits System.Web.UI.Page
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Public str As String = String.Empty
    Public ds As DataSet = Nothing
    Public nRecent As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            nRecent = CInt(Request.QueryString("recent").ToString())
        Catch ex As Exception
            nRecent = 0
        End Try
        If nRecent = 1 Then
            lblTitle.InnerHtml = "<div class='DetailBox'><h1 class='pagetitle'>Most Recent Searches</h1><div class='ActionLinks'><a href='../Admin/SearchWords.aspx'>Most Popular Searches</a></div></div>"
            fillSearch(nRecent)
        Else
            fillSearch(0)
            lblTitle.InnerHtml = "<div class='DetailBox'><h1 class='pagetitle'>Most Popular Searches</h1><div class='ActionLinks'><a href='../Admin/SearchWords.aspx?recent=1'>Most Recent Searches</a></div></div>"
        End If
    End Sub

    Protected Sub grdSearch_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdSearch.PageIndexChanging
        Try
            nRecent = CInt(Request.QueryString("recent").ToString())
        Catch ex As Exception
            nRecent = 0
        End Try
        If nRecent = 1 Then
            fillSearch(nRecent)
        Else
            fillSearch(0)
        End If
        grdSearch.PageIndex = e.NewPageIndex
        grdSearch.DataBind()
    End Sub
    Public Sub fillSearch(Optional ByVal nRecent As Integer = 0)
        Try
            If nRecent = 1 Then
                str = " SELECT TOP 50 A.* FROM (SELECT DISTINCT SearchTerm, COUNT(Id) Total, MAX(CreatedDate) LastEdited FROM SearchHistory sh GROUP BY sh.SearchTerm) A ORDER BY A.LastEdited desc "
            Else
                str = " SELECT TOP 100 A.* FROM (SELECT DISTINCT SearchTerm, COUNT(Id) Total, MAX(CreatedDate) LastEdited FROM SearchHistory sh GROUP BY sh.SearchTerm) A ORDER BY A.LastEdited desc "
            End If

            ds = SQLData.generic_select(str, strConnection)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    grdSearch.DataSource = ds.Tables(0)
                    grdSearch.DataBind()
                End If
            End If
        Catch ex As Exception

        End Try
       
    End Sub
End Class
