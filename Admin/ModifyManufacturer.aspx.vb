Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_ModifyManufacturer
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Dim ManufacID As Integer = 0
    Dim ManuMisspellingId As Integer = 0
    Dim ManuAlternativeId As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("Id") Is Nothing Then
            ' nUserID = CInt(Session("Id").ToString())
            If Not Page.IsPostBack Then
                Session("ManuMissId") = Nothing
                Session("ManuParentId") = Nothing
                Session("ManuAltId") = Nothing
                Try
                    ManufacID = CInt(Request.QueryString("ManuParentId").ToString())
                Catch ex As Exception
                    ManufacID = 0
                End Try
                If ManufacID > 0 Then
                    Session("ManuParentId") = ManufacID
                End If

                Try
                    ManuMisspellingId = CInt(Request.QueryString("ManuMissId").ToString())
                Catch ex As Exception
                    ManuMisspellingId = 0
                End Try
                If ManuMisspellingId > 0 Then
                    Session("ManuMissId") = ManuMisspellingId
                End If

                Try
                    ManuAlternativeId = CInt(Request.QueryString("ManuAltId").ToString())
                Catch ex As Exception
                    ManuAlternativeId = 0
                End Try
                If ManuAlternativeId > 0 Then
                    Session("ManuAltId") = ManuAlternativeId
                End If

                If Not (Session("ManuAltId")) Is Nothing Then
                    fill_Controls_Manu(CInt(Session("ManuAltId").ToString().Trim))
                End If
                If Not (Session("ManuMissId")) Is Nothing Then
                    fill_Controls_Manu(CInt(Session("ManuMissId").ToString().Trim))
                End If
                If Not (Session("ManuParentId")) Is Nothing Then
                    fill_Controls(CInt(Session("ManuParentId").ToString().Trim))
                End If
            End If
        Else
            Response.Redirect("Login.aspx")
        End If
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
                                Session("ManuParentId") = CInt(ds.Tables(0).Rows(0)("Id").ToString())
                            End If
                            If Not ds.Tables(0).Rows(0)("Name") Is Nothing AndAlso ds.Tables(0).Rows(0)("Name") <> "" Then
                                txtManufactName.Text = ds.Tables(0).Rows(0)("Name").ToString()
                            End If
                            
                            If CInt(ds.Tables(0).Rows(0)("IsActive").ToString().Length) > 0 Then
                                chkActive.Checked = CInt(ds.Tables(0).Rows(0)("IsActive").ToString())
                            End If

                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub
    Private Sub fill_Controls_Manu(ByVal nManuID As Integer)
        If nManuID > 0 Then
            Try
                Dim sql As String = ""
                Dim dss As DataSet = Nothing
                If Not Session("ManuAltId") Is Nothing Then
                    If CInt(Session("ManuAltId").ToString()) > 0 Then
                        sql = "SELECT * FROM ManufacturerAlternative ma WHERE ma.Id = " & nManuID
                    End If

                End If
                If Not Session("ManuMissId") Is Nothing Then
                    If CInt(Session("ManuMissId").ToString()) > 0 Then
                        sql = "SELECT * FROM ManufacturerMisspellings mm WHERE mm.Id = " & nManuID
                    End If

                End If

                If sql.Length > 0 Then
                    dss = BRIClassLibrary.SQLData.generic_select(sql, appGlobal.CONNECTIONSTRING)
                    If Not dss Is Nothing Then
                        If dss.Tables(0).Rows.Count > 0 Then
                            If Not Session("ManuAltId") Is Nothing Then
                                If Not dss.Tables(0).Rows(0)("Id") Is Nothing AndAlso CInt(dss.Tables(0).Rows(0)("Id").ToString()) > 0 Then
                                    Session("ManuAltId") = CInt(dss.Tables(0).Rows(0)("Id").ToString())
                                End If
                                If Not dss.Tables(0).Rows(0)("Alternative") Is Nothing AndAlso dss.Tables(0).Rows(0)("Alternative") <> "" Then
                                    txtManufactName.Text = dss.Tables(0).Rows(0)("Alternative").ToString()
                                End If
                            End If

                            If Not Session("ManuMissId") Is Nothing Then
                                If Not dss.Tables(0).Rows(0)("Id") Is Nothing AndAlso CInt(dss.Tables(0).Rows(0)("Id").ToString()) > 0 Then
                                    Session("ManuMissId") = CInt(dss.Tables(0).Rows(0)("Id").ToString())
                                End If
                                If Not dss.Tables(0).Rows(0)("Misspellings").ToString() Is Nothing AndAlso dss.Tables(0).Rows(0)("Misspellings").ToString() <> "" Then
                                    txtManufactName.Text = dss.Tables(0).Rows(0)("Misspellings").ToString()
                                End If
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub
    Public Function Setdata(ByVal Obj As Manufacturer) As Manufacturer
        Try
            If CInt(Session("ManuParentId").ToString()) > 0 Then
                Obj.Id = CInt(Session("ManuParentId").ToString())
            End If
            If Not String.IsNullOrEmpty(txtManufactName.Text.ToString().Trim) AndAlso Not txtManufactName.Text.ToString().Trim = "" Then
                Obj.Name = txtManufactName.Text.ToString().Trim
            Else
                Obj.Name = ""
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
    Public Function Setdata_Misspellings(ByVal Obj_Miss As ManufacturerMisspellings) As ManufacturerMisspellings

        Try
            If CInt(Session("ManuMissId").ToString()) > 0 Then
                Obj_Miss.Id = CInt(Session("ManuMissId").ToString())
            End If
            If Not String.IsNullOrEmpty(txtManufactName.Text.ToString().Trim) AndAlso Not txtManufactName.Text.ToString().Trim = "" Then
                Obj_Miss.Misspellings = txtManufactName.Text.ToString().Trim
            Else
                Obj_Miss.Misspellings = ""
            End If

        Catch ex As Exception

        End Try

        Return Obj_Miss
    End Function
    Public Function Setdata_Alt(ByVal Obj_alt As ManufacturerAlternative) As ManufacturerAlternative
        Try

            If CInt(Session("ManuAltId").ToString()) > 0 Then
                Obj_alt.Id = CInt(Session("ManuAltId").ToString())
            End If
            If Not String.IsNullOrEmpty(txtManufactName.Text.ToString().Trim) AndAlso Not txtManufactName.Text.ToString().Trim = "" Then
                Obj_alt.Alternative = txtManufactName.Text.ToString().Trim
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
            Try
                If CInt(Session("ManuParentId").ToString()) > 0 Then
                    Dim Obj As New Manufacturer(appGlobal.CONNECTIONSTRING)
                    Obj = Setdata(Obj)
                    If Obj.UpdateManufacturerStatus() Then
                        Clear_Controls()
                        DisplayAlert("Your account is successfully Updated !!!")
                    Else
                        DisplayAlert("Your request not Updated!")
                    End If

                End If
            Catch ex As Exception

            End Try

            Try
                If CInt(Session("ManuMissId").ToString()) > 0 Then
                    Dim Obj_Miss As New ManufacturerMisspellings(appGlobal.CONNECTIONSTRING)
                    Obj_Miss = Setdata_Misspellings(Obj_Miss)
                    If Obj_Miss.UpdateManufacturerMissStatus() Then
                        Clear_Controls()
                        DisplayAlert("Your account is successfully Updated !!!")
                    Else
                        DisplayAlert("Your request not Updated!")
                    End If

                End If
            Catch ex As Exception

            End Try

            Try
                If CInt(Session("ManuAltId").ToString()) > 0 Then
                    Dim Obj_alt As New ManufacturerAlternative(appGlobal.CONNECTIONSTRING)
                    Obj_alt = Setdata_Alt(Obj_alt)
                    If Obj_alt.UpdateManufacturerAltStatus() Then
                        Clear_Controls()
                        DisplayAlert("Your account is successfully Updated !!!")

                    Else
                        DisplayAlert("Your request not Updated!")
                    End If
                End If
            Catch ex As Exception
            End Try
        Else
            DisplayAlert(errStr)
        End If
    End Sub
    Public Sub Clear_Controls()
        txtManufactName.Text = ""
        chkActive.Checked = False
    End Sub
End Class
