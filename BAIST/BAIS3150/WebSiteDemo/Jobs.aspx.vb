
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
                    Response.Write("<h2>All Job Listings</h2>")

                    While allJobsReader.Read
                        Dim jobLine = String.Format("{0}, {1}, {2} <br />", _
                                                    allJobsReader("JobCode"), _
                                                    allJobsReader("JobClass"), _
                                                    allJobsReader("HourlyRate"))
                        Response.Write(jobLine)
                    End While
                Else
                    Response.Write("<h2>No jobs</h2>")
                End If
            End Using
        End Using
    End Sub
End Class
