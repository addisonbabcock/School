/***************************************************\
* Project:		Lab 6 - Linked Lists				*
* Files:		main.cpp, main.h					*
* Date:			01 Dec 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	Displays word count statistics based*
*				on a text file.						*
\***************************************************/
#include "main.h"

/*******************************************************\
| Function: bool LoadFile (SData* &pHead,				|
|						   char* szFileName)			|
| Description: Loads a file and stores the length of	|
|	the words and frequency that words of that length	|
|	appear in a linked list.							|
| Parameters: A reference to the head of the linked		|
|	list. This will be changed during the loading and	|
|	anything stored in it will be deleted. szFileName	|
|	is the path to the file to be loaded.				|
| Returns: A boolean, true if the file could be loaded,	|
|	false if it couldn't.								|
\*******************************************************/
bool LoadFile (SData* &pHead, char* szFileName)
{
	//the file
	fstream file;
	//a line read in from the file
	char szBuffer [65536] = {0};
	char *szSubstr = 0;

	//unload the previous file if needed
	if (pHead)
	{
		DeleteList (pHead);
		pHead = 0;
	}

	//try to open the file
	file.open (szFileName, ios::in);

	//file does not exist?
	if (!file)
	{
		pHead = 0;
		return false;
	}

	//read in all lines of the file
	while (true)
	{
		//get a line of text
		file.getline (szBuffer, 65536);

		//process the line
		//get the first substring
		szSubstr = strtok (szBuffer, " \n\t");
		//while there are still words in the line left to be processed
		while (szSubstr)
		{
			//add the current substring to the list
			AddNode (pHead, static_cast <int> (strlen (szSubstr)));
			//get the next substring
			szSubstr = strtok (0, " \n\t");
		}

		//if the file is completely read in, exit the loop
		if (file.eof ())
			break;
	}

	//close the file
	file.close ();
	//tell the calling function that the file was loaded properly
	return true;
}

/*******************************************************\
| Function: void AddNode (SData* &pHead, int iData)		|
| Description: Adds a node to the linked list. Will		|
|	properly handle append to end, add to head as well	|
|	as add to middle techniques. The resulting list		|
|	will be sorted based on m_iWordSize. This function	|
|	also handles allocating the memory for the new node.|
|	If the list already contains iData, then the		|
|	m_iWordCount for that node will be incremented.		|
| Parameters: pHead is a pointer to the head of the		|
|	list. This may be changed! iData is the WordSize to |
|	be added to the list.								|
| Returns: None.										|
\*******************************************************/
void AddNode (SData* &pHead, int iData)
{
	//a "loop counter", sort of
	SData* pCur = 0;
	//the new node to be added
	SData* pNew = 0;

	//is the list empty?
	if (!pHead)
	{
		//empty list, add to the head
		pHead = new SData;
		//the head is the only node
		pHead->m_pNext = 0;
		//only one word of iData size is in the list
		pHead->m_iWordCount = 1;
		//set the word size
		pHead->m_iWordSize = iData;
		return;
	}

	//dont bother going through the list if we found whats needed at the front
	if (iData == pHead->m_iWordSize)
	{
		//a word of that size has already been added, just increment the 
		//count and be done with it
		pHead->m_iWordCount++;
		return;
	}

	//add to head
	if (iData < pHead->m_iWordSize)
	{
		//data belongs at the beginning, add to the head
		//this basically works in the same way as the above add to head bit
		pNew = new SData;
		pNew->m_iWordCount = 1;
		pNew->m_iWordSize = iData;
		pNew->m_pNext = pHead;
		pHead = pNew;
		return;
	}

	//node not immediately found, go through the list
	//at this point, the data needed may or may not exist in the list,
	//at might be at the beginning or the end
	pCur = pHead;
	//go through the entire list, or until we went too far
	while (pCur && pCur->m_iWordSize <= iData)
	{
		//iData was found in the list, increment the counter and exit
		if (pCur->m_iWordSize == iData)
		{
			pCur->m_iWordCount++;
			return;
		}
		pCur = pCur->m_pNext;
	}

	//at this point, the data was not already in the list so we will need
	//to figure out where to insert it.

	//try to insert the node to the end of the list
	pCur = pHead;
	//while we still havent gone too far into the list
	while (pCur->m_iWordSize < iData)
	{
		//is this the end?
		if (pCur->m_pNext == 0)
		{
			//Add to tail
			pNew = new SData;
			pNew->m_iWordSize = iData;
			pNew->m_iWordCount = 1;
			pNew->m_pNext = 0;
			pCur->m_pNext = pNew;
			return;
		}
		else
			pCur = pCur->m_pNext;
	}

	//now that finding the data in the list, adding to the head and adding to
	//the tail have all failed, the only other option is insert to middle.

	//go through the entire list to find the node before where the new node 
	//belongs
	pCur = pHead;
	while (pCur->m_pNext->m_iWordSize < iData)
		pCur = pCur->m_pNext;

	//now that we know where to insert the new node, create it and insert it
	pNew = new SData;
	pNew->m_pNext = pCur->m_pNext;
	pCur->m_pNext = pNew;
	pNew->m_iWordCount = 1;
	pNew->m_iWordSize = iData;
}

