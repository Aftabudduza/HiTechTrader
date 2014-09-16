Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient

Public Class OrderHistory
    REM If 'ConnectionString' is set by the constructor or externally, then the connection will be
    REM opened and closed with each operation. If 'conn' is set, then the connection stays open.
    Public Conn As SqlConnection = Nothing
    Public ConnectionString As String = ""
    Public BooleanFalse As String = "N"
    Public BooleanTrue As String = "Y"
    Public LastError As String = ""
    Public Valid As Boolean = False
    Public ColInfo As ArrayList
    Public GridPageSize As Integer
    Public GridTotalRecords As Long
    Public GridShowProgress As Boolean
    Public GridAllowAdd As Boolean = True
    Public GridAllowEdit As Boolean = True
    Public GridAllowDelete As Boolean = True
    Public GridAllowUpdate As Boolean = True
    Public GridCurrentRow As Integer = 0
    Public GridEndRow As Long
    Public GridTable As DataTable

    REM A variable for each column in the table.
    Public Id As Integer
    Public Name As String
    Public Company As String
    Public Email As String
    Public Address As String
    Public Address1 As String
    Public City As String
    Public State As String
    Public Zip As String
    Public Country As String
    Public Phone As String
    Public Fax As String
    Public Message As String
    Public ItemToPurchase As String
    Public ItemsToSale As String
    Public IsNeedASAP As Short
    Public IsNeedFuture As Short
    Public IsAdd As Short
    Public IsContact As Short
    Public Type As String
    Public OptionPrice As String
    Public Price As Double
    Public CardType As String
    Public PONumber As String
    Public IsCheck As Short
    Public IsOther As Short
    Public OrderDate As DateTime
    Public ProductId As Integer
    Public ItemNumber As String


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
        ColInfo.Add(New ColumnInfo("Name", "String", 40, True, True))
        ColInfo.Add(New ColumnInfo("Company", "String", 40, True, True))
        ColInfo.Add(New ColumnInfo("Email", "String", 40, True, True))
        ColInfo.Add(New ColumnInfo("Address", "String", 100, True, True))
        ColInfo.Add(New ColumnInfo("Address1", "String", 100, True, True))
        ColInfo.Add(New ColumnInfo("City", "String", 30, True, True))
        ColInfo.Add(New ColumnInfo("State", "String", 2, True, True))
        ColInfo.Add(New ColumnInfo("Zip", "String", 10, True, True))
        ColInfo.Add(New ColumnInfo("Country", "String", 40, True, True))
        ColInfo.Add(New ColumnInfo("Phone", "String", 30, True, True))
        ColInfo.Add(New ColumnInfo("Fax", "String", 30, True, True))
        ColInfo.Add(New ColumnInfo("Message", "String", 500, True, True))
        ColInfo.Add(New ColumnInfo("ItemToPurchase", "String", 500, True, True))
        ColInfo.Add(New ColumnInfo("ItemsToSale", "String", 500, True, True))
        ColInfo.Add(New ColumnInfo("IsNeedASAP", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsNeedFuture", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsAdd", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsContact", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("Type", "String", 30, True, True))
        ColInfo.Add(New ColumnInfo("OptionPrice", "String", 20, True, True))
        ColInfo.Add(New ColumnInfo("Price", "Double", 15, True, True))
        ColInfo.Add(New ColumnInfo("CardType", "String", 20, True, True))
        ColInfo.Add(New ColumnInfo("PONumber", "String", 20, True, True))
        ColInfo.Add(New ColumnInfo("IsCheck", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("IsOther", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("OrderDate", "Datetime", 19, True, True))
        ColInfo.Add(New ColumnInfo("ProductId", "Integer", 11, False, False))
        ColInfo.Add(New ColumnInfo("ItemNumber", "String", 50, True, True))
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
    ''' <summary>Clear the local column variables associated with a OrderHistory table record</summary>
    Public Sub Blank()
        Id = 0
        Name = ""
        Company = ""
        Email = ""
        Address = ""
        Address1 = ""
        City = ""
        State = ""
        Zip = ""
        Country = ""
        Phone = ""
        Fax = ""
        Message = ""
        ItemToPurchase = ""
        ItemsToSale = ""
        IsNeedASAP = 0
        IsNeedFuture = 0
        IsAdd = 0
        IsContact = 0
        Type = ""
        OptionPrice = ""
        Price = 0
        CardType = ""
        PONumber = ""
        IsCheck = 0
        IsOther = 0
        OrderDate = Nothing
        ProductId = 0
        ItemNumber = ""
        Valid = False
    End Sub

    ''' <summary>Get rows from the OrderHistory table</summary>
    ''' <param name="ColumnList">A list of column names in the OrderHistory table separated by commas</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the OrderHistory table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As String, ByRef WhereClause As String, ByRef OrderBy As String) As DataTable
        Dim cmd as SqlCommand
        Dim da as SqlDataAdapter
        Dim dt as New DataTable
        Dim ob as String = OrderBy

        Try
            Connect()
            If OrderBy <> "" Then ob = " ORDER BY " & ob
            cmd = New SqlCommand("SELECT " & columnlist & " FROM ""OrderHistory"" WHERE " & whereClause & ob, conn)
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

    ''' <summary>Get rows from the OrderHistory table</summary>
    ''' <param name="ColumnList">A list of column names in the OrderHistory table separated by commas</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the OrderHistory table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As String, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    ''' <summary>Get a rows from the OrderHistory table</summary>
    ''' <param name="ColumnList">An array of column names in the OrderHistory</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the OrderHistory table</returns>
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

    ''' <summary>Get a rows from the OrderHistory table</summary>
    ''' <param name="ColumnList">An array of column names in the OrderHistory</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the OrderHistory table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList() As String, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    ''' <summary>Get a rows from the OrderHistory table</summary>
    ''' <param name="ColumnList">An ArrayList of Strings representing column names in the OrderHistory</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the OrderHistory table</returns>
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

    ''' <summary>Get a rows from the OrderHistory table</summary>
    ''' <param name="ColumnList">An ArrayList of Strings representing column names in the OrderHistory</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the OrderHistory table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As ArrayList, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    ''' <summary>Get a page of rows from the OrderHistory table</summary>
    ''' <param name="ColumnList">A list of column names in the OrderHistory table separated by commas</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="PageSize">The number of rows in the page</param>
    ''' <param name="CalcTotal">Determines whether the total number of records should be calculated and saved in 'GridTotalRecords'</param>
    ''' <returns>A DataTable from the OrderHistory table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetPage(ByRef ColumnList As String, ByRef WhereClause As String, ByVal PageSize As Integer, ByVal CalcTotal As Boolean) As DataTable
        Dim cmd as SqlCommand
        Dim da as SqlDataAdapter
        Dim dt as DataTable = Nothing
        Dim i as Integer

        GridPageSize = PageSize
        GridShowProgress = CalcTotal
        Try
            Connect()
            For I = 1 To 2
                cmd = New SqlCommand("SELECT " & columnlist & " FROM (SELECT TOP " & PageSize & " * FROM (SELECT TOP " & (GridCurrentRow+PageSize) & " " & columnlist & " FROM ""OrderHistory"" WHERE " & whereclause & " ORDER BY  ASC) AS big ORDER BY  DESC) AS small ORDER BY  ASC", conn)
                da = New SqlDataAdapter(cmd)
                dt = New DataTable()
                da.Fill(dt)
                da.Dispose()
                if dt.Rows.Count > 0 Or gridCurrentRow = 0 Then Exit For
                REM No more rows, go back to the beginning.
                GridCurrentRow = 0
            Next
            GridEndRow = GridCurrentRow + dt.Rows.Count
            If CalcTotal Then
                Dim ct As New DataTable()
                cmd = New SqlCommand("SELECT COUNT(*) FROM ""OrderHistory"" WHERE " & whereClause, conn)
                da = New SqlDataAdapter(cmd)
                da.Fill(ct)
                da.Dispose()
                GridTotalRecords = CLng(ct.Rows(0)(0))
            End If
            Disconnect()
            If dt.Rows.Count > 0 Then Return dt
            LastError = "No records found"
        Catch ex As SqlException
            LastError = TranslateException(ex)
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return Nothing
    End Function

    ''' <summary>Insert a record in the OrderHistory table from the data stored in the local variables</summary>
    ''' <returns>'True', if successful</returns>
    Public Function Insert() As Boolean
        Dim cmd as SqlCommand

        Try
            Connect()
            cmd = New SqlCommand("INSERT INTO ""OrderHistory"" (Name, Company, Email, Address, Address1, City, State, Zip, Country, Phone, Fax, Message, ItemToPurchase, ItemsToSale, IsNeedASAP, IsNeedFuture, IsAdd, IsContact, Type, OptionPrice, Price, CardType, PONumber, IsCheck, IsOther) VALUES (@Name, @Company, @Email, @Address, @Address1, @City, @State, @Zip, @Country, @Phone, @Fax, @Message, @ItemToPurchase, @ItemsToSale, @IsNeedASAP, @IsNeedFuture, @IsAdd, @IsContact, @Type, @OptionPrice, @Price, @CardType, @PONumber, @IsCheck, @IsOther)", conn)
            cmd.Parameters.AddWithValue("@Name", Name)
            cmd.Parameters.AddWithValue("@Company", Company)
            cmd.Parameters.AddWithValue("@Email", Email)
            cmd.Parameters.AddWithValue("@Address", Address)
            cmd.Parameters.AddWithValue("@Address1", Address1)
            cmd.Parameters.AddWithValue("@City", City)
            cmd.Parameters.AddWithValue("@State", State)
            cmd.Parameters.AddWithValue("@Zip", Zip)
            cmd.Parameters.AddWithValue("@Country", Country)
            cmd.Parameters.AddWithValue("@Phone", Phone)
            cmd.Parameters.AddWithValue("@Fax", Fax)
            cmd.Parameters.AddWithValue("@Message", Message)
            cmd.Parameters.AddWithValue("@ItemToPurchase", ItemToPurchase)
            cmd.Parameters.AddWithValue("@ItemsToSale", ItemsToSale)
            cmd.Parameters.AddWithValue("@IsNeedASAP", IsNeedASAP)
            cmd.Parameters.AddWithValue("@IsNeedFuture", IsNeedFuture)
            cmd.Parameters.AddWithValue("@IsAdd", IsAdd)
            cmd.Parameters.AddWithValue("@IsContact", IsContact)
            cmd.Parameters.AddWithValue("@Type", Type)
            cmd.Parameters.AddWithValue("@OptionPrice", OptionPrice)
            cmd.Parameters.AddWithValue("@Price", Price)
            cmd.Parameters.AddWithValue("@CardType", CardType)
            cmd.Parameters.AddWithValue("@PONumber", PONumber)
            cmd.Parameters.AddWithValue("@IsCheck", IsCheck)
            cmd.Parameters.AddWithValue("@IsOther", IsOther)
            cmd.ExecuteScalar()
            cmd.Dispose()

            REM Attempt to load the auto-generated Id. This is not fool-proof.
            Dim da as SqlDataAdapter
            Dim dt as New DataTable

            cmd = New SqlCommand("SELECT TOP 1 Id FROM ""OrderHistory"" WHERE Name = @Name AND Company = @Company AND Email = @Email AND Address = @Address AND Address1 = @Address1 AND City = @City AND State = @State AND Zip = @Zip AND Country = @Country AND Phone = @Phone AND Fax = @Fax AND Message = @Message AND ItemToPurchase = @ItemToPurchase AND ItemsToSale = @ItemsToSale AND IsNeedASAP = @IsNeedASAP AND IsNeedFuture = @IsNeedFuture AND IsAdd = @IsAdd AND IsContact = @IsContact AND Type = @Type AND OptionPrice = @OptionPrice AND Price = @Price AND CardType = @CardType AND PONumber = @PONumber AND IsCheck = @IsCheck AND IsOther = @IsOther ORDER BY Id DESC", conn)
            cmd.Parameters.AddWithValue("@Name", Name)
            cmd.Parameters.AddWithValue("@Company", Company)
            cmd.Parameters.AddWithValue("@Email", Email)
            cmd.Parameters.AddWithValue("@Address", Address)
            cmd.Parameters.AddWithValue("@Address1", Address1)
            cmd.Parameters.AddWithValue("@City", City)
            cmd.Parameters.AddWithValue("@State", State)
            cmd.Parameters.AddWithValue("@Zip", Zip)
            cmd.Parameters.AddWithValue("@Country", Country)
            cmd.Parameters.AddWithValue("@Phone", Phone)
            cmd.Parameters.AddWithValue("@Fax", Fax)
            cmd.Parameters.AddWithValue("@Message", Message)
            cmd.Parameters.AddWithValue("@ItemToPurchase", ItemToPurchase)
            cmd.Parameters.AddWithValue("@ItemsToSale", ItemsToSale)
            cmd.Parameters.AddWithValue("@IsNeedASAP", IsNeedASAP)
            cmd.Parameters.AddWithValue("@IsNeedFuture", IsNeedFuture)
            cmd.Parameters.AddWithValue("@IsAdd", IsAdd)
            cmd.Parameters.AddWithValue("@IsContact", IsContact)
            cmd.Parameters.AddWithValue("@Type", Type)
            cmd.Parameters.AddWithValue("@OptionPrice", OptionPrice)
            cmd.Parameters.AddWithValue("@Price", Price)
            cmd.Parameters.AddWithValue("@CardType", CardType)
            cmd.Parameters.AddWithValue("@PONumber", PONumber)
            cmd.Parameters.AddWithValue("@IsCheck", IsCheck)
            cmd.Parameters.AddWithValue("@IsOther", IsOther)
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

    Public Function InsertInquiry() As Integer
        Dim cmd As SqlCommand
        Dim checkInsert As Integer = 0
        Try
            Connect()
            cmd = New SqlCommand("INSERT INTO ""OrderHistory"" (Name, Company, Email, Address, Address1, City, State, Zip, Country, Phone, Fax, Message, ItemToPurchase, ItemsToSale, IsNeedASAP, IsNeedFuture, IsAdd, IsContact, Type, OptionPrice, Price, CardType, PONumber, IsCheck, IsOther, OrderDate, ProductId, ItemNumber) VALUES (@Name, @Company, @Email, @Address, @Address1, @City, @State, @Zip, @Country, @Phone, @Fax, @Message, @ItemToPurchase, @ItemsToSale, @IsNeedASAP, @IsNeedFuture, @IsAdd, @IsContact, @Type, @OptionPrice, @Price, @CardType, @PONumber, @IsCheck, @IsOther, @OrderDate, @ProductId, @ItemNumber)", Conn)
            cmd.Parameters.AddWithValue("@Name", Name)
            cmd.Parameters.AddWithValue("@Company", Company)
            cmd.Parameters.AddWithValue("@Email", Email)
            cmd.Parameters.AddWithValue("@Address", Address)
            cmd.Parameters.AddWithValue("@Address1", Address1)
            cmd.Parameters.AddWithValue("@City", City)
            cmd.Parameters.AddWithValue("@State", State)
            cmd.Parameters.AddWithValue("@Zip", Zip)
            cmd.Parameters.AddWithValue("@Country", Country)
            cmd.Parameters.AddWithValue("@Phone", Phone)
            cmd.Parameters.AddWithValue("@Fax", Fax)
            cmd.Parameters.AddWithValue("@Message", Message)
            cmd.Parameters.AddWithValue("@ItemToPurchase", ItemToPurchase)
            cmd.Parameters.AddWithValue("@ItemsToSale", ItemsToSale)
            cmd.Parameters.AddWithValue("@IsNeedASAP", IsNeedASAP)
            cmd.Parameters.AddWithValue("@IsNeedFuture", IsNeedFuture)
            cmd.Parameters.AddWithValue("@IsAdd", IsAdd)
            cmd.Parameters.AddWithValue("@IsContact", IsContact)
            cmd.Parameters.AddWithValue("@Type", Type)
            cmd.Parameters.AddWithValue("@OptionPrice", OptionPrice)
            cmd.Parameters.AddWithValue("@Price", Price)
            cmd.Parameters.AddWithValue("@CardType", CardType)
            cmd.Parameters.AddWithValue("@PONumber", PONumber)
            cmd.Parameters.AddWithValue("@IsCheck", IsCheck)
            cmd.Parameters.AddWithValue("@IsOther", IsOther)
            cmd.Parameters.AddWithValue("@OrderDate", OrderDate)
            cmd.Parameters.AddWithValue("@ProductId", ProductId)
            cmd.Parameters.AddWithValue("@ItemNumber", ItemNumber)
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

    ''' <summary>Insert a record in the OrderHistory table from a DataRow</summary>
    ''' <param name="Row">The DataRow to be inserted in the OrderHistory table</param>
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
            cmd = New SqlCommand("INSERT INTO ""OrderHistory"" (" & setList & ") VALUES (" & valList & ")", conn)
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

    Public Function Delete(ByVal Id As Integer) As Boolean
        Dim cmd As SqlCommand
        Try
            Connect()
            cmd = New SqlCommand("DELETE FROM ""OrderHistory"" WHERE Id = @Id", Conn)
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
            cmd = New SqlCommand("DELETE FROM ""OrderHistory"" WHERE Id = @Id", Conn)
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
    ''' <summary></summary>
    ''' <returns>The column names from OrderHistory table separated by commas</returns>
    Public Overrides Function ToString() As String
        Dim p as String = ""
        p &= Cstr(Id) & ", "
        p &= Name & ", "
        p &= Company & ", "
        p &= Email & ", "
        p &= Address & ", "
        p &= Address1 & ", "
        p &= City & ", "
        p &= State & ", "
        p &= Zip & ", "
        p &= Country & ", "
        p &= Phone & ", "
        p &= Fax & ", "
        p &= Message & ", "
        p &= ItemToPurchase & ", "
        p &= ItemsToSale & ", "
        p &= Cstr(IsNeedASAP) & ", "
        p &= Cstr(IsNeedFuture) & ", "
        p &= Cstr(IsAdd) & ", "
        p &= Cstr(IsContact) & ", "
        p &= Type & ", "
        p &= OptionPrice & ", "
        p &= Cstr(Price) & ", "
        p &= CardType & ", "
        p &= PONumber & ", "
        p &= Cstr(IsCheck) & ", "
        p &= CStr(IsOther) & ", "
        p &= CStr(IsOther) & ", "
        p &= CStr(ProductId) & ", "
        p &= ItemNumber
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
