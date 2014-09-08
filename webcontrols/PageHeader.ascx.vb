
Partial Class webcontrols_PageHeader
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '%--style="filter:progid:DXImageTransform.Microsoft.Gradient(endColorstr='#000099', startColorstr='#5B5BFF', gradientType='0');"--%>
    End Sub

    Public Property ErrorMessage() As String
        Get
            Return Me.lblError.Text
        End Get
        Set(ByVal value As String)
            Me.lblError.Text = value
        End Set
    End Property

    Public Property HeaderText() As String
        Get
            Return Me.tcHeader.Text
        End Get
        Set(ByVal value As String)
            Me.tcHeader.Text = value
        End Set
    End Property
End Class
