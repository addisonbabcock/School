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

/***************************************************\
*	NOTE:											*
*	If you change the SaveGame/LoadGame functions	*
*	be sure to update gkszFileHeader accordingly	*
\***************************************************/

#include "main.h"
#include "utilities.h"
#include "io.h"
#include "logic.h"

/***********************************************************\
| Function: void DisplayWelcome ()							|
| Description: Pretty much self-descriptive. Clears the		|
|	screen and displays a welcome message.					|
| Parameters: None.											|
| Returns: None.											|
\***********************************************************/
void DisplayWelcome ()
{
	system ("CLS");
	cout << "\t\t\tWelcome to Doom Yahtzee 1.0\n\n";
	cout << "Admit it, this game is far beyond our time. In order to truly \n"
		<< "comprehend its awesomeness, you will be repeatedly tortured by\n"
		<< "endless clones. MWAHAHAHAHA\n\n\n";
}

/*******************************************************************\
| Function: void GetRerolls (char szRerolls [gkuiDiceCount + 1])	|
| Description: An input validation function. Gets 5 'y' or 'n' or	|
|	any combination of the two from the keyboard and store them in	|
|	szRerolls.														|
| Parameters: szRerolls is the location to store the results.		|
| Returns: None.													|
\*******************************************************************/
void GetRerolls (char szRerolls [gkuiDiceCount + 1])
{
	//Is the input valid?
	bool bInputValid = false;

	//while invalid input, try again
	while (!bInputValid)
	{
		//Get input
		cout << "Enter which dice you would like to reroll\n"
			<< "I'm expecting a series of 5 y or n where y means reroll: ";
		FlushCINBuffer ();
		cin.getline (szRerolls, gkuiDiceCount + 1);

		//if not 5 chars
		if (strlen (szRerolls) != gkuiDiceCount)
		{
			//nag!
			cout << "Input must be exactly 5 characters.\n";
			bInputValid = false;
		}
		else
		{
			//assume its good until proven otherwise
			bInputValid = true;

			//check each character...
			for (int i = 0; i < gkuiDiceCount; i++)
			{
				//to make sure its a y or n
				if (!(toupper (szRerolls [i]) == 'Y' ||
					toupper (szRerolls [i]) == 'N'))
				{
					//if not try again
					cout <<  "\'" << szRerolls [i] << "\' is not a valid character.\n";
					bInputValid = false;
					break;
				}
			}
		}//if (strlen....)
	}//While (!bInputValid)
}//GetRerolls ()

/***********************************************************************\
| Function:																|
|	void DisplayScoreMenu (SCategory sScores [gkuiCategoryCount],		|
|	   char szValidInput [gkuiMaxStrLen])								|
| Description: Displays the category menu. Also, builds a string to be	|
|	used in GetMenuChoice ()											|
| Parameters: sScores is the array of structs for each category.		|
|	szValidInput is the string that will contain the valid menu options	|
| Returns: None.														|
\***********************************************************************/
void DisplayScoreMenu (SCategory const sScores [gkuiCategoryCount], 
					   char szValidInput [gkuiMaxStrLen])
{
	//Contains the character for the current menu option
	char cCurrentOption = 0;
	int iStrPos = 0;

	//Empty the array before filling it
	for (int i = 0; i < gkuiMaxStrLen; i++)
		szValidInput [i] = 0;

	//for each scoring category
	for (int i = 0; i < gkuiCategoryCount; i++)
	{
		//if the category is not used
		if (!sScores [i].m_bIsUsed)
		{
			//starts at 65 == 'A'
			cCurrentOption = 65 + i;

			//Display the category
			cout << cCurrentOption << ". " << sScores [i].m_szName << endl;

			//The option displayed is now a valid selection
			//upper case
			szValidInput [iStrPos * 2] = cCurrentOption;
			//lower case
			szValidInput [iStrPos * 2 + 1] = cCurrentOption + 32;
			//ie: if a, b, and c are valid, the resulting string would be:
			//"AaBbCc"

			iStrPos++;
		}
	}
}

