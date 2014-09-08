Imports System.Data
Imports BRIClassLibrary
Imports System.IO
Imports System.Globalization
Partial Class Admin_ProductCopy
    Inherits System.Web.UI.Page
    Dim sSQl As String = ""
    Dim nProductId As Integer = 0
    Dim nInsert As Integer = 0
    Dim nImage As Integer = 0
    Public strConnection As String = appGlobal.CONNECTIONSTRING()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                nProductId = CInt(Request.QueryString("CopyID").ToString())
            Catch ex As Exception
                nProductId = 0
            End Try
            Try
                nImage = CInt(Request.QueryString("CopyImage").ToString())
            Catch ex As Exception
                nImage = 0
            End Try
            If nProductId > 0 Then
                nInsert = GetProductCopy(nProductId, nImage)
                'spanProduct.InnerHtml = "<a href='ProductDetails.aspx?Id=" & nInsert & "'>Edit this item</a>"
                spanProduct.InnerHtml = "<a href='../Admin/AddNewItem.aspx?PID=" & nInsert & "'>Edit this item</a>"
            End If
        End If
    End Sub
    Public Function GetItemNo() As String
        Dim sItemNumber As String = ""
        Try
            Dim itemno As String = ""
            Dim getItem As String = ""
            Dim FinalItemNo As String = ""
            Dim dtime As DateTime = DateTime.UtcNow
            itemno = dtime.ToString("yyyy-MM-dd")
            Dim Str As String = ""
            Dim ds As DataSet = Nothing
            Str = "SELECT p.ItemNumber FROM Product p WHERE CAST(CONVERT(varchar(8), p.DateCreated, 112) AS DATETIME)  = '" & itemno & "' AND id IN(SELECT max(id) FROM Product p2)"

            If Str.Length > 0 Then
                ds = BRIClassLibrary.SQLData.generic_select(Str, appGlobal.CONNECTIONSTRING)
                If Not ds Is Nothing Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        getItem = ds.Tables(0).Rows(0)("ItemNumber").ToString().Trim
                        FinalItemNo = getItem.ToString().Substring(0, getItem.Length - 6)
                        If FinalItemNo.Contains("T") Then
                            If Session("IsThirdParty") = 3 Then
                                FinalItemNo = FinalItemNo.Remove(0, 1)
                                Dim NewItem As Integer = CInt(FinalItemNo) + 1
                                sItemNumber = "T" & "" & NewItem.ToString("000") & "" & dtime.ToString("yyMMdd")
                            Else
                                FinalItemNo = FinalItemNo.Remove(0, 1)
                                Dim NewItem As Integer = CInt(FinalItemNo) + 1
                                sItemNumber = NewItem.ToString("000") & "" & dtime.ToString("yyMMdd")
                            End If
                        Else
                            sItemNumber = (FinalItemNo + 1).ToString("000") & "" & dtime.ToString("yyMMdd")
                        End If

                    Else
                        If Session("IsThirdParty") = 3 Then
                            sItemNumber = "T" & "001" & dtime.ToString("yyMMdd")
                        Else
                            sItemNumber = "001" & dtime.ToString("yyMMdd")
                        End If
                    End If
                End If
            End If
        Catch ex As Exception

        End Try

        Return sItemNumber

    End Function
    Private Function GetProductCopy(ByVal nProductId As Integer, ByVal nImageId As Integer) As Integer
        Dim sId As String = "0"
        Dim nId As Integer = 0
        Dim ObjProduct As New Product(appGlobal.CONNECTIONSTRING)
        Dim sItemNumber As String = GetItemNo()
        If nProductId > 0 Then
            Try
                Dim sql As String = ""
                Dim ds As DataSet = Nothing
                sql = "SELECT * FROM Product p WHERE p.Id = " & nProductId
                If sql.Length > 0 Then
                    ds = BRIClassLibrary.SQLData.generic_select(sql, appGlobal.CONNECTIONSTRING)
                    If Not ds Is Nothing Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If sItemNumber.Length > 0 Then
                                ObjProduct.ItemNumber = sItemNumber.Trim
                            Else
                                If Not ds.Tables(0).Rows(0)("ItemNumber").ToString() Is Nothing AndAlso ds.Tables(0).Rows(0)("ItemNumber").ToString() <> "" Then
                                    ObjProduct.ItemNumber = ds.Tables(0).Rows(0)("ItemNumber").ToString()
                                End If
                            End If
                            If Not ds.Tables(0).Rows(0)("ProductName") Is Nothing AndAlso ds.Tables(0).Rows(0)("ProductName").ToString() <> "" Then
                                ObjProduct.ProductName = ds.Tables(0).Rows(0)("ProductName").ToString()
                            Else
                                ObjProduct.ProductName = ""
                            End If

                            If Not ds.Tables(0).Rows(0)("ManufacturerId") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("ManufacturerId").ToString()) > 0 Then
                                ObjProduct.ManufacturerId = CInt(ds.Tables(0).Rows(0)("ManufacturerId").ToString())
                            Else
                                ObjProduct.ManufacturerId = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("Make") Is Nothing AndAlso ds.Tables(0).Rows(0)("Make") <> "" Then
                                ObjProduct.Make = ds.Tables(0).Rows(0)("Make").ToString()
                            Else
                                ObjProduct.Make = ""
                            End If
                            If Not ds.Tables(0).Rows(0)("Model") Is Nothing AndAlso ds.Tables(0).Rows(0)("Model") <> "" Then
                                ObjProduct.Model = ds.Tables(0).Rows(0)("Model").ToString()
                            Else
                                ObjProduct.Model = ""
                            End If

                            If Not ds.Tables(0).Rows(0)("Description") Is Nothing AndAlso ds.Tables(0).Rows(0)("Description").ToString() <> "" Then
                                ObjProduct.Description = ds.Tables(0).Rows(0)("Description").ToString()
                            Else
                                ObjProduct.Description = ""
                            End If
                            If Not ds.Tables(0).Rows(0)("Condition") Is Nothing AndAlso ds.Tables(0).Rows(0)("Condition").ToString() <> "" Then
                                ObjProduct.Condition = ds.Tables(0).Rows(0)("Condition").ToString()
                            Else
                                ObjProduct.Condition = ""
                            End If

                            If Not ds.Tables(0).Rows(0)("Age") Is Nothing AndAlso ds.Tables(0).Rows(0)("Age").ToString() <> "" Then
                                ObjProduct.Age = ds.Tables(0).Rows(0)("Age").ToString()
                            Else
                                ObjProduct.Age = ""
                            End If
                            If Not ds.Tables(0).Rows(0)("Weight") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("Weight")) > 0 Then
                                ObjProduct.Weight = CDbl(ds.Tables(0).Rows(0)("Weight").ToString())
                            Else
                                ObjProduct.Weight = 0
                            End If


                            If Not ds.Tables(0).Rows(0)("Price") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("Price")) > 0 Then
                                ObjProduct.Price = CDbl(ds.Tables(0).Rows(0)("Price").ToString()).ToString("0.00")
                            Else
                                ObjProduct.Price = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("Qty") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("Qty")) > 0 Then
                                ObjProduct.Qty = CInt(ds.Tables(0).Rows(0)("Qty").ToString())
                            Else
                                ObjProduct.Qty = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("QuantitySold") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("QuantitySold")) > 0 Then
                                ObjProduct.QuantitySold = CInt(ds.Tables(0).Rows(0)("QuantitySold").ToString())
                            Else
                                ObjProduct.QuantitySold = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("LowestPrice") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("LowestPrice")) > 0 Then
                                ObjProduct.LowestPrice = 0 = CDbl(ds.Tables(0).Rows(0)("LowestPrice").ToString()).ToString("0.00")
                            Else
                                ObjProduct.LowestPrice = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("CostofGoods") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("CostofGoods")) > 0 Then
                                ObjProduct.CostofGoods = CDbl(ds.Tables(0).Rows(0)("CostofGoods").ToString()).ToString("0.00")
                            Else
                                ObjProduct.CostofGoods = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("AuctionStart") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("AuctionStart")) > 0 Then
                                ObjProduct.AuctionStart = CDbl(ds.Tables(0).Rows(0)("AuctionStart").ToString()).ToString("0.00")
                            Else
                                ObjProduct.AuctionStart = 0
                            End If

                            If Not ds.Tables(0).Rows(0)("SellingPrice") Is Nothing AndAlso CDbl(ds.Tables(0).Rows(0)("SellingPrice")) > 0 Then
                                ObjProduct.SellingPrice = CDbl(ds.Tables(0).Rows(0)("SellingPrice").ToString()).ToString("0.00")
                            Else
                                ObjProduct.SellingPrice = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("Location") Is Nothing AndAlso ds.Tables(0).Rows(0)("Location").ToString() <> "" Then
                                ObjProduct.Location = ds.Tables(0).Rows(0)("Location").ToString()
                            Else
                                ObjProduct.Location = ""
                            End If

                            If Not ds.Tables(0).Rows(0)("AdminNotes") Is Nothing AndAlso ds.Tables(0).Rows(0)("AdminNotes").ToString() <> "" Then
                                ObjProduct.AdminNotes = ds.Tables(0).Rows(0)("AdminNotes").ToString()
                            Else
                                ObjProduct.AdminNotes = ""
                            End If

                            If Not ds.Tables(0).Rows(0)("Barcode") Is Nothing AndAlso ds.Tables(0).Rows(0)("Barcode").ToString() <> "" Then
                                ObjProduct.Barcode = ds.Tables(0).Rows(0)("Barcode").ToString()
                            Else
                                ObjProduct.Barcode = ""
                            End If
                            If Not ds.Tables(0).Rows(0)("BarcodeParent") Is Nothing AndAlso ds.Tables(0).Rows(0)("BarcodeParent").ToString() <> "" Then
                                ObjProduct.BarcodeParent = ds.Tables(0).Rows(0)("BarcodeParent").ToString()
                            Else
                                ObjProduct.BarcodeParent = ""
                            End If

                            If Not ds.Tables(0).Rows(0)("TotalPieces") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("TotalPieces")) > 0 Then
                                ObjProduct.TotalPieces = CDbl(ds.Tables(0).Rows(0)("TotalPieces").ToString())
                            Else
                                ObjProduct.TotalPieces = 0
                            End If

                            If Not ds.Tables(0).Rows(0)("ManualItemNo") Is Nothing AndAlso ds.Tables(0).Rows(0)("ManualItemNo").ToString() <> "" Then
                                ObjProduct.ManualItemNo = ds.Tables(0).Rows(0)("ManualItemNo").ToString()
                            Else
                                ObjProduct.ManualItemNo = ""
                            End If

                            If Not ds.Tables(0).Rows(0)("Category") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("Category")) > 0 Then
                                ObjProduct.Category = CInt(ds.Tables(0).Rows(0)("Category").ToString())
                            Else
                                ObjProduct.Category = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("ParentCategory") Is Nothing AndAlso CInt(ds.Tables(0).Rows(0)("ParentCategory")) > 0 Then
                                ObjProduct.ParentCategory = CInt(ds.Tables(0).Rows(0)("ParentCategory").ToString())
                            Else
                                ObjProduct.ParentCategory = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsNotOnWeb") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsNotOnWeb").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsNotOnWeb")) > 0 Then
                                    ObjProduct.IsNotOnWeb = 1
                                Else
                                    ObjProduct.IsNotOnWeb = 0
                                End If
                            Else
                                ObjProduct.IsNotOnWeb = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsDoNotRelease") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsDoNotRelease").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsDoNotRelease")) > 0 Then
                                    ObjProduct.IsDoNotRelease = 1
                                Else
                                    ObjProduct.IsDoNotRelease = 0
                                End If
                            Else
                                ObjProduct.IsDoNotRelease = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsFeaturedItem") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsFeaturedItem").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsFeaturedItem")) > 0 Then
                                    ObjProduct.IsFeaturedItem = 1
                                Else
                                    ObjProduct.IsFeaturedItem = 0
                                End If
                            Else
                                ObjProduct.IsFeaturedItem = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsNewArrivalsPage") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsNewArrivalsPage").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsNewArrivalsPage")) > 0 Then
                                    ObjProduct.IsNewArrivalsPage = 1
                                Else
                                    ObjProduct.IsNewArrivalsPage = 0
                                End If
                            Else
                                ObjProduct.IsNewArrivalsPage = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsConsignmentItem") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsConsignmentItem").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsConsignmentItem")) > 0 Then
                                    ObjProduct.IsConsignmentItem = 1
                                Else
                                    ObjProduct.IsConsignmentItem = 0
                                End If
                            Else
                                ObjProduct.IsConsignmentItem = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsJustOfftheTruck") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsJustOfftheTruck").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsJustOfftheTruck")) > 0 Then
                                    ObjProduct.IsJustOfftheTruck = 1
                                Else
                                    ObjProduct.IsJustOfftheTruck = 0
                                End If
                            Else
                                ObjProduct.IsJustOfftheTruck = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsSold") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsSold").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsSold")) > 0 Then
                                    ObjProduct.IsSold = 1
                                Else
                                    ObjProduct.IsSold = 0
                                End If
                            Else
                                ObjProduct.IsSold = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsHold") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsHold").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsHold")) > 0 Then
                                    ObjProduct.IsHold = 1
                                Else
                                    ObjProduct.IsHold = 0
                                End If
                            Else
                                ObjProduct.IsHold = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsSpecial") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsSpecial").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsSpecial")) > 0 Then
                                    ObjProduct.IsSpecial = 1
                                Else
                                    ObjProduct.IsSpecial = 0
                                End If
                            Else
                                ObjProduct.IsSpecial = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsDeleteItem") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsDeleteItem").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsDeleteItem")) > 0 Then
                                    ObjProduct.IsDeleteItem = 1
                                Else
                                    ObjProduct.IsDeleteItem = 0
                                End If
                            Else
                                ObjProduct.IsDeleteItem = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsLabX") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsLabX").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsLabX")) > 0 Then
                                    ObjProduct.IsLabX = 1
                                Else
                                    ObjProduct.IsLabX = 0
                                End If
                            Else
                                ObjProduct.IsLabX = 0
                            End If
                           
                            If Not ds.Tables(0).Rows(0)("IsIncludeinNewsletter") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsIncludeinNewsletter").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsIncludeinNewsletter")) > 0 Then
                                    ObjProduct.IsIncludeinNewsletter = 1
                                Else
                                    ObjProduct.IsIncludeinNewsletter = 0
                                End If
                            Else
                                ObjProduct.IsIncludeinNewsletter = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsDeletePermanently") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsDeletePermanently").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsDeletePermanently")) > 0 Then
                                    ObjProduct.IsDeletePermanently = 1
                                Else
                                    ObjProduct.IsDeletePermanently = 0
                                End If
                            Else
                                ObjProduct.IsDeletePermanently = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsPaid") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsPaid").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsPaid")) > 0 Then
                                    ObjProduct.IsPaid = 1
                                Else
                                    ObjProduct.IsPaid = 0
                                End If
                            Else
                                ObjProduct.IsPaid = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsShipped") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsShipped").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsShipped")) > 0 Then
                                    ObjProduct.IsShipped = 1
                                Else
                                    ObjProduct.IsShipped = 0
                                End If
                            Else
                                ObjProduct.IsShipped = 0
                            End If
                            If Not ds.Tables(0).Rows(0)("IsCompleted") Is Nothing AndAlso ds.Tables(0).Rows(0)("IsCompleted").ToString() <> "" Then
                                If CInt(ds.Tables(0).Rows(0)("IsCompleted")) > 0 Then
                                    ObjProduct.IsCompleted = 1
                                Else
                                    ObjProduct.IsCompleted = 0
                                End If
                            Else
                                ObjProduct.IsCompleted = 0
                            End If


                            If nImage = 1 Then
                                If Not ds.Tables(0).Rows(0)("ImageFileName") Is Nothing AndAlso ds.Tables(0).Rows(0)("ImageFileName").ToString() <> "" Then
                                    ObjProduct.ImageFileName = ds.Tables(0).Rows(0)("ImageFileName").ToString()
                                Else
                                    ObjProduct.ImageFileName = ""
                                End If
                                If Not ds.Tables(0).Rows(0)("ThumbFileName") Is Nothing AndAlso ds.Tables(0).Rows(0)("ThumbFileName").ToString() <> "" Then
                                    ObjProduct.ThumbFileName = ds.Tables(0).Rows(0)("ThumbFileName").ToString()
                                Else
                                    ObjProduct.ThumbFileName = ""
                                End If
                            Else
                                ObjProduct.ImageFileName = ""
                                ObjProduct.ThumbFileName = ""
                            End If

                           
                            If Not ds.Tables(0).Rows(0)("VideoURL") Is Nothing AndAlso ds.Tables(0).Rows(0)("VideoURL").ToString() <> "" Then
                                ObjProduct.VideoURL = ds.Tables(0).Rows(0)("VideoURL").ToString()
                            Else
                                ObjProduct.VideoURL = ""
                            End If

                            If Not ds.Tables(0).Rows(0)("Watermarkimg") Is Nothing AndAlso ds.Tables(0).Rows(0)("Watermarkimg").ToString() <> "" Then
                                ObjProduct.Watermarkimg = ds.Tables(0).Rows(0)("Watermarkimg").ToString()
                            Else
                                ObjProduct.Watermarkimg = ""
                            End If

                            ObjProduct.SeoText = ""
                            If Not Session("ID") Is Nothing Then
                                ObjProduct.CreatorID = CInt(Session("ID").ToString())
                            Else
                                ObjProduct.CreatorID = 0
                            End If
                            ObjProduct.DateCreated = DateTime.UtcNow
                        End If
                    End If


                    nId = ObjProduct.InsertIntoProduct()

                    If nId > 0 Then
                        Try
                            Dim sqlCat As String = ""
                            Dim dsCat As DataSet = Nothing
                            sqlCat = "SELECT isnull(ProductCategoryId,0) ProductCategoryId FROM ProductCategoryCrossRef pccr WHERE pccr.ProductId = " & nProductId
                            If sqlCat.Length > 0 Then
                                dsCat = BRIClassLibrary.SQLData.generic_select(sqlCat, appGlobal.CONNECTIONSTRING)
                                If Not dsCat Is Nothing Then
                                    If dsCat.Tables(0).Rows.Count > 0 Then
                                        For Each dr As DataRow In dsCat.Tables(0).Rows
                                            If Not dr(0) Is Nothing And CInt(dr(0).ToString()) > 0 Then
                                                Dim sIns As String = "INSERT INTO ProductCategoryCrossRef ([ProductId] ,[ProductCategoryId]) values (" & nId & " , " & CInt(dr(0).ToString()) & ")"
                                                SQLData.generic_command(sIns, SQLData.ConnectionString)
                                            End If
                                        Next
                                    End If
                                End If
                            End If

                        Catch ex As Exception

                        End Try
                    End If

                    If nId > 0 Then
                        Try
                            Dim sqlFile As String = ""
                            Dim dsFile As DataSet = Nothing
                            sqlFile = "SELECT isnull(ImageUrl,'') ImageUrl FROM ProductImageCrossRef pccr WHERE pccr.ProductId = " & nProductId
                            If sqlFile.Length > 0 Then
                                dsFile = BRIClassLibrary.SQLData.generic_select(sqlFile, appGlobal.CONNECTIONSTRING)
                                If Not dsFile Is Nothing Then
                                    If dsFile.Tables(0).Rows.Count > 0 Then
                                        For Each dr As DataRow In dsFile.Tables(0).Rows
                                            If Not dr(0) Is Nothing And dr(0).ToString() <> String.Empty Then
                                                Dim sIns As String = "INSERT INTO ProductImageCrossRef ([ProductId] ,[ImageUrl]) values (" & nId & " , " & CInt(dr(0).ToString()) & ")"
                                                SQLData.generic_command(sIns, SQLData.ConnectionString)
                                            End If
                                        Next
                                    End If
                                End If
                            End If

                        Catch ex As Exception

                        End Try
                    End If


                    'If nId > 0 Then
                    '    Dim sIns As String = "INSERT INTO ProductCategoryCrossRef ([ProductId] ,[ProductCategoryId]) (SELECT ProductId ,ProductCategoryId FROM ProductCategoryCrossRef pccr WHERE pccr.ProductId = " & nProductId & ")"
                    '    SQLData.generic_command(sIns, SQLData.ConnectionString)
                    'End If
                    'If nId > 0 Then
                    '    Dim sIns As String = "INSERT INTO ProductImageCrossRef ([ProductId] ,[ImageUrl]) (SELECT ProductId ,ImageUrl FROM ProductImageCrossRef pccr WHERE pccr.ProductId = " & nProductId & ")"
                    '    SQLData.generic_command(sIns, SQLData.ConnectionString)
                    'End If

                End If
            Catch ex As Exception
            End Try

        End If

        Return nId
    End Function
End Class
