//
//
//
// Filename : Program.cs
// Implementing the test driver designed in
// GMR101. Sept 22 2010

using System;

namespace RealLifeCoding
{
	class Program
	{
		static void Main (string [] args)
		{
			Course my101course = new Course ("GMR101", "Intro", "WB306", 11, "Jay");

			Console.WriteLine (my101course.ToString ());
			Console.WriteLine ();

			my101course.SetClassroom ("WB312");
			my101course.SetStudentCount (22);
			my101course.SetInstructor ("Someone");

			Console.WriteLine (my101course.ToString ());

			Console.ReadKey ();
		}
	}
}