/***********************************************************************\
| Function:																|
|	void DisplayScores (SPlayer sPlayers [gkuiMaxPlayers])				|
| Description: Displays the score table. Warning #4309 is disabled		|
| Parameters: sPlayers is the array that contains all the data to be	|
|	displayed.															|
| Returns: None.														|
\***********************************************************************/
void DisplayScores (SPlayer const sPlayers [gkuiMaxPlayers])
{

	//Disable warning #4309: 'initializing' : truncation of constant value
	//occurs on declaration of static const char's, changing them to unsigned
	//will cause setfill() to whine
#pragma warning(disable : 4309)

	//Border characters
	//I used the double line borders, but they can be changed here
	//The comments below may not display correctly because VS likes
	//to garble them. Push Alt+# to see the real ones. 
	static const char cTopLeft		= 201; //+
	static const char cOutHoriz		= 205; //-
	static const char cTopRight		= 187; //+
	static const char cOutVert		= 186; //¦
	static const char cBottomRight	= 188; //+
	static const char cInHoriz		= 196; //-
	static const char cBottomLeft	= 200; //+
	static const char cInVert		= 179; //¦
	static const char cLHBreakTop	= 204; //¦
	static const char cRHBreakTop	= 185; //¦
	static const char cLHBreakBot	= 199; //¦
	static const char cRHBreakBot	= 182; //¦
	static const char cTopBreakLeft = 203; //-
	static const char cTopBreakRight= 209; //-
	static const char cMidBreakLeft	= 215; //+
	static const char cMidBreakRight= 197; //+
	static const char cBotBreakLeft	= 202; //-
	static const char cBotBreakRight= 207; //-
	//Offset from LHS, adjust so it looks good (approx. centered)
	static const char szOffset [] = "       ";
	//Title of the table
	static const char szTitle [gkuiMaxStrLen] = " At the end of round ";
	//Column sizes
	static const int iColumnSize1 = 20;
	static const int iColumnSize2 = 10;
	static const int iTableW = 66;
	//The current round number
	int iRoundNumber = GetRoundCount (sPlayers);

	//blank page
	system ("cls");

	/* WARNING: heavy i/o manipulators follow */
	/*	 the weak of heart have been warned   */

	//Top bar
	cout << szOffset << cTopLeft;
	for (int i = 0; i < iTableW - 2; i++)
		cout << cOutHoriz;
	cout << cTopRight << endl;

	//Title row
	cout << szOffset << cOutVert << left
		<< szTitle << left << setw (iTableW - strlen (szTitle) - 2) << iRoundNumber
		<< cOutVert << endl;

	//Seperator
	cout << szOffset << cLHBreakTop;
	for (int i = 0; i < iColumnSize1; i++)
		cout << cOutHoriz;
	cout << cTopBreakLeft;
	for (int i = 0; i < gkuiMaxPlayers; i++)
	{
		for (int j = 0; j < iColumnSize2; j++)
			cout << cOutHoriz;
		if (i < gkuiMaxPlayers - 1)
			cout << cTopBreakRight;
	}
	cout << cRHBreakTop << endl;

	//Column titles
	cout << szOffset << cOutVert << left << setw (iColumnSize1)
		<< " Category" << cOutVert;
	for (int i = 0; i < gkuiMaxPlayers; i++)
	{
		cout << ' ' << left << setw (iColumnSize2 - 1) 
			<< sPlayers [i].m_szName;
		if (i < gkuiMaxPlayers - 1)
			cout << cInVert;
		else
			cout << cOutVert;
	}
	cout << endl;

	//Seperator
	cout << szOffset << cLHBreakBot;
	for (int i = 0; i < iColumnSize1; i++)
		cout << cInHoriz;
	cout << cMidBreakLeft;
	for (int i = 0; i < gkuiMaxPlayers; i++)
	{
		for (int j = 0; j < iColumnSize2; j++)
			cout << cInHoriz;
		if (i < gkuiMaxPlayers - 1)
			cout << cMidBreakRight;
	}
	cout << cRHBreakBot << endl;

	//Ones to sixes
	for (int i = 0; i < 6; i++)
	{
		cout << szOffset << cOutVert << ' ' << left << setw (iColumnSize1 - 1) 
			<< sPlayers [0].m_sScores [i].m_szName << cOutVert;

		for (int j = 0; j < gkuiMaxPlayers; j++)
		{
			cout << ' ';
			//dont output a 0 when the category hasnt been used
			//ouput a space instead
			if (sPlayers [j].m_sScores [i].m_bIsUsed)
				cout << setw (iColumnSize2 - 1) 
				<< sPlayers [j].m_sScores [i].m_uiUserScore;
			else
				cout << setw (iColumnSize2 - 1) << ' ';

			if (j < gkuiMaxPlayers - 1)
				cout << cInVert;
			else
				cout << cOutVert;
		}

		cout << endl;
	}

	//Seperator
	cout << szOffset << cLHBreakBot;
	for (int i = 0; i < iColumnSize1; i++)
		cout << cInHoriz;
	cout << cMidBreakLeft;
	for (int i = 0; i < gkuiMaxPlayers; i++)
	{
		for (int j = 0; j < iColumnSize2; j++)
			cout << cInHoriz;
		if (i < gkuiMaxPlayers - 1)
			cout << cMidBreakRight;
	}
	cout << cRHBreakBot << endl;

	//Bonus points
	cout << szOffset << cOutVert << left << setw (iColumnSize1) 
		<< " Bonus points" << cOutVert;
	for (int i = 0; i < gkuiMaxPlayers; i++)
	{
		cout << ' ' << setw (iColumnSize2 - 1) << sPlayers [i].m_uiBonusPoints;
		if (i < gkuiMaxPlayers - 1)
			cout << cInVert;
		else
			cout << cOutVert;
	}
	cout << endl;

	//Seperator
	cout << szOffset << cLHBreakBot;
	for (int i = 0; i < iColumnSize1; i++)
		cout << cInHoriz;
	cout << cMidBreakLeft;
	for (int i = 0; i < gkuiMaxPlayers; i++)
	{
		for (int j = 0; j < iColumnSize2; j++)
			cout << cInHoriz;
		if (i < gkuiMaxPlayers - 1)
			cout << cMidBreakRight;
	}
	cout << cRHBreakBot << endl;

	//High categories
	for (int i = 6; i < gkuiCategoryCount; i++)
	{
		cout << szOffset << cOutVert << ' ' << left << setw (iColumnSize1 - 1) 
			<< sPlayers [0].m_sScores [i].m_szName << cOutVert;

		for (int j = 0; j < gkuiMaxPlayers; j++)
		{
			cout << ' ';
			//dont output a 0 when the category hasnt been used
			//ouput a space instead, 
			//always display yahtzee since m_bIsUsed has a different meaning
			if (sPlayers [j].m_sScores [i].m_bIsUsed || 11 == i)
				cout << setw (iColumnSize2 - 1) 
				<< sPlayers [j].m_sScores [i].m_uiUserScore;
			else
				cout << setw (iColumnSize2 - 1) << ' ';

			if (j < gkuiMaxPlayers - 1)
				cout << cInVert;
			else
				cout << cOutVert;
		}
		cout << endl;
	}

	//Seperator
	cout << szOffset << cLHBreakBot;
	for (int i = 0; i < iColumnSize1; i++)
		cout << cInHoriz;
	cout << cMidBreakLeft;
	for (int i = 0; i < gkuiMaxPlayers; i++)
	{
		for (int j = 0; j < iColumnSize2; j++)
			cout << cInHoriz;
		if (i < gkuiMaxPlayers - 1)
			cout << cMidBreakRight;
	}
	cout << cRHBreakBot << endl;

	//Total points
	cout << szOffset << cOutVert << left << setw (iColumnSize1) 
		<< " Total points" << cOutVert;
	for (int i = 0; i < gkuiMaxPlayers; i++)
	{
		cout << ' '	<< setw (iColumnSize2 - 1) << sPlayers [i].m_uiTotalPoints;
		if (i < gkuiMaxPlayers - 1)
			cout << cInVert;
		else
			cout << cOutVert;
	}
	cout << endl;

	//Bottom border
	cout << szOffset << cBottomLeft;
	for (int i = 0; i < iColumnSize1; i++)
		cout << cOutHoriz;
	cout << cBotBreakLeft;
	for (int i = 0; i < gkuiMaxPlayers; i++)
	{
		for (int j = 0; j < iColumnSize2; j++)
			cout << cOutHoriz;
		if (i < gkuiMaxPlayers - 1)
			cout << cBotBreakRight;
	}
	cout << cBottomRight << endl;
}

