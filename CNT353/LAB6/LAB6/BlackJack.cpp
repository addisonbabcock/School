/****************************************************
Project: Lab 06 - Back In Blackjack
Files: BlackJack.h, BlackJack.cpp
Date: 17 April 2007
Author: Addison Babcock		Class: CNT2K
Instructor: Herb V.			Course: CNT353
****************************************************/

#pragma warning (disable : 4244)

#include <sstream>
#include "BlackJack.h"

// Function name   : Put 
// Description     : Displays a card on the console
// Return type     : void 
// Argument        : int iCard - The value of the card to be displayed

void Put (int iCard)
{
	//2-10 just output the value
	if (iCard > gkiAce && iCard < gkiJack)
	{
		cout << iCard << gkcSpace;
	}
	else
	{
		switch (iCard)
		{
		case gkiAce: //Ace
			cout << gkszAce;
			break;
		case gkiJack: //Jack
			cout << gkszJack;
			break;
		case gkiQueen: //Queen
			cout << gkszQueen;
			break;
		case gkiKing: //King
			cout << gkszKing;
			break;
		default: //Bad card
			stringstream str;
			str << gkszPutBadCard << iCard;
			throw str.str ();
		}
	}
}

// Function name   : IsPlayerWin 
// Description     : A helper function for count_if
// Return type     : bool - true if the player won, false otherwise 
// Argument        : char cResult - A character representing the winner

bool IsPlayerWin (char cResult)
{
	return gkcPlayer == cResult;
}

// Function name   : IsDealerWin 
// Description     : A helper function for count_if
// Return type     : bool - true if the dealer won, false otherwise
// Argument        : char cResult - A character representing the winner

bool IsDealerWin (char cResult)
{
	return gkcDealer == cResult;
}

// Function name   : CBlackJack::DealCard 
// Description     : Deals a card to a given hand
// Return type     : void 
// Argument        : list <int> & hand - The hand that needs a card

void CBlackJack::DealCard (list <int> & hand)
{
	hand.push_back (_deck.front ());
	_deck.pop_front ();
}

// Function name   : CBlackJack::CBlackJack 
// Description     : CTOR for the CBlackJack class

CBlackJack::CBlackJack (void) 
: _iDealerHitMax (gkiMaxHitDefault), _iPlayerHitMax (gkiMaxHitDefault)
{}

// Function name   : CBlackJack::PopulateDeck
// Description     : Fills the shoe with iDeckCount number of decks
// Return type     : void
// Argument        : int iDeckCount - How many decks to place in the shoe

void CBlackJack::PopulateDeck (int iDeckCount)
{
	int iCurDeck (0),	//The current cards being placed into the deck
		iCurCard (0),	//The current card being placed into the deck
		iCurColour (0);	//The colour of the card being placed on the deck

	//empty the deck then populate it
	_deck.clear ();
	//for each deck requested
	for (iCurDeck = 0; iCurDeck < iDeckCount; ++iCurDeck)
	{
		//for each face value
		for (iCurCard = 0; iCurCard < gkiKing; ++iCurCard)
		{
			//for each colour
			for (iCurColour = 0; iCurColour < gkiColours; ++iCurColour)
			{
				//add the card
				_deck.push_back (iCurCard+1);
			}
		}
	}
}

// Function name   : CBlackJack::Shuffle 
// Description     : Shuffles the deck of cards
// Return type     : void 

void CBlackJack::Shuffle (void)
{
	random_shuffle (_deck.begin (), _deck.end ());
}

// Function name   : CBlackJack::Sum 
// Description     : Finds the best possible total for a given hand
// Return type     : int - The best possible score
// Argument        : list <int> const & hand - The hand to be scored

int CBlackJack::Sum (list <int> const & hand) const
{
	int iAces (0);	//how many aces were found
	int	iSum (0);	//the sum of the cards

	//go through the hand
	for (list<int>::const_iterator i (hand.begin ());
		i != hand.end (); ++i)
	{
		//check for an ace
		if (*i == gkiAce)
		{
			iSum += gkiAceDefPts;	//ace found, add the max points to the 
			++iAces;				//total and remember that an ace was found
			continue;
		}

		//check for 2-10
		if (*i > gkiAce && *i < gkiJack)
		{
			iSum += *i;		//plain number card found, add the face value
			continue;
		}

		//check for face card
		if (*i >= gkiJack)
		{
			iSum += gkiFaceCardPts;	//Add a face card to the total
		}
	}

	//while the total is over gkiBlackJack and there are still aces left,
	//try to keep this hand from being a bust
	while (iSum > gkiBlackJack && iAces)
	{
		iSum -= gkiAceDefPts - gkiAceMinPts;
		--iAces;
	}

	return iSum;
}

// Function name   : CBlackJack::SetPlayerMax 
// Description     : Sets the aggressiveness of the player
// Return type     : void 
// Argument        : int iNewPlayerHitMax - How long the player should keep
//					 hitting 

void CBlackJack::SetPlayerMax (int iNewPlayerHitMax)
{
	_iPlayerHitMax = iNewPlayerHitMax;
}

// Function name   : CBlackJack::SetDealerMax 
// Description     : Sets the aggressiveness of the dealer
// Return type     : void 
// Argument        : int iNewDealerHitMax - How long the dealer should keep
//					 hitting

