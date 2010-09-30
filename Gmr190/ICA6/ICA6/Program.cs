﻿using System;

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
			int health = 0;
			int damage = 0;
			Random rand = new Random ();

			health = GrabInt ("Please enter your player's health: ");

			while (health > 0)
			{
				//damage = GrabInt ("Please enter the amount of damage: ");
				damage = rand.Next (health) + 1;
				health -= damage;

				Console.WriteLine ("Player health remaining: {0}", health);
			}

			Console.WriteLine ("Player is dead.");
			Console.ReadKey ();
		}
	}
}