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

#ifndef MAIN_H
#define MAIN_H

#include <iostream>
#include <string>
#include <fstream>

using namespace std;

//Number of dice per roll
const unsigned int gkuiDiceCount = 5;
//Dice go from 1-6
const unsigned int gkuiDiceFaceMax = 6;
const unsigned int gkuiDiceFaceMin = 1;
//13 scoring categories available
const unsigned int gkuiCategoryCount = 13;
//Strings can be up to 30 chars
const unsigned int gkuiMaxStrLen = 30;
//but player names can only be 8, so the score table looks good
const unsigned int gkuiMaxPNameLen = 8;
//The default player name (8 spaces)
const char gkszDefaultPlayerName [] = "        ";
//only 4 players at a time, once again because of the score table
const unsigned int gkuiMaxPlayers = 4;

struct SCategory
{
	//The categories name
	char m_szName [gkuiMaxStrLen];
	//Is this category already sued?
	bool m_bIsUsed;
	//The max number of points
	unsigned int m_uiPotentialScore;
	//How many points the user got in this category
	unsigned int m_uiUserScore;
};

struct SPlayer
{
	//The players name
	char m_szName [gkuiMaxPNameLen + 1];
	//the scoring data
	SCategory m_sScores [gkuiCategoryCount];
	//The points
	unsigned int m_uiBonusPoints;
	unsigned int m_uiTotalPoints;
	//How many yahtzee's this player has gotten so far
	unsigned int m_uiYahtzeeCount;
};

#endif //MAIN_H