/***************************************************\
* Project:		Lab 5 - Searching/Sorting			*
* Files:		main.cpp, main.h, utilities.cpp		*
*				utilities.h, sorting.h, sorting.cpp	*
* Date:			21 Nov 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	A student database program.			*
\***************************************************/

#include <string.h>
#include "sorting.h"
#include "main.h"

/***************************************************************************\
| Function: void BubbleSortByName (Student* psData,							|
								   unsigned int uiStudentCount)				|
| Description: Uses the bubble sort algorithm to sort the student data by	|
|	name.																	|
| Parameters: psData is the location of the array on the heap.				|
|	uiStudentCount is how many students are stored in the array.			|
| Returns: None.															|
\***************************************************************************/
void BubbleSortByName (Student* psData, unsigned int uiStudentCount)
{
	//a temporary storage for swapping
	Student sTemp;

	//make several passes through the array
	for (unsigned int i = 0; i < uiStudentCount - 1; i++)
	{
		//make a pass through the array
		for (unsigned int j = 0; j < uiStudentCount - 1; j++)
		{
			//if the values are out of order...
			if (strcmpi (psData [j].m_szName, psData [j + 1].m_szName) > 0)
			{
				//...swap them
				sTemp = psData [j];
				psData [j] = psData [j + 1];
				psData [j + 1] = sTemp;
			}
		}
	}
}

/***************************************************************************\
| Function: void InsertionSortByNumber (Student* psData,					|
|										unsigned int uiStudentCount)		|
| Description: Uses the insertion sort algorithm to sort the student data	|
|	by student ID number.													|
| Parameters: psData is the location of the array on the heap.				|
|	uiStudentCount is how many students are stored in the array.			|
| Returns: None.															|
\***************************************************************************/
void InsertionSortByNumber (Student* psData, unsigned int uiStudentCount)
{
	//a temporary storage location
	Student sTemp;
	//counter variables
	int i = 0;
	int j = 0;

	for (i = 1; i < uiStudentCount; i++)
	{
		j = i - 1;
		sTemp = psData [i];

		while (j >= 0 && psData [j].m_iSNumber - sTemp.m_iSNumber > 0)
		{
			psData [j + 1] = psData [j];
			j--;
		}
		psData [j + 1] = sTemp;
	}
}

/***************************************************************************\
| Function: void SelectionSortByGrade (Student* psData,						|
|									   unsigned int uiStudentCount)			|
| Description: Uses the selection sort algorithm to sort the student data	|
|	by the students floating point grade.									|
| Parameters: psData is the location of the array on the heap.				|
|	uiStudentCount is how many students are stored in the array.			|
| Returns: None.															|
\***************************************************************************/
void SelectionSortByGrade (Student* psData, unsigned int uiStudentCount)
{
	//the "smallest" student
	//a temporary storage
	Student sMin, sTemp;
	//where the "smallest" student is located
	int iMinIndex = 0;
	//pass counter, where the sorted part of the array ends
	int iCur = 0;
	//counter to find the "smallest" student
	int iScan = 0;

	//go through the array several times
	for (iCur = 0; iCur < uiStudentCount - 1; iCur++)
	{
		//the smallest is set to the first value in the unsorted array
		sMin = psData [iCur];
		iMinIndex = iCur;

		//go through the unsorted array and look for the smallest value
		for (iScan = iCur + 1; iScan < uiStudentCount; iScan++)
		{
			//if a smaller value is found then the current one...
			if (psData [iScan].m_fGrade > sMin.m_fGrade)
			{
				//...set it as the smallest
				sMin = psData [iScan];
				iMinIndex = iScan;
			}
		}

		//swap the first unsorted value with the smallest value found
		sTemp = psData [iMinIndex];
		psData [iMinIndex] = psData [iCur];
		psData [iCur] = sTemp;
	}
}

/***************************************************************************\
| Function: int CompareLetterGrade (const void* pLeft, const void* pRight)	|
| Description: Compares 2 students based on their letter grade.	Made for use|
|	use with qsort () and bsearch ().										|
| Parameters: pLeft where the 1st student is stored. pRight is where the	|
|	second student is stored.												|
| Returns: Positive value if pLeft is greater then pRight. 0 if pLeft is the|
|	same as pRight. Negative value if pRight is greater then pLeft.			|
\***************************************************************************/
int CompareLetterGrade (const void* pLeft, const void* pRight)
{
	return static_cast<int>
						(reinterpret_cast<const Student*> (pLeft)->m_cLetter -
						reinterpret_cast<const Student*> (pRight)->m_cLetter);
}

/***************************************************************************\
| Function: int CompareName (const void* pLeft, const void* pRight)			|
| Description: Compares 2 students based on their name.	Made for use with	|
|	qsort () and bsearch ().												|
| Parameters: pLeft where the 1st student is stored. pRight is where the	|
|	second student is stored.												|
| Returns: Positive value if pLeft is greater then pRight. 0 if pLeft is the|
|	same as pRight. Negative value if pRight is greater then pLeft.			|
\***************************************************************************/
int CompareName (const void* pLeft, const void* pRight)
{
	return strcmpi (reinterpret_cast <const Student*> (pLeft)->m_szName,
					reinterpret_cast <const Student*> (pRight)->m_szName);
}