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

#ifndef MAIN_H
#define MAIN_H

//Include the utilities
#include "utilities.h"
//Needed for screen output
#include <iostream>
//Needed for the intro screen
#include <iomanip>
//Needed for strcat ()
#include <string>
//Needed for file i/o
#include <fstream>

//The maximum size a string should be
const int gkiMaxStrLen = 100;
//The maximum number of strings to load
const int gkiMaxItems = 100;
//The maximum size each name/verb/noun should be
const int gkiMaxItemLen = 50;

int LoadItems (char pItems [gkiMaxItems][gkiMaxItemLen], char pcFileName [gkiMaxStrLen]);

#endif //MAIN_H