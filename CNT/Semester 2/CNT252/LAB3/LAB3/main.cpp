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

#include "main.h"

/***************************************************************************\
| Function: bool InitializeDeck (SCard sCards [52])							|
| Description: Loads up the deck of cards from a file.						|
| Parameters: sCards is the location of the deck of cards. Anything stored	|
|	in the array will be overwritten.										|
| Returns: A bool to indicate wether or not the file could be loaded.		|
\***************************************************************************/
bool InitializeDeck (SCard sCards [52])
{
	//the file containing the deck data
	ifstream fCards;
	//a temporary string to hold the file header
	char szHeader [12] = {0};

	//attempt to open the file
	fCards.open ("cards.txt", ios::in);
	if (!fCards)
	{
		cout << "Error: Could not load card data.\nTerminating program...\n";
		system ("pause");
		return false;
	}

	//make sure the file header is valid
	fCards.getline (szHeader, 12);
	if (strcmp (szHeader, ";cards data"))
	{
		cout << "Error: Attempted to load invalid file.\nTerminating program...\n";
		system ("pause");
		return false;
	}

	//load up the data
	for (int i = 0; i < 52 && !fCards.eof (); i++)
	{
		fCards >> sCards[i].m_szSuit;
		fCards >> sCards[i].m_szFace;
		fCards >> sCards[i].m_uiFaceValue;
	}

	ResetDeck (sCards);

	return true;
}

/***************************************************************************\
| Function: void InitializePlayers (SPlayer sPlayers [5])					|
| Description: Gets the player names and sets up their data. Sets			|
|	sPlayers [0] to be the dealer.											|
| Parameters: sPlayers is the array of player data.							|
| Returns: Nothing.															|
\***************************************************************************/
void InitializePlayers (SPlayer sPlayers [5])
{
	//just a loop counter
	unsigned int uiPlayerCount = 0;

	//set up the dealer
	sPlayers [0].m_bIsDealer = true;
	strcpy (sPlayers [0].m_szName, "dealer");

	//disable each player, they will be enabled later
	for (int i = 0; i < gkuiPlayerCount; i++)
		sPlayers [i].m_bIsActive = false;

	DisplayTitle ();
	cout << "Press return to stop creating players.\n";

	do
	{
		//Get the players name
		cout << "Enter your name, player " << ++uiPlayerCount << ": ";
		FlushCINBuffer ();
		cin.getline (sPlayers [uiPlayerCount].m_szName, 9);

		//Must have at least one player.
		if (1 == uiPlayerCount && 0 == sPlayers [1].m_szName[0])
		{
			cout << "Must have at least one player!\n";
			uiPlayerCount--;
			continue;
		}

		//If a player name was entered, initialize it
		if (sPlayers [uiPlayerCount].m_szName [0])
		{
			sPlayers [uiPlayerCount].m_bIsDealer = false;
			sPlayers [uiPlayerCount].m_dBank = 100.0;
			sPlayers [uiPlayerCount].m_dBet = 0.0;
			sPlayers [uiPlayerCount].m_uiScore = 0;
			sPlayers [uiPlayerCount].m_bIsActive = true;
		}
	//While a name was entered and there is still room for more players
	} while (sPlayers [uiPlayerCount].m_szName [0] && uiPlayerCount < 4);
}

/***************************************************************************\
| Function: void ResetDeck (SCard sCards [52])								|
| Description: "Shuffles" the deck. Actually, it just resets all the		|
|	m_bIsUsed flags.														|
| Parameters: sCards is the location of the deck of cards.					|
| Returns: None																|
\***************************************************************************/
void ResetDeck (SCard sCards [52])
{
	//Go through every card
	for (int i = 0; i < 52; i++)
		//and make it available again
		sCards [i].m_bIsUsed = false;
}

/***************************************************************************\
| Function: void ShowCard (SCard* sCard)									|
| Description: Displays a card onto the screen.								|
| Parameters: sCard is the card to be displayed.							|
| Returns: None.															|
\***************************************************************************/
void ShowCard (SCard* sCard)
{
	//output the card....
	cout << sCard->m_szFace << " of " << sCard->m_szSuit;
}

