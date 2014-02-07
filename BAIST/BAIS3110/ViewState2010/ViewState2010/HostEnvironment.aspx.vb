Imports System.Web.Hosting

Public Class HostEnvironment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Write("MaxConcurrentRequestsPerCPU: " + HostingEnvironment.MaxConcurrentRequestsPerCPU.ToString() + " <br />")
        Response.Write("MaxConcurrentThreadsPerCPU: " + HostingEnvironment.MaxConcurrentThreadsPerCPU.ToString() + " <br />")
        Response.Write("ApplicationHost: " + HostingEnvironment.ApplicationHost.ToString() + " <br />")
        Response.Write("ApplicationPhysicalPath: " + HostingEnvironment.ApplicationPhysicalPath.ToString() + " <br />")
        Response.Write("Cache: " + HostingEnvironment.Cache.ToString() + " <br />")

    End Sub

End Class