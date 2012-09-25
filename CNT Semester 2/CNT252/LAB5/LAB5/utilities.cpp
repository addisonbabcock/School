/***************************************************\
* Project:		Lab 5 - Searching/Sorting			*
* Files:		main.cpp, main.h, utilities.cpp		*
*				utilities.h, sorting.h, sorting.cpp	*
* Date:			21 Nov 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	A student database program.			*
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

/***************************************************************************\
| Function: void GetInput (int* const piValue, char const * const szPrompt,	|
						   int const iLow, int const iHigh)					|
| Description: Prompts the user for an integer value between iLow and iHigh	|
|	EXCLUSIVELY.															|
| Parameters: piValue is where to store the input. szPrompt is what to		|
|	display on the screen. iLow is the lowest possible value that can be	|
|	entered. iHigh is the highest possible value that can be entered.		|
| Returns: None.															|
\***************************************************************************/
void GetInput (int* const piValue, char const * const szPrompt,
			   int const iLow, int const iHigh)
{
	//get a value from the user
	cout << szPrompt;
	FlushCINBuffer ();
	cin >> *piValue;

	//check it for validity
	while (*piValue > iHigh || *piValue < iLow || cin.fail ())
	{
		//keep getting new values until the user enters a valid one
		cout << szPrompt;
		FlushCINBuffer ();
		cin >> *piValue;
	}
}

/***************************************************************************\
| Function: void GetInput (double* const piValue,							|
						   char const * const szPrompt,						|
						   double const iLow, double const iHigh)			|
| Description: Prompts the user for a double value between dLow and dHigh	|
|	EXCLUSIVELY.															|
| Parameters: pdValue is where to store the input. szPrompt is what to		|
|	display on the screen. dLow is the lowest possible value that can be	|
|	entered. dHigh is the highest possible value that can be entered.		|
| Returns: None.															|
\***************************************************************************/
void GetInput (double* const pdValue, char const * const szPrompt,
			   double const dLow, double const dHigh)
{
	//get a value from the user
	cout << szPrompt;
	FlushCINBuffer ();
	cin >> *pdValue;

	//check it for validity
	while (*pdValue > dHigh || *pdValue < dLow || cin.fail ())
	{
		//keep getting new values until the user enters a valid one
		cout << szPrompt;
		FlushCINBuffer ();
		cin >> *pdValue;
	}
}


/****************************************************************************\
| Function: void GetInput (float* const pfValue, char const * const szPrompt,|
						   float const fLow, float const fHigh)				 |
| Description: Prompts the user for a float value between fLow and fHigh	 |
|	EXCLUSIVELY.															 |
| Parameters: pfValue is where to store the input. szPrompt is what to		 |
|	display on the screen. fLow is the lowest possible value that can be	 |
|	entered. fHigh is the highest possible value that can be entered.		 |
| Returns: None.															 |
\****************************************************************************/
void GetInput (float* const pfValue, char const * const szPrompt,
			   float const fLow, float const fHigh)
{
	//get a value from the user
	cout << szPrompt;
	FlushCINBuffer ();
	cin >> *pfValue;

	//check it for validity
	while (*pfValue > fHigh || *pfValue < fLow || cin.fail ())
	{
		//keep getting new values until the user enters a valid one
		cout << szPrompt;
		FlushCINBuffer ();
		cin >> *pfValue;
	}
}