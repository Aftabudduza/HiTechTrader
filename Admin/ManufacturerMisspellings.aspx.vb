Imports System.Data
Imports BRIClassLibrary
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports System.Web.Services
Partial Class Admin_ManufacturerMisspellings
    Inherits System.Web.UI.Page
    Public strSQL As String
    Public errStr As String
    Public lgStr As String
    Public loggedout As String
    Public strErrorsLogin As String
    Private strConnection As String = appGlobal.CONNECTIONSTRING()
    Public objCommand As Data.IDbCommand = Nothing
    Public m_objCN As Data.IDbConnection
    Dim nUserID As Integer = 0
    Public objDataReader As Data.IDataReader = Nothing
    <WebMethod()> _
Public Shared Function FindManufacturerName(ByVal searchText As String, ByVal maxResults As Integer) As String
        Dim ds As DataSet = Nothing
        Dim ds2 As DataSet = Nothing
        Dim dt As DataTable = Nothing
        Dim sQuery As String = String.Empty
        Dim query As String = String.Empty
        Dim stateArray As New ArrayList
        Dim serializer As New JavaScriptSerializer
        Dim sConnection As String = appGlobal.CONNECTIONSTRING()
        sQuery = "select DISTINCT A.ManufacturerName FROM (SELECT m.[Name] ManufacturerName, ma.Misspelling FROM ManufacturerMisspelling ma, Manufacturer m WHERE ma.ManufacturerId = m.Id AND ma.Misspelling LIKE '%" & searchText & "%') A GROUP BY A.ManufacturerName ORDER BY A.ManufacturerName ASC "
        dt = BRIClassLibrary.SQLData.generic_select(sQuery, sConnection).Tables(0)
        If Not dt Is Nothing Then
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim st As New State()
                    st.value = dr("ManufacturerName").ToString()
                    st.label = dr("ManufacturerName").ToString()
                    stateArray.Add(st)
                Next
            End If
        End If
        Return serializer.Serialize(stateArray)
    End Function
End Class
Public Class State
    Public value As String
    Public label As String

End Class
