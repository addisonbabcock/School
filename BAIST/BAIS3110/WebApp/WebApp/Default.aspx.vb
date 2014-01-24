Imports System.Collections.Specialized

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim loop1, loop2 As Integer
        Dim arr1(), arr2() As String
        Dim col1 As NameValueCollection

        col1 = Request.ServerVariables
        arr1 = col1.AllKeys
        For loop1 = 0 To arr1.GetUpperBound(0)
            Response.Write("Key: " & arr1(loop1) & "<br>")
            arr2 = col1.GetValues(loop1)
            For loop2 = 0 To arr2.GetUpperBound(0)
                Response.Write("Value " & CStr(loop2) & ": " & Server.HtmlEncode(arr2(loop2)) & "<br>")
            Next
        Next
    End Sub

End Class