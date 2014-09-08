Imports System.IO

Partial Class forms_AddCMSImage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""
        If Not IsPostBack Then
            lblMsg.Text = "Add Content Image"

        End If
    End Sub
    Private Sub Fill_Thumbs()
        Dim sDir As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageFilePath") & "\CMS"
        Dim sWebPath As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageURL") & "/CMS"
        Dim d As New DirectoryInfo(sDir)
        If Not File.Exists(sDir) Then
            Try
                d.Create()
            Catch ex As Exception
                lblMsg.Text = "Error generating CMS Directory. Check Permissions."
                Return
            End Try
        End If
        Dim sb As New System.Text.StringBuilder
        With sb
            For Each f As FileInfo In d.GetFiles("*.*")
                System.Diagnostics.Debug.WriteLine(f.Name)
                If f.Name.IndexOf(".db") = -1 Then
                    'list out the images
                    sb.Append("<img src=""" & sWebPath & "/" & f.Name & """ width=""40px"" alt=""Click to add the image."" onclick=""javascript:AddImageToText('" & sWebPath & "/" & f.Name & "');"" /><br>" & f.Name & "<br>")
                End If
            Next
        End With

    End Sub
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        'upload the image
        Dim sDir As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageFilePath") & "\CMS"
        Dim sWebPath As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageURL") & "/CMS"
        Dim d As New DirectoryInfo(sDir)
        If Not File.Exists(sDir) Then
            Try
                d.Create()
            Catch ex As Exception
                lblMsg.Text = "Error generating CMS Directory. Check Permissions."
                Return
            End Try
        End If
        Try
            'upload the file
            If Me.FileUpload1.FileName <> "" Then
                FileUpload1.SaveAs(Actions.FixPath(d.FullName) & Me.FileUpload1.FileName)
                Fill_Thumbs()
                lblMsg.Text = "File Uploaded Successfully."
            Else
                lblMsg.Text = "No File to Upload."
            End If
        Catch ex As Exception
            lblMsg.Text = "Error uploading image: " & ex.Message
        End Try
    End Sub
End Class
