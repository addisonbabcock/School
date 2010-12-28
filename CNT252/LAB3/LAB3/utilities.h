/***************************************************\
* Project:		Lab 3 - BlackJack					*
* Files:		main.cpp, main.h, utilities.cpp		*
*				utilities.h							*
* Date:			10 Oct 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	A game of human vs computer 		*
*				Blackjack.							*
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
double GetDouble (char const * const szPrompt, double dLower, double dUpper);

#endif // UTILITIES_H