/***************************************************\
* Project:		Lab 2 - Lottery Ticket Simulator	*
* Files:		main.cpp, main.h					*
* Date:			02 Mar 2006							*
* Author:		Addison Babcock		Class:  CNT15	*
* Instructor:	J. D. Silver		Course: CNT151	*
* Description:	Asks the user for 4 unique ints		*
*	then does 1 million simulations					*
\***************************************************/

#include <iostream>
#include <iomanip>
#include <cmath>
#include <ctime>
#include "main.h"

using namespace std;

/***********************************************\
| Function: void Instructions (void)			|
| Description: Displays the instructions to the	|
|	user.										|
| Parameters: None.								|
| Returns: None.								|
\***********************************************/
void Instructions (void)
{
	cout << "Lottery Simulator" << endl << endl;
	cout << "This program will prompt you for a 4-digit lottery ticket" << endl
		 << "number, where each number is an integer between 1 and 20." << endl
		 << "Then, 1000000 random lottery draws will be generated, and" << endl
		 << "both the number of jackpots (4 matching numbers) and " << endl
		 << "secondary prizes (3 matching numbers) for your ticket will" << endl
		 << "be displayed." << endl 
		 << endl
		 << "If you would like to have random numbers generated for" << endl
		 << "you, enter 0 as your first number." << endl << endl;
}

/***********************************************\
| Function: int GetInt (int iLowerBound,		|
						int iUpperBound)		|
| Description: Gets an integer from cin that is |
|	between iLowerBound and iUpperbound			|
| Parameters:	iLowerBound: the lowest number	|
|	that can be input.							|
|				iUpperBound: the highest number	|
|	thats can be input.							|
|	If iUpperBound < iLowerBound, they are		|
|	automatically swapped internally.			|
| Returns: The number that was entered.			|
\***********************************************/
int GetInt (int iLowerBound, int iUpperBound)
{
	//The value to be returned
	int iRetVal = 0;

	//Swap the boundaries if they are invalid
	if (iLowerBound > iUpperBound)
	{
		int iTemp = iLowerBound;
		iLowerBound = iUpperBound;
		iUpperBound = iTemp;
	}

	//Grab a number form the keyboard
	cin >> iRetVal;

	//If the input is out of bounds or is not a number at all, spit out
	//an error message
	while (cin.fail () || iRetVal < iLowerBound || iRetVal > iUpperBound)
	{
		//clear any problems cin may be having
		cin.clear ();
		cin.ignore (cin.rdbuf ()->in_avail (), '\n');

		//Ask the user to try again
		cout << "Error, input must be an integer from " << iLowerBound << " to " << iUpperBound << " :";
		cin  >> iRetVal;
	}

	return iRetVal;
} //GetInt()

/***************************************************\
| Function: void GetTicketNumbers (int* piNumber1,	|
|								   int* piNumber2,	|
|								   int* piNumber3,	|
|								   int* piNumber4)	|
| Description: Gets 4 unique integers from the user.|
|	If the first number entered is 0, then all 4	|
|	will be randomly generated numbers.				|
| Parameters: 4 int*, used to return the numbers	|
|	that were received from GetInt ()				|
| Returns: None.									|
\***************************************************/
void GetTicketNumbers (int* piNumber1, 
					   int* piNumber2, 
					   int* piNumber3, 
					   int* piNumber4)
{
	//Get the first number
	cout << "Enter the first ticket number: ";
	*piNumber1 = GetInt (0, kiMaxTicketNumber);
    
	//If Number1 is 0 then generate all 4 randomly, diplay them and exit
	if (0 == *piNumber1)
	{
		cout << "Generating your numbers randomly....\n";
		Draw (piNumber1, piNumber2, piNumber3, piNumber4);
		cout << "Your randomly selected number are: " 
			 << *piNumber1 << ", "
			 << *piNumber2 << ", "
			 << *piNumber3 << " and "
			 << *piNumber4 << endl;
		return;
	}

	//Get the second number
	cout << "Enter the second ticket number: ";
	*piNumber2 = GetInt (kiMinTicketNumber, kiMaxTicketNumber);

	//Make sure the second number is unique
	while (*piNumber1 == *piNumber2)
	{
		cout << "Error, each number must be unique: ";
		*piNumber2 = GetInt (kiMinTicketNumber, kiMaxTicketNumber);
	}

	//Get the third number
	cout << "Enter the third ticket number: ";
	*piNumber3 = GetInt (kiMinTicketNumber, kiMaxTicketNumber);

	//Make sure the third number is unique
	while (*piNumber1 == *piNumber3 || *piNumber2 == *piNumber3)
	{
		cout << "Error, each number must be unique: ";
		*piNumber3 = GetInt (kiMinTicketNumber, kiMaxTicketNumber);
	}
    
	//Get the fourth number
	cout << "Enter the fourth ticket number: ";
	*piNumber4 = GetInt (kiMinTicketNumber, kiMaxTicketNumber);

	//Make sure the fourth number is unique
	while (*piNumber1 == *piNumber4 || *piNumber2 == *piNumber4 || *piNumber3 == *piNumber4)
	{
		cout << "Error, each number must be unique: ";
		*piNumber4 = GetInt (kiMinTicketNumber, kiMaxTicketNumber);
	}
} //GetTicketNumbers()

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
inline int GetRandInt (int iLowerBound, int iUpperBound)
{
	//check if the boundaries are invalid
	if (iLowerBound > iUpperBound)
	{
		//swap them if they are
		int temp = iUpperBound;
		iUpperBound = iLowerBound;
		iLowerBound = temp;
	}

	//send back the random number
	return rand() % (iUpperBound - iLowerBound) + iLowerBound;
}

