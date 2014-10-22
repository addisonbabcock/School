
Option Explicit On
Option Strict On

Partial Class CreateJob
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Protected Sub CreateJobButton_Click(sender As Object, e As EventArgs)

        If Page.IsValid Then    'Disabled browser JS can skip built in validators, force them
            Dim HRManager = New PMSHandler()
            Dim job = New Job()

            Try
                job.JobCode = Integer.Parse(JobCodeTextBox.Text)
                job.JobClass = JobClassTextBox.Text
                job.HourlyRate = Decimal.Parse(HourlyRateTextBox.Text)

                Dim confirmation = HRManager.CreateJob(job)

                If (confirmation) Then
                    MessageLabel.Text = "Successfully added job"
                    MessageLabel.ForeColor = Drawing.Color.Green
                Else
                    MessageLabel.Text = "Failed adding job"
                    MessageLabel.ForeColor = Drawing.Color.Red
                End If
            Catch ex As Exception
                MessageLabel.Text = "Failed adding job. Guru meditation: " & ex.Message
                MessageLabel.ForeColor = Drawing.Color.Red
            End Try
        End If

    End Sub
End Class
