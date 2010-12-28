/***************************************************\
* Project:		Lab 4 - Drawing Thinger				*
* Files:		main.cpp, main.h, utilities.cpp		*
*				utilities.h, CDRrawInterface.h,		*
*				graphics.h, graphics.cpp			*
* Date:			02 Nov 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	A primitive drawing program.		*
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
byte GetInput (char const * const szPrompt);
void GetInput (int* const piValue, char const * const szPrompt,
			   int const iLow = INT_MIN, int const iHigh = INT_MAX);
void GetInput (double* const pdValue, char const * const szPrompt,
			   double const dLow = -DBL_MAX, double const dHigh = DBL_MAX);

#endif // UTILITIES_H