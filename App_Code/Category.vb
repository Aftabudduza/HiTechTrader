Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient

Public Class Category
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
    Public IsMainorLabX As Integer
    Public CategoryName As String
    Public Catlevel As Integer
    Public CategoryParentId As Integer
    Public NoItems As Integer
    Public lineAge As String
    Public IsActive As Short
    Public MetaDescription As String
    Public MetaKeywords As String
    Public IsLabX As Short
    Public LabXCategory As Integer
    Public IsProductCategory As Short
    Public LabXParentId As Integer

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
        ColInfo.Add(New ColumnInfo("IsMainorLabX", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("CategoryName", "String", 100, True, True))
        ColInfo.Add(New ColumnInfo("Catlevel", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("CategoryParentId", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("NoItems", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("lineAge", "String", 200, True, True))
        ColInfo.Add(New ColumnInfo("IsActive", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("MetaDescription", "String", 200, True, True))
        ColInfo.Add(New ColumnInfo("MetaKeywords", "String", 200, True, True))
        ColInfo.Add(New ColumnInfo("IsLabX", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("LabXCategory", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("IsProductCategory", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("LabXParentId", "Integer", 11, True, True))
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
    ''' <summary>Clear the local column variables associated with a Category table record</summary>
    Public Sub Blank()
        Id = 0
        IsMainorLabX = 0
        CategoryName = ""
        Catlevel = 0
        CategoryParentId = 0
        NoItems = 0
        lineAge = ""
        IsActive = 0
        MetaDescription = ""
        MetaKeywords = ""
        IsLabX = 0
        LabXCategory = 0
        IsProductCategory = 0
        LabXParentId = 0
        Valid = False
    End Sub

    ''' <summary>Get rows from the Category table</summary>
    ''' <param name="ColumnList">A list of column names in the Category table separated by commas</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the Category table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As String, ByRef WhereClause As String, ByRef OrderBy As String) As DataTable
        Dim cmd as SqlCommand
        Dim da as SqlDataAdapter
        Dim dt as New DataTable
        Dim ob as String = OrderBy

        Try
            Connect()
            If OrderBy <> "" Then ob = " ORDER BY " & ob
            cmd = New SqlCommand("SELECT " & columnlist & " FROM ""Category"" WHERE " & whereClause & ob, conn)
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

    ''' <summary>Get rows from the Category table</summary>
    ''' <param name="ColumnList">A list of column names in the Category table separated by commas</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the Category table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As String, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    ''' <summary>Get a rows from the Category table</summary>
    ''' <param name="ColumnList">An array of column names in the Category</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the Category table</returns>
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

    ''' <summary>Get a rows from the Category table</summary>
    ''' <param name="ColumnList">An array of column names in the Category</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the Category table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList() As String, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    ''' <summary>Get a rows from the Category table</summary>
    ''' <param name="ColumnList">An ArrayList of Strings representing column names in the Category</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the Category table</returns>
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

    ''' <summary>Get a rows from the Category table</summary>
    ''' <param name="ColumnList">An ArrayList of Strings representing column names in the Category</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the Category table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As ArrayList, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    Public Function Delete(ByVal Id As Integer) As Boolean
        Dim cmd As SqlCommand
        Try
            Connect()
            cmd = New SqlCommand("DELETE FROM ""Category"" WHERE Id = @Id", Conn)
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
            cmd = New SqlCommand("DELETE FROM ""Category"" WHERE Id = @Id", Conn)
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
    ''' <summary>Insert a record in the Category table from the data stored in the local variables</summary>
    ''' <returns>'True', if successful</returns>
    Public Function Insert() As Boolean
        Dim cmd as SqlCommand

        Try
            Connect()
            cmd = New SqlCommand("INSERT INTO ""Category"" (IsMainorLabX, CategoryName, Catlevel, CategoryParentId, NoItems, lineAge, IsActive, MetaDescription, MetaKeywords, IsLabX, LabXCategory, IsProductCategory) VALUES (@IsMainorLabX, @CategoryName, @Catlevel, @CategoryParentId, @NoItems, @lineAge, @IsActive, @MetaDescription, @MetaKeywords, @IsLabX, @LabXCategory, @IsProductCategory)", Conn)
            cmd.Parameters.AddWithValue("@IsMainorLabX", IsMainorLabX)
            cmd.Parameters.AddWithValue("@CategoryName", CategoryName)
            cmd.Parameters.AddWithValue("@Catlevel", Catlevel)
            cmd.Parameters.AddWithValue("@CategoryParentId", CategoryParentId)
            cmd.Parameters.AddWithValue("@NoItems", NoItems)
            cmd.Parameters.AddWithValue("@lineAge", lineAge)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
            cmd.Parameters.AddWithValue("@MetaDescription", MetaDescription)
            cmd.Parameters.AddWithValue("@MetaKeywords", MetaKeywords)
            cmd.Parameters.AddWithValue("@IsLabX", IsLabX)
            cmd.Parameters.AddWithValue("@LabXCategory", LabXCategory)
            cmd.Parameters.AddWithValue("@IsProductCategory", IsProductCategory)
            cmd.ExecuteScalar()
            cmd.Dispose()

            REM Attempt to load the auto-generated Id. This is not fool-proof.
            Dim da as SqlDataAdapter
            Dim dt as New DataTable

            cmd = New SqlCommand("SELECT TOP 1 Id FROM ""Category"" WHERE IsMainorLabX = @IsMainorLabX AND CategoryName = @CategoryName AND Catlevel = @Catlevel AND CategoryParentId = @CategoryParentId AND NoItems = @NoItems AND lineAge = @lineAge AND IsActive = @IsActive AND MetaDescription = @MetaDescription AND MetaKeywords = @MetaKeywords AND IsLabX = @IsLabX AND LabXCategory = @LabXCategory AND IsProductCategory = @IsProductCategory ORDER BY Id DESC", Conn)
            cmd.Parameters.AddWithValue("@IsMainorLabX", IsMainorLabX)
            cmd.Parameters.AddWithValue("@CategoryName", CategoryName)
            cmd.Parameters.AddWithValue("@Catlevel", Catlevel)
            cmd.Parameters.AddWithValue("@CategoryParentId", CategoryParentId)
            cmd.Parameters.AddWithValue("@NoItems", NoItems)
            cmd.Parameters.AddWithValue("@lineAge", lineAge)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
            cmd.Parameters.AddWithValue("@MetaDescription", MetaDescription)
            cmd.Parameters.AddWithValue("@MetaKeywords", MetaKeywords)
            cmd.Parameters.AddWithValue("@IsLabX", IsLabX)
            cmd.Parameters.AddWithValue("@LabXCategory", LabXCategory)
            cmd.Parameters.AddWithValue("@IsProductCategory", IsProductCategory)
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
    Public Function InsertIntoCategory() As Integer
        Dim cmd As SqlCommand
        Dim checkInsert As Integer = 0

        Try
            Connect()
            cmd = New SqlCommand("INSERT INTO ""Category"" (IsMainorLabX, CategoryName, Catlevel, CategoryParentId, NoItems, lineAge, IsActive, MetaDescription, MetaKeywords, IsLabX, LabXCategory, IsProductCategory,LabXParentId) VALUES (@IsMainorLabX, @CategoryName, @Catlevel, @CategoryParentId, @NoItems, @lineAge, @IsActive, @MetaDescription, @MetaKeywords, @IsLabX, @LabXCategory, @IsProductCategory, @LabXParentId)", Conn)
            cmd.Parameters.AddWithValue("@IsMainorLabX", IsMainorLabX)
            cmd.Parameters.AddWithValue("@CategoryName", CategoryName)
            cmd.Parameters.AddWithValue("@Catlevel", Catlevel)
            cmd.Parameters.AddWithValue("@CategoryParentId", CategoryParentId)
            cmd.Parameters.AddWithValue("@NoItems", NoItems)
            cmd.Parameters.AddWithValue("@lineAge", lineAge)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
            cmd.Parameters.AddWithValue("@MetaDescription", MetaDescription)
            cmd.Parameters.AddWithValue("@MetaKeywords", MetaKeywords)
            cmd.Parameters.AddWithValue("@IsLabX", IsLabX)
            cmd.Parameters.AddWithValue("@LabXCategory", LabXCategory)
            cmd.Parameters.AddWithValue("@IsProductCategory", IsProductCategory)
            cmd.Parameters.AddWithValue("@LabXParentId", LabXParentId)
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
    ''' <summary>Insert a record in the Category table from a DataRow</summary>
    ''' <param name="Row">The DataRow to be inserted in the Category table</param>
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
            cmd = New SqlCommand("INSERT INTO ""Category"" (" & setList & ") VALUES (" & valList & ")", conn)
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
            cmd = New SqlCommand("UPDATE ""Category"" SET IsMainorLabX = @IsMainorLabX , CategoryName = @CategoryName , Catlevel = @Catlevel , CategoryParentId = @CategoryParentId , NoItems = @NoItems , lineAge = @lineAge , IsActive = @IsActive , MetaDescription = @MetaDescription , MetaKeywords = @MetaKeywords , IsLabX = @IsLabX , LabXCategory = @LabXCategory , IsProductCategory = @IsProductCategory ,LabXParentId=@LabXParentId  WHERE Id = @Id", Conn)
            cmd.Parameters.AddWithValue("@Id", Id)
            cmd.Parameters.AddWithValue("@IsMainorLabX", IsMainorLabX)
            cmd.Parameters.AddWithValue("@CategoryName", CategoryName)
            cmd.Parameters.AddWithValue("@Catlevel", Catlevel)
            cmd.Parameters.AddWithValue("@CategoryParentId", CategoryParentId)
            cmd.Parameters.AddWithValue("@NoItems", NoItems)
            cmd.Parameters.AddWithValue("@lineAge", lineAge)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
            cmd.Parameters.AddWithValue("@MetaDescription", MetaDescription)
            cmd.Parameters.AddWithValue("@MetaKeywords", MetaKeywords)
            cmd.Parameters.AddWithValue("@IsLabX", IsLabX)
            cmd.Parameters.AddWithValue("@LabXCategory", LabXCategory)
            cmd.Parameters.AddWithValue("@IsProductCategory", IsProductCategory)
            cmd.Parameters.AddWithValue("@LabXParentId", LabXParentId)
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

   
    ''' <remarks>The DataRow must contain all primary key columns</remarks>
    Public Function UpdateCategoryStatus() As Boolean
        Try
            Connect()
            Dim cmd As SqlCommand
            cmd = New SqlCommand("UPDATE ""Category"" SET IsActive = @IsActive WHERE Id = @Id", Conn)
            cmd.Parameters.AddWithValue("@Id", Id)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
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
            cmd = New SqlCommand("UPDATE ""Category"" SET " & setList & " WHERE Id = @Id", Conn)
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
    ''' <returns>The column names from Category table separated by commas</returns>
    Public Overrides Function ToString() As String
        Dim p as String = ""
        p &= Cstr(Id) & ", "
        p &= CStr(IsMainorLabX) & ", "
        p &= CategoryName & ", "
        p &= Cstr(Catlevel) & ", "
        p &= CStr(CategoryParentId) & ", "
        p &= Cstr(NoItems) & ", "
        p &= lineAge & ", "
        p &= Cstr(IsActive) & ", "
        p &= MetaDescription & ", "
        p &= MetaKeywords & ", "
        p &= Cstr(IsLabX) & ", "
        p &= CStr(LabXCategory) & ", "
        p &= CStr(LabXParentId) & ", "
        p &= Cstr(IsProductCategory)
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
