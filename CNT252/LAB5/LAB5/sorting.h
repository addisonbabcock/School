/***************************************************\
* Project:		Lab 5 - Searching/Sorting			*
* Files:		main.cpp, main.h, utilities.cpp		*
*				utilities.h, sorting.h, sorting.cpp	*
* Date:			21 Nov 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	A student database program.			*
\***************************************************/

#ifndef SORTING_H
#define SORTING_H

#include "main.h"

void BubbleSortByName (Student* psData, unsigned int uiStudentCount);
void InsertionSortByNumber (Student* psData, unsigned int uiStudentCount);
void SelectionSortByGrade (Student* psData, unsigned int uiStudentCount);
int CompareLetterGrade (const void* pLeft, const void* pRight);
int CompareName (const void* pLeft, const void* pRight);

#endif //SORTING_H