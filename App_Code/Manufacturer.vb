Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient

Public Class Manufacturer
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
    Public Name As String
    Public Address1 As String
    Public Address2 As String
    Public City As String
    Public State As String
    Public Zip As String
    Public Country As String
    Public ExternalSource As String
    Public ExternalID As Integer
    Public Phone As String
    Public FullAddress As String
    Public CompanyName As String
    Public Email As String
    Public PassWord As String
    Public Profile As String
    Public DateCreated As DateTime
    Public IsActive As Short
    Public Misspellings As String
    Public Alternative As String
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

        Public Sub New(ByRef _ColumnName As String, ByRef _DataType As String, ByVal _Length As Integer, ByVal _AllowInsert As Boolean, ByVal _AllowUpdate As Boolean)
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
        ColInfo.Add(New ColumnInfo("Name", "String", 150, True, True))
        ColInfo.Add(New ColumnInfo("Address1", "String", 30, True, True))
        ColInfo.Add(New ColumnInfo("Address2", "String", 30, True, True))
        ColInfo.Add(New ColumnInfo("City", "String", 25, True, True))
        ColInfo.Add(New ColumnInfo("State", "String", 5, True, True))
        ColInfo.Add(New ColumnInfo("Zip", "String", 10, True, True))
        ColInfo.Add(New ColumnInfo("Country", "String", 25, True, True))
        ColInfo.Add(New ColumnInfo("ExternalSource", "String", 50, True, True))
        ColInfo.Add(New ColumnInfo("ExternalID", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("Phone", "String", 20, True, True))
        ColInfo.Add(New ColumnInfo("FullAddress", "String", 200, True, True))
        ColInfo.Add(New ColumnInfo("CompanyName", "String", 200, True, True))
        ColInfo.Add(New ColumnInfo("Email", "String", 100, True, True))
        ColInfo.Add(New ColumnInfo("PassWord", "String", 50, True, True))
        ColInfo.Add(New ColumnInfo("Profile", "String", 20, True, True))
        ColInfo.Add(New ColumnInfo("DateCreated", "DateTime", 19, True, True))
        ColInfo.Add(New ColumnInfo("IsActive", "Short", 6, True, True))
        ColInfo.Add(New ColumnInfo("Misspellings", "String", 50, True, True))
        ColInfo.Add(New ColumnInfo("Alternative", "String", 50, True, True))
        ColInfo.Sort()
    End Sub

    ''' <summary></summary>
    ''' <param name="_Conn">Database connection object to be used in further operations</param>
    Public Sub New(ByRef _Conn As SqlConnection)
        Me.New()
        Conn = _Conn
    End Sub

    ''' <summary></summary>
    ''' <param name="_ConnectionString">Database connection string to be used in further operations</param>
    Public Sub New(ByRef _ConnectionString As String)
        Me.New()
        ConnectionString = _ConnectionString
    End Sub

#End Region

#Region " Methods "
    ''' <summary>Clear the local column variables associated with a Manufacturer table record</summary>
    Public Sub Blank()
        Id = 0
        Name = ""
        Address1 = ""
        Address2 = ""
        City = ""
        State = ""
        Zip = ""
        Country = ""
        ExternalSource = ""
        ExternalID = 0
        Phone = ""
        FullAddress = ""
        CompanyName = ""
        Email = ""
        PassWord = ""
        Profile = ""
        Misspellings = ""
        Alternative = ""
        DateCreated = Nothing
        IsActive = 0
        Valid = False
    End Sub

    ''' <summary>Get rows from the Manufacturer table</summary>
    ''' <param name="ColumnList">A list of column names in the Manufacturer table separated by commas</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the Manufacturer table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As String, ByRef WhereClause As String, ByRef OrderBy As String) As DataTable
        Dim cmd As SqlCommand
        Dim da As SqlDataAdapter
        Dim dt As New DataTable
        Dim ob As String = OrderBy

        Try
            Connect()
            If OrderBy <> "" Then ob = " ORDER BY " & ob
            cmd = New SqlCommand("SELECT " & columnlist & " FROM ""Manufacturer"" WHERE " & whereClause & ob, conn)
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

    ''' <summary>Get rows from the Manufacturer table</summary>
    ''' <param name="ColumnList">A list of column names in the Manufacturer table separated by commas</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the Manufacturer table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As String, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    ''' <summary>Get a rows from the Manufacturer table</summary>
    ''' <param name="ColumnList">An array of column names in the Manufacturer</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the Manufacturer table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList() As String, ByRef WhereClause As String, ByRef OrderBy As String) As DataTable
        Dim cl As String = ""
        Dim p As String

        For Each p In ColumnList
            If cl <> "" Then cl &= ", "
            cl &= p
        Next
        Return GetRows(cl, WhereClause, OrderBy)
    End Function

    ''' <summary>Get a rows from the Manufacturer table</summary>
    ''' <param name="ColumnList">An array of column names in the Manufacturer</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the Manufacturer table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList() As String, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    ''' <summary>Get a rows from the Manufacturer table</summary>
    ''' <param name="ColumnList">An ArrayList of Strings representing column names in the Manufacturer</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the Manufacturer table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As ArrayList, ByRef WhereClause As String, ByRef OrderBy As String) As DataTable
        Dim cl As String = ""
        Dim p As String

        For Each p In ColumnList
            If cl <> "" Then cl &= ", "
            cl &= p
        Next
        Return GetRows(cl, WhereClause, OrderBy)
    End Function

    ''' <summary>Get a rows from the Manufacturer table</summary>
    ''' <param name="ColumnList">An ArrayList of Strings representing column names in the Manufacturer</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the Manufacturer table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As ArrayList, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function
    Public Function Delete(ByVal Id As Integer) As Boolean
        Dim cmd As SqlCommand
        Try
            Connect()
            cmd = New SqlCommand("DELETE FROM ""Manufacturer"" WHERE Id = @Id", Conn)
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
            cmd = New SqlCommand("DELETE FROM ""Manufacturer"" WHERE Id = @Id", Conn)
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

    ''' <summary>Insert a record in the Manufacturer table from the data stored in the local variables</summary>
    ''' <returns>'True', if successful</returns>
    Public Function Insert() As Boolean
        Dim cmd As SqlCommand

        Try
            Connect()
            cmd = New SqlCommand("INSERT INTO ""Manufacturer"" (Name, Address1, Address2, City, State, Zip, Country, ExternalSource, ExternalID, Phone, FullAddress, CompanyName, Email, PassWord, Profile, DateCreated, Active) VALUES (@Name, @Address1, @Address2, @City, @State, @Zip, @Country, @ExternalSource, @ExternalID, @Phone, @FullAddress, @CompanyName, @Email, @PassWord, @Profile, @DateCreated, @Active)", conn)
            cmd.Parameters.AddWithValue("@Name", Name)
            cmd.Parameters.AddWithValue("@Address1", Address1)
            cmd.Parameters.AddWithValue("@Address2", Address2)
            cmd.Parameters.AddWithValue("@City", City)
            cmd.Parameters.AddWithValue("@State", State)
            cmd.Parameters.AddWithValue("@Zip", Zip)
            cmd.Parameters.AddWithValue("@Country", Country)
            cmd.Parameters.AddWithValue("@ExternalSource", ExternalSource)
            cmd.Parameters.AddWithValue("@ExternalID", ExternalID)
            cmd.Parameters.AddWithValue("@Phone", Phone)
            cmd.Parameters.AddWithValue("@FullAddress", FullAddress)
            cmd.Parameters.AddWithValue("@CompanyName", CompanyName)
            cmd.Parameters.AddWithValue("@Email", Email)
            cmd.Parameters.AddWithValue("@PassWord", PassWord)
            cmd.Parameters.AddWithValue("@Profile", Profile)
            cmd.Parameters.AddWithValue("@DateCreated", DateCreated)
            cmd.Parameters.AddWithValue("@Active", IsActive)
            cmd.ExecuteScalar()
            cmd.Dispose()

            REM Attempt to load the auto-generated Id. This is not fool-proof.
            Dim da As SqlDataAdapter
            Dim dt As New DataTable

            cmd = New SqlCommand("SELECT TOP 1 Id FROM ""Manufacturer"" WHERE Name = @Name AND Address1 = @Address1 AND Address2 = @Address2 AND City = @City AND State = @State AND Zip = @Zip AND Country = @Country AND ExternalSource = @ExternalSource AND ExternalID = @ExternalID AND Phone = @Phone AND FullAddress = @FullAddress AND CompanyName = @CompanyName AND Email = @Email AND PassWord = @PassWord AND Profile = @Profile AND DateCreated = @DateCreated AND Active = @Active ORDER BY Id DESC", conn)
            cmd.Parameters.AddWithValue("@Name", Name)
            cmd.Parameters.AddWithValue("@Address1", Address1)
            cmd.Parameters.AddWithValue("@Address2", Address2)
            cmd.Parameters.AddWithValue("@City", City)
            cmd.Parameters.AddWithValue("@State", State)
            cmd.Parameters.AddWithValue("@Zip", Zip)
            cmd.Parameters.AddWithValue("@Country", Country)
            cmd.Parameters.AddWithValue("@ExternalSource", ExternalSource)
            cmd.Parameters.AddWithValue("@ExternalID", ExternalID)
            cmd.Parameters.AddWithValue("@Phone", Phone)
            cmd.Parameters.AddWithValue("@FullAddress", FullAddress)
            cmd.Parameters.AddWithValue("@CompanyName", CompanyName)
            cmd.Parameters.AddWithValue("@Email", Email)
            cmd.Parameters.AddWithValue("@PassWord", PassWord)
            cmd.Parameters.AddWithValue("@Profile", Profile)
            cmd.Parameters.AddWithValue("@DateCreated", DateCreated)
            cmd.Parameters.AddWithValue("@Active", IsActive)
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
    Public Function InsertIntoManufacturer() As Integer
        Dim cmd As SqlCommand
        Dim checkInsert As Integer = 0

        Try
            Connect()
            cmd = New SqlCommand("INSERT INTO ""Manufacturer"" (Name, IsActive) VALUES (@Name, @IsActive)", Conn)
            cmd.Parameters.AddWithValue("@Name", Name)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
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
    ''' <summary>Insert a record in the Manufacturer table from a DataRow</summary>
    ''' <param name="Row">The DataRow to be inserted in the Manufacturer table</param>
    ''' <returns>'True', if successful</returns>
    ''' <remarks>The DataRow must contain all primary key columns</remarks>
    Public Function Insert(ByRef Row As DataRow) As Boolean
        Dim cmd As SqlCommand
        Dim col As DataColumn
        Dim setList As String = ""
        Dim valList As String = ""

        Try
            Connect()
            REM Build the set list
            For Each col In Row.Table.Columns
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
            cmd = New SqlCommand("INSERT INTO ""Manufacturer"" (" & setList & ") VALUES (" & valList & ")", conn)
            REM Create the parameters
            For Each col In Row.Table.Columns
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
            cmd = New SqlCommand("UPDATE ""Manufacturer"" SET Name = @Name ,IsActive = @IsActive  WHERE Id = @Id", Conn)
            cmd.Parameters.AddWithValue("@Id", Id)
            cmd.Parameters.AddWithValue("@Name", Name)
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
    Public Function UpdateManufacturerStatus() As Boolean
        Try
            Connect()
            Dim cmd As SqlCommand
            cmd = New SqlCommand("UPDATE ""Manufacturer"" SET IsActive = @IsActive WHERE Id = @Id", Conn)
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
    ''' <summary>Update the Customer table from a DataRow</summary>
    ''' <param name="Row">The DataRow to be updated in the Customer table</param>
    ''' <returns>'True', if successful</returns>
    ''' <remarks>The DataRow must contain all primary key columns</remarks>

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
            cmd = New SqlCommand("UPDATE ""Manufacturer"" SET " & setList & " WHERE Id = @Id", Conn)
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
    ''' <returns>The column names from Manufacturer table separated by commas</returns>
    Public Overrides Function ToString() As String
        Dim p As String = ""
        p &= CStr(Id) & ", "
        p &= Name & ", "
        p &= Address1 & ", "
        p &= Address2 & ", "
        p &= City & ", "
        p &= State & ", "
        p &= Zip & ", "
        p &= Country & ", "
        p &= ExternalSource & ", "
        p &= CStr(ExternalID) & ", "
        p &= Phone & ", "
        p &= FullAddress & ", "
        p &= CompanyName & ", "
        p &= Email & ", "
        p &= PassWord & ", "
        p &= Misspellings & ""
        p &= Alternative & ""
        p &= Profile & ", "
        p &= DateCreated.ToString() & ", "
        p &= CStr(IsActive)
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
