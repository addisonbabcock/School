#include "GetInput.h"

void GetInput (int* const piValue, char const * const szPrompt,
			   int const iLow, int const iHigh)
{
	cout << szPrompt;
	Flush ();
	cin >> *piValue;

	while (*piValue > iHigh || *piValue < iLow || cin.fail ())
	{
		cout << szPrompt;
		Flush ();
		cin >> *piValue;
	}
}

void GetInput (double* const pdValue, char const * const szPrompt,
			   double const dLow, double const dHigh)
{
	cout << szPrompt;
	Flush ();
	cin >> *pdValue;

	while (*pdValue > dHigh || *pdValue < dLow || cin.fail ())
	{
		cout << szPrompt;
		Flush ();
		cin >> *pdValue;
	}
}

void Flush ()
{
	cin.clear ();
	cin.ignore (cin.rdbuf ()->in_avail ());
}