/***************************************************************************\
| Function: SCard* DrawCard (SCard sCards [52])								|
| Description: Picks a card out of the deck that hasn't been used and marks	|
|	it as used.																|
| Parameters: sCards is the deck of cards.									|
| Returns: A pointer to the card that was selected.							|
\***************************************************************************/
SCard* DrawCard (SCard sCards [52])
{
	//get a random card
	int i = GetRandInt (0, 51);

	//check too see if the card has already been used
	while (sCards [i].m_bIsUsed)
		//if it has, keep getting another one until an unused card is found
		i = GetRandInt (0, 51);

	//set the card as used
	sCards [i].m_bIsUsed = true;

	//and return a pointer to the card
	return sCards + i;
}

/***************************************************************************\
| Function: unsigned int CalculateScore (SCard* psHand [],					|
|										 unsigned int uiHandSize)			|
| Description: Calculates the best possible score from a given hand. Aces	|
|	are either 1 or 11, wichever is the closest to 21 without busting.		|
| Parameters: psHand is the array of SCard* that point to the cards in the	|
|	hand. uiHandSize is how many cards are in the hand.						|
| Returns: The best possible calculated score.								|
\***************************************************************************/
unsigned int CalculateScore (SCard* psHand [], unsigned int uiHandSize)
{
	//how many aces are in the hand?
	unsigned int uiAcesFound = 0;
	//the current score of the hand
	unsigned int uiScore = 0;

	//go through each card in the hand
	for (unsigned int i = 0; i < uiHandSize; i++)
	{
		//add the value of the card to the running total
		uiScore += static_cast <unsigned int> (psHand [i]->m_uiFaceValue);

		//if an ace is found, make a note of it
		if (11 == psHand [i]->m_uiFaceValue)
			uiAcesFound++;
	}

	//we want to make the score as big as possible without going over 21.
	//the way to do this is to keep taking 10 off the score while aces are 
	//still available and the score is still over 21
	while (uiScore > 21 && uiAcesFound > 0)
	{
		uiScore -= 10;
		uiAcesFound--;
	}

	return uiScore;
}

/***************************************************************************\
| Function: unsigned int Turn (SCard sDeck [52], bool bIsDealer)			|
| Description: The algorithm required to play a hand of Blackjack. Also		|
|	includes the AI.														|
| Parameters: sDeck is the deck of cards. bIsDealer is set to true if the	|
|	AI should be used, false if the user input should be used.				|
| Returns: The score that the user got on that turn.						|
\***************************************************************************/
unsigned int Turn (SCard sDeck [52], bool bIsDealer)
{

	//psPlayerHand is an array of pointers that are pointing to the cards in 
	//the players hand
	SCard* psPlayerHand [52];
	//uiPlayerHandSize is how many cards the player has in his hand
	//the player always starts with 2 cards, 
	//the first one will be drawn outside the loop
	unsigned int uiPlayerHandSize = 1;
	//uiPlayerScore is the players current score, max 21
	//set to 0 because no cards have been drawn yet
	unsigned int uiPlayerScore = 0;

	//bKeepDrawingCards is set to false when the user is done drawing cards
	//we want this to be true so the loop will draw the second card
	bool bKeepDrawingCards = true;

	//Display a nice title
	DisplayTitle ();

	//The player starts with 2 cards... this is the first
	if (!bIsDealer)
		cout << "Your card: ";
	else
		cout << "The dealer draws: ";
	ShowCard (psPlayerHand [0] = DrawCard (sDeck));

	//as long as the player wants to keep hitting
	while (bKeepDrawingCards)
	{
		//get another card
		psPlayerHand [uiPlayerHandSize] = DrawCard (sDeck);

		//show it to the user
		if (!bIsDealer)
			cout << "\nYour card: ";
		else
			cout << "\nThe dealer draws: ";
		ShowCard (psPlayerHand [uiPlayerHandSize++]);

		//recalculate the score and then show it
		uiPlayerScore = CalculateScore (psPlayerHand, uiPlayerHandSize);
		if (!bIsDealer)
			cout << "\nYour current score: " 
			<< uiPlayerScore;

		//dont let the user draw another card if the score is >= 21
		if (uiPlayerScore < 21)
		{
			//A.I.
			if (bIsDealer)
			{
				//Dealer stands at 17
				if (uiPlayerScore < 17)
					bKeepDrawingCards = true;
				else
					bKeepDrawingCards = false;
			}
			else
			{
				cout << "\nAnother card? ";
				bKeepDrawingCards = 'Y' == toupper (GetMenuChoice ("YNyn"));
			}
		}
		else
		{
			//Display a message based on the score...
			if (uiPlayerScore > 21)
				cout << "\nBUST!!\n";
			else
				cout << "\n21!!\n";

			//dont let the user keep getting more cards after a 21 or a bust
			bKeepDrawingCards = false;
		}
	}

	//return the acheived score, unless the player busted, then return 0
	if (uiPlayerScore > 21)
		return 0;
	else
		return uiPlayerScore;
}

