/***************************************************\
* Project:		Lab 5 - Searching/Sorting			*
* Files:		main.cpp, main.h, utilities.cpp		*
*				utilities.h, sorting.h, sorting.cpp	*
* Date:			21 Nov 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	A student database program.			*
\***************************************************/

#include <iostream>
#include <fstream>
#include <crtdbg.h>
#include "utilities.h"
#include "main.h"
#include "sorting.h"

using namespace std;

/***************************************************************************\
| Function: char DisplayMenu (bool bFileLoaded)								|
| Description: Displays a menu and returns the users selection.				|
| Parameters: bFileLoaded is wether or not a complete menu should be		|
|	displayed.																|
| Returns: The upper case selection that was entered by the user. This is	|
|	already checked to be a valid option.									|
\***************************************************************************/
char DisplayMenu (bool bFileLoaded)
{
	//A list of valid menu options
	char szValidOptions [21];

	system ("cls");
	//if a full menu should be displayed
	if (bFileLoaded)
	{
		//barf!
		cout << "l. load file.\n"
			 << "b. bubble sort by name.\n"
			 << "i. insertion sort by student number.\n"
			 << "s. selection sort by grade.\n"
			 << "q. quick sort by grade letter.\n"
			 << "f. find student by name.\n"
			 << "a. add student.\n"
			 << "d. delete student.\n"
			 << "w. write data to a file.\n"
			 << "x. exit.\n";
		//set the options as valid
		strcpy (szValidOptions, "LBISQFADWXlbisqfadwx");
	}
	else
	{
		//small menu
		cout << "l. load file.\n"
			 << "a. add student.\n"
			 << "x. exit.\n";
		//very few options are valid
		strcpy (szValidOptions, "LAXlax");
	}

	//get the users input and return the upper case version of it
	cout << "\nYour selection please: ";
	return toupper (GetMenuChoice (szValidOptions));
}

/***************************************************************************\
| Function: void DisplayStudents (Student* psData,							|
								  unsigned int uiStudentCount,				|
								  bool bDisplayIndex)						|
| Description: Displays the entire collection of students. Pauses after each|
|	full screen. If bDisplayIndex is enabled, it will display the index of	|
|	each student as well as the data.										|
| Parameters: psData is the location of the students on the heap.			|
|	uiStudentCount is how many students are stored on the heap.				|
|	bDisplayIndex is wether or not the index should be shown as well.		|
| Returns: None.															|
\***************************************************************************/
void DisplayStudents (Student* psData, unsigned int uiStudentCount, 
					  bool bDisplayIndex)
{
	system ("cls");

	//go through each student
	for (int i = 0; i < uiStudentCount; i++)
	{
		//for every 23rd student
		if (0 == i % 23)
		{
			//pause then clear the screen if a page full was displayed
			if (i > 0)
			{
				system ("pause");
				system ("cls");
			}

			//display a title
			//if an index will be displayed, move the title slightly to the 
			//right
			if (bDisplayIndex)
				cout << "   ";
			cout << "Name                            "
				 << "ID Number  " << "%      Mark" << endl;
		}

		//show the index if needed
		if (bDisplayIndex)
			cout << setw (3) << setfill (' ') << i;

		//display the data
		cout << setw (32) << setfill ('.') << left << psData [i].m_szName
			 << setw (10) << setfill (' ') << left << psData [i].m_iSNumber 
			 << ' ' << setprecision (3) << setw (4) << setfill (' ') << left 
			 << psData [i].m_fGrade << " = " << left << psData [i].m_cLetter 
			 << endl;
	}
}

/***************************************************************************\
| Function: void InputStudent (Student* psData)								|
| Description: Gets all the details for a student from the user.			|
| Parameters: psData is the location of the student on the heap.			|
| Returns: None.															|
\***************************************************************************/
void InputStudent (Student* psData)
{
	//get the students name
	cout << "Enter the students name: ";
	FlushCINBuffer ();
	cin.getline (psData->m_szName, gkuiNameLen);

	//get the students ID#
	GetInput (&(psData->m_iSNumber), "Enter the students ID number: ", 0);

	//get the students grade
	GetInput (&(psData->m_fGrade), "Enter the students floating point grade: ",
			  0.0f, 100.0f);

	//get the students letter grade
	cout << "Enter the students letter value grade: ";
	psData->m_cLetter = toupper (GetMenuChoice ("ABCDEFabcdef"));
}

