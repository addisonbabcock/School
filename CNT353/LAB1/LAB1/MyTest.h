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
#include <iostream>
#include "optimize.h"
#include "CDrawInterface.h"

using namespace std;

/*******************************************************\
| Function: void Lab1Test ()							|
| Description: Tests the functions contained in lab 1.	|
| Parameters: void										|
| Returns: void											|
\*******************************************************/
void Lab1Test ()
{
	// give the library time to initialize
	gfx.SetBackroundColor (100,100,100);
	gfx.SetColor (255, 255, 255);
	gfx.SetSpace (0, 0);

	// Sample test code
	char * pcMem (0); // pointer to memory block
	int iSize (gkuiFileSize), iOldSize (0);

	// attempt to load the file
	pcMem = Load (gkszFileName);
	if (!pcMem)
	{
		return;
	}

	// show the unoptimized memory
	Show (pcMem, iSize);
	iSize  = Optimize (pcMem, iSize);
	Show (pcMem, iSize);
	do {
		iOldSize = iSize;
		iSize = Optimize (pcMem, iSize);
		Show (pcMem, iSize);
	} while (iSize != iOldSize);
	Free (pcMem, iSize);
}