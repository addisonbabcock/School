using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab01
{
	class Program
	{
		private static Random rand;
		private static List<int> key;
		private static List<int> guess;

		public static int GrabInt (string message, int min, int max)
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

				if (ret < min || ret > max)
				{
					Console.WriteLine ("Value entered must be between {0} and {1}.", min, max);
					continue;
				}

				return ret;
			}
		}

		static char ProcessMenu (string message, string validOptions)
		{
			char ret = ' ';

			do
			{
				Console.Write (message);
				string input = Console.ReadLine ();

				if (input.Length != 1)
				{
					Console.WriteLine ("Please select only one of the possibilities: " + validOptions);
					continue;
				}

				ret = input.ToUpper ()[0];
				if (validOptions.Contains (ret))
					return ret;
			} while (true);
		}

		static void Main (string[] args)
		{
			rand = new Random ();
			key = new List<int> ();
			guess = new List<int> ();

			GenerateKey (key);

			ShowKey (key);

			ProcessMainLoop ();
		}

		private static void ProcessMainLoop ()
		{
			char keepPlaying = 'Y';
			int guesses = 0;
			int exactMatches = 0;
			int closeMatches = 0;

			do
			{
				GetGuess (guess);
				exactMatches = CalculateExactMatches (key, guess);
				closeMatches = CalculateCloseMatches (key, guess);
				ShowResults (exactMatches, closeMatches);

				if (exactMatches == 4)
				{
					Console.WriteLine ("Y O U  W I N !");
					Console.ReadKey ();
					break;
				}

				++guesses;
				keepPlaying = ProcessMenu ("Would you like to keep playing (Y/N)? ", "YN");
			} while (keepPlaying == 'Y' && guesses < 10);
		}

		private static void ShowResults (int exactMatches, int closeMatches)
		{
			Console.WriteLine ("{0}A {1}B", exactMatches, closeMatches);
		}

		private static int CalculateCloseMatches (List<int> key, List<int> guess)
		{
			int count = 0;

			for (int i = 0; i < key.Count; ++i)
			{
				for (int j = 0; j < guess.Count; ++j)
				{
					if (i != j) //if they are in the same spot, they are an exact match
					{
						if (key[i] == guess[j])
							++count;
					}
				}
			}

			return count;
		}

		private static int CalculateExactMatches (List<int> key, List<int> guess)
		{
			int count = 0;

			for (int i = 0; i < key.Count && i < guess.Count; ++i)
			{
				if (key[i] == guess[i])
					++count;
			}

			return count;
		}

		private static void GetGuess (List<int> guess)
		{
			guess.Clear ();
			for (int i = 0; i < 4; ++i)
			{
				guess.Add (GrabInt ("Please enter a guess: ", 0, 9));
			}
		}

		private static void GenerateKey (List<int> key)
		{
			do
			{
				key.Clear ();
				for (int i = 0; i < 4; ++i)
					key.Add (rand.Next (10));
			} while (!VerifyUniqueness (key));
		}

		private static void ShowKey (List<int> key)
		{
#if DEBUG
			foreach (int i in key)
			{
				Console.Write ("{0} ", i);
			}
			Console.WriteLine ("");
#endif
		}

		private static bool VerifyUniqueness (List <int> key)
		{
			for (int i = 0; i < key.Count; ++i)
			{
				for (int j = 0; j < key.Count; ++j)
				{
					if (i != j)
					{
						if (key [i] == key [j])
						{
							return false;
						}
					}
				}
			}

			return true;
		}
	}
}
