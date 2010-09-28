using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICA6
{
	class Program
	{
		static void Main (string [] args)
		{
			int health;
			int damage;

			try
			{
				Console.Write ("Please enter your player's health: ");
				health = Convert.ToInt32 (Console.ReadLine ());

				while (health > 0)
				{
					Console.Write ("Please enter the amount of damage: ");
					damage = Convert.ToInt32 (Console.ReadLine ());

					health -= damage;
				}

				Console.WriteLine ("Player is dead.");
			}
			catch (Exception e)
			{
				Console.WriteLine ("Some error occured: " + e.Message);
			}
			finally
			{
				Console.ReadKey ();
			}
		}
	}
}
