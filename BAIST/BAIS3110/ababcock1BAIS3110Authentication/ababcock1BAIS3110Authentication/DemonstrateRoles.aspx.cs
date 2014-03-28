using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Principal;

namespace ababcock1BAIS3110Authentication
{
	public partial class DemonstrateRoles : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			TestRoles();
		}

		private void TestIsInRole(CustomPrincipal cp, string role)
		{
			if (cp.IsInRole(role))
			{
				Response.Write(cp.Identity.Name + " is in the " + role + " role. <p />");
			}
			else
			{
				Response.Write(cp.Identity.Name + " is <i>not</i> in the " + role + " Role. <p />");
			}
		}

		private void TestRoles()
		{
			CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

			if (cp == null)
			{
				Response.Write("User is not logged in.");
			}
			else
			{
				Response.Write("Authenticated identity is: " + cp.Identity.Name);
				Response.Write("<p />");

				TestIsInRole(cp, "Senior Manager");
				TestIsInRole(cp, "User");
				TestIsInRole(cp, "Sales");
			}
		}
	}
}