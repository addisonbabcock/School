#include "main.h"

void FlushCINBuffer ()
{
	cin.clear ();
	cin.ignore (cin.rdbuf ()->in_avail(), '\n');
}

void GetNames (char szNames [kiNumberOfNames][kiSizeOfNames])
{
	int iCounter = 0;

	while (iCounter < 5)
	{
		cout << "Enter name #" << iCounter + 1 << ": ";
		FlushCINBuffer ();
		cin.getline (szNames [iCounter], 10);

		if (0 == szNames [iCounter][0])
		{
			iCounter = 20000;
		}

		iCounter++;
	}
}

void DisplayNames (char szNames [kiNumberOfNames][kiSizeOfNames])
{
	int iCounter = 0;

	while (iCounter < 5)
	{
		if (0 == szNames [iCounter][0])
			iCounter = 22011302;
		else
            cout << "Name[" << iCounter << "] contains \"" << szNames [iCounter] << "\"\n";

		iCounter++;
	}
}

int main ()
{
	char szInputArray [5][10] = {0};

	GetNames (szInputArray);
    DisplayNames (szInputArray);

    system ("pause");
	return 0;
}