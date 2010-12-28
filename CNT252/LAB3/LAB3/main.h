/***************************************************\
* Project:		Lab 3 - BlackJack					*
* Files:		main.cpp, main.h, utilities.cpp		*
*				utilities.h							*
* Date:			10 Oct 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	A game of human vs computer 		*
*				Blackjack.							*
\***************************************************/

/*

TODO:

-get the whole betting thing working
-write ai
--maybe take the "while (bKeepDrawing)" part and put it in a function so the 
	ai can be written around it, since it should be mostly the same code and all
-make a nice ui
-split functions into seperate file according to whatever they do

*/

#ifndef MAIN_H
#define MAIN_H

#include <iostream>
#include <fstream>
#include "utilities.h"

using namespace std;

const unsigned int gkuiSuitStrLen = 9;
const unsigned int gkuiFaceStrLen = 6;
const unsigned int gkuiPlayerCount = 5;
//This needs to be updated everytime the save/load functions change
const char gkszFileHeader [13] = "Blackjack1.0";
const int gkiHeaderLen = 13;


//This is one card in the deck
struct SCard
{
	//The face value of the card (1, 2, 3... 10, 11)
	unsigned int m_uiFaceValue;
	//The suit of the card (spades, diamonds, etc)
	char m_szSuit [gkuiSuitStrLen];
	//The name of the card (ace, two...queen, king)
	char m_szFace [gkuiFaceStrLen];
	//Has the card been drawn off the deck yet?
	bool m_bIsUsed;
};

//This is one player
struct SPlayer
{
	//Is this player the computer dealer?
	//ie: should this player be controlled by the AI?
	bool m_bIsDealer;
	//The name of the character, 8 chars max
	char m_szName [9];
	//The players score on the current hand
	unsigned int m_uiScore;

	//NOTE: the next 2 variables are not used for the dealer
	//How much money this player has in the bank
	double m_dBank;
	//How much money this player bet on the current hand
	double m_dBet;
	//Is this player in the game?
	bool m_bIsActive;
};

bool InitializeDeck (SCard sCards [52]);
void InitializePlayers (SPlayer sPlayers [5]);
void ResetDeck (SCard sCards [52]);
void ShowCard (SCard* sCard);
SCard* DrawCard (SCard sCards [52]);
unsigned int CalculateScore (SCard* sHand [52], unsigned int uiHandSize);
unsigned int Turn (SCard sDeck [52], bool bIsDealer);
void GetBets (SPlayer sPlayers [5]);
void CalculateNewBanks (SPlayer sPlayers [5]);
void DisplayTitle (void);
bool LoadGame (SPlayer sPlayers [gkuiPlayerCount]);
void SaveGame (SPlayer sPlayers [gkuiPlayerCount]);

#endif //MAIN_H