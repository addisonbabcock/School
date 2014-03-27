using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

namespace ababcock1BAIS3110Authentication
{
	public partial class Logon : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Submit_Click(object sender, EventArgs e)
		{
			if (SignonSuccessful (UserEmail.Text, UserPass.Text))
			{
				FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, Persist.Checked);
			}
			else
			{
				Msg.Text = "Invalid credentials. Please try again.";
			}
		}

		private bool SignonSuccessful(string userEmail, string userPass)
		{
			var userInfo = RetrieveUserInfo(userEmail);
			if (userInfo == null)
			{
				return false;
			}

			var hashedPassword = AuthenticationHelpers.CreatePasswordHash(userPass, userInfo.userPasswordSalt);

			return userEmail == userInfo.userEmail && hashedPassword == userInfo.userPasswordHash;
		}

		private class UserInfo
		{
			public string userEmail;
			public string userPasswordHash;
			public string userPasswordSalt;
		};

		private UserInfo RetrieveUserInfo(string userEmail)
		{
			var userInfo = new UserInfo();
			SqlDataReader reader = null;

			var connection = new SqlConnection();
			connection.ConnectionString = ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString;

			var command = new SqlCommand("LookupUser", connection);
			command.CommandType = CommandType.StoredProcedure;
			SqlParameter sqlParameter = null;

			sqlParameter = command.Parameters.Add("@userEmail", System.Data.SqlDbType.VarChar, 255);
			sqlParameter.Value = userEmail;

			try
			{
				connection.Open();
				reader = command.ExecuteReader();

				if (reader.Read())
				{
					userInfo.userEmail = (string)reader[0];
					userInfo.userPasswordHash = (string)reader[1];
					userInfo.userPasswordSalt = (string)reader[2];
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
	}
}