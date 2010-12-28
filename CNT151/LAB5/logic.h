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

#ifndef LOGIC_H
#define LOGIC_H

void Initialize (SPlayer sPlayers [gkuiCategoryCount], 
				 unsigned int* const puiPlayerCount);
void Roll (unsigned int uiDice [gkuiDiceCount]);
int GetRoundCount (SPlayer const sPlayers [gkuiMaxPlayers]);
void BuildFacesArray (unsigned int const uiDice [gkuiDiceCount],
					  unsigned int uiFaceCounts [gkuiDiceFaceMax]);
bool Score (unsigned int const uiSelection, 
			SCategory sScores [gkuiCategoryCount],
			unsigned int const uiDice [gkuiDiceCount],
			unsigned int * const puiYahtzeeCount);
void CalculateScore (SPlayer* const sPlayer);
unsigned int GetWinner (SPlayer const sPlayers [gkuiMaxPlayers]);

#endif //LOGIC_H