Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient

Public Class Product
    REM If 'ConnectionString' is set by the constructor or externally, then the connection will be
    REM opened and closed with each operation. If 'conn' is set, then the connection stays open.
    Public Conn As SqlConnection = Nothing
    Public ConnectionString As String = ""
    Public BooleanFalse As String = "N"
    Public BooleanTrue As String = "Y"
    Public LastError As String = ""
    Public Valid As Boolean = False
    Public ColInfo As ArrayList

    REM A variable for each column in the table.
    Public Id As Integer
    Public ItemNumber As String
    Public ProductName As String
    Public ManufacturerId As Integer
    Public Make As String
    Public Model As String
    Public Description As String
    Public AdminNotes As String
    Public Condition As String
    Public Age As String
    Public Weight As Double
    Public Price As Double
    Public Qty As Integer
    Public QuantitySold As Integer
    Public LowestPrice As Double
    Public CostofGoods As Double
    Public AuctionStart As Double
    Public SellingPrice As Double
    Public TotalPieces As Integer
    Public ManualItemNo As String
    Public Category As Integer
    Public ParentCategory As Integer
    Public Location As String
    Public Barcode As String
    Public BarcodeParent As String
    Public IsNotOnWeb As Short
    Public IsNewArrivalsPage As Short
    Public IsFeaturedItem As Short
    Public IsJustOfftheTruck As Short
    Public IsConsignmentItem As Short
    Public IsDoNotRelease As Short
    Public IsHold As Short
    Public IsSold As Short
    Public IsSpecial As Short
    Public IsDeleteItem As Short
    Public IsLabX As Short
    Public IsIncludeinNewsletter As Short
    Public IsDeletePermanently As Short
    Public IsPaid As Short
    Public IsShipped As Short
    Public IsCompleted As Short
    Public ImageFileName As String
    Public ThumbFileName As String
    Public VideoURL As String
    Public SeoText As String
    Public DateCreated As DateTime
    Public CreatorID As Integer
    Public LastEdited As DateTime
    Public EditorID As Integer
    Public Watermarkimg As String
    Public DateSold As DateTime
    Public DateDeleted As DateTime
    Public ClientIP As String
    Public TotalViews As Integer


#Region " User Variables "
    REM Add your variables to this object here. Adding them here ensures
    REM that they will be retained when the object is re-generated.
#End Region

#Region " Column Info "
    REM This class maintains information about each column in the table.
    Public Class ColumnInfo
        Implements IComparable

        Public ColumnName As String
        Public DataType As String
        Public Length As Integer
        Public AllowInsert As Boolean
        Public AllowUpdate As Boolean

        Public Sub New(ByRef _ColumnName As String, ByRef _DataType As String, ByVal _Length As Integer, ByVal _AllowInsert As Boolean, ByVal _AllowUpdate as Boolean)
            ColumnName = _ColumnName
            DataType = _DataType
            Length = _Length
            AllowInsert = _AllowInsert
            AllowUpdate = _AllowUpdate
        End Sub

        Public Overloads Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
            If obj.GetType() Is Me.GetType Then
                Dim ci As ColumnInfo = CType(obj, ColumnInfo)
                Return Me.ColumnName.CompareTo(ci.ColumnName)
            End If
            Dim s As String = CStr(obj)
            Return Me.ColumnName.CompareTo(s)
        End Function
    End Class
#End Region