/***********************************************************************\
| Function:	void DisplayDice (unsigned int const uiDice [gkuiDiceCount])|
| Description: Draws an array of dice onto the screen.					|
| Parameters: uiDice is the array of dice to be displayed.				|
| Returns: None.														|
\***********************************************************************/
void DisplayDice (unsigned int const uiDice [gkuiDiceCount])
{
	//Below is the enormous 3D array to store the dice faces.
	//First subscript is the dice face value, second is the
	//row number, third is the column.
	//The blank face used as a template is kept in case it 
	//is needed for whatever reason.
	static const char szDiceData [7][7][13] = 
	{{"/----------\\",
	"|          |",
	"|          |",
	"|          |",
	"|          |",
	"|          |",
	"\\----------/"},

	{"/----------\\",
	"|          |",
	"|          |",
	"|    **    |",
	"|          |",
	"|          |",
	"\\----------/"},

	{"/----------\\",
	"| **       |",
	"|          |",
	"|          |",
	"|          |",
	"|       ** |",
	"\\----------/"},

	{"/----------\\",
	"| **       |",
	"|          |",
	"|    **    |",
	"|          |",
	"|       ** |",
	"\\----------/"},

	{"/----------\\",
	"| **    ** |",
	"|          |",
	"|          |",
	"|          |",
	"| **    ** |",
	"\\----------/"},

	{"/----------\\",
	"| **    ** |",
	"|          |",
	"|    **    |",
	"|          |",
	"| **    ** |",
	"\\----------/"},

	{"/----------\\",
	"| **    ** |",
	"|          |",
	"| **    ** |",
	"|          |",
	"| **    ** |",
	"\\----------/"}};

	//The dice have to be displayed one row at a time because of
	//cout's lack of a setpos (x,y) function

	//Loop through each row
	for (int i = 0; i < 7; i++)
	{
		//Loop through each die
		for (int j = 0; j < gkuiDiceCount; j++)
			//Output one row of one die at a time, seperated by ' '
			cout << szDiceData [uiDice [j]][i] << ' ';
		//One row of dice complete, next row
		cout << endl;
	}
}

