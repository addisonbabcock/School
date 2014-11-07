
using System.Data;
using System.Data.SqlClient;

public class Programs
{
	public bool AddProgram(string ProgramCode, string Description)
	{
		var success = false;

		using (var dbConnection = new SqlConnection(new Helpers().GetConnectionString()))
		{
			using (var insertCommand = new SqlCommand("AddProgram", dbConnection))
			{
				insertCommand.CommandType = CommandType.StoredProcedure;

				var programCodeParam = new SqlParameter();
				programCodeParam.Direction = ParameterDirection.Input;
				programCodeParam.SqlDbType = SqlDbType.VarChar;
				programCodeParam.ParameterName = "@ProgramCode";
				programCodeParam.SqlValue = ProgramCode;

				var descriptionParam = new SqlParameter();
				descriptionParam.Direction = ParameterDirection.Input;
				descriptionParam.SqlDbType = SqlDbType.VarChar;
				descriptionParam.ParameterName = "@Description";
				descriptionParam.SqlValue = Description;

				insertCommand.Parameters.Add(programCodeParam);
				insertCommand.Parameters.Add(descriptionParam);

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

	public Program GetProgram(string programCode)
	{
		var program = new Program();

		using (var dbConnection = new SqlConnection(new Helpers().GetConnectionString()))
		{
			using (var getCommand = new SqlCommand("GetProgram", dbConnection))
			{
				getCommand.CommandType = CommandType.StoredProcedure;

				var programCodeParam = new SqlParameter();
				programCodeParam.Direction = ParameterDirection.Input;
				programCodeParam.SqlDbType = SqlDbType.VarChar;
				programCodeParam.ParameterName = "@ProgramCode";
				programCodeParam.Value = programCode;

				getCommand.Parameters.Add(programCodeParam);

				var returnStatusParam = new SqlParameter();
				returnStatusParam.ParameterName = "@return_status";
				returnStatusParam.SqlDbType = SqlDbType.Int;
				returnStatusParam.Direction = ParameterDirection.ReturnValue;
				getCommand.Parameters.Add(returnStatusParam);

				dbConnection.Open();

				var dataReader = getCommand.ExecuteReader();

				if (dataReader.Read())
				{
					var students = new Students();

					program.ProgramCode = dataReader[0].ToString();
					program.Description = dataReader[1].ToString();
					program.EnrolledStudents = students.GetStudents(programCode);
				}

				dataReader.Close();
				dbConnection.Close();
			}
		}

		return program;
	}
}
