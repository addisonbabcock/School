/***************************************************\
* Project:		Lab 2 - 8-bit Binary Calculator		*
* Files:		main.cpp, utilities.h, utilities.cpp*
* Date:			22 Sept 2006						*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	An 8 bit binary calculator that 	*
*	will perform most binary operations.			*
\***************************************************/

#include <iostream>
#include "utilities.h"

using namespace std;

void Instructions ();
void OperationsMenu ();
void ShowByte (unsigned char const ucByte);
unsigned char GetByte ();
unsigned int GetShift ();
char GetOperation ();

/*******************************************************\
| Function: void Instructions ()						|
| Description: Displays the introduction on the screen.	|
| Parameters: None										|
| Returns: None.										|
\*******************************************************/
void Instructions ()
{
	//display a welcome
	cout << "		8-bit Binary Calculator\n\n";

	cout << "This calculator will perform logical AND, OR, XOR, Invert\n"
		 << "shift left and shift right on 8 bit binary values. Please\n"
		 << "enter the first 8 bit value, then select the operation.\n\n";
}

/*******************************************************\
| Function: void OperationsMenu ()						|
| Description: Displays the menu of available			|
|	operations on the screen.							|
| Parameters: None.										|
| Returns: None.										|
\*******************************************************/
void OperationsMenu ()
{
	//barf! menu!
	cout << "Select one of the following operations...\n\n";
	cout << "| Binary OR.\n"
		<< "& Binary AND.\n"
		<< "^ Binary XOR.\n"
		<< "~ Binary Invert.\n"
		<< "< Shift Left.\n"
		<< "> Shift Right.\n\n"
		<< "Select Operation: ";
}

/*******************************************************\
| Function: void ShowByte (unsigned char const ucByte)	|
| Description: Displays an unsigned character as a		|
|	binary value.										|
| Parameters: The unsigned character to be displayed.	|
| Returns: None.										|
\*******************************************************/
void ShowByte (unsigned char const ucByte)
{
	//This mask is used to pull one bit out of a unsigned char
	//10000000
	unsigned char ucMask = 0x80;

	//Go through each bit of the character
	for (int i = 8; i > 0; i--)
	{
		//Display a one if the value is 1, otherwise display 0.
		if (ucByte & ucMask)
			cout << "1";
		else
			cout << "0";

		//Move the mask one bit to the right.
		ucMask = ucMask >> 1;
	}
}

/*******************************************************\
| Function: unsigned char GetByte ()					|
| Description: Retrieves an 8 bit binary value from the	|
|	user and stores it as an unsigned character. Also	|
|	performs error checking to make sure the value		|
|	entered is exactly 8 bits in length and that all	|
|	the characters are valid.							|
| Parameters: None.										|
| Returns: The value entered as an unsigned char.		|
\*******************************************************/
unsigned char GetByte ()
{
	//Is the input valid?
	bool bValid = false;
	//The string the hold the input
	//8 characters for each bit, 1 for error correction, 1 for null terminator
	char szInput [10] = {0};
	//The byte that will be returned
	unsigned char ucRetVal = 0;
	//The mask used in converting a string into a byte.
	// 10000000
	unsigned char ucMask = 0x80;

	// while (!bValid)
	//Keep getting a new input while its not valid
	do{
		//Keep getting a new string until its 8 bytes long
		while (strlen (szInput) != 8)
		{
			cout << "Enter an eight bit binary value: ";
			FlushCINBuffer ();
			cin.getline (szInput, 10);
		}

		//No faults found yet
		bValid = true;
		//Go through each 'bit'
		for (int i = 0; i < 8; i++)
		{
			//Make sure that only 1 and 0 was entered
			if (szInput [i] != '1' && szInput [i] != '0')
			{
				//The entire string is now invalid, and must be entered again
				cout << '\'' << szInput [i] << "\' is not a \'1\' or \'0\'.\n";
				cout << "Please correct it:\n";
				bValid = false;
				szInput [0] = 0;
				i = 0;
				ucMask = 0x80;
				ucRetVal = 0;
				break;
			}

			//If the bit entered is 1, add the mask into the result.
			if ('1' == szInput [i])
				ucRetVal += ucMask;

			//The mask is shifted 1 to the right, the next iteration will be 
			//dealing with a lesser significant bit
			ucMask = ucMask >> 1;
		}
	}
	while (!bValid);

	return ucRetVal;
}

