using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCHardware.App_Code
{
	[Serializable]
	public class SalesReceipt
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string SalesPerson { get; set; }
		public Customer Customer { get; set; }
		public double Subtotal { get; set; }
		public double GST { get; set; }
		public double Total { get; set; }
		public List<SalesItem> Items { get; set; }

		public SalesReceipt()
		{
			Items = new List<SalesItem>();
			Id = 0;
			Date = new DateTime(0);
			SalesPerson = "";
			Subtotal = 0;
			GST = 0;
			Total = 0;
		}
	}
}