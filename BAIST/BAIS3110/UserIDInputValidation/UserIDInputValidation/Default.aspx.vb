Public Class _Default
    Inherits System.Web.UI.Page

    Protected Function ValidateQueryString() As Boolean
        Dim nameString As String
        nameString = Request.QueryString("Name")
        If String.IsNullOrEmpty(nameString) = False Then
            If System.Text.RegularExpressions.Regex.IsMatch(nameString, "^[a-zA-Z'.\\s]{1,40}$") = False Then
                Response.Write("Invalid query string")
                Return False
            End If
        End If

        Return True
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If ValidateQueryString() = False Then
            Return
        End If

        Dim fileControl As TextBox
        fileControl = CType(form1.FindControl("aFilename"), TextBox)
        If ValidateFilePath(fileControl.Text) = False Then
            Response.Write("File path failed validation")
            Return
        End If

        Response.Write("Hello ")
        Response.Write(Request.QueryString("name"))
        Response.Write("<br /><br />")
        Response.Write("Message=<br />")
        Response.Write(ValidateMessage())
        Response.Write("<br />")

    End Sub

    Protected Function ValidateMessage() As String

        Dim messageControl As TextBox
        messageControl = CType(form1.FindControl("msg"), TextBox)

        Dim sb As StringBuilder
        sb = New StringBuilder(HttpUtility.HtmlEncode(messageControl.Text))

        sb.Replace("&lt;b&gt;", "<b>")
        sb.Replace("&lt;/b&gt;", "</b>")
        sb.Replace("&lt;i&gt;", "<i>")
        sb.Replace("&lt;/i&gt;", "</i>")
        sb.Replace("&lt;u&gt;", "<u>")
        sb.Replace("&lt;/u&gt;", "</u>")

        sb.Replace("&lt;B&gt;", "<B>")
        sb.Replace("&lt;/B&gt;", "</B>")
        sb.Replace("&lt;I&gt;", "<I>")
        sb.Replace("&lt;/I&gt;", "</I>")
        sb.Replace("&lt;U&gt;", "<U>")
        sb.Replace("&lt;/U&gt;", "</U>")

        Return sb.ToString()

        'Return messageControl.Text
    End Function

    Protected Function ValidateFilePath(ByVal filepath As String) As Boolean
        Try
            Request.MapPath(filepath, Request.ApplicationPath, False)
        Catch ex As HttpException
            Return False
        End Try

        Return True
    End Function
End Class