using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCHardware.App_Code
{
	public class Customer
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Province { get; set; }
		public string PC { get; set; }
		public bool Deleted { get; set; }

		public Customer(int id, string name, string address, string city, string province, string pc, bool deleted)
		{
			Id = id;
			Name = name;
			Address = address;
			City = city;
			Province = province;
			PC = pc;
			Deleted = deleted;
		}

		public Customer()
			: this(0, "", "", "", "", "", false)
		{
		}
	}
}