using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrayList
{
	class FakeList
	{
		int [] data;
		int size;
		int count;

		public FakeList ()
		{
			data = new int [5];
			size = 5;
			count = 0;
		}

		private void Reallocate ()
		{
			int [] temp = new int [size * 2];
			for (int i = 0; i < data.Length; ++i)
				temp [i] = data [i];
			size *= 2;
			data = temp;
		}

		public void AddToEnd (int stuff)
		{
			if (count == size)
			{
				Reallocate ();
			}

			data [count++] = stuff;
		}

		public void AddToAnywhere (int where, int stuff)
		{
			if (count == size)
				Reallocate ();

			//move everything up 1 spot
			for (int i = count; i > where; --i)
			{
				data [i] = data [i - 1];
			}
			data [where] = stuff;
			++count;
		}

		public void RemoveFromAnywhere (int where)
		{
			for (int i = where; i < count; ++i)
				data [i] = data [i + 1];
			--count;
		}

		public int Count ()
		{
			return count;
		}

		public int Get (int where)
		{
			if (where >= count || count < 0)
				return 0;
			return data [where];
		}
	}

	class Program
	{
		static void Main (string [] args)
		{
			FakeList fakeList = new FakeList ();
			for (int i = 0; i < 10; ++i)
				fakeList.AddToEnd (i);

			Console.WriteLine ("Filled list with 0 to 9");
			for (int i = 0; i < fakeList.Count (); ++i)
				Console.Write ("{0} ", fakeList.Get (i));
			Console.WriteLine ();

			fakeList.AddToAnywhere (4, 100);

			Console.WriteLine ("Inserted 100 to position 4");
			for (int i = 0; i < fakeList.Count (); ++i)
				Console.Write ("{0} ", fakeList.Get (i));
			Console.WriteLine ();

			fakeList.RemoveFromAnywhere (4);

			Console.WriteLine ("Removed 100 from position 4");
			for (int i = 0; i < fakeList.Count (); ++i)
				Console.Write ("{0} ", fakeList.Get (i));
			Console.WriteLine ();

			Console.ReadKey ();
		}
	}
}
