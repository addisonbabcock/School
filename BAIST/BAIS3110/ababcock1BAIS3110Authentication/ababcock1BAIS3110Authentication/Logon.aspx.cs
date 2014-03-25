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
			var connection = new SqlConnection();
			connection.ConnectionString = ConfigurationManager.ConnectionStrings["UsersDB"].ConnectionString;

			var command = new SqlCommand("RegisterUser", connection);
			command.CommandType = CommandType.StoredProcedure;
			SqlParameter sqlParameter = null;

			sqlParameter = command.Parameters.Add("@userEmail", System.Data.SqlDbType.VarChar, 255);
			sqlParameter.Value = userEmail;

			try
			{
				connection.Open();
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				return false;
			}
			finally
			{
				connection.Close();
			}

			return true;
		}
	}
}