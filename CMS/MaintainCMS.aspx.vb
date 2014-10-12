Imports System.Data
Imports System.IO

Partial Class CMS_MaintainCMS
    Inherits System.Web.UI.Page
    Dim sStylePrefix As String = "<div style=""font-size: 11px;	font-weight: normal !important;"">"
    '  Private VERSIONMAX As Integer = 100

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PageHeader1.ErrorMessage = ""

        If Not IsPostBack Then
            '   fill_ddlCategory()
            fill_ddlVersion(1)
            fill_treeview()
            ' Me.btnEdit.Disabled = True
            Me.btnUpdate.Enabled = False
            Me.btnUpdateNew.Enabled = False
            Me.hidCurCMSPage.Value = ""

            If Me.hidCurCMSPage.Value <> "" Then
                fillContent()
            End If
            Get_Files()
        End If

    End Sub

    Private Sub fill_ddlVersion(ByVal VersionMax As Integer)
        Me.ddlVersion.Items.Clear()
        For x As Integer = 1 To VersionMax
            Me.ddlVersion.Items.Add(New ListItem(x))
        Next
    End Sub
#Region "Treeview Methods"

    Private Sub fill_treeview()
        Me.TreeView1.Nodes.Clear()
        AddSystemCategories()
        Dim sSQL As String = "select ID, CMSCATEGORY,(select count(*) from cmspageref where cmscategoryid=id and LIVE='Y') as pagecount from refcmscategory "
        Dim ds As DataSet = SQLData.generic_select(sSQL, SQLData.ConnectionString)

        If ds.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In ds.Tables(0).Rows
                'create the node
                Dim oNode As New TreeNode(sStylePrefix & dr("CMSCategory") & " (" & dr("pagecount") & ")" & "</div>", dr("Id"), "~/icons/materials.png")
                'add sub nodes
                If CInt(dr("pagecount")) > 0 Then
                    GeneratePageNodes(oNode, dr("id"))
                End If
                oNode.Expanded = True
                Me.TreeView1.Nodes.Add(oNode)
            Next
        End If
        Dim lastNode As New TreeNode(sStylePrefix & "UnCategorized" & "</div>", 0, "~/icons/materials.png")
        GeneratePageNodes(lastNode, 0)
        lastNode.Expanded = True
        Me.TreeView1.Nodes.Add(lastNode)
        Me.TreeView1.NodeStyle.CssClass = "menulink"
    End Sub

    Private Sub AddSystemCategories()
        Dim oNode As TreeNode
        oNode = CreateSystemCatNode("Default Site Pages")
        oNode.ChildNodes.Add(CreateSystemNode("About Us Page", "AboutusMessage"))
        oNode.ChildNodes.Add(CreateSystemNode("Home Page", "HomePageMessage"))
        oNode.ChildNodes.Add(CreateSystemNode("Contact Page", "ContactMessage"))
        oNode.ChildNodes.Add(CreateSystemNode("Terms and Conditions Page", "TermsMessage"))
        ' oNode.ChildNodes.Add(CreateSystemNode("Testimonials Page", "TestamonialMessage"))
        oNode.ChildNodes.Add(CreateSystemNode("FAQ Page", "FAQMessage"))
        ' oNode.ChildNodes.Add(CreateSystemNode("Shipping Terms Page", "ShippingTermsMessage"))
        oNode.Expanded = False
        Me.TreeView1.Nodes.Add(oNode)

        'oNode = CreateSystemCatNode("Customer Order Text")
        'oNode.ChildNodes.Add(CreateSystemNode("Order Confirmation Message", "ConfirmationMessage"))
        'oNode.ChildNodes.Add(CreateSystemNode("Order Shipment Message", "ShipmentMessage"))
        'oNode.ChildNodes.Add(CreateSystemNode("Order Cancellation Message", "CancelOrderMessage"))
        'oNode.Expanded = False
        'Me.TreeView1.Nodes.Add(oNode)

        'oNode = CreateSystemCatNode("Cart Text")
        'oNode.ChildNodes.Add(CreateSystemNode("No Search Results Message", "NoResultsMsg"))
        'oNode.ChildNodes.Add(CreateSystemNode("Login Heading Message", "LoginHeadingText"))
        'oNode.ChildNodes.Add(CreateSystemNode("Login Register Message", "LoginRegisterText"))
        'oNode.ChildNodes.Add(CreateSystemNode("Login Already Registered Message", "LoginAlreadyRegText"))
        'oNode.ChildNodes.Add(CreateSystemNode("Login Anonymously Message", "LoginAnonymousText"))
        'oNode.Expanded = False
        'Me.TreeView1.Nodes.Add(oNode)

    End Sub

    Private Function CreateSystemCatNode(ByVal sText As String) As TreeNode
        Dim oNode As New TreeNode(sStylePrefix & sText & "</div>", sText, "~/icons/materials.png")
        oNode.Expanded = True
        Return oNode
    End Function

    Private Function CreateSystemNode(ByVal sText As String, ByVal sVal As String) As TreeNode
        Dim pageNode As New TreeNode(sStylePrefix & sText & "</div>", sVal, "~/icons/notviewed.gif")
        pageNode.ToolTip = sText
        Return pageNode
    End Function


    Private Sub GeneratePageNodes(ByRef oNode As TreeNode, ByVal iId As Integer)
        Dim sSQL As String = "SELECT CMSPAGE, CMSTITLE FROM CMSPAGEREF WHERE LIVE='Y' "
        If iId > 0 Then
            sSQL &= " AND CMSCATEGORYID=" & iId
        Else
            sSQL &= " AND CMSCATEGORYID <= 0 "
        End If
        sSQL &= " ORDER BY CMSTITLE"
        Dim ds As DataSet = SQLData.generic_select(sSQL, SQLData.ConnectionString)
        If ds.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In ds.Tables(0).Rows
                Dim pageNode As New TreeNode(sStylePrefix & dr("cmsTitle") & "</div>", dr("cmspage"), "~/icons/notviewed.gif")

                oNode.ChildNodes.Add(pageNode)
            Next
        End If
    End Sub

    Protected Sub TreeView1_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TreeView1.SelectedNodeChanged
        Dim objNode As TreeNode = TreeView1.SelectedNode
        If objNode.Depth = 1 Then
            If objNode.ToolTip.Trim = "" Then
                GetDisplayPage(objNode.Value)
            Else
                GetDisplayPageWebsite(objNode.Value, objNode.ToolTip)
            End If
        End If

    End Sub

