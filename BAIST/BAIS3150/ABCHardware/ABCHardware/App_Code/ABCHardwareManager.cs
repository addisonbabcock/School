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

		public List<Item> GetAllItems()
		{
			var manager = new ItemManager();
			return manager.GetAllItems();
		}

		public List<Item> GetAllActiveItems()
		{
			var manager = new ItemManager();
			return manager.GetAllActiveItems();
		}
		
		public bool AddCustomer(Customer customer)
		{
			try
			{
				var manager = new CustomerManager();
				return manager.AddCustomer(customer);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool UpdateCustomer(Customer customer)
		{
			try
			{
				var manager = new CustomerManager();
				return manager.UpdateCustomer(customer);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public Customer GetCustomer(int customerId)
		{
			var manager = new CustomerManager();
			return manager.GetCustomer(customerId);
		}

		public int AddSale(SalesReceipt receipt)
		{
			try
			{
				var manager = new SalesManager();
				return manager.AddSale(receipt);
			}
			catch (Exception e)
			{
				return 0;
			}
		}
	}
}