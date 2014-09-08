
Partial Class Admin_Logout
    Inherits System.Web.UI.Page
    Public m_objCN As Data.IDbConnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("Id") Is Nothing Then
            Try
                Dim sIns As String = "INSERT INTO UserLog ([UserId] ,[Username],[LogoutTime]) value (" & CInt(Session("Id").ToString()) & ",'" & Session("UserName").ToString() & "','" & CDate(DateTime.UtcNow.ToString()) & "')"
                SQLData.generic_command(sIns, SQLData.ConnectionString)
            Catch ex As Exception

            End Try
        End If
       

        Session.Abandon()
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class
