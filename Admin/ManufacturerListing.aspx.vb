Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_ManufacturerListing
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
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IsSuperAdmin") = True Then
            If Not Session("Id") Is Nothing Then
                'nUserID = CInt(Session("Id").ToString())
                If Not Page.IsPostBack Then
                    ModifiedManufacturer()
                    DeletedManufacturer()
                    ManufacturerAlternative()
                    ManufacturerMisspellings()
                End If
            Else
                Response.Redirect("Login.aspx")
            End If
        Else
            Response.Redirect("Login.aspx")
        End If

    End Sub
    Public Sub ModifiedManufacturer()
        Try

            Dim sSQl As String = "SELECT * FROM Manufacturer mf WHERE mf.IsActive= 1"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlManufacturer.AppendDataBoundItems = True
            ddlManufacturer.Items.Add(New ListItem("----------------  Select Manufacturer Name ---------------------", "-1"))
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.ddlManufacturer.Items.Add(New ListItem(dr("Name"), dr("Id")))
            Next
            ddlManufacturer.SelectedValue = "-1"
        Catch ex As Exception

        End Try

    End Sub
    Public Sub ManufacturerMisspellings()
        Dim sSQl As String = "SELECT * FROM Manufacturer mf WHERE mf.IsActive= 1"
        Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
        ddlManufacturerMissname.AppendDataBoundItems = True
        ddlManufacturerMissname.Items.Add(New ListItem("------------------  Select Manufacturer Name -------------------", "-1"))
        For Each dr As DataRow In ds.Tables(0).Rows
            Me.ddlManufacturerMissname.Items.Add(New ListItem(dr("Name"), dr("Id")))
        Next
        ddlManufacturerMissname.SelectedValue = "-1"

    End Sub
    Public Sub ManufacturerAlternative()
        Try

            Dim sSQl As String = "SELECT * FROM Manufacturer mf WHERE mf.IsActive= 1"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlManufacturerAltname.AppendDataBoundItems = True
            ddlManufacturerAltname.Items.Add(New ListItem("------------------  Select Manufacturer Name -------------------", "-1"))
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.ddlManufacturerAltname.Items.Add(New ListItem(dr("Name"), dr("Id")))
            Next
            ddlManufacturerAltname.SelectedValue = "-1"
        Catch ex As Exception

        End Try

    End Sub
    Public Sub DeletedManufacturer()
        Try

            Dim sSQl As String = "SELECT * FROM Manufacturer mf WHERE mf.IsActive= 0"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlDeletedManufacturer.AppendDataBoundItems = True
            ddlDeletedManufacturer.Items.Add(New ListItem("--------------- Deleted Manufacturer Name ------------------", "-1"))
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.ddlDeletedManufacturer.Items.Add(New ListItem(dr("Name"), dr("Id")))
            Next
            ddlDeletedManufacturer.SelectedValue = "-1"
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub btnEditManufacturer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditManufacturer.Click
        Try
            If ddlManufacturer.SelectedValue <> "-1" Then
                Response.Redirect("ModifyManufacturer.aspx?ManuParentId=" & CInt(ddlManufacturer.SelectedValue.ToString()))
            ElseIf ddlDeletedManufacturer.SelectedValue <> "-1" Then
                Response.Redirect("ModifyManufacturer.aspx?ManuParentId=" & CInt(ddlDeletedManufacturer.SelectedValue.ToString()))
            ElseIf ddlmanuMiss.SelectedValue <> "-1" Then
                Response.Redirect("ModifyManufacturer.aspx?ManuMissId=" & CInt(ddlmanuMiss.SelectedValue.ToString()))
            ElseIf ddlManuAlt.SelectedValue <> "-1" Then
                Response.Redirect("ModifyManufacturer.aspx?ManuAltId=" & CInt(ddlManuAlt.SelectedValue.ToString()))
            Else
                DisplayAlert("Please Select Manufacturer Name !!")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnManuDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnManuDelete.Click
        Try
            If ddlManufacturer.SelectedValue <> "-1" Then
                Dim Obj As New Manufacturer(appGlobal.CONNECTIONSTRING)
                Obj.Id = CInt(ddlManufacturer.SelectedValue.ToString())
                Obj.IsActive = 0
                If Obj.UpdateManufacturerStatus() Then
                    ModifiedManufacturer()
                    DeletedManufacturer()
                    DisplayAlert("Manufacturer Deleted Sucessfully!!")
                End If
            Else
                DisplayAlert("Manufacturer Deleted Failed!!")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub

    Protected Sub ddlManufacturerMissname_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlManufacturerMissname.SelectedIndexChanged
        Try
            If ddlManufacturerMissname.SelectedValue <> "-1" Then
                Dim sSQl As String = "SELECT * FROM ManufacturerMisspellings mm WHERE mm.ManufacturerId=" & CInt(ddlManufacturerMissname.SelectedValue.ToString())
                Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
                ddlmanuMiss.AppendDataBoundItems = True
                ddlmanuMiss.Items.Add(New ListItem("----------  Select Manufacturer Misspelling Name ---------", "-1"))
                For Each dr As DataRow In ds.Tables(0).Rows
                    Me.ddlmanuMiss.Items.Add(New ListItem(dr("Misspellings"), dr("Id")))
                Next
                ddlmanuMiss.SelectedValue = "-1"
            End If
            
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ddlManufacturerAltname_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlManufacturerAltname.SelectedIndexChanged
        Try
            If ddlManufacturerAltname.SelectedValue <> "-1" Then
                Dim sSQl As String = "SELECT * FROM ManufacturerAlternative ma WHERE ma.ManufacturerId=" & CInt(ddlManufacturerAltname.SelectedValue.ToString())
                Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
                ddlManuAlt.AppendDataBoundItems = True
                ddlManuAlt.Items.Add(New ListItem("----------  Select Manufacturer Alternative Name ---------", "-1"))
                For Each dr As DataRow In ds.Tables(0).Rows
                    Me.ddlManuAlt.Items.Add(New ListItem(dr("Alternative"), dr("Id")))
                Next
                ddlManuAlt.SelectedValue = "-1"
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnalt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnalt.Click
        Try
            btnEditManufacturer_Click(sender, e)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnmiss_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmiss.Click
        Try
            btnEditManufacturer_Click(sender, e)
        Catch ex As Exception

        End Try
    End Sub
End Class
