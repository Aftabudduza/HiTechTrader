Imports System.IO

Partial Class forms_AddCMSFile
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""
        If Not IsPostBack Then
            lblMsg.Text = "Add Content File"
        End If
    End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        'upload the image
        Dim sDir As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageFilePath") & "CMS\Files"
        Dim sWebPath As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageURL") & "CMS/Files"

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
                Fill_Files()
                lblMsg.Text = "File Uploaded Successfully."
            Else
                lblMsg.Text = "No File to Upload."
            End If
        Catch ex As Exception
            lblMsg.Text = "Error uploading image: " & ex.Message
        End Try
    End Sub
    Private Sub Fill_Files()
        Dim sDir As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageFilePath") & "CMS\Files"
        Dim sWebPath As String = System.Configuration.ConfigurationManager.AppSettings.Get("ImageURL") & "CMS/Files"
        Dim d As New DirectoryInfo(sDir)
        If Not File.Exists(sDir) Then
            Try
                d.Create()
            Catch ex As Exception
                lblMsg.Text = "Error generating CMS File Directory. Check Permissions."
                Return
            End Try
        End If
        Dim sb As New System.Text.StringBuilder
        With sb
            For Each f As FileInfo In d.GetFiles("*.*")
                If f.Name.IndexOf(".db") = -1 Then
                    'list out the images
                    sb.Append("<a href=""#"" alt=""Click to add the file.""  onclick=""javascript:AddFileToText('" & sWebPath & "/" & f.Name & "');"" >" & f.Name & "</a><br><br>")
                End If
            Next
        End With

    End Sub
End Class
