
Imports System.Data
Imports System.Data.SqlClient


Partial Class GetCustomersByCountry
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Using connection = New SqlConnection("Data Source=(LocalDB)\v11.0; Database=Northwind; Integrated Security=SSPI")
            connection.Open()

            Dim readCommand = New SqlCommand("ababcock1GetCustomersByCountry", connection)
            readCommand.CommandType = CommandType.StoredProcedure

            Dim countryParam = New SqlParameter()
            countryParam.Direction = ParameterDirection.Input
            countryParam.ParameterName = "@Country"
            countryParam.SqlValue = "UK"
            countryParam.SqlDbType = SqlDbType.NVarChar
            readCommand.Parameters.Add(countryParam)

            Dim dataReader = readCommand.ExecuteReader()

            Response.Write("Customers By Country Columns: <br />")

            For i As Integer = 0 To dataReader.FieldCount - 1
                Response.Write(dataReader.GetName(i) & "; ")
            Next

            Response.Write("<br /><br />Customers By Country Values: <br />")

            While dataReader.Read()
                For i As Integer = 0 To dataReader.FieldCount - 1
                    Response.Write(dataReader(i) & "; ")
                Next
                Response.Write("<br />")
            End While

            dataReader.Close()
            connection.Close()
        End Using
    End Sub
End Class
