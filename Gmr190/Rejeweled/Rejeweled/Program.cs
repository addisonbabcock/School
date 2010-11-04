using System;

namespace Rejeweled
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			using (Rejeweled game = new Rejeweled())
			{
				game.Run();
			}
		}
	}
}

