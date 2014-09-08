Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient

Public Class UserInfo
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
    Public Email As String
    Public Password As String
    Public UserName As String
    Public FirstName As String
    Public LastName As String
    Public Address1 As String
    Public Address2 As String
    Public City As String
    Public State As String
    Public ZipCode As Integer
    Public CellPhone As String
    Public HomePhone As String
    Public OfficePhone As String
    Public SubContractorId As String
    Public SubContractorName As String
    Public TypeOfUser As String
    Public IsClientDesktop As Boolean
    Public IsSubContractor As Boolean
    Public IsProfile As Boolean
    Public IsUserInformation As Boolean
    Public IsComplianceDesktopArea As Boolean
    Public IsReports As Boolean
    Public IsAdmin As Boolean
    Public IsActive As Boolean
    Public IsLogin As Boolean
    Public FilePath As String
    Public IsSuperAdmin As Boolean
    Public CreatedBy As Integer
    Public CreatedDate As DateTime
    Public ModifiedBy As Integer
    Public ModifiedDate As DateTime
    Public UserPermission As Integer
    Public ClientId As Integer
    Public IsSiteAdministrator As Boolean
    Public IsThirdPartyUser As Boolean
    Public Status As Integer
    Public ReleaseAds As String
    Public ItemNo As Integer

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
        ColInfo.Add(New ColumnInfo("Email", "String", 30, True, True))
        ColInfo.Add(New ColumnInfo("Password", "String", 100, True, True))
        ColInfo.Add(New ColumnInfo("UserName", "String", 20, True, True))
        ColInfo.Add(New ColumnInfo("FirstName", "String", 30, True, True))
        ColInfo.Add(New ColumnInfo("LastName", "String", 30, True, True))
        ColInfo.Add(New ColumnInfo("Address1", "String", 100, True, True))
        ColInfo.Add(New ColumnInfo("Address2", "String", 100, True, True))
        ColInfo.Add(New ColumnInfo("City", "String", 30, True, True))
        ColInfo.Add(New ColumnInfo("State", "String", 2, True, True))
        ColInfo.Add(New ColumnInfo("ZipCode", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("CellPhone", "String", 10, True, True))
        ColInfo.Add(New ColumnInfo("HomePhone", "String", 10, True, True))
        ColInfo.Add(New ColumnInfo("OfficePhone", "String", 10, True, True))
        ColInfo.Add(New ColumnInfo("SubContractorId", "String", 40, True, True))
        ColInfo.Add(New ColumnInfo("SubContractorName", "String", 40, True, True))
        ColInfo.Add(New ColumnInfo("TypeOfUser", "String", 20, True, True))
        ColInfo.Add(New ColumnInfo("IsClientDesktop", "Boolean", 1, True, True))
        ColInfo.Add(New ColumnInfo("IsSubContractor", "Boolean", 1, True, True))
        ColInfo.Add(New ColumnInfo("IsProfile", "Boolean", 1, True, True))
        ColInfo.Add(New ColumnInfo("IsUserInformation", "Boolean", 1, True, True))
        ColInfo.Add(New ColumnInfo("IsComplianceDesktopArea", "Boolean", 1, True, True))
        ColInfo.Add(New ColumnInfo("IsReports", "Boolean", 1, True, True))
        ColInfo.Add(New ColumnInfo("IsAdmin", "Boolean", 1, True, True))
        ColInfo.Add(New ColumnInfo("IsActive", "Boolean", 1, True, True))
        ColInfo.Add(New ColumnInfo("IsLogin", "Boolean", 1, True, True))
        ColInfo.Add(New ColumnInfo("FilePath", "String", 50, True, True))
        ColInfo.Add(New ColumnInfo("IsSuperAdmin", "Boolean", 1, True, True))
        ColInfo.Add(New ColumnInfo("CreatedBy", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("CreatedDate", "DateTime", 19, True, True))
        ColInfo.Add(New ColumnInfo("ModifiedBy", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("ModifiedDate", "DateTime", 19, True, True))
        ColInfo.Add(New ColumnInfo("UserPermission", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("ClientId", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("IsSiteAdministrator", "Boolean", 1, True, True))
        ColInfo.Add(New ColumnInfo("IsThirdPartyUser", "Boolean", 1, True, True))
        ColInfo.Add(New ColumnInfo("Status", "Integer", 11, True, True))
        ColInfo.Add(New ColumnInfo("ReleaseAds", "String", 10, True, True))
        ColInfo.Add(New ColumnInfo("ItemNo", "Integer", 11, True, True))
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
    ''' <summary>Clear the local column variables associated with a UserInfo table record</summary>
    Public Sub Blank()
        Id = 0
        Email = ""
        Password = ""
        UserName = ""
        FirstName = ""
        LastName = ""
        Address1 = ""
        Address2 = ""
        City = ""
        State = ""
        ZipCode = 0
        CellPhone = ""
        HomePhone = ""
        OfficePhone = ""
        SubContractorId = ""
        SubContractorName = ""
        TypeOfUser = ""
        IsClientDesktop = false
        IsSubContractor = false
        IsProfile = false
        IsUserInformation = false
        IsComplianceDesktopArea = false
        IsReports = false
        IsAdmin = false
        IsActive = false
        IsLogin = false
        FilePath = ""
        IsSuperAdmin = false
        CreatedBy = 0
        CreatedDate = Nothing
        ModifiedBy = 0
        ModifiedDate = Nothing
        UserPermission = 0
        ClientId = 0
        IsSiteAdministrator = false
        IsThirdPartyUser = false
        Status = 0
        ReleaseAds = ""
        ItemNo = 0
        Valid = False
    End Sub

    ''' <summary>Get rows from the UserInfo table</summary>
    ''' <param name="ColumnList">A list of column names in the UserInfo table separated by commas</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the UserInfo table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As String, ByRef WhereClause As String, ByRef OrderBy As String) As DataTable
        Dim cmd as SqlCommand
        Dim da as SqlDataAdapter
        Dim dt as New DataTable
        Dim ob as String = OrderBy

        Try
            Connect()
            If OrderBy <> "" Then ob = " ORDER BY " & ob
            cmd = New SqlCommand("SELECT " & columnlist & " FROM ""UserInfo"" WHERE " & whereClause & ob, conn)
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

    ''' <summary>Get rows from the UserInfo table</summary>
    ''' <param name="ColumnList">A list of column names in the UserInfo table separated by commas</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the UserInfo table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As String, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    ''' <summary>Get a rows from the UserInfo table</summary>
    ''' <param name="ColumnList">An array of column names in the UserInfo</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the UserInfo table</returns>
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

    ''' <summary>Get a rows from the UserInfo table</summary>
    ''' <param name="ColumnList">An array of column names in the UserInfo</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the UserInfo table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList() As String, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function

    ''' <summary>Get a rows from the UserInfo table</summary>
    ''' <param name="ColumnList">An ArrayList of Strings representing column names in the UserInfo</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <param name="OrderBy">An SQL ORDER BY clause for a SELECT statement</param>
    ''' <returns>A DataTable from the UserInfo table</returns>
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

    ''' <summary>Get a rows from the UserInfo table</summary>
    ''' <param name="ColumnList">An ArrayList of Strings representing column names in the UserInfo</param>
    ''' <param name="WhereClause">An SQL WHERE clause for a SELECT statement that may return multiple rows</param>
    ''' <returns>A DataTable from the UserInfo table</returns>
    ''' <remarks>Use care with web forms since the resulting SELECT statement is not parameterized</remarks>
    Public Function GetRows(ByRef ColumnList As ArrayList, ByRef WhereClause As String) As DataTable
        Return GetRows(ColumnList, WhereClause, "")
    End Function
    Public Function Delete(ByVal Id As Integer) As Boolean
        Dim cmd As SqlCommand
        Try
            Connect()
            cmd = New SqlCommand("DELETE FROM ""UserInfo"" WHERE Id = @Id", Conn)
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
            cmd = New SqlCommand("DELETE FROM ""UserInfo"" WHERE Id = @Id", Conn)
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
   

    ''' <summary>Insert a record in the UserInfo table from the data stored in the local variables</summary>
    ''' <returns>'True', if successful</returns>
    Public Function Insert() As Boolean
        Dim cmd as SqlCommand

        Try
            Connect()
            cmd = New SqlCommand("INSERT INTO ""UserInfo"" (Email, Password, UserName, FirstName, LastName, Address1, Address2, City, State, ZipCode, CellPhone, HomePhone, OfficePhone, SubContractorId, SubContractorName, TypeOfUser, IsClientDesktop, IsSubContractor, IsProfile, IsUserInformation, IsComplianceDesktopArea, IsReports, IsAdmin, IsActive, IsLogin, FilePath, IsSuperAdmin, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, UserPermission, ClientId, IsSiteAdministrator, IsThirdPartyUser, Status, ReleaseAds, ItemNo) VALUES (@Email, @Password, @UserName, @FirstName, @LastName, @Address1, @Address2, @City, @State, @ZipCode, @CellPhone, @HomePhone, @OfficePhone, @SubContractorId, @SubContractorName, @TypeOfUser, @IsClientDesktop, @IsSubContractor, @IsProfile, @IsUserInformation, @IsComplianceDesktopArea, @IsReports, @IsAdmin, @IsActive, @IsLogin, @FilePath, @IsSuperAdmin, @CreatedBy, @CreatedDate, @ModifiedBy, @ModifiedDate, @UserPermission, @ClientId, @IsSiteAdministrator, @IsThirdPartyUser, @Status, @ReleaseAds, @ItemNo)", conn)
            cmd.Parameters.AddWithValue("@Email", Email)
            cmd.Parameters.AddWithValue("@Password", Password)
            cmd.Parameters.AddWithValue("@UserName", UserName)
            cmd.Parameters.AddWithValue("@FirstName", FirstName)
            cmd.Parameters.AddWithValue("@LastName", LastName)
            cmd.Parameters.AddWithValue("@Address1", Address1)
            cmd.Parameters.AddWithValue("@Address2", Address2)
            cmd.Parameters.AddWithValue("@City", City)
            cmd.Parameters.AddWithValue("@State", State)
            cmd.Parameters.AddWithValue("@ZipCode", ZipCode)
            cmd.Parameters.AddWithValue("@CellPhone", CellPhone)
            cmd.Parameters.AddWithValue("@HomePhone", HomePhone)
            cmd.Parameters.AddWithValue("@OfficePhone", OfficePhone)
            cmd.Parameters.AddWithValue("@SubContractorId", SubContractorId)
            cmd.Parameters.AddWithValue("@SubContractorName", SubContractorName)
            cmd.Parameters.AddWithValue("@TypeOfUser", TypeOfUser)
            cmd.Parameters.AddWithValue("@IsClientDesktop", IsClientDesktop)
            cmd.Parameters.AddWithValue("@IsSubContractor", IsSubContractor)
            cmd.Parameters.AddWithValue("@IsProfile", IsProfile)
            cmd.Parameters.AddWithValue("@IsUserInformation", IsUserInformation)
            cmd.Parameters.AddWithValue("@IsComplianceDesktopArea", IsComplianceDesktopArea)
            cmd.Parameters.AddWithValue("@IsReports", IsReports)
            cmd.Parameters.AddWithValue("@IsAdmin", IsAdmin)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
            cmd.Parameters.AddWithValue("@IsLogin", IsLogin)
            cmd.Parameters.AddWithValue("@FilePath", FilePath)
            cmd.Parameters.AddWithValue("@IsSuperAdmin", IsSuperAdmin)
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate)
            cmd.Parameters.AddWithValue("@ModifiedBy", ModifiedBy)
            cmd.Parameters.AddWithValue("@ModifiedDate", ModifiedDate)
            cmd.Parameters.AddWithValue("@UserPermission", UserPermission)
            cmd.Parameters.AddWithValue("@ClientId", ClientId)
            cmd.Parameters.AddWithValue("@IsSiteAdministrator", IsSiteAdministrator)
            cmd.Parameters.AddWithValue("@IsThirdPartyUser", IsThirdPartyUser)
            cmd.Parameters.AddWithValue("@Status", Status)
            cmd.Parameters.AddWithValue("@ReleaseAds", ReleaseAds)
            cmd.Parameters.AddWithValue("@ItemNo", ItemNo)
            cmd.ExecuteScalar()
            cmd.Dispose()

            REM Attempt to load the auto-generated Id. This is not fool-proof.
            Dim da as SqlDataAdapter
            Dim dt as New DataTable

            cmd = New SqlCommand("SELECT TOP 1 Id FROM ""UserInfo"" WHERE Email = @Email AND Password = @Password AND UserName = @UserName AND FirstName = @FirstName AND LastName = @LastName AND Address1 = @Address1 AND Address2 = @Address2 AND City = @City AND State = @State AND ZipCode = @ZipCode AND CellPhone = @CellPhone AND HomePhone = @HomePhone AND OfficePhone = @OfficePhone AND SubContractorId = @SubContractorId AND SubContractorName = @SubContractorName AND TypeOfUser = @TypeOfUser AND IsClientDesktop = @IsClientDesktop AND IsSubContractor = @IsSubContractor AND IsProfile = @IsProfile AND IsUserInformation = @IsUserInformation AND IsComplianceDesktopArea = @IsComplianceDesktopArea AND IsReports = @IsReports AND IsAdmin = @IsAdmin AND IsActive = @IsActive AND IsLogin = @IsLogin AND FilePath = @FilePath AND IsSuperAdmin = @IsSuperAdmin AND CreatedBy = @CreatedBy AND CreatedDate = @CreatedDate AND ModifiedBy = @ModifiedBy AND ModifiedDate = @ModifiedDate AND UserPermission = @UserPermission AND ClientId = @ClientId AND IsSiteAdministrator = @IsSiteAdministrator AND IsThirdPartyUser = @IsThirdPartyUser AND Status = @Status AND ReleaseAds = @ReleaseAds AND ItemNo = @ItemNo ORDER BY Id DESC", conn)
            cmd.Parameters.AddWithValue("@Email", Email)
            cmd.Parameters.AddWithValue("@Password", Password)
            cmd.Parameters.AddWithValue("@UserName", UserName)
            cmd.Parameters.AddWithValue("@FirstName", FirstName)
            cmd.Parameters.AddWithValue("@LastName", LastName)
            cmd.Parameters.AddWithValue("@Address1", Address1)
            cmd.Parameters.AddWithValue("@Address2", Address2)
            cmd.Parameters.AddWithValue("@City", City)
            cmd.Parameters.AddWithValue("@State", State)
            cmd.Parameters.AddWithValue("@ZipCode", ZipCode)
            cmd.Parameters.AddWithValue("@CellPhone", CellPhone)
            cmd.Parameters.AddWithValue("@HomePhone", HomePhone)
            cmd.Parameters.AddWithValue("@OfficePhone", OfficePhone)
            cmd.Parameters.AddWithValue("@SubContractorId", SubContractorId)
            cmd.Parameters.AddWithValue("@SubContractorName", SubContractorName)
            cmd.Parameters.AddWithValue("@TypeOfUser", TypeOfUser)
            cmd.Parameters.AddWithValue("@IsClientDesktop", IsClientDesktop)
            cmd.Parameters.AddWithValue("@IsSubContractor", IsSubContractor)
            cmd.Parameters.AddWithValue("@IsProfile", IsProfile)
            cmd.Parameters.AddWithValue("@IsUserInformation", IsUserInformation)
            cmd.Parameters.AddWithValue("@IsComplianceDesktopArea", IsComplianceDesktopArea)
            cmd.Parameters.AddWithValue("@IsReports", IsReports)
            cmd.Parameters.AddWithValue("@IsAdmin", IsAdmin)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
            cmd.Parameters.AddWithValue("@IsLogin", IsLogin)
            cmd.Parameters.AddWithValue("@FilePath", FilePath)
            cmd.Parameters.AddWithValue("@IsSuperAdmin", IsSuperAdmin)
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate)
            cmd.Parameters.AddWithValue("@ModifiedBy", ModifiedBy)
            cmd.Parameters.AddWithValue("@ModifiedDate", ModifiedDate)
            cmd.Parameters.AddWithValue("@UserPermission", UserPermission)
            cmd.Parameters.AddWithValue("@ClientId", ClientId)
            cmd.Parameters.AddWithValue("@IsSiteAdministrator", IsSiteAdministrator)
            cmd.Parameters.AddWithValue("@IsThirdPartyUser", IsThirdPartyUser)
            cmd.Parameters.AddWithValue("@Status", Status)
            cmd.Parameters.AddWithValue("@ReleaseAds", ReleaseAds)
            cmd.Parameters.AddWithValue("@ItemNo", ItemNo)
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
    Public Function InsertIntoUserInfo() As Integer
        Dim cmd As SqlCommand
        Dim checkInsert As Integer = 0

        Try
            Connect()
            cmd = New SqlCommand("INSERT INTO ""UserInfo"" (Email, UserName,Password, CreatedBy, CreatedDate, UserPermission, Status, ReleaseAds,ItemNo) VALUES (@Email, @UserName,@Password, @CreatedBy, @CreatedDate, @UserPermission, @Status, @ReleaseAds, @ItemNo)", Conn)
            cmd.Parameters.AddWithValue("@Email", Email)
            cmd.Parameters.AddWithValue("@UserName", UserName)
            cmd.Parameters.AddWithValue("@Password", Password)
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate)
            cmd.Parameters.AddWithValue("@UserPermission", UserPermission)
            cmd.Parameters.AddWithValue("@Status", Status)
            cmd.Parameters.AddWithValue("@ReleaseAds", ReleaseAds)
            cmd.Parameters.AddWithValue("@ItemNo", ItemNo)
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
    ''' <summary>Insert a record in the UserInfo table from a DataRow</summary>
    ''' <param name="Row">The DataRow to be inserted in the UserInfo table</param>
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
            cmd = New SqlCommand("INSERT INTO ""UserInfo"" (" & setList & ") VALUES (" & valList & ")", conn)
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
            cmd = New SqlCommand("UPDATE ""UserInfo"" SET Email = @Email , UserName = @UserName ,Password = @Password, ModifiedBy = @ModifiedBy , ModifiedDate = @ModifiedDate , UserPermission = @UserPermission , Status = @Status , ReleaseAds = @ReleaseAds, ItemNo = @ItemNo  WHERE Id = @Id", Conn)
            cmd.Parameters.AddWithValue("@Id", Id)
            cmd.Parameters.AddWithValue("@Email", Email)
            cmd.Parameters.AddWithValue("@UserName", UserName)
            cmd.Parameters.AddWithValue("@Password", Password)
            cmd.Parameters.AddWithValue("@ModifiedBy", ModifiedBy)
            cmd.Parameters.AddWithValue("@ModifiedDate", ModifiedDate)
            cmd.Parameters.AddWithValue("@UserPermission", UserPermission)
            cmd.Parameters.AddWithValue("@Status", Status)
            cmd.Parameters.AddWithValue("@ReleaseAds", ReleaseAds)
            cmd.Parameters.AddWithValue("@ItemNo", ItemNo)
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
    Public Function UpdateUserStatus() As Boolean
        Try
            Connect()
            Dim cmd As SqlCommand
            cmd = New SqlCommand("UPDATE ""UserInfo"" SET Status = @Status WHERE Id = @Id", Conn)
            cmd.Parameters.AddWithValue("@Id", Id)
            cmd.Parameters.AddWithValue("@Status", Status)
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
            cmd = New SqlCommand("UPDATE ""UserInfo"" SET " & setList & " WHERE Id = @Id", Conn)
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
    ''' <returns>The column names from UserInfo table separated by commas</returns>
    Public Overrides Function ToString() As String
        Dim p as String = ""
        p &= Cstr(Id) & ", "
        p &= Email & ", "
        p &= Password & ", "
        p &= UserName & ", "
        p &= FirstName & ", "
        p &= LastName & ", "
        p &= Address1 & ", "
        p &= Address2 & ", "
        p &= City & ", "
        p &= State & ", "
        p &= Cstr(ZipCode) & ", "
        p &= CellPhone & ", "
        p &= HomePhone & ", "
        p &= OfficePhone & ", "
        p &= SubContractorId & ", "
        p &= SubContractorName & ", "
        p &= TypeOfUser & ", "
        p &= Cstr(IsClientDesktop) & ", "
        p &= Cstr(IsSubContractor) & ", "
        p &= Cstr(IsProfile) & ", "
        p &= Cstr(IsUserInformation) & ", "
        p &= Cstr(IsComplianceDesktopArea) & ", "
        p &= Cstr(IsReports) & ", "
        p &= Cstr(IsAdmin) & ", "
        p &= Cstr(IsActive) & ", "
        p &= Cstr(IsLogin) & ", "
        p &= FilePath & ", "
        p &= Cstr(IsSuperAdmin) & ", "
        p &= Cstr(CreatedBy) & ", "
        p &= CreatedDate.ToString() & ", "
        p &= Cstr(ModifiedBy) & ", "
        p &= ModifiedDate.ToString() & ", "
        p &= Cstr(UserPermission) & ", "
        p &= Cstr(ClientId) & ", "
        p &= Cstr(IsSiteAdministrator) & ", "
        p &= Cstr(IsThirdPartyUser) & ", "
        p &= CStr(Status) & ", "
        p &= ReleaseAds & ", "
        p &= Cstr(ItemNo)
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
