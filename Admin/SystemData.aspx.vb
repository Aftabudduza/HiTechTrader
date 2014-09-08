Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_SystemData
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
    Dim ProductId As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IsSuperAdmin") = True Then
            If Not Page.IsPostBack Then
                Session("TypeId") = Nothing
                fillGridView(CInt(ddlDatatype.SelectedValue.ToString()))
                If Not (Session("TypeId")) Is Nothing Then
                    fill_Controls(CInt(Session("TypeId")).ToString())
                    'spanEdit.InnerHtml = "<a href='ProductCopy.aspx?CopyImage=0&CopyID=" & CInt(Session("ProductId").ToString()) & "'>Copy Data to New Item</a>  :  <a href='ProductCopy.aspx?CopyImage=1&CopyID=" & CInt(Session("ProductId").ToString()) & "'>Copy Data & Image to New Item</a>"
                End If
            End If

        Else
        Response.Redirect("Login.aspx")
        End If

    End Sub
    Private Sub fillGridView(ByVal nType As Integer)
        Try
            Dim str As String = ""
            Dim ds As DataSet = Nothing
            gvData.DataSource = Nothing
            If ddlDatatype.SelectedValue <> "-1" Then
                str = "SELECT * FROM AdminSystemData asd WHERE asd.[Type]=" & CInt(nType)
                If str.Length > 0 Then
                    ds = SQLData.generic_select(str, strConnection)
                    If Not ds Is Nothing Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            gvData.DataSource = ds.Tables(0)
                            gvData.DataBind()
                        Else
                            gvData.DataSource = ds.Tables(0)
                            gvData.DataBind()
                        End If
                    Else
                        gvData.DataSource = ds.Tables(0)
                        gvData.DataBind()
                    End If
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Public Function Validate_Control() As String
        errStr = String.Empty
        If ddlDatatype.SelectedValue = "-1" Then
            errStr &= "Please Select Data Type." & System.Environment.NewLine
        End If
        Return errStr
    End Function
    Public Function Setdata(ByVal Obj As AdminSystemData) As AdminSystemData
        Try
            If Not Session("TypeId") Is Nothing Then
                Obj.Id = CInt(Session("TypeId").ToString().Trim)
            End If
            If Not String.IsNullOrEmpty(txtTypeName.Text.ToString()) AndAlso Not txtTypeName.Text.ToString() Is Nothing Then
                Obj.Name = txtTypeName.Text.ToString().Trim
            Else
                Obj.Name = ""
            End If
            If ddlDatatype.SelectedValue <> "-1" Then
                Obj.Type = CInt(ddlDatatype.SelectedValue.ToString())
            Else
                Obj.Type = "-1"
            End If


        Catch ex As Exception
        End Try
        Return Obj
    End Function
    Private Sub fill_Controls(ByVal nId As Integer)
        If nId > 0 Then
            Try
                Dim sSQl As String = "SELECT * FROM AdminSystemData asd WHERE asd.Id= " & nId
                Dim dt As DataTable = BRIClassLibrary.SQLData.generic_select(sSQl, strConnection).Tables(0)
                If dt.Rows.Count > 0 Then
                    If Not dt.Rows(0)("Name").ToString() Is Nothing Then
                        txtTypeName.Text = dt.Rows(0)("Name").ToString()
                    Else
                        txtTypeName.Text = ""
                    End If
                    If CInt(dt.Rows(0)("Type").ToString()) = "-1" Then
                        ddlDatatype.SelectedValue = "-1"
                    Else
                        ddlDatatype.SelectedValue = CInt(dt.Rows(0)("Type").ToString())
                    End If
                    
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub
    Private Sub Clear_Controls()
        ddlDatatype.SelectedValue = "-1"
        txtTypeName.Text = ""
    End Sub
    Protected Sub gvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvData.PageIndexChanging
        Try
            fillGridView(CInt(ddlDatatype.SelectedValue.ToString()))
            gvData.PageIndex = e.NewPageIndex
            gvData.DataBind()
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub btnsubmitt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsubmitt.Click
        Dim checkInsert As Integer = 0
        errStr = String.Empty
        errStr = Validate_Control()
        If errStr.Length <= 0 Then
            Dim Obj As New AdminSystemData(appGlobal.CONNECTIONSTRING)
            Obj = Setdata(Obj)
            Try
                If Not Session("TypeId") Is Nothing Then
                    Obj.Id = CInt(Session("TypeId").ToString())
                    If Obj.Update() Then
                        Session("TypeId") = Obj.Id
                        fillGridView(CInt(ddlDatatype.SelectedValue.ToString()))
                        Clear_Controls()
                        btnsubmitt.Text = "Add"
                        Session("Id") = Nothing
                        DisplayAlert("Your Data is updated!")
                    Else
                        DisplayAlert("Your request not submitted!")
                    End If
                Else
                    checkInsert = Obj.InsertIntoSystemData()
                    If checkInsert > 0 Then
                        Session("TypeId") = checkInsert
                        fillGridView(CInt(ddlDatatype.SelectedValue.ToString()))
                        Clear_Controls()
                        Session("TypeId") = Nothing
                        DisplayAlert("Your Data is successfully Inserted !!!")
                    Else
                        DisplayAlert("Your request not submitted!")
                    End If
                End If
            Catch ex As Exception

            End Try
        Else
            DisplayAlert(errStr)

        End If
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim obj As New AdminSystemData(appGlobal.CONNECTIONSTRING)
            Dim row As GridViewRow = TryCast(DirectCast(sender, LinkButton).Parent.Parent, GridViewRow)
            Dim hdId As HiddenField = TryCast(gvData.Rows(row.RowIndex).FindControl("hdID"), HiddenField)
            If (hdId.Value > 0) Then
                If (obj.Delete(hdId.Value)) Then
                    fillGridView(CInt(ddlDatatype.SelectedValue.ToString()))
                    DisplayAlert("Your Data has been successfully Deleted !")
                Else
                    DisplayAlert("Delete Failed !")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Edit(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim row As GridViewRow = TryCast(DirectCast(sender, LinkButton).Parent.Parent, GridViewRow)
            Dim hdId As HiddenField = TryCast(gvData.Rows(row.RowIndex).FindControl("hdID"), HiddenField)
            ' Dim hdType As HiddenField = TryCast(gvData.Rows(row.RowIndex).FindControl("hdType"), HiddenField)
            If (hdId.Value > 0) Then
                Session("TypeId") = hdId.Value
                ' Session("Type") = hdType.Value
                fill_Controls(hdId.Value)
                btnsubmitt.Text = "Update"

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ddlDatatype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDatatype.SelectedIndexChanged
        Try
            If ddlDatatype.SelectedValue <> "-1" Then
                fillGridView(CInt(ddlDatatype.SelectedValue.ToString()))
            End If
           
        Catch ex As Exception

        End Try
    End Sub

    
End Class