/***************************************************************************\
| Function: unsigned int LoadFile (Student** ppsData)						|
| Description: Gets all the details for the students from a file.			|
| Parameters: ppsData is the location of the pointer to the student on the  |
|	heap. This function needs double inderiction because *ppsData will be	|
|	changed as the file is loaded.											|
| Returns: How many students were loaded by the function.					|
\***************************************************************************/
unsigned int LoadFile (Student** ppsData)
{
	//the file to be loaded
	char szFileName [256];
	//the file object
	fstream file;
	//the size of the file in bytes
	unsigned int uiFileSize = 0;
	//how many students are stored in the file
	unsigned int uiStudentCount = 0;

	//if something is already loaded
	if (0 != *ppsData)
	{
		//unload it
		delete [] *ppsData;
		*ppsData = 0;
	}

	//while the file is invalid
	do 
	{
		//get a file name from the user
		cout << "Open which file? ";
		FlushCINBuffer ();
		cin.getline (szFileName, 256);

		//if a blank string was entered, quit this function	
		if (0 == szFileName [0])
			return 0;

		//try to open the file
		file.open (szFileName, ios::binary | ios::in);

		//if it didn't work
		if (file.fail ())
		{
			//inform the user and get another one (next iteration)
			cout << "Could not open that file.\n";
			file.close ();
			file.clear ();
		}
		else
			//file is valid and opened, break out of the loop
			break;
	} while (true);

	//get the size of the file
	file.seekg (0, ios::end);
	uiFileSize = file.tellg ();
	file.seekg (0, ios::beg);

	//calculate how many students are stored in the file
	uiStudentCount = uiFileSize / sizeof (Student);
	//the allocate memory to store all of them
	*ppsData = new Student [uiStudentCount];

	//load the data into memory
	file.read (reinterpret_cast <char*> (*ppsData), uiFileSize);
	file.close ();

	//show how many students were loaded and then return that value
	cout << "Read in " << uiStudentCount << " students.\n";
	return uiStudentCount;
}

/***************************************************************************\
| Function: void WriteFile (Student* psData, unsigned int uiStudentCount)	|
| Description: Saves all the details for the students to a file.			|
| Parameters: psData is the location of the students on the	heap.			|
|	uiStudentCount is how many students should be saved.					|
| Returns: None.															|
\***************************************************************************/
void WriteFile (Student* psData, unsigned int uiStudentCount)
{
	//the files name
	char szFileName [256] = {0};
	//the file object
	fstream file;

	//while the file can not be opened
	do 
	{
		//find out where to save the data
		cout << "Save to which file? ";
		FlushCINBuffer ();
		cin.getline (szFileName, 256);

		//if a blank string was entered, return to the menu
		if (0 == szFileName [0])
			return;

		//attempt to open the file
		file.open (szFileName, ios::binary | ios::out);

		//the file could not be opened, get another one next iteration
		if (file.fail ())
		{
			cout << "Could not open that file.\n";
			file.close ();
			file.clear ();
		}
		else
			//the file was valid and is now opened, break out of the loop
			break;
	} while (true);

	//save the data and close the file
	file.write (reinterpret_cast <char*> (psData), 
				uiStudentCount * sizeof (Student));
	file.close ();

	cout << "File saved.\n";
	return;
}

/***************************************************************************\
| Function: void AddToArray (Student** ppsData, unsigned int* puiMaxSize)	|
| Description: Adds a single student to an array. If the array does not		|
|	exist, it will be created.												|
| Parameters: ppsData is the location of the pointer to the student on the	|
|	heap. This function needs double inderiction because *ppsData will be	|
|	changed as the student is added. puiMaxSize is a pointer to the current	|
|	size of the array.														|
| Returns: None.															|
\***************************************************************************/
void AddToArray (Student** ppsData, unsigned int* puiMaxSize)
{
	//a temporary copy of the array
	Student* psTemp = 0;

	//we will be adding to the array so it will be one size bigger
	*puiMaxSize += 1;
	//allocate memory for the temp copy
	psTemp = new Student [*puiMaxSize];

	//if data is contained in the old array
	if (*ppsData != 0)
	{
		//save it in the new bigger array
		for (int i = 0; i < (*puiMaxSize) - 1; i++)
		{
			psTemp [i] = (*ppsData) [i];
		}

		//and delete the old copy
		delete [] *ppsData;
	}

	//change where the program will look for the data
	*ppsData = psTemp;

	//get the details for the new student
	InputStudent ((*ppsData) + *puiMaxSize - 1);
}

