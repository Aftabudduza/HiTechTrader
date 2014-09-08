Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_AddManufacturer
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Dim ManufacID As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("Id") Is Nothing Then
            ' nUserID = CInt(Session("Id").ToString())
            If Not Page.IsPostBack Then
                Session("ManuId") = Nothing
                ModifiedManufacturer()
                Try
                    ManufacID = CInt(Request.QueryString("ManuId").ToString())
                Catch ex As Exception
                    ManufacID = 0
                End Try
                If ManufacID > 0 Then
                    Session("ManuId") = ManufacID
                End If
                If Not (Session("ManuId")) Is Nothing Then
                    fill_Controls(CInt(Session("ManuId").ToString().Trim))
                End If
            End If
        Else
            Response.Redirect("Login.aspx")
        End If
    End Sub
    Public Function Setdata(ByVal Obj As Manufacturer) As Manufacturer
        Try
            If ddlManufacturer.SelectedValue <> "-1" Then
                If Not String.IsNullOrEmpty(txtManufactName.Text.ToString().Trim) AndAlso Not txtManufactName.Text.ToString().Trim = "" Then
                    Obj.Name = txtManufactName.Text.ToString().Trim
                Else
                    Obj.Name = ""
                End If
            Else
                Obj.Name = CInt(ddlManufacturer.SelectedValue.ToString())
            End If
           
            If Not String.IsNullOrEmpty(txtManuMisspelling.Text.ToString().Trim) AndAlso Not txtManuMisspelling.Text.ToString().Trim = "" Then
                Obj.Misspellings = txtManuMisspelling.Text.ToString().Trim
            Else
                Obj.Misspellings = ""
            End If
            If Not String.IsNullOrEmpty(txtManuAlterName.Text.ToString().Trim) AndAlso Not txtManuAlterName.Text.ToString().Trim = "" Then
                Obj.Alternative = txtManuAlterName.Text.ToString().Trim
            Else
                Obj.Alternative = ""
            End If
            If chkActive.Checked = True Then
                Obj.IsActive = 1
            Else
                Obj.IsActive = 0
            End If
        Catch ex As Exception

        End Try

        Return Obj
    End Function
    Public Sub ModifiedManufacturer()
        Try

            Dim sSQl As String = "SELECT * FROM Manufacturer mf WHERE mf.IsActive= 1"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlManufacturer.AppendDataBoundItems = True
            ddlManufacturer.Items.Add(New ListItem("------ Select Manufacturer Name ------", "-1"))
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.ddlManufacturer.Items.Add(New ListItem(dr("Name"), dr("Id")))
            Next
            ddlManufacturer.SelectedValue = "-1"
        Catch ex As Exception

        End Try

    End Sub
    Public Function Setdata_Misspellings(ByVal Obj_Miss As ManufacturerMisspellings) As ManufacturerMisspellings

        Try
            If CInt(Session("ManuId").ToString().Length) > 0 Then
                Obj_Miss.ManufacturerId = CInt(Session("ManuId").ToString())
            End If
            If Not String.IsNullOrEmpty(txtManuMisspelling.Text.ToString().Trim) AndAlso Not txtManuMisspelling.Text.ToString().Trim = "" Then
                Obj_Miss.Misspellings = txtManuMisspelling.Text.ToString().Trim
            Else
                Obj_Miss.Misspellings = ""
            End If
           
        Catch ex As Exception

        End Try

        Return Obj_Miss
    End Function
    Public Function Setdata_Alt(ByVal Obj_alt As ManufacturerAlternative) As ManufacturerAlternative
        Try

            If CInt(Session("ManuId").ToString().Length) > 0 Then
                Obj_alt.ManufacturerId = CInt(Session("ManuId").ToString())
            End If
            If Not String.IsNullOrEmpty(txtManuAlterName.Text.ToString().Trim) AndAlso Not txtManuAlterName.Text.ToString().Trim = "" Then
                Obj_alt.Alternative = txtManuAlterName.Text.ToString().Trim
            Else
                Obj_alt.Alternative = ""
            End If

        Catch ex As Exception

        End Try

        Return Obj_alt
    End Function
    Public Function Validate_Control() As String
        If Not String.IsNullOrEmpty(txtManufactName.Text.ToString().Trim) AndAlso Len(txtManufactName.ToString().Trim) <= 0 Then
            errStr &= "Please enter Manufacturer Name" & Environment.NewLine
        End If
        Return errStr
    End Function
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Protected Sub btnManufactur_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnManufactur.Click
        Dim checkInsert As Integer = 0
        errStr = String.Empty
        errStr = Validate_Control()
        If errStr.Length <= 0 Then
            Dim Obj As New Manufacturer(appGlobal.CONNECTIONSTRING)
            Obj = Setdata(Obj)
           
            Try
                If Not Session("ManuId") Is Nothing Then
                    Obj.Id = CInt(Session("ManuId").ToString())
                    If Obj.Update() Then
                        ModifiedManufacturer()
                        fill_Controls(CInt(Session("ManuId").ToString()))
                        FunctionTitle.InnerHtml = "Update Manufacturer"
                        DisplayAlert("Your account is successfully Updated !!!")
                    Else
                        DisplayAlert("Your request not Updated!")
                    End If
                Else
                    checkInsert = Obj.InsertIntoManufacturer()
                    If checkInsert > 0 Then
                        Session("ManuId") = checkInsert
                        ModifiedManufacturer()
                        If CInt(Session("ManuId").ToString().Length) > 0 Then
                            Dim Obj_Miss As New ManufacturerMisspellings(appGlobal.CONNECTIONSTRING)
                            Obj_Miss = Setdata_Misspellings(Obj_Miss)
                            Dim Obj_alt As New ManufacturerAlternative(appGlobal.CONNECTIONSTRING)
                            Obj_alt = Setdata_Alt(Obj_alt)
                            Obj_Miss.InsertIntoManufacturerMis()
                            Obj_alt.InsertIntoManufacturerAlt()

                            Clear_Controls()
                            DisplayAlert("Your account is successfully created !!!")
                        End If
                        Session("ManuId") = Nothing
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
        txtManuAlterName.Text = ""
        txtManufactName.Text = ""
        txtManuMisspelling.Text = ""
        ddlManufacturer.SelectedValue = "-1"
    End Sub
    Private Sub fill_Controls(ByVal nManuID As Integer)
        If nManuID > 0 Then
            Try
                Dim sql As String = ""
                Dim ds As DataSet = Nothing
                sql = "SELECT * FROM Manufacturer mf WHERE mf.Id = " & nManuID
                If sql.Length > 0 Then
                    ds = BRIClassLibrary.SQLData.generic_select(sql, appGlobal.CONNECTIONSTRING)
                    If Not ds Is Nothing Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If Not ds.Tables(0).Rows(0)("Id") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("Id").ToString()) > 0 Then
                                Session("ManuId") = CInt(ds.Tables(0).Rows(0)("Id").ToString())
                            End If
                            If Not ds.Tables(0).Rows(0)("Name") Is Nothing AndAlso ds.Tables(0).Rows(0)("Name") <> "" Then
                                txtManufactName.Text = ds.Tables(0).Rows(0)("Name").ToString()
                            End If
                           
                            If Not ds.Tables(0).Rows(0)("Misspellings").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("Misspellings").ToString() <> "" Then
                                txtManuMisspelling.Text = ds.Tables(0).Rows(0)("Misspellings").ToString()
                            End If

                            If Not ds.Tables(0).Rows(0)("Alternative").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("Alternative").ToString() <> "" Then
                                txtManuAlterName.Text = ds.Tables(0).Rows(0)("Alternative").ToString()
                            End If
                            If CInt(ds.Tables(0).Rows(0)("IsActive").ToString().Length) > 0 Then
                                chkActive.Checked = CInt(ds.Tables(0).Rows(0)("IsActive").ToString())
                            End If

                            
                        End If
                    End If
                End If
                btnManufactur.Text = "Update Category"
            Catch ex As Exception
            End Try
        End If
    End Sub

    
End Class
