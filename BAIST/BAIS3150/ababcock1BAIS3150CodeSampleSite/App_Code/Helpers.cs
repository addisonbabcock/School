
using System.Data.SqlClient;
using System;
using System.Configuration;

public class Helpers
{
    public string GetConnectionString()
	{
		return ConfigurationManager.ConnectionStrings["Students"].ConnectionString;
	}

	public string GetNorthwindConnectionString()
	{
		return ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
	}
}