/***********************************************\
| Function: void Draw  (int* piNumber1,			|
|						int* piNumber2,			|
|						int* piNumber3,			|
|						int* piNumber4)			|
| Description: Gets 4 unique random numbers  	|
| Parameters: Requires the location of 4 ints,	|
|	which are used to return the numbers		|
|	that have been randomly selected.			|
| Returns: None.								|
\***********************************************/
void Draw (int* piNumber1, 
		   int* piNumber2, 
		   int* piNumber3, 
		   int* piNumber4)
{
	do
	{
		//Get 4 random numbers
		*piNumber1 = GetRandInt (kiMinTicketNumber, kiMaxTicketNumber);
		*piNumber2 = GetRandInt (kiMinTicketNumber, kiMaxTicketNumber);
		*piNumber3 = GetRandInt (kiMinTicketNumber, kiMaxTicketNumber);
		*piNumber4 = GetRandInt (kiMinTicketNumber, kiMaxTicketNumber);

	//Make sure all the numbers are unique (if they aren't try again)
	} while (!EqualityTest (*piNumber1, *piNumber2, *piNumber3, *piNumber4));
}

/***********************************************\
| Function:										|
|   int Simulate (const int iTicketNumber1,		|
|				  const int iTicketNumber2,		|
|				  const int iTicketNumber3,		|
|				  const int iTicketNumber4,		|
|				  const int iTimesToDraw)		|
| Description: Simulates the user buying 		|
|	iTimesToDraw lottery tickets, all with the	|
|	same numbers. Also keeps track and displays	|
|	of how many times those numbers win or		|
|	almost win the lottery.						|
| Parameters: The 4 numbers chosen by the user,	|
|	and how many times the simulation should be |
|	executed.									|
| Returns: None.								|
\***********************************************/
void Simulate (const int iTicketNumber1, 
			   const int iTicketNumber2, 
			   const int iTicketNumber3,
			   const int iTicketNumber4, 
			   const int iTimesToDraw)
{
	//counter for 'for' loop
	int i = 1;
	//Number of times a complete match is found (4 numbers)
	int iWins = 0;
	//Number of times a partial match is found (3 numbers)
	int iPartialWins = 0;
	//counter the amount of matching number found
	int iMatchCount = 0;
	//4 randomly drawn numbers, the winning lottery ticket
	int iDrawNumber1 = 0;
	int iDrawNumber2 = 0;
	int iDrawNumber3 = 0;
	int iDrawNumber4 = 0;

	//Repeat how ever many times are needed
	for (; i < iTimesToDraw; i++)
	{
		//Reset iMatchCount every time
		iMatchCount = 0;

		//Pick 4 unique random numbers
        Draw (&iDrawNumber1, &iDrawNumber2, &iDrawNumber3, &iDrawNumber4);

		//See how many numbers match the users tickets
		if (iTicketNumber1 == iDrawNumber1 ||
			iTicketNumber1 == iDrawNumber2 ||
			iTicketNumber1 == iDrawNumber3 ||
			iTicketNumber1 == iDrawNumber4)
			iMatchCount++;

		if (iTicketNumber2 == iDrawNumber1 ||
			iTicketNumber2 == iDrawNumber2 ||
			iTicketNumber2 == iDrawNumber3 ||
			iTicketNumber2 == iDrawNumber4)
			iMatchCount++;

		if (iTicketNumber3 == iDrawNumber1 ||
			iTicketNumber3 == iDrawNumber2 ||
			iTicketNumber3 == iDrawNumber3 ||
			iTicketNumber3 == iDrawNumber4)
			iMatchCount++;

		if (iTicketNumber4 == iDrawNumber1 ||
			iTicketNumber4 == iDrawNumber2 ||
			iTicketNumber4 == iDrawNumber3 ||
			iTicketNumber4 == iDrawNumber4)
			iMatchCount++;

		//All 4 matching counts as a win!
		if (4 == iMatchCount)
			iWins++;

		//Only 3 matching counts as a near miss
		if (3 == iMatchCount)
			iPartialWins++;
	}//for i < iTimesToDraw

	//Display the results
	cout << endl << "Your numbers have " 
		 << iWins << " wins and " 
		 << iPartialWins << " partial matches." << endl;
} //simulate ()

/***********************************************\
| Function:										|
|   bool EqualityTest  (const int kiA,			|
|						const int kiB,			|
|						const int kiC,			|
|						const int kiD,			|
| Description: Test to make sure all numbers	|
|	passed to the function are unique.			|
| Parameters: Any 4 numbers that need to be 	|
|	tested.										|
| Returns: false if any numbers are non-unique	|
|	true if all are unique.						|
\***********************************************/
bool EqualityTest (const int kiA, const int kiB, const int kiC, const int kiD)
{
	//Are any values duplicated?
	if (kiA == kiB || kiA == kiC || kiA == kiD ||
		kiB == kiC || kiB == kiD ||
		kiC == kiD)
		return false;	//yes
	else
		return true;	//no
}

int main ()
{
	//The numbers selected by the user
	int iTicketNumber1 = 0;
	int iTicketNumber2 = 0;
	int iTicketNumber3 = 0;
	int iTicketNumber4 = 0;

	//seed rand()
	srand (static_cast<unsigned int> (time (0)));

	//Display the instructions
	Instructions ();

	//Get the ticket numbers from the user
	GetTicketNumbers (&iTicketNumber1, &iTicketNumber2, &iTicketNumber3, &iTicketNumber4);

	//Run the simulation 1 million times!
	Simulate (iTicketNumber1, iTicketNumber2, iTicketNumber3, iTicketNumber4, 1000000);

	//Bye-bye
	system ("pause");
	return 0;
}