Imports Microsoft.VisualBasic
Imports System.Data

Public Class EmailControl
    Public Shared Function Send_Email(ByVal strUserTo As String, ByVal strEmailTo As String, ByVal sSubject As String, ByVal sbBody As System.Text.StringBuilder, ByVal sServer As String, ByVal sEmailAccount As String, ByVal sEmailPass As String, ByVal iEmailPort As Integer, ByRef sError As String) As Boolean
        Dim objSMTPClient As Net.Mail.SmtpClient
        Dim objCustomerEmail As Net.Mail.MailMessage

        Try

            objCustomerEmail = New Net.Mail.MailMessage
            'Works also
            objSMTPClient = New Net.Mail.SmtpClient(sServer, iEmailPort) '25

            objSMTPClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
            'objSMTPClient.UseDefaultCredentials = True
            'bypass the authentication if there is no pass
            If sEmailPass.Trim <> "" Then
                objSMTPClient.Credentials = New Net.NetworkCredential(sEmailAccount, sEmailPass)
            End If
            objCustomerEmail.From = New Net.Mail.MailAddress(sEmailAccount)
            objCustomerEmail.ReplyTo = New Net.Mail.MailAddress(sEmailAccount)

            objCustomerEmail.To.Add(New Net.Mail.MailAddress(strEmailTo.ToString))
            'objCustomerEmail.Bcc.Add(New Net.Mail.MailAddress("betterwaythere@hotmail.com"))
            objCustomerEmail.Headers.Set("Return-Path", sEmailAccount)
            objCustomerEmail.IsBodyHtml = True
            objCustomerEmail.Subject = sSubject
            objCustomerEmail.Body = sbBody.ToString
            objSMTPClient.Send(objCustomerEmail)

            objSMTPClient = Nothing
            objCustomerEmail = Nothing

            Return True
        Catch ex As Exception
            sError = ex.Message
            ErrorLog.LogDataError("Error sending email: " & ex.Message)
            Return False
        End Try

    End Function

    Public Shared Function Send_Email_Attachments(ByVal strUserTo As String, ByVal strEmailTo As String, ByVal sSubject As String, ByVal sbBody As System.Text.StringBuilder, ByVal aFiles As ArrayList, ByVal sServer As String, ByVal sEmailAccount As String, ByVal sEmailPass As String, ByVal iEmailPort As Integer, ByRef sError As String) As Boolean
        Dim objSMTPClient As Net.Mail.SmtpClient
        Dim objCustomerEmail As Net.Mail.MailMessage

        Try

            objCustomerEmail = New Net.Mail.MailMessage
            'Works also
            objSMTPClient = New Net.Mail.SmtpClient(sServer, iEmailPort) '25
            objSMTPClient.Credentials = New Net.NetworkCredential(sEmailAccount, sEmailPass)
            objCustomerEmail.From = New Net.Mail.MailAddress(sEmailAccount)
            objCustomerEmail.ReplyTo = New Net.Mail.MailAddress(sEmailAccount)
            objCustomerEmail.To.Add(New Net.Mail.MailAddress(strEmailTo.ToString))
            'objCustomerEmail.Bcc.Add(New Net.Mail.MailAddress("betterwaythere@hotmail.com"))
            objCustomerEmail.Headers.Set("Return-Path", sEmailAccount)
            objCustomerEmail.IsBodyHtml = True
            objCustomerEmail.Subject = sSubject
            objCustomerEmail.Body = sbBody.ToString
            For Each sFilePath As String In aFiles
                objCustomerEmail.Attachments.Add(New System.Net.Mail.Attachment(sFilePath))
            Next

            objSMTPClient.Send(objCustomerEmail)

            objSMTPClient = Nothing
            objCustomerEmail = Nothing

            Return True
        Catch ex As Exception
            sError = ex.Message
            ErrorLog.LogDataError("Error sending email: " & ex.Message)
            Return False
        End Try

    End Function

    Public Shared Function GetBccs(ByVal iWebId As Integer) As ArrayList
        Dim a As New ArrayList

        Dim sSQL As String = "SELECT EMAILADDRESS FROM websiteBccs where websiteid=" & iWebId
        
        Dim ds As DataSet = SQLData.generic_select(sSQL, SQLData.ConnectionString)
        If ds.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In ds.Tables(0).Rows
                a.Add(dr("emailAddress"))
            Next
        End If

        Return a
    End Function

    Public Shared Function GetCompanyEmail(ByVal iWebId As Integer) As String
        Dim sSQL As String = "select top 1 contactEmail from company where id = (select companyid from website where id = " & iWebId & ")"
        Dim sEmail As String = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        Return sEmail
    End Function
End Class
