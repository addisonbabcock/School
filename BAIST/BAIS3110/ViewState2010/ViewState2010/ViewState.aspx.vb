Public Class ViewState
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Dim values As ArrayList = New ArrayList()

            values.Add("Chips")
            values.Add("Chocolate Bars")
            values.Add("Popcorn")

            food.DataSource = values
            food.DataBind()
        Else
            ViewState("PostBackCounter") += 1
        End If

    End Sub

    Protected Sub Choose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Choose.Click
        Dim counter As Integer = (ViewState("PostBackCounter"))

        Response.Write("You chose " + food.SelectedItem.Text + "<br />")
        Response.Write("The button has been clicked " + counter.ToString() + " times. <br />")
    End Sub

End Class