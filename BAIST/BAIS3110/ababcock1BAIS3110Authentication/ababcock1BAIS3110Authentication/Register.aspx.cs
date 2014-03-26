using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Security;

namespace ababcock1BAIS3110Authentication
{
	public partial class Register : System.Web.UI.Page
	{
		private static string CreateSalt(int size)
		{
			var rng = new RNGCryptoServiceProvider();
			var buff = new byte[size];
			rng.GetBytes(buff);
			return Convert.ToBase64String(buff);
		}

		private static string CreatePasswordHash(string pwd, string salt)
		{
			var saltAndPwd = String.Concat(pwd, salt);
			var hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "SHA1");
			return hashedPwd;
		}

		protected void Submit_Click(object sender, EventArgs e)
		{
			string salt = CreateSalt(5);
			string passwordHash = CreatePasswordHash(UserPass.Text, salt);

			try
			{
				StoreAccountDetails(UserEmail.Text, passwordHash, salt);
			}
			catch (Exception ex)
			{
				Msg.Text = ex.Message;
			}
		}

		private void StoreAccountDetails(string userName, string passwordHash, string salt)
		{
			var connection = new SqlConnection();
			connection.ConnectionString = ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString;

			var command = new SqlCommand("RegisterUser", connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;
			SqlParameter sqlParameter = null;

			sqlParameter = command.Parameters.Add("@userEmail", System.Data.SqlDbType.VarChar, 255);
			sqlParameter.Value = userName;

			sqlParameter = command.Parameters.Add("@passwordHash", System.Data.SqlDbType.VarChar, 40);
			sqlParameter.Value = passwordHash;

			sqlParameter = command.Parameters.Add("@salt", System.Data.SqlDbType.VarChar, 10);
			sqlParameter.Value = salt;

			try
			{
				connection.Open();
				command.ExecuteNonQuery();
				FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, Persist.Checked);
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

		protected void Page_Load(object sender, EventArgs e)
		{

		}
	}
}