#End Region

    Private Sub GetDisplayPageWebsite(ByVal sCMSPage As String, ByVal sCMSTitle As String)
        Dim sSQL As String = "SELECT " & sCMSPage & " FROM WEBSITE "
        Dim ds As DataSet = SQLData.generic_select(sSQL, SQLData.ConnectionString)
        'Me.btnEdit.Disabled = True
        Me.btnUpdate.Enabled = False
        Me.btnUpdateNew.Enabled = False
        ' Me.ddlCategory.Enabled = False
        Me.txtCMSTitle.Enabled = False

        Me.ddlVersion.Enabled = False
        Me.btnSetLive.Enabled = False
        Me.btnNewVersion.Enabled = False
        Me.txtPageURL.Enabled = False
        '  Me.hlnkViewPage.Enabled = False

        If ds.Tables(0).Rows.Count > 0 Then
            Dim dr As DataRow = ds.Tables(0).Rows(0)
            Me.txtCMSTitle.Text = sCMSTitle
            Dim sDetails As String = ""
            Me.lblDetails.Text = sDetails
            Me.txtContent.Text = HttpUtility.HtmlDecode(dr(sCMSPage).ToString().Trim)

            Dim sUrl As String = ""
            sUrl = "../forms/EditorPopUp.aspx?table=WEBSITE&id=1&idname=ID&field=" & sCMSPage & "&version=1"
            ' Me.btnEdit.Attributes.Add("onclick", "setRefresh();popUp('" & sUrl & "', 950, 800 )")
            Me.hidCurCMSPage.Value = sCMSPage
            ' Me.btnEdit.Disabled = False
            'Me.btnUpdate.Enabled = True
        Else
            Me.PageHeader1.ErrorMessage = "Error retrieving page info."
            Return
        End If
    End Sub


    Private Sub GetDisplayPage(ByVal sCMSPage As String)
        Dim sSQL As String = "SELECT * FROM CMSPAGEREF WHERE CMSPAGE='" & sCMSPage.Replace("'", "''") & "'  AND LIVE='Y'"
        Dim ds As DataSet = SQLData.generic_select(sSQL, SQLData.ConnectionString)
        ' Me.btnEdit.Disabled = True
        Me.btnUpdate.Enabled = False
        Me.btnUpdateNew.Enabled = False
        '  Me.ddlCategory.Enabled = True
        Me.txtCMSTitle.Enabled = True

        Me.ddlVersion.Enabled = True
        Me.btnSetLive.Enabled = True
        Me.btnNewVersion.Enabled = True
        Me.txtPageURL.Enabled = True
        ' Me.hlnkViewPage.Enabled = True
        If ds.Tables(0).Rows.Count > 0 Then
            Dim dr As DataRow = ds.Tables(0).Rows(0)
            DisplayPage(dr)
            ' Me.btnEdit.Disabled = False
            Me.btnUpdate.Enabled = True
            Me.btnUpdateNew.Enabled = True
        Else
            Me.PageHeader1.ErrorMessage = "Error retrieving page info."
            Return
        End If
    End Sub

    Private Sub GetDisplayPageVersion(ByVal sCMSPage As String, ByVal iVersion As Integer)
        Dim sSQL As String = "SELECT * FROM CMSPAGEREF WHERE CMSPAGE='" & sCMSPage.Replace("'", "''") & "' AND CMSVERSION='" & iVersion & "'"
        Dim ds As DataSet = SQLData.generic_select(sSQL, SQLData.ConnectionString)
        ' Me.btnEdit.Disabled = True
        Me.btnUpdate.Enabled = False
        Me.btnUpdateNew.Enabled = False
        ' Me.ddlCategory.Enabled = True
        Me.txtCMSTitle.Enabled = True
        If ds.Tables(0).Rows.Count > 0 Then
            Dim dr As DataRow = ds.Tables(0).Rows(0)
            DisplayPage(dr)
            ' Me.btnEdit.Disabled = False
            Me.btnUpdate.Enabled = True
        Else
            Me.PageHeader1.ErrorMessage = "Error retrieving page info."
            Return
        End If
    End Sub

    Private Sub DisplayPage(ByRef dr As DataRow)
        '  Me.ddlCategory.SelectedValue = dr("CMSCategoryid")
        Me.txtCMSTitle.Text = dr("CMSTitle")
        Dim sDetails As String = "<table width=""100%""><tr><td><b>Developer Ref:</b></td><td>" & dr("cmspage") & "</td></tr>"
        sDetails &= "<tr><td><b>Date Created:</b> </td><td>" & fixdate(dr("datecreated")) & "</td></tr>"
        sDetails &= "<tr><td><b>Last Modified:</b></td><td>" & fixdate(dr("datemodified")) & "</td></tr></table>"
        Me.lblDetails.Text = sDetails
        '  Me.lblContent.Text = dr("CMSContent")
        Me.txtContent.Text = HttpUtility.HtmlDecode(dr("CMSContent").ToString().Trim)
        Dim iMAXVERSION As Integer = GetMaxVersion(dr("cmspage"))
        fill_ddlVersion(iMAXVERSION)
        Me.ddlVersion.SelectedValue = dr("CMSVersion")
        If dr("LIVE") = "Y" Then
            Me.lblLiveCMS.Text = "LIVE IN SYSTEM"
            Me.btnSetLive.Enabled = False
        Else
            Me.lblLiveCMS.Text = ""
            Me.btnSetLive.Enabled = True
        End If

        Me.txtPageURL.Text = dr("PageURL")
        Dim sRefURL As String = ""
        Me.litPage.Text = ""
        If CStr(dr("PageURL")).Trim <> "" Then
            sRefURL = dr("pageUrl")
            If dr("LIVE") <> "Y" Then
                sRefURL &= "?version=" & dr("CMSVersion")
            End If
            ' Me.hlnkViewPage.NavigateUrl = sRefURL
            ' Me.hlnkViewPage.Enabled = True
            'Dim sFrameHTML As String = "<frameset><frame src=""" & sRefURL & """></frameset>"
            'Me.litPage.Text = sFrameHTML

        Else
            '  Me.hlnkViewPage.NavigateUrl = "#"
            ' Me.hlnkViewPage.Enabled = False
        End If

        Dim sUrl As String = ""
        sUrl = "../forms/EditorPopUp.aspx?table=CMSPAGEREF&id=" & dr("cmspage") & "&idname=CMSPAGE&field=CMSCONTENT&version=" & dr("CMSVersion")
        '  Me.btnEdit.Attributes.Add("onclick", "setRefresh();popUp('" & sUrl & "', 950, 800 )")
        Me.hidCurCMSPage.Value = dr("cmspage")
        metaTitle.Text = dr("metaTitle")
        metaKeywords.Text = dr("metaKeywords")
        metaDescription.Text = dr("metaDescription")
    End Sub

    Private Function GetMaxVersion(ByVal sPage As String) As Integer
        Dim sSQL As String = "SELECT MAX(CMSVERSION) FROM CMSPAGEREF WHERE CMSPAGE='" & sPage.Replace("'", "''") & "' "
        Dim sCnt As String = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        If sCnt = "" Then
            sCnt = "1"
        End If
        Return CInt(sCnt)
    End Function

    Private Function fixdate(ByVal s As Object) As String
        If s Is DBNull.Value Then
            Return "N/A"
        End If
        Try
            s = CType(s, DateTime).ToString("MM/dd/yyyy hh:mm tt")
        Catch ex As Exception
            s = "N/A"
        End Try
        Return s
    End Function

    Private Sub fillContent()
        Dim sSQL As String = "SELECT TOP 1 cmspage, datecreated, datemodified, cmscontent FROM CMSPAGEREF WHERE CMSPAGE='" & Me.hidCurCMSPage.Value.Replace("'", "''") & "' and CMSVersion=" & Me.ddlVersion.SelectedValue
        Dim ds As DataSet = SQLData.generic_select(sSQL, SQLData.ConnectionString)
        If ds.Tables(0).Rows.Count > 0 Then
            Dim dr As DataRow = ds.Tables(0).Rows(0)
            Dim sDetails As String = "<table width=""100%""><tr><td><b>Developer Ref:</b></td><td>" & dr("cmspage") & "</td></tr>"
            sDetails &= "<tr><td><b>Date Created:</b> </td><td>" & fixdate(dr("datecreated")) & "</td></tr>"
            sDetails &= "<tr><td><b>Last Modified:</b></td><td>" & fixdate(dr("datemodified")) & "</td></tr></table>"
            Me.lblDetails.Text = sDetails
            '  Me.lblContent.Text = dr("CMSContent")
            Me.txtContent.Text = HttpUtility.HtmlDecode(dr("CMSContent").ToString().Trim)
        Else
            Dim objNode As TreeNode = Me.TreeView1.SelectedNode
            If objNode.Depth = 1 Then
                If objNode.ToolTip.Trim = "" Then
                    'GetDisplayPage(objNode.Value)
                Else
                    GetDisplayPageWebsite(objNode.Value, objNode.ToolTip)
                End If
            End If
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Me.txtCMSTitle.Text.Trim = "" Then
            Me.PageHeader1.ErrorMessage = "You must enter a page title"
            Return
        End If

        Dim sUpd As String = "UPDATE CMSPAGEREF SET CMSTITLE='" & Me.txtCMSTitle.Text.Replace("'", "''") & "', IsFooter='" & CInt(Me.rdoIsFooter.SelectedValue.ToString().Replace("'", "''")) & "',IsLeftMenu='" & CInt(Me.rdoIsLeftSideBar.SelectedValue.ToString().Replace("'", "''")) & "',LeftMenuOrder='" & CInt(Me.txtLeftMenuOrder.Text.ToString().Replace("'", "''")) & "',FooterMenuOrder='" & CInt(Me.txtFooterMenuOrder.Text.ToString().Replace("'", "''")) & "', MetaTitle='" & Me.metaTitle.Text.Replace("'", "''") & "', metaKeywords='" & Me.metaKeywords.Text.Replace("'", "''") & "', metadescription='" & Me.metaDescription.Text.Replace("'", "''") & "', metatag='" & Me.metaTag.Text.Replace("'", "''") & "', PageURL='" & Me.txtPageURL.Text.Replace("'", "''") & "', CMSContent='" & txtContent.Text.ToString().Trim & "' WHERE CMSPAGE='" & Me.hidCurCMSPage.Value.Replace("'", "''") & "' "
        If SQLData.generic_command(sUpd, SQLData.ConnectionString) Then
            Me.PageHeader1.ErrorMessage = "Page Information Updated Successfully."
            fill_treeview()
            For Each objNode As TreeNode In TreeView1.Nodes
                For Each innerNode As TreeNode In objNode.ChildNodes
                    If innerNode.Value = Me.hidCurCMSPage.Value Then
                        innerNode.Selected = True
                        Exit For
                    End If
                Next
            Next
        Else
            Me.PageHeader1.ErrorMessage = "Error Updating Page Information."
        End If
    End Sub

