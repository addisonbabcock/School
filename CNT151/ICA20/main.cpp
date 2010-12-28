#include <iostream>
#include <string>
#include <fstream>

using namespace std;

int main (int argc, char* argv [])
{
	ifstream ifTheFile;
	char szString [65353] = {0};
	char* pcPartialString = 0;
	int iLineCount = 0;
	int iFoundCount = 0;

	for (int i = 0; i < argc; i++)
		cout << argv [i] << '\t';
	cout << endl << endl;

	if (3 != argc)
	{
		cout << "Invalid command line arguments. Syntax is as follows: \n"
			 << "ica20 file_to_open string_to_search_for\n\n";
		system ("pause");
		return 1;
	}
    
	ifTheFile.open (argv [1], ios::in);

	if (!ifTheFile)
	{
		cout << "Could not open file \"" << argv[1] << "\". Exiting program...\n\n";
		system ("pause");
		return 1;
	}

	system ("pause");
	system ("cls");
    while (!(ifTheFile.getline (szString, 65353)).eof ())
	{
		cout << ++iLineCount << ": " << szString << endl;

		//dont over fill the screen (25 lines - 1 for pause)
		if (iLineCount % 24 == 0)
		{
			system ("pause");
			system ("cls");
		}

		pcPartialString = szString;
        
		while (pcPartialString != NULL)
		{
			pcPartialString = strstr (pcPartialString, argv [2]);

			if (pcPartialString)
			{
				iFoundCount++;
                pcPartialString += strlen (argv [2]);
			}
		}
	}

	ifTheFile.close ();

	cout << "\nFound " << iFoundCount << " occurrences of \"" << argv [2] << "\"." << endl << endl;

	system ("pause");
	return 0;
}