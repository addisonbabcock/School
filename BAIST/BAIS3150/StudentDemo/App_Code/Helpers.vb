Imports Microsoft.VisualBasic
Imports System.Data.SqlClient

Public Class Helpers
    Private Const connectionString = "Server=SERVIN8TOR\sqlexpress; Database=ababcock1_BAIS3150_StudentsDemo; Integrated Security=SSPI"

    Public Function GetConnectionString() As String
        Return connectionString
    End Function

End Class
