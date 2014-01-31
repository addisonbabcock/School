Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim local As com.cdyne.ws.IP2Geo = New com.cdyne.ws.IP2Geo()
        Dim address As com.cdyne.ws.IPInformation

        address = local.ResolveIP("50.63.202.1", "0")

        Response.Write(address.City.ToString() + "<br />")
        Response.Write(address.Country.ToString() + "<br />")
        Response.Write(address.Latitude.ToString() + "<br />")
        Response.Write(address.Longitude.ToString() + "<br />")
    End Sub

End Class