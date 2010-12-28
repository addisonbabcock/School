/***************************************************\
* Project:		Lab 3 - Mastermind					*
* Files:		main.cpp, main.h,					*
*				utilities.cpp, utilities.h			*
* Date:			21 Mar 2006							*
* Author:		Addison Babcock		Class:  CNT15	*
* Instructor:	J. D. Silver		Course: CNT151	*
* Description:	A simple text-based game of			*
*				mastermind							*
\***************************************************/

#ifndef UTILITIES_H
#define UTILITIES_H

#include <iostream>
#include <iomanip>
#include <ctime>
#include <cmath>

using namespace std;

void SeedRandomGenerator (void);
void DisplayArray (const int iArray [], const int iSize, const char cDelimiter);
int GetRandInt (int iLowerBound, int iUpperBound);
void FlushCINBuffer ();

#endif // UTILITIES_H