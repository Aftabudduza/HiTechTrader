Imports System
Imports System.Collections
Imports System.Linq
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Linq
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class AutoFill
    Inherits System.Web.Services.WebService
    Dim cn As New SqlClient.SqlConnection()
    Dim ds As New DataSet
    Dim dt As New DataTable

    <WebMethod()> _
Public Function GetCompletionList(ByVal prefixText As String, _
 ByVal count As Integer) As String()

        'ADO.Net
        Dim strCn As String = appGlobal.CONNECTIONSTRING
        cn.ConnectionString = strCn
        Dim cmd As New SqlClient.SqlCommand
        cmd.Connection = cn
        cmd.CommandType = CommandType.Text
        cmd.CommandText = ("select DISTINCT A.ManufacturerName FROM (SELECT m.[Name] ManufacturerName, ma.Misspelling FROM ManufacturerMisspelling ma, Manufacturer m WHERE ma.ManufacturerId = m.Id AND ma.Misspelling LIKE @myParameter ) A GROUP BY A.ManufacturerName ORDER BY A.ManufacturerName ASC  ")
        cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%")

        Try
            cn.Open()
            cmd.ExecuteNonQuery()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(ds)
        Catch ex As Exception
        Finally
            cn.Close()
        End Try

        dt = ds.Tables(0)

        'Then return List of string(txtItems) as result

        Dim txtItems As New List(Of String)
        Dim dbValues As String

        For Each row As DataRow In dt.Rows

            ''String From DataBase(dbValues)
            dbValues = row("ManufacturerName").ToString()
            dbValues = dbValues.ToLower()
            txtItems.Add(dbValues)

        Next

        Return txtItems.ToArray()

    End Function
    <WebMethod()> _
Public Function GetCompletionListAlternative(ByVal prefixText As String, _
 ByVal count As Integer) As String()

        'ADO.Net
        Dim strCn As String = appGlobal.CONNECTIONSTRING
        cn.ConnectionString = strCn
        Dim cmd As New SqlClient.SqlCommand
        cmd.Connection = cn
        cmd.CommandType = CommandType.Text
        cmd.CommandText = ("select DISTINCT A.ManufacturerName FROM (SELECT m.[Name] ManufacturerName, ma.Alternative FROM ManufacturerAlternative ma, Manufacturer m WHERE ma.ManufacturerId = m.Id AND ma.Alternative LIKE @myParameter ) A GROUP BY A.ManufacturerName ORDER BY A.ManufacturerName ASC  ")

        cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%")

        Try
            cn.Open()
            cmd.ExecuteNonQuery()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(ds)
        Catch ex As Exception
        Finally
            cn.Close()
        End Try

        dt = ds.Tables(0)

        'Then return List of string(txtItems) as result

        Dim txtItems As New List(Of String)
        Dim dbValues As String

        For Each row As DataRow In dt.Rows

            ''String From DataBase(dbValues)
            dbValues = row("ManufacturerName").ToString()
            dbValues = dbValues.ToLower()
            txtItems.Add(dbValues)

        Next

        Return txtItems.ToArray()

    End Function

    
End Class
