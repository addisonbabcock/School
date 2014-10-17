Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Programs

    Public Function AddProgram(ProgramCode As String, Description As String) As Boolean
        Dim success = False

        Using dbConnection = New SqlConnection(New Helpers().GetConnectionString())
            Using insertCommand = New SqlCommand("AddProgram", dbConnection)
                insertCommand.CommandType = CommandType.StoredProcedure

                Dim programCodeParam = New SqlParameter()
                programCodeParam.Direction = ParameterDirection.Input
                programCodeParam.SqlDbType = SqlDbType.VarChar
                programCodeParam.ParameterName = "@ProgramCode"
                programCodeParam.SqlValue = ProgramCode

                Dim descriptionParam = New SqlParameter()
                descriptionParam.Direction = ParameterDirection.Input
                descriptionParam.SqlDbType = SqlDbType.VarChar
                descriptionParam.ParameterName = "@Description"
                descriptionParam.SqlValue = Description

                insertCommand.Parameters.Add(programCodeParam)
                insertCommand.Parameters.Add(descriptionParam)

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


    Public Function GetProgram(programCode As String) As Program

        Dim program As New Program

        Using dbConnection = New SqlConnection(New Helpers().GetConnectionString())
            Using getCommand = New SqlCommand("GetProgram", dbConnection)
                Dim programCodeParam = New SqlParameter()
                programCodeParam.Direction = ParameterDirection.Input
                programCodeParam.SqlDbType = SqlDbType.VarChar
                programCodeParam.ParameterName = "@ProgramCode"
                programCodeParam.Value = programCode

                getCommand.Parameters.Add(programCodeParam)

                Dim returnStatusParam = New SqlParameter()
                returnStatusParam.ParameterName = "@return_status"
                returnStatusParam.SqlDbType = SqlDbType.Int
                returnStatusParam.Direction = ParameterDirection.ReturnValue
                getCommand.Parameters.Add(returnStatusParam)

                dbConnection.Open()

                Dim dataReader = getCommand.ExecuteReader()

                If CType(returnStatusParam.Value, Integer) = 0 Then

                    If dataReader.Read() Then
                        Dim students = New Students()

                        program.ProgramCode = dataReader(0)
                        program.Description = dataReader(1)
                        program.EnrolledStudents = students.GetStudents(programCode)
                    End If
                End If

                dataReader.Close()
                dbConnection.Close()
            End Using
        End Using

        Return program

    End Function
End Class
