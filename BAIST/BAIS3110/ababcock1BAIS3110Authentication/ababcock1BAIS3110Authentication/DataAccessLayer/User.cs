using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.Security;

namespace ababcock1BAIS3110Authentication.DataAccessLayer
{
	public class UserInfo
	{
		public string email;
		public string passwordHash;
		public string passwordSalt;
		public string roles;
	}

	public class UserTable
	{
		public static UserInfo GetUser(string userEmail)
		{
			var userInfo = new UserInfo();

			var connection = Database.GetNewConnection();
			var command = new SqlCommand("LookupUser", connection);
			command.CommandType = CommandType.StoredProcedure;
			SqlParameter sqlParameter = null;

			sqlParameter = command.Parameters.Add("@userEmail", System.Data.SqlDbType.VarChar, 255);
			sqlParameter.Value = userEmail;
			
			SqlDataReader reader = null;

			try
			{
				connection.Open();
				reader = command.ExecuteReader();

				if (reader.Read())
				{
					userInfo.email = (string)reader[0];
					userInfo.passwordHash = (string)reader[1];
					userInfo.passwordSalt = (string)reader[2];
					userInfo.roles = (string)reader[3];
				}
			}
			catch (Exception ex)
			{
				return null;
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
				connection.Close();
			}

			return userInfo;
		}

		public static void AddUser(UserInfo userInfo)
		{
			var connection = Database.GetNewConnection();
			var command = new SqlCommand("RegisterUser", connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;
			SqlParameter sqlParameter = null;

			sqlParameter = command.Parameters.Add("@userEmail", System.Data.SqlDbType.VarChar, 255);
			sqlParameter.Value = userInfo.email;

			sqlParameter = command.Parameters.Add("@passwordHash", System.Data.SqlDbType.VarChar, 40);
			sqlParameter.Value = userInfo.passwordHash;

			sqlParameter = command.Parameters.Add("@salt", System.Data.SqlDbType.VarChar, 10);
			sqlParameter.Value = userInfo.passwordSalt;

			sqlParameter = command.Parameters.Add("@roles", System.Data.SqlDbType.VarChar, 255);
			sqlParameter.Value = userInfo.roles; ;

			try
			{
				connection.Open();
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw new Exception("Exception adding account. " + ex.Message);
			}
			finally
			{
				connection.Close();
			}
		}

		public static bool ValidateUser(string enteredEmail, string enteredPassword, UserInfo dbInfo)
		{
			var hashedPassword = AuthenticationHelpers.CreatePasswordHash(enteredPassword, dbInfo.passwordSalt);

			return enteredEmail == dbInfo.email && hashedPassword == dbInfo.passwordHash;
		}
	}
}