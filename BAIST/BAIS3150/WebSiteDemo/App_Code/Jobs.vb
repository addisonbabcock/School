Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Jobs

    Private Const connectionString = "Server=SERVIN8TOR\sqlexpress; Database=ababcock1_BAIS3150_JobsDemo; Integrated Security=SSPI"

    Public Function AddJob(job As Job) _
                       As Boolean
        Dim success = False

        Using dbConnection = New SqlConnection(connectionString)
            Using insertCommand = New SqlCommand("AddJob", dbConnection)
                insertCommand.Parameters.Add(New SqlParameter("@JobCode", job.JobCode))         'these should be expanded to 
                insertCommand.Parameters.Add(New SqlParameter("@JobClass", job.JobClass))       'also set .SqlDbType and .Direction
                insertCommand.Parameters.Add(New SqlParameter("@HourlyRate", job.HourlyRate))

                Dim returnStatusParam = New SqlParameter()
                returnStatusParam.ParameterName = "@return_status"
                returnStatusParam.SqlDbType = SqlDbType.Int
                returnStatusParam.Direction = ParameterDirection.ReturnValue
                insertCommand.Parameters.Add(returnStatusParam)

                dbConnection.Open()
                insertCommand.ExecuteNonQuery()

                If CType(returnStatusParam.Value, Integer) = 0 Then
                    success = True
                End If
                dbConnection.Close()
            End Using
        End Using

        Return success
    End Function

End Class
