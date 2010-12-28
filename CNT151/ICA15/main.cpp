#include <iostream>
#include <ctime>
#include <cmath>

using namespace std;

int const gkiRows = 22;
int const gkiCols = 79;
int const gkiSpaceshipRow = 10;
char const gkcSpaceship = static_cast<char> (175);

void FillIt (char pcaGrid [][gkiCols]);
void DisplayIt (char pcaGrid [][gkiCols]);
int GetRandInt (int iLowerBound, int iUpperBound);

int main ()
{
	char cGrid [gkiRows][gkiCols] = {0};
	char iUnderneathSpaceship = 0;

	srand (static_cast <unsigned int> (time (0)));

	FillIt (cGrid);

	for (int i = 0; i < gkiCols; i++)
	{
		iUnderneathSpaceship = cGrid [gkiSpaceshipRow][i];

		if ('*' == iUnderneathSpaceship)
		{
			cGrid [gkiSpaceshipRow - 1]	[i - 1]	= '\\';	//   \|/ 
			cGrid [gkiSpaceshipRow - 1]	[i]		= '|';  //   -@- 
			cGrid [gkiSpaceshipRow - 1]	[i + 1]	= '/';  //   /|\ 
			cGrid [gkiSpaceshipRow]		[i - 1]	= '-';  // KABOOM!
			cGrid [gkiSpaceshipRow]		[i]		= '@';
			cGrid [gkiSpaceshipRow]		[i + 1]	= '-';
			cGrid [gkiSpaceshipRow + 1]	[i - 1]	= '/';
			cGrid [gkiSpaceshipRow + 1]	[i]		= '|';
			cGrid [gkiSpaceshipRow + 1]	[i + 1]	= '\\';

			DisplayIt (cGrid);

			system ("pause");
			return 0;
		}
		cGrid [gkiSpaceshipRow][i] = gkcSpaceship;

		DisplayIt (cGrid);

		cGrid [gkiSpaceshipRow][i] = iUnderneathSpaceship;

		system ("cls");
	}

	system ("pause");
	return 0;
}

int GetRandInt (int iLowerBound, int iUpperBound)
{
	//check if the boundaries are invalid
	if (iLowerBound > iUpperBound)
	{
		//swap them if they are
		int temp = iUpperBound;
		iUpperBound = iLowerBound;
		iLowerBound = temp;
	}

	//send back the random number
	return rand() % (iUpperBound - iLowerBound) + iLowerBound;
}

void FillIt (char pcaGrid [][gkiCols])
{
	int iStarsPlaced = 0;
	int iNewStarX = 0;
	int iNewStarY = 0;

	do
	{
		iNewStarX = GetRandInt (0, gkiCols - 1);
		iNewStarY = GetRandInt (0, gkiRows - 1);

		if (static_cast <char> (0) == pcaGrid [iNewStarY][iNewStarX])
		{
			pcaGrid [iNewStarY][iNewStarX] = '*';
			iStarsPlaced++;
		}
		else
		{
			pcaGrid [iNewStarY][iNewStarX] = ' ';
		}
	} while (iStarsPlaced <= 80);
}

void DisplayIt (char pcaGrid [][gkiCols])
{
	int i = 0;
	int j = 0;

	for (j = 0; j < gkiRows; j++)
	{
		for (i = 0; i < gkiCols; i++)
		{
			cout << pcaGrid [j][i];
		}
		cout << endl;
	}
}