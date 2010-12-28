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

#ifndef MAIN_H
#define MAIN_H

#include <iostream>
#include <string>
#include "utilities.h"

using namespace std;

//How many digits is the secret number?
unsigned int const kuiSecretSize = 4;

//The range of the secret number
unsigned int const kuiGuessRangeMin = 1;
unsigned int const kuiGuessRangeMax = 4;

//The maximum length of strings
unsigned int const kuiMaxStrLen = 30;

void DisplayInstructions (void);
bool PlayAgain (void);
void SetSecret  (int iSecret [kuiSecretSize]);
void GetSecret  (int iGuess  [kuiSecretSize]);
void CheckGuess (const int iGuess  [kuiSecretSize], 
				 const int iSecret [kuiSecretSize], 
				 int* iCorrect, int* iWrongSpot);

#endif //MAIN_H