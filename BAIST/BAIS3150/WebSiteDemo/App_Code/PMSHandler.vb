Imports Microsoft.VisualBasic

Public Class PMSHandler

    Public Function CreateJob(job As Job) As Boolean

        Dim confirmation As Boolean
        Dim jobManager = New Jobs()

        confirmation = jobManager.AddJob(job)

        Return confirmation
    End Function

End Class
