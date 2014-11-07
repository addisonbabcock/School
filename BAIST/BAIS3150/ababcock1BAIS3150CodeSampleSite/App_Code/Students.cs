
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class Students
{
	public bool AddStudent(Student AcceptedStudent, string ProgramCode)
	{
		var success = false;

		using (var dbConnection = new SqlConnection(new Helpers().GetConnectionString()))
		{
			using (var insertCommand = new SqlCommand("AddStudent", dbConnection))
			{
				insertCommand.CommandType = CommandType.StoredProcedure;

				var studentIdParam = new SqlParameter();
				studentIdParam.Direction = ParameterDirection.Input;
				studentIdParam.SqlDbType = SqlDbType.VarChar;
				studentIdParam.ParameterName = "@StudentID";
				studentIdParam.SqlValue = AcceptedStudent.StudentID;

				var firstNameParam = new SqlParameter();
				firstNameParam.Direction = ParameterDirection.Input;
				firstNameParam.SqlDbType = SqlDbType.VarChar;
				firstNameParam.ParameterName = "@FirstName";
				firstNameParam.SqlValue = AcceptedStudent.FirstName;

				var lastNameParam = new SqlParameter();
				lastNameParam.Direction = ParameterDirection.Input;
				lastNameParam.SqlDbType = SqlDbType.VarChar;
				lastNameParam.ParameterName = "@LastName";
				lastNameParam.SqlValue = AcceptedStudent.LastName;

				var emailParam = new SqlParameter();
				emailParam.Direction = ParameterDirection.Input;
				emailParam.SqlDbType = SqlDbType.VarChar;
				emailParam.ParameterName = "@Email";
				emailParam.SqlValue = AcceptedStudent.Email;

				var programCodeParam = new SqlParameter();
				programCodeParam.Direction = ParameterDirection.Input;
				programCodeParam.SqlDbType = SqlDbType.VarChar;
				programCodeParam.ParameterName = "@ProgramCode";
				programCodeParam.SqlValue = ProgramCode;

				insertCommand.Parameters.Add(studentIdParam);
				insertCommand.Parameters.Add(firstNameParam);
				insertCommand.Parameters.Add(lastNameParam);
				insertCommand.Parameters.Add(emailParam);
				insertCommand.Parameters.Add(programCodeParam);

				var returnStatusParam = new SqlParameter();
				returnStatusParam.ParameterName = "@return_status";
				returnStatusParam.SqlDbType = SqlDbType.Int;
				returnStatusParam.Direction = ParameterDirection.ReturnValue;
				insertCommand.Parameters.Add(returnStatusParam);

				dbConnection.Open();
				insertCommand.ExecuteNonQuery();

				if ((int)(returnStatusParam.Value) == 0)
				{
					success = true;
				}

				dbConnection.Close();
			}
		}

		return success;
	}

	public Student GetStudent(string StudentID)
	{
		var EnrolledStudent = new Student();

		using (var sqlConnection = new SqlConnection(new Helpers().GetConnectionString()))
		{
			using (var readCommand = new SqlCommand("GetStudent", sqlConnection))
			{
				readCommand.CommandType = CommandType.StoredProcedure;

				var studentIdParam = new SqlParameter();
				studentIdParam.Direction = ParameterDirection.Input;
				studentIdParam.SqlDbType = SqlDbType.VarChar;
				studentIdParam.ParameterName = "@StudentID";
				studentIdParam.SqlValue = StudentID;
				readCommand.Parameters.Add(studentIdParam);

				sqlConnection.Open();
				var dataReader = readCommand.ExecuteReader();

				if (dataReader.Read())
				{
					EnrolledStudent.StudentID = dataReader["StudentID"].ToString();
					EnrolledStudent.Email = dataReader["Email"].ToString();
					EnrolledStudent.FirstName = dataReader["FirstName"].ToString();
					EnrolledStudent.LastName = dataReader["LastName"].ToString();
				}
				else
				{
					throw new Exception();
				}

				dataReader.Close();

			}
		}

		return EnrolledStudent;
	}

	public List<Student> GetStudents(string programCode)
	{
		var EnrolledStudents = new List<Student>();

		using (var sqlConnection = new SqlConnection(new Helpers().GetConnectionString()))
		{
			using (var getCommand = new SqlCommand("GetStudents", sqlConnection))
			{
				getCommand.CommandType = CommandType.StoredProcedure;

				var programCodeParam = new SqlParameter();
				programCodeParam.Direction = ParameterDirection.Input;
				programCodeParam.SqlDbType = SqlDbType.VarChar;
				programCodeParam.ParameterName = "@ProgramCode";
				programCodeParam.SqlValue = programCode;

				var returnCodeParam = new SqlParameter();
				returnCodeParam.Direction = ParameterDirection.ReturnValue;
				returnCodeParam.SqlDbType = SqlDbType.Int;

				getCommand.Parameters.Add(programCodeParam);
				getCommand.Parameters.Add(returnCodeParam);

				sqlConnection.Open();
				var reader = getCommand.ExecuteReader();

				while (reader.Read())
				{
					var student = new Student();

					student.StudentID = reader["StudentID"].ToString();
					student.FirstName = reader["FirstName"].ToString();
					student.LastName = reader["LastName"].ToString();
					student.Email = reader["Email"].ToString();

					EnrolledStudents.Add(student);
				}

				reader.Close();
			}

			sqlConnection.Close();
		}

		return EnrolledStudents;
	}

	public bool UpdateStudent(Student enrolledStudent)
	{
		var success = false;

		using (var sqlConnection = new SqlConnection(new Helpers().GetConnectionString()))
		{
			using (var updateCommand = new SqlCommand("UpdateStudent", sqlConnection))
			{
				updateCommand.CommandType = CommandType.StoredProcedure;

				var studentIdParam = new SqlParameter();
				studentIdParam.Direction = ParameterDirection.Input;
				studentIdParam.SqlDbType = SqlDbType.VarChar;
				studentIdParam.ParameterName = "@StudentID";
				studentIdParam.SqlValue = enrolledStudent.StudentID;

				var firstNameParam = new SqlParameter();
				firstNameParam.Direction = ParameterDirection.Input;
				firstNameParam.SqlDbType = SqlDbType.VarChar;
				firstNameParam.ParameterName = "@FirstName";
				firstNameParam.SqlValue = enrolledStudent.FirstName;

				var lastNameParam = new SqlParameter();
				lastNameParam.Direction = ParameterDirection.Input;
				lastNameParam.SqlDbType = SqlDbType.VarChar;
				lastNameParam.ParameterName = "@LastName";
				lastNameParam.SqlValue = enrolledStudent.LastName;

				var emailParam = new SqlParameter();
				emailParam.Direction = ParameterDirection.Input;
				emailParam.SqlDbType = SqlDbType.VarChar;
				emailParam.ParameterName = "@Email";
				emailParam.SqlValue = enrolledStudent.Email;

				updateCommand.Parameters.Add(studentIdParam);
				updateCommand.Parameters.Add(firstNameParam);
				updateCommand.Parameters.Add(lastNameParam);
				updateCommand.Parameters.Add(emailParam);

				var returnStatusParam = new SqlParameter();
				returnStatusParam.ParameterName = "@return_status";
				returnStatusParam.SqlDbType = SqlDbType.Int;
				returnStatusParam.Direction = ParameterDirection.ReturnValue;
				updateCommand.Parameters.Add(returnStatusParam);

				sqlConnection.Open();
				updateCommand.ExecuteNonQuery();

				if ((int)(returnStatusParam.Value) == 0)
				{
					success = true;
				}

				sqlConnection.Close();
			}
		}

		return success;
	}

	public bool DeleteStudent(string StudentID)
	{
		var success = false;

		using (var sqlConnection = new SqlConnection(new Helpers().GetConnectionString()))
		{
			using (var updateCommand = new SqlCommand("DeleteStudent", sqlConnection))
			{
				updateCommand.CommandType = CommandType.StoredProcedure;

				var studentIdParam = new SqlParameter();
				studentIdParam.Direction = ParameterDirection.Input;
				studentIdParam.SqlDbType = SqlDbType.VarChar;
				studentIdParam.ParameterName = "@StudentID";
				studentIdParam.SqlValue = StudentID;
				updateCommand.Parameters.Add(studentIdParam);

				var returnStatusParam = new SqlParameter();
				returnStatusParam.ParameterName = "@return_status";
				returnStatusParam.SqlDbType = SqlDbType.Int;
				returnStatusParam.Direction = ParameterDirection.ReturnValue;
				updateCommand.Parameters.Add(returnStatusParam);

				sqlConnection.Open();
				updateCommand.ExecuteNonQuery();

				if ((int)(returnStatusParam.Value) == 0)
				{
					success = true;
				}

				sqlConnection.Close();
			}
		}
		
		return success;
	}
}