#Region "Add Page Methods"

    Protected Sub btnbtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If valid_CMSMessage() Then
            'Generate the Insert Statement
            Dim sIns As String = "INSERT INTO CMSPAGEREF (CMSPAGE, WEBSITEID,CMSCONTENT, CMSTITLE, DATECREATED, DATEMODIFIED,LIVE, METATITLE, METAKEYWORDS, METADESCRIPTION, METATAG, ISFOOTER,ISLEFTMENU,LEFTMENUORDER,FOOTERMENUORDER) VALUES " & _
                " ('" & Me.txtNewCMSPage.Text.Replace("'", "''") & "', '1','Please Add Content','" & Me.txtNewCMSTitle.Text.Replace("'", "''") & "', GetDate(), GetDate(), 'Y'" & ",'" & metaTitle.Text.Replace("'", "''") & "','" & metaKeywords.Text.Replace("'", "''") & "','" & metaDescription.Text.Replace("'", "''") & "','" & metaTag.Text.Replace("'", "''") & "','" & CInt(rdoIsFooter.SelectedValue.ToString().Replace("'", "''")) & "','" & CInt(rdoIsLeftSideBar.SelectedValue.ToString().Replace("'", "''")) & "','" & CInt(txtLeftMenuOrder.Text.ToString().Replace("'", "''")) & "','" & CInt(txtFooterMenuOrder.Text.ToString().Replace("'", "''")) & "');"
            If SQLData.generic_command(sIns, SQLData.ConnectionString) Then
                Me.PageHeader1.ErrorMessage = "Content Page Added Successfully."
                Me.hidCurCMSPage.Value = Me.txtNewCMSPage.Text
                Me.txtNewCMSPage.Text = ""
                Me.txtNewCMSTitle.Text = ""
                fill_treeview()
                For Each objNode As TreeNode In TreeView1.Nodes
                    For Each innerNode As TreeNode In objNode.ChildNodes
                        If innerNode.Value = Me.hidCurCMSPage.Value Then
                            innerNode.Selected = True
                            GetDisplayPage(Me.hidCurCMSPage.Value)
                            Exit For
                        End If
                    Next
                Next
            Else
                Me.PageHeader1.ErrorMessage = "Error Adding Content Page."
            End If
        End If
    End Sub

    Private Function valid_CMSMessage() As Boolean
        Dim sError As String = ""
        If Me.txtNewCMSPage.Text = "" Then
            sError &= "You must enter a Content Page Name.<br>"
        End If
        If Me.txtNewCMSPage.Text.IndexOf(" ") <> -1 Then
            sError &= "Content Page Name CANNOT contain spaces.<br>"
        End If
        If Me.txtNewCMSPage.Text.IndexOf("'") <> -1 Then
            sError &= "Content Page Name CANNOT contain ' (single quote).<br>"
        End If
        If Me.txtNewCMSTitle.Text = "" Then
            sError &= "You Must Enter a Content Page Display Title.<br>"
        End If
        If Me.rdoIsFooter.SelectedIndex = -1 Then
            sError &= "You Must Select Want to show in footer or not.<br>"
        End If
        If Me.rdoIsLeftSideBar.SelectedIndex = -1 Then
            sError &= "You Must Select Want to show in Left Side Bar or not.<br>"
        End If
        If Me.txtLeftMenuOrder.Text = "" Then
            sError &= "You Must Enter a Left Menu Order Position.<br>"
        End If
        If Me.txtFooterMenuOrder.Text = "" Then
            sError &= "You Must Enter a Footer Menu Order Position.<br>"
        End If
        'test if it exists
        Dim sSQL As String = "SELECT COUNT(*) FROM CMSPAGEREF WHERE CMSPAGE='" & Me.txtNewCMSPage.Text.Replace("'", "''") & "'"
        Dim sCnt As String = SQLData.generic_scalar(sSQL, SQLData.ConnectionString)
        If sCnt = "" Then
            sCnt = "0"
        End If
        If CInt(sCnt) > 0 Then
            sError &= "Content Page Name already exists in the system.<br>"
        End If
        If sError.Trim = "" Then
            Return True
        Else
            Me.PageHeader1.ErrorMessage = sError
            Return False
        End If
    End Function
