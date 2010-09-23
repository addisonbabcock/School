//
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

		public Course (string _ID, string _Name, string _Classroom, int _StudentCount, string _Instructor)
		{
			ID = _ID;
			Name = _Name;

			SetClassroom (_Classroom);
			SetInstructor (_Instructor);
			SetStudentCount (_StudentCount);
		}

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
	}
}
