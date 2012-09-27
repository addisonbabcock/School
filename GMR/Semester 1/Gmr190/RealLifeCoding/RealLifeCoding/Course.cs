﻿//
//
//
// Filename : Course.cs
// Implementing the "Course" class designed in
// GMR101 Sept 22 2010

namespace RealLifeCoding
{
	class Course
	{
		string ID;
		string Name;
		string Classroom;
		string Instructor;
		int StudentCount;

		//Constructor
		public Course (string _ID, string _Name, string _Classroom, int _StudentCount, string _Instructor)
		{
			ID = _ID;
			Name = _Name;

			SetClassroom (_Classroom);
			SetInstructor (_Instructor);
			SetStudentCount (_StudentCount);
		}

		//setters
		public void SetStudentCount (int _StudentCount)
		{
			StudentCount = _StudentCount;
		}

		public void SetInstructor (string _Instructor)
		{
			Instructor = _Instructor;
		}

		public void SetClassroom (string _Classroom)
		{
			Classroom = _Classroom;
		}

		//getters
		public string GetID ()
		{
			return ID;
		}

		public string GetName ()
		{
			return Name;
		}

		public int GetStudentCount ()
		{
			return StudentCount;
		}

		public string GetInstructor ()
		{
			return Instructor;
		}

		public string GetClassroom ()
		{
			return Classroom;
		}

		//overrides
		public override string ToString ()
		{
			string accumulator = "";

			accumulator += "ID: " + GetID ();
			accumulator += "\nName: " + GetName ();
			accumulator += "\nStudentCount: " + GetStudentCount ();
			accumulator += "\nInstructor: " + GetInstructor ();
			accumulator += "\nClassroom: " + GetClassroom ();
			accumulator += "\n";

			return accumulator;
		}
	}
}