/*******************************************************\
| Function: unsigned int GetShift ()					|
| Description: Gets an int from 0 to 8 from the user.	|
| Parameters: None.										|
| Returns: The value from the user as an unsigned int.	|
\*******************************************************/
unsigned int GetShift ()
{
	//Get an int and send it back
	return GetInt (0, 8);
}

/*******************************************************\
| Function: char GetOperation ()						|
| Description: Displays the operations menu and then	|
|	asks the user to select an option from that menu.	|
| Parameters: None.										|
| Returns: The operation entered as a character.		|
\*******************************************************/
char GetOperation ()
{
	//Show the menu, get the selection and send it back
	OperationsMenu ();
	return GetMenuChoice ("|&^~<>");
}

int main (void)
{
	//The bytes that have been entered
	unsigned char ucBytes [2] = {0};
	//The operation to perform on the byte(s)
	char cOperation = 0;
	//How many bits to shift
	int iShift = 0;
	//Keep running the program
	bool bContinue = true;

	//Display the intro
	Instructions ();

	//Keep running the program until the user is done.
	while (bContinue)
	{
		//Clear out the bytes
		ucBytes [0] = ucBytes [1] = 0;
		//No operation or shift selected yet
		cOperation = 0;
		iShift = 0;

		//Get the first operand
		cout << endl << "First operand...\n";
		ucBytes [0] = GetByte ();

		//Get the operation
		cOperation = GetOperation ();

		//If the operation requires 2 operands, get the second operand as well
		if ('|' == cOperation || '&' == cOperation || '^' == cOperation)
		{
			cout << "Second Operand...\n";
			ucBytes [1] = GetByte ();
		}

		//do a the operation
		switch (cOperation)
		{
		//bit-wise or
		case '|':
			cout << "\n  ";
			ShowByte (ucBytes [0]);
			cout << "\n| ";
			ShowByte (ucBytes [1]);
			cout << "\n----------\n"
				<< "  ";
			ShowByte (ucBytes [0] | ucBytes [1]);
			break;

		//bit-wise and
		case '&':
			cout << "\n  ";
			ShowByte (ucBytes [0]);
			cout << "\n& ";
			ShowByte (ucBytes [1]);
			cout << "\n----------\n"
				<< "  ";
			ShowByte (ucBytes [0] & ucBytes [1]);
			break;

		//bit-wise xor
		case '^':
			cout << "\n  ";
			ShowByte (ucBytes [0]);
			cout << "\n^ ";
			ShowByte (ucBytes [1]);
			cout << "\n----------\n"
				<< "  ";
			ShowByte (ucBytes [0] ^ ucBytes [1]);
			break;

		//bit-wise invert
		case '~':
			cout << "\n~ ";
			ShowByte (ucBytes [0]);
			cout << "\n----------\n"
				<< "  ";
			ShowByte (~ucBytes [0]);
			break;

		//left shift
		case '<':
			//get the shift distance
			cout << "Enter the number of bits to shift left: ";
			iShift = GetShift ();

			//display the results
			cout << "\n\n";
			ShowByte (ucBytes [0]);
			cout << " << " << iShift << " = ";
			ShowByte (ucBytes [0] << static_cast<unsigned char> (iShift));
			break;

		//right shift
		case '>':
			//get the shift distance
			cout << "Enter the number of bits to shift right: ";
			iShift = GetShift ();

			//display the results
			cout << "\n\n";
			ShowByte (ucBytes [0]);
			cout << " >> " << iShift << " = ";
			ShowByte (ucBytes [0] >> static_cast<unsigned char> (iShift));
			break;
		}

		//find out if the user is done
		cout << "\n\nExit program? <Y> for yes: ";
		bContinue = 'N' == toupper (GetMenuChoice ("YyNn"));
	}//while (bContinue)

	//bye bye
	cout << endl;
	system ("pause");
	return 0;
}
