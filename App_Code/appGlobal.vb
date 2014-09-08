Imports Microsoft.VisualBasic
Imports System.Data
Imports BRIClassLibrary
Imports System
Imports System.IO

Public Class appGlobal

    Public Shared ReadOnly Property WebsiteID() As Integer
        Get
            Return CStr(System.Configuration.ConfigurationManager.AppSettings("WebsiteID"))
        End Get
    End Property
    Public Shared Sub LogDataError(ByVal Information As String)
        ' Always on
        Try
            Dim sw As StreamWriter
            Dim curPath As String = System.Web.HttpContext.Current.Request.PhysicalApplicationPath
            sw = New StreamWriter(Actions.FixPath(ErrorLog.ErrorLogPath) & "SQLDataError.log", True)
            sw.WriteLine(Now.ToString & ": " & Information)
            sw.Flush()
            sw.Close()
            'later update function to write to __log table
            'Dim a As Boolean = (New Data).LogFileItCabinetEvent(sCabinetID, sConn, DateTime.Now(), "Source", "EventId", "Category", Information, sUserName)
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Function GetCMS_Message(ByVal sPageRef As String, ByVal sDefaultMessage As String) As String
        Dim s As String = "No Connection Found"
        Dim sSQL As String = "SELECT TOP 1 CMSCONTENT FROM CMSPAGEREF where LIVE='Y' and CMSPAGE='" & sPageRef & "'"
        Try
            s = BRIClassLibrary.SQLData.generic_scalar(sSQL, appGlobal.CONNECTIONSTRING)
        Catch ex As Exception
        End Try
        If s = "" Then
            'add if not Found!!!
            s = sDefaultMessage
            Dim sIns As String = "INSERT INTO CMSPAGEREF (CMSPAGE,WEBSITEID,CMSCONTENT, CMSTITLE, LIVE) VALUES ("
            sIns &= "'" & sPageRef & "','1','" & sDefaultMessage.Replace("'", "''") & "','" & sPageRef & "', 'Y')"
            BRIClassLibrary.SQLData.generic_command(sIns, appGlobal.CONNECTIONSTRING)

            'reset so it doesn't duplicate
            s = ""
            Return sDefaultMessage
        End If

        Return s
    End Function
    Public Shared ENC_PASS As String = "Jh3YdgQh4gBD0pQlM8b6xFH"
    Public Shared Function base64Encode(ByVal sData As String) As String
        Try
            Dim encData_byte As Byte() = New Byte(sData.Length - 1) {}
            encData_byte = System.Text.Encoding.UTF8.GetBytes(sData)
            Dim encodedData As String = Convert.ToBase64String(encData_byte)
            Return (encodedData)
        Catch ex As Exception
            Throw (New Exception("Error in base64Encode" & ex.Message))
        End Try
    End Function
    Public Shared Function base64Decode(ByVal sData As String) As String
        Dim encoder As New System.Text.UTF8Encoding()
        Dim utf8Decode As System.Text.Decoder = encoder.GetDecoder()
        Dim todecode_byte As Byte() = Convert.FromBase64String(sData)
        Dim charCount As Integer = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length)
        Dim decoded_char As Char() = New Char(charCount - 1) {}
        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0)
        Dim result As String = New [String](decoded_char)
        Return result
    End Function
    Public Shared ReadOnly Property CONNECTIONSTRING() As String
        Get
            Try
                Return System.Configuration.ConfigurationManager.ConnectionStrings("HitechConnectionString").ConnectionString

            Catch ex As Exception
                BRIClassLibrary.Logging.LogDataErrorPath("ERROR GETTING CONNECTION STRING: " & ex.Message, appGlobal.ERRORPATH)
            End Try
            Return ""
        End Get
    End Property

    Public Shared ReadOnly Property ERRORPATH() As String
        Get
            Return CStr(System.Configuration.ConfigurationManager.AppSettings("ErrorPath"))
        End Get
    End Property

    Public Shared Function EMAILACCOUNT() As String
        Dim emailSQL As String = "select emailaccount from website where id = " & WebsiteID()
        Return BRIClassLibrary.SQLData.generic_scalar(emailSQL, appGlobal.CONNECTIONSTRING)
    End Function

    Public Shared Function aEMAILBCCs() As ArrayList
        Dim aBCCs As New ArrayList
        Dim sMainEmailSQL As String = "select contactemail from company where id=(Select companyid from website where id=" & appGlobal.WebsiteID & ")"
        Dim sEmail As String = BRIClassLibrary.SQLData.generic_scalar(sMainEmailSQL, appGlobal.CONNECTIONSTRING)
        If sEmail.Trim <> "" Then
            aBCCs.Add(sEmail)
        End If

        Dim sBCCSQL As String = "select emailaddress from websitebccs where websiteid=" & appGlobal.WebsiteID
        Dim dsBCC As DataSet = BRIClassLibrary.SQLData.generic_select(sBCCSQL, appGlobal.CONNECTIONSTRING)
        If dsBCC.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In dsBCC.Tables(0).Rows
                aBCCs.Add(dr("EmailAddress"))
            Next
        End If
        If aBCCs.Count = 0 Then
            aBCCs.Add("sbutcher@binaryresearch.com")
        End If
        Return aBCCs
    End Function

    Public Shared ReadOnly Property CurrentSiteRoot() As String
        Get
            Return CStr(System.Configuration.ConfigurationManager.AppSettings("CurrentSiteRoot"))
        End Get
    End Property


    Public Shared Function GetCMS_MessageCategory(ByVal CatId As Integer) As String
        Dim s As String = "No Connection Found"
        Dim sSQL As String = "select Description from ProductCategory where id=" & CatId & " AND WEBSITEID=" & WebsiteID
        Try
            s = BRIClassLibrary.SQLData.generic_scalar(sSQL, appGlobal.CONNECTIONSTRING)
        Catch ex As Exception
        End Try


        Return s
    End Function
    Public Shared Function GetCMS_MessageManufacturer(ByVal Id As Integer) As String
        Dim s As String = "No Connection Found"
        Dim sSQL As String = "select Name from Manufacturer where id=" & Id & " AND WEBSITEID=" & WebsiteID
        Try
            s = BRIClassLibrary.SQLData.generic_scalar(sSQL, appGlobal.CONNECTIONSTRING)
        Catch ex As Exception
        End Try


        Return s
    End Function

