using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace ababcock1BAIS3110Authentication
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			Welcome.Text = "Hello, " + Context.User.Identity.Name;
        }

		protected void Signout_Click(object sender, EventArgs e)
		{
			FormsAuthentication.SignOut();

			Session.Clear();
			Session.Abandon();

			Response.Redirect("Logon.aspx");
		}
    }
}