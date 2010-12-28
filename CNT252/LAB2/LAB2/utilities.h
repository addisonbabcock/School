/***************************************************\
* Project:		Lab 2 - 8-bit Binary Calculator		*
* Files:		main.cpp, utilities.h, utilities.cpp*
* Date:			22 Sept 2006						*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	An 8 bit binary calculator that 	*
*	will perform most binary operations.			*
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