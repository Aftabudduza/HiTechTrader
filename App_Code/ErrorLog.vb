Imports Microsoft.VisualBasic
Imports System.io


Public Class ErrorLog
    Public Shared ReadOnly Property ErrorLogPath() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings("ErrorLogPath")
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
End Class
