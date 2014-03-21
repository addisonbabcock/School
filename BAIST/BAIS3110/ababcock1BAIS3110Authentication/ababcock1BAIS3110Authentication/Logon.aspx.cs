using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography;
using System.Web.Security;
using System.Data.SqlClient;

namespace ababcock1BAIS3110Authentication
{
	public partial class Logon : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Submit_Click(object sender, EventArgs e)
		{
			if ((UserEmail.Text == "jchen@contoso.com") && (UserPass.Text == "password"))
			{
				FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, Persist.Checked);
			}
			else
			{
				Msg.Text = "Invalid credentials. Please try again.";
			}
		}
	}
}