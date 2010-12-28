//TODO:
//Modify to use pass by reference
//Finish up the AddNode function

#include "main.h"

void FlushCINBuffer ()
{
	cin.clear ();
	cin.ignore (cin.rdbuf()->in_avail());
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

char Menu (bool bFullMenu)
{
	system ("cls");
	cout << "Linked List\n\n";
	if (bFullMenu)
	{
		cout << "a. Display nodes forward.\n"
			<< "b. Display nodes in reverse.\n"
			<< "c. Add a node.\n"
			<< "d. Delete a node.\n"
			<< "e. Exit program.\n";
		cout << "Your input please: ";
		return tolower (GetMenuChoice ("ABCDEabcde"));
	}
	else
	{
		cout << "c. Add a node.\n"
			<< "e. Exit program.\n";
		cout << "Your input please: ";
		return tolower (GetMenuChoice ("CEce"));
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

SData* FindNode (SData* pHead, int iQuery)
{
	SData* pCurrent = pHead;

	while (pCurrent->m_iX != iQuery)
		if ((pCurrent = pCurrent->m_pNext) == 0)
			return 0;

	return pCurrent;
}

SData* FindNodePrior (SData* pHead, int iQuery)
{
	SData* pCurrent = pHead;

	while (pCurrent->m_pNext && pCurrent->m_pNext->m_iX != iQuery)
		pCurrent = pCurrent->m_pNext;

	if (pCurrent->m_pNext == 0)
		return 0;

	if (pCurrent->m_pNext->m_iX == iQuery)
		return pCurrent;
	else
		return 0;
}

void AddNode (SData* &pHead, int iData)
{
	SData* pCurrent = 0;
	SData* pNew = 0;

	if (0 == pHead)
		//Add to head, empty list
	{
		pHead = new SData;
		pHead->m_iF = 1;
		pHead->m_iX = iData;
		pHead->m_pNext = 0;
		return;
	}

	//non-empty list, try to find the data in it
	pCurrent = FindNode (pHead, iData);	
	//if it was found, increase the freq and we are done
	if (0 != pCurrent)
	{
		pCurrent->m_iF++;
		return;
	}

	if (pHead->m_iX > iData)
	{
		//add to head, non-empty list
		pNew = new SData;
		pNew->m_iF = 1;
		pNew->m_iX = iData;
		pNew->m_pNext = pHead;
		pHead = pNew;
		return;
	}

	//work on me!
	pCurrent = pHead;
	while (pCurrent->m_iX < iData)
	{
		if (pCurrent->m_pNext == 0)
		{
			//Add to tail
			pNew = new SData;
			pNew->m_iX = iData;
			pNew->m_iF = 1;
			pNew->m_pNext = 0;
			pCurrent->m_pNext = pNew;
			return;
		}
		else
			pCurrent = pCurrent->m_pNext;
	}
	//Add to middle
	//pCurrnt = FindNodePrior (pHead, iData);
	pCurrent = pHead;
	while (pCurrent->m_pNext->m_iX < iData)
		pCurrent = pCurrent->m_pNext;
	pNew = new SData;
	pNew->m_pNext = pCurrent->m_pNext;
	pCurrent->m_pNext = pNew;
	pNew->m_iF = 1;
	pNew->m_iX = iData;
}

void DeleteNode (SData* &pHead, int iData)
{
	SData* pPrev = 0;
	SData* pDelete = 0;

	//node at head?
	if (pHead->m_iX == iData)
	{
		pDelete = pHead;
		pHead = pHead->m_pNext;
		delete pDelete;
		pDelete = 0;
		return;
	}

	//attempt to find the node
	pPrev = FindNodePrior (pHead, iData);

	//the node wasnt found
	if (pPrev == 0)
	{
		cout << "Node could not be found!\n";
		system ("Pause");
		return;
	}

	//node at tail?
	if (0 == pPrev->m_pNext)
	{
		//node wasnt really found
		//TODO:
		//	may never execute?
		if (pPrev->m_pNext->m_iX != iData)
		{
			cout << "Node could not be found!\n";
			system ("pause");
			return;
		}
		else
		{
			//node is at tail
			delete pPrev->m_pNext;
			pPrev->m_pNext = 0;
		}
	}
	else
	//node in the middle
	{
		pDelete = pPrev->m_pNext;
		pPrev->m_pNext = pPrev->m_pNext->m_pNext;
		delete pDelete;
		pDelete = 0;
	}
}

void DisplayForward (SData* pCur)
{
	while (pCur)
	{
		cout << pCur->m_iX << 'x' << pCur->m_iF << '\t';
		pCur = pCur->m_pNext;
	}
}

void DisplayReverse (SData* pCur)
{
	if (pCur != 0)
	{
		DisplayReverse (pCur->m_pNext);
		cout << pCur->m_iX << 'x' << pCur->m_iF << '\t';
	}
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
	int iData = 0;
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
			//AddToHead (&pHead);
			GetInput (&iData, "Enter a value: ", INT_MIN, INT_MAX);
			AddNode (pHead, iData);
			break;
		case 'd':
			GetInput (&iData, "Enter a value: ", INT_MIN, INT_MAX);
			DeleteNode (pHead, iData);
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