/*******************************************************\
| Function: void DisplayList (SData* pHead)				|
| Description: Displays a linked list on the screen.	|
|	Data shown includes how many words of x size.		|
| Parameters: pHead is a pointer to the head of the		|
|	list.												|
| Returns: None.										|
\*******************************************************/
void DisplayList (SData* pHead)
{
	//Start at the start
	SData* pCur = pHead;

	//go through each node
	while (pCur)
	{
		//show it on the screen
		cout << setw(5) << setfill (' ') << left << pCur->m_iWordCount 
			<< "words of length " << pCur->m_iWordSize << endl;
		//and advance to the next one
		pCur = pCur->m_pNext;
	}
}

/*******************************************************\
| Function: void InsertionSortList (SData* &pHead)		|
| Description: Sorts a list using the insertion method.	|
|	Sorting is done based on m_iWordCount and works by	|
|	pulling one element out of the unsorted list at a	|
|	time and placing it in the correct spot of the		|
|	sorted list.										|
| Parameters: pHead is a pointer to the head of the		|
|	list. This will be changed to point to the new 		|
|	sorted list.										|
| Returns: None.										|
\*******************************************************/
void InsertionSortList (SData* &pHead)
{
	//The start of the unsorted list
	//since the list is completely unsorted, this starts as null
	SData* pSortedHead = 0;
	//The end of the sorted list
	//nothing in the list yet
	SData* pSortedTail = 0;
	//The start of the unsorted list
	//since the list is completely unsorted, this starts as pHead
	SData* pUnsortedHead = pHead;
	//a "loop counter" of sorts
	SData* pCurrent = 0;
	//the element thats currently being looked at
	SData* pTemp = pUnsortedHead;

	if (!pHead)
		//nothing to sort
		return;

	//to get the sorted list started, pull the first element out of the 
	//unsorted list and put it in the sorted list
	pSortedHead = pUnsortedHead;
//	pSortedTail = pSortedHead;
	pUnsortedHead = pUnsortedHead->m_pNext;
	pSortedHead->m_pNext = 0;

	//-while there are still elements left in the unsorted part
	while (pUnsortedHead)
	{
		//we are looking at the start of the unsorted list
		pTemp = pUnsortedHead;
//		pTemp->m_pNext = 0;

		//find the end of the sorted list
		pSortedTail = pSortedHead;
		while (pSortedTail->m_pNext)
			pSortedTail = pSortedTail->m_pNext;

		//-compare the end of the sorted list with the start of the unsorted list
		if (pSortedTail->m_iWordCount < pUnsortedHead->m_iWordCount)
		{
			//-if < then just move the element to the end of the sorted list
			pSortedTail->m_pNext = pUnsortedHead;
			pUnsortedHead = pUnsortedHead->m_pNext;
			pSortedTail->m_pNext->m_pNext = 0;
		}
		else
		{
			//-otherwise, place the element at the start of the unsorted list into where
			//--it belongs in the sorted list (be sure to removed it from the unsorted!)
			pTemp = pUnsortedHead;
			pUnsortedHead = pUnsortedHead->m_pNext;
			pTemp->m_pNext = 0;
			//move one further into the unsorted list

			//now that we have pulled pTemp out of the unsorted list,
			//find where it belongs in the sorted list

			//does it belong at the start of the unsorted list?
			if (pTemp->m_iWordCount <= pSortedHead->m_iWordCount)
			{
				//if yes, put it there
				pTemp->m_pNext = pSortedHead;
				pSortedHead = pTemp;
				continue;
			}

			//at this point, the data may belong in the middle or the end
			//first, look at the end since we already know where that is
			if (pTemp->m_iWordCount >= pSortedTail->m_iWordCount)
			{
				//belongs at the end, so put it there
				pSortedTail->m_pNext = pTemp;
				continue;
			}

			//now the element can only belong in the middle of the list

			//go through the list to try and find the right spot
			pCurrent = pSortedHead;
			while (/*pCurrent->m_pNext &&*/ pCurrent->m_pNext->m_iWordCount < pTemp->m_iWordCount)
			{
				pCurrent = pCurrent->m_pNext;
			}//end while

			//the correct position has been found, put the element there
			pTemp->m_pNext = pCurrent->m_pNext;
			pCurrent->m_pNext = pTemp;
		}//end if
	}//end while

	//"return" the sorted list to main
	pHead = pSortedHead;
}//end function