void CBlackJack::SetDealerMax (int iNewDealerHitMax)
{
	_iDealerHitMax = iNewDealerHitMax;
}

// Function name   : CBlackJack::Display 
// Description     : Shows the result of a game on the console and determines
//					 winner.
// Return type     : bool - true if the player won, false if the dealer won

bool CBlackJack::Display () const
{
	int iPlayerScore (0),	//The score of the players hand
		iDealerScore (0);	//The score of the dealers hand
	bool bPlayerWin (false),//Did the player win?
		bDealerWin (true); //Did the dealer win?

	//show the players hand and score
	cout << gkszPlayersHand;
	for_each (_playerHand.begin (), _playerHand.end (), Put);
	cout << gkszTotal << (iPlayerScore = Sum (_playerHand)) << endl;

	//show the dealers hand and score
	cout << gkszDealersHand;
	for_each (_dealerHand.begin (), _dealerHand.end (), Put);
	cout << gkszTotal << (iDealerScore = Sum (_dealerHand)) << endl;

	//check for the player busting
	if (iPlayerScore > gkiBlackJack)
		bPlayerWin = !(bDealerWin = true);
	else
	{
		//check for the dealer busting
		if (iDealerScore > gkiBlackJack)
			bDealerWin = !(bPlayerWin = true);
		else
			//no busts, compare the hands
			if (iPlayerScore > iDealerScore)
				bDealerWin = !(bPlayerWin = true);
	}

	//the player wins if he/she gets gkiBlackJack or has a bigger score then the dealer
	//and didnt bust
	if (bPlayerWin)
	{
		//output the hand winner and a line of 60 dashes
		cout << gkszPlayer << gksz60Dashes << endl;
		return true;
	}
	else
	{
		//output the hand winner and a line of 60 dashes
		cout << gkszDealer << gksz60Dashes << endl;
		return false;
	}
}

// Function name   : CBlackJack::PlayHands 
// Description     : Plays blackjack until the shoe is empty, keeping track
//					 of stats and displaying scores
// Return type     : void 

void CBlackJack::PlayHands ()
{
	int iCard (0);	//A card freshly pulled off the deck
	bool bPlayerWin (false), //Did the player win?
		bDealerWin (false); //Did the dealer win?
	int iPlayerScore (0); //The players score
	int iDealerScore (0); //The dealers score

	//there must be 8 cards in the deck for a hand to be played
	while (_deck.size () > 8)
	{
		//nobody has won this hand yet
		bPlayerWin = bDealerWin = false;
		iPlayerScore = iDealerScore = 0;

		//clear the hands
		_playerHand.clear ();
		_dealerHand.clear ();

		//give some default hands
		DealCard (_playerHand);
		DealCard (_dealerHand);
		DealCard (_playerHand);

		//score the hands
		iPlayerScore = Sum (_playerHand);
		iDealerScore = Sum (_dealerHand);

		//while the player still wants cards
		while (iPlayerScore <= _iPlayerHitMax)
		{
			//give the player a card
			DealCard (_playerHand);

			//rescore the hand
			iPlayerScore = Sum (_playerHand);
		}

		//if the player gets gkiBlackJack or over, he autowin/loses and the dealer passes
		if (iPlayerScore < gkiBlackJack)
		{
			while (iDealerScore <= _iDealerHitMax && //The dealer hasnt maxed
				iDealerScore < iPlayerScore && //The dealer hasnt won
				iDealerScore < gkiBlackJack) //The dealer hasnt busted
			{
				//give the dealer a card
				DealCard (_dealerHand);

				//rescore the hand
				iDealerScore = Sum (_dealerHand);
			}
		}

		//show the winner and add the win to the record
		_winRecords.push_back (Display () ? gkcPlayer : gkcDealer);

		//now add both hands to the frequency count
		_handTotalsCount[iPlayerScore] += 1;
		_handTotalsCount[iDealerScore] += 1;
	}
}

// Function name   : CBlackJack::Stats
// Description     : Displays accumulated statistics on the console
// Return type     : void 

void CBlackJack::Stats (void) const
{
	// go through the win records
	vector<char>::const_iterator iter = _winRecords.begin ();
	for ( ; iter != _winRecords.end () ; ++iter)
	{
		//output an X for player wins
		if (IsPlayerWin (*iter))
			cout << gkcWin;
		else
			cout << gkcLoss;
	}
	cout << endl;
	//go through the win records again
	for (iter = _winRecords.begin (); iter != _winRecords.end (); ++iter)
	{
		//output an X for dealer wins
		if (IsDealerWin (*iter))
			cout << gkcWin;
		else
			cout << gkcLoss;
	}

	//output the win totals
	cout << gkszPlayerWins 
		<< count_if (_winRecords.begin (), _winRecords.end (), IsPlayerWin)
		<< gkszDealerWins
		<< count_if (_winRecords.begin (), _winRecords.end (), IsDealerWin)
		<< endl;

	//output the frequency each count was obtained
	for (map<int,int>::const_iterator iter(_handTotalsCount.begin ()); iter != _handTotalsCount.end (); ++iter)
	{
		cout << iter->first << " : " << iter->second << endl;
	}
}