#Region " Constructors "
    ''' <summary></summary>
    Public Sub New()
        Blank()
        ColInfo = New ArrayList()
        ColInfo.Add(New ColumnInfo("Id", "Integer", 11, False, False))
        ColInfo.Add(New ColumnInfo("ItemNumber", "String", 20, True, True))
        ColInfo.Add(New ColumnInfo("ProductName", "String", 300, True, True))
        ColInfo.Add(New ColumnInfo("ManufacturerId", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("Make", "String", 50, True, True))
        ColInfo.Add(New ColumnInfo("Model", "String", 50, True, True))
        ColInfo.Add(New ColumnInfo("Description", "String", 2147483647, True, True))
        ColInfo.Add(New ColumnInfo("AdminNotes", "String", 400, True, True))
        ColInfo.Add(New ColumnInfo("Condition", "String", 50, True, True))
        ColInfo.Add(New ColumnInfo("Age", "String", 50, True, True))
        ColInfo.Add(New ColumnInfo("Weight", "Double", 15, True, True))
        ColInfo.Add(New ColumnInfo("Price", "Double", 15, True, True))
        ColInfo.Add(New ColumnInfo("Qty", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("QuantitySold", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("LowestPrice", "Double", 15, True, True))
        ColInfo.Add(New ColumnInfo("CostofGoods", "Double", 15, True, True))
        ColInfo.Add(New ColumnInfo("AuctionStart", "Double", 15, True, True))
        ColInfo.Add(New ColumnInfo("SellingPrice", "Double", 15, True, True))
        ColInfo.Add(New ColumnInfo("TotalPieces", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("ManualItemNo", "String", 20, True, True))
        ColInfo.Add(New ColumnInfo("Category", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("ParentCategory", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("Location", "String", 40, True, True))
        ColInfo.Add(New ColumnInfo("Barcode", "String", 50, True, True))
        ColInfo.Add(New ColumnInfo("BarcodeParent", "String", 50, True, True))
        ColInfo.Add(New ColumnInfo("IsNotOnWeb", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsNewArrivalsPage", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsFeaturedItem", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsJustOfftheTruck", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsConsignmentItem", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsDoNotRelease", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsHold", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsSold", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsSpecial", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsDeleteItem", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsLabX", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsIncludeinNewsletter", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsDeletePermanently", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsPaid", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsShipped", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsCompleted", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("ImageFileName", "String", 200, True, True))
        ColInfo.Add(New ColumnInfo("ThumbFileName", "String", 200, True, True))
        ColInfo.Add(New ColumnInfo("VideoURL", "String", 200, True, True))
        ColInfo.Add(New ColumnInfo("SeoText", "String", 200, True, True))
        ColInfo.Add(New ColumnInfo("DateCreated", "DateTime", 19, True, True))
        ColInfo.Add(New ColumnInfo("CreatorID", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("LastEdited", "DateTime", 19, True, True))
        ColInfo.Add(New ColumnInfo("EditorID", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("Watermarkimg", "String", 200, True, True))
        ColInfo.Add(New ColumnInfo("DateSold", "DateTime", 19, True, True))
        ColInfo.Add(New ColumnInfo("DateDeleted", "DateTime", 19, True, True))
        ColInfo.Add(New ColumnInfo("ClientIP", "String", 20, True, True))
        ColInfo.Add(New ColumnInfo("TotalViews", "Integer", 11, True, True))
        ColInfo.Sort()
    End Sub

    ''' <summary></summary>
    ''' <param name="_Conn">Database connection object to be used in further operations</param>
    Public Sub New(ByRef _Conn as SqlConnection)
        Me.New()
        Conn = _Conn
    End Sub

    ''' <summary></summary>
    ''' <param name="_ConnectionString">Database connection string to be used in further operations</param>
    Public Sub New(ByRef _ConnectionString as String)
        Me.New()
        ConnectionString = _ConnectionString
    End Sub

#End Region

#Region " Methods "
    ''' <summary>Clear the local column variables associated with a Product table record</summary>
    Public Sub Blank()
        Id = 0
        ItemNumber = ""
        ProductName = ""
        ManufacturerId = 0
        Make = ""
        Model = ""
        Description = ""
        AdminNotes = ""
        Condition = ""
        Age = ""
        Weight = 0
        Price = 0
        Qty = 0
        QuantitySold = 0
        LowestPrice = 0
        CostofGoods = 0
        AuctionStart = 0
        SellingPrice = 0
        TotalPieces = 0
        ManualItemNo = ""
        Category = 0
        ParentCategory = 0
        Location = ""
        Barcode = ""
        BarcodeParent = ""
        IsNotOnWeb = 0
        IsNewArrivalsPage = 0
        IsFeaturedItem = 0
        IsJustOfftheTruck = 0
        IsConsignmentItem = 0
        IsDoNotRelease = 0
        IsHold = 0
        IsSold = 0
        IsSpecial = 0
        IsDeleteItem = 0
        IsLabX = 0
        IsIncludeinNewsletter = 0
        IsDeletePermanently = 0
        IsPaid = 0
        IsShipped = 0
        IsCompleted = 0
        ImageFileName = ""
        ThumbFileName = ""
        VideoURL = ""
        SeoText = ""
        DateCreated = Nothing
        CreatorID = 0
        LastEdited = Nothing
        EditorID = 0
        Watermarkimg = ""
        DateCreated = Nothing
        DateDeleted = Nothing
        ClientIP = ""
        TotalViews = 0
        Valid = False
    End Sub

    ''' <summary>Get rows from the Product table</summary>
    ''' <param name="ColumnList">A list of column names in the Product table separated by commas</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the Product table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As String, ByRef WhereClause As String, ByRef OrderBy As String) As DataTable
        Dim cmd as SqlCommand
        Dim da as SqlDataAdapter
        Dim dt as New DataTable
        Dim ob as String = OrderBy

        Try
            Connect()
            If OrderBy <> "" Then ob = " ORDER BY " & ob
            cmd = New SqlCommand("SELECT " & columnlist & " FROM ""Product"" WHERE " & whereClause & ob, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)
            da.Dispose()
            Disconnect()
            Return dt
        Catch ex As SqlException
            LastError = TranslateException(ex)
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return Nothing
    End Function

    ''' <summary>Get rows from the Product table</summary>
    ''' <param name="ColumnList">A list of column names in the Product table separated by commas</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the Product table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As String, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    ''' <summary>Get a rows from the Product table</summary>
    ''' <param name="ColumnList">An array of column names in the Product</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the Product table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList() As String, ByRef WhereClause As String, ByRef OrderBy As String) As DataTable
        Dim cl as String = ""
        Dim p as String

        For Each p in ColumnList
            If cl <> "" Then cl &= ", "
            cl &= p
        Next
        Return GetRows(cl, WhereClause, OrderBy)
    End Function

    ''' <summary>Get a rows from the Product table</summary>
    ''' <param name="ColumnList">An array of column names in the Product</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the Product table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList() As String, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    ''' <summary>Get a rows from the Product table</summary>
    ''' <param name="ColumnList">An ArrayList of Strings representing column names in the Product</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the Product table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As ArrayList, ByRef WhereClause As String, ByRef OrderBy As String) As DataTable
        Dim cl as String = ""
        Dim p as String

        For Each p in ColumnList
            If cl <> "" Then cl &= ", "
            cl &= p
        Next
        Return GetRows(cl, WhereClause, OrderBy)
    End Function

    ''' <summary>Get a rows from the Product table</summary>
    ''' <param name="ColumnList">An ArrayList of Strings representing column names in the Product</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the Product table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As ArrayList, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    Public Function Delete(ByVal Id As Integer) As Boolean
        Dim cmd As SqlCommand
        Try
            Connect()
            cmd = New SqlCommand("DELETE FROM ""Product"" WHERE Id = @Id", Conn)
            cmd.Parameters.AddWithValue("@Id", Id)
            cmd.ExecuteScalar()
            cmd.Dispose()
            Disconnect()
        Catch ex As SqlException
            Disconnect()
            LastError = TranslateException(ex)
            Return False
        Catch ex As Exception
            Disconnect()
            LastError = ex.Message
            Return False
        End Try
        Blank()
        Return True
    End Function

    ''' <summary>Delete a row from the Customer table</summary>
    ''' <param name="Row">The DataRow to be deletedfrom the Customer table</param>
    ''' <returns>'True', if successful</returns>
    ''' <remarks>The DataRow must contain all primary key columns</remarks>
    Public Function Delete(ByRef Row As DataRow) As Boolean
        Dim cmd As SqlCommand

        Try
            Connect()
            cmd = New SqlCommand("DELETE FROM ""Product"" WHERE Id = @Id", Conn)
            cmd.Parameters.AddWithValue("@Id", Row("Id"))
            cmd.ExecuteScalar()
            cmd.Dispose()
            Disconnect()
        Catch ex As SqlException
            LastError = TranslateException(ex)
            Disconnect()
            Return False
        Catch ex As Exception
            LastError = ex.Message
            Disconnect()
            Return False
        End Try
        Return True
    End Function

    ''' <summary>Delete the current row from the Customer table</summary>
    ''' <returns>'True', if successful</returns>
    Public Function Delete() As Boolean
        If Not Valid Then
            LastError = "A valid current record is needed to delete"
            Return False
        End If
        Return Delete(Id)
    End Function
    ''' <summary>Insert a record in the Product table from the data stored in the local variables</summary>
    ''' <returns>'True', if successful</returns>
    Public Function Insert() As Boolean
        Dim cmd as SqlCommand

        Try
            Connect()
            cmd = New SqlCommand("INSERT INTO ""Product"" (ItemNumber, ProductName, ManufacturerId, Make, Model, Description, AdminNotes, Condition, Age, Weight, Price, Qty, QuantitySold, LowestPrice, CostofGoods, AuctionStart, SellingPrice, TotalPieces, ManualItemNo, Category, ParentCategory, Location, Barcode, BarcodeParent, IsNotOnWeb, IsNewArrivalsPage, IsFeaturedItem, IsJustOfftheTruck, IsConsignmentItem, IsDoNotRelease, IsHold, IsSold, IsSpecial, IsDeleteItem, IsLabX, IsIncludeinNewsletter, IsDeletePermanently, IsPaid, IsShipped, IsCompleted, ImageFileName, ThumbFileName, VideoURL, SeoText, DateCreated, CreatorID, LastEdited, EditorID) VALUES (@ItemNumber, @ProductName, @ManufacturerId, @Make, @Model, @Description, @AdminNotes, @Condition, @Age, @Weight, @Price, @Qty, @QuantitySold, @LowestPrice, @CostofGoods, @AuctionStart, @SellingPrice, @TotalPieces, @ManualItemNo, @Category, @ParentCategory, @Location, @Barcode, @BarcodeParent, @IsNotOnWeb, @IsNewArrivalsPage, @IsFeaturedItem, @IsJustOfftheTruck, @IsConsignmentItem, @IsDoNotRelease, @IsHold, @IsSold, @IsSpecial, @IsDeleteItem, @IsLabX, @IsIncludeinNewsletter, @IsDeletePermanently, @IsPaid, @IsShipped, @IsCompleted, @ImageFileName, @ThumbFileName, @VideoURL, @SeoText, @DateCreated, @CreatorID, @LastEdited, @EditorID)", conn)
            cmd.Parameters.AddWithValue("@ItemNumber", ItemNumber)
            cmd.Parameters.AddWithValue("@ProductName", ProductName)
            cmd.Parameters.AddWithValue("@ManufacturerId", ManufacturerId)
            cmd.Parameters.AddWithValue("@Make", Make)
            cmd.Parameters.AddWithValue("@Model", Model)
            cmd.Parameters.AddWithValue("@Description", Description)
            cmd.Parameters.AddWithValue("@AdminNotes", AdminNotes)
            cmd.Parameters.AddWithValue("@Condition", Condition)
            cmd.Parameters.AddWithValue("@Age", Age)
            cmd.Parameters.AddWithValue("@Weight", Weight)
            cmd.Parameters.AddWithValue("@Price", Price)
            cmd.Parameters.AddWithValue("@Qty", Qty)
            cmd.Parameters.AddWithValue("@QuantitySold", QuantitySold)
            cmd.Parameters.AddWithValue("@LowestPrice", LowestPrice)
            cmd.Parameters.AddWithValue("@CostofGoods", CostofGoods)
            cmd.Parameters.AddWithValue("@AuctionStart", AuctionStart)
            cmd.Parameters.AddWithValue("@SellingPrice", SellingPrice)
            cmd.Parameters.AddWithValue("@TotalPieces", TotalPieces)
            cmd.Parameters.AddWithValue("@ManualItemNo", ManualItemNo)
            cmd.Parameters.AddWithValue("@Category", Category)
            cmd.Parameters.AddWithValue("@ParentCategory", ParentCategory)
            cmd.Parameters.AddWithValue("@Location", Location)
            cmd.Parameters.AddWithValue("@Barcode", Barcode)
            cmd.Parameters.AddWithValue("@BarcodeParent", BarcodeParent)
            cmd.Parameters.AddWithValue("@IsNotOnWeb", IsNotOnWeb)
            cmd.Parameters.AddWithValue("@IsNewArrivalsPage", IsNewArrivalsPage)
            cmd.Parameters.AddWithValue("@IsFeaturedItem", IsFeaturedItem)
            cmd.Parameters.AddWithValue("@IsJustOfftheTruck", IsJustOfftheTruck)
            cmd.Parameters.AddWithValue("@IsConsignmentItem", IsConsignmentItem)
            cmd.Parameters.AddWithValue("@IsDoNotRelease", IsDoNotRelease)
            cmd.Parameters.AddWithValue("@IsHold", IsHold)
            cmd.Parameters.AddWithValue("@IsSold", IsSold)
            cmd.Parameters.AddWithValue("@IsSpecial", IsSpecial)
            cmd.Parameters.AddWithValue("@IsDeleteItem", IsDeleteItem)
            cmd.Parameters.AddWithValue("@IsLabX", IsLabX)
            cmd.Parameters.AddWithValue("@IsIncludeinNewsletter", IsIncludeinNewsletter)
            cmd.Parameters.AddWithValue("@IsDeletePermanently", IsDeletePermanently)
            cmd.Parameters.AddWithValue("@IsPaid", IsPaid)
            cmd.Parameters.AddWithValue("@IsShipped", IsShipped)
            cmd.Parameters.AddWithValue("@IsCompleted", IsCompleted)
            cmd.Parameters.AddWithValue("@ImageFileName", ImageFileName)
            cmd.Parameters.AddWithValue("@ThumbFileName", ThumbFileName)
            cmd.Parameters.AddWithValue("@VideoURL", VideoURL)
            cmd.Parameters.AddWithValue("@SeoText", SeoText)
            cmd.Parameters.AddWithValue("@DateCreated", DateCreated)
            cmd.Parameters.AddWithValue("@CreatorID", CreatorID)
            cmd.Parameters.AddWithValue("@LastEdited", LastEdited)
            cmd.Parameters.AddWithValue("@EditorID", EditorID)
            cmd.ExecuteScalar()
            cmd.Dispose()

            REM Attempt to load the auto-generated Id. This is not fool-proof.
            Dim da as SqlDataAdapter
            Dim dt as New DataTable

            cmd = New SqlCommand("SELECT TOP 1 Id FROM ""Product"" WHERE ItemNumber = @ItemNumber AND ProductName = @ProductName AND ManufacturerId = @ManufacturerId AND Make = @Make AND Model = @Model AND Description = @Description AND AdminNotes = @AdminNotes AND Condition = @Condition AND Age = @Age AND Weight = @Weight AND Price = @Price AND Qty = @Qty AND QuantitySold = @QuantitySold AND LowestPrice = @LowestPrice AND CostofGoods = @CostofGoods AND AuctionStart = @AuctionStart AND SellingPrice = @SellingPrice AND TotalPieces = @TotalPieces AND ManualItemNo = @ManualItemNo AND Category = @Category AND ParentCategory = @ParentCategory AND Location = @Location AND Barcode = @Barcode AND BarcodeParent = @BarcodeParent AND IsNotOnWeb = @IsNotOnWeb AND IsNewArrivalsPage = @IsNewArrivalsPage AND IsFeaturedItem = @IsFeaturedItem AND IsJustOfftheTruck = @IsJustOfftheTruck AND IsConsignmentItem = @IsConsignmentItem AND IsDoNotRelease = @IsDoNotRelease AND IsHold = @IsHold AND IsSold = @IsSold AND IsSpecial = @IsSpecial AND IsDeleteItem = @IsDeleteItem AND IsLabX = @IsLabX AND IsIncludeinNewsletter = @IsIncludeinNewsletter AND IsDeletePermanently = @IsDeletePermanently AND IsPaid = @IsPaid AND IsShipped = @IsShipped AND IsCompleted = @IsCompleted AND ImageFileName = @ImageFileName AND ThumbFileName = @ThumbFileName AND VideoURL = @VideoURL AND SeoText = @SeoText AND DateCreated = @DateCreated AND CreatorID = @CreatorID AND LastEdited = @LastEdited AND EditorID = @EditorID ORDER BY Id DESC", conn)
            cmd.Parameters.AddWithValue("@ItemNumber", ItemNumber)
            cmd.Parameters.AddWithValue("@ProductName", ProductName)
            cmd.Parameters.AddWithValue("@ManufacturerId", ManufacturerId)
            cmd.Parameters.AddWithValue("@Make", Make)
            cmd.Parameters.AddWithValue("@Model", Model)
            cmd.Parameters.AddWithValue("@Description", Description)
            cmd.Parameters.AddWithValue("@AdminNotes", AdminNotes)
            cmd.Parameters.AddWithValue("@Condition", Condition)
            cmd.Parameters.AddWithValue("@Age", Age)
            cmd.Parameters.AddWithValue("@Weight", Weight)
            cmd.Parameters.AddWithValue("@Price", Price)
            cmd.Parameters.AddWithValue("@Qty", Qty)
            cmd.Parameters.AddWithValue("@QuantitySold", QuantitySold)
            cmd.Parameters.AddWithValue("@LowestPrice", LowestPrice)
            cmd.Parameters.AddWithValue("@CostofGoods", CostofGoods)
            cmd.Parameters.AddWithValue("@AuctionStart", AuctionStart)
            cmd.Parameters.AddWithValue("@SellingPrice", SellingPrice)
            cmd.Parameters.AddWithValue("@TotalPieces", TotalPieces)
            cmd.Parameters.AddWithValue("@ManualItemNo", ManualItemNo)
            cmd.Parameters.AddWithValue("@Category", Category)
            cmd.Parameters.AddWithValue("@ParentCategory", ParentCategory)
            cmd.Parameters.AddWithValue("@Location", Location)
            cmd.Parameters.AddWithValue("@Barcode", Barcode)
            cmd.Parameters.AddWithValue("@BarcodeParent", BarcodeParent)
            cmd.Parameters.AddWithValue("@IsNotOnWeb", IsNotOnWeb)
            cmd.Parameters.AddWithValue("@IsNewArrivalsPage", IsNewArrivalsPage)
            cmd.Parameters.AddWithValue("@IsFeaturedItem", IsFeaturedItem)
            cmd.Parameters.AddWithValue("@IsJustOfftheTruck", IsJustOfftheTruck)
            cmd.Parameters.AddWithValue("@IsConsignmentItem", IsConsignmentItem)
            cmd.Parameters.AddWithValue("@IsDoNotRelease", IsDoNotRelease)
            cmd.Parameters.AddWithValue("@IsHold", IsHold)
            cmd.Parameters.AddWithValue("@IsSold", IsSold)
            cmd.Parameters.AddWithValue("@IsSpecial", IsSpecial)
            cmd.Parameters.AddWithValue("@IsDeleteItem", IsDeleteItem)
            cmd.Parameters.AddWithValue("@IsLabX", IsLabX)
            cmd.Parameters.AddWithValue("@IsIncludeinNewsletter", IsIncludeinNewsletter)
            cmd.Parameters.AddWithValue("@IsDeletePermanently", IsDeletePermanently)
            cmd.Parameters.AddWithValue("@IsPaid", IsPaid)
            cmd.Parameters.AddWithValue("@IsShipped", IsShipped)
            cmd.Parameters.AddWithValue("@IsCompleted", IsCompleted)
            cmd.Parameters.AddWithValue("@ImageFileName", ImageFileName)
            cmd.Parameters.AddWithValue("@ThumbFileName", ThumbFileName)
            cmd.Parameters.AddWithValue("@VideoURL", VideoURL)
            cmd.Parameters.AddWithValue("@SeoText", SeoText)
            cmd.Parameters.AddWithValue("@DateCreated", DateCreated)
            cmd.Parameters.AddWithValue("@CreatorID", CreatorID)
            cmd.Parameters.AddWithValue("@LastEdited", LastEdited)
            cmd.Parameters.AddWithValue("@EditorID", EditorID)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Id = CInt(dt.Rows(0)("Id"))
            End If
            dt.Dispose()
            da.Dispose()
            Disconnect()
        Catch ex As SqlException
            LastError = TranslateException(ex)
            Disconnect()
            Return False
        Catch ex As Exception
            LastError = ex.Message
            Disconnect()
            Return False
        End Try
        Return True
    End Function
    Public Function InsertIntoProduct() As Integer
        Dim cmd As SqlCommand
        Dim checkInsert As Integer = 0

        Try
            Connect()
            cmd = New SqlCommand("INSERT INTO ""Product"" (ItemNumber, ProductName, ManufacturerId, Make, Model, Description, AdminNotes, Condition, Age, Weight, Price, Qty, QuantitySold, LowestPrice, CostofGoods, AuctionStart, SellingPrice, TotalPieces, ManualItemNo, Category, ParentCategory, Location, Barcode, BarcodeParent, IsNotOnWeb, IsNewArrivalsPage, IsFeaturedItem, IsJustOfftheTruck, IsConsignmentItem, IsDoNotRelease, IsHold, IsSold, IsSpecial, IsDeleteItem, IsLabX, IsIncludeinNewsletter, IsDeletePermanently, IsPaid, IsShipped, IsCompleted, ImageFileName, ThumbFileName, VideoURL, SeoText, DateCreated, CreatorID,Watermarkimg) VALUES (@ItemNumber, @ProductName, @ManufacturerId, @Make, @Model, @Description, @AdminNotes, @Condition, @Age, @Weight, @Price, @Qty, @QuantitySold, @LowestPrice, @CostofGoods, @AuctionStart, @SellingPrice, @TotalPieces, @ManualItemNo, @Category, @ParentCategory, @Location, @Barcode, @BarcodeParent, @IsNotOnWeb, @IsNewArrivalsPage, @IsFeaturedItem, @IsJustOfftheTruck, @IsConsignmentItem, @IsDoNotRelease, @IsHold, @IsSold, @IsSpecial, @IsDeleteItem, @IsLabX, @IsIncludeinNewsletter, @IsDeletePermanently, @IsPaid, @IsShipped, @IsCompleted, @ImageFileName, @ThumbFileName, @VideoURL, @SeoText, @DateCreated, @CreatorID, @Watermarkimg)", Conn)
            cmd.Parameters.AddWithValue("@ItemNumber", ItemNumber)
            cmd.Parameters.AddWithValue("@ProductName", ProductName)
            cmd.Parameters.AddWithValue("@ManufacturerId", ManufacturerId)
            cmd.Parameters.AddWithValue("@Make", Make)
            cmd.Parameters.AddWithValue("@Model", Model)
            cmd.Parameters.AddWithValue("@Description", Description)
            cmd.Parameters.AddWithValue("@AdminNotes", AdminNotes)
            cmd.Parameters.AddWithValue("@Condition", Condition)
            cmd.Parameters.AddWithValue("@Age", Age)
            cmd.Parameters.AddWithValue("@Weight", Weight)
            cmd.Parameters.AddWithValue("@Price", Price)
            cmd.Parameters.AddWithValue("@Qty", Qty)
            cmd.Parameters.AddWithValue("@QuantitySold", QuantitySold)
            cmd.Parameters.AddWithValue("@LowestPrice", LowestPrice)
            cmd.Parameters.AddWithValue("@CostofGoods", CostofGoods)
            cmd.Parameters.AddWithValue("@AuctionStart", AuctionStart)
            cmd.Parameters.AddWithValue("@SellingPrice", SellingPrice)
            cmd.Parameters.AddWithValue("@TotalPieces", TotalPieces)
            cmd.Parameters.AddWithValue("@ManualItemNo", ManualItemNo)
            cmd.Parameters.AddWithValue("@Category", Category)
            cmd.Parameters.AddWithValue("@ParentCategory", ParentCategory)
            cmd.Parameters.AddWithValue("@Location", Location)
            cmd.Parameters.AddWithValue("@Barcode", Barcode)
            cmd.Parameters.AddWithValue("@BarcodeParent", BarcodeParent)
            cmd.Parameters.AddWithValue("@IsNotOnWeb", IsNotOnWeb)
            cmd.Parameters.AddWithValue("@IsNewArrivalsPage", IsNewArrivalsPage)
            cmd.Parameters.AddWithValue("@IsFeaturedItem", IsFeaturedItem)
            cmd.Parameters.AddWithValue("@IsJustOfftheTruck", IsJustOfftheTruck)
            cmd.Parameters.AddWithValue("@IsConsignmentItem", IsConsignmentItem)
            cmd.Parameters.AddWithValue("@IsDoNotRelease", IsDoNotRelease)
            cmd.Parameters.AddWithValue("@IsHold", IsHold)
            cmd.Parameters.AddWithValue("@IsSold", IsSold)
            cmd.Parameters.AddWithValue("@IsSpecial", IsSpecial)
            cmd.Parameters.AddWithValue("@IsDeleteItem", IsDeleteItem)
            cmd.Parameters.AddWithValue("@IsLabX", IsLabX)
            cmd.Parameters.AddWithValue("@IsIncludeinNewsletter", IsIncludeinNewsletter)
            cmd.Parameters.AddWithValue("@IsDeletePermanently", IsDeletePermanently)
            cmd.Parameters.AddWithValue("@IsPaid", IsPaid)
            cmd.Parameters.AddWithValue("@IsShipped", IsShipped)
            cmd.Parameters.AddWithValue("@IsCompleted", IsCompleted)
            cmd.Parameters.AddWithValue("@ImageFileName", ImageFileName)
            cmd.Parameters.AddWithValue("@ThumbFileName", ThumbFileName)
            cmd.Parameters.AddWithValue("@VideoURL", VideoURL)
            cmd.Parameters.AddWithValue("@SeoText", SeoText)
            cmd.Parameters.AddWithValue("@DateCreated", DateCreated)
            cmd.Parameters.AddWithValue("@CreatorID", CreatorID)
            cmd.Parameters.AddWithValue("@Watermarkimg", Watermarkimg)
            cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            cmd.CommandText = "Select @@Identity"
            checkInsert = CInt(cmd.ExecuteScalar().ToString())
            cmd.Dispose()
            Disconnect()
        Catch ex As SqlException
            LastError = TranslateException(ex)
            Disconnect()
            Return 0
        Catch ex As Exception
            LastError = ex.Message
            Disconnect()
            Return 0
        End Try
        Return checkInsert
    End Function
    ''' <summary>Insert a record in the Product table from a DataRow</summary>
    ''' <param name="Row">The DataRow to be inserted in the Product table</param>
    ''' <returns>'True', if successful</returns>
    ''' <remarks>The DataRow must contain all primary key columns</remarks>
    Public Function Insert(ByRef Row As DataRow) As Boolean
        Dim cmd as SqlCommand
        Dim col as DataColumn
        Dim setList as String = ""
        Dim valList as String = ""

        Try
            Connect()
            REM Build the set list
            For Each col in Row.Table.Columns
                Select Case col.ColumnName.ToLower()
                    Case "id"
                    Case Else
                        If setList <> "" Then
                            setList &= ", "
                            valList &= ", "
                        End If
                        setList &= col.ColumnName
                        valList &= " @" & col.ColumnName
                End Select
            Next
            cmd = New SqlCommand("INSERT INTO ""Product"" (" & setList & ") VALUES (" & valList & ")", conn)
            REM Create the parameters
            For Each col in Row.Table.Columns
                Select Case col.ColumnName.ToLower()
                    Case "id"
                    Case Else
                        cmd.Parameters.AddWithValue("@" & col.ColumnName, Row(col.ColumnName))
                End Select
            Next
            cmd.ExecuteScalar()
            cmd.Dispose()
            Disconnect()
        Catch ex As SqlException
            LastError = TranslateException(ex)
            Disconnect()
            Return False
        Catch ex As Exception
            LastError = ex.Message
            Disconnect()
            Return False
        End Try
        Return True
    End Function
    Public Function Update() As Boolean
        Try
            Connect()
            Dim cmd As SqlCommand
            cmd = New SqlCommand("UPDATE ""Product"" SET ItemNumber = @ItemNumber , ProductName = @ProductName , ManufacturerId = @ManufacturerId , Make = @Make , Model = @Model , Description = @Description , AdminNotes = @AdminNotes , Condition = @Condition , Age = @Age , Weight = @Weight , Price = @Price , Qty = @Qty , QuantitySold = @QuantitySold , LowestPrice = @LowestPrice , CostofGoods = @CostofGoods , AuctionStart = @AuctionStart , SellingPrice = @SellingPrice , TotalPieces = @TotalPieces , ManualItemNo = @ManualItemNo , Category = @Category , ParentCategory = @ParentCategory , Location = @Location , Barcode = @Barcode , BarcodeParent = @BarcodeParent , IsNotOnWeb = @IsNotOnWeb , IsNewArrivalsPage = @IsNewArrivalsPage , IsFeaturedItem = @IsFeaturedItem , IsJustOfftheTruck = @IsJustOfftheTruck , IsConsignmentItem = @IsConsignmentItem , IsDoNotRelease = @IsDoNotRelease , IsHold = @IsHold , IsSold = @IsSold , IsSpecial = @IsSpecial , IsDeleteItem = @IsDeleteItem , IsLabX = @IsLabX , IsIncludeinNewsletter = @IsIncludeinNewsletter , IsDeletePermanently = @IsDeletePermanently , IsPaid = @IsPaid , IsShipped = @IsShipped , IsCompleted = @IsCompleted , ImageFileName = @ImageFileName , ThumbFileName = @ThumbFileName , VideoURL = @VideoURL , SeoText = @SeoText , LastEdited = @LastEdited , EditorID = @EditorID, Watermarkimg = @Watermarkimg  WHERE Id = @Id", Conn)
            cmd.Parameters.AddWithValue("@Id", Id)
            cmd.Parameters.AddWithValue("@ItemNumber", ItemNumber)
            cmd.Parameters.AddWithValue("@ProductName", ProductName)
            cmd.Parameters.AddWithValue("@ManufacturerId", ManufacturerId)
            cmd.Parameters.AddWithValue("@Make", Make)
            cmd.Parameters.AddWithValue("@Model", Model)
            cmd.Parameters.AddWithValue("@Description", Description)
            cmd.Parameters.AddWithValue("@AdminNotes", AdminNotes)
            cmd.Parameters.AddWithValue("@Condition", Condition)
            cmd.Parameters.AddWithValue("@Age", Age)
            cmd.Parameters.AddWithValue("@Weight", Weight)
            cmd.Parameters.AddWithValue("@Price", Price)
            cmd.Parameters.AddWithValue("@Qty", Qty)
            cmd.Parameters.AddWithValue("@QuantitySold", QuantitySold)
            cmd.Parameters.AddWithValue("@LowestPrice", LowestPrice)
            cmd.Parameters.AddWithValue("@CostofGoods", CostofGoods)
            cmd.Parameters.AddWithValue("@AuctionStart", AuctionStart)
            cmd.Parameters.AddWithValue("@SellingPrice", SellingPrice)
            cmd.Parameters.AddWithValue("@TotalPieces", TotalPieces)
            cmd.Parameters.AddWithValue("@ManualItemNo", ManualItemNo)
            cmd.Parameters.AddWithValue("@Category", Category)
            cmd.Parameters.AddWithValue("@ParentCategory", ParentCategory)
            cmd.Parameters.AddWithValue("@Location", Location)
            cmd.Parameters.AddWithValue("@Barcode", Barcode)
            cmd.Parameters.AddWithValue("@BarcodeParent", BarcodeParent)
            cmd.Parameters.AddWithValue("@IsNotOnWeb", IsNotOnWeb)
            cmd.Parameters.AddWithValue("@IsNewArrivalsPage", IsNewArrivalsPage)
            cmd.Parameters.AddWithValue("@IsFeaturedItem", IsFeaturedItem)
            cmd.Parameters.AddWithValue("@IsJustOfftheTruck", IsJustOfftheTruck)
            cmd.Parameters.AddWithValue("@IsConsignmentItem", IsConsignmentItem)
            cmd.Parameters.AddWithValue("@IsDoNotRelease", IsDoNotRelease)
            cmd.Parameters.AddWithValue("@IsHold", IsHold)
            cmd.Parameters.AddWithValue("@IsSold", IsSold)
            cmd.Parameters.AddWithValue("@IsSpecial", IsSpecial)
            cmd.Parameters.AddWithValue("@IsDeleteItem", IsDeleteItem)
            cmd.Parameters.AddWithValue("@IsLabX", IsLabX)
            cmd.Parameters.AddWithValue("@IsIncludeinNewsletter", IsIncludeinNewsletter)
            cmd.Parameters.AddWithValue("@IsDeletePermanently", IsDeletePermanently)
            cmd.Parameters.AddWithValue("@IsPaid", IsPaid)
            cmd.Parameters.AddWithValue("@IsShipped", IsShipped)
            cmd.Parameters.AddWithValue("@IsCompleted", IsCompleted)
            cmd.Parameters.AddWithValue("@ImageFileName", ImageFileName)
            cmd.Parameters.AddWithValue("@ThumbFileName", ThumbFileName)
            cmd.Parameters.AddWithValue("@VideoURL", VideoURL)
            cmd.Parameters.AddWithValue("@SeoText", SeoText)
            cmd.Parameters.AddWithValue("@LastEdited", LastEdited)
            cmd.Parameters.AddWithValue("@EditorID", EditorID)
            cmd.Parameters.AddWithValue("@Watermarkimg", Watermarkimg)
            cmd.ExecuteScalar()
            cmd.Dispose()
            Disconnect()
        Catch ex As SqlException
            LastError = TranslateException(ex)
            Disconnect()
            Return False
        Catch ex As Exception
            LastError = ex.Message
            Disconnect()
            Return False
        End Try
        Return True
    End Function
    ''' <summary>Update the Customer table from a DataRow</summary>
    ''' <param name="Row">The DataRow to be updated in the Customer table</param>
   
    Public Function UpdateImage() As Boolean
        Try
            Connect()
            Dim cmd As SqlCommand
            cmd = New SqlCommand("UPDATE ""Product"" SET ImageFileName = @ImageFileName WHERE Id = @Id", Conn)
            cmd.Parameters.AddWithValue("@Id", Id)
            cmd.Parameters.AddWithValue("@ImageFileName", ImageFileName)
            cmd.ExecuteScalar()
            cmd.Dispose()
            Disconnect()
        Catch ex As SqlException
            LastError = TranslateException(ex)
            Disconnect()
            Return False
        Catch ex As Exception
            LastError = ex.Message
            Disconnect()
            Return False
        End Try
        Return True
    End Function
    Public Function Update(ByRef Row As DataRow) As Boolean
        Dim cmd As SqlCommand
        Dim col As DataColumn
        Dim setList As String = ""

        Try
            Connect()
            REM Build the set list
            For Each col In Row.Table.Columns
                If setList <> "" Then setList &= ", "
                setList &= col.ColumnName & " = @" & col.ColumnName
            Next
            cmd = New SqlCommand("UPDATE ""Product"" SET " & setList & " WHERE Id = @Id", Conn)
            REM Create the parameters
            For Each col In Row.Table.Columns
                cmd.Parameters.AddWithValue("@" & col.ColumnName, Row(col.ColumnName))
            Next
            cmd.ExecuteScalar()
            cmd.Dispose()
            Disconnect()
        Catch ex As SqlException
            LastError = TranslateException(ex)
            Disconnect()
            Return False
        Catch ex As Exception
            LastError = ex.Message
            Disconnect()
            Return False
        End Try
        Return True
    End Function
    ''' <summary></summary>
    ''' <returns>The column names from Product table separated by commas</returns>
    Public Overrides Function ToString() As String
        Dim p as String = ""
        p &= Cstr(Id) & ", "
        p &= ItemNumber & ", "
        p &= ProductName & ", "
        p &= Cstr(ManufacturerId) & ", "
        p &= Make & ", "
        p &= Model & ", "
        p &= Description & ", "
        p &= AdminNotes & ", "
        p &= Condition & ", "
        p &= Age & ", "
        p &= Cstr(Weight) & ", "
        p &= Cstr(Price) & ", "
        p &= Cstr(Qty) & ", "
        p &= Cstr(QuantitySold) & ", "
        p &= Cstr(LowestPrice) & ", "
        p &= Cstr(CostofGoods) & ", "
        p &= Cstr(AuctionStart) & ", "
        p &= Cstr(SellingPrice) & ", "
        p &= Cstr(TotalPieces) & ", "
        p &= ManualItemNo & ", "
        p &= Cstr(Category) & ", "
        p &= Cstr(ParentCategory) & ", "
        p &= Location & ", "
        p &= Barcode & ", "
        p &= BarcodeParent & ", "
        p &= Cstr(IsNotOnWeb) & ", "
        p &= Cstr(IsNewArrivalsPage) & ", "
        p &= Cstr(IsFeaturedItem) & ", "
        p &= Cstr(IsJustOfftheTruck) & ", "
        p &= Cstr(IsConsignmentItem) & ", "
        p &= Cstr(IsDoNotRelease) & ", "
        p &= Cstr(IsHold) & ", "
        p &= Cstr(IsSold) & ", "
        p &= Cstr(IsSpecial) & ", "
        p &= Cstr(IsDeleteItem) & ", "
        p &= Cstr(IsLabX) & ", "
        p &= Cstr(IsIncludeinNewsletter) & ", "
        p &= Cstr(IsDeletePermanently) & ", "
        p &= Cstr(IsPaid) & ", "
        p &= Cstr(IsShipped) & ", "
        p &= Cstr(IsCompleted) & ", "
        p &= ImageFileName & ", "
        p &= ThumbFileName & ", "
        p &= VideoURL & ", "
        p &= SeoText & ", "
        p &= DateCreated.ToString() & ", "
        p &= Cstr(CreatorID) & ", "
        p &= LastEdited.ToString() & ", "
        p &= CStr(EditorID) & ", "
        p &= ManualItemNo & ", "
        p &= Watermarkimg.ToString() & ","
        p &= CStr(TotalPieces) & ","
        p &= CStr(DateSold) & ","
        p &= CStr(DateDeleted) & ","
        p &= ClientIP & ","
        p &= CStr(TotalViews)
        Return p
    End Function

#End Region

#Region " Private Methods "
    Private Sub Connect()
        If connectionString = "" Then
            If conn Is Nothing Then Throw New Exception("Database not connected")
        Else
            conn = New SqlConnection(connectionString)
            conn.Open()
        End If
    End Sub

    Private Sub Disconnect()
        If connectionString <> "" And Not (conn Is Nothing) Then
            Try
                conn.Close()
                conn.Dispose()
                conn = Nothing
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Function TranslateException(ByRef ex As SqlException) As String
        Dim p As String = ""
        Dim er As SqlError
        For Each er In ex.Errors
            p &= er.Message & Chr(13) & Chr(10)
        Next
        Return p
    End Function

#End Region

#Region " User Code "
#End Region
End Class
