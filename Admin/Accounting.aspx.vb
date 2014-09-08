Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Imports System.Data.SqlClient
Partial Class Admin_Accounting
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Private objSmtpClient As Net.Mail.SmtpClient
    Private objMailMessage As Net.Mail.MailMessage
    Private strMailPort As String
    Public str As String = String.Empty
    Public ds As DataSet = Nothing
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Private reEmail As New Regex("^(?:[0-9A-Z_-]+(?:\.[0-9A-Z_-]+)*@[0-9A-Z-]+(?:\.[0-9A-Z-]+)*(?:\.[A-Z]{2,4}))$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared rePassword As New Regex("^[A-Za-z0-9]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Private Shared reUsername As New Regex("^[A-Za-z0-9_.@]*$", RegexOptions.Compiled Or RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("Id") Is Nothing Then
            nUserID = CInt(Session("Id").ToString())
            If Not Page.IsPostBack Then
                fill_ItemValue()
                fill_ItemValueOnHold()
                fill_SoldItemValue()
            End If
        Else
            Response.Redirect("Login.aspx")
        End If
    End Sub
    Public Sub fill_ItemValue()
        Try
            str = " SELECT ISNULL(sum(p.Qty-p.QuantitySold),0) TotalItem, ISNULL(sum((p.Qty-p.QuantitySold)*p.Price),0) TotalValue, ISNULL(SUM((p.Qty-p.QuantitySold)*p.LowestPrice),0) TotalLowestValue, ISNULL(SUM((p.Qty-p.QuantitySold)*p.CostofGoods),0) TotalCostOfGoods FROM Product p WHERE (p.IsNotOnWeb = 0 OR p.IsNotOnWeb IS NULL OR p.IsDoNotRelease IS null OR p.IsDoNotRelease = 0) AND p.IsLabX = 0 AND p.IsDeleteItem <> 1 "
            ds = SQLData.generic_select(str, strConnection)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If Not ds.Tables(0).Rows(0)(0).ToString() Is Nothing And CInt(ds.Tables(0).Rows(0)(0).ToString()) > 0 Then
                        lblTotalWebItem.InnerHtml = ds.Tables(0).Rows(0)(0).ToString()
                    Else
                        lblTotalWebItem.InnerHtml = "$0.00"
                    End If
                    If Not ds.Tables(0).Rows(0)(1).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(1).ToString()) > 0 Then
                        lblTotalWebValue.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(1).ToString()).ToString("#,##0.00")
                    Else
                        lblTotalWebValue.InnerHtml = "$0.00"
                    End If
                    If Not ds.Tables(0).Rows(0)(2).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(2).ToString()) > 0 Then
                        lblTotalWebLowestValue.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(2).ToString()).ToString("#,##0.00")
                    Else
                        lblTotalWebLowestValue.InnerHtml = "$0.00"
                    End If
                    If Not ds.Tables(0).Rows(0)(3).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(3).ToString()) > 0 Then
                        lblTotalWebCostOfGood.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(3).ToString()).ToString("#,##0.00")
                    Else
                        lblTotalWebCostOfGood.InnerHtml = "$0.00"
                    End If
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub
    Public Sub fill_ItemValueOnHold()
        Try
            str = " SELECT ISNULL(sum(p.Qty-p.QuantitySold),0) TotalItem, ISNULL(sum((p.Qty-p.QuantitySold)*p.Price),0) TotalValue, ISNULL(SUM((p.Qty-p.QuantitySold)*p.LowestPrice),0) TotalLowestValue, ISNULL(SUM((p.Qty-p.QuantitySold)*p.CostofGoods),0) TotalCostOfGoods FROM Product p WHERE (p.IsLabX = 1  OR p.IsDoNotRelease = 1) AND p.IsDeleteItem <> 1 "
            ds = SQLData.generic_select(str, strConnection)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If Not ds.Tables(0).Rows(0)(0).ToString() Is Nothing And CInt(ds.Tables(0).Rows(0)(0).ToString()) > 0 Then
                        lblTotalHoldItem.InnerHtml = ds.Tables(0).Rows(0)(0).ToString()
                    Else
                        lblTotalHoldItem.InnerHtml = "$0.00"
                    End If
                    If Not ds.Tables(0).Rows(0)(1).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(1).ToString()) > 0 Then
                        lblTotalHoldValue.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(1).ToString()).ToString("#,##0.00")
                    Else
                        lblTotalHoldValue.InnerHtml = "$0.00"
                    End If
                    If Not ds.Tables(0).Rows(0)(2).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(2).ToString()) > 0 Then
                        lblTotalHoldLowestValue.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(2).ToString()).ToString("#,##0.00")
                    Else
                        lblTotalHoldLowestValue.InnerHtml = "$0.00"
                    End If
                    If Not ds.Tables(0).Rows(0)(3).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(3).ToString()) > 0 Then
                        lblTotalHoldCostOfGood.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(3).ToString()).ToString("#,##0.00")
                    Else
                        lblTotalHoldCostOfGood.InnerHtml = "$0.00"
                    End If
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub
    'Public Sub fill_LabXItemValue()
    '    Try
    '        str = " SELECT ISNULL(sum(p.Qty-p.QuantitySold),0) TotalItem, ISNULL(sum((p.Qty-p.QuantitySold)*p.Price),0) TotalValue, ISNULL(SUM((p.Qty-p.QuantitySold)*p.AuctionStart),0) TotalLowestValue, ISNULL(SUM((p.Qty-p.QuantitySold)*p.CostofGoods),0) TotalCostOfGoods FROM Product p WHERE  p.IsLabX = 1 AND p.IsDeleteItem <> 1 "
    '        ds = SQLData.generic_select(str, strConnection)
    '        If Not ds Is Nothing Then
    '            If ds.Tables(0).Rows.Count > 0 Then
    '                If Not ds.Tables(0).Rows(0)(0).ToString() Is Nothing And CInt(ds.Tables(0).Rows(0)(0).ToString()) > 0 Then
    '                    lblLabXItem.InnerHtml = ds.Tables(0).Rows(0)(0).ToString()
    '                Else
    '                    lblLabXItem.InnerHtml = "$0.00"
    '                End If
    '                If Not ds.Tables(0).Rows(0)(1).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(1).ToString()) > 0 Then
    '                    lblLabXValue.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(1).ToString()).ToString("#,##0.00")
    '                Else
    '                    lblLabXValue.InnerHtml = "$0.00"
    '                End If
    '                If Not ds.Tables(0).Rows(0)(2).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(2).ToString()) > 0 Then
    '                    lblLabXAuctionValue.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(2).ToString()).ToString("#,##0.00")
    '                Else
    '                    lblLabXAuctionValue.InnerHtml = "$0.00"
    '                End If
    '                If Not ds.Tables(0).Rows(0)(3).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(3).ToString()) > 0 Then
    '                    lblLabXCostOfGoods.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(3).ToString()).ToString("#,##0.00")
    '                Else
    '                    lblLabXCostOfGoods.InnerHtml = "$0.00"
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception

    '    End Try

    'End Sub
    Public Sub fill_SoldItemValue()
        Try
            str = " SELECT ISNULL(sum(p.QuantitySold),0) TotalItem, ISNULL(sum(p.QuantitySold*p.Price),0) TotalValue, ISNULL(SUM(p.QuantitySold*p.LowestPrice),0) TotalLowestValue, ISNULL(SUM(p.QuantitySold*p.SellingPrice),0) TotalSellingValue, ISNULL(SUM(p.QuantitySold*p.CostofGoods),0) TotalCostOfGoods FROM Product p WHERE  p.IsDoNotRelease <> 1 AND p.IsLabX = 0 AND p.IsDeleteItem <> 1 "
            ds = SQLData.generic_select(str, strConnection)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If Not ds.Tables(0).Rows(0)(0).ToString() Is Nothing And CInt(ds.Tables(0).Rows(0)(0).ToString()) > 0 Then
                        lblSoldItem.InnerHtml = ds.Tables(0).Rows(0)(0).ToString()
                    Else
                        lblSoldItem.InnerHtml = "$0.00"
                    End If
                    If Not ds.Tables(0).Rows(0)(1).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(1).ToString()) > 0 Then
                        lblSoldValue.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(1).ToString()).ToString("#,##0.00")
                    Else
                        lblSoldValue.InnerHtml = "$0.00"
                    End If
                    If Not ds.Tables(0).Rows(0)(2).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(2).ToString()) > 0 Then
                        lblSoldLowestValue.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(2).ToString()).ToString("#,##0.00")
                    Else
                        lblSoldLowestValue.InnerHtml = "$0.00"
                    End If
                    If Not ds.Tables(0).Rows(0)(3).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(3).ToString()) > 0 Then
                        lblSellingPriceValue.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(3).ToString()).ToString("#,##0.00")
                    Else
                        lblSellingPriceValue.InnerHtml = "$0.00"
                    End If
                    If Not ds.Tables(0).Rows(0)(4).ToString() Is Nothing And CDbl(ds.Tables(0).Rows(0)(4).ToString()) > 0 Then
                        lblSoldCostOfGoods.InnerHtml = "$" & CDbl(ds.Tables(0).Rows(0)(4).ToString()).ToString("#,##0.00")
                    Else
                        lblSoldCostOfGoods.InnerHtml = "$0.00"
                    End If
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
End Class
