/***************************************************\
* Project:		Lab 2 - 8-bit Binary Calculator		*
* Files:		main.cpp, utilities.h, utilities.cpp*
* Date:			22 Sept 2006						*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	An 8 bit binary calculator that 	*
*	will perform most binary operations.			*
\***************************************************/

#include "utilities.h"

/*******************************************************\
| Function: void DisplayArray (const int iArray [],		|
|							   const int iSize,			|
|							   const char cDelimitier)	|
| Description: Outputs an array from 0 to iSize			|
|	seperated by cDelimiter. Mostly a debug function.	|
| Parameters: The array to be output, the size of the	|
|	array and the seperating character					|
| Returns: None.										|
\*******************************************************/
void DisplayArray (const unsigned int iArray [], 
				   const int iSize, 
				   const char cDelimiter)
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
| Description: Calls srand () once only.		|
| Parameters: None.								|
| Returns: None.								|
\***********************************************/
void SeedRandomGenerator (void)
{
	//Is srand () already called?
	static bool bIsSeeded = false;

	//only seed the generator once
	if (!bIsSeeded)
	{
		//Seed the random generator based on the current time
		srand (static_cast <unsigned int> (time (static_cast<time_t> (0))));
		bIsSeeded = true;
	}
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
| Function: void FlushCINBuffer (void)			|
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

/*******************************************************\
| Function: void GetMenuChoice (char const * const		|
|								szValidChoices)			|
| Description: A function to get input and make sure	|
|	the input is a valid menu option. Assumes that the	|
|	menu and a suitable prompt are already displayed	|
| Parameters: A null terminated string containing all	|
|	valid options.										|
| Returns: The users choice.							|
\*******************************************************/
char GetMenuChoice (char const * const szValidChoices)
{
	//The users input
	char cRetVal = 0;
	//The users input in string format (temporary variable)
	char szInput [2] = " ";

	//NOTE: this will not loop infinitely
	while (true)
	{
		//Get the menu option
		FlushCINBuffer ();
		cin >> cRetVal;

		//turning cRetVal into a string so strstr () will accept it
		szInput [0] = cRetVal;
		szInput [1] = 0;

		//If the input is a valid option
		if (strstr (szValidChoices, szInput))
		{
			//Return the input
			return cRetVal;
		}
		else
		{
			//display an error message and loop back to the top
			cout << "Invalid selection, please try again: ";
		}
	}

	//This will never execute, it's just to keep
	//compiler warnings to a minimum
	return 0;
}

/***********************************************************\
| Function: int GetInt (int iLowerBound, int iUpperBound)	|
| Description: Gets an integer value from the keyboard		|
|	between iLowerBound and iUpperBound, exclusively. This	|
|	is mostly a debug function in this program.				|
| Parameters: Two int's are required for boundaries.		|
| Returns: The int that was entered.						|
\***********************************************************/
int GetInt (int iLowerBound, int iUpperBound)
{
	//the return value
	int iRetVal;

	//if the boundaries are mixed up, swap them
	if (iLowerBound > iUpperBound)
	{
		int iTemp = iUpperBound;
		iUpperBound = iLowerBound;
		iLowerBound = iTemp;
	}

	//try to get a int
	FlushCINBuffer ();
	cin >> iRetVal;
	
	//while the int does not meet requirements
	while (cin.fail () || iRetVal > iUpperBound || iRetVal < iLowerBound)
	{
		//NAG!
		cout << "Error, must be a value between " 
			 << iLowerBound << " and " << iUpperBound << ": ";

		FlushCINBuffer ();
		cin >> iRetVal;
	}

	return iRetVal;
}