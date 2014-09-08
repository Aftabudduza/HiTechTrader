Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_NewsletterImages
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
            If Not Session("Id") Is Nothing Then
                If Not Page.IsPostBack Then
                    Session("ImgId") = Nothing
                    fillGridView()

                End If
            Else
                Response.Redirect("Login.aspx")
            End If
        Else
            Response.Redirect("Login.aspx")
        End If

    End Sub
    Private Sub fillGridView()
        Try
            Dim str As String = ""
            Dim ds As DataSet = Nothing
            gvImage.DataSource = Nothing
            str = "SELECT * FROM NewsletterImages ni"
            If str.Length > 0 Then
                ds = SQLData.generic_select(str, appGlobal.CONNECTIONSTRING)
                If Not ds Is Nothing Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        gvImage.DataSource = ds.Tables(0)
                        gvImage.DataBind()
                    Else
                        gvImage.DataSource = ds.Tables(0)
                        gvImage.DataBind()
                    End If
                Else
                    gvImage.DataSource = ds.Tables(0)
                    gvImage.DataBind()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub gvImage_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvImage.PageIndexChanging
        Try
            fillGridView()
            gvImage.PageIndex = e.NewPageIndex
            gvImage.DataBind()
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim deletedimage As String = ""
            Dim row As GridViewRow = TryCast(DirectCast(sender, LinkButton).Parent.Parent, GridViewRow)
            Dim hdId As HiddenField = TryCast(gvImage.Rows(row.RowIndex).FindControl("hdID"), HiddenField)
            Dim labelText = TryCast(gvImage.Rows(row.RowIndex).FindControl("lblImage"), Label).Text
            deletedimage = labelText.ToString()
            Dim obj As NewsletterImages = New NewsletterImages(appGlobal.CONNECTIONSTRING)
            If (hdId.Value > 0) Then
                Dim filePath As String = Path.Combine(Request.PhysicalApplicationPath, "ProductImages/Large/")
                Dim File As String = ""
                File = filePath & deletedimage.ToString()
                Try
                    If System.IO.File.Exists(File) Then
                        System.IO.File.Delete(File)
                    End If
                Catch ex As Exception
                End Try
                If (obj.Delete(hdId.Value)) Then
                    fillGridView()
                    DisplayAlert("Image successfully Deleted !")
                Else
                    DisplayAlert("Image Delete Failed !")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function Setdata(ByVal Obj As NewsletterImages) As NewsletterImages
        Try
            If flUpload.FileName <> "" Then
                Dim filePath As String = Path.Combine(Request.PhysicalApplicationPath, "ProductImages/Large/")
                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If
                Dim File As String = Path.Combine(filePath, flUpload.FileName)
                Dim fileExt As String = Path.GetExtension(flUpload.FileName).ToLower()
                If fileExt = ".jpg" Or fileExt = ".jpeg" Or fileExt = ".gif" Then
                    Try
                        If System.IO.File.Exists(File) Then
                            System.IO.File.Delete(File)
                        End If
                    Catch ex As Exception
                    End Try
                    flUpload.SaveAs(File)
                    Obj.Images = flUpload.FileName
                    Session("fileName") = flUpload.FileName
                    If rdoCriteria.SelectedIndex <> -1 Then
                        Obj.UpdateFormat = CInt(rdoCriteria.SelectedValue.ToString())
                    End If
                Else
                    DisplayAlert("Only file with extension "".jpg"", "".jpeg"", "".gif"" are  allowed")
                End If
            Else
                DisplayAlert("Please select a file")
            End If
           


        Catch ex As Exception

        End Try

        Return Obj
    End Function
    Public Function Validate_Control() As String
        If Not String.IsNullOrEmpty(flUpload.FileName.ToString().Trim) AndAlso Len(flUpload.FileName.ToString().Trim) <= 0 Then
            errStr &= "Please Select A File" & Environment.NewLine
        End If
        Return errStr
    End Function
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim checkInsert As Integer = 0
        errStr = String.Empty
        errStr = Validate_Control()
        If errStr.Length <= 0 Then
            Dim Obj As New NewsletterImages(appGlobal.CONNECTIONSTRING)
            Obj = Setdata(Obj)
            Try
                If Session("ImgId") Is Nothing Then
                    checkInsert = Obj.InsertIntoNewsletterImages()
                    If checkInsert > 0 Then
                        Session("ImgId") = checkInsert
                        fillGridView()
                        DisplayAlert("Image Upload successfully  !!!")
                    Else
                        DisplayAlert("Image upload Failed !!!")
                    End If
                End If
            Catch ex As Exception
            End Try
        Else
            DisplayAlert(errStr)
        End If
    End Sub
    Function getString(ByVal ImageName As String) As String
        Dim imgurl As String = ""
        If Not String.IsNullOrEmpty(ImageName) Then
            imgurl = "http://192.82.249.221/ProductImages/Large/<a target='_blank' href='http://localhost:49210/HitechTrader/ProductImages/Large/" & ImageName & "'>" & ImageName & "</a>"
        End If
        Return (imgurl.ToString())
    End Function
End Class
