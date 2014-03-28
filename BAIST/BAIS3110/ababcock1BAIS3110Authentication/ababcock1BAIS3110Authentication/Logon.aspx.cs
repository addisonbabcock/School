using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using ababcock1BAIS3110Authentication.DataAccessLayer;
using System.Web;

namespace ababcock1BAIS3110Authentication
{
	public partial class Logon : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Submit_Click(object sender, EventArgs e)
		{
			if (Authenticate (UserEmail.Text, UserPass.Text))
			{
				FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, Persist.Checked);
			}
			else
			{
				Msg.Text = "Invalid credentials. Please try again.";
			}
		}

		private bool Authenticate(string userEmail, string userPass)
		{
			UserInfo userInfo = UserTable.GetUser(userEmail);
			if (userInfo == null)
			{
				return false;
			}

			if (UserTable.ValidateUser(userEmail, userPass, userInfo))
			{
				Response.Cookies.Add(
					AuthenticationHelpers.GetAuthCookie(userInfo, Persist.Checked));
				return true;
			}

			return false;
		}
	}
}