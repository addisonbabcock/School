#include "main.h"

void FlushCINBuffer (void)
{
	cin.clear ();
	cin.ignore (cin.rdbuf()->in_avail(), '\n');
}

char GetMenuChoice (char const * const szValidChoices)
{
	char cRetVal = 0;
	char szInput [2] = " ";

	while (true)
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

void DisplayMenu (unsigned int const uiStudentCount)
{
	cout << "\n\n\t\t\tStudent Database\n\n"
		 << "There are " << uiStudentCount << " records. "
		 << "Actions available...\n\n"
		 << "I. Input a Student.\n";

	if (uiStudentCount)
		cout << "D. Display a Student.\n"
			 << "S. Save to a file.\n";

	cout << "L. Load a file.\n"
		 << "Q. Exit Program.\n\n"
		 << "Your selection: ";
}

void SaveClass (SStudent const * const Students, unsigned int const uiStudentCount)
{
	ofstream OutFile;
	char szFileName [gkiMaxStrLen] = {0};
	int iBytesToWrite = 0;

	cout << "Enter the file name: ";
	FlushCINBuffer ();
	cin.getline (szFileName, gkiMaxStrLen);

	OutFile.open (szFileName, ios::out | ios::binary);

	if (OutFile)
	{
		iBytesToWrite = uiStudentCount * sizeof (SStudent);
		OutFile.write (reinterpret_cast <char const * const> (Students), iBytesToWrite);

		cout << "File saved. Stored " << iBytesToWrite << " bytes.\n";
	}
	else
	{
		cout << "Could not open the file for saving.\n";
	}
}

unsigned int LoadClass (SStudent* const Students)
{
	ifstream InFile;
	char szFileName [gkiMaxStrLen] = {0};
	unsigned int uiStudentCount = 0;
	const unsigned int kuiMaxBytesToRead = sizeof (SStudent) * kuiMaxSize;
	unsigned int uiBytesRead = 0;

	cout << "Enter the file to load: ";
	FlushCINBuffer ();
	cin.getline (szFileName, gkiMaxStrLen);

	InFile.open (szFileName, ios::in | ios::binary);

	if (InFile)
	{
		//input file
		InFile.read (reinterpret_cast <char* const> (Students), kuiMaxBytesToRead);
		uiBytesRead = InFile.gcount ();

		cout << "File loaded. Read in " << uiBytesRead << " bytes.\n";

		uiStudentCount = uiBytesRead / sizeof (SStudent);
	}
	else
	{
		cout << "Could not open file, please check that it exists and is not in use.\n";
	}

	return uiStudentCount;
}

int main ()
{
	char cChoice = 0;
	bool bContinue = true;
	SStudent sTheStudents [kuiMaxSize];
	unsigned int uiStudentCount = 0;

	while (bContinue)
	{
		DisplayMenu (uiStudentCount);

		if (uiStudentCount)
            cChoice = GetMenuChoice ("iIdDlLsSqQ");
		else
			cChoice = GetMenuChoice ("iIlLqQ");

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

		case 'S':
		case 's':
			SaveClass (sTheStudents, uiStudentCount);
			break;

		case 'l':
		case 'L':
			uiStudentCount = LoadClass (sTheStudents);
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