/*******************************************************\
| Function: void DeleteList (SData* &pHead)				|
| Description: Frees all memory allocated to a linked	|
|	list.												|
| Parameters: pHead is a pointer to the head of the		|
|	list. This will be changed to point to null at the	|
|	end of the function.								|
| Returns: None.										|
\*******************************************************/
void DeleteList (SData* &pHead)
{
	//the element to be deleted
	SData* pDelete = 0;
	//the element after the one to be deleted
	//a "head" of sorts
	SData* pCurrent = 0;

	//start at the head
	pCurrent = pHead;
	//go through each one
	while (pCurrent)
	{
		//and nuke it!
		//keep track of the node to be nuked
		pDelete = pCurrent;
		//move the loop counter to the next node
		pCurrent = pCurrent->m_pNext;
		//nuke!
		delete pDelete;
	}
	//the list is now empty
	pHead = 0;
}

int main (int argc, char** argv)
{
	//The start of the linked list
	SData* pHead = 0;

	//turn on leak checking
	_CrtSetDbgFlag (_CRTDBG_ALLOC_MEM_DF | 
					_CRTDBG_LEAK_CHECK_DF |
					_CRTDBG_CHECK_ALWAYS_DF);

	//this program requires only one argument, the path and name of the file
	//to be analysed. If we get anything else, error out.
	if (2 != argc)
	{
		cout << "Invalid command line usage.\n";
		cout << "Please correct it to the following: \n";
		cout << argv [0] << " <filename>\n";
		system ("pause");
		return 1;
	}

	//Attempt to load the file into the list...
	if (!LoadFile (pHead, argv[1]))
	{
		//...if it didn't work, inform the user and exit.
		cout << "Could not open the file: \n"
			 << argv[1] << endl
			 << "Please try again with a different file.\n";
		return 1;
	}

	//file is now loaded and the data is stored in the list
	//so display some statistics....
	cout << "Word Length Statistics - Sorted by size\n"
		 << "---------------------------------------\n";
	DisplayList (pHead);
	cout << endl;

	//...then sort it...
	InsertionSortList (pHead);

	//...and display the same stats in a different order
	cout << "Word Length Statistics - Sorted by frequency\n"
		 << "--------------------------------------------\n";
	DisplayList (pHead);

	//kill the list, and exit normally.
	DeleteList (pHead);
	cout << endl;
	system ("pause");
	return 0;
}