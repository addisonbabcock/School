#include "main.h"

int main ()
{
	SData* pRoot = 0;
	SData* pNew = 0;
	int iInput = 0;
	char cSelection = 0;

	while ((cSelection = Menu (pRoot)) != 'Q')
	{
		switch (cSelection)
		{
		case 'A':
			pNew = new SData;
			pNew->m_pLeft = 0;
			pNew->m_pRight = 0;
			GetInput (&(pNew->m_iData), "Enter an integer: ", INT_MIN, INT_MAX);
			AddNode (pRoot, pNew);
			break;
		case 'D':
			DisplayTree (pRoot);
			system ("pause");
			break;
		case 'K':
			DeleteTree (pRoot);
			break;
		case 'Q':
			break;
		}
	}

	DeleteTree (pRoot);

	cout << endl;
	system ("pause");
	return 0;
}

char Menu (bool bFullMenu)
{
	system ("cls");
	cout << "\t\tBinary Tree\n\n";

	if (bFullMenu)
	{
		cout << "a.\tAdd a node.\n"
			 << "d.\tDisplay Tree.\n"
			 << "k.\tKill Tree.\n"
			 << "q.\tQuit program.\n\n"
			 << "Your selection please: ";
		return toupper (GetMenuChoice ("ADKQadkq"));
	}
	else
	{
		cout << "a.\tAdd a node.\n"
			 << "q.\tQuit program.\n\n"
			 << "Your selection please: ";
		return toupper (GetMenuChoice ("AQaq"));
	}
}

void AddNode (SData* &pCurrent, SData* pNew)
{
	if (0 == pCurrent)
		pCurrent = pNew;
	else
	{
		if (pNew->m_iData < pCurrent->m_iData)
			AddNode (pCurrent->m_pLeft, pNew);
		else
			AddNode (pCurrent->m_pRight, pNew);
	}
}

void DisplayTree (SData* pCurrent)
{
	if (pCurrent)
	{
		DisplayTree (pCurrent->m_pLeft);
		cout << pCurrent->m_iData << '\t';
		DisplayTree (pCurrent->m_pRight);
	}
}

void DeleteTree (SData* &pCurrent)
{
	if (pCurrent)
	{
		DeleteTree (pCurrent->m_pLeft);
		DeleteTree (pCurrent->m_pRight);
		delete pCurrent;
		pCurrent = 0;
	}
}

void GetInput (int* const piValue, char const * const szPrompt,
			   int const iLow, int const iHigh)
{
	cout << szPrompt;
	FlushCINBuffer ();
	cin >> *piValue;

	while (*piValue >= iHigh || *piValue <= iLow || cin.fail ())
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

void FlushCINBuffer ()
{
	cin.clear ();
	cin.ignore (cin.rdbuf ()->in_avail());
}
