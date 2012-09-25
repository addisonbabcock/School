/***************************************************\
* Project:		Lab 5 - Searching/Sorting			*
* Files:		main.cpp, main.h, utilities.cpp		*
*				utilities.h, sorting.h, sorting.cpp	*
* Date:			21 Nov 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	A student database program.			*
\***************************************************/

#ifndef UTILITIES_H
#define UTILITIES_H

#include <iostream>
#include <iomanip>
#include <ctime>
#include <cmath>
#include <float.h>

using namespace std;

typedef unsigned char byte;

int round (const double a);
void DisplayArray (const unsigned int iArray [], 
				   const int iSize, 
				   const char cDelimiter);
void SeedRandomGenerator (void);
int GetRandInt (int iLowerBound, int iUpperBound);
void FlushCINBuffer ();
char GetMenuChoice (char const * const szValidChoices);
void GetInput (int* const piValue, char const * const szPrompt,
			   int const iLow = INT_MIN, int const iHigh = INT_MAX);
void GetInput (double* const pdValue, char const * const szPrompt,
			   double const dLow = -DBL_MAX, double const dHigh = DBL_MAX);
void GetInput (float* const pfValue, char const * const szPrompt,
			   float const fLow = -FLT_MAX, float const fHigh = FLT_MAX);

#endif // UTILITIES_H