#End Region

#Region "Version Methods"
    Protected Sub ddlVersion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVersion.SelectedIndexChanged
        GetDisplayPageVersion(Me.hidCurCMSPage.Value, Me.ddlVersion.SelectedValue)
    End Sub

    Protected Sub btnNewVersion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewVersion.Click
        'add a new version of the current page
        Dim sPageRef As String = Me.hidCurCMSPage.Value
        Dim iMaxVersion As Integer = Me.GetMaxVersion(sPageRef)
        iMaxVersion += 1
        Dim sIns As String = GenerateVersionInsert(sPageRef, iMaxVersion)
        If SQLData.generic_command(sIns, SQLData.ConnectionString) Then
            Me.GetDisplayPageVersion(sPageRef, iMaxVersion)
            Me.PageHeader1.ErrorMessage = "New version added successfully."
        Else
            Me.PageHeader1.ErrorMessage = "Error adding new version."
        End If
    End Sub

    Private Function GenerateVersionInsert(ByVal sPageRef As String, ByVal iVersion As Integer) As String
        Dim sb As New System.Text.StringBuilder
        With sb
            .Append(" INSERT INTO [CMSPageRef] ([CMSPage] ")
            .Append(",[WebsiteID] ")
            .Append(",[CMSContent] ")
            .Append(",[CMSTitle] ")
            .Append(",[DateCreated] ")
            .Append(",[DateModified] ")
            .Append(",[EditorName] ")
            .Append(",[AffiliateID] ")
            .Append(",[CustomerID] ")
            .Append(",[CMSCategoryId] ")
            .Append(",[CMSVersion] ")
            .Append(",[Live], [PageURL]) ")
            .Append("( SELECT TOP 1 [CMSPage] ")
            .Append(",[WebsiteID] ")
            .Append(",[CMSContent] ")
            .Append(",[CMSTitle] ")
            .Append(",GetDate()  as [DateCreated] ")
            .Append(",GetDate() as [DateModified] ")
            If Not Session("UserName") Is Nothing Then
                .Append(",'" & Session("UserName").ToString().Replace("'", "''") & "' as [EditorName] ")
            Else
                .Append(",'Test' as [EditorName] ")
            End If
            .Append(",[AffiliateID] ")
            .Append(",[CustomerID] ")
            .Append(",0 as [CMSCategoryId] ")
            .Append("," & iVersion & " as [CMSVersion] ")
            .Append(",'N' as [Live], [PageURL] FROM CMSPAGEREF WHERE CMSPAGE='" & sPageRef.Replace("'", "''") & "')")
        End With
        Return sb.ToString
    End Function

    Protected Sub btnSetLive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetLive.Click
        'make the current version the live version
        Dim sPageRef As String = Me.hidCurCMSPage.Value
        Dim sb As New System.Text.StringBuilder
        With sb
            .Append("UPDATE CMSPAGEREF set LIVE='N' where cmspage='" & sPageRef.Replace("'", "''") & "'")
            .Append("UPDATE CMSPAGEREF set LIVE='Y' where cmspage='" & sPageRef.Replace("'", "''") & "' AND CMSVERSION='" & Me.ddlVersion.SelectedValue & "';")
        End With
        If SQLData.Generic_SQLTransaction(sb, SQLData.ConnectionString) Then
            Me.PageHeader1.ErrorMessage = "Page set to live."
            Me.GetDisplayPageVersion(sPageRef, Me.ddlVersion.SelectedValue)
        Else
            Me.PageHeader1.ErrorMessage = "Error setting page live."
        End If
    End Sub
