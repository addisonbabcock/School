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

#include "utilities.h"

/*******************************************************\
| Function: void DisplayArray (const int iArray [],		|
|							   const int iSize,			|
|							   const char cDelimitier)	|
| Description: Outputs an array from 0 to iSize			|
|	seperated by cDelimiter								|
| Parameters: The array to be output, the size of the	|
|	array and the seperating character					|
| Returns: None.										|
\*******************************************************/
void DisplayArray (const int iArray [], const int iSize, const char cDelimiter)
{
	//for each element in the array
	for (int i = 0; i < iSize; i++)
	{
		//output it and the delimiter
		cout << iArray [i] << cDelimiter;
	}
	cout << endl;
}

/***********************************************\
| Function: void SeedRandomGenerator (void)		|
| Description: Does nothing more than call		|
|	srand ().									|
| Parameters: None.								|
| Returns: None.								|
\***********************************************/
void SeedRandomGenerator (void)
{
	srand (static_cast <unsigned int> (time (static_cast<time_t> (0))));
}

/***********************************************\
| Function: int GetRandInt (int iLowerBound,	|
|							int iUpperBound)	|
| Description: Gets a random number from		|
|	iLowerBound to iUpperBound. If iLowerBound	|
|	> iUpperBound, then they are swapped. 		|
|	Assumes that rand() is already seeded		|
| Parameters: iLowerBound is the lowest number	|
|	that can be returned.						|
|	iUpperBound is the biggest number that can  |
|	be returned.								|
| Returns: The random number that was generated	|
\***********************************************/
int GetRandInt (int iLowerBound, int iUpperBound)
{
	//If the boundaries are mixed up, switch them
	//to prevent an infinite loop
	if (iLowerBound > iUpperBound)
	{
		int iTemp = iLowerBound;
		iLowerBound = iUpperBound;
		iUpperBound = iTemp;
	}

	//Get a random number between iLower and iUpper, inclusively
	return (rand () % (iUpperBound - iLowerBound + 1)) + iLowerBound;
}

/***********************************************\
| Function: void SeedRandomGenerator (void)		|
| Description: Does nothing more than empty		|
|	the cin buffer.								|
| Parameters: None.								|
| Returns: None.								|
\***********************************************/
void FlushCINBuffer (void)
{
	cin.clear ();
	cin.ignore (cin.rdbuf ()->in_avail (), '\n');
}