using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCHardware.App_Code
{
	public class ABCHardwareManager
	{
		public bool AddItem(Item item)
		{
			try
			{
				var manager = new ItemManager();
				return manager.AddItem(item);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool UpdateItem(Item item)
		{
			try
			{
				var manager = new ItemManager();
				return manager.UpdateItem(item);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public Item GetItem(string itemCode)
		{
			var manager = new ItemManager();
			return manager.GetItem(itemCode);
		}
	}
}