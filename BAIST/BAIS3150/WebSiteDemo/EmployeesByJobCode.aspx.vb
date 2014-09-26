
Imports System.Data
Imports System.Data.SqlClient

Partial Class EmployeesByJobCode
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim connectionString = "Server=SERVIN8TOR\sqlexpress; Database=ababcock1_BAIS3150_JobsDemo; Integrated Security=SSPI"
        Dim PMSDataSet = New DataSet("PMSDataSet")

        Using dbConnection = New SqlConnection(connectionString)

            Dim GetEmployeesByJobCodeCommand = New SqlCommand("GetEmployeesWithJob", dbConnection)
            GetEmployeesByJobCodeCommand.CommandType = CommandType.StoredProcedure
            GetEmployeesByJobCodeCommand.Parameters.Add(New SqlParameter("@JobCode", 3))

            Dim GetEmployeesByJobCodeDataAdapter = New SqlDataAdapter(GetEmployeesByJobCodeCommand)

            GetEmployeesByJobCodeDataAdapter.Fill(PMSDataSet, "PMSEmployee")
            Dim PMSEmployeeTable = PMSDataSet.Tables("PMSEmployee")

            For Each EmployeeRow As DataRow In PMSEmployeeTable.Rows
                Dim RowData = String.Format("{0}, {1}, {2}, {3:C} <br />", _
                                             EmployeeRow("EmployeeNumber"), _
                                             EmployeeRow("EmployeeName"), _
                                             EmployeeRow("JobClass"), _
                                             EmployeeRow("HourlyRate"))
                Response.Write(RowData)
            Next
        End Using

    End Sub
End Class
