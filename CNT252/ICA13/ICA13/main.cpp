#include <iostream>

using namespace std;

void SelectionSort (char szArray [50])
{
	int iMinIndex = 0;
	int iSortedIndex = 0;
	int iCur = 0;
	const int iMaxIndex = strlen (szArray);
	char cTemp = 0;

	for (iSortedIndex = 0; iSortedIndex < iMaxIndex - 1; iSortedIndex++)
	{
		iMinIndex = iSortedIndex;

		for (iCur = iSortedIndex + 1; iCur < iMaxIndex; iCur++)
		{
			if (toupper (szArray [iCur]) > toupper (szArray [iMinIndex]))
			{
				iMinIndex = iCur;
			}
		}
		cTemp = szArray [iMinIndex];
		szArray [iMinIndex] = szArray [iSortedIndex];
		szArray [iSortedIndex] = cTemp;
	}
}

void SelSort (char* szArray)
{
	char cMinVal = 0;
	int iMinIndex = 0;
	char cTemp = 0;
	int iCur = 0;
	int iScan = 0;
	int iMax = strlen (szArray);

	for (iCur = 0; iCur < iMax - 1; iCur++)
	{
		cMinVal = szArray [iCur];
		iMinIndex = iCur;

		for (iScan = iCur + 1; iScan < iMax; iScan++)
		{
			if (toupper (szArray [iScan]) > toupper (cMinVal))
			{
				cMinVal = szArray [iScan];
				iMinIndex = iScan;
			}
		}
		cTemp = szArray [iMinIndex];
		szArray [iMinIndex] = szArray [iCur];
		szArray [iCur] = cTemp;
	}
}

int main ()
{
	char szStr [50] = {0};
	char szStr2 [50] = {0};

	cout << "Enter a string: ";
	cin.getline (szStr, 50);
	strcpy (szStr2, szStr);

	SelSort (szStr);
	cout << szStr << endl;

	SelectionSort (szStr2);
	cout << szStr2 << endl;

	system ("pause");
	return 0;
}