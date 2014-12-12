using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ABCHardware.App_Code
{
	public class CustomerManager
	{
		public bool AddCustomer(Customer customer)
		{
			var success = false;
			var connectionString = ConfigurationManager.ConnectionStrings["ABCHardware"].ConnectionString;

			using (var connection = new SqlConnection(connectionString))
			{
				using (var command = new SqlCommand("AddCustomer", connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;

					var idParam = new SqlParameter();
					idParam.Direction = System.Data.ParameterDirection.Input;
					idParam.SqlDbType = System.Data.SqlDbType.Int;
					idParam.SqlValue = customer.Id;
					idParam.ParameterName = "@CustomerId";
					command.Parameters.Add(idParam);

					var nameParam = new SqlParameter();
					nameParam.Direction = System.Data.ParameterDirection.Input;
					nameParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					nameParam.SqlValue = customer.Name;
					nameParam.ParameterName = "@CustomerName";
					command.Parameters.Add(nameParam);

					var addressParam = new SqlParameter();
					addressParam.Direction = System.Data.ParameterDirection.Input;
					addressParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					addressParam.SqlValue = customer.Address;
					addressParam.ParameterName = "@CustomerAddress";
					command.Parameters.Add(addressParam);
					
					var cityParam = new SqlParameter();
					cityParam.Direction = System.Data.ParameterDirection.Input;
					cityParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					cityParam.SqlValue = customer.City;
					cityParam.ParameterName = "@CustomerCity";
					command.Parameters.Add(cityParam);

					var provinceParam = new SqlParameter();
					provinceParam.Direction = System.Data.ParameterDirection.Input;
					provinceParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					provinceParam.SqlValue = customer.Province;
					provinceParam.ParameterName = "@CustomerProvince";
					command.Parameters.Add(provinceParam);

					var pcParam = new SqlParameter();
					pcParam.Direction = System.Data.ParameterDirection.Input;
					pcParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					pcParam.SqlValue = customer.PC;
					pcParam.ParameterName = "@CustomerPC";
					command.Parameters.Add(pcParam);

					var deletedParam = new SqlParameter();
					deletedParam.Direction = System.Data.ParameterDirection.Input;
					deletedParam.SqlDbType = System.Data.SqlDbType.Bit;
					deletedParam.SqlValue = customer.Deleted ? 1 : 0;
					deletedParam.ParameterName = "@Deleted";
					command.Parameters.Add(deletedParam);

					var retParam = new SqlParameter();
					retParam.Direction = System.Data.ParameterDirection.ReturnValue;
					retParam.SqlDbType = System.Data.SqlDbType.Int;
					command.Parameters.Add(retParam);

					connection.Open();
					command.ExecuteNonQuery();

					if ((int)retParam.Value == 0)
					{
						success = true;
					}

					connection.Close();
				}
			}

			return success;
		}

		public bool UpdateCustomer(Customer customer)
		{
			bool success = false;
			var connectionString = ConfigurationManager.ConnectionStrings["ABCHardware"].ConnectionString;

			using (var connection = new SqlConnection(connectionString))
			{
				using (var command = new SqlCommand("UpdateItem", connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;

					var idParam = new SqlParameter();
					idParam.Direction = System.Data.ParameterDirection.Input;
					idParam.SqlDbType = System.Data.SqlDbType.Int;
					idParam.SqlValue = customer.Id;
					idParam.ParameterName = "@CustomerId";
					command.Parameters.Add(idParam);

					var nameParam = new SqlParameter();
					nameParam.Direction = System.Data.ParameterDirection.Input;
					nameParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					nameParam.SqlValue = customer.Name;
					nameParam.ParameterName = "@CustomerName";
					command.Parameters.Add(nameParam);

					var addressParam = new SqlParameter();
					addressParam.Direction = System.Data.ParameterDirection.Input;
					addressParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					addressParam.SqlValue = customer.Address;
					addressParam.ParameterName = "@CustomerAddress";
					command.Parameters.Add(addressParam);

					var cityParam = new SqlParameter();
					cityParam.Direction = System.Data.ParameterDirection.Input;
					cityParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					cityParam.SqlValue = customer.City;
					cityParam.ParameterName = "@CustomerCity";
					command.Parameters.Add(cityParam);

					var provinceParam = new SqlParameter();
					provinceParam.Direction = System.Data.ParameterDirection.Input;
					provinceParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					provinceParam.SqlValue = customer.Province;
					provinceParam.ParameterName = "@CustomerProvince";
					command.Parameters.Add(provinceParam);

					var pcParam = new SqlParameter();
					pcParam.Direction = System.Data.ParameterDirection.Input;
					pcParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					pcParam.SqlValue = customer.PC;
					pcParam.ParameterName = "@CustomerPC";
					command.Parameters.Add(pcParam);

					var deletedParam = new SqlParameter();
					deletedParam.Direction = System.Data.ParameterDirection.Input;
					deletedParam.SqlDbType = System.Data.SqlDbType.Bit;
					deletedParam.SqlValue = customer.Deleted ? 1 : 0;
					deletedParam.ParameterName = "@Deleted";
					command.Parameters.Add(deletedParam);

					var retParam = new SqlParameter();
					retParam.Direction = System.Data.ParameterDirection.ReturnValue;
					retParam.SqlDbType = System.Data.SqlDbType.Int;
					command.Parameters.Add(retParam);

					connection.Open();
					command.ExecuteNonQuery();

					if ((int)retParam.Value == 0)
					{
						success = true;
					}

					connection.Close();
				}
			}

			return success;
		}

		public Customer GetCustomer(int customerId)
		{
			Customer customer;
			var connectionString = ConfigurationManager.ConnectionStrings["ABCHardware"].ConnectionString;

			using (var connection = new SqlConnection(connectionString))
			{
				using (var command = new SqlCommand("GetCustomer", connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;
					var codeParam = new SqlParameter();
					codeParam.Direction = System.Data.ParameterDirection.Input;
					codeParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					codeParam.SqlValue = customerId;
					codeParam.ParameterName = "@CustomerId";
					command.Parameters.Add(codeParam);

					connection.Open();
					var reader = command.ExecuteReader();

					if (reader.Read())
					{
						customer = new Customer();
						customer.Id = int.Parse(reader["CustomerId"].ToString());
						customer.Name = reader["CustomerName"].ToString();
						customer.Address = reader["CustomerAddress"].ToString();
						customer.City = reader["CustomerCity"].ToString();
						customer.Province = reader["CustomerProvince"].ToString();
						customer.PC = reader["CustomerPC"].ToString();
						customer.Deleted = bool.Parse(reader["Deleted"].ToString());
					}
					else
					{
						return null;
					}
				}
			}

			return customer;
		}
	}
}