Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_Registration
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Dim UserID As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IsSuperAdmin") = True Then
            If Not Session("Id") Is Nothing Then
                If Not Page.IsPostBack Then
                    Session("UID") = Nothing
                    Try
                        UserID = CInt(Request.QueryString("UId").ToString())
                    Catch ex As Exception
                        UserID = 0
                    End Try
                    If UserID > 0 Then
                        Session("UID") = UserID
                    End If
                    If Not (Session("UID")) Is Nothing Then
                        fill_Controls(CInt(Session("UID")))
                        PageHeader.InnerHtml = "Update User"
                    End If
                End If
            Else
                Response.Redirect("Login.aspx")
            End If
        Else
            Response.Redirect("Login.aspx")
        End If
        
    End Sub
    Public Function ValidEmail(ByVal value As String) As Boolean
        If (value Is Nothing) Then Return False
        Return reEmail.IsMatch(value)
    End Function
    Public Function Setdata(ByVal ObjUserInfo As UserInfo) As UserInfo
        Try
            If Not String.IsNullOrEmpty(txtUserName.Text.ToString().Trim) AndAlso Not txtUserName.Text.ToString().Trim = "" Then
                ObjUserInfo.UserName = txtUserName.Text.ToString().Trim
            Else
                ObjUserInfo.UserName = ""
            End If
            If Not String.IsNullOrEmpty(txtEmail.Text.ToString().Trim) AndAlso Not txtEmail.Text.ToString().Trim = "" Then
                ObjUserInfo.Email = txtEmail.Text.ToString().Trim
            Else
                ObjUserInfo.Email = ""
            End If
            If Not String.IsNullOrEmpty(txtPassword.Text.ToString().Trim) AndAlso Not txtPassword.Text.ToString().Trim = "" Then
                ObjUserInfo.Password = appGlobal.base64Encode(txtPassword.Text.ToString().Trim)
            End If
            If ddlUserLevel.SelectedValue <> "-1" Then
                ObjUserInfo.UserPermission = CInt(ddlUserLevel.SelectedValue.ToString().Trim)
            Else
                ObjUserInfo.UserPermission = "-1"
            End If
            If ddlStatus.SelectedValue <> "-1" Then
                ObjUserInfo.Status = CInt(ddlStatus.SelectedValue.ToString().Trim)
            Else
                ObjUserInfo.Status = "-1"
            End If

            'If rdoReleaseads.SelectedValue = "Yes" Then
            '    ObjUserInfo.ReleaseAds = "Yes"
            'Else
            '    ObjUserInfo.ReleaseAds = "No"
            'End If
            'If Not String.IsNullOrEmpty(txtItemNo.Text.ToString().Trim) AndAlso Not txtItemNo.Text.ToString().Trim = "" Then
            '    ObjUserInfo.ItemNo = CInt(txtItemNo.Text.ToString().Trim)
            'Else
            '    ObjUserInfo.ItemNo = 0
            'End If

            If Not Session("UID") Is Nothing Then
                ObjUserInfo.ModifiedBy = Session("Id").ToString().Trim
                ObjUserInfo.ModifiedDate = DateTime.UtcNow
            Else
                ObjUserInfo.CreatedBy = Session("Id").ToString().Trim
                ObjUserInfo.CreatedDate = DateTime.UtcNow
            End If
            ObjUserInfo.IsActive = True

        Catch ex As Exception

        End Try

        Return ObjUserInfo
    End Function
    Public Function Validate_Control() As String
        If Not String.IsNullOrEmpty(txtUserName.Text.ToString().Trim) AndAlso Len(txtUserName.ToString().Trim) <= 0 Then
            errStr &= "Please enter User Name" & Environment.NewLine
        End If
        If Len(txtEmail.Text.Trim()) <= 0 Then
            errStr &= "Please enter email address" & Environment.NewLine
        Else
            If Not ValidEmail(txtEmail.Text.Trim) Then
                errStr &= "Invalid email address" & Environment.NewLine
            End If
        End If
        If Len(txtPassword.Text.ToString().Trim) <= 0 Then
            errStr &= "Password field is required" & Environment.NewLine
        End If
        
        If IsUserNameDup(txtEmail.Text) Then
            errStr &= "Login ID already in system. Please enter a different Login ID" & Environment.NewLine
        End If
        Return errStr
    End Function
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Function MakeDBString(ByVal value As String) As String
        Return "'" & Replace(value, "'", "''") & "'"
    End Function
    Public Function ValidPassword(ByVal value As String) As Boolean
        If (value Is Nothing) Then Return False

        Return rePassword.IsMatch(value)
    End Function
    Function IsUserNameDup(ByVal uName As String) As Boolean
        Try
            Dim objCommand As Data.IDbCommand = Nothing
            Dim objDataReader As Data.IDataReader = Nothing
            Dim objTransaction As Data.IDbTransaction = Nothing
            Dim strSQL As String
            Dim dupCount As Integer

            m_objCN = New Data.SqlClient.SqlConnection(appGlobal.CONNECTIONSTRING)

            m_objCN.Open()
            objTransaction = m_objCN.BeginTransaction()

            strSQL = "Select count(*) as dupCount From UserInfo where UPPER(Email) = " & UCase(MakeDBString(uName))

            objCommand = New Data.SqlClient.SqlCommand(strSQL, CType(m_objCN, Data.SqlClient.SqlConnection), CType(objTransaction, Data.SqlClient.SqlTransaction))
            objDataReader = objCommand.ExecuteReader()

            While objDataReader.Read()
                dupCount = objDataReader("dupCount")
            End While

            If (objDataReader Is Nothing) = False Then
                objDataReader.Close()
                objDataReader.Dispose()

                objDataReader = Nothing
            End If

            If (objTransaction Is Nothing) = False Then
                objTransaction.Dispose()

                objTransaction = Nothing
            End If

            If (objCommand Is Nothing) = False Then
                objCommand.Dispose()

                objCommand = Nothing
            End If

            If (m_objCN Is Nothing) = False Then _
             m_objCN.Close()
            If Not Session("UID") Is Nothing Then
                If dupCount > 1 Then
                    Return True
                Else
                    Return False
                End If
            Else
                If dupCount > 0 Then
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception

        End Try
    End Function
    Protected Sub btnAddUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddUser.Click
        Dim checkInsert As Integer = 0
        errStr = String.Empty
        errStr = Validate_Control()
        If errStr.Length <= 0 Then
            Dim Obj As New UserInfo(appGlobal.CONNECTIONSTRING)
            Obj = Setdata(Obj)
            Try
                If Not Session("UID") Is Nothing Then
                    Obj.Id = CInt(Session("UID").ToString())
                    If Obj.Update() Then
                        'Clear_Controls()
                        'Session("UID") = Nothing
                        ' btnAddUser.Text = "Add User"
                        fill_Controls(CInt(Session("UID").ToString()))
                        PageHeader.InnerHtml = "Update User"
                        DisplayAlert("Your account is successfully Updated !!!")
                    Else
                        DisplayAlert("Your request not Updated!")
                    End If
                Else
                    checkInsert = Obj.InsertIntoUserInfo()
                    If checkInsert > 0 Then
                        Session("UID") = checkInsert
                        Session("UID") = Nothing
                        Clear_Controls()
                        DisplayAlert("Your account is successfully created !!!")
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
    Public Sub Clear_Controls()
        txtUserName.Text = ""
        txtEmail.Text = ""
        'txtItemNo.Text = ""
        ddlUserLevel.SelectedValue = "-1"
        ddlStatus.SelectedValue = "-1"
        ' rdoReleaseads.SelectedValue = ""
        txtPassword.Text = ""
    End Sub
    Private Sub fill_Controls(ByVal nUserID As Integer)
        If nUserID > 0 Then
            Try
                Dim sql As String = ""
                Dim ds As DataSet = Nothing
                sql = "SELECT * FROM UserInfo ui WHERE ui.Id = " & nUserID
                If sql.Length > 0 Then
                    ds = BRIClassLibrary.SQLData.generic_select(sql, appGlobal.CONNECTIONSTRING)
                    If Not ds Is Nothing Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If Not ds.Tables(0).Rows(0)("Id") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("Id").ToString()) > 0 Then
                                Session("UID") = CInt(ds.Tables(0).Rows(0)("Id").ToString())
                            End If
                            If Not ds.Tables(0).Rows(0)("UserName") Is Nothing AndAlso ds.Tables(0).Rows(0)("UserName") <> "" Then
                                txtUserName.Text = ds.Tables(0).Rows(0)("UserName").ToString()
                            End If
                            If Not ds.Tables(0).Rows(0)("Email") Is Nothing AndAlso ds.Tables(0).Rows(0)("Email") <> "" Then
                                txtEmail.Text = ds.Tables(0).Rows(0)("Email").ToString()
                            End If
                            If Not ds.Tables(0).Rows(0)("Password") Is Nothing AndAlso ds.Tables(0).Rows(0)("Password").ToString() <> "" Then
                                Dim strpass As String = ""
                                strpass = appGlobal.base64Decode(ds.Tables(0).Rows(0)("Password").ToString())
                                Reminder.Attributes.Add("value", strpass.ToString())
                            End If
                            'If Not ds.Tables(0).Rows(0)("ItemNo").ToString() Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("ItemNo").ToString()) > 0 Then
                            '    txtItemNo.Text = ds.Tables(0).Rows(0)("ItemNo").ToString()
                            'End If
                            If Not ds.Tables(0).Rows(0)("UserPermission") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("UserPermission").ToString()) > 0 Then
                                ddlUserLevel.SelectedValue = ds.Tables(0).Rows(0)("UserPermission").ToString()
                            End If
                            If Not ds.Tables(0).Rows(0)("Status") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("Status").ToString()) > 0 Then
                                ddlStatus.SelectedValue = ds.Tables(0).Rows(0)("Status").ToString()
                            End If
                            'Try
                            '    If Not ds.Tables(0).Rows(0)("ReleaseAds") Is Nothing AndAlso Not ds.Tables(0).Rows(0)("ReleaseAds") = "" Then
                            '        If ds.Tables(0).Rows(0)("ReleaseAds") = "Yes" Then
                            '            rdoReleaseads.SelectedValue = "Yes"
                            '        Else
                            '            rdoReleaseads.SelectedValue = "No"
                            '        End If

                            '    End If
                            'Catch ex As Exception

                            'End Try

                            Try
                                If Not ds.Tables(0).Rows(0)("CreatedDate").ToString() Is Nothing Then
                                    If CDate(ds.Tables(0).Rows(0)("CreatedDate")).ToString("MM/dd/yyyy") = "12/31/1977" Then
                                        lblCreatedDate.Text = DateTime.UtcNow.ToString()
                                    Else
                                        lblCreatedDate.Text = CDate(ds.Tables(0).Rows(0)("CreatedDate").ToString())
                                    End If
                                End If
                            Catch ex As Exception
                                lblCreatedDate.Text = DateTime.UtcNow.ToString()
                            End Try
                            Try
                                If Not ds.Tables(0).Rows(0)("ModifiedDate").ToString() Is Nothing Then
                                    If CDate(ds.Tables(0).Rows(0)("ModifiedDate")).ToString("MM/dd/yyyy") = "12/31/1977" Then
                                        lblLastEdited.Text = DateTime.UtcNow.ToString()
                                    Else
                                        lblLastEdited.Text = CDate(ds.Tables(0).Rows(0)("ModifiedDate").ToString())
                                    End If
                                End If
                               
                            Catch ex As Exception
                                lblLastEdited.Text = DateTime.UtcNow.ToString()
                            End Try
                            btnAddUser.Text = "Update User"
                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub
    'Public Sub UserLevel()
    '    Try

    '        Dim sSQl As String = "SELECT * FROM AdminSystemData asd WHERE asd.[Type]=1"
    '        Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
    '        ddlUserLevel.AppendDataBoundItems = True
    '        ddlUserLevel.Items.Add(New ListItem("--------  Select User Level -------", "-1"))
    '        For Each dr As DataRow In ds.Tables(0).Rows
    '            Me.ddlUserLevel.Items.Add(New ListItem(dr("Name"), dr("Id")))
    '        Next
    '        ddlUserLevel.SelectedValue = "-1"
    '    Catch ex As Exception

    '    End Try

    'End Sub
End Class
