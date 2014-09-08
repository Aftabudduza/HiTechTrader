Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_ProductDisplay
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
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)

    Public Sub FillProduct()
        Try

            Dim sSQl As String = "SELECT p.Id,p.ProductName FROM Product p"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlProduct.AppendDataBoundItems = True
            ddlProduct.Items.Add(New ListItem("----------------  Select Product ---------------------", "-1"))
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.ddlProduct.Items.Add(New ListItem(dr("ProductName"), dr("Id")))
            Next
            ddlProduct.SelectedValue = "-1"
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            FillProduct()
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            If ddlProduct.SelectedValue <> "-1" Then
                Response.Redirect("AddNewItem.aspx?PID=" & CInt(ddlProduct.SelectedValue.ToString()))
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
