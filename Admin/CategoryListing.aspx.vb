Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_CategoryListing
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IsSuperAdmin") = True Then
            If Not Session("Id") Is Nothing Then
                'nUserID = CInt(Session("Id").ToString())
                If Not Page.IsPostBack Then
                    Session("ModifyCat") = Nothing
                    Session("deleteCat") = Nothing
                    Try
                        modifyCategory = CInt(Request.QueryString("cat").ToString())
                    Catch ex As Exception
                        modifyCategory = 0
                    End Try
                    If modifyCategory > 0 Then
                        Session("ModifyCat") = modifyCategory
                        FunctionTitle.InnerHtml = "Modify a Category"
                        btnCatgory.Text = "Edit Category"
                    End If
                    Try
                        DeleteCategory = CInt(Request.QueryString("del").ToString())
                    Catch ex As Exception
                        DeleteCategory = 0
                    End Try
                    If DeleteCategory > 0 Then
                        Session("deleteCat") = DeleteCategory
                        FunctionTitle.InnerHtml = "Delete a Category"
                        btnCatgory.Text = "Delete Category"
                    End If

                    ModifiedUserList()
                End If
            Else
                Response.Redirect("Login.aspx")
            End If
        Else
            Response.Redirect("Login.aspx")
        End If

    End Sub
    Public Sub ModifiedUserList()
        Try
            ddlCategoryList.Items.Clear()
            If Not Session("ModifyCat") Is Nothing Then
                sSQl = "SELECT * FROM Category c"
            End If
            If Not Session("deleteCat") Is Nothing Then
                sSQl = "SELECT * FROM Category c WHERE c.IsActive= 1"
            End If
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlCategoryList.AppendDataBoundItems = True
            ddlCategoryList.Items.Add(New ListItem("----------  Select Category ---------", "-1"))
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.ddlCategoryList.Items.Add(New ListItem(dr("CategoryName"), dr("Id")))
            Next
            ddlCategoryList.SelectedValue = "-1"
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub btnCatgory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCatgory.Click
        Try
            If ddlCategoryList.SelectedValue <> "-1" Then
                If Not Session("ModifyCat") Is Nothing Then
                    Response.Redirect("Category.aspx?CatId=" & CInt(ddlCategoryList.SelectedValue.ToString()))
                End If
                If Not Session("deleteCat") Is Nothing Then
                    deleteUserfromdb(CInt(ddlCategoryList.SelectedValue.ToString()))
                End If

            Else
                DisplayAlert("Please Select User Name !!")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Private Sub deleteUserfromdb(ByVal nCatID As Integer)
        If nCatID > 0 Then
            Try
                Dim Obj As New Category(appGlobal.CONNECTIONSTRING)
                Obj.Id = CInt(nCatID.ToString())
                Obj.IsActive = 0
                If Obj.UpdateCategoryStatus() Then
                    ddlCategoryList.SelectedValue = "-1"
                    DisplayAlert("Category Deleted Successfully !!")
                Else
                    DisplayAlert("Category Deleted Faild !!")
                End If

            Catch ex As Exception

            End Try
        End If

    End Sub
End Class
