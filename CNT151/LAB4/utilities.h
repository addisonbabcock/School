/***************************************************\
* Project:		Lab 4 - Random Insulter				*
* Files:		main.cpp, main.h,					*
*				utilities.cpp, utilities.h			*
* Date:			03 Apr 2006							*
* Author:		Addison Babcock		Class:  CNT15	*
* Instructor:	J. D. Silver		Course: CNT151	*
* Description:	A program that will randomly insult	*
*	a random person									*
\***************************************************/

#ifndef UTILITIES_H
#define UTILITIES_H

#include <iostream>
#include <iomanip>
#include <ctime>
#include <cmath>

using namespace std;

void DisplayArray (const int iArray [], const int iSize, const char cDelimiter);
int GetRandInt (int iLowerBound, int iUpperBound);
void FlushCINBuffer ();

#endif // UTILITIES_H