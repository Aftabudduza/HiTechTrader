Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_ThirdPartyReport
    Inherits System.Web.UI.Page
    Public errStr As String = String.Empty
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Public ds As DataSet = New DataSet()
    Dim dt As New DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
    End Sub
    'Private Sub fill_Product()
    '    Try
    '        Dim sSQl As String = "SELECT p.ProductName, p.Id FROM Product p WHERE p.IsLabX = 1 and  p.CreatorID =  " & CInt(Session("Id").ToString()) & " order by p.ProductName asc "
    '        Dim ds As DataSet = BRIClassLibrary.SQLData.generic_select(sSQl, appGlobal.CONNECTIONSTRING)
    '        ddlProduct.Items.Clear()
    '        ddlProduct.AppendDataBoundItems = True
    '        For Each dr As DataRow In ds.Tables(0).Rows
    '            If Not dr("Id").ToString() Is Nothing And dr("Id").ToString() <> String.Empty Then
    '                If CInt(dr("Id").ToString()) > 0 Then
    '                    Me.ddlProduct.Items.Add(New ListItem(dr("UserName"), dr("Id")))
    '                End If
    '            End If
    '        Next
    '    Catch ex As Exception

    '    End Try

    'End Sub
    Private Sub DisplayAlert(ByVal msg As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", msg.Replace("'", "\'").Replace(vbCrLf, "\n")), True)
    End Sub
    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try
            If Not Session("Id") Is Nothing Then
                GenerateFile()
            Else
                Response.Redirect("Login.aspx")
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub GenerateFile()
        Try

            Dim fileName As String = String.Empty

            Dim str As String = "SELECT p.*, c.CategoryName ParentCategoryName   FROM Product p, Category c WHERE p.ParentCategory = c.Id and p.CreatorID = " & CInt(Session("Id").ToString()) & "  AND c.IsMainorLabX = 0 and p.IsLabX = 1 ORDER BY p.ItemNumber asc "


            If str.Length > 0 Then
                dt = BRIClassLibrary.SQLData.generic_select(str, strConnection).Tables(0)
            End If

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim filePath = Path.Combine(Request.PhysicalApplicationPath, "Files\Product\ThirdParty\")
                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If

                fileName = "Product_Export_" & DateTime.Now.ToString("yyyyMMddhhmmss") & ".csv"
                filePath = Path.Combine(filePath, fileName)
                Try
                    If System.IO.File.Exists(filePath) Then
                        System.IO.File.Delete(filePath)
                    End If
                Catch ex As Exception

                End Try

                Dim wr = New StreamWriter(filePath)
                wr.Write("Itemnumber")
                wr.Write(",")
                wr.Write("Title")
                wr.Write(",")
                wr.Write("Make")
                wr.Write(",")
                wr.Write("Model")
                wr.Write(",")
                wr.Write("Description")
                wr.Write(",")
                wr.Write("Condition")
                wr.Write(",")
                wr.Write("Age")
                wr.Write(",")
                wr.Write("Weight")
                wr.Write(",")
                wr.Write("Price")
                wr.Write(",")
                wr.Write("Qty")
                wr.Write(",")
                wr.Write("Qty Sold")
                wr.Write(",")
                wr.Write("Lowest Price")
                wr.Write(",")
                wr.Write("Cost of Goods")
                wr.Write(",")
                wr.Write("Auction Start")
                wr.Write(",")
                wr.Write("Selling Price")
                wr.Write(",")
                wr.Write("Category Name")
                wr.Write(",")
                wr.Write("Parent Category")
                wr.Write(",")
                wr.Write("Location")
                wr.Write(",")
                wr.Write("Barcode")
                wr.Write(",")
                wr.Write("Barcode Parent")
                wr.Write(",")
                wr.Write("Hold")
                wr.Write(",")
                wr.Write("Sold")
                wr.Write(",")
                wr.Write("Specials")
                wr.Write(",")
                wr.Write("New")
                wr.Write(",")
                wr.Write("Featured")
                wr.Write(",")
                wr.Write("Offtruck")
                wr.Write(",")
                wr.Write("Consignment")
                wr.Write(",")
                wr.Write("Deleted")
                wr.Write(",")
                wr.Write("LabX Ad")
                wr.Write(",")
                wr.Write("Main Pic")
                wr.Write(",")
                wr.Write("product_image")
                wr.Write(",")
                wr.Write("No Watermark Image")
                wr.Write(wr.NewLine)
                For Each dr As DataRow In dt.Rows
                    wr.Write(dr("ItemNumber").ToString())
                    wr.Write(",")
                    wr.Write(dr("ProductName").ToString())
                    wr.Write(",")
                    wr.Write(dr("Make").ToString())
                    wr.Write(",")
                    wr.Write(dr("Model").ToString())
                    wr.Write(",")
                    Dim sDesc As String = HttpUtility.HtmlDecode(dr("Description").ToString())
                    If sDesc.Length > 0 Then
                        sDesc = sDesc.Replace(",", " ")
                    End If
                    wr.Write(sDesc.Trim)
                    wr.Write(",")
                    wr.Write(dr("Condition").ToString())
                    wr.Write(",")
                    wr.Write(dr("Age").ToString())
                    wr.Write(",")
                    wr.Write(dr("Weight").ToString())
                    wr.Write(",")
                    wr.Write(dr("Price").ToString())
                    wr.Write(",")
                    wr.Write(dr("Qty").ToString())
                    wr.Write(",")
                    wr.Write(dr("QuantitySold").ToString())
                    wr.Write(",")
                    wr.Write(dr("LowestPrice").ToString())
                    wr.Write(",")
                    wr.Write(dr("CostofGoods").ToString())
                    wr.Write(",")
                    wr.Write(dr("AuctionStart").ToString())
                    wr.Write(",")
                    wr.Write(dr("SellingPrice").ToString())
                    wr.Write(",")
                    wr.Write(Create_Category(CInt(dr("Id").ToString())))
                    wr.Write(",")
                    wr.Write(dr("ParentCategoryName").ToString())
                    wr.Write(",")
                    wr.Write(dr("Location").ToString())
                    wr.Write(",")
                    wr.Write(dr("Barcode").ToString())
                    wr.Write(",")
                    wr.Write(dr("BarcodeParent").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsHold").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsSold").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsSpecial").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsNewArrivalsPage").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsFeaturedItem").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsJustOfftheTruck").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsConsignmentItem").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsDeleteItem").ToString())
                    wr.Write(",")
                    wr.Write(dr("IsLabX").ToString())
                    wr.Write(",")
                    wr.Write(dr("ImageFileName").ToString())
                    wr.Write(",")
                    wr.Write(Create_Images(CInt(dr("Id").ToString())))
                    wr.Write(",")
                    wr.Write(dr("Watermarkimg").ToString())
                    wr.Write(wr.NewLine)
                Next
                wr.Close()
                Session("ApplicationFile") = filePath
                Response.Redirect("../Files/Product/ThirdParty/" & fileName)
            Else
                DisplayAlert("You Have Nothing to Export")
            End If
        Catch ex As Exception
            DisplayAlert("Opetation Not Proceed")
        End Try
    End Sub
    Private Function Create_Category(ByVal nProductId As Integer) As String
        Try
            'create the Models
            Dim sSQL As String = " SELECT C.CategoryName  FROM ProductCategoryCrossRef pccr, Category c WHERE pccr.ProductCategoryId = c.Id  and pccr.ProductId = " & nProductId
            Dim ds As DataSet = SQLData.generic_select(sSQL, SQLData.ConnectionString)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim sCategories As String = ""
                    For Each dr As DataRow In ds.Tables(0).Rows
                        Dim sCatName As String = String.Empty
                        If Not dr("CategoryName") Is Nothing Then
                            If Not String.IsNullOrEmpty(dr("CategoryName").ToString().Trim()) Then
                                sCatName = dr("CategoryName").ToString().Trim()
                                If sCatName.Length > 0 Then
                                    sCatName = sCatName.Trim
                                End If
                            End If
                        End If

                        If sCatName.Trim <> "" Then
                            If sCategories.Trim <> "" Then
                                sCategories &= ";"
                            End If
                            sCategories &= sCatName.Trim
                        End If
                    Next
                    Return sCategories
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        Catch ex As Exception

        End Try

        Return ""

    End Function
    Private Function Create_Images(ByVal nProductId As Integer) As String
        Try
            'create the Models
            Dim sSQL As String = " SELECT picr.ImageUrl FROM ProductImageCrossRef picr  WHERE picr.ProductId  =  " & nProductId
            Dim ds As DataSet = SQLData.generic_select(sSQL, SQLData.ConnectionString)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim sImages As String = ""
                    For Each dr As DataRow In ds.Tables(0).Rows
                        Dim sImageName As String = String.Empty
                        If Not dr("ImageUrl") Is Nothing Then
                            If Not String.IsNullOrEmpty(dr("ImageUrl").ToString().Trim()) Then
                                sImageName = dr("ImageUrl").ToString().Trim()
                                If sImageName.Length > 0 Then
                                    sImageName = sImageName.Trim
                                End If
                            End If
                        End If

                        If sImageName.Trim <> "" Then
                            If sImages.Trim <> "" Then
                                sImages &= ";"
                            End If
                            sImages &= sImageName.Trim
                        End If
                    Next
                    Return sImages
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        Catch ex As Exception

        End Try

        Return ""

    End Function
    
End Class
