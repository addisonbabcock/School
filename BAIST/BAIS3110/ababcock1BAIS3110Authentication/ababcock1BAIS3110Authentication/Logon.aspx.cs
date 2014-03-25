using System;
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