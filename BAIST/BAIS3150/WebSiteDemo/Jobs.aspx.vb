
Imports System.Data.SqlClient
Imports System.Data

Partial Class Jobs
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim connectionString = "Server=SERVIN8TOR\sqlexpress; Database=ababcock1_BAIS3150_JobsDemo; Integrated Security=SSPI"
        Using dbConnection = New SqlConnection(connectionString)
            dbConnection.Open()

            Using getAllJobsCmd = New SqlCommand("GetAllJobs", dbConnection)
                getAllJobsCmd.CommandType = CommandType.StoredProcedure

                Dim allJobsReader = getAllJobsCmd.ExecuteReader()

                If (allJobsReader.HasRows) Then
                    While allJobsReader.Read

                        Dim jobRow = New TableRow()
                        If JobListingTable.Rows.Count Mod 2 = 0 Then
                            jobRow.BackColor = Drawing.Color.AntiqueWhite
                        Else
                            jobRow.BackColor = Drawing.Color.White
                        End If

                        Dim jobCode = New TableCell()
                        Dim jobClass = New TableCell()
                        Dim jobHourlyRate = New TableCell()

                        jobCode.Text = allJobsReader.Item("JobCode")
                        jobClass.Text = allJobsReader.Item("JobClass")
                        jobClass.Width = 150
                        jobHourlyRate.Text = String.Format("{0:c}", allJobsReader.Item("HourlyRate"))
                        jobHourlyRate.HorizontalAlign = HorizontalAlign.Right

                        jobRow.Cells.Add(jobCode)
                        jobRow.Cells.Add(jobClass)
                        jobRow.Cells.Add(jobHourlyRate)

                        JobListingTable.Rows.Add(jobRow)

                    End While
                Else
                    Dim mostlyEmptyRow = New TableRow()
                    Dim mostlyEmptyCell = New TableCell()

                    mostlyEmptyCell.Text = "No jobs found."
                    mostlyEmptyRow.Cells.Add(mostlyEmptyCell)
                    JobListingTable.Rows.Add(mostlyEmptyRow)
                End If
            End Using
        End Using
    End Sub
End Class
