Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_ReportUserLog
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Public ds As DataSet = New DataSet()
    Dim dt As New DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("Id") Is Nothing Then
            If Not Page.IsPostBack Then
                fill_User()
            End If
        Else
            Response.Redirect("Login.aspx")
        End If
    End Sub
    Private Sub fill_User()
        Try
            Dim sSQl As String = "SELECT c.Id, c.UserName FROM UserInfo c WHERE c.Id = " & CInt(Session("Id").ToString()) & " or c.CreatedBy = " & CInt(Session("Id").ToString()) & ""
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlUser.Items.Clear()
            ddlUser.AppendDataBoundItems = True
            For Each dr As DataRow In ds.Tables(0).Rows
                If Not dr("Id").ToString() Is Nothing And dr("Id").ToString() <> String.Empty Then
                    If CInt(dr("Id").ToString()) > 0 Then
                        Me.ddlUser.Items.Add(New ListItem(dr("UserName"), dr("Id")))
                    End If
                End If
            Next
        Catch ex As Exception

        End Try

    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try
            If CInt(ddlUser.SelectedValue) > 0 Then
                GenerateFile(CInt(ddlUser.SelectedValue))
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub GenerateFile(ByVal nUserId As Integer)
        Try
            Dim fileName As String = String.Empty
            Dim str As String = "SELECT *  FROM UserLog ul WHERE ul.UserId =  " & nUserId & " order by ul.Id asc "
            If str.Length > 0 Then
                dt = BRIClassLibrary.SQLData.generic_select(str, strConnection).Tables(0)
            End If

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim filePath = Path.Combine(Request.PhysicalApplicationPath, "Files\UserLog\")
                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If

                fileName = "User_Export_" & nUserId.ToString() & "_" & DateTime.Now.ToString("yyyyMMddhhmmss") & ".csv"
                filePath = Path.Combine(filePath, fileName)
                Try
                    If System.IO.File.Exists(filePath) Then
                        System.IO.File.Delete(filePath)
                    End If
                Catch ex As Exception

                End Try

                Dim wr = New StreamWriter(filePath)
                wr.Write("User")
                wr.Write(",")
                wr.Write("Login Time")
                wr.Write(",")
                wr.Write("Logout Time")
                wr.Write(wr.NewLine)
                For Each dr As DataRow In dt.Rows
                    If Not dr("Username") Is Nothing And dr("Username").ToString() <> String.Empty Then
                        wr.Write(dr("Username").ToString())
                    Else
                        wr.Write("")
                    End If
                    wr.Write(",")
                    If Not dr("LoginTime") Is Nothing And dr("LoginTime").ToString() <> String.Empty Then
                        If IsDate(dr("LoginTime").ToString()) Then
                            If CDate(dt.Rows(0)("LoginTime")).ToString("MM/dd/yyyy") = "12/31/1977" Then
                                wr.Write("")
                            Else
                                wr.Write(dr("LoginTime").ToString())
                            End If
                        Else
                            wr.Write("")
                        End If
                    Else
                        wr.Write("")
                    End If
                    wr.Write(",")
                    If Not dr("LogoutTime") Is Nothing And dr("LogoutTime").ToString() <> String.Empty Then
                        If IsDate(dr("LogoutTime").ToString()) Then
                            If CDate(dt.Rows(0)("LogoutTime")).ToString("MM/dd/yyyy") = "12/31/1977" Then
                                wr.Write("")
                            Else
                                wr.Write(dr("LogoutTime").ToString())
                            End If
                        Else
                            wr.Write("")
                        End If
                    Else
                        wr.Write("")
                    End If
                    wr.Write(wr.NewLine)
                Next
                wr.Close()
                Response.Redirect("../Files/UserLog/" & fileName)
            Else
                DisplayAlert("You Have Nothing to Export")
            End If
        Catch ex As Exception
            DisplayAlert("Opetation Not Proceed")
        End Try
    End Sub
End Class