/***************************************************************************\
| Function: void DeleteFromArray (Student** ppsData,						|
|								  unsigned int* puiMaxSize)					|
| Description: Deletes a single student from the array.						|
| Parameters: ppsData is the location of the pointer to the student on the	|
|	heap. This function needs double inderiction because *ppsData will be	|
|	changed as the student is deleted. puiMaxSize is a pointer to the		|
|	current	size of the array.												|
| Returns: None.															|
\***************************************************************************/
void DeleteFromArray (Student** ppsData, unsigned int* puiSize)
{
	//a copy of the data
	Student* psTemp = 0;
	//which student to delete
	int iDelIndex = 0;

	//show all the students with an index
	DisplayStudents (*ppsData, static_cast<unsigned int>(*puiSize), true);
	//find out which index the user wishes to delete
	GetInput (&iDelIndex, "Delete which index? ", 0, *puiSize);

	//CONFIRMATION
	//show that student only
	DisplayStudents (*ppsData + iDelIndex, 1, true);
	//make sure the user really really wants to delete that student
	cout << "Are you sure you want to delete this student? ";
	if ('Y' != toupper (GetMenuChoice ("YyNn")))
	{
		//if not, return to the calling function without changing anything
		cout << "CANCELED!\n";
		return;
	}

	//the array will be 1 size smaller
	*puiSize -= 1;
	//allocate the new array
	psTemp = new Student [*puiSize];

	//copy all the data before the student to be deleted
	for (int i = 0; i < iDelIndex; i++)
		psTemp [i] = (*ppsData) [i];

	//copy all the data after the student to be deleted
	for (int i = iDelIndex; i < *puiSize; i++)
		psTemp [i] = (*ppsData) [i + 1];

	//delete the old array
	delete [] *ppsData;
	//change the location of the array as viewed from the rest of the program
	*ppsData = psTemp;
}

int main ()
{
	//the menu selection
	char cMenuSelection = 0;
	//a pointer to the student data on the heap
	Student* psData = 0;
	//a pointer to the student matching the search criteria
	Student* psFound = 0;
	//how many students are loaded in memory
	unsigned int uiStudentCount = 0;
	//the name of the student to be searched for
	char szSearchFor [gkuiNameLen] = {0};
	
	//turn on memory leak checking
	_CrtSetDbgFlag (_CRTDBG_ALLOC_MEM_DF |
					_CRTDBG_LEAK_CHECK_DF |
					_CRTDBG_CHECK_ALWAYS_DF);

	//while the user does not want to quit
	while ('X' != cMenuSelection)
	{
		//get a new menu choice, display the full menu if the pointer is not null
		//and there is more then one student loaded
		cMenuSelection = DisplayMenu (0 != psData && 0 != uiStudentCount);

		switch (cMenuSelection)
		{
		case 'L':
			//load a file
			uiStudentCount = LoadFile (&psData);
			//show what was loaded
			DisplayStudents (psData, uiStudentCount);
			break;
		case 'B':
			//sort and display the results
			BubbleSortByName (psData, uiStudentCount);
			DisplayStudents (psData, uiStudentCount);
			break;
		case 'I':
			//sort and display the results
			InsertionSortByNumber (psData, uiStudentCount);
			DisplayStudents (psData, uiStudentCount);
			break;
		case 'S':
			//sort and display the results
			SelectionSortByGrade (psData, uiStudentCount);
			DisplayStudents (psData, uiStudentCount);
			break;
		case 'Q':
			//sort and display the results
			qsort (psData, uiStudentCount, sizeof (Student), 
				   CompareLetterGrade);
			DisplayStudents (psData, uiStudentCount);
			break;
		case 'F':
			//sort the data so bsearch () can be used 
			qsort (psData, uiStudentCount, sizeof (Student),
				   CompareName);

			//find out who we are searching for
			cout << "Search for who? ";
			FlushCINBuffer ();
			cin.getline (szSearchFor, gkuiNameLen);

			//have a look for him/her
			psFound = reinterpret_cast <Student*> 
				(bsearch (szSearchFor, psData,
						  uiStudentCount, 
						  sizeof (Student),
						  CompareName));

			//if he/she was found
			if (psFound)
			{
				//display his or her data
				DisplayStudents (psFound, 1);
			}
			else
			{
				//otherwise display an error
				cout << "Student not found!\n";
			}
			break;
		case 'A':
			//add a student to the array
			AddToArray (&psData, &uiStudentCount);
			break;
		case 'D':
			//take a student out of the array
			DeleteFromArray (&psData, &uiStudentCount);
			break;
		case 'W':
			//save the file
			WriteFile (psData, uiStudentCount);
			break;
		}
		system ("pause");
	}

	//if there is data allocated
	if (0 != psData)
	{
		//dealloc it
		delete [] psData;
		psData = 0;
	}

	cout << endl;
	system ("pause");
	return 0;
}