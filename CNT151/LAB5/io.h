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

#ifndef IO_H
#define IO_H

//This needs to be updated everytime the save/load functions change
const char gkszFileHeader [15] = "YahtzeeDoom1.2";
const int gkiHeaderLen = 15;

void DisplayWelcome ();
void GetRerolls (char szRerolls [gkuiDiceCount + 1]);
void DisplayScoreMenu (SCategory const sScores [gkuiCategoryCount], 
					   char szValidInput [gkuiMaxStrLen]);
void DisplayScores (SPlayer const sPlayers [gkuiMaxPlayers]);
void DisplayDice (unsigned int const uiDice [gkuiDiceCount]);
void SaveGame (SPlayer sPlayers [gkuiMaxPlayers], 
			   unsigned int* const uiPlayerCount);
void LoadGame (SPlayer sPlayers [gkuiMaxPlayers], 
			   unsigned int* const uiPlayerCount);
void HighScores (const unsigned int uiPlayersScore);

#endif //IO_H