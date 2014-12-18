using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ABCHardware.App_Code
{
	public class ItemManager
	{
		public bool AddItem(Item item)
		{
			var success = false;
			var connectionString = ConfigurationManager.ConnectionStrings["ABCHardware"].ConnectionString;

			using (var connection = new SqlConnection(connectionString))
			{
				using (var command = new SqlCommand("AddItem", connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;

					var codeParam = new SqlParameter();
					codeParam.Direction = System.Data.ParameterDirection.Input;
					codeParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					codeParam.SqlValue = item.Code;
					codeParam.ParameterName = "@ItemCode";
					command.Parameters.Add(codeParam);

					var descParam = new SqlParameter();
					descParam.Direction = System.Data.ParameterDirection.Input;
					descParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					descParam.SqlValue = item.Description;
					descParam.ParameterName = "@ItemDescription";
					command.Parameters.Add(descParam);

					var priceParam = new SqlParameter();
					priceParam.Direction = System.Data.ParameterDirection.Input;
					priceParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					priceParam.SqlValue = item.Price;
					priceParam.ParameterName = "@UnitPrice";
					command.Parameters.Add(priceParam);

					var deletedParam = new SqlParameter();
					deletedParam.Direction = System.Data.ParameterDirection.Input;
					deletedParam.SqlDbType = System.Data.SqlDbType.Bit;
					deletedParam.SqlValue = item.Deleted ? 1 : 0;
					deletedParam.ParameterName = "@Deleted";
					command.Parameters.Add(deletedParam);

					var quantityParam = new SqlParameter();
					quantityParam.Direction = System.Data.ParameterDirection.Input;
					quantityParam.SqlDbType = System.Data.SqlDbType.Int;
					quantityParam.SqlValue = item.InventoryQuantity;
					quantityParam.ParameterName = "@InventoryQuantity";
					command.Parameters.Add(quantityParam);

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

		public bool UpdateItem(Item item)
		{
			bool success = false;
			var connectionString = ConfigurationManager.ConnectionStrings["ABCHardware"].ConnectionString;

			using (var connection = new SqlConnection(connectionString))
			{
				using (var command = new SqlCommand("UpdateItem", connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;

					var codeParam = new SqlParameter();
					codeParam.Direction = System.Data.ParameterDirection.Input;
					codeParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					codeParam.SqlValue = item.Code;
					codeParam.ParameterName = "@ItemCode";
					command.Parameters.Add(codeParam);

					var descParam = new SqlParameter();
					descParam.Direction = System.Data.ParameterDirection.Input;
					descParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					descParam.SqlValue = item.Description;
					descParam.ParameterName = "@ItemDescription";
					command.Parameters.Add(descParam);

					var priceParam = new SqlParameter();
					priceParam.Direction = System.Data.ParameterDirection.Input;
					priceParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					priceParam.SqlValue = item.Price;
					priceParam.ParameterName = "@UnitPrice";
					command.Parameters.Add(priceParam);

					var deletedParam = new SqlParameter();
					deletedParam.Direction = System.Data.ParameterDirection.Input;
					deletedParam.SqlDbType = System.Data.SqlDbType.Bit;
					deletedParam.SqlValue = item.Deleted ? 1 : 0;
					deletedParam.ParameterName = "@Deleted";
					command.Parameters.Add(deletedParam);

					var quantityParam = new SqlParameter();
					quantityParam.Direction = System.Data.ParameterDirection.Input;
					quantityParam.SqlDbType = System.Data.SqlDbType.Int;
					quantityParam.SqlValue = item.InventoryQuantity;
					quantityParam.ParameterName = "@InventoryQuantity";
					command.Parameters.Add(quantityParam);

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

		public Item GetItem(string ItemCode)
		{
			Item item;
			var connectionString = ConfigurationManager.ConnectionStrings["ABCHardware"].ConnectionString;

			using (var connection = new SqlConnection(connectionString))
			{
				using (var command = new SqlCommand("GetItem", connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;
					var codeParam = new SqlParameter();
					codeParam.Direction = System.Data.ParameterDirection.Input;
					codeParam.SqlDbType = System.Data.SqlDbType.NVarChar;
					codeParam.SqlValue = ItemCode;
					codeParam.ParameterName = "@ItemCode";
					command.Parameters.Add(codeParam);

					connection.Open();
					var reader = command.ExecuteReader();

					if (reader.Read())
					{
						item = new Item();
						item.Code = reader["ItemCode"].ToString();
						item.Description = reader["Description"].ToString();
						item.Price = double.Parse(reader["UnitPrice"].ToString());
						item.Deleted = bool.Parse(reader["Deleted"].ToString());
						item.InventoryQuantity = int.Parse(reader["InventoryQuantity"].ToString());
					}
					else
					{
						return null;
					}
				}
			}

			return item;
		}

		public List<Item> GetAllItems()
		{
			var items = new List<Item>();
			var connectionString = ConfigurationManager.ConnectionStrings["ABCHardware"].ConnectionString;

			using (var connection = new SqlConnection(connectionString))
			{
				using (var command = new SqlCommand("GetAllItems", connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;

					connection.Open();
					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						var item = new Item();
						item.Code = reader["ItemCode"].ToString();
						item.Description = reader["Description"].ToString();
						item.Price = double.Parse(reader["UnitPrice"].ToString());
						item.Deleted = bool.Parse(reader["Deleted"].ToString());
						item.InventoryQuantity = int.Parse(reader["InventoryQuantity"].ToString());

						items.Add(item);
					}
				}
			}

			return items;
		}

		public List<Item> GetAllActiveItems()
		{
			var items = new List<Item>();
			var connectionString = ConfigurationManager.ConnectionStrings["ABCHardware"].ConnectionString;

			using (var connection = new SqlConnection(connectionString))
			{
				using (var command = new SqlCommand("GetAllActiveItems", connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;

					connection.Open();
					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						var item = new Item();
						item.Code = reader["ItemCode"].ToString();
						item.Description = reader["Description"].ToString();
						item.Price = double.Parse(reader["UnitPrice"].ToString());
						item.Deleted = bool.Parse(reader["Deleted"].ToString());
						item.InventoryQuantity = int.Parse(reader["InventoryQuantity"].ToString());

						items.Add(item);
					}
				}
			}

			return items;
		}
	}
}