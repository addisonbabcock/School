#include <iostream>
#include <fstream>

using namespace std;

int main (int argc, char** argv)
{
	unsigned int const kuiMaxStrLen = 256;
	unsigned int uiLineCount = 0;
	unsigned int uiStringCount = 0;
	char szLine [kuiMaxStrLen] = {0};
	char* pcPartString = 0;
	fstream fIn;
	
	//Error checking, make sure the user entered the correct number of command 
	//line arguments
	if (!(argc == 3 || argc == 5))
	{
		cout << "I BLOW UP NOW!" << endl;
		cout << "Correct format is:\nLAB1 source-file string-to-search-for\n";
		system ("pause");
		return 1;
	}

	//Open the file and make sure it opened properly
	fIn.open (argv [1], ios::in);
	if (!fIn)
	{
		cout << "\"" << argv [1] << "\" could not be opened for input. \n";
		system ("pause");
		return 1;
	}

	while (!(fIn.getline (szLine, kuiMaxStrLen)).eof ())
	{
		pcPartString = szLine;
		uiStringCount = 0;

		while (pcPartString != NULL)
		{
			pcPartString = strstr (pcPartString, argv [2]);

			if (pcPartString)
			{
				uiStringCount++;
				pcPartString += strlen (argv [2]);
			}
		}

		cout << "Found " << uiStringCount << " occurance(s) of \"" << argv [2] 
			 << "\" on line " << ++uiLineCount << endl;
	}

	system ("pause");
	return 0;
}