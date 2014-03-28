using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Security;
using ababcock1BAIS3110Authentication.DataAccessLayer;

namespace ababcock1BAIS3110Authentication
{
	public partial class Register : System.Web.UI.Page
	{
		protected void Submit_Click(object sender, EventArgs e)
		{
			string salt = AuthenticationHelpers.CreateSalt(5);
			string passwordHash = AuthenticationHelpers.CreatePasswordHash(UserPass.Text, salt);

			try
			{
				var userInfo = new UserInfo();
				userInfo.email = UserEmail.Text;
				userInfo.passwordHash = passwordHash;
				userInfo.passwordSalt = salt;
				userInfo.roles = "User";

				UserTable.AddUser(userInfo);
				Response.Cookies.Add(
					AuthenticationHelpers.GetAuthCookie(userInfo, Persist.Checked));
				FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, Persist.Checked);
			}
			catch (Exception ex)
			{
				Msg.Text = ex.Message;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{

		}
	}
}