#End Region

    
    Private Sub Get_Files()
        Try
            Dim str As String = Path.Combine(Request.PhysicalApplicationPath, "ProductImages\Large\CMS\")
            Dim files As String() = Directory.GetFiles(str)
            Array.Sort(files)
            Dim html As String = ""
            For Each filePath As String In files
                Dim fileExt As String = Path.GetExtension(filePath).ToLower()
                If fileExt = ".jpg" Or fileExt = ".jpeg" Or fileExt = ".gif" Or fileExt = ".png" Or fileExt = ".bmp" Then
                    html += "<img src='" & System.Configuration.ConfigurationManager.AppSettings.Get("ImageURL") & "/CMS/" & Path.GetFileName(filePath) & "'  style='float:left;margin:10px 0 0 10px;' alt='' Width='100px' Height='100px'>"
                End If
            Next
            ImageContainer.InnerHtml = html
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub btnNewVersionNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewVersionNew.Click
        btnNewVersion_Click(sender, e)
    End Sub
    Protected Sub btnUpdateNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateNew.Click
        btnUpdate_Click(sender, e)
    End Sub

    Protected Sub btnupload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnupload.Click
        Try
            If flImage.FileName <> "" Then
                Dim filePath As String = Path.Combine(Request.PhysicalApplicationPath, "ProductImages/Large/CMS/")
                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If
                Dim File As String = Path.Combine(filePath, flImage.FileName)
                Dim fileExt As String = Path.GetExtension(flImage.FileName).ToLower()
                If fileExt = ".jpg" Or fileExt = ".jpeg" Or fileExt = ".gif" Or fileExt = ".png" Or fileExt = ".bmp" Then
                    If Not System.IO.File.Exists(File) Then
                        flImage.SaveAs(File)
                        Me.PageHeader1.ErrorMessage = "File Uploaded Sucessfully"
                        Get_Files()
                    Else
                        Me.PageHeader1.ErrorMessage = "File Exits!!!"
                        Get_Files()
                    End If
                    Session("fileName") = flImage.FileName
                End If
            Else
                Me.PageHeader1.ErrorMessage = "Please select a file"
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class
