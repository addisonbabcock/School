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
		cout << "Q. QuickSort all the students.\n"
		<< "E. Search for a student by name.\n"
		<< "D. Display a Student.\n"
		<< "S. Save to a file.\n";

	cout << "L. Load a file.\n"
		<< "X. Exit Program.\n\n"
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

int QuickSort (SStudent * const Students, int iLeft, int iRight, bool bIsTop)
{
	int ilptr,
		irptr;
	SStudent spivot,
		stemp;
	static int iSwaps;

	if (bIsTop)
		iSwaps = 0;

	ilptr = iLeft;
	irptr = iRight;

	spivot = Students [(iLeft + iRight) / 2];
	do
	{
		while (Students [ilptr].m_iGrade > spivot.m_iGrade && ilptr < iRight)
			ilptr++;
		while (spivot.m_iGrade > Students [irptr].m_iGrade && irptr > iLeft)
			irptr--;

		if (ilptr <= irptr)
		{
			stemp = Students [ilptr];
			Students [ilptr] = Students [irptr];
			Students [irptr] = stemp;
			ilptr++;
			irptr--;
			iSwaps++;
		}
	} while (ilptr <= irptr);

	if (iLeft < irptr) 
		QuickSort (Students, iLeft, irptr, false);
	if (irptr < iRight) 
		QuickSort (Students, ilptr, iRight, false);

	return iSwaps;
}

int StudentCompare (void const * sLeft, void const * sRight)
{
	return strcmpi (reinterpret_cast<SStudent const *> (sLeft)->m_szName,
		reinterpret_cast<SStudent const *> (sRight)->m_szName);
}

int main ()
{
	char cChoice = 0;
	bool bContinue = true;
	bool bIsSorted = false;
	char szSearchFor [gkiMaxStrLen] = "";
	SStudent sTheStudents [kuiMaxSize];
	SStudent* psStudentFound = 0;
	unsigned int uiStudentCount = 0;

	while (bContinue)
	{
		DisplayMenu (uiStudentCount);

		if (uiStudentCount)
			cChoice = GetMenuChoice ("iIdDlLsSqQeExX");
		else
			cChoice = GetMenuChoice ("iIlLxX");

		switch (cChoice)
		{
		case 'X':
		case 'x':
			bContinue = false;
			break;

		case 'I':
		case 'i':
			uiStudentCount = InputClass (sTheStudents);
			bIsSorted = false;
			break;

		case 'S':
		case 's':
			SaveClass (sTheStudents, uiStudentCount);
			break;

		case 'l':
		case 'L':
			uiStudentCount = LoadClass (sTheStudents);
			bIsSorted = false;
			break;

		case 'D':
		case 'd':
			if (0 == uiStudentCount)
				cout << "No students to display!\n\n";
			else
				DisplayClass (sTheStudents, uiStudentCount);

			break;
		case 'Q':
		case 'q':
			if (0 == uiStudentCount)
				cout << "No students to sort!\n\n";
			else
			{
				qsort (static_cast<void*>(sTheStudents), 
					sizeof (sTheStudents) / sizeof (sTheStudents[0]), 
					sizeof (sTheStudents [0]),
					StudentCompare);
				bIsSorted = true;
			}
			break;
		case 'E':
		case 'e':
			if (bIsSorted)
			{
				cout << "Who are you searching for? ";
				FlushCINBuffer ();
				cin.getline (szSearchFor, gkiMaxStrLen);
				psStudentFound = static_cast <SStudent*>(bsearch (szSearchFor,
															static_cast<void*>(sTheStudents),
															sizeof (sTheStudents) / sizeof (sTheStudents [0]),
															sizeof (sTheStudents [0]),
															StudentCompare));

				if (psStudentFound)
				{
					cout << psStudentFound->m_szName << " has been found and has a grade of "
						<< psStudentFound->m_iGrade << " at the address 0x"
						<< hex << psStudentFound << ".\n";
				}
				else
				{
					cout << szSearchFor << " was not found!\n";
				}
			}
			else
			{
				cout << "The array must be sorted first.\n";
			}
			break;
		}
	}

	system ("pause");
	return 0;
}

