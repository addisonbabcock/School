Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Class Students

    Function AddStudent(AcceptedStudent As Student, ProgramCode As String) As Object

        Dim success = False

        Using dbConnection = New SqlConnection(New Helpers().GetConnectionString())
            Using insertCommand = New SqlCommand("AddStudent", dbConnection)
                insertCommand.CommandType = CommandType.StoredProcedure

                Dim studentIdParam = New SqlParameter()
                studentIdParam.Direction = ParameterDirection.Input
                studentIdParam.SqlDbType = SqlDbType.VarChar
                studentIdParam.ParameterName = "@StudentID"
                studentIdParam.SqlValue = AcceptedStudent.StudentID

                Dim firstNameParam = New SqlParameter()
                firstNameParam.Direction = ParameterDirection.Input
                firstNameParam.SqlDbType = SqlDbType.VarChar
                firstNameParam.ParameterName = "@FirstName"
                firstNameParam.SqlValue = AcceptedStudent.FirstName

                Dim lastNameParam = New SqlParameter()
                lastNameParam.Direction = ParameterDirection.Input
                lastNameParam.SqlDbType = SqlDbType.VarChar
                lastNameParam.ParameterName = "@LastName"
                lastNameParam.SqlValue = AcceptedStudent.LastName

                Dim emailParam = New SqlParameter()
                emailParam.Direction = ParameterDirection.Input
                emailParam.SqlDbType = SqlDbType.VarChar
                emailParam.ParameterName = "@Email"
                emailParam.SqlValue = AcceptedStudent.Email

                Dim programCodeParam = New SqlParameter()
                programCodeParam.Direction = ParameterDirection.Input
                programCodeParam.SqlDbType = SqlDbType.VarChar
                programCodeParam.ParameterName = "@ProgramCode"
                programCodeParam.SqlValue = ProgramCode

                insertCommand.Parameters.Add(studentIdParam)
                insertCommand.Parameters.Add(firstNameParam)
                insertCommand.Parameters.Add(lastNameParam)
                insertCommand.Parameters.Add(emailParam)
                insertCommand.Parameters.Add(programCodeParam)

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

    Function GetStudent(StudentID As String) As Student

        Dim EnrolledStudent = New Student()

        Using sqlConnection = New SqlConnection(New Helpers().GetConnectionString())
            Using readCommand = New SqlCommand("GetStudent", sqlConnection)
                readCommand.CommandType = CommandType.StoredProcedure

                Dim studentIdParam = New SqlParameter()
                studentIdParam.Direction = ParameterDirection.Input
                studentIdParam.SqlDbType = SqlDbType.VarChar
                studentIdParam.ParameterName = "@StudentID"
                studentIdParam.SqlValue = StudentID
                readCommand.Parameters.Add(studentIdParam)

                sqlConnection.Open()
                Dim dataReader = readCommand.ExecuteReader()
                If dataReader.Read() Then
                    EnrolledStudent.StudentID = dataReader("StudentID")
                    EnrolledStudent.Email = dataReader("Email")
                    EnrolledStudent.FirstName = dataReader("FirstName")
                    EnrolledStudent.LastName = dataReader("LastName")
                End If
                dataReader.Close()

            End Using
        End Using

        Return EnrolledStudent
    End Function

    Function GetStudents(programCode As String) As List(Of Student)
        Throw New NotImplementedException
    End Function

    Function UpdateStudent(enrolledStudent As Student) As Boolean

        Dim success = False

        Using sqlConnection = New SqlConnection(New Helpers().GetConnectionString())
            Using updateCommand = New SqlCommand("UpdateStudent", sqlConnection)
                updateCommand.CommandType = CommandType.StoredProcedure

                Dim studentIdParam = New SqlParameter()
                studentIdParam.Direction = ParameterDirection.Input
                studentIdParam.SqlDbType = SqlDbType.VarChar
                studentIdParam.ParameterName = "@StudentID"
                studentIdParam.SqlValue = enrolledStudent.StudentID

                Dim firstNameParam = New SqlParameter()
                firstNameParam.Direction = ParameterDirection.Input
                firstNameParam.SqlDbType = SqlDbType.VarChar
                firstNameParam.ParameterName = "@FirstName"
                firstNameParam.SqlValue = enrolledStudent.FirstName

                Dim lastNameParam = New SqlParameter()
                lastNameParam.Direction = ParameterDirection.Input
                lastNameParam.SqlDbType = SqlDbType.VarChar
                lastNameParam.ParameterName = "@LastName"
                lastNameParam.SqlValue = enrolledStudent.LastName

                Dim emailParam = New SqlParameter()
                emailParam.Direction = ParameterDirection.Input
                emailParam.SqlDbType = SqlDbType.VarChar
                emailParam.ParameterName = "@Email"
                emailParam.SqlValue = enrolledStudent.Email

                updateCommand.Parameters.Add(studentIdParam)
                updateCommand.Parameters.Add(firstNameParam)
                updateCommand.Parameters.Add(lastNameParam)
                updateCommand.Parameters.Add(emailParam)

                Dim returnStatusParam = New SqlParameter()
                returnStatusParam.ParameterName = "@return_status"
                returnStatusParam.SqlDbType = SqlDbType.Int
                returnStatusParam.Direction = ParameterDirection.ReturnValue
                updateCommand.Parameters.Add(returnStatusParam)

                sqlConnection.Open()
                updateCommand.ExecuteNonQuery()

                If CType(returnStatusParam.Value, Integer) = 0 Then
                    success = True
                End If

                sqlConnection.Close()

            End Using
        End Using

        Return success
    End Function

	Function DeleteStudent(StudentID As String) As Boolean

		Dim success = False

		Using sqlConnection = New SqlConnection(New Helpers().GetConnectionString())
			Using updateCommand = New SqlCommand("DeleteStudent", sqlConnection)
				updateCommand.CommandType = CommandType.StoredProcedure

				Dim studentIdParam = New SqlParameter()
				studentIdParam.Direction = ParameterDirection.Input
				studentIdParam.SqlDbType = SqlDbType.VarChar
				studentIdParam.ParameterName = "@StudentID"
				studentIdParam.SqlValue = StudentID
				updateCommand.Parameters.Add(studentIdParam)

				Dim returnStatusParam = New SqlParameter()
				returnStatusParam.ParameterName = "@return_status"
				returnStatusParam.SqlDbType = SqlDbType.Int
				returnStatusParam.Direction = ParameterDirection.ReturnValue
				updateCommand.Parameters.Add(returnStatusParam)

				sqlConnection.Open()
				updateCommand.ExecuteNonQuery()

				If CType(returnStatusParam.Value, Integer) = 0 Then
					success = True
				End If

				sqlConnection.Close()

			End Using
		End Using

		Return success
	End Function

End Class
