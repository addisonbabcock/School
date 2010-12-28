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

/***********************************************************************\
| Function:	int GetRoundCount (SPlayer sPlayers [gkuiMaxPlayers],		| 
|							   unsigned int uiPlayer = 0)				|
| Description: returns the current round # based on a players scoring	|
|	data.																|
| Parameters: sPlayers is the array player data. uiPlayerNumber is the	|
	player for which the round will be determined (starts at 0).		|
| Returns: The current round number.									|
\***********************************************************************/
int GetRoundCount (SPlayer const sPlayers [gkuiMaxPlayers])
{
	//The value to be returned
	int iRetVal = 0;

	//Loop through each category
	for (int i = 0; i < gkuiCategoryCount; i++)
		//If the category is used
		if (sPlayers [0].m_sScores [i].m_bIsUsed)
			//Then we are one round further in
			iRetVal++;

	return iRetVal;
}

/***********************************************************************\
| Function:																|
|	void BuildFacesArray (unsigned int uiDice [gkuiDiceCount],			|
|		  unsigned int uiFaceCounts [gkuiDiceFaceMax])					|
| Description: Builds the array containing the number of each type of	|
|	dice that was found. EX: if the input is {1, 2, 3, 4, 5}, then		|
|	uiFaceCounts will contain {1, 1, 1, 1, 1, 0}						|
| Parameters: uiDice is the array that contains which dice were rolled	|
|	uiFaceCounts is the array to contain the number of each face.		|
| Returns: None.														|
\***********************************************************************/
void BuildFacesArray (unsigned int const uiDice [gkuiDiceCount],
					  unsigned int uiFaceCounts [gkuiDiceFaceMax])
{
	//empty the array
	for (int i = 0; i < gkuiDiceFaceMax; i++)
		uiFaceCounts [i] = 0;

	//loop through the dice
	for (int i = 0; i < gkuiDiceCount; i++)
		//count the dice
		uiFaceCounts [uiDice [i] - 1]++;
}

