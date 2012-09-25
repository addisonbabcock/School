#include <iostream>
#include <fstream>
#include <string>

using namespace std;

void FlushCINBuffer ()
{
	cin.clear ();
	cin.ignore (cin.rdbuf()->in_avail(), '\n');
}

int main ()
{
	ofstream outFile;
	ifstream inFile;
	char szFileName [256] = {0};
	int iToBeStored = 0;
	int iTotal = 0;
	int iNumbersInFile = 0;
	bool bKeepEnteringValues = true;

	cout << "Enter the file name to store the values: ";
	FlushCINBuffer ();
	cin.getline (szFileName, 256);

	outFile.open (szFileName, ios::out | ios::trunc);

	while (!outFile)
	{
		outFile.clear ();

		cout << "Could not open file, try a different file...\n";
		cout << "Enter the file to create: ";
		FlushCINBuffer ();
		cin.getline (szFileName, 256);

		outFile.open (szFileName, ios::out | ios::trunc);
	}

	do
	{
		cout << "Enter a number, or -1 to exit: ";
		FlushCINBuffer ();
		cin >> iToBeStored;

		if (-1 == iToBeStored)
		{
			bKeepEnteringValues = false;
		}
		else
		{
			outFile << iToBeStored << endl;
		}
	} while (bKeepEnteringValues);

	outFile.close ();

	cout << "Enter the file name to read: ";
	FlushCINBuffer ();
	cin.getline (szFileName, 256);

	inFile.open (szFileName, ios::in);

	while (!inFile)
	{
		inFile.clear ();

		cout << "File not found. PLease enter again: ";
		FlushCINBuffer ();
		cin.getline (szFileName, 256);

		inFile.open (szFileName, ios::in);
	}

	while (!(inFile >> iToBeStored).eof ())
	{
		cout << iToBeStored << endl;

		iTotal += iToBeStored;
		iNumbersInFile++;
	}

	if (iNumbersInFile != 0)
	{
		cout << "The average of the numbers is " << iTotal / iNumbersInFile << endl << endl;
	}
	else
	{
		cout << "The file is empty, average could not be calculated!\n\n";
	}

	inFile.close ();
	system ("pause");
	return 0;
}