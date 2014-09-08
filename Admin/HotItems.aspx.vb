Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Imports System.Data.SqlClient
Partial Class Admin_HotItems
    Inherits System.Web.UI.Page
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Public str As String = String.Empty
    Public ds As DataSet = Nothing
    Public nRecent As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            fill_HomeHit()
            fill_Items()
        End If
    End Sub
    Public Sub fill_HomeHit()
        Try
            str = " SELECT Isnull(COUNT(p.Id),0) Total FROM ProductHit P WHERE p.IsHome = 1 "
            ds = SQLData.generic_select(str, strConnection)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    lblHomeHitcount.InnerHtml = ds.Tables(0).Rows(0)(0).ToString()
                End If
            End If
        Catch ex As Exception

        End Try
       
    End Sub
    Public Sub fill_Items()
        Try
            str = " SELECT TOP 50 p.Id, p.Make, p.Model, p.[Description], p.DateCreated, p.Price, p.ItemNumber, isnull(p.IsFeaturedItem,0) IsFeaturedItem, A.Total   FROM (SELECT DISTINCT ph.ProductId, ISNULL(count(ph.Id),0) Total FROM ProductHit ph WHERE ph.IsProduct = 1 GROUP BY ph.ProductID ) A, Product p WHERE A.ProductId = p.Id AND A.Total > 0 ORDER BY A.Total DESC, p.Make ASC "
            ds = SQLData.generic_select(str, strConnection)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    grdItems.DataSource = ds.Tables(0)
                    grdItems.DataBind()
                End If
            End If
        Catch ex As Exception

        End Try
       
    End Sub

    Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click
        Try
            Dim sIns As String = " Delete from  ProductHit where IsHome = 1 "
            SQLData.generic_command(sIns, SQLData.ConnectionString)
        Catch ex As Exception

        End Try
        fill_HomeHit()
        fill_Items()
    End Sub

    Protected Sub btnProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProduct.Click
        Try
            Dim sIns As String = " Delete from  ProductHit where IsProduct = 1 "
            SQLData.generic_command(sIns, SQLData.ConnectionString)
        Catch ex As Exception

        End Try
        fill_HomeHit()
        fill_Items()
    End Sub
End Class
