/***************************************************\
* Project:		Lab 6 - Linked Lists				*
* Files:		main.cpp, main.h					*
* Date:			01 Dec 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	Displays word count statistics based*
*				on a text file.						*
\***************************************************/
#ifndef MAIN_H
#define MAIN_H

#include <iostream>
#include <iomanip>
#include <fstream>
#include <crtdbg.h>
#include "utilities.h"

using namespace std;

struct SData
{
	int m_iWordSize;
	int m_iWordCount;
	SData* m_pNext;
};

bool LoadFile (SData* &pHead, char* szFileName);
void AddNode (SData* &pHead, int iData);
void DisplayList (SData* pHead);
void InsertionSortList (SData* &pHead);
void DeleteList (SData* &pHead);

#endif //MAIN_H