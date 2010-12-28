#include "main.h"

void FlushCINBuffer ()
{
	cin.clear ();
	cin.ignore (cin.rdbuf()->in_avail());
}

char Menu (bool bFullMenu)
{
	system ("cls");
	cout << "Linked List\n\n";
	if (bFullMenu)
	{
		cout << "a. Display nodes forward.\n"
			 << "b. Display nodes in reverse.\n"
			 << "c. Add a node to the head.\n"
			 << "d. Add a node to the tail.\n"
			 << "e. Exit program.\n";
		cout << "Your input please: ";
		return tolower (GetMenuChoice ("ABCDEabcde"));
	}
	else
	{
		cout << "c. Add a node to the head.\n"
			 << "d. Add a node to the tail.\n"
			 << "e. Exit program.\n";
		cout << "Your input please: ";
		return tolower (GetMenuChoice ("CDEcde"));
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

void DisplayForward (SData* pCur)
{
	while (pCur)
	{
		cout << pCur->m_iX << '\t';
		pCur = pCur->m_pNext;
	}
}

void DisplayReverse (SData* pCur)
{
	if (pCur != 0)
	{
		DisplayReverse (pCur->m_pNext);
		cout << pCur->m_iX << '\t';
	}
}

void AddToHead (SData** pHead)
{
	SData* pNew = new(nothrow) SData;

	pNew->m_pNext = *pHead;
	*pHead = pNew;

	cout << "Enter a number: ";
	cin >> pNew->m_iX;
}

void AddToEnd (SData** pHead)
{
	SData* pCurrent = *pHead;
	SData* pNew = new SData;

	pNew->m_pNext = 0;

	if (!*pHead)
	{
		*pHead = pNew;
	}
	else
	{
		while (pCurrent->m_pNext)
			pCurrent = pCurrent->m_pNext;
		pCurrent->m_pNext = pNew;
	}

	cout << "Enter a number: ";
	cin >> pNew->m_iX;
}

void DeleteList (SData* pHead)
{
	SData* pDel = 0;
	SData* pCur = pHead;

	while (pCur)
	{
		pDel = pCur;
		pCur = pCur->m_pNext;
		delete pDel;
	}
}

int main ()
{
	char cInput = 0;
	SData* pHead=  0;

	_CrtSetDbgFlag (_CRTDBG_ALLOC_MEM_DF |
					_CRTDBG_LEAK_CHECK_DF |
					_CRTDBG_CHECK_ALWAYS_DF);

	while (cInput != 'e')
	{
		cInput = Menu (0 != pHead);

		switch (cInput)
		{
		case 'a':
			DisplayForward (pHead);
			system ("pause");
			break;
		case 'b':
			DisplayReverse (pHead);
			system ("pause");
			break;
		case 'c':
			AddToHead (&pHead);
			break;
		case 'd':
			AddToEnd (&pHead);
			break;
		case 'e':
			break;
		}
	}

	DeleteList (pHead);

	cout << endl;
	system ("pause");
	return 0;
}