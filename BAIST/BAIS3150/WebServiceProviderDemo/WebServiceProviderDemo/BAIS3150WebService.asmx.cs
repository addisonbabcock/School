using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebServiceProviderDemo
{
	/// <summary>
	/// Summary description for BAIS3150WebService
	/// </summary>
	[WebService(Namespace = "BAIS3150")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class BAIS3150WebService : System.Web.Services.WebService
	{

		[WebMethod]
		public string HelloWorld()
		{
			return "Hello World";
		}

		[WebMethod]
		public string HelloName(string name)
		{
			return "Hello " + name;
		}

		[WebMethod(Description="Return the quote of the day")]
		public string QuoteOfTheDay()
		{
			switch (DateTime.Now.DayOfWeek)
			{
				case DayOfWeek.Sunday:
					return "Back to work tomorrow sucker!";
					
				case DayOfWeek.Monday:
				case DayOfWeek.Tuesday:
				case DayOfWeek.Wednesday:
				case DayOfWeek.Thursday:
					return "Ugh... work...";

				case DayOfWeek.Friday:
					return "Friday!!";

				case DayOfWeek.Saturday:
					return "Weekend!!!";
			}

			return "Wut";
		}
	}
}
