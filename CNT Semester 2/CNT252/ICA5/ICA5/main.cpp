#include "main.h"

void FlushCINBuffer (void)
{
	cin.clear ();
	cin.ignore (cin.rdbuf()->in_avail(), '\n');
}

char GetMenuChoice (char const * const szValidChoices)
{
	bool bInputOK = false;
	char cRetVal = 0;
	char szInput [2] = " ";

	while (!bInputOK)
	{
		FlushCINBuffer ();
		cin >> cRetVal;

		//turning cRetVal into a string so strstr () will accept it
		szInput [0] = cRetVal;
		szInput [1] = 0;

		if (strstr (szValidChoices, szInput))
		{
			return cRetVal;
		}
		else
		{
			cout << "Invalid selection, please try again: ";
		}
	}

	//This will never execute, it's just to keep
	//compiler warnings to a minimum
	return 0;
}

unsigned int InputClass (SStudent sStudentsToInput [kuiMaxSize])
{
	unsigned int uiStudentCount = 0;
	char cYorN = 'y';

	while (cYorN == 'y' || cYorN == 'Y')
	{
		cout << "Enter student name: ";
		FlushCINBuffer ();
		cin.getline (sStudentsToInput[uiStudentCount].m_szName, gkiMaxStrLen);

		cout << "Enter Student Grade: ";
		FlushCINBuffer ();
		cin >> sStudentsToInput[uiStudentCount].m_iGrade;

		while (	sStudentsToInput[uiStudentCount].m_iGrade < 0 || 
				sStudentsToInput[uiStudentCount].m_iGrade > 100 ||
				cin.fail ())
		{
			cout << "Grade was out of bounds, please try again: ";
			FlushCINBuffer ();
			cin >> sStudentsToInput[uiStudentCount].m_iGrade;
		}

		uiStudentCount++;
		
		if (uiStudentCount != kuiMaxSize)
		{
			cout << "Enter <y> for another student: ";
			cYorN = GetMenuChoice ("yYnN");
		}
		else
		{
			cout << "Max database size has been reached, no further students could be stored." << endl << endl;
			return uiStudentCount;
		}
	}
	cout << endl;

	return uiStudentCount;
}

void DisplayClass (SStudent const sStudentToDisplay [kuiMaxSize], unsigned int const uiStudentsToDisplay)
{
	for (unsigned int i = 0; i < uiStudentsToDisplay; i++)
	{
		cout << "Name: " << sStudentToDisplay[i].m_szName << endl
			 << "Grade: " << sStudentToDisplay[i].m_iGrade << endl << endl;
	}
	cout << endl;
}

void DisplayMenu (void)
{
	cout << "\n\n\t\t\tStudent Database\n\n"
		<< "Actions available...\n\n"
		<< "I. Input a Student.\n"
		<< "D. Display a Student.\n"
		<< "Q. Exit Program.\n\n"
		<< "Your selection: ";
}		

int main ()
{
	char cChoice = 0;
	bool bContinue = true;
	SStudent sTheStudents [kuiMaxSize];
	unsigned int uiStudentCount = 0;

	while (bContinue)
	{
		DisplayMenu ();
		cChoice = GetMenuChoice ("iIdDqQ");

		switch (cChoice)
		{
		case 'Q':
		case 'q':
			bContinue = false;
			break;

		case 'I':
		case 'i':
			uiStudentCount = InputClass (sTheStudents);
			break;

		case 'D':
		case 'd':
			if (0 == uiStudentCount)
				cout << "No students to display!\n\n";
			else
                DisplayClass (sTheStudents, uiStudentCount);

			break;
		}
	}

	system ("pause");
	return 0;
}

