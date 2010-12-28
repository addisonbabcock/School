/***************************************************\
* Project:		Lab 1 - Command Line Parameters		*
* Files:		main.cpp							*
* Date:			18 Sept 2006						*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	A search and replace program that	*
*				will load a file into memory and	*
*				replace all occurences of a string	*
*				found inside the file.				*
\***************************************************/

#include <iostream>
#include <fstream>

using namespace std;

/*******************************************************************\
| Function: void Search (char szQuery [256], char szFile [256])		|
| Description: Searches for a string inside a given file.			|
| Parameters: szQuery is the string to search for. szFile is the 	|
|	file to look in.												|
| Returns: None.													|
\*******************************************************************/
void Search (char szQuery [256], char szFile [256]);

/*******************************************************************\
| Function: void SearchReplace (char szSearchFor [256],				|
|					char szReplaceWith [256], char szInFile [256],	|
|					char szOutFile [256])							|
| Description: Searches for a string inside a given file and		|
|	replaces it with another string, then saves the results to		|
|	another file.													|
| Parameters: szQuery is the string to search for. szFile is the 	|
|	file to look in. szReplaceWith is the string to be inserted.	|
|	szOutFile is the output file.									|
| Returns: None.													|
\*******************************************************************/
void SearchReplace (char szSearchFor [256], char szReplaceWith [256], 
					char szInFile [256], char szOutFile [256]);

int main (int argc, char** argv)
{
	switch (argc)
	{
	case 3:
		//3 arguments mean only search for a string
		Search (argv [2], argv [1]);
		break;

	case 5:
		//5 arguments mean search and replace a string and save the output
		SearchReplace (argv [3], argv [4], argv [1], argv [2]);
		break;

	default:
		cout << "Improper command line usage \n\n";
		cout << "Please replace keyboard/chair interface\n\n";
		system ("pause");
		return 1;
	}
	
	system ("pause");
	return 0;
}

/*******************************************************************\
| Function: void Search (char szQuery [256], char szFile [256])		|
| Description: Searches for a string inside a given file.			|
| Parameters: szQuery is the string to search for. szFile is the 	|
|	file to look in.												|
| Returns: None.													|
\*******************************************************************/
void Search (char szQuery [256], char szFile [256])
{
	fstream fIn;
	char szFileContents [256][256] = {0};
	char* pcPartStr = 0;
	int iLinesRead = 0;
	int iQueryFound = 0;

	//open the file and make sure it is open
	fIn.open (szFile, ios::in);
	if (!fIn)
	{
		cout << "Could not open \"" << szFile << "\" for input.\n";
		system ("pause");
		return;
	}

	//read the file into the buffer and the close it
	while (!(fIn.getline (szFileContents [iLinesRead], 256)).eof ())
		iLinesRead++;
	fIn.close ();

	//for each line in the file, 
	//look for all the possible occurances of szQuery
	for (int i = 0; i < iLinesRead; i++)
	{
		//set the pointer to the start of the current line
		pcPartStr = szFileContents [i];
		//havent found any szQuery in this line yet
		iQueryFound = 0;
		
		//while matches are still being found
		while (pcPartStr = strstr (pcPartStr, szQuery))
		{
			iQueryFound++;
			//move a bit further down the string
			pcPartStr++;
		}

		//say how many matches we found
		cout << "Found " << iQueryFound << " occurance(s) of \"" << szQuery 
			 << "\" on line " << i+1 << endl;
	}

	//Display the contents of the file onto the screen
	cout << endl << "Contents of the file \"" << szFile 
		 << "\" are..." << endl << endl;
	for (int i = 0; i < iLinesRead; i++)
		cout << szFileContents [i] << endl;
	cout << endl;
}

/*******************************************************************\
| Function: void SearchReplace (char szSearchFor [256],				|
|					char szReplaceWith [256], char szInFile [256],	|
|					char szOutFile [256])							|
| Description: Searches for a string inside a given file and		|
|	replaces it with another string, then saves the results to		|
|	another file.													|
| Parameters: szQuery is the string to search for. szFile is the 	|
|	file to look in. szReplaceWith is the string to be inserted.	|
|	szOutFile is the output file.									|
| Returns: None.													|
\*******************************************************************/
void SearchReplace (char szSearchFor [256], char szReplaceWith [256], 
					char szInFile [256], char szOutFile [256])
{
	fstream fIn;
	fstream fOut;
	int iLinesRead = 0;
	char* pcPartStr = 0;
	int iQueryFound = 0;
	char szOriginalFile [256][256] = {0};
	char szFileContents [256][256] = {0};
	char szTemp [256] = {0};

	//open the file and make sure it is open
	fIn.open (szInFile, ios::in);
	if (!fIn)
	{
		cout << "Could not open \"" << szInFile << "\" for input.\n";
		system ("pause");
		return;
	}

	//read the file into the buffer and the close it
	while (!(fIn.getline (szFileContents [iLinesRead], 256)).eof ())
	{
		//save the original file for future output
		strcpy (szOriginalFile [iLinesRead], szFileContents [iLinesRead]);
		//measure the length of the file in lines
		iLinesRead++;
	}
	fIn.close ();

	//for each line in the file, 
	//look for all the possible occurances of szQuery
	for (int i = 0; i < iLinesRead; i++)
	{
		//set the pointer to the beginning of the current line
		pcPartStr = szFileContents [i];
		//no matches found on this line yet
		iQueryFound = 0;

		//while matches are still being found
		while (pcPartStr = strstr (pcPartStr, szSearchFor))
		{
			//copy everything after the match into a temp string
			strcpy (szTemp, pcPartStr + strlen (szSearchFor));
			//then erase everything after the start of the match
			pcPartStr [0] = 0;
			//put the new substring onto the end of the old string
			strcat (szFileContents [i], szReplaceWith);
			//and replace the end of the string
			strcat (szFileContents [i], szTemp);

			//a match was found
			iQueryFound++;
			//move a bit further down the string
			pcPartStr++;
		}

		//display how many matches where found
		cout << "Found " << iQueryFound << " occurance(s) of \"" 
			 << szSearchFor << "\" on line " << i+1 << endl;
	}

	//Display the contents of the file onto the screen
	cout << endl << "Contents of the file \"" << szInFile << "\" are..." 
		 << endl << endl;
	for (int i = 0; i < iLinesRead; i++)
		cout << szOriginalFile [i] << endl;

	//try to open the file for output
	fOut.open (szOutFile, ios::out);
	if (!fOut)
	{
		cout << "The file \"" << szOutFile 
			 << "\" could not be opened for output.\n\n";
		//file could not be opened, do not try to write to it
		return;
	}

	//Display the contents of the file onto the screen
	cout << endl << "Contents of the file \"" << szOutFile << "\" are..." 
		 << endl << endl;
	for (int i = 0; i < iLinesRead; i++)
	{
		cout << szFileContents [i] << endl;
		fOut << szFileContents [i] << endl;
	}
	cout << endl;
}