using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebServiceDemo
{
	/// <summary>
	/// Summary description for MyWebService
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/", Description = "My First Web Service", Name = "ababcock1BAIS3150Service")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class MyWebService : System.Web.Services.WebService
	{

		[WebMethod]
		public string HelloWorld()
		{
			return "Hello World";
		}

		[WebMethod]
		public DataSet GetCustomersByCountry(string country)
		{
		}
	}
}
