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
		public int InventoryQuantity { get; set; }

		public Item(string code, string desc, double price, bool deleted, int inventory)
		{
			Code = code;
			Description = desc;
			Price = price;
			Deleted = deleted;
			InventoryQuantity = inventory;
		}

		public Item()
			: this ("", "", 0.00, false, 0)
		{
		}
	}
}