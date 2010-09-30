using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace do_while_exercise3
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

		public static double GrabDouble (string message)
		{
			double ret = 0;
			while (true)
			{
				try
				{
					Console.Write (message);
					ret = Convert.ToDouble (Console.ReadLine ());
				}
				catch (Exception e)
				{
					Console.WriteLine ("Invalid input: " + e.Message);
					continue;
				}

				return ret;
			}
		}

		static public char GetLetterGrade (double mark)
		{
			if (mark >= 60)
			{
				if (mark >= 70)
				{
					if (mark >= 80)
					{
						return 'A';
					}

					return 'B';
				}

				return 'C';
			}

			return 'F';
		}

		static void Main (string [] args)
		{
			int groupSize = 0;
			int count = 0;
			string studentName = "";
			double studentAverage = 0.0;

			double mark110 = 0.0;
			double mark115 = 0.0;
			double mark135 = 0.0;
			double mark140 = 0.0;
			double mark185 = 0.0;
			
			double running110 = 0.0;
			double running115 = 0.0;
			double running135 = 0.0;
			double running140 = 0.0;
			double running185 = 0.0;
	
			char grade = 'F';

			double overallAverage = 0.0;
			double average110 = 0.0;
			double average115 = 0.0;
			double average135 = 0.0;
			double average140 = 0.0;
			double average185 = 0.0;

			groupSize = GrabInt ("Enter the size of the group: ");

			if (groupSize > 0)
			{
				do
				{
					Console.Write ("Enter the students name: ");
					studentName = Console.ReadLine ();

					mark110 = GrabDouble ("Enter 110 mark: ");
					mark115 = GrabDouble ("Enter 115 mark: ");
					mark135 = GrabDouble ("Enter 135 mark: ");
					mark140 = GrabDouble ("Enter 140 mark: ");
					mark185 = GrabDouble ("Enter 185 mark: ");

					running110 += mark110;
					running115 += mark115;
					running135 += mark135;
					running140 += mark140;
					running185 += mark185;

					studentAverage = (mark110 + mark115 + mark135 + mark140 + mark185) / 5;
					grade = GetLetterGrade (studentAverage);

					Console.WriteLine ("{0} {1} {2}", studentName, studentAverage, grade);
					++count;
				} while (count < groupSize);

				average110 = running110 / count;
				average115 = running115 / count;
				average135 = running135 / count;
				average140 = running140 / count;
				average185 = running185 / count;

				overallAverage = (average110 + average115 + average135 + average140 + average185) / 5;

				Console.WriteLine ("");
				Console.WriteLine ("Averages by course: ");
				Console.WriteLine ("Overall 110 average: {0}", average110);
				Console.WriteLine ("Overall 115 average: {0}", average115);
				Console.WriteLine ("Overall 135 average: {0}", average135);
				Console.WriteLine ("Overall 140 average: {0}", average140);
				Console.WriteLine ("Overall 185 average: {0}", average185);
				Console.WriteLine ("");
				Console.WriteLine ("Overall: ");
				Console.WriteLine ("Overall Average: {0}", overallAverage);
			}

			Console.ReadKey ();
		}
	}
}
