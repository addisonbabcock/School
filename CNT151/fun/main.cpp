#include <iostream>
#include <stdlib.h>
#include <time.h>

using namespace std;

#include "main.h"

int humanScore = 0;		// Init vars
int computerScore = 0;
int roundCount = 0;
int humanInput = 0;
int computerInput = 0;

void testForWinCondition (int human, int computer)
{
	switch (human)
	{
	case rock:
		switch (computer)
		{
			case rock:
				executeResults (tie);
				break;
			case paper:
				executeResults (computerWin);
				break;
			case scissors:
				executeResults (humanWin);
				break;
		}
		break;
	case paper:
		switch (computer)
		{
			case rock:
				executeResults (humanWin);
				break;
			case paper:
				executeResults (tie);
				break;
			case scissors:
				executeResults (computerWin);
				break;
		}
		break;
	case scissors:
		switch (computer)
		{
			case rock:
				executeResults (computerWin);
				break;
			case paper:
				executeResults (humanWin);
				break;
			case scissors:
				executeResults (tie);
				break;
		}
		break;
	}
}

void executeResults (int victor)
{
	computerInput = 0;
	humanInput = 0;

	if (victor == tie)
	{
		cout << "TIE!" << endl;

		return;
	}
	if (victor == computerWin)
	{
		cout << "Computer wins this round." << endl;
		computerScore++;
		roundCount++;
		displayScore();

		return;
	}
	if (victor == humanWin)
	{
		cout << "YOU WON THIS ROUND!" << endl;
		humanScore++;
		roundCount++;
		displayScore();

		return;
	}
	cout << "oops." << endl;
}

void displayScore ()
{
	cout << "At the end of round " << roundCount << " the score is computer " << computerScore << ", human " << humanScore<< ". " << endl;
	return;
}

int main ()
{
	cout << "Welcome to Rock, Paper, Scissors" << endl;

	while (roundCount < 7)
	{
		while (!(humanInput >= 1 && humanInput <= 3)) // Display choices, get input and ensure validity
		{
			cout << "Rock: " << rock << endl << "Paper: " << paper << endl << "Scissors: " << scissors << endl;
			cout << "Please input your choice of attack: ";
			cin >> humanInput;
		}
		
		//Get random number here when I remember how to get random numbers
		srand ((unsigned) time(NULL));
		computerInput = (rand () % 3) + 1;	
	
		cout << "Computer has chosen: " << computerInput << endl;	
	
		testForWinCondition (humanInput, computerInput);	
	}
	cout << "Goodbye. " << endl;
	system ("pause");

	return 0;
}