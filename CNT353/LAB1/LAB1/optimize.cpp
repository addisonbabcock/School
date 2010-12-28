/***************************************************\
* Project:		Lab 1 - Opt-o-tron					*
* Date:			January 18, 2007					*
* Author:		Addison Babcock						*
* Files:		MyTest.h, optimize.h, main.cpp,		*
*				optimize.cpp						*
* Class:		CNT3K	 Course:		CNT353		*
* Instructor:	H. Vanselow							*
\***************************************************/
#include <iostream>
#include <fstream>
#include <string.h>
#include "CDrawInterface.h"
#include "optimize.h"

using namespace std;

CDrawInterface gfx;

/*******************************************************\
| Function: char * Load (char const * const szFileName)	|
| Description: Loads gkuiFileSize bytes from a file and	|
|	stores them in memory. The memory is dynamically	|
|	allocated.											|
| Parameters: A null terminated string, the full path	|
|	to the file to be loaded.							|
| Returns: A pointer to the memory containing the data.	|
\*******************************************************/
char * Load (char const * const szFileName)
{
	fstream file; // the input file
	char * pcData (0); // our chunk of memory
	int iCount (0); // a loop counter

	// try to allocate enough memory, if its not possible
	// return an error
	pcData = new (nothrow) char [gkuiFileSize];
	if (!pcData)
		return 0;

	// attempt to open the file
	file.open (szFileName, ios::in);
	if (!file.is_open ())
	{
		cout << gkszFileError;
		delete [] pcData;
		pcData = 0;
		return 0;
	}

	// read in the file one byte at a time, discarding carriage returns
	// and tabs
	iCount = 0;
	while (iCount < gkuiFileSize)
	{
		file.get (pcData [iCount]);
		if (pcData [iCount] == gkcNewline || pcData [iCount] == gkcTab)
		{
			continue;
		}
		iCount++;
	}

	return pcData;
}

/*******************************************************\
| Function: void Free (char * &pcData, int &iLength)	|
| Description: Releases the memory given by pcData and	|
|	sets iLength to 0.									|
| Parameters: A pointer to the memory to be deleted and	|
|	the length of that memory block.					|
| Returns: void											|
\*******************************************************/
void Free (char * &pcData, int &iLength)
{
	//release the old data
	delete [] pcData;
	pcData = 0;
	iLength = 0;
}

/*******************************************************\
| Function: int Optimize (char * &pcData, int iLength)	|
| Description: Attempts to optimize a memory map.		|
| Parameters: The location of the "memory map" to be	|
|	optimized and its length.							|
| Returns: The new length of the "memory map".			|
\*******************************************************/
int Optimize (char * &pcData, int iLength)
{
	
	int iStartOfBlock (0), //the start of the memory to be moved
						   //after the memory has been moved, this is 
						   //the new length of the array
		iEndOfBlock (0), //the end of the memory to be moved
		iStartOfEmpty (0),  //the start of where the memory will be moved to
		iEndOfEmpty (0); //the end of where the memory will be moved to

	// finding the end of the block that will be moved
	for (iEndOfBlock = iLength - 1; iEndOfBlock >= 0; --iEndOfBlock)
	{
		if (!isspace (pcData [iEndOfBlock]))
		{
			break;
		}
	}

	// finding the start of the block that will be moved
	for (iStartOfBlock = iEndOfBlock - 1; iStartOfBlock > 0; --iStartOfBlock)
	{
		if (isspace (pcData [iStartOfBlock]))
		{
			break;
		}
	}

	// is there only one block that starts at the beginning?
	if (iStartOfBlock == 0)
	{
		return iLength;
	}

	//find an empty space big enough
	while (iStartOfEmpty < iStartOfBlock)
	{
		// finding the start of an empty space
		for (; iStartOfEmpty < iStartOfBlock; ++iStartOfEmpty)
		{
			if (isspace (pcData [iStartOfEmpty]))
			{
				break;
			}
		}

		// finding the end of the empty space
		for (iEndOfEmpty = iStartOfEmpty; 
			iEndOfEmpty < iStartOfBlock + 1; ++iEndOfEmpty)
		{
			if (!isspace (pcData [iEndOfEmpty]))
			{
				break;
			}
		}

		//moving the block, only if the empty part is big enough
		if (iEndOfBlock - iStartOfBlock <= iEndOfEmpty - iStartOfEmpty)
		{
			for (int iLoop = iStartOfEmpty; 
				iLoop < iEndOfBlock - iStartOfBlock + iStartOfEmpty; ++iLoop)
			{
				pcData [iLoop] = 
					pcData [iLoop + iStartOfBlock - iStartOfEmpty + 1];
			}

			// the new length of the memory
			int iNewLength (iStartOfBlock + 1);

			//re-allocate the array
			char * pcNewData = new char [iNewLength];

			//copy the old data over to the new array
			for (int iLoop = 0; iLoop < iNewLength; ++iLoop)
				pcNewData [iLoop] = pcData [iLoop];

			//release the old array
			Free (pcData, iLength);
			pcData = pcNewData;

			return iNewLength;
		}

		++iStartOfEmpty;
	}

	return iLength;
}

/***************************************************************\
| Function: void Show (char const * const pcData, int iLength)	|
| Description: Outputs the "memory map" to the display.			|
| Parameters: The location and length of the "memory map".		|
| Returns: void													|
\***************************************************************/
void Show (char const * const pcData, int iLength)
{
	// reset the display
	gfx.Clear ();
	gfx.SetBackroundColor (128, 128, 128);

	// go through all the allocated memory
	for (int iLoop (0); iLoop < iLength; ++iLoop)
	{
		// set a green point or space if the block is free
		if (isspace (pcData [iLoop]))
		{
			gfx.SetColor (0, 255, 0);
		}
		else
		{
			// set a red point or x if the block is taken
			gfx.SetColor (255, 0, 0);
		}
		gfx.SetSpace(iLoop / gkuiDisplayHeight, iLoop % gkuiDisplayWidth);
	}

	// all the unallocated memory is set to blue
	gfx.SetColor (0, 0, 255);
	for (int iLoop (iLength); iLoop < gkuiFileSize; ++iLoop)
	{
		gfx.SetSpace(iLoop / gkuiDisplayHeight, iLoop % gkuiDisplayWidth);
	}
}