#Region "ImageRotation"

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
#End Region

#Region "Image Control"
    Public Shared Function NotFoundFileName() As String
        Dim sSQL As String = "select imagerootpath + '/'+ notfoundfilename from website where id=" & appGlobal.WebsiteID
        'Return "http://116.68.193.227:90/productimages/Large/ </title><a href=http://improvement-loans.com title=payday+loan ><img src=http://improvement-loans.com/bg.gif border=0 ></a>"
        Return BRIClassLibrary.SQLData.generic_scalar(sSQL, appGlobal.CONNECTIONSTRING)
    End Function

    Public Shared Function ImageRootPath() As String
        Dim sSQL As String = "select imagerootpath  from website where id=" & appGlobal.WebsiteID
        Return BRIClassLibrary.StringMethods.fix_path_web(BRIClassLibrary.SQLData.generic_scalar(sSQL, appGlobal.CONNECTIONSTRING))
    End Function
    Public Shared Function ThumbRootPath() As String
        Dim sSQL As String = "select thumbrootpath  from website where id=" & appGlobal.WebsiteID
        Return BRIClassLibrary.StringMethods.fix_path_web(BRIClassLibrary.SQLData.generic_scalar(sSQL, appGlobal.CONNECTIONSTRING))
    End Function
    Public Shared Function ImageRootFilePath() As String
        Dim sSQL As String = "select imagerootfilepath from website where id=" & appGlobal.WebsiteID
        Return BRIClassLibrary.StringMethods.fix_path(BRIClassLibrary.SQLData.generic_scalar(sSQL, appGlobal.CONNECTIONSTRING))
    End Function
    Public Shared Function ThumbRootFilePath() As String
        Dim sSQL As String = "select thumbrootfilepath  from website where id=" & appGlobal.WebsiteID
        Return "C:\Works\ADI\Projects\API Project\API\productimages\Thumbs"
        'Return BRIClassLibrary.StringMethods.fix_path(BRIClassLibrary.SQLData.generic_scalar(sSQL, appGlobal.CONNECTIONSTRING))
    End Function

    Public Shared Function THUMBIMAGEWIDTH() As Integer
        Dim sSQL As String = "select thumbimagewidth  from website where id=" & appGlobal.WebsiteID
        Dim s As String = BRIClassLibrary.StringMethods.fix_path(BRIClassLibrary.SQLData.generic_scalar(sSQL, appGlobal.CONNECTIONSTRING))
        If Not IsNumeric(s) Then
            s = "100"
        End If
        Return CInt(s)
    End Function
#End Region
End Class


#Region "WebExt"

Public Module WebExt

    <System.Runtime.CompilerServices.Extension()> _
    Public Function IsValidEmailAddress(ByVal s As String) As Boolean
        Dim Regex2 = New Regex("^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") ' I have no idea how this string works inside the ()...
        REM "^(** start **)
        REM [\w-\.] (** word characters (letters, digits, and underscores) possibly containing (.)(not special character) after **)
        REM +@(** special character @ after **)
        REM ([\w-]+\.)(** word characters (letters, digits, and underscores) WITH (.)(not special character) after **)
        REM +[\w-]{2,4} (** 2, 3, or 4 ({2,4}) word characters ([\w-]) (letters, digits, and underscores) after previous **)
        REM $" (** END **)
        Return Regex2.IsMatch(s)
    End Function

    <System.Runtime.CompilerServices.Extension()> _
    Public Function NoHack(ByVal s As String) As String
        Return Replace(Replace(Replace(Replace(Replace(s, "'", "''"), "--", ""), ";", ":"), "/*", ""), "....", "...")
    End Function

End Module

#End Region