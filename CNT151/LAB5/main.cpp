/***************************************************\
* Project:		Lab 5 - Yahtzee						*
* Files:		main.cpp, main.h,					*
*				utilities.cpp, utilities.h			*
*				io.h, io.cpp, logic.h, logic.cpp	*
* Date:			12 Apr 2006							*
* Author:		Addison Babcock		Class:  CNT15	*
* Instructor:	J. D. Silver		Course: CNT151	*
* Description:	A text-based game of Yahtzee		*
\***************************************************/

#include "main.h"
#include "utilities.h"
#include "io.h"
#include "logic.h"

int main (void)
{
	//An array to hold the dice that were randomly generated
	unsigned int uiDice [gkuiDiceCount] = {0};
	//A counter for how many time the dice were rolled
	//used to limit the user to 2 rerolls
	unsigned int uiRerollCount = 1;
	//The category that was entered
	unsigned int uiCategorySelected = 0;
	//The string for which dice to reroll
	char szReroll [gkuiDiceCount + 1] = "YYYYY";
	//Valid menu options for the scoring selection screen
	char szValidOptions [gkuiMaxStrLen] = {0};
	//Keep playing?
	bool bPlayAgain = true;
	//The players
	SPlayer sPlayers [gkuiMaxPlayers];
	//How many players are currently playing?
	unsigned int uiPlayerCount = 0;
	//The round number
	unsigned int uiRoundCount = 0;
	//The winner of the game
	unsigned int uiWinner = 0;

	//Init the game
	DisplayWelcome ();
	SeedRandomGenerator ();

	//While we want to keep going
	while (bPlayAgain)
	{
		//if the user wants to open a save game, let him
		//otherwise init sScores to the default values
		cout << "Would you like to load a game? ";
		if ('Y' == toupper (GetMenuChoice ("YyNn")))
			LoadGame (sPlayers, &uiPlayerCount);
		else
			Initialize (sPlayers, &uiPlayerCount);
		system ("CLS");

		//One game is gkuiCategoryCount rounds
		//this loops through each round
		while (gkuiCategoryCount > GetRoundCount (sPlayers))
		{
			//Give each player a turn
			for (unsigned int uiCurrentPlayer = 0; 
				 uiCurrentPlayer < uiPlayerCount; 
				 uiCurrentPlayer++)
			{
				//Empty the dice array
				for (int i = 0; i < gkuiDiceCount; i++)
					uiDice [i] = 0;

#ifdef _DEBUG
				cout << sPlayers [uiCurrentPlayer].m_szName << ", Enter dice: ";
				for (int i = 0; i < gkuiDiceCount; i++)
					cin >> uiDice [i];
#endif

#ifndef _DEBUG
				//only 1 roll
				uiRerollCount = 1;

				//Get some random dice and display them
				Roll (uiDice);
				cout << sPlayers [uiCurrentPlayer].m_szName << ", your dice are:\n";
				DisplayDice (uiDice);

				//Do the reroll's
				do	{
					//find out which dice need to be rerolled
					GetRerolls (szReroll);

					//If the user doesnt want to reroll any, drop out of the loop
					if (0 == strcmpi (szReroll, "NNNNN"))
						uiRerollCount = 3;

					//the dice that need to be rerolled are blanked...
					for (int i = 0; i < gkuiDiceCount; i++)
					{			
						if ('Y' == szReroll [i] ||
							'y' == szReroll [i])
						{
							uiDice [i] = 0;
						}
					}

					//...and rerolled...
					Roll (uiDice);

					//...then displayed
					system ("CLS");
					cout << sPlayers [uiCurrentPlayer].m_szName 
						 << ", your new dice are:\n";
					DisplayDice (uiDice);

					uiRerollCount++;
				//only allow 2 chances to reroll, 3 rolls total
				} while (uiRerollCount <= 2);
#endif

				//now that we have our dice, they can be scored
				//display the menu
				DisplayScoreMenu (sPlayers [uiCurrentPlayer].m_sScores, szValidOptions);
				//get the category choice
				cout << "Enter your choice: ";
				uiCategorySelected = toupper (GetMenuChoice (szValidOptions)) - 'A';

				//score the selected category
				//if the player gets another turn...
				if (Score (uiCategorySelected, 
						   sPlayers [uiCurrentPlayer].m_sScores, 
						   uiDice, 
						   &(sPlayers[uiCurrentPlayer].m_uiYahtzeeCount)))
				{
					system ("CLS");
					cout << "Congratulations, " << sPlayers [uiCurrentPlayer].m_szName
						 << ", you have been granted an extra turn!\n\n";
					//...run this loop again for the same player
					uiCurrentPlayer--;
					continue;
				}

				//add up the totals
				CalculateScore (sPlayers + uiCurrentPlayer);
				system ("CLS");
			}//for each player

			//and display the whole thing
			DisplayScores (sPlayers);
			system ("pause");
			system ("cls");

			//ask the user if he/she would like to save after each round
			cout << "Would you like to save your game? ";
			if ('Y' == toupper (GetMenuChoice ("YyNn")))
				SaveGame (sPlayers, &uiPlayerCount);
		}//while (GetRoundCount (sPlayers) < gkuiCategoryCount)

		//Display the winner
		uiWinner = GetWinner (sPlayers);
		system ("cls");
		cout << sPlayers [uiWinner].m_szName << " won this game with a total of " 
			 << sPlayers [uiWinner].m_uiTotalPoints << " points.\n ";

		//Check the high scores
		HighScores (sPlayers [uiWinner].m_uiTotalPoints);

		//Should we start a new game?
		cout << "\nPlay again (y/n)? ";
		bPlayAgain = ('Y' == toupper (GetMenuChoice ("YyNn")));
	}//while (bPlayAgain)

	return 0;
}//end main()