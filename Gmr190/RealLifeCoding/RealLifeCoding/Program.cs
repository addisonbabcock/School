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
			try
			{
				Console.WriteLine ("Welcome to NAIT course registration system!");

				Console.Write ("Please enter course ID: ");
				string courseID = Console.ReadLine ();

				Console.Write ("Please enter course name: ");
				string courseName = Console.ReadLine ();

				Console.Write ("Please enter the room number: ");
				string room = Console.ReadLine ();

				Console.Write ("Please enter the number of students: ");
				string studentCount = Console.ReadLine ();

				Console.Write ("Please enter the name of the instructor: ");
				string instructor = Console.ReadLine ();

				int nStudentCount = Convert.ToInt32 (studentCount);

				Course my101course = new Course (
					courseID,
					courseName,
					room,
					Convert.ToInt32 (studentCount),
					instructor);

				Console.WriteLine ();
				Console.WriteLine ("Process complete!");
				Console.WriteLine (my101course.ToString ());
				Console.WriteLine ();
			}
			catch (Exception e)
			{
				Console.WriteLine ("Failed to convert input to integer: " + e.Message);
			}
			finally
			{
				Console.ReadKey ();
			}

/*			my101course.SetClassroom ("WB312");
			my101course.SetStudentCount (22);
			my101course.SetInstructor ("Someone");

			Console.WriteLine (my101course.ToString ());

			Console.ReadKey ();*/
		}
	}
}
