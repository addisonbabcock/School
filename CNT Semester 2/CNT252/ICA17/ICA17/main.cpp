#include <crtdbg.h>
#include <iostream>
#include "main.h"

using namespace std;

void FlushCINBuffer (void)
{
	cin.clear ();
	//Clears out the entire cin buffer
	cin.ignore (cin.rdbuf ()->in_avail (), '\n');
}

void GetInput (int* const piValue, char const * const szPrompt,
			   int const iLow, int const iHigh)
{
	cout << szPrompt;
	FlushCINBuffer ();
	cin >> *piValue;

	while (*piValue >= iHigh || *piValue < iLow || cin.fail ())
	{
		cout << szPrompt;
		FlushCINBuffer ();
		cin >> *piValue;
	}
}

char GetMenuChoice (char const * const szValidChoices)
{
	//The users input
	char cRetVal = 0;
	//The users input in string format (temporary variable)
	char szInput [2] = " ";

	//NOTE: this will not loop infinitely
	while (true)
	{
		//Get the menu option
		FlushCINBuffer ();
		cin >> cRetVal;

		//turning cRetVal into a string so strstr () will accept it
		szInput [0] = cRetVal;
		szInput [1] = 0;

		//If the input is a valid option
		if (strstr (szValidChoices, szInput))
		{
			//Return the input
			return cRetVal;
		}
		else
		{
			//display an error message and loop back to the top
			cout << "Invalid selection, please try again: ";
		}
	}

	//This will never execute, it's just to keep
	//compiler warnings to a minimum
	return 0;
}

char Menu (bool bFullMenu)
{
	system ("cls");

	if (bFullMenu)
	{
		cout << "\t\t\tDynamic Array Of Integers\n"
			 << "Please make a selection...\n"
			 << "a. Add an integer to array.\n"
			 << "x. Delete an integer from array\n"
			 << "d. Display the array.\n"
			 << "q. Quit program.\n"
			 << "Your selection: ";
		return toupper (GetMenuChoice ("AaXxDdQq"));
	}
	else
	{
		cout << "\t\t\tDynamic Array Of Integers\n"
			 << "Please make a selection...\n"
			 << "a. Add an integer to array.\n"
			 << "q. Quit program.\n"
			 << "Your selection: ";
		return toupper (GetMenuChoice ("AaQq"));
	}
}

void AddToArray (int** ppiData, int* piMaxSize)
{
	int* piTemp = 0;

	*piMaxSize += 1;
	piTemp = new int [*piMaxSize];
	for (int i = 0; i < (*piMaxSize) - 1; i++)
	{
		piTemp [i] = (*ppiData) [i];
	}

	delete [] *ppiData;
	*ppiData = piTemp;

	cout << "Enter an integer: ";
	FlushCINBuffer ();
	cin >> piTemp [*piMaxSize - 1];
}

void DeleteFromArray (int** ppiData, int* iSize)
{
	int* piTemp = 0;
	int iDelIndex = 0;

	DisplayArray (*ppiData, *iSize);
	GetInput (&iDelIndex, "Delete which index? ", 0, *iSize);

	*iSize -= 1;
	piTemp = new int [*iSize];

	for (int i = 0; i < iDelIndex; i++)
		piTemp [i] = (*ppiData) [i];

	for (int i = iDelIndex; i < *iSize; i++)
		piTemp [i] = (*ppiData) [i + 1];

	delete [] *ppiData;
	*ppiData = piTemp;
}

void DisplayArray (const int iArray [], const int iSize)
{
	//for each element in the array
	for (int i = 0; i < iSize; i++)
	{
		cout << "Array[";
		cout << i;
		cout << "] = ";
		cout << iArray [i];
		cout << "\n";
	}
	cout << endl;
}

int main ()
{
	int* piData = 0;
	int iArraySize = 0;

	char cInput = 0;

	_CrtSetDbgFlag (_CRTDBG_ALLOC_MEM_DF |
					_CRTDBG_LEAK_CHECK_DF |
					_CRTDBG_CHECK_ALWAYS_DF);

	while ('Q' != cInput)
	{
		cInput = Menu (iArraySize > 0);

		switch (cInput)
		{
		case 'A':
			AddToArray (&piData, &iArraySize);
			break;
		case 'X':
			DeleteFromArray (&piData, &iArraySize);
			break;
		case 'D':
			DisplayArray (piData, iArraySize);
			system ("pause");
			break;
		case 'Q':
			break;
		}
	}

	delete [] piData;
}