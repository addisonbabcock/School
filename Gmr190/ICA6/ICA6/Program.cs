using System;

namespace ICA6
{
	class Program
	{
		public static int GrabInt (string message)
		{
			int ret = 0;
			while (true)
			{
				try
				{
					Console.Write (message);
					ret = Convert.ToInt32 (Console.ReadLine ());
				}
				catch (Exception e)
				{
					Console.WriteLine ("Invalid input: " + e.Message);
					continue;
				}

				return ret;
			}
		}

		static void Main (string [] args)
		{
			int health;
			int damage;
			string healthMessage = "Please enter your player's health: ";
			string damageMessage = "Please enter the amount of damage: ";

			health = GrabInt (healthMessage);

			while (health > 0)
			{
				damage = GrabInt (damageMessage);
				health -= damage;
			}

			Console.WriteLine ("Player is dead.");
			Console.ReadKey ();
		}
	}
}
