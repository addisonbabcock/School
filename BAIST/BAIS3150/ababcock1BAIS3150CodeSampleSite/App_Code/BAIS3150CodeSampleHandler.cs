using Microsoft.VisualBasic;
using System;

public class BAIS3150CodeSampleHandler
{
    public bool CreateProgram(string ProgramCode, string Description)
	{
		try
		{
            var ProgramManager = new Programs();
            bool Confirmation = ProgramManager.AddProgram(ProgramCode, Description);
            return Confirmation;
		}
        catch (Exception e)
		{
			return false;
		}
	}

	public Program FindProgram(string ProgramCode)
	{
		var ProgramManager = new Programs();
		var ActiveProgram = ProgramManager.GetProgram(ProgramCode);
		return ActiveProgram;
	}

    public bool EnrollStudent(Student AcceptedStudent, string ProgramCode)
	{
		try
		{
			var StudentManager = new Students();
            var Confirmation = StudentManager.AddStudent(AcceptedStudent, ProgramCode);
            return Confirmation;
		}
        catch (Exception ex)
		{
            return false;
		}
	}

    public Student FindStudent(string StudentID)
	{
        var StudentManager = new Students();
        var EnrolledStudent = StudentManager.GetStudent(StudentID);
        return EnrolledStudent;
	}

	public bool UpdateStudent(Student EnrolledStudent)
	{
		var StudentManager = new Students();
		var Confirmation = StudentManager.UpdateStudent(EnrolledStudent);
		return Confirmation;
	}

	public bool RemoveStudent(string StudentID)
	{
		var StudentManager = new Students();
		var Confirmation = StudentManager.DeleteStudent(StudentID);
		return Confirmation;
	}
}
