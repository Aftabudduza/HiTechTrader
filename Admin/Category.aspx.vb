Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_Category
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Dim CategoryID As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("Id") Is Nothing Then
            ' nUserID = CInt(Session("Id").ToString())
            If Not Page.IsPostBack Then
                Session("CatId") = Nothing
                fill_ParentCategory()
                fill_LabXCategory()
                Try
                    CategoryID = CInt(Request.QueryString("CatId").ToString())
                Catch ex As Exception
                    CategoryID = 0
                End Try
                If CategoryID > 0 Then
                    Session("CatId") = CategoryID
                End If
                If Not (Session("CatId")) Is Nothing Then
                    fill_Controls(CInt(Session("CatId").ToString().Trim))
                End If
            End If
        Else
            Response.Redirect("Login.aspx")
        End If
    End Sub
    Private Sub fill_ParentCategory()
        Try
            Dim sSQl As String = "SELECT * FROM Category c WHERE c.CategoryParentId =0 AND c.IsMainorLabX = 0"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddlParentCategory.Items.Clear()
            ddlParentCategory.AppendDataBoundItems = True
            ddlParentCategory.Items.Add(New ListItem("-------------  Select Parent Category  -------------", "-1"))
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.ddlParentCategory.Items.Add(New ListItem(dr("CategoryName"), dr("Id")))
            Next
            ddlParentCategory.SelectedValue = "-1"
        Catch ex As Exception

        End Try

    End Sub
    Private Sub fill_LabXCategory()
        Try
            Dim sSQl As String = "SELECT * FROM Category c WHERE c.IsLabX = 1 ORDER BY c.Id"
            Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
            ddllabxcat.Items.Clear()
            ddllabxcat.AppendDataBoundItems = True
            ddllabxcat.Items.Add(New ListItem("-------------  Select LabX Category  -------------", "-1"))
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If CInt(dr("Id").ToString().Length) > 0 Then
                        If CInt(dr("LabXParentId").ToString()) = 0 Then
                            Session("ParentId") = CInt(dr("Id").ToString())
                            Dim str As String = "SELECT * FROM Category c WHERE c.LabXParentId =" & CInt(dr("Id").ToString())
                            Dim ds2 As DataSet = BRIClassLibrary.SQLData.generic_select(str, appGlobal.CONNECTIONSTRING)
                            Me.ddllabxcat.Items.Add(New ListItem(dr("CategoryName"), dr("Id")))
                            If Not ds2 Is Nothing AndAlso ds2.Tables(0).Rows.Count > 0 Then
                                For Each dr2 As DataRow In ds2.Tables(0).Rows
                                    Me.ddllabxcat.Items.Add(New ListItem("-" & dr2("CategoryName"), CInt(Session("ParentId").ToString())))
                                Next
                            End If
                            Session("ParentId") = Nothing

                        End If

                    End If
                Next
            End If
            
            ddllabxcat.SelectedValue = "-1"
        Catch ex As Exception

        End Try

    End Sub
    Public Function Setdata(ByVal Obj As Category) As Category
        Try
            If Not String.IsNullOrEmpty(txtCategoryName.Text.ToString().Trim) AndAlso Not txtCategoryName.Text.ToString().Trim = "" Then
                Obj.CategoryName = txtCategoryName.Text.ToString().Trim
            Else
                Obj.CategoryName = ""
            End If
            If chkActive.Checked = True Then
                Obj.IsActive = 1
            Else
                Obj.IsActive = 0
            End If
            If chkProductCat.Checked = True Then
                Obj.IsProductCategory = 1
            Else
                Obj.IsProductCategory = 0
            End If
            If rdoLevel.SelectedIndex <> -1 Then
                Obj.Catlevel = CInt(rdoLevel.SelectedValue.ToString().Trim)
            End If
            If Not String.IsNullOrEmpty(txtMetadescription.Text.ToString()) AndAlso txtMetadescription.Text.ToString() <> "" Then
                Obj.MetaDescription = txtMetadescription.Text.ToString()
            End If
            If Not String.IsNullOrEmpty(txtmetaKeywords.Text.ToString()) AndAlso txtmetaKeywords.Text.ToString() <> "" Then
                Obj.MetaKeywords = txtmetaKeywords.Text.ToString()
            End If
            If rdoIslabX.SelectedIndex <> -1 Then
                Obj.IsLabX = CInt(rdoIslabX.SelectedValue.ToString().Trim)
            End If

            If rdoLevel.SelectedValue = "1" Then
                If rdoIslabX.SelectedValue = "1" Then
                    Obj.IsMainorLabX = 1
                Else
                    Obj.IsMainorLabX = 0
                End If
            ElseIf rdoLevel.SelectedValue = "2" Then
                If rdoIslabX.SelectedValue = "1" Then
                    Obj.IsMainorLabX = 1
                    Obj.LabXParentId = CInt(ddllabxcat.SelectedValue.ToString())
                Else
                    Obj.IsMainorLabX = 0
                    Obj.CategoryParentId = CInt(ddlParentCategory.SelectedValue.ToString())
                End If
            End If
           
        Catch ex As Exception

        End Try

        Return Obj
    End Function
    Public Function Validate_Control() As String
        If Not String.IsNullOrEmpty(txtCategoryName.Text.ToString().Trim) AndAlso Len(txtCategoryName.ToString().Trim) <= 0 Then
            errStr &= "Please enter Category Name" & Environment.NewLine
        End If
        If rdoLevel.SelectedIndex = -1 Then
            errStr &= "Please Select a Lavel" & Environment.NewLine
        End If
        If txtMetadescription.Text.Length > 150 Then
            errStr &= "Please enter a maximum of 150 characters" & Environment.NewLine
        End If
        If txtmetaKeywords.Text.Length > 250 Then
            errStr &= "Please enter a maximum of 250 characters" & Environment.NewLine
        End If
        If rdoLevel.SelectedValue = "2" Then
            If rdoIslabX.SelectedValue = "1" Then
                If ddllabxcat.SelectedValue = "-1" Then
                    errStr &= "Please Select a LabX Parent Category" & Environment.NewLine
                End If
            ElseIf ddlParentCategory.SelectedValue = "-1" Then
                errStr &= "Please Select a Parent Category" & Environment.NewLine
            End If
        End If
        Return errStr
    End Function
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Protected Sub btnAddCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCategory.Click
        Dim checkInsert As Integer = 0
        errStr = String.Empty
        errStr = Validate_Control()
        If errStr.Length <= 0 Then
            Dim Obj As New Category(appGlobal.CONNECTIONSTRING)
            Obj = Setdata(Obj)
            Try
                If Not Session("CatId") Is Nothing Then
                    Obj.Id = CInt(Session("CatId").ToString())
                    If Obj.Update() Then
                        'Clear_Controls()
                        'Session("UID") = Nothing
                        ' btnAddUser.Text = "Add User"
                        fill_Controls(CInt(Session("CatId").ToString()))
                        fill_ParentCategory()
                        fill_LabXCategory()
                        PageHeader.InnerHtml = "Update User"
                        DisplayAlert("Your account is successfully Updated !!!")
                    Else
                        DisplayAlert("Your request not Updated!")
                    End If
                Else
                    checkInsert = Obj.InsertIntoCategory()
                    If checkInsert > 0 Then
                        Session("CatId") = checkInsert
                        Session("CatId") = Nothing
                        fill_ParentCategory()
                        fill_LabXCategory()
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
        txtCategoryName.Text = ""
        ddlParentCategory.SelectedValue = "-1"
        rdoLevel.SelectedIndex = -1
        rdoIslabX.SelectedIndex = -1
        txtMetadescription.Text = ""
        txtmetaKeywords.Text = ""
    End Sub
    Private Sub fill_Controls(ByVal nCatID As Integer)
        If nCatID > 0 Then
            Try
                Dim sql As String = ""
                Dim ds As DataSet = Nothing
                sql = "SELECT * FROM Category c WHERE c.Id = " & nCatID
                If sql.Length > 0 Then
                    ds = BRIClassLibrary.SQLData.generic_select(sql, appGlobal.CONNECTIONSTRING)
                    If Not ds Is Nothing Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If Not ds.Tables(0).Rows(0)("Id") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("Id").ToString()) > 0 Then
                                Session("CatId") = CInt(ds.Tables(0).Rows(0)("Id").ToString())
                            End If
                            If Not ds.Tables(0).Rows(0)("CategoryName") Is Nothing AndAlso ds.Tables(0).Rows(0)("CategoryName") <> "" Then
                                txtCategoryName.Text = ds.Tables(0).Rows(0)("CategoryName").ToString()
                            End If
                            If CInt(ds.Tables(0).Rows(0)("IsActive")) > 0 Then
                                chkActive.Checked = True
                            Else
                                chkActive.Checked = False
                            End If
                            If CInt(ds.Tables(0).Rows(0)("IsProductCategory")) > 0 Then
                                chkProductCat.Checked = True
                            Else
                                chkProductCat.Checked = False
                            End If

                            If CInt(ds.Tables(0).Rows(0)("Catlevel").ToString().Length) > 0 Then
                                If CInt(ds.Tables(0).Rows(0)("Catlevel")) = 1 Then
                                    rdoLevel.SelectedValue = "1"
                                Else
                                    rdoLevel.SelectedValue = "2"
                                    If CInt(ds.Tables(0).Rows(0)("CategoryParentId").ToString()) > 0 Then
                                        ddlParentCategory.SelectedValue = CInt(ds.Tables(0).Rows(0)("CategoryParentId").ToString())
                                        ddllabxcat.Visible = False
                                    End If
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("MetaDescription").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("MetaDescription").ToString() <> "" Then
                                txtMetadescription.Text = ds.Tables(0).Rows(0)("MetaDescription").ToString()
                            End If

                            If Not ds.Tables(0).Rows(0)("MetaKeywords").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("MetaKeywords").ToString() <> "" Then
                                txtmetaKeywords.Text = ds.Tables(0).Rows(0)("MetaKeywords").ToString()
                            End If

                            If CInt(ds.Tables(0).Rows(0)("IsLabX")) > 0 Then
                                If CInt(ds.Tables(0).Rows(0)("IsLabX")) = 1 Then
                                    rdoIslabX.SelectedValue = "1"
                                    ddlParentCategory.Visible = False
                                Else
                                    rdoIslabX.SelectedValue = "2"
                                End If
                            End If
                            If CInt(ds.Tables(0).Rows(0)("LabXParentId").ToString()) > 0 Then
                                ddllabxcat.SelectedValue = CInt(ds.Tables(0).Rows(0)("LabXParentId").ToString())
                            End If
                            'ddllabxcat.SelectedValue = CInt(ds.Tables(0).Rows(0)("IsMainorLabX").ToString())
                        End If
                    End If
                End If
                btnAddCategory.Text = "Update Category"
            Catch ex As Exception
            End Try
        End If
    End Sub
    Private Sub RadioButton_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoIslabX.SelectedIndexChanged, rdoLevel.SelectedIndexChanged

        Try
            If rdoLevel.SelectedValue = "1" Then
                If rdoIslabX.SelectedValue = "1" Then
                    ddlParentCategory.Visible = False
                    ddllabxcat.Visible = True
                End If
            End If
            If rdoLevel.SelectedValue = "2" Then
                If rdoIslabX.SelectedValue = "1" Then
                    ddlParentCategory.Visible = False
                    ddllabxcat.Visible = True
                End If
            End If

            If rdoLevel.SelectedValue = "2" Then
                If rdoIslabX.SelectedValue = "2" Then
                    ddlParentCategory.Visible = True
                    ddllabxcat.Visible = False
                End If
            End If

            If rdoLevel.SelectedValue = "1" Then
                If rdoIslabX.SelectedValue = "2" Then
                    ddllabxcat.Visible = False
                    ddlParentCategory.Visible = True
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class
