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

#include "utilities.h"

int round (const double a)
{
	return int ((a > 0) ? (a + 0.5) : (a - 0.5));
}

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

byte GetInput (char const * const szPrompt)
{
	int iRes = 0;
	GetInput (&iRes, szPrompt, 0, 255);
	return static_cast <byte> (iRes);
}

void GetInput (int* const piValue, char const * const szPrompt,
			   int const iLow, int const iHigh)
{
	cout << szPrompt;
	FlushCINBuffer ();
	cin >> *piValue;

	while (*piValue > iHigh || *piValue < iLow || cin.fail ())
	{
		cout << szPrompt;
		FlushCINBuffer ();
		cin >> *piValue;
	}
}

void GetInput (double* const pdValue, char const * const szPrompt,
			   double const dLow, double const dHigh)
{
	cout << szPrompt;
	FlushCINBuffer ();
	cin >> *pdValue;

	while (*pdValue > dHigh || *pdValue < dLow || cin.fail ())
	{
		cout << szPrompt;
		FlushCINBuffer ();
		cin >> *pdValue;
	}
}