#include <iostream>
#include <string>

using namespace std;

int GetInt (int, int);
void FlushcinBuffer (void);

unsigned int const kuiMaxStringLen = 1000;

int GetInt (int iLowerBound, int iUpperBound)
{
	int iRetVal;

	if (iLowerBound > iUpperBound)
	{
		int iTemp = iUpperBound;
		iUpperBound = iLowerBound;
		iLowerBound = iTemp;
	}

	cin.clear ();
	cin.ignore (cin.rdbuf()->in_avail(), '\n');
	cin >> iRetVal;
	
	while (cin.fail() || iRetVal > iUpperBound || iRetVal < iLowerBound)
	{
		cout << "Error, must be a value between " << iLowerBound << " and " << iUpperBound << ": ";

		cin.clear ();
		cin.ignore (cin.rdbuf()->in_avail(), '\n');
		cin >> iRetVal;
	}

	return iRetVal;
}

void FlushcinBuffer (void)
{
	cin.clear ();
	cin.ignore (cin.rdbuf ()->in_avail (), '\n');
}

int main ()
{
	int i = 0;
	unsigned int uiChannel = 0;
	char szStringToEncode [kuiMaxStringLen] = "";

	cout << "Enter the encoding channel: ";
	uiChannel = GetInt (1, 10);

	do {
		i = 0;
		cout << endl << "Enter string: ";
		FlushcinBuffer ();
		cin.getline (szStringToEncode, kuiMaxStringLen);

		while (i < kuiMaxStringLen && szStringToEncode[i] != '\0')
		{
			szStringToEncode [i] += uiChannel;
			i++;
		}

		cout << szStringToEncode << endl;

	} while (strlen (szStringToEncode) > 0);

	system ("pause");
	return 0;
}