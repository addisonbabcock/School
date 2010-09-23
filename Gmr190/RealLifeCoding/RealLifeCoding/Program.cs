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
			Course GMR = new Course ("GMR101", "Intro", "WB306", 11, "Jay");

			Console.WriteLine ("ID: " + GMR.GetID ());
			Console.WriteLine ("Name: " + GMR.GetName ());
			Console.WriteLine ("Classroom: " + GMR.GetClassroom ());
			Console.WriteLine ("Number of students: " + GMR.GetStudentCount ());
			Console.WriteLine ("Instructor: " + GMR.GetInstructor ());
			Console.WriteLine ();

			GMR.SetClassroom ("WB312");
			GMR.SetStudentCount (22);
			GMR.SetInstructor ("Someone");

			Console.WriteLine ("Classroom: " + GMR.GetClassroom ());
			Console.WriteLine ("Number of students: " + GMR.GetStudentCount ());
			Console.WriteLine ("Instructor: " + GMR.GetInstructor ());

			Console.ReadKey ();
		}
	}
}
