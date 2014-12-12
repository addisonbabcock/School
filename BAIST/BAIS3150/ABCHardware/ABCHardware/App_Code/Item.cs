using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCHardware.App_Code
{
	public class Item
	{
		public string Code { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public bool Deleted { get; set; }
	}
}