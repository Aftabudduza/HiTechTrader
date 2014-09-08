Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_InquiryEmailDetails
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Dim Inquiry As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("Id") Is Nothing Then
            ' nUserID = CInt(Session("Id").ToString())
            If Not Page.IsPostBack Then
                Session("InquiryId") = Nothing
                
                Try
                    Inquiry = CInt(Request.QueryString("InquiryId").ToString())
                Catch ex As Exception
                    Inquiry = 0
                End Try
                If Inquiry > 0 Then
                    Session("InqId") = Inquiry
                End If
                If Not (Session("InqId")) Is Nothing Then
                    fill_Controls(CInt(Session("InqId").ToString().Trim))
                End If
            End If
        Else
            Response.Redirect("Login.aspx")
        End If
    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Private Sub fill_Controls(ByVal nInqID As Integer)
        If nInqID > 0 Then
            Try
                Dim sql As String = ""
                Dim ds As DataSet = Nothing
                Dim comments As String = ""
                sql = "SELECT * FROM OrderHistory oh WHERE oh.Id = " & CInt(nInqID.ToString())
                If sql.Length > 0 Then
                    ds = BRIClassLibrary.SQLData.generic_select(sql, appGlobal.CONNECTIONSTRING)
                    If Not ds Is Nothing Then
                        If ds.Tables(0).Rows.Count > 0 Then

                            If Not ds.Tables(0).Rows(0)("Type") Is Nothing AndAlso ds.Tables(0).Rows(0)("Type") <> "" Then
                                InquiryTYpe.InnerHtml = "Type :" & " " & ds.Tables(0).Rows(0)("Type").ToString()
                            Else
                                InquiryTYpe.InnerHtml = ""
                            End If

                            Try
                                If CDate(ds.Tables(0).Rows(0)("Odate")).ToString("MM/dd/yyyy") = "12/31/1977" Then
                                    InqDate.InnerHtml = "Date :" & " " & DateTime.UtcNow.ToString("MMM dd,yyyy")
                                Else
                                    InqDate.InnerHtml = "Date :" & " " & CDate(ds.Tables(0).Rows(0)("Odate")).ToString("MMM dd,yyyy")
                                End If
                            Catch ex As Exception
                                InqDate.InnerHtml = "Date :" & " " & DateTime.UtcNow.ToString("MMM dd,yyyy")
                            End Try


                            If Not ds.Tables(0).Rows(0)("Name") Is Nothing AndAlso ds.Tables(0).Rows(0)("Name") <> "" Then
                                lblName.Text = ds.Tables(0).Rows(0)("Name").ToString()
                            Else
                                lblName.Text = ""
                            End If

                            If Not ds.Tables(0).Rows(0)("Email").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("Email").ToString() <> "" Then
                                lblEmail.Text = ds.Tables(0).Rows(0)("Email").ToString()
                            Else
                                lblEmail.Text = ""
                            End If

                            If Not ds.Tables(0).Rows(0)("Company").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("Company").ToString() <> "" Then
                                lblCompany.Text = ds.Tables(0).Rows(0)("Company").ToString()
                            Else
                                lblCompany.Text = ""
                            End If
                            If Not ds.Tables(0).Rows(0)("Phone").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("Phone").ToString() <> "" Then
                                lblPhone.Text = ds.Tables(0).Rows(0)("Phone").ToString()
                            Else
                                lblPhone.Text = ""
                            End If
                            If Not ds.Tables(0).Rows(0)("Fax").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("Fax").ToString() <> "" Then
                                lblFax.Text = ds.Tables(0).Rows(0)("Fax").ToString()
                            Else
                                lblFax.Text = "<span style='color:#aaaaaa'>None Listed</span>"
                            End If
                            If Not ds.Tables(0).Rows(0)("Address").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("Address").ToString() <> "" Then
                                lblAddress1.Text = ds.Tables(0).Rows(0)("Address").ToString()
                            Else
                                lblAddress1.Text = ""
                            End If
                            If Not ds.Tables(0).Rows(0)("Address1").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("Address1").ToString() <> "" Then
                                lblAddress2.Text = ds.Tables(0).Rows(0)("Address1").ToString()
                            Else
                                lblAddress2.Text = "<span style='color:#aaaaaa'>None Listed</span>"
                            End If
                            If Not ds.Tables(0).Rows(0)("City").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("City").ToString() <> "" Then
                                lblCity.Text = ds.Tables(0).Rows(0)("City").ToString()
                            Else
                                lblCity.Text = ""
                            End If
                            If Not ds.Tables(0).Rows(0)("State").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("State").ToString() <> "" Then
                                'lblState.Text = ds.Tables(0).Rows(0)("State").ToString()
                                GetSatetName(ds.Tables(0).Rows(0)("State").ToString())
                            Else
                                lblState.Text = ""
                            End If
                            If Not ds.Tables(0).Rows(0)("Country").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("Country").ToString() <> "" Then
                                lblCountry.Text = ds.Tables(0).Rows(0)("Country").ToString()
                            Else
                                lblCountry.Text = ""
                            End If
                            If Not ds.Tables(0).Rows(0)("Zip").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("Zip").ToString() <> "" Then
                                lblZip.Text = ds.Tables(0).Rows(0)("Zip").ToString()
                            Else
                                lblZip.Text = ""
                            End If
                            If Not ds.Tables(0).Rows(0)("PONumber").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("PONumber").ToString() <> "" Then
                                GetProductId(ds.Tables(0).Rows(0)("PONumber").ToString())
                                'lblCompany.Text = ds.Tables(0).Rows(0)("PONumber").ToString()
                            Else
                                lblItemNo.Text = ""
                            End If
                            If Not ds.Tables(0).Rows(0)("Message").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("Message").ToString() <> "" Then
                                comments &= "<div style='float:Left; margin-bottom:10px;width:100%;'>" & ds.Tables(0).Rows(0)("Message").ToString() & "</div>"

                            End If
                            If Not ds.Tables(0).Rows(0)("ItemToPurchase").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("ItemToPurchase").ToString() <> "" Then
                                comments &= "<div style='float:Left;width:100%;'>Items to Purchase: " & ds.Tables(0).Rows(0)("ItemToPurchase").ToString() & "</div>"
                            Else
                                comments &= ""
                            End If
                            If Not ds.Tables(0).Rows(0)("IsNeedFuture").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("IsNeedFuture").ToString() <> "" Then
                                If ds.Tables(0).Rows(0)("IsNeedFuture").ToString() > 0 Then
                                    comments &= "<div style='float:Left;width:100%;'>We need these in the future.</div>"
                                Else
                                    comments &= ""
                                End If

                            Else
                                comments &= ""
                            End If

                            If Not ds.Tables(0).Rows(0)("IsNeedASAP").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("IsNeedASAP").ToString() <> "" Then
                                If ds.Tables(0).Rows(0)("IsNeedASAP").ToString() > 0 Then
                                    comments &= "<div style='float:Left;width:100%;'> We need these ASAP!</div>"
                                Else
                                    comments &= ""
                                End If

                            Else
                                comments &= ""
                            End If

                            If Not ds.Tables(0).Rows(0)("OptionPrice").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("OptionPrice").ToString() <> "" Then
                                comments &= "<div style='float:Left;width:100%;'>Price Option :" & ds.Tables(0).Rows(0)("OptionPrice").ToString() & "</div>"
                            Else
                                comments &= ""
                            End If
                            If Not ds.Tables(0).Rows(0)("ItemsToSale").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("ItemsToSale").ToString() <> "" Then
                                comments &= "<div style='float:Left;width:100%;'>Items For Sale: :" & ds.Tables(0).Rows(0)("ItemsToSale").ToString() & "</div>"
                            Else
                                comments &= ""
                            End If
                            lblComments.Text = comments.ToString()
                            If Not ds.Tables(0).Rows(0)("Country").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("Country").ToString() <> "" Then
                                lblCountry.Text = ds.Tables(0).Rows(0)("Country").ToString()
                            Else
                                lblCountry.Text = ""
                            End If
                        End If
                    End If
                End If

            Catch ex As Exception
            End Try
        End If
    End Sub
    Public Sub GetProductId(ByVal nid As String)
        If nid.Length > 0 Then
            Dim sql As String = ""
            Dim ds As DataSet = Nothing
            sql = "select p.Id from product p where p.ItemNumber='" & nid & "'"
            If sql.Length > 0 Then
                ds = BRIClassLibrary.SQLData.generic_select(sql, appGlobal.CONNECTIONSTRING)
                If Not ds Is Nothing Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        lblItemNo.Text = "<a href='AddNewItem.aspx?PID=" & CInt(ds.Tables(0).Rows(0)("Id").ToString()) & "'>" & nid.ToString() & "</a>"
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub GetSatetName(ByVal sname As String)
        If sname.Length > 0 Then
            Dim sql As String = ""
            Dim ds As DataSet = Nothing
            sql = "SELECT r.STATENAME FROM REFSTATES r WHERE r.[STATE]='" & sname & "'"
            If sql.Length > 0 Then
                ds = BRIClassLibrary.SQLData.generic_select(sql, appGlobal.CONNECTIONSTRING)
                If Not ds Is Nothing Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        lblState.Text = ds.Tables(0).Rows(0)("STATENAME")
                    Else
                        lblState.Text = ""
                    End If
                End If
            End If
        End If
    End Sub
End Class