/***********************************************************************\
| Function:																|
|	void Score (unsigned int uiSelection,								|
|		SCategory sScores [gkuiCategoryCount],							|
|		unsigned int uiDice [gkuiDiceCount])							|
| Description: Calculates the score of a particular category. When a	|
|	category is used, it is no longer available to be used again.		|
| Parameters: sScores is the array of Categories, uiDice is the array	|
|	of int's that represents the dice that the score will be based on.	|
|	uiSelection is which category the user would like to be	scored.		|
|	puiYahtzeeCount is a pointer to the number of Yahtzee's previously	|
|	scored.																|
| Returns: true if the player should be given another turn.				|
\***********************************************************************/
bool Score (unsigned int const uiSelection, 
			SCategory sScores [gkuiCategoryCount],
			unsigned int const uiDice [gkuiDiceCount],
			unsigned int * const puiYahtzeeCount)
{
	//An array to store the number of each face that was found in uiDice
	unsigned int uiFaceCount [gkuiDiceFaceMax] = {0};
	//A temporary variable used in the calculation of categories 6,7,11,12
	unsigned int uiOfAKind = 0;
	//A temporary variable used in the calculation of bonus points for #11
	unsigned int uiPreviousScore = 0;
	//Has a yahtzee been found?
	bool bYahtzeeFound = false;

	//don't bother running this function if the category is already used
	//Yahtzee (#11) can be scored more then once
	if (sScores [uiSelection].m_bIsUsed && uiSelection != 11)
		return false;

	BuildFacesArray (uiDice, uiFaceCount);

	switch (uiSelection)
	{
		//Ones through Sixes
	case 0:
	case 1:
	case 2:
	case 3:
	case 4:
	case 5:
		//This category is now used and can not be used again
		sScores [uiSelection].m_bIsUsed = true;
		//For ones through sixes, the score is simply the number of faces
		//multiplied by the face value
		sScores [uiSelection].m_uiUserScore = uiFaceCount [uiSelection] *
											  sScores [uiSelection].m_uiPotentialScore;
		break;

		//3 of a kind, 4 of a kind, 5 of a kind (yahtzee), chance
	case 6:
	case 7:
	case 12:
		//If the user is trying to score a 3 of a kind, uiOfAKind
		//is set to 3, if the user is looking to score a 4 of a kind,
		//uiOfAKind is set to 4, and so on.

		//Chance actually more or less uses the same algorithm to calculate score
		//so it is included here as well (#12)
		//Note that if chance is selected, the outer 'for' and 'if' are not relevant 

		//3oak
		if (6 == uiSelection)
			uiOfAKind = 3;
		//4oak
		if (7 == uiSelection)
			uiOfAKind = 4;
		//chance (0oak)
		if (12 == uiSelection)
			uiOfAKind = 0;

		//look for a >= 3 or 4 or 0
		for (int i = 0; i < gkuiDiceFaceMax; i++)
		{
			if (uiFaceCount [i] >= uiOfAKind)
			{
				//x of a kind found, add up the score
				for (int j = 0; j < gkuiDiceCount; j++)
					sScores [uiSelection].m_uiUserScore += uiDice [j];
				//leave the loop when the score has been calculated
				break;
			}
		}

		//set the category to used even if there was no x of a kind
		sScores [uiSelection].m_bIsUsed = true;
		break;

		//Full house
	case 8:
		//Loop to find the 3
		for (int i = 0; i < gkuiDiceFaceMax; i++)
			//Loop to find the 2
			for (int j = 0; j < gkuiDiceFaceMax; j++)
				//If both are found
				if (3 == uiFaceCount [i] &&
					2 == uiFaceCount [j])
					//The user get points!
					sScores [8].m_uiUserScore = sScores [8].m_uiPotentialScore;

		sScores [8].m_bIsUsed = true;
		break;

		//Half-straight
	case 9:
		//Loop through each possible straight
		//1111xx
		//x1111x
		//xx1111
		for (int i = 0; i <= gkuiDiceFaceMax - 4; i++)
		{
			//all are greater then 1
			if (uiFaceCount [i    ] &&
				uiFaceCount [i + 1] &&
				uiFaceCount [i + 2] &&
				uiFaceCount [i + 3])
			{
				//SCORE!
				sScores [9].m_uiUserScore = sScores [9].m_uiPotentialScore;
			}
		}
		sScores [9].m_bIsUsed = true;
		break;

		//full straight
	case 10:
		//loop through each possible full straight
		//11111x
		//x11111
		for (int i = 0; i <= gkuiDiceFaceMax - 5; i++)
		{
			//all are greater then 1
			if (uiFaceCount [i    ] &&
				uiFaceCount [i + 1] &&
				uiFaceCount [i + 2] &&
				uiFaceCount [i + 3] &&
				uiFaceCount [i + 4])
			{
				//SCORE!
				sScores [10].m_uiUserScore = sScores [10].m_uiPotentialScore;
			}
		}
		sScores [10].m_bIsUsed = true;
		break;

	case 11:
		//Set the category to used, unless a yahtzee is found
		sScores [11].m_bIsUsed = true;

		//look for a 5
		for (int i = 0; i < gkuiDiceFaceMax; i++)
		{
			if (5 == uiFaceCount [i])
			{
				//Give the user some points
				bYahtzeeFound = true;
				break;
			}
		}

		if (bYahtzeeFound)
		{
			//50...150...250...350
			sScores [11].m_uiUserScore = 50 + 100 * ((*puiYahtzeeCount)++);
			//Category can be used again
			sScores [11].m_bIsUsed = false;
			//Give another turn
			return true;
		}

		break;
	}	//switch (uiSelection)

	//By default, don't hand out an extra turn
	return false;
}

/***********************************************************************\
| Function:																|
|	void CalculateScore (SPlayer* sPlayer)								|
| Description: Calculates the total and bonus points based off of		|
|	sScores.															|
| Parameters: sPlayer is a pointer to the player data.					|
| Returns: None.														|
\***********************************************************************/
void CalculateScore (SPlayer* const sPlayer)
{
	//counter for the for loops
	int i = 0;

	//Since the score will be recalculated, empty the score
	sPlayer->m_uiBonusPoints = 0;
	sPlayer->m_uiTotalPoints = 0;

	//add ones through sixes
	for (; i < 6; i++)
		sPlayer->m_uiTotalPoints += sPlayer->m_sScores [i].m_uiUserScore;

	//Give bonus points for having ones through sixes >= 63
	if (sPlayer->m_uiTotalPoints >= 63)
		sPlayer->m_uiBonusPoints = 50;

	//add the rest
	for (; i < gkuiCategoryCount; i++)
		sPlayer->m_uiTotalPoints += sPlayer->m_sScores [i].m_uiUserScore;

	//Add the bonus points to the total
	sPlayer->m_uiTotalPoints += sPlayer->m_uiBonusPoints;
}

