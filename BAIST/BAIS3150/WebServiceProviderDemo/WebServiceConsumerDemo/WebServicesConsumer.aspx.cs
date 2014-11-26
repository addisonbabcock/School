using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServiceConsumerDemo
{
	public partial class WebServicesConsumer : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void submit_Click(object sender, EventArgs e)
		{
			var service = new BAIS3150WebService.BAIS3150WebService();
			message.Text = service.HelloName(name.Text);
		}
	}
}