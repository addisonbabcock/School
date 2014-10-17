
Partial Class CreateJob
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim HRManager = New PMSHandler()
        Dim job = New Job()
        job.JobCode = 500
        job.JobClass = "Software Engineer"
        job.HourlyRate = 99.99

        Dim confirmation = HRManager.CreateJob(job)

        If (confirmation) Then
            Response.Write("Successfully added job")
        Else
            Response.Write("Failed adding job")
        End If

    End Sub
End Class