/***********************************************************************\
| Function:	void SaveGame (SPlayer sPlayers [gkuiMaxPlayers],			|
|						   unsigned int* puiPlayerCount)				|
| Description: Opens a save game file (inputed by the user) and stores	|
|	sPlayers and *puiPlayerCount into the file.							|
| Parameters: sPlayers is the array containing all the data to be saved	|
|	puiPlayerCount is how many players are playing.						|
| Returns: None.														|
\***********************************************************************/
void SaveGame (SPlayer sPlayers [gkuiMaxPlayers], 
			   unsigned int* const puiPlayerCount)
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
	oFile.write (reinterpret_cast <char*> (puiPlayerCount), sizeof (unsigned int));
	oFile.write (reinterpret_cast <char*> (sPlayers), sizeof (SPlayer) * gkuiMaxPlayers);

	cout << "Game has now been saved at " << szFile << ". \n\n";
	oFile.close ();
}

/***********************************************************************\
| Function:	void LoadGame (SPlayer sPlayers [gkuiMaxPlayers],			|
|						   unsigned int* puiPlayerCount)				|
| Description: Opens a save game file (inputed by the user) checks for	|
|	validity, and loads it into sPlayers. Also sets *puiPlayerCount		|
to the number of players that were saved.							|
| Parameters: sPlayers is the location of the data will be loaded.		|
|	puiPlayerCount is how many players are playing.						|
| Returns: None.														|
\***********************************************************************/
void LoadGame (SPlayer sPlayers [gkuiMaxPlayers], unsigned int* const puiPlayerCount)
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
	{
		//Load the default values and exit
		Initialize (sPlayers, puiPlayerCount);
		return;
	}

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
			{
				Initialize (sPlayers, puiPlayerCount);
				return;
			}

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
	iFile.read (reinterpret_cast <char*> (puiPlayerCount), sizeof (unsigned int));
	iFile.read (reinterpret_cast <char*> (sPlayers), sizeof (SPlayer) * gkuiMaxPlayers);
	iFile.close ();
	cout << "Game loaded. \n\n";
}

/***********************************************************************\
| Function:	void HighScores (const unsigned int uiPlayersScore)			|
| Description: Does all the handling of the highscores. Takes the users	|
|	score as an unsigned int and checks it against highscores.bin to	|
|	determine the current high score.									|
| Parameters: uiPlayersScore is the score that the winning player got.	|
| Returns: None.														|
\***********************************************************************/
void HighScores (const unsigned int uiPlayersScore)
{
	//The file variable
	fstream File;
	//The score that was read from the file
	unsigned int uiHighScore = 0;

	File.open ("highscores.bin", ios::in | ios::binary);

	//File not found!
	if (!File)
	{
		File.close ();
		cout << "Error, could not open high scores file.\n"
			<< "Which means you win by default!\n";
		uiHighScore = 0;
		system ("pause");
	}
	else
	{
		//Get the high score
		File.read (reinterpret_cast<char*>(&uiHighScore), sizeof (unsigned int));
		File.close ();
	}

	//Which is higher?
	if (uiPlayersScore > uiHighScore)
	{
		//The players score is highest
		uiHighScore = uiPlayersScore;
		cout << "Congratulations, you have achieved the new high score!\n";

		//Open the file
		File.open ("highscores.bin", ios::out | ios::binary | ios::trunc);

		//File is in use or something
		if (!File.is_open())
		{
			cout << "Could not open highscores.bin for output.\n";
			system ("pause");
			return;
		}

		//Save the score
		File.write (reinterpret_cast<char*>(&uiHighScore), sizeof (unsigned int));
		File.close ();

		cout << "Your high score has been saved.\n";
	}
	else
	{
		cout << "Sorry, your score is lower then " << uiHighScore << ", please try again.\n";
	}

	system ("pause");
}