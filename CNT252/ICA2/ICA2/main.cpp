#include <iostream>
#include <iomanip>
#include <math.h>

using namespace std;

unsigned int GetInput ();

int main ()
{
	unsigned int uiMask = 0x80000000;
	unsigned int uiInput = 0;
	unsigned int uiRes = 0;

	uiInput = GetInput ();

	cout << "Your number in binary: ";

	for (int i = 32; i > 0; i--)
	{
		uiRes = uiInput & uiMask;
		if (uiRes)
			cout << "1";
		else
			cout << "0";

		uiMask = uiMask >> 1;
	}

	cout << endl;
	system ("pause");
	return 0;
}

unsigned int GetInput ()
{
	unsigned int uiInput = 0;
	bool bValid = false;
	while (!bValid)
	{
		cout << "Enter a 32 bit number in hexidecimal: ";
		cin >> hex >> uiInput;

		if (!(bValid = !cin.fail ()))
		{
			cout << "Incorrect input!" << endl;
			cin.clear ();
			cin.ignore (cin.rdbuf ()->in_avail ());
		}
	}

	return uiInput;
}