/***************************************************************************\
| Function: void GetBets (SPlayer sPlayers [5])								|
| Description: This function gets the size of bet that each player wants to	|
|	place.																	|
| Parameters: sPlayers is the array of players that the betting data will	|
|	be stored into.															|
| Returns: Nothing															|
\***************************************************************************/
void GetBets (SPlayer sPlayers [5])
{
	DisplayTitle ();

	//Go through each player
	for (int i = 1; i < gkuiPlayerCount; i++)
	{
		//If that player is active, get his bet
		if (sPlayers [i].m_bIsActive)
		{
			//Get a bet
			cout << sPlayers [i].m_szName << ", you have $" 
				 << sPlayers [i].m_dBank << " in your bank.\n";
			sPlayers [i].m_dBet = GetDouble("How much would you like to bet? ",
											5.0, sPlayers [i].m_dBank);
		}
		else
		{
			sPlayers [i].m_dBet = 0;
		}
	}
}

/***************************************************************************\
| Function: void CalculateNewBanks (SPlayer sPlayers [5])					|
| Description: Not a very good function name, adjusts a players bank size	|
|	based on their bets and wether or not they won the hand.				|
| Parameters: sPlayers is the array of players that the betting data will	|
|	be stored into.															|
| Returns: Nothing															|
\***************************************************************************/
void CalculateNewBanks (SPlayer sPlayers [5])
{
	DisplayTitle ();

	//go through each player
	for (int i = 1; i < gkuiPlayerCount; i++)
	{
		//dont bother doing calculations if that player isnt active
		if (sPlayers [i].m_bIsActive)
		{
			//If the player got a higher score then the dealer...
			if (sPlayers [i].m_uiScore > sPlayers [0].m_uiScore)
			{
				//...add to his bank
				cout << sPlayers [i].m_szName << " gains $"
					<< sPlayers [i].m_dBet << " to bring his or her bank to $"
					<< (sPlayers [i].m_dBank += sPlayers [i].m_dBet) << ".\n";
			}

			//If the dealer got higher then the player...
			if (sPlayers [i].m_uiScore < sPlayers [0].m_uiScore)
			{
				//...take away some money
				cout << sPlayers [i].m_szName << " loses $"
					<< sPlayers [i].m_dBet << " to bring his or her bank to $"
					<< (sPlayers [i].m_dBank -= sPlayers [i].m_dBet) << ".\n";

				//$5 is the minimum bet. If a player has any less then that,
				//boot his ass out the door :)
				if (sPlayers [i].m_dBank < 5.0)
				{
					cout << sPlayers [i].m_szName << " is now bankrupt.\n"
						<< "Please exit the building.\n";
					sPlayers [i].m_bIsActive = false;
				}
			}
		}
	}
}

/***************************************************************************\
| Function: void DisplayTitle (void)										|
| Description: Displays a title across the top of the screen.				|
| Parameters: Nothing														|
| Returns: Nothing															|
\***************************************************************************/
void DisplayTitle (void)
{
	system ("cls");
	cout << "\n\t\t\tBlackjack\n\n";
}

/***********************************************************************\
| Function:	void SaveGame (SPlayer sPlayers [gkuiPlayerCount])			|
| Description: Opens a save game file (inputed by the user) and stores	|
|	sPlayers in a binary file.											|
| Parameters: sPlayers is the array containing all the data to be saved	|
| Returns: None.														|
\***********************************************************************/
void SaveGame (SPlayer sPlayers [gkuiPlayerCount])
{
	//File handle
	ofstream oFile;
	//File name
	char szFile [256];

	//Get the file name
	system ("cls");
	cout << "Enter the file: ";
	FlushCINBuffer ();
	cin.getline (szFile, 256);

	//attempt to open the file
	oFile.open (szFile, ios::binary | ios::out | ios::trunc);

	//While the file does not open correctly, nag the user for a new file
	//and attempt to reopen it.
	while (!oFile)
	{
		cout << "Could not open file for output, please enter a different one: ";
		FlushCINBuffer ();
		cin.getline (szFile, 256);

		oFile.clear ();
		oFile.close ();
		oFile.open (szFile, ios::binary | ios::out);
	}

	//write the file header
	oFile.write (gkszFileHeader, gkiHeaderLen);
	//write the actual data
	oFile.write (reinterpret_cast <char*> (sPlayers), sizeof (SPlayer) * gkuiPlayerCount);

	cout << "Game has now been saved at " << szFile << ". \n\n";
	oFile.close ();
}

