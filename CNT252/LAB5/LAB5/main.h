/***************************************************\
* Project:		Lab 5 - Searching/Sorting			*
* Files:		main.cpp, main.h, utilities.cpp		*
*				utilities.h, sorting.h, sorting.cpp	*
* Date:			21 Nov 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	A student database program.			*
\***************************************************/

#ifndef MAIN_H
#define MAIN_H

const unsigned int gkuiNameLen = 30;

struct Student
{
	char	m_szName [gkuiNameLen];
	int		m_iSNumber;
	float	m_fGrade;
	char	m_cLetter;
};

char DisplayMenu (bool bFileLoaded);
void DisplayStudents (Student* psData, unsigned int uiStudentCount, 
					  bool bDisplayIndex = false);
void InputStudent (Student* psData);
unsigned int LoadFile (Student** ppsData);
void WriteFile (Student* psData, unsigned int uiStudentCount);
void AddToArray (Student** ppsData, unsigned int* puiMaxSize);
void DeleteFromArray (Student** ppsData, unsigned int* uiSize);

#endif //MAIN_H