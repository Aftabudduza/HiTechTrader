Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_InquiryEmail
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nProductID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Dim ProductId As Integer = 0
    Dim sSQL As String = ""
    Dim imgarrow As String = ""
    Dim sOrderBy As String = ""
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Session("Id") Is Nothing Then
                If Not Page.IsPostBack Then
                    Session("pagerSQL") = Nothing
                    Session("strOrder") = Nothing
                    Session("strOrderValue") = Nothing
                    Session("strwhere") = Nothing
                    Session("pagerSQL") = " SELECT * FROM OrderHistory oh "
                    If Not Session("strwhere") Is Nothing Then
                        If Not Session("strOrder") Is Nothing Then
                            sSQL = Session("pagerSQL") & Session("strwhere") & "  " & Session("strOrder").ToString()
                            SqlDataSource1.SelectCommand = sSQL
                        Else
                            SqlDataSource1.SelectCommand = Session("pagerSQL") & Session("strwhere") & " ORDER BY oh.Orderdate DESC "
                        End If
                    Else
                        Session("strOrderValue") = 6
                        Session("strOrder") = "  DESC"
                        SqlDataSource1.SelectCommand = Session("pagerSQL") & " WHERE oh.OrderDate BETWEEN DATEADD(dd, -30, GETDATE()) AND GETDATE() ORDER BY oh.Orderdate DESC "
                        ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                        lnk30days.Attributes("Class") = "lnkdays"
                        lnk30days.Enabled = False
                    End If
                Else
                    If Not Session("pagerSQL") Is Nothing Then
                        If Not Session("strwhere") Is Nothing Then
                            If Not Session("strOrder") Is Nothing Then
                                sSQL = Session("pagerSQL") & Session("strwhere") & "  " & Session("strOrder").ToString()
                                SqlDataSource1.SelectCommand = sSQL
                            Else
                                SqlDataSource1.SelectCommand = Session("pagerSQL") & Session("strwhere") & " ORDER BY oh.Orderdate DESC "
                            End If
                        Else
                            SqlDataSource1.SelectCommand = Session("pagerSQL") & " ORDER BY oh.Orderdate DESC "
                        End If
                    End If
                End If
            Else
                Response.Redirect("Login.aspx")
            End If
        Catch ex As Exception

        End Try
       
    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub

    Protected Sub lnkChkAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkChkAll.Click
        Try
            Dim checkbox As CheckBox = Nothing
            Dim i As Integer = 0

            For Each ListItem In lvInquiry.Items
                checkbox = ListItem.FindControl("chkDelete")
                If checkbox.Checked = True Then
                    checkbox.Checked = False
                Else
                    checkbox.Checked = True
                End If
                If checkbox.Checked = True Then
                    i = i + 1
                End If
            Next

            If i = lvInquiry.Items.Count Then
                lnkChkAll.Text = "Uncheck All"
            Else
                lnkChkAll.Text = "Check All"
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkDeleteAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeleteAll.Click
        Try

            Dim checkbox As CheckBox = Nothing
            Dim OrderHistory As New OrderHistory(appGlobal.CONNECTIONSTRING)
            For Each ListItem In lvInquiry.Items
                checkbox = ListItem.FindControl("chkDelete")
                Dim hdID As HiddenField = CType(ListItem.FindControl("hdID"), HiddenField)
                Dim nInquiryID As Integer = CInt(hdID.Value)
                If checkbox.Checked = True Then
                    If nInquiryID > 0 Then
                        If OrderHistory.Delete(nInquiryID) Then

                        End If
                    End If
                End If
            Next

            Session("pagerSQL") = " SELECT * FROM OrderHistory oh "
            If Not Session("strwhere") Is Nothing Then
                If Not Session("strOrder") Is Nothing Then
                    sSQL = Session("pagerSQL") & Session("strwhere") & Session("strOrder").ToString()
                    SqlDataSource1.SelectCommand = sSQL
                Else
                    SqlDataSource1.SelectCommand = Session("pagerSQL") & Session("strwhere") & " ORDER BY oh.Orderdate DESC "
                End If
            Else
                Session("strOrderValue") = 6
                Session("strOrder") = "DESC"
                SqlDataSource1.SelectCommand = Session("pagerSQL") & " WHERE oh.OrderDate BETWEEN DATEADD(dd, -30, GETDATE()) AND GETDATE() ORDER BY oh.Orderdate DESC "
                ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                lnk30days.Attributes("Class") = "lnkdays"
                lnk30days.Enabled = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnk30days_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk30days.Click
        Try
            lnk30days.Attributes("Class") = "lnkdays"
            lnk30days.Enabled = False
            lnk6months.Enabled = True
            lnk6months.Attributes("Class") = ""
            lnk1year.Enabled = True
            lnk1year.Attributes("Class") = ""
            lnkall.Enabled = True
            lnkall.Attributes("Class") = ""
            sOrderBy = String.Empty
            sSQL = String.Empty
            Session("strwhere") = " WHERE oh.OrderDate BETWEEN DATEADD(dd, -30, GETDATE()) AND GETDATE() "
            Dim sOrdervalue As String = ""
            If Not Session("strOrderValue") Is Nothing Then
                If Session("strOrderValue").ToString() = 1 Then
                    ImgArrowType.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " ASC "
                    sOrdervalue = "ORDER BY oh.[Type] "
                ElseIf Session("strOrderValue").ToString() = 2 Then
                    ImgArrowType.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC "
                    sOrdervalue = "ORDER BY oh.[Type] "

                ElseIf Session("strOrderValue").ToString() = 3 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " ASC "
                    sOrdervalue = "ORDER BY oh.PONumber "

                ElseIf Session("strOrderValue").ToString() = 4 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC "
                    sOrdervalue = "ORDER BY oh.PONumber "

                ElseIf Session("strOrderValue").ToString() = 5 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " ASC"
                    sOrdervalue = "ORDER BY oh.Orderdate "

                ElseIf Session("strOrderValue").ToString() = 6 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC"
                    sOrdervalue = "ORDER BY oh.Orderdate "

                ElseIf Session("strOrderValue").ToString() = 7 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " ASC"
                    sOrdervalue = "ORDER BY oh.[Name] "
                ElseIf Session("strOrderValue").ToString() = 8 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC"
                    sOrdervalue = "ORDER BY oh.[Name] "
                ElseIf Session("strOrderValue").ToString() = 9 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    sOrderBy = " ASC"
                    sOrdervalue = "ORDER BY oh.Email "
                ElseIf Session("strOrderValue").ToString() = 10 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    sOrderBy = " DESC"
                    sOrdervalue = "ORDER BY oh.Email "
                End If

                Session("strOrder") = sOrderBy.ToString().Trim

                If Not Session("pagerSQL") Is Nothing Then
                    If Not Session("strwhere") Is Nothing Then
                        sSQL = Session("pagerSQL").ToString() & Session("strwhere").ToString() & sOrdervalue.ToString() & sOrderBy.ToString()
                    Else
                        sSQL = Session("pagerSQL").ToString() & " WHERE oh.OrderDate BETWEEN DATEADD(yy, -1, GETDATE()) AND GETDATE() " & sOrdervalue.ToString() & sOrderBy.ToString()
                    End If
                    SqlDataSource1.SelectCommand = sSQL
                End If
            Else
                If Not Session("pagerSQL") Is Nothing Then
                    If Not Session("strwhere") Is Nothing Then
                        sSQL = Session("pagerSQL").ToString() & Session("strwhere").ToString() & " ORDER BY oh.Orderdate desc "
                    Else
                        sSQL = Session("pagerSQL").ToString() & " WHERE oh.OrderDate BETWEEN DATEADD(yy, -1, GETDATE()) AND GETDATE() ORDER BY oh.Orderdate desc "
                    End If
                    SqlDataSource1.SelectCommand = sSQL
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnk1year_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk1year.Click
        Try
            Try
                lnk30days.Attributes("Class") = ""
                lnk30days.Enabled = True
                lnk6months.Enabled = True
                lnk6months.Attributes("Class") = ""
                lnk1year.Enabled = False
                lnk1year.Attributes("Class") = "lnkdays"
                lnkall.Enabled = True
                lnkall.Attributes("Class") = ""
                sOrderBy = String.Empty
                sSQL = String.Empty
                Session("strwhere") = " WHERE oh.OrderDate BETWEEN DATEADD(yy, -1, GETDATE()) AND GETDATE() "
                Dim sOrdervalue As String = ""
                If Not Session("strOrderValue") Is Nothing Then
                    If Session("strOrderValue").ToString() = 1 Then
                        ImgArrowType.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " ASC"
                        sOrdervalue = "ORDER BY oh.[Type] "
                    ElseIf Session("strOrderValue").ToString() = 2 Then
                        ImgArrowType.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " DESC"
                        sOrdervalue = "ORDER BY oh.[Type] "

                    ElseIf Session("strOrderValue").ToString() = 3 Then
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " ASC "
                        sOrdervalue = "ORDER BY oh.PONumber "

                    ElseIf Session("strOrderValue").ToString() = 4 Then
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " DESC "
                        sOrdervalue = "ORDER BY oh.PONumber "

                    ElseIf Session("strOrderValue").ToString() = 5 Then
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " ASC"
                        sOrdervalue = "ORDER BY oh.Orderdate "

                    ElseIf Session("strOrderValue").ToString() = 6 Then
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " DESC"
                        sOrdervalue = "ORDER BY oh.Orderdate "

                    ElseIf Session("strOrderValue").ToString() = 7 Then
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " ASC"
                        sOrdervalue = "ORDER BY oh.[Name] "
                    ElseIf Session("strOrderValue").ToString() = 8 Then
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " DESC"
                        sOrdervalue = "ORDER BY oh.[Name] "
                    ElseIf Session("strOrderValue").ToString() = 9 Then
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                        sOrderBy = " ASC"
                        sOrdervalue = "ORDER BY oh.Email "
                    ElseIf Session("strOrderValue").ToString() = 10 Then
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                        sOrderBy = " DESC"
                        sOrdervalue = "ORDER BY oh.Email "
                    End If

                    Session("strOrder") = sOrderBy.ToString().Trim
                    If Not Session("pagerSQL") Is Nothing Then
                        If Not Session("strwhere") Is Nothing Then
                            sSQL = Session("pagerSQL").ToString() & Session("strwhere").ToString() & sOrdervalue.ToString() & sOrderBy.ToString()
                        Else
                            sSQL = Session("pagerSQL").ToString() & " WHERE oh.OrderDate BETWEEN DATEADD(yy, -1, GETDATE()) AND GETDATE() " & sOrdervalue.ToString() & sOrderBy.ToString()
                        End If
                        SqlDataSource1.SelectCommand = sSQL
                    End If
                Else
                    If Not Session("pagerSQL") Is Nothing Then
                        If Not Session("strwhere") Is Nothing Then
                            sSQL = Session("pagerSQL").ToString() & Session("strwhere").ToString() & " ORDER BY oh.Orderdate desc "
                        Else
                            sSQL = Session("pagerSQL").ToString() & " WHERE oh.OrderDate BETWEEN DATEADD(yy, -1, GETDATE()) AND GETDATE()  ORDER BY oh.Orderdate desc "
                        End If
                        SqlDataSource1.SelectCommand = sSQL
                    End If
                End If

            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnk6months_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk6months.Click
        Try
            lnk30days.Attributes("Class") = ""
            lnk30days.Enabled = True
            lnk6months.Enabled = False
            lnk6months.Attributes("Class") = "lnkdays"
            lnk1year.Enabled = True
            lnk1year.Attributes("Class") = ""
            lnkall.Enabled = True
            lnkall.Attributes("Class") = ""
            sOrderBy = String.Empty
            sSQL = String.Empty
            Session("strwhere") = " WHERE oh.OrderDate BETWEEN DATEADD(dd, -180, GETDATE()) AND GETDATE() "
            Dim sOrdervalue As String = ""
            If Not Session("strOrderValue") Is Nothing Then
                If Session("strOrderValue").ToString() = 1 Then
                    ImgArrowType.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " ASC"
                    sOrdervalue = "ORDER BY oh.[Type] "
                ElseIf Session("strOrderValue").ToString() = 2 Then
                    ImgArrowType.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC"
                    sOrdervalue = "ORDER BY oh.[Type] "

                ElseIf Session("strOrderValue").ToString() = 3 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " ASC "
                    sOrdervalue = "ORDER BY oh.PONumber "

                ElseIf Session("strOrderValue").ToString() = 4 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC "
                    sOrdervalue = "ORDER BY oh.PONumber "

                ElseIf Session("strOrderValue").ToString() = 5 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " ASC"
                    sOrdervalue = "ORDER BY oh.Orderdate "

                ElseIf Session("strOrderValue").ToString() = 6 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC"
                    sOrdervalue = "ORDER BY oh.Orderdate "

                ElseIf Session("strOrderValue").ToString() = 7 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " ASC"
                    sOrdervalue = "ORDER BY oh.[Name] "
                ElseIf Session("strOrderValue").ToString() = 8 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC"
                    sOrdervalue = "ORDER BY oh.[Name] "
                ElseIf Session("strOrderValue").ToString() = 9 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    sOrderBy = " ASC"
                    sOrdervalue = "ORDER BY oh.Email "
                ElseIf Session("strOrderValue").ToString() = 10 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    sOrderBy = " DESC"
                    sOrdervalue = " ORDER BY oh.Email "
                End If

                Session("strOrder") = sOrderBy.ToString().Trim
                If Not Session("pagerSQL") Is Nothing Then
                    If Not Session("strwhere") Is Nothing Then
                        sSQL = Session("pagerSQL").ToString() & Session("strwhere").ToString() & sOrdervalue.ToString() & Session("strOrder").ToString()
                    Else
                        sSQL = Session("pagerSQL").ToString() & " WHERE oh.OrderDate BETWEEN DATEADD(dd, -180, GETDATE()) AND GETDATE() " & sOrdervalue.ToString() & Session("strOrder").ToString()
                    End If
                    SqlDataSource1.SelectCommand = sSQL
                End If
            Else
                If Not Session("pagerSQL") Is Nothing Then
                    If Not Session("strwhere") Is Nothing Then
                        sSQL = Session("pagerSQL").ToString() & Session("strwhere").ToString() & " ORDER BY oh.Orderdate desc "
                    Else
                        sSQL = Session("pagerSQL").ToString() & " WHERE oh.OrderDate BETWEEN DATEADD(dd, -180, GETDATE()) AND GETDATE()  ORDER BY oh.Orderdate desc "
                    End If
                    SqlDataSource1.SelectCommand = sSQL
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkall.Click
        Try
            sOrderBy = String.Empty
            sSQL = String.Empty
            lnk30days.Attributes("Class") = ""
            lnk30days.Enabled = True
            lnk6months.Enabled = True
            lnk6months.Attributes("Class") = ""
            lnk1year.Enabled = True
            lnk1year.Attributes("Class") = ""
            lnkall.Enabled = False
            lnkall.Attributes("Class") = "lnkdays"
            Dim sOrdervalue As String = ""
            sOrderBy = String.Empty
            sSQL = String.Empty
            If Not Session("strOrderValue") Is Nothing Then
                If Session("strOrderValue").ToString() = 1 Then
                    ImgArrowType.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " ASC"
                    sOrdervalue = " ORDER BY oh.[Type] "
                ElseIf Session("strOrderValue").ToString() = 2 Then
                    ImgArrowType.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC"
                    sOrdervalue = "ORDER BY oh.[Type] "

                ElseIf Session("strOrderValue").ToString() = 3 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " ASC "
                    sOrdervalue = "ORDER BY oh.PONumber "

                ElseIf Session("strOrderValue").ToString() = 4 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC "
                    sOrdervalue = "ORDER BY oh.PONumber "

                ElseIf Session("strOrderValue").ToString() = 5 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " ASC"
                    sOrdervalue = "ORDER BY oh.Orderdate "

                ElseIf Session("strOrderValue").ToString() = 6 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC"
                    sOrdervalue = "ORDER BY oh.Orderdate "

                ElseIf Session("strOrderValue").ToString() = 7 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " ASC"
                    sOrdervalue = "ORDER BY oh.[Name] "
                ElseIf Session("strOrderValue").ToString() = 8 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC"
                    sOrdervalue = "ORDER BY oh.[Name] "
                ElseIf Session("strOrderValue").ToString() = 9 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                    sOrderBy = " ASC"
                    sOrdervalue = "ORDER BY oh.Email "
                ElseIf Session("strOrderValue").ToString() = 10 Then
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    sOrderBy = " DESC"
                    sOrdervalue = "ORDER BY oh.Email "
                End If

                Session("strOrder") = sOrdervalue.ToString().Trim & sOrderBy.ToString().Trim

                If Not Session("pagerSQL") Is Nothing Then
                    If Not Session("strOrder") Is Nothing Then
                        sSQL = Session("pagerSQL").ToString() & sOrdervalue.ToString() & sOrderBy.ToString()
                    Else
                        sSQL = Session("pagerSQL").ToString() & " ORDER BY oh.Orderdate desc "
                    End If
                    SqlDataSource1.SelectCommand = sSQL
                End If
            Else
                If Not Session("pagerSQL") Is Nothing Then
                    sSQL = Session("pagerSQL").ToString()
                Else
                    sSQL = Session("pagerSQL").ToString() & " ORDER BY oh.Orderdate desc "
                End If
                SqlDataSource1.SelectCommand = sSQL
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkType.Click
        Try
            sOrderBy = String.Empty
            sSQL = String.Empty
            If Not Session("strOrder") Is Nothing Then
                If Session("strOrder") = "DESC" Then
                    If Session("strOrderValue").ToString() = "1" Then
                        Session("strOrderValue") = 2
                        ImgArrowType.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " DESC"
                    Else
                        Session("strOrderValue") = 1
                        ImgArrowType.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " ASC"

                    End If
                Else
                    Session("strOrderValue") = 1
                    ImgArrowType.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC"
                End If
            Else
                Session("strOrderValue") = 1
                ImgArrowType.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                imgArrowItem.Attributes("src") = ""
                ImgArrowDate.Attributes("src") = ""
                imgArrowName.Attributes("src") = ""
                imgArrowEmail.Attributes("src") = ""
                sOrderBy = " DESC"
            End If


            Session("strOrder") = sOrderBy.ToString().Trim

            If Not Session("pagerSQL") Is Nothing Then
                If Not Session("strwhere") Is Nothing Then
                    sSQL = Session("pagerSQL").ToString() & Session("strwhere").ToString() & " ORDER BY oh.[Type] " & Session("strOrder").ToString()
                Else
                    sSQL = Session("pagerSQL").ToString() & " WHERE oh.OrderDate BETWEEN DATEADD(dd, -30, GETDATE()) AND GETDATE() " & "ORDER BY oh.[Type] " & Session("strOrder").ToString()
                End If

                SqlDataSource1.SelectCommand = sSQL

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkItem.Click
        Try
            sOrderBy = String.Empty
            sSQL = String.Empty
            If Not Session("strOrder") Is Nothing Then
                If Session("strOrder") = "DESC" Then
                    If Session("strOrderValue").ToString() = "3" Then
                        Session("strOrderValue") = 4
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " DESC "
                    Else
                        Session("strOrderValue") = 3
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " ASC "
                    End If
                Else
                    Session("strOrderValue") = 3
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC "
                End If
            Else
                Session("strOrderValue") = 3
                ImgArrowType.Attributes("src") = ""
                imgArrowItem.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                ImgArrowDate.Attributes("src") = ""
                imgArrowName.Attributes("src") = ""
                imgArrowEmail.Attributes("src") = ""
                sOrderBy = " DESC "
            End If

            Session("strOrder") = sOrderBy.ToString().Trim

            If Not Session("pagerSQL") Is Nothing Then
                If Not Session("strwhere") Is Nothing Then
                    sSQL = Session("pagerSQL").ToString() & Session("strwhere").ToString() & " ORDER BY oh.PONumber " & Session("strOrder").ToString()
                Else
                    sSQL = Session("pagerSQL").ToString() & " WHERE oh.OrderDate BETWEEN DATEADD(dd, -30, GETDATE()) AND GETDATE()" & " ORDER BY oh.PONumber " & Session("strOrder").ToString()
                End If
                SqlDataSource1.SelectCommand = sSQL
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDate.Click
        Try
            sOrderBy = String.Empty
            sSQL = String.Empty
            If Not Session("strOrder") Is Nothing Then
                If Session("strOrder") = "DESC" Then
                    If Session("strOrderValue").ToString() = "5" Then
                        Session("strOrderValue") = 6
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " DESC"
                    Else
                        Session("strOrderValue") = 5
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " ASC"
                    End If
                Else
                    Session("strOrderValue") = 5
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC "
                End If
            Else
                Session("strOrderValue") = 5
                ImgArrowType.Attributes("src") = ""
                imgArrowItem.Attributes("src") = ""
                ImgArrowDate.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                imgArrowName.Attributes("src") = ""
                imgArrowEmail.Attributes("src") = ""
                sOrderBy = " DESC "
            End If
            Session("strOrder") = sOrderBy.ToString().Trim
            If Not Session("pagerSQL") Is Nothing Then
                If Not Session("strwhere") Is Nothing Then
                    sSQL = Session("pagerSQL").ToString() & Session("strwhere").ToString() & " ORDER BY oh.Orderdate " & Session("strOrder").ToString()
                Else
                    sSQL = Session("pagerSQL").ToString() & " WHERE oh.OrderDate BETWEEN DATEADD(dd, -30, GETDATE()) AND GETDATE()" & " ORDER BY oh.Orderdate " & Session("strOrder").ToString()
                End If

                SqlDataSource1.SelectCommand = sSQL

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkName.Click
        Try
            sOrderBy = String.Empty
            sSQL = String.Empty
            If Not Session("strOrder") Is Nothing Then
                If Session("strOrder") = "DESC" Then
                    If Session("strOrderValue").ToString() = "7" Then
                        Session("strOrderValue") = 8
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " DESC"
                    Else
                        Session("strOrderValue") = 7
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                        imgArrowEmail.Attributes("src") = ""
                        sOrderBy = " ASC"
                    End If
                Else
                    Session("strOrderValue") = 7
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    imgArrowEmail.Attributes("src") = ""
                    sOrderBy = " DESC "
                End If
            Else
                Session("strOrderValue") = 7
                ImgArrowType.Attributes("src") = ""
                imgArrowItem.Attributes("src") = ""
                ImgArrowDate.Attributes("src") = ""
                imgArrowName.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                imgArrowEmail.Attributes("src") = ""
                sOrderBy = " DESC "
            End If

            Session("strOrder") = sOrderBy.ToString().Trim

            If Not Session("pagerSQL") Is Nothing Then
                If Not Session("strwhere") Is Nothing Then
                    sSQL = Session("pagerSQL").ToString() & Session("strwhere").ToString() & " ORDER BY oh.[Name] " & Session("strOrder").ToString()
                Else
                    sSQL = Session("pagerSQL").ToString() & " WHERE oh.OrderDate BETWEEN DATEADD(dd, -30, GETDATE()) AND GETDATE()" & " ORDER BY oh.[Name] " & Session("strOrder").ToString()
                End If

                SqlDataSource1.SelectCommand = sSQL

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEmail.Click
        Try
            sOrderBy = String.Empty
            sSQL = String.Empty
            If Not Session("strOrder") Is Nothing Then
                If Session("strOrder") = "DESC" Then
                    If Session("strOrderValue").ToString() = "9" Then
                        Session("strOrderValue") = 10
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                        sOrderBy = " DESC"
                    Else
                        Session("strOrderValue") = 9
                        ImgArrowType.Attributes("src") = ""
                        imgArrowItem.Attributes("src") = ""
                        ImgArrowDate.Attributes("src") = ""
                        imgArrowName.Attributes("src") = ""
                        imgArrowEmail.Attributes("src") = "../App_Themes/Hitech/images/SortAsc.gif"
                        sOrderBy = " ASC"
                    End If
                Else
                    Session("strOrderValue") = 9
                    ImgArrowType.Attributes("src") = ""
                    imgArrowItem.Attributes("src") = ""
                    ImgArrowDate.Attributes("src") = ""
                    imgArrowName.Attributes("src") = ""
                    imgArrowEmail.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                    sOrderBy = " DESC "
                End If
            Else
                Session("strOrderValue") = 9
                ImgArrowType.Attributes("src") = ""
                imgArrowItem.Attributes("src") = ""
                ImgArrowDate.Attributes("src") = ""
                imgArrowName.Attributes("src") = ""
                imgArrowEmail.Attributes("src") = "../App_Themes/Hitech/images/SortDesc.gif"
                sOrderBy = " DESC "
            End If
            Session("strOrder") = sOrderBy.ToString().Trim

            If Not Session("pagerSQL") Is Nothing Then
                If Not Session("strwhere") Is Nothing Then
                    sSQL = Session("pagerSQL").ToString() & Session("strwhere").ToString() & " ORDER BY oh.Email " & Session("strOrder").ToString()
                Else
                    sSQL = Session("pagerSQL").ToString() & " WHERE oh.OrderDate BETWEEN DATEADD(dd, -30, GETDATE()) AND GETDATE()" & " ORDER BY oh.Email " & Session("strOrder").ToString()
                End If

                SqlDataSource1.SelectCommand = sSQL

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lvInquiry_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvInquiry.ItemDataBound
        Try
            If e.Item.ItemType = ListViewItemType.DataItem Then
                Dim sLabel As Label = DirectCast(e.Item.FindControl("lbltype"), Label)
                If sLabel.Text = "Order" Then
                    DirectCast(e.Item.FindControl("lvtr1"), HtmlTableRow).Style.Add("background", "#fbf4ee")
                    DirectCast(e.Item.FindControl("lvtr2"), HtmlTableRow).Style.Add("background", "#fbf4ee")

                ElseIf sLabel.Text = "Contact" Then
                    DirectCast(e.Item.FindControl("lvtr1"), HtmlTableRow).Style.Add("background", "#eef4fb")
                    DirectCast(e.Item.FindControl("lvtr2"), HtmlTableRow).Style.Add("background", "#eef4fb")
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub
    Function GetProductId(ByVal str As String) As String
        If str.Length > 0 Then
            Dim sql As String = ""
            Dim ds As DataSet = Nothing
            sql = "select p.Id from product p where p.ItemNumber='" & str & "'"
            If sql.Length > 0 Then
                ds = BRIClassLibrary.SQLData.generic_select(sql, appGlobal.CONNECTIONSTRING)
                If Not ds Is Nothing Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        str = ds.Tables(0).Rows(0)("Id")
                    End If
                End If
            End If
        Else
            Return (str.ToString())
        End If
        Return (str.ToString())
    End Function
End Class
