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

		//HACK?: turning cRetVal into a string so strstr will accept it
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

void InputRecord (SStudent * const pStudentToInput)
{
	cout << "Enter student name: ";
	FlushCINBuffer ();
	cin.getline (pStudentToInput->m_szName, gkiMaxStrLen);

	cout << "Enter Student Grade: ";
	FlushCINBuffer ();
	cin >> pStudentToInput->m_iGrade;

	while (pStudentToInput->m_iGrade < 0 || 
		   pStudentToInput->m_iGrade > 100 ||
		   cin.fail ())
	{
		cout << "Grade was out of bounds, please try again: ";
		FlushCINBuffer ();
		cin >> pStudentToInput->m_iGrade;
	}

	return;
}

void DisplayRecord (SStudent const * const pStudentToDisplay)
{
	cout << "Name: " << pStudentToDisplay->m_szName << endl
		 << "Grade: " << pStudentToDisplay->m_iGrade << endl;
}

void DisplayMenu (void)
{
	cout << "\t\t\tStudent Database\n\n"
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
	bool bStudentInitialized = false;
	SStudent sTheStudent = {{0}, -1};

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
			InputRecord (&sTheStudent);
			bStudentInitialized = true;
			break;
		
		case 'D':
		case 'd':
			if (bStudentInitialized)
				DisplayRecord (&sTheStudent);
			else
				cout << "You need to enter a student first!\n\n";

			break;
		}
	}

	system ("pause");
	return 0;
}