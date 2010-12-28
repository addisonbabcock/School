/***************************************************\
* Project:		Lab 1 - Opt-o-tron					*
* Date:			January 18, 2007					*
* Author:		Addison Babcock						*
* Files:		MyTest.h, optimize.h, main.cpp,		*
*				optimize.cpp						*
* Class:		CNT3K	 Course:		CNT353		*
* Instructor:	H. Vanselow							*
\***************************************************/
#pragma once
#include "CDrawInterface.h"

char * Load (char const * const szFileName);
void Free (char * &pcData, int &iLength);
int Optimize (char * &pcData, int iLength);
void Show (char const * const pcData, int iLength);
void Lab1Test ();

unsigned int const gkuiFileSize (2500); // how long each file is
unsigned int const gkuiDisplayWidth (50); // how wide the display is
unsigned int const gkuiDisplayHeight (50); // how tall the display is
char const gkcNewline ('\n'); // a new line
char const gkcTab ('\t'); // a tab
char const gkszFileName[] = 
	"..\\testfile.txt";
	// the file to load
char const gkszFileError[] = "Could not load the file!"; 
	// the error message to be displayed when the file cant be loaded

extern CDrawInterface gfx; // the interface to the fancy graphics