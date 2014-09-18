Option Explicit On
Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Imports System.Drawing

Partial Class Admin_ProductImage
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
    Dim nProductId As Integer = 0
    Dim ItemNo As String = ""
    Dim ProductTitle As String = ""
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("Id") Is Nothing Then
            If Not Page.IsPostBack Then
                Session("ItemNo") = Nothing
                Session("Title") = Nothing
                Try
                    nProductId = CInt(Request.QueryString("ProductId").ToString())
                Catch ex As Exception
                    nProductId = 0
                End Try
                If nProductId > 0 Then
                    Session("ProductId") = nProductId
                    fillGridView()
                End If
                If nProductId > 0 Then
                    Try
                        Dim str As String = "SELECT ItemNumber, ProductName FROM Product p where  p.Id =  " & nProductId
                        Dim ds As DataSet = Nothing

                        If str.Length > 0 Then
                            ds = SQLData.generic_select(str, appGlobal.CONNECTIONSTRING)
                            If Not ds Is Nothing Then
                                If ds.Tables(0).Rows.Count > 0 Then
                                    If Not ds.Tables(0).Rows(0)(0).ToString() Is Nothing And ds.Tables(0).Rows(0)(0).ToString() <> String.Empty Then
                                        ProductId.InnerHtml = "Item No : " & " " & ds.Tables(0).Rows(0)(0).ToString()
                                    End If
                                    If Not ds.Tables(0).Rows(0)(1).ToString() Is Nothing And ds.Tables(0).Rows(0)(1).ToString() <> String.Empty Then
                                        Session("Title") = ds.Tables(0).Rows(0)(1).ToString()
                                        ProductName.InnerHtml = " Title :" & ds.Tables(0).Rows(0)(1).ToString()
                                    End If
                                End If
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                End If
            End If
        Else
            Response.Redirect("Login.aspx")
        End If
    End Sub
    Public Shared Function create_thumbPath(ByVal sOrigFilePath As String, ByVal sThumbPath As String, Optional ByVal iWidth As Integer = 124, Optional ByVal iHeight As Integer = 93) As Boolean

        Dim FilePath As String = sOrigFilePath '[Global].CURRENTPATH & [Global].PRODIMGPATH & "\" & sCompany & "\" & sItem & HCActions.get_image_ext(sItem, sCompany) & ".jpg"
        '// We've selected 120 pixels as the arbitrary height 
        '// for the thumbnails. The code preserves the size ratio, 
        '// given this height. If you want larger thumbnails, 
        '// you can modify this value.
        Dim THUMBNAIL_WIDTH As Integer = iWidth
        'Dim THUMBNAIL_HEIGHT As Integer = iHeight 'default to 50
        Dim bmp As Bitmap = Nothing
        Try
            'load bitmap from original file
            bmp = New Bitmap(sThumbPath)
            Dim intHeight As Integer = iHeight
            'clear old Thumbnail
            'Dim fThumb As New FileInfo(sThumbPath)
            'If fThumb.Exists Then
            '    fThumb.Delete()
            'End If
            'fThumb = Nothing
            'generate the thumbnail
            Dim myCallBack As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)
            Dim img As System.Drawing.Image = bmp.GetThumbnailImage(THUMBNAIL_WIDTH, intHeight, myCallBack, IntPtr.Zero)

            'save to thumbnail path (should be SERVER PATH)
            Dim thumb As Bitmap = CType(img, Bitmap)
            If Not IsNothing(thumb) Then
                'Dim ImgFormat As Drawing.Imaging.ImageFormat = GetImageFormat(FilePath)
                thumb.Save(FilePath, System.Drawing.Imaging.ImageFormat.Jpeg)
            End If

            Return True
            'Return CType(img, Bitmap)
        Catch ex As ArgumentException
            System.Diagnostics.Debug.WriteLine(ex.Message)
            Return False
        Finally
            If Not IsNothing(bmp) Then
                bmp.Dispose()
            End If
        End Try
        Return False
    End Function
    Private Shared Function ThumbnailCallback() As Boolean
        'necessary for thumbnail creation!
        Return False
    End Function
    Protected Sub btUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btUpload.Click
        Try
            Dim iUploadedCnt As Integer = 0
            Dim iFailedCnt As Integer = 0
            Dim filelength As Integer = 0
            Dim sfile() As File = Nothing
            Dim hfc As HttpFileCollection = Request.Files
            Dim obj As New ProductImageCrossRef(appGlobal.CONNECTIONSTRING)
            'If hfc.Count < 0 Then
            For i As Integer = 0 To hfc.Count - 1
                Dim hpf As HttpPostedFile = hfc(i)
                filelength = CInt(hpf.ContentLength.ToString())
                If filelength > 0 Then
                    filelength = filelength / (1024 * 1024)
                    If filelength <= 1 Then
                        Dim filePath As String = ""
                        Dim thumbPath As String = Path.Combine(Request.PhysicalApplicationPath, "ProductImages/Thumb/")
                        filePath = Path.Combine(Request.PhysicalApplicationPath, "ProductImages/Large/Images/")
                        If Not Directory.Exists(filePath) Then
                            Directory.CreateDirectory(filePath)
                        End If
                        Dim File As String = Path.Combine(filePath, hpf.FileName)
                        Dim ThubmFile As String = Path.Combine(filePath, hpf.FileName)
                        Dim fileExt As String = Path.GetExtension(hpf.FileName).ToLower()
                        If fileExt = ".jpg" Or fileExt = ".jpeg" Or fileExt = ".gif" Or fileExt = ".png" Or fileExt = ".bmp" Or fileExt = ".xlsx" Or fileExt = ".xls" Or fileExt = ".doc" Or fileExt = ".docx" Or fileExt = ".pdf" Then
                            Try
                                If System.IO.File.Exists(File) Then
                                    System.IO.File.Delete(File)
                                End If
                            Catch ex As Exception
                            End Try
                            Try
                                If System.IO.File.Exists(ThubmFile) Then
                                    System.IO.File.Delete(ThubmFile)
                                End If
                            Catch ex As Exception
                            End Try

                            hpf.SaveAs(File)
                            If fileExt = ".jpg" Or fileExt = ".jpeg" Or fileExt = ".gif" Or fileExt = ".png" Or fileExt = ".bmp" Then
                                Dim img As System.Drawing.Image
                                Try
                                    img = System.Drawing.Image.FromFile(File)
                                    If img.Height <> 115 Or img.Width <> 130 Then
                                        create_thumbPath(filePath & hpf.FileName, thumbPath & hpf.FileName & "t", 130, 115)
                                    End If
                                Catch ex As Exception
                                    System.Diagnostics.Debug.WriteLine(ex.Message)
                                End Try
                            End If
                           

                            Try
                                obj.ProductId = CInt(Session("ProductId").ToString())
                                obj.ImageUrl = hpf.FileName
                                If obj.DeleteByImageAndProductId(CInt(Session("ProductId").ToString()), hpf.FileName.ToString()) Then

                                End If
                            Catch ex As Exception

                            End Try

                            Try
                                obj = New ProductImageCrossRef(appGlobal.CONNECTIONSTRING)
                                obj.ProductId = CInt(Session("ProductId").ToString())
                                obj.ImageUrl = hpf.FileName
                                obj.InsertIntoProductCross()
                            Catch ex As Exception

                            End Try

                            iUploadedCnt = iUploadedCnt + 1
                        Else
                            DisplayAlert("Only file with extension "".jpg"", "".jpeg"", "".gif"", "".png"", "".bmp"","" .xlsx"","".xls"","".doc"","".docx"","".pdf"" are  allowed")
                        End If
                    Else
                        DisplayAlert("File Size can not be more than 1 MB")
                    End If
                End If

            Next i
            lblUploadStatus.Text = "<b>" & iUploadedCnt & "</b> file(s) Uploaded."

            fillGridView()

        Catch ex As Exception

        End Try
    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Private Sub fillGridView()
        Try
            Dim str As String = ""
            Dim ds As DataSet = Nothing
            gvImage.DataSource = Nothing
            If Not Session("ProductId") Is Nothing Then
                str = "SELECT * FROM ProductImageCrossRef p where  p.ProductId =  " & CInt(Session("ProductId").ToString())
            End If
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
    Function getString(ByVal ImageName As String) As String
        Dim imgurl As String = ""
        If Not String.IsNullOrEmpty(ImageName) Then
            imgurl = "http://192.82.249.221/ProductImages/Large/Images/<a target='_blank' href='http://192.82.249.221/ProductImages/Large/Images/" & ImageName & "'>" & ImageName & "</a>"
        End If
        Return (imgurl.ToString())
    End Function
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim deletedimage As String = ""
            Dim row As GridViewRow = TryCast(DirectCast(sender, LinkButton).Parent.Parent, GridViewRow)
            Dim hdId As HiddenField = TryCast(gvImage.Rows(row.RowIndex).FindControl("hdID"), HiddenField)
            Dim labelText = TryCast(gvImage.Rows(row.RowIndex).FindControl("lblImage"), Label).Text
            deletedimage = labelText.ToString()
            Dim obj As ProductImageCrossRef = New ProductImageCrossRef(appGlobal.CONNECTIONSTRING)
            If (hdId.Value > 0) Then
                Dim thumbPath As String = Path.Combine(Request.PhysicalApplicationPath, "ProductImages/Thumb/")
                Dim filePath As String = Path.Combine(Request.PhysicalApplicationPath, "ProductImages/Large/Images/")
                Dim File As String = ""
                File = filePath & deletedimage.ToString()
                Try
                    If System.IO.File.Exists(File) Then
                        System.IO.File.Delete(File)
                    End If
                Catch ex As Exception
                End Try

                Dim thumbFile As String = thumbPath & deletedimage.ToString() & "t"
                Try
                    Try
                        If System.IO.File.Exists(thumbFile) Then
                            System.IO.File.Delete(thumbFile)
                        End If
                    Catch ex As Exception
                    End Try
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
End Class