/***********************************************************************\
| Function:	void LoadGame (SPlayer sPlayers [gkuiPlayerCount])			|
| Description: Opens a save game file (inputed by the user) checks for	|
|	validity, and loads it into sPlayers.								|
| Parameters: sPlayers is the location of the data will be loaded.		|
| Returns: Wether or not the game was loaded succesfully.				|
\***********************************************************************/
bool LoadGame (SPlayer sPlayers [gkuiPlayerCount])
{
	//file handle
	ifstream iFile;
	//file location
	char szFile [256];
	//The first gkiHeaderLen bytes read in from the file
	char szFileHeaderRead [gkiHeaderLen] = {0};
	//Is the file a valid save game?
	bool bFileValid = false;

	//Get the file name
	system ("cls");
	cout << "Enter the game to load: ";
	FlushCINBuffer ();
	cin.getline (szFile, 256);

	//If the string is null
	if (strlen (szFile) == 0)
		return false;
	
	//Open the file
	iFile.open (szFile, ios::binary | ios::in);

	//while the file is not valid
	while (!bFileValid)
	{
		//while the open didnt work
		while (!iFile)
		{
			//nag the user for a new file....
			cout << "Could not load the file for input: ";
			FlushCINBuffer ();
			cin.getline (szFile, 256);

			//...unless the file path is null
			if (strlen (szFile) == 0)
				return false;
			
			//...and try to open it
			iFile.clear ();
			iFile.close ();
			iFile.open (szFile, ios::binary | ios::in);
		}

		//file opening worked fine, read in the header
		iFile.read (szFileHeaderRead, gkiHeaderLen);

		//if the file is not from this program
		if (strcmp (gkszFileHeader, szFileHeaderRead))
		{
			//get a new file
			cout << "That is not a valid save game file: ";
			FlushCINBuffer ();
			cin.getline (szFile, 256);
		}
		else
		{
			//all checks passed, the file is valid
			bFileValid = true;
		}
	}//while (!bValidFile)

	//read in the values from the file
	iFile.read (reinterpret_cast <char*> (sPlayers), sizeof (SPlayer) * gkuiPlayerCount);
	iFile.close ();
	cout << "Game loaded. \n\n";
	return true;
}

int main ()
{
	//the deck of cards
	SCard sCards [52];
	//the array of players and the dealer
	SPlayer sPlayers [5];

	//dBank is how much money the player has left
	double dBank = 100.0;
	//dPot is how much money the player bet
	double dPot = 0;
	//bContinue is set to false when the program should exit
	bool bContinue = true;

	//init stuff, seed rand() and load up the deck
	SeedRandomGenerator ();
	if (!InitializeDeck (sCards))
		return 1;

	DisplayTitle ();
	cout << "Would you like to load a saved game? ";
	if ('Y' == toupper (GetMenuChoice ("YyNn")))
		if (!LoadGame (sPlayers))
			//Didn't want to load a saved game...
			//...Init the player data
			InitializePlayers (sPlayers);
		else;
	else
		//LoadGame () failed, init the players manually
		InitializePlayers (sPlayers);

	//as long as the player wants to keep playing
	while (bContinue)
	{
		//"Shuffle" the deck
		ResetDeck (sCards);

		//Get the players bets
		GetBets (sPlayers);

		//Go through each player and give him/her a turn
		for (int i = 1; i < gkuiPlayerCount; i++)
		{
			//but only if the player is active
			if (sPlayers [i].m_bIsActive)
			{
				sPlayers [i].m_uiScore = Turn (sCards, sPlayers [i].m_bIsDealer);
				cout << endl;
				system ("pause");
			}
		}

		//Give the computer its turn
		cout << "\nThe dealers score: " 
			<< (sPlayers [0].m_uiScore = Turn (sCards, true));
		cout << endl << endl;
		system ("pause");

		//Calculate the now bank sizes
		CalculateNewBanks (sPlayers);

		//keep playing?
		cout << "\n\nAnother hand? ";
		bContinue = 'Y' == toupper (GetMenuChoice ("YyNn"));

		//If the players want to quit
		if (!bContinue)
		{
			//Offer to save the game
			cout << "Would you like to save this game? ";
			if ('Y' == toupper (GetMenuChoice ("YyNn")))
				SaveGame (sPlayers);
		}
	}

	system ("pause");
	return 0;
}