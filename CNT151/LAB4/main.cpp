/***************************************************\
* Project:		Lab 4 - Random Insulter				*
* Files:		main.cpp, main.h,					*
*				utilities.cpp, utilities.h			*
* Date:			03 Apr 2006							*
* Author:		Addison Babcock		Class:  CNT15	*
* Instructor:	J. D. Silver		Course: CNT151	*
* Description:	A program that will randomly insult	*
*	a random person									*
\***************************************************/

#include "main.h"

using namespace std;

/*******************************************************\
| Function: void LoadItems (char** szItems,				|
|							const* szFileName,			|
| Description: Loads a file into an array of strings	|
| Parameters: The storage area and the file to read		|
| Returns: How many strings were read in				|
\*******************************************************/
int LoadItems (char szItems [gkiMaxItems][gkiMaxItemLen], const char* const szFileName)
{
	//The file object to be used for reading
	ifstream inFile;
	//How many strings are read in
	int iStringCount = 0;

	//Open the file
	inFile.open (szFileName);

	//Quit on error
	if (!inFile)
	{
		inFile.close ();
		return 0;
	}

	//first, the file input is read into szItems, then end of file is checked for
	while (!(inFile.getline (szItems [iStringCount], gkiMaxItemLen)).eof () && 
	//array bound are also checked here
		   iStringCount < gkiMaxItems)
		   iStringCount++;
	//NOTE: semicolon at the end of while () is intentional

	//Close the file and return the number of string read in
    inFile.close ();
	return iStringCount+1;
}

int main ()
{
	//This array holds the names of people to be insulted
	char szNames [gkiMaxItems][gkiMaxItemLen] = {0};
	//This array holds the verbs that people do...
	char szVerbs [gkiMaxItems][gkiMaxItemLen] = {0};
	//...and what people do to something
	char szNouns [gkiMaxItems][gkiMaxItemLen] = {0};
	//The final insult will be stored here
	char szInsult [gkiMaxItemLen * 3] = {0};
	//Stores the number of each string read in, and which one 
	//is to be used in the insult
	int iNameCount = 0, iNameToUse = 0;
	int iVerbCount = 0, iVerbToUse = 0;
	int iNounCount = 0, iNounToUse = 0;
	//The object for file output
	fstream OutFile;

	SeedRandomGenerator ();

	//Display a title screen
	cout << "\t\t\tRandom Insult Generator!\n\n";
	cout << right << "Prepare to be insulted!\n\n\n";

	//Load up the files
	iNameCount = LoadItems (szNames, "names.txt");
	iVerbCount = LoadItems (szVerbs, "actions.txt");
	iNounCount = LoadItems (szNouns, "objects.txt");

	//Check to make sure the files were loaded properly
	if (!iNameCount)
	{
		//Does not compute!
		cout << "Could not find file \"names.txt\", please make sure it exists\n"
			 << "and is not empty.\n\n";
		system ("pause");
		return 1;
	}
	//If they were, display how many names were found
	cout << iNameCount << " names have been found.\n";

	//Check to make sure the files were loaded properly
	if (!iVerbCount)
	{
		//Does not compute!
		cout << "Could not find file \"actions.txt\", please make sure it exists\n"
			 << "and is not empty.\n\n";
		system ("pause");
		return 1;
	}
	//If they were, display how many verbs were found
	cout << iVerbCount << " verbs have been found.\n";

	//Check to make sure the files were loaded properly
	if (!iNounCount)
	{
		//Does not compute!
		cout << "Could not find file \"objects.txt\", please make sure it exists\n"
			 << "and is not empty.\n\n";
		system ("pause");
		return 1;
	}
	//If they were, display how many nouns were found
	cout << iNounCount << " objects have been found.\n\n";

	//randomly pick a string out of each array...
	iNameToUse = GetRandInt (0, iNameCount - 1);
	iVerbToUse = GetRandInt (0, iVerbCount - 1);
	iNounToUse = GetRandInt (0, iNounCount - 1);

	//...and slap them together
	strcat (szInsult, szNames [iNameToUse]);
	strcat (szInsult, " ");
	strcat (szInsult, szVerbs [iVerbToUse]);
	strcat (szInsult, " ");
	strcat (szInsult, szNouns [iNounToUse]);

#ifdef _DEBUG
	//If the program is built in debug mode, display which strings were used
	cout << iNameToUse << '\t' << szNames [iNameToUse] << endl;
	cout << iVerbToUse << '\t' << szVerbs [iVerbToUse] << endl;
	cout << iNounToUse << '\t' << szNouns [iNounToUse] << endl;
	cout << szNouns [iNounToUse] << endl;
#endif

	//Display the final insult
	cout << szInsult << endl << endl;

	//And save it to a file
	OutFile.open ("results.txt", ios::out | ios::app);
	//But only if the file is open
	if (OutFile.is_open ())
		OutFile << szInsult << endl;
	else
		cout << "Warning, output could not be saved to a file.\n"
			 << "The file \"results.txt\" may be in use.\n\n";
	OutFile.close ();
    
	return 0;
}