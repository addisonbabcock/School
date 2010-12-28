#include "main.h"

void BubbleSort (char szArray [10][30], int iMax)
{
	char szTemp [30] = {0};

	for (int i = 0; i < iMax - 1; i++)
	{
		for (int j = 0; j < iMax - 1; j++)
		{
			if (strcmpi (szArray [j], szArray [j + 1]) > 0)
			{
				strcpy (szTemp, szArray [j]);
				strcpy (szArray [j], szArray [j + 1]);
				strcpy (szArray [j + 1], szTemp);
			}
		}
	}
}

void InsertionSort (char szArray [10][30], int iMax)
{
	char szTemp [30] = "";
	int i = 0;
	int j = 0;

	for (i = 1; i < iMax; i++)
	{
		j = i - 1;
		strcpy (szTemp, szArray [i]);

		while (j >= 0 && strcmpi (szTemp, szArray [j]) < 0)
		{
			strcpy (szArray [j + 1], szArray [j]);
			j--;
		}
		strcpy (szArray [j + 1], szTemp);
	}
}

int main ()
{
	char szStrings [10][30];
	int i = 0,
		j = 0;

	for (i = 0; i < 10; i++)
	{
		cout << "Enter string #" << setw (2) << i+1 << ": ";
		cin.clear ();
		cin.ignore (cin.rdbuf ()->in_avail ());
		cin.getline (szStrings [i], 30);

		if (0 == szStrings [i][0])
		{
			i++;
			break;
		}
	}

	cout << "\n\nSorting...\n\n";

	//BubbleSort (szStrings, i);
	InsertionSort (szStrings, i);

	for (j = 0; j < i; j++)
	{
		cout << szStrings [j] << endl;
	}

	cout << endl;
	system ("pause");
	return 0;
}