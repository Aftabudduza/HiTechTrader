Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class SQLData

    Public Shared Function ConnectionString() As String
        Return System.Configuration.ConfigurationManager.AppSettings("ConnectionString")
    End Function
#Region "DB FUNCTIONS"

    Public Shared Function fix_dataset_nulls(ByVal ds As DataSet) As DataSet
        'fix nulls in dataset
        For Each dr As DataRow In ds.Tables(0).Rows
            For Each col As DataColumn In ds.Tables(0).Columns
                If IsDBNull(dr.Item(col.ColumnName)) Then
                    Select Case col.DataType.ToString
                        Case "System.String"
                            dr.Item(col.ColumnName) = ""
                        Case "System.Int32"
                            dr.Item(col.ColumnName) = 0
                        Case "System.DateTime"
                            dr.Item(col.ColumnName) = "12/31/1977"
                    End Select
                End If
            Next
        Next
        Return ds
    End Function

    Public Shared Function fix_datarow_nulls(ByVal dr As DataRow) As DataRow
        'fix nulls in dataset

        For Each col As DataColumn In dr.Table.Columns
            If IsDBNull(dr.Item(col.ColumnName)) Then
                Select Case col.DataType.ToString
                    Case "System.String"
                        dr.Item(col.ColumnName) = ""
                    Case "System.Int32"
                        dr.Item(col.ColumnName) = 0
                    Case "System.DateTime"
                        dr.Item(col.ColumnName) = "12/31/1977"
                End Select
            End If
        Next
        Return dr
    End Function

    Public Shared Function generic_select(ByVal sSQL As String, ByVal sConn As String) As DataSet
        ' get user rights
        Dim cn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim ds As New DataSet
        Dim da As SqlDataAdapter = Nothing
        'System.Diagnostics.Debug.WriteLine(sSQL)
        Try
            cn = New SqlConnection(sConn)
            cn.Open()
            cmd = New SqlCommand(sSQL, cn)
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            da.Dispose()
        Catch ex As SqlException
            ErrorLog.LogDataError("ERROR Getting Generic Select (Shopping Cart SQL SERVER) -> Details => " & ex.Message & "::" & sSQL)
        Finally
            cmd.Dispose()
            cn.Close()
            cn.Dispose()
        End Try
        Return fix_dataset_nulls(ds)
    End Function

    Public Shared Function generic_scalar(ByVal sSQL As String, ByVal sConn As String) As String
        ' get user rights
        Dim cn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim str As String = ""
        'System.Diagnostics.Debug.WriteLine(sSQL)
        Try
            cn = New SqlConnection(sConn)
            cn.Open()
            cmd = New SqlCommand(sSQL, cn)
            Dim sTemp As Object
            sTemp = cmd.ExecuteScalar
            If IsDBNull(sTemp) Or IsNothing(sTemp) Then
                sTemp = ""
            End If
            str = sTemp
        Catch ex As SqlException
            ErrorLog.LogDataError("ERROR Getting generic scalar (Shopping Cart - SQL SERVER) -> Details => " & ex.Message & "::" & sSQL)
        Finally
            cmd.Dispose()
            cn.Close()
            cn.Dispose()
        End Try
        If IsDBNull(str) Or IsNothing(str) Then
            str = ""
        End If
        Return str
    End Function

    Public Shared Function generic_command(ByVal sSQL As String, ByVal sConn As String) As Boolean
        ' get user rights
        Dim cn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        'System.Diagnostics.Debug.WriteLine(sSQL)
        Try
            cn = New SqlConnection(sConn)
            cn.Open()
            cmd = New SqlCommand(sSQL, cn)
            'System.Diagnostics.Debug.WriteLine(sSQL)
            cmd.CommandTimeout = 3600
            cmd.ExecuteNonQuery()
        Catch ex As SqlException
            ErrorLog.LogDataError("ERROR GENERIC COMMAND (Shopping Cart - SQL SERVER) -> Details => " & ex.Message & " SQL:" & sSQL)
            Return False
        Finally
            cmd.Dispose()
            cn.Close()
            cn.Dispose()
        End Try
        Return True
    End Function

    Public Shared Function generic_command(ByVal sSQL As String, ByVal sConn As String, ByVal iTimeout As Integer) As Boolean
        ' get user rights
        Dim cn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        'System.Diagnostics.Debug.WriteLine(sSQL)
        Try
            cn = New SqlConnection(sConn)
            cn.Open()
            cmd = New SqlCommand(sSQL, cn)
            cmd.CommandTimeout = iTimeout
            'System.Diagnostics.Debug.WriteLine(sSQL)
            cmd.ExecuteNonQuery()
        Catch ex As SqlException
            ErrorLog.LogDataError("ERROR GENERIC COMMAND (Shopping Cart - SQL SERVER) -> Details => " & ex.Message & " SQL:" & sSQL)
            Return False
        Finally
            cmd.Dispose()
            cn.Close()
            cn.Dispose()
        End Try
        Return True
    End Function


    Public Shared Function Generic_SQLTransaction(ByVal strSQL As System.Text.StringBuilder, ByVal sConnection As String) As Boolean
        Dim conn As SqlConnection = Nothing
        Dim sqlTrans As SqlTransaction = Nothing
        Dim sqlCmd As SqlCommand = Nothing

        Try
            conn = New SqlConnection(sConnection)
            conn.Open()
            sqlTrans = conn.BeginTransaction

            'System.Diagnostics.Debug.WriteLine(strSQL.ToString)
            sqlCmd = New SqlCommand(strSQL.ToString, conn, sqlTrans)
            sqlCmd.ExecuteNonQuery()
            sqlTrans.Commit()

        Catch ex As SqlException
            sqlTrans.Rollback()
            System.Diagnostics.Debug.WriteLine(ex.Message)
            ErrorLog.LogDataError("ERROR GENERIC TRANSACTION: " & ex.Message & ":" & strSQL.ToString)
            Return False
        Finally
            conn.Close()
        End Try
        Return True
    End Function

#End Region
End Class

