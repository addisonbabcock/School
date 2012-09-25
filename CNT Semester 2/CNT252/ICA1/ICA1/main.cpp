#include <iostream>
#include <fstream>

using namespace std;

int main (int argc, char* argv [])
{
	ofstream ofDest;
	ifstream ifSrc;
	char cTemp = 0;
	int iCharsCopied = 0;

	if (argc != 3)
	{
		cout << "Incorrect number of parameters on command line!" << endl;
		return 1;
	}

	cout << "Destination file: " << argv [1] << endl;
	cout << "Source file: " << argv [2] << endl;

	ofDest.open (argv [1], ios::out);
	ifSrc.open  (argv [2], ios::in);

	if (!ofDest || !ifSrc)
	{
		cout << "Could not open one of the files." << endl;
		return 1;
	}

	while (!ifSrc.get (cTemp).eof ())
	{
		ofDest << cTemp;
		iCharsCopied++;
	}

	ofDest.close ();
	ifSrc.close ();

	cout << iCharsCopied << " characters have been copied." << endl;

	system ("pause");
	return 0;
}