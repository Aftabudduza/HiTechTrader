Imports Microsoft.VisualBasic
Imports System.Drawing
Imports System.IO
Imports System.Collections.Generic

Public Class Actions
#Region "File Control"
    Public Shared Function FixPath(ByVal sPath As String) As String
        If sPath.Length > 0 Then
            If sPath.Substring(sPath.Length - 1, 1) <> "\" Then
                sPath &= "\"
            End If
        End If
        Return sPath
    End Function

    Public Shared Function FixURLPath(ByVal sPath As String) As String
        If sPath.Length > 0 Then
            If sPath.Substring(sPath.Length - 1, 1) <> "/" Then
                sPath &= "/"
            End If
        End If
        Return sPath
    End Function

    Public Shared Function Get_MultImg(ByVal sPath As String) As String
        'return the first image in the folder
        Dim d As New DirectoryInfo(sPath)
        Dim sFile As String = ""
        Try
            If d.Exists Then
                For Each f As FileInfo In d.GetFiles
                    sFile = f.Name
                    Exit For
                Next
            End If
        Catch ex As Exception
            sFile = "#"
        End Try
        Return sFile
    End Function

    Public Shared Function Get_FilenameFromURL(ByVal sURL As String) As String
        Dim i As Integer = sURL.LastIndexOf("/")
        Dim s As String = sURL.Substring(i + 1, sURL.Length - i - 1)
        Return s
    End Function

    Public Shared Function Clean_FileName(ByVal s As String) As String
        Dim sNewFile As String = ""
        For Each c As Char In s
            Dim iAscii As Integer = Asc(c)
            If (iAscii >= 48 And iAscii <= 57) Or (iAscii >= 65 And iAscii <= 90) Or (iAscii >= 97 And iAscii <= 122) Then
                sNewFile &= c
            Else
                sNewFile &= "_"
            End If
            'System.Diagnostics.Debug.WriteLine(c & ": " & Asc(c))
        Next
        Return sNewFile
    End Function

    Public Shared Function Save_File(ByVal aBytes() As Byte, ByVal sFullFileName As String, ByRef sError As String) As Boolean
        Try
            Dim objFile As New System.IO.FileInfo(sFullFileName)
            If objFile.Exists Then
                Try
                    objFile.Delete()
                Catch ex As Exception
                    Return -1
                End Try
            End If
            Dim fn As String = sFullFileName
            Actions.Clean_ByteArray(aBytes)
            'take the filedata and write it to the file
            FileOpen(1, fn, OpenMode.Binary, OpenAccess.Write)
            For Each ch As Byte In aBytes
                FilePut(1, Chr(ch))
            Next
            FileClose(1)
        Catch ex As Exception
            sError = "Error saving file: " & ex.Message
            Throw ex
            Return False
        End Try
        Return True
    End Function

    'Public Shared Sub Clean_ByteArray(ByRef xmlByte() As Byte)
    '    For z As Integer = 0 To xmlByte.Length - 1
    '        If IsNothing(xmlByte(z)) Then
    '            xmlByte(z) = AscW(" ")
    '        End If
    '    Next
    'End Sub

#End Region

#Region "Control Methods"
    Public Shared Function FindControlRecursive(ByVal root As Control, ByVal id As String) As Control
        If root.ID = id Then
            Return root
        End If
        Dim c As Control
        For Each c In root.Controls
            Dim t As Control = FindControlRecursive(c, id)
            If Not t Is Nothing Then
                Return t
            End If
        Next
        Return Nothing
    End Function

    Public Shared Function CreateBreakLiteral() As WebControls.Literal
        Dim lit As New Literal
        lit.Text = "<br>"
        Return lit
    End Function

#End Region

