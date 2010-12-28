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

#include "main.h"

/***********************************************\
| Function: void DisplayInstructions (void)		|
| Description: Displays the instructions to the	|
|	user.										|
| Parameters: None.								|
| Returns: None.								|
\***********************************************/
void DisplayInstructions (void)
{
	cout << "\t\t\tMastermind!\n\n"
		<< "Welcome to the game of Mastermind. I will generate a "
		<< kuiSecretSize << " digit secret\n"
		<< "number composed of the digits " << kuiGuessRangeMin 
		<< " to " << kuiGuessRangeMax << ". You will attempt to guess my\n"
		<< "secret number and I will tell you how many digits are in the secret\n"
		<< "number, and how many are also in the exact position.\n\n";

	system ("pause");
	system ("cls");
}

/***********************************************\
| Function: bool PlayAgain (void)				|
| Description: Asks the user if he/she would	|
|	like to play again.							|
| Parameters: None.								|
| Returns: false if "quit" is entered,		 	|
|	returns true if the user enters yes			|
\***********************************************/
bool PlayAgain ()
{
	//A temporary  string to hold input
	char szInput [kuiMaxStrLen] = {0};

	while (true)
	{
		//Does the user want to quit?
		cout << "Would you like to play again (yes/quit)? ";
		FlushCINBuffer ();
		cin.getline (szInput, kuiMaxStrLen);
		cout << endl;	

		//If the user wants to quit
		if (!strcmpi (szInput, "quit"))
			//exit the function (and the program)
			return false;
        
		if (!strcmpi (szInput, "yes"))
			//exit the function, but continue to play.
			return true;
	}
}

/***********************************************\
| Function: void SetSecret (int* piSecret)		|
| Description: Randomly generates a secret code	|
|	for the user to guess.						|
| Parameters: A pointer to an array of size 	|
|	kuiSecretSize.								|
| Returns: A randomly generated number returned	|
|	through piSecret.							|
\***********************************************/
void SetSecret (int* piSecret)
{
	//Give all the array a random number
	for (int i = 0; i < kuiSecretSize; i++)
		piSecret[i] = GetRandInt (kuiGuessRangeMin, kuiGuessRangeMax);
}

/***********************************************\
| Function: void GetSecret (int* iGuess)		|
| Description: Prompts the user to attempt to	|
|	crack the secret code, also does a lot of	|
|	error checking.								|
| Parameters: A pointer to an array of size		|
|	kuiSecretSize.								|
| Returns: The array entered through iGuess.	|
\***********************************************/
void GetSecret (int iGuess [kuiSecretSize])
{
	//A string to hold the input
	char szInput [kuiMaxStrLen] = {0};
	//Is the input valid?
	bool bInputOK = true;
	//The size of the input
	unsigned int uiInputLen = 0;

	do
	{
		//Get the users guess
		FlushCINBuffer ();
		cout << "Enter your guess: ";
		cin.getline (szInput, kuiMaxStrLen);
		uiInputLen = static_cast <unsigned int> (strlen (szInput));

		//Check to make sure the guess is the right size
		if (uiInputLen < kuiSecretSize)
		{
			cout << "Guess is too small, please try again.\n";
			bInputOK = false;
		}
		if (uiInputLen > kuiSecretSize)
		{
			cout << "Guess is too big, please try again.\n";
			bInputOK = false;
		}

		if (uiInputLen == kuiSecretSize)
		{
			//the input is the right length, now check each character
			for (int i = 0; i < kuiSecretSize; i++)
			{
				//make sure all the digits entered in numbers
				if (!isdigit (szInput [i]))
				{
					//nag the user
					cout << "\'" << szInput [i] << "\' is not a valid digit.\n";
					i = kuiSecretSize; //exit the loop
					bInputOK = false;  //get a new guess
				}
				else
				{
					//input is a digit, turn it into a number
					iGuess [i] = static_cast<int> (szInput [i] - '0');

					//is that number out of range?
					if (iGuess [i] > kuiGuessRangeMax || 
						iGuess [i] < kuiGuessRangeMin)
					{
						//nag the user
						cout << "\'" << iGuess [i] << "\' is out of range.\n";
						i = kuiSecretSize; //exit the loop
						bInputOK = false;  //and get a new number
					}
					else
						bInputOK = true;   //everything checks out for that digit
				}
			} //for (int i ...)
		} //if InputLength == SecretSize
	} while (!bInputOK); //continue to get new input while the input is bad
} //end of GetSecret ()

/***************************************************************\
| Function: void CheckGuess (const int iGuess [kuiSecretSize],	|
|	const int iSecret [kuiSecretSize],							|
|	int* piCorrect, int* piWrongSpot)							|
| Description: Checks how many of the guessed numbers are		|
|	correct and how many are the right number in the wrong spot |
| Parameters: The two arrays to be compared and two int* for 	|
|	the return values.											|
| Returns: The number of correct digits and the number of semi-	|
|	correct digits.												|
\***************************************************************/
void CheckGuess (const int iGuess [kuiSecretSize], 
				 const int iSecret [kuiSecretSize], 
				 int* piCorrect, int* piWrongSpot)
{
	//clear piCorrect and piWrongSpot
	(*piCorrect) = (*piWrongSpot) = 0;

	//Loop through each number in iGuess
	for (int i = 0; i < kuiSecretSize; i++)
	{
		//If they match, add 1 to the correct count
		if (iGuess [i] == iSecret [i])
		{
			(*piCorrect)++;
		}
		else
		{
			//if they don't match, loop through iSecret
			for (int j = 0; j < kuiSecretSize; j++)
			{
				//Numbers in the wrong spot are counted, 
				//but not numbers in the right spot.
				if (iGuess [i] == iSecret [j] && i != j)
				{
					(*piWrongSpot)++;
				}
			} // for j
		} // if guess == secret
	} //for i
} // end of CheckGuess ()

int main ()
{
	//The random secret code
	int iSecret [kuiSecretSize] = {0};
	//The users crack at the secret
	int iGuess [kuiSecretSize] = {0};
	//A count of how many are right
	int iCorrect = 0;
	//A count of how many are close
	int iClose = 0;
	//A count of how many guess it took to crack the code
	int iGuessCount = 0;
	//Should the game keep playing?
	bool bKeepPlaying = true;
	//Did the user guess wrong?
	bool bKeepGuessing = true;

	//Initialize
	SeedRandomGenerator ();
	DisplayInstructions ();

	while (bKeepPlaying)
	{
		//Get a new secret number
		SetSecret (iSecret);
		//Reset bKeepGuessing
		bKeepGuessing = true;

		while (bKeepGuessing)
		{

#ifdef _DEBUG
			//Display the secret code if in debug mode
			DisplayArray (iSecret, kuiSecretSize, ' ');
#endif

			//Get the users guess and check for winning conditions
			GetSecret (iGuess);
			CheckGuess (iGuess, iSecret, &iCorrect, &iClose);
			iGuessCount++;

			//If the user got it right
			if (kuiSecretSize == iCorrect)
				bKeepGuessing = false;
			else
				cout << iCorrect << " are correct and "
					 << iClose << " are the right number in the wrong spot.\n\n";
		}

		//Display a congratulatory message and the users score
		cout << "Congratulations! You've cracked the code!\n"
			 << "It took you " << iGuessCount << " tries.\n\n";
		//Reset the score
		iGuessCount = 0;
		//Would you like to play again?
		bKeepPlaying = PlayAgain ();
	} //while (bKeepPlaying)	

	return 0; //Bye-bye
}