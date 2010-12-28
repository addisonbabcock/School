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

#include "utilities.h"

/***********************************************\
| Function: void SeedRandomGenerator (void)		|
| Description: Does nothing more than call		|
|	srand ().									|
| Parameters: None.								|
| Returns: None.								|
\***********************************************/
void SeedRandomGenerator (void)
{
	//Seed the random generator based on the current time
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
	//Clears out the entire cin buffer
	cin.ignore (cin.rdbuf ()->in_avail (), '\n');
}