#Region "Image Methods"
    Public Shared Function create_thumbPathHeight(ByVal sOrigFilePath As String, ByVal sThumbPath As String, Optional ByVal iHeight As Integer = 50) As Boolean

        Dim FilePath As String = sOrigFilePath '[Global].CURRENTPATH & [Global].PRODIMGPATH & "\" & sCompany & "\" & sItem & HCActions.get_image_ext(sItem, sCompany) & ".jpg"
        '// We've selected 120 pixels as the arbitrary height 
        '// for the thumbnails. The code preserves the size ratio, 
        '// given this height. If you want larger thumbnails, 
        '// you can modify this value.
        Dim THUMBNAIL_HEIGHT As Integer = iHeight 'default to 50

        Dim bmp As Bitmap = Nothing
        Try
            'load bitmap from original file
            bmp = New Bitmap(FilePath)
            Dim decRatio As Decimal = (CType(bmp.Width / bmp.Height, Decimal))
            Dim intWidth As Integer = CType((decRatio * THUMBNAIL_HEIGHT), Integer)

            'generate the thumbnail
            Dim myCallBack As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)
            Dim img As System.Drawing.Image = bmp.GetThumbnailImage(intWidth, THUMBNAIL_HEIGHT, myCallBack, IntPtr.Zero)

            'save to thumbnail path (should be SERVER PATH)
            Dim thumb As Bitmap = CType(img, Bitmap)
            If Not IsNothing(thumb) Then
                'Dim ImgFormat As Drawing.Imaging.ImageFormat = GetImageFormat(FilePath)
                thumb.Save(sThumbPath, System.Drawing.Imaging.ImageFormat.Jpeg)
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

    Public Shared Function create_thumbPathWidth(ByVal sOrigFilePath As String, ByVal sThumbPath As String, Optional ByVal iWidth As Integer = 50) As Boolean

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
            bmp = New Bitmap(FilePath)
            Dim decRatio As Decimal = (CType(bmp.Height / bmp.Width, Decimal))
            Dim intHeight As Integer = CType((decRatio * THUMBNAIL_WIDTH), Integer)

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
                thumb.Save(sThumbPath, System.Drawing.Imaging.ImageFormat.Jpeg)
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

    Public Shared Function Convert_Image(ByVal sFilePath As String, ByVal sDonePath As String, ByVal ImgType As System.Drawing.Imaging.ImageFormat) As Boolean
        Dim FilePath As String = sFilePath '[Global].CURRENTPATH & [Global].PRODIMGPATH & "\" & sCompany & "\" & sItem & HCActions.get_image_ext(sItem, sCompany) & ".jpg"

        Dim bmp As Bitmap = Nothing
        Try
            'load bitmap from original file
            bmp = New Bitmap(FilePath)

            'generate the thumbnail
            Dim img As System.Drawing.Image = Image.FromFile(FilePath) ' bmp.GetThumbnailImage(THUMBNAIL_WIDTH, intHeight, myCallBack, IntPtr.Zero)

            'save to thumbnail path (should be SERVER PATH)
            Dim thumb As Bitmap = CType(img, Bitmap)
            If Not IsNothing(thumb) Then
                'Dim ImgFormat As Drawing.Imaging.ImageFormat = GetImageFormat(FilePath)
                thumb.Save(sDonePath, ImgType)
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
#End Region

