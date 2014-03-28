using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace ababcock1BAIS3110Authentication.DataAccessLayer
{
	public class Database
	{
		public static SqlConnection GetNewConnection()
		{
			var connection = new SqlConnection();
			connection.ConnectionString = ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString;

			return connection;
		}
	}
}