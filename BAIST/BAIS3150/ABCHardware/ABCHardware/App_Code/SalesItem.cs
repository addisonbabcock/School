using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCHardware.App_Code
{
	public class SalesItem
	{
		public Item Item { get; set; }
		public int Quantity { get; set; }
		public double ItemTotal { get; set; }
	}
}