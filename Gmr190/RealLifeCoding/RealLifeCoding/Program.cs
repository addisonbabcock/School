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

			Console.WriteLine ("ID: {0}", my101course.GetID ());
			Console.WriteLine ("Name: {0}", my101course.GetName ());
			Console.WriteLine ("Classroom: {0}", my101course.GetClassroom ());
			Console.WriteLine ("Number of students: {0}", my101course.GetStudentCount ());
			Console.WriteLine ("Instructor: {0}", my101course.GetInstructor ());
			Console.WriteLine ();

			my101course.SetClassroom ("WB312");
			my101course.SetStudentCount (22);
			my101course.SetInstructor ("Someone");

			Console.WriteLine ("Classroom: {0}", my101course.GetClassroom ());
			Console.WriteLine ("Number of students: {0}", my101course.GetStudentCount ());
			Console.WriteLine ("Instructor: {0}", my101course.GetInstructor ());

			Console.ReadKey ();
		}
	}
}