/*******************************************************************\
| Function: void Initialize (SPlayer sPlayers [gkuiCategoryCount],	|
							 unsigned int* puiPlayerCount)			|
| Description: Init's the player data. Sets up all the myriad		|
|	scoring data and player names.									|
| Parameters: sPlayers is the array that will contain all the data	|
|	once initialized. puiPlayerCount is how many players will be	|
|	playing.														|
| Returns: None.													|
\*******************************************************************/
void Initialize (SPlayer sPlayers [gkuiCategoryCount], 
				 unsigned int* const puiPlayerCount)
{
	//How many players are we dealing with?
	cout << "How many players would like to play? ";
	*puiPlayerCount = GetInt (1, gkuiMaxPlayers);

	//For each player
	for (unsigned int i = 0; i < gkuiMaxPlayers; i++)
	{
		//If [i] is playing
		if (i < *puiPlayerCount)
		{
			//Get the players name
			cout << "What is your name, player " << i + 1 << "? ";
			FlushCINBuffer ();
			cin.getline (sPlayers [i].m_szName, gkuiMaxPNameLen + 1);
		}
		else
		{
			//otherwise, blank the name
			strcpy (sPlayers [i].m_szName, gkszDefaultPlayerName);
		}

		sPlayers [i].m_uiBonusPoints = 0;
		sPlayers [i].m_uiTotalPoints = 0;
		sPlayers [i].m_uiYahtzeeCount = 0;

		//Initialize all categories to the same defaults
		for (int j = 0; j < gkuiCategoryCount; j++)
		{
			//The category has obviously not been used yet
			sPlayers [i].m_sScores [j].m_bIsUsed = false;
			//Applicable only to ones through sixes
			sPlayers [i].m_sScores [j].m_uiPotentialScore = j + 1;
			//No points have been earned yet
			sPlayers [i].m_sScores [j].m_uiUserScore = 0;
		}

		//Set the potential score accordingly
		sPlayers [i].m_sScores [6].m_uiPotentialScore = 0;
		sPlayers [i].m_sScores [7].m_uiPotentialScore = 0;
		sPlayers [i].m_sScores [8].m_uiPotentialScore = 25;
		sPlayers [i].m_sScores [9].m_uiPotentialScore = 30;
		sPlayers [i].m_sScores [10].m_uiPotentialScore = 40;
		sPlayers [i].m_sScores [11].m_uiPotentialScore = 50;
		sPlayers [i].m_sScores [12].m_uiPotentialScore = 0;

		//Set the names of each category
		strcpy (sPlayers [i].m_sScores [0].m_szName, "Ones");
		strcpy (sPlayers [i].m_sScores [1].m_szName, "Twos");
		strcpy (sPlayers [i].m_sScores [2].m_szName, "Threes");
		strcpy (sPlayers [i].m_sScores [3].m_szName, "Fours");
		strcpy (sPlayers [i].m_sScores [4].m_szName, "Fives");
		strcpy (sPlayers [i].m_sScores [5].m_szName, "Sixes");
		strcpy (sPlayers [i].m_sScores [6].m_szName, "Three of a kind");
		strcpy (sPlayers [i].m_sScores [7].m_szName, "Four of a kind");
		strcpy (sPlayers [i].m_sScores [8].m_szName, "Full house");
		strcpy (sPlayers [i].m_sScores [9].m_szName, "Small straight");
		strcpy (sPlayers [i].m_sScores [10].m_szName, "Large straight");
		strcpy (sPlayers [i].m_sScores [11].m_szName, "YAHTZEE!");
		strcpy (sPlayers [i].m_sScores [12].m_szName, "Chance");
	}
}

/***********************************************************\
| Function: void Roll (unsigned int uiDice [gkuiDiceCount])	|
| Description: Randomly generates gkuiDiceCount random		|
|	numbers between gkuiDiceFaceMin and gkuiDiceFaceMax,	|
|	then stores them in an array. If the array already		|
|	contains a number, it will not be changed.				|
| Parameters: An array of size gkuiDiceCount to store the	|
|	numbers.												|
| Returns: None.											|
\***********************************************************/
void Roll (unsigned int uiDice [gkuiDiceCount])
{
	//For each element in the array
	for (int i = 0; i < gkuiDiceCount; i++)
		//Make sure the element is empty
		if (0 == uiDice [i])
			//Then randomize it
			uiDice [i] = static_cast <unsigned int> (GetRandInt (gkuiDiceFaceMin, 
																 gkuiDiceFaceMax));
}

/***********************************************************************\
| Function: unsigned int GetWinner (SPlayer sPlayers [gkuiMaxPlayers])	|
| Description: Determines the winner of a game based un m_uiTotalPoints	|
| Parameters: sPlayers is the array of structs to hold the player data.	|
| Returns: The winning player #, starting at 0.							|
\***********************************************************************/
unsigned int GetWinner (SPlayer const sPlayers [gkuiMaxPlayers])
{
	int iWinner = 0;
	
	for (int i = 0; i < gkuiMaxPlayers; i++)
		if (sPlayers [i].m_uiTotalPoints > sPlayers [iWinner].m_uiTotalPoints)
			iWinner = i;

	return iWinner;
}