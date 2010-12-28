/****************************************************
Project: Lab 06 - Back In Blackjack
Files: BlackJack.h, BlackJack.cpp
Date: 17 April 2007
Author: Addison Babcock		Class: CNT2K
Instructor: Herb V.			Course: CNT353
****************************************************/

#pragma once
#include <deque>
#include <list>
#include <vector>
#include <map>
#include <algorithm>
#include <iostream>

using namespace std;

//Global Constants
int const gkiMaxHitDefault (17); //The default hit value for both players
int const gkiBlackJack (21); //The win value for a game of black jack
int const gkiColours (4); //How many different suites are in a deck
int const gkiAce (1); //Value that represents an ace
int const gkiJack (11);	//Value that represents a Jack
int const gkiQueen (12); //Value that represents a Queen
int const gkiKing (13); //Value that represents a King
int const gkiAceDefPts (11); //Aces are worth this much by default
int const gkiAceMinPts (1); //Aces are worth a minimum of this much
int const gkiFaceCardPts (10); //face cards are worth this much
char const gkcSpace (' '); //Used to seperate cards in Put ()
char const gkcPlayer ('P'); //Used to represent the player
char const gkcDealer ('D'); //Used to represent the dealer
char const gkcWin ('X');	//Used to show a win in CBlackJack::Stats ()
char const gkcLoss (' ');	//Used to show a lose in CBlackJack::Stats ()
char const gksz60Dashes [] = //A line of 60 dashes
	"------------------------------------------------------------";
char const gkszAce	[] = "A "; //String to display for an ace
char const gkszJack [] = "J "; //String to display for a jack
char const gkszQueen[] = "Q "; //String to display for a queen
char const gkszKing	[] = "K "; //String to display for a king
char const gkszPlayerWins [] = "\nPlayer Wins: "; //Show prior to the players
	//win count
char const gkszDealerWins [] = "\nDealer Wins: "; //Show prior to the dealers
	//win count
char const gkszPlayer [] = "Player\n"; //Shown to indicate the winner...
char const gkszDealer [] = "Dealer\n"; //...of a given hand
char const gkszTotal [] = ": Total : "; //Show prior to a players score
char const gkszPlayersHand [] = "Players hand : "; //Show before the players...
char const gkszDealersHand[]="Dealers hand : ";//..or the dealers hand is shown

//Error messages
char const gkszPutBadCard [] = "Put (int) : Bad card value : ";
	//Put tried to display a bad card

void Put (int iCard);
bool IsPlayerWin (char cResult);
bool IsDealerWin (char cResult);

class CBlackJack
{
protected:
	deque<int> _deck; //The deck of cards
	list <int> _playerHand; //The players hand
	list <int> _dealerHand; //The dealers hand
	vector<char> _winRecords; //Keep track of the results of each game
	map <int, int> _handTotalsCount; //the frequency of hand totals
	int _iPlayerHitMax; //the player will hit until this total
	int _iDealerHitMax; //the dealer will hit until this total

	void DealCard (list <int> & hand);

public:
	CBlackJack (void);
	void PopulateDeck (int iDeckCount);
	void Shuffle (void);
	int Sum (list <int> const & hand) const; //should be static?
	void SetPlayerMax (int iNewPlayerHitMax);
	void SetDealerMax (int iNewDealerHitMax);
	bool Display (void) const;
	void PlayHands (void);
	void Stats (void) const;
};