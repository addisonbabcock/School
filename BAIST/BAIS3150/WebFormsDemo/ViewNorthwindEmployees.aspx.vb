Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Partial Class ViewNorthwindEmployees
    Inherits System.Web.UI.Page

    Public Function ObjectToByteArray(ByVal ob As Object) As Byte()
        Try
            Dim stream = New MemoryStream()
            Dim formatter As New BinaryFormatter()
            formatter.Serialize(stream, ob)
            Return stream.ToArray()
        Catch ex As Exception

        End Try

        Return Nothing
    End Function

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Using dbConn = New SqlConnection("Server=(LocalDB)\v11.0; Database=Northwind; Integrated Security=SSPI")
            Using command = New SqlCommand("GetNorthwindEmployees", dbConn)

                dbConn.Open()
                Dim reader = command.ExecuteReader()

                Dim headerRow = New TableRow()
                For i As Integer = 0 To reader.FieldCount - 1
                    Dim headerCell = New TableCell()
                    headerCell.Text = "<b>" & reader.GetName(i) & "</b>"
                    headerRow.Cells.Add(headerCell)
                Next
                Employees.Rows.Add(headerRow)

                While reader.Read()
                    Dim employeeRow = New TableRow()
                    For i As Integer = 0 To reader.FieldCount - 1
                        Dim employeeCell = New TableCell()

                        If reader.GetName(i) = "Photo" Then
                            Dim bytes = ObjectToByteArray(reader(i))
                            Dim base64 = Convert.ToBase64String(reader(i))
                            employeeCell.Text = "<img src=""data:image/png;base64," & base64 & """ />"
                            'employeeCell.Text = "<img src=""data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg=="" alt=""Red dot""/>"
                        Else
                            employeeCell.Text = reader(i)
                        End If

                        employeeRow.Cells.Add(employeeCell)
                    Next
                    Employees.Rows.Add(employeeRow)
                End While

            End Using
        End Using

    End Sub
End Class