#Region "String Methods"
    Public Shared Function Parse_Text_To_Array(ByVal sText As String, ByVal AllowDups As Boolean) As ArrayList
        Dim a As New ArrayList
        Dim iPlace As Integer
        Dim x As Integer = 1
        Do While sText.IndexOf(" ") <> -1 'And x < 10000
            iPlace = sText.IndexOf(" ")
            'System.Diagnostics.Debug.WriteLine("IND:" & iPlace & " LENGTH:" & sText.Length & "  :" & sText & ":")
            If AllowDups Then
                a.Add(sText.Substring(0, iPlace))
            Else
                'test for duplicates
                If Not Actions.inArray(a, sText.Substring(0, iPlace)) Then
                    a.Add(sText.Substring(0, iPlace))
                End If
            End If
            sText = sText.Substring(iPlace + 1, sText.Length - iPlace - 1)
            x += 1
        Loop
        a.Add(sText.Trim)
        'display_array(a)
        Return a
    End Function

    Public Shared Function inArray(ByVal a As ArrayList, ByVal s As String) As Boolean
        Dim bFound As Boolean = False
        For Each i As String In a
            If i.ToUpper = s.ToUpper Then
                bFound = True
                Exit For
            End If
        Next
        Return bFound
    End Function

    Public Shared Function lenFormat(ByVal s As String, ByVal l As Integer) As String
        If s.Length > l Then
            s = s.Substring(0, l)
        End If
        Return s.PadRight(l)
    End Function

    Public Shared Sub Clean_ByteArray(ByRef xmlByte() As Byte)
        Dim aByteSkips As ArrayList = Actions.BYTESKIPS
        Dim aBytes As New List(Of Byte)
        For z As Integer = 0 To xmlByte.Length - 1
            If IsNothing(xmlByte(z)) Then
                xmlByte(z) = AscW(" ")
            End If

            If inArray(aByteSkips, xmlByte(z)) Then
                'xmlByte(z) = AscW("")
            Else
                aBytes.Add(xmlByte(z))
            End If
        Next
        Dim newByte(aBytes.Count - 1) As Byte
        For x As Integer = 0 To aBytes.Count - 1
            newByte(x) = aBytes(x)
        Next
        xmlByte = newByte
    End Sub

    Public Shared Function Clean_ByteArraySpecialChars(ByRef xmlByte() As Byte) As String
        'Dim xmlByte() As Byte = Encoding.UTF8.GetBytes(sOrigVal)
        Dim aBytes As New List(Of Byte)
        For z As Integer = 0 To xmlByte.Length - 1
            If xmlByte(z) >= 129 And xmlByte(z) <> 194 Then
                Dim bNewByte As Byte
                bNewByte = 194
                aBytes.Add(bNewByte)
            End If
            aBytes.Add(xmlByte(z))
        Next
        Dim newByte(aBytes.Count - 1) As Byte
        For x As Integer = 0 To aBytes.Count - 1
            newByte(x) = aBytes(x)
        Next
        xmlByte = newByte

        Dim sNewVal As String = Encoding.UTF8.GetString(newByte)

        Return sNewVal
    End Function

    Public Shared Function BYTESKIPS() As ArrayList
        'this lists out the bytes that should be skipped
        Dim a As New ArrayList
        With a
            a.Add(194) '
        End With
        Return a
    End Function

    Public Shared Function Get_FileBytesToString(ByVal sPath As String) As System.Collections.Generic.List(Of String)
        Dim sr As StreamReader = Nothing
        Dim sFilestr As String = ""
        Dim aLines As New System.Collections.Generic.List(Of String)
        Try
            Dim aByte() As Byte = File.ReadAllBytes(sPath)
            sFilestr = Actions.Clean_ByteArraySpecialChars(aByte) 'Encoding.UTF8.GetString(aByte)
            Dim sLines() As String = sFilestr.Split(vbCrLf)
            For Each s As String In sLines
                'System.Diagnostics.Debug.WriteLine(s)
                If s.Trim <> "" Then
                    aLines.Add(s.Replace(chr(10), ""))
                End If
            Next
        Catch ex As Exception
            ErrorLog.LogDataError("Error reading file by bytes: " & sPath & " : " & ex.Message)
        Finally

        End Try
        Return aLines
    End Function

    Public Shared Function Clean_SQLString(ByVal s As String) As String
        Return s.Replace("'", "''")
    End Function

    Public Shared Function ValidEmailAddress(ByVal s As String) As Boolean
        Dim regex As Regex
        regex = New Regex("^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")
        Return regex.IsMatch(s)
    End Function

    Public Shared Function SetSQLDateTime(ByVal d As DateTime) As String
        '2008-03-27 17:06:10.000
        Return d.ToString("yyyy-MM-dd HH:mm:ss")
    End Function

    Public Shared Function FormatCSV(ByVal s As String) As String
        If s Is DBNull.Value Then
            s = ""
        End If
        If s.IndexOf(",") <> -1 Or s.IndexOf("""") <> -1 Then
            s = """" & s.Replace("""", """""") & """"
        End If
        s = s.Replace(Chr(13), "<br>").Replace(Chr(10), "")
        For x As Integer = 129 To 255
            s = s.Replace(Chr(x), "")
        Next
        s = s.Replace(Chr(0), "")
        'For Each c As Char In s
        '    System.Diagnostics.Debug.WriteLine(c & ":" & AscW(c))
        'Next
        Return s
    End Function

#End Region

#Region "CC Methods"
    Public Shared Function display_credit_card(ByVal CCNum As String) As String
        If CCNum.Length = 16 Then
            Return "#### #### #### " & CCNum.Substring(12, 4)
        ElseIf CCNum.Length = 15 Then
            Return "#### #### ### " & CCNum.Substring(11, 4)
        Else
            Return ""
        End If
    End Function

    Public Shared Function ConvertTransType(ByVal sTrans As String) As String
        Dim s As String = ""
        Select Case sTrans
            Case "AU"
                s = "Authorized"
            Case "DP", "PA"
                s = "Deposited"
            Case "DC"
                s = "Auth and Deposit"
            Case "RF"
                s = "Refunded"
        End Select
        Return s
    End Function
#End Region

#Region "ZIP FILE Methods"
    
#End Region

#Region "ListView Methods"
    Public Shared Function GetListViewTextBox(ByVal ctlName As String, ByVal sId As String, ByRef lv As ListView) As TextBox
        Dim txt As TextBox = Nothing
        For Each obj As ListViewDataItem In lv.Items
            For Each c As Control In obj.Controls
                'System.Diagnostics.Debug.WriteLine(c.ID)
                If c.ID = ctlName Then
                    Dim txtTest As TextBox = CType(c, TextBox)
                    If txtTest.ToolTip = sId Then
                        txt = txtTest
                        Exit For
                    End If
                End If
            Next
        Next
        Return txt
    End Function

    Public Shared Function GetListViewCheckBox(ByVal ctlName As String, ByVal sId As String, ByRef lv As ListView) As CheckBox
        Dim chk As CheckBox = Nothing
        For Each obj As ListViewDataItem In lv.Items
            For Each c As Control In obj.Controls
                'System.Diagnostics.Debug.WriteLine(c.ID)
                If c.ID = ctlName Then
                    Dim txtTest As CheckBox = CType(c, CheckBox)
                    If txtTest.ToolTip = sId Then
                        chk = txtTest
                        Exit For
                    End If
                End If
            Next
        Next
        Return chk
    End Function

    Public Shared Function GetListViewDropDown(ByVal ctlName As String, ByVal sId As String, ByRef lv As ListView) As DropDownList
        Dim ddl As DropDownList = Nothing
        For Each obj As ListViewDataItem In lv.Items
            For Each c As Control In obj.Controls
                'System.Diagnostics.Debug.WriteLine(c.ID)
                If c.ID = ctlName Then
                    Dim txtTest As DropDownList = CType(c, DropDownList)
                    If txtTest.ToolTip = sId Then
                        ddl = txtTest
                        Exit For
                    End If
                End If
            Next
        Next
        Return ddl
    End Function

    Public Shared Function GetListViewButton(ByVal ctlName As String, ByVal sId As String, ByRef lv As ListView) As Button
        Dim txt As Button = Nothing
        For Each obj As ListViewDataItem In lv.Items
            For Each c As Control In obj.Controls
                'System.Diagnostics.Debug.WriteLine(c.ID)
                If c.ID = ctlName Then
                    Dim txtTest As Button = CType(c, Button)
                    If txtTest.ToolTip = sId Then
                        txt = txtTest
                        Exit For
                    End If
                End If
            Next
        Next
        Return txt
    End Function
#End Region

#Region "Image Methods"
    Public Shared Function getExtension(ByVal sFileName As String) As String
        'include the .
        If sFileName.Trim = "" Then
            Return sFileName
        End If
        Dim i As Integer = sFileName.IndexOf(".")
        If i = -1 Then
            Return sFileName
        End If
        Dim sExt As String = sFileName.Substring(i, sFileName.Length - i)
        Return sExt
    End Function

#End Region
End Class
