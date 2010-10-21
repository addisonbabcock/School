using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassExample3
{
	public class MyPrint
	{
		public void PrintHeader (string _header)
		{
			Console.WriteLine (_header);
			Console.WriteLine ("************************");
		}

		public void PrintItem (string _item)
		{
			Console.Write (_item.ToUpper () + ": ");
		}

		public MyPrint ()
		{
		}
	}
}
