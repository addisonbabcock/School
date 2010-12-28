/***************************************************\
* Project:		Lab 5 - Yahtzee						*
* Files:		main.cpp, main.h,					*
*				utilities.cpp, utilities.h			*
*				io.h, io.cpp, logic.h, logic.cpp	*
* Date:			12 Apr 2006							*
* Author:		Addison Babcock		Class:  CNT15	*
* Instructor:	J. D. Silver		Course: CNT151	*
* Description:	A text-based game of Yahtzee		*
\***************************************************/

#ifndef UTILITIES_H
#define UTILITIES_H

#include <iostream>
#include <iomanip>
#include <ctime>
#include <cmath>

using namespace std;

void DisplayArray (const unsigned int iArray [], 
				   const int iSize, 
				   const char cDelimiter);
void SeedRandomGenerator (void);
int GetRandInt (int iLowerBound, int iUpperBound);
void FlushCINBuffer ();
char GetMenuChoice (char const * const szValidChoices);
int GetInt (int iLowerBound, int iUpperBound);

#endif // UTILITIES_H