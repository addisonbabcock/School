#include "Utilities.h"

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

double GetDouble (double iLowerBound, double iUpperBound)
{
	double dRetVal;

	if (iLowerBound > iUpperBound)
	{
		double iTemp = iUpperBound;
		iUpperBound = iLowerBound;
		iLowerBound = iTemp;
	}

	cin.clear ();
	cin.ignore (cin.rdbuf()->in_avail(), '\n');
	cin >> dRetVal;
	
	while (cin.fail() || dRetVal > iUpperBound || dRetVal < iLowerBound)
	{
		cout << "Error, must be a value between " << iLowerBound << " and " << iUpperBound << ": ";

		cin.clear ();
		cin.ignore (cin.rdbuf()->in_avail(), '\n');
		cin >> dRetVal;
	}

	return dRetVal;
}

template <typename Type>
Type GetValue (Type LowerBound, Type UpperBound)
{
	Type RetVal;

	if (LowerBound > UpperBound)
	{
		Type Temp = UpperBound;
		UpperBound = LowerBound;
		LowerBound = Temp;
	}

	cin.clear ();
	cin.ignore (cin.rdbuf()->in_avail(), '\n');
	cin >> RetVal;

	while (cin.fail () || RetVal > UpperBound || RetVal < LowerBound)
	{
		cout << "Error, must be a value between " << LowerBound << " and " << UpperBound << ": ";

		cin.clear();
		cin.ignore (cin.rdbuf ()->in_avail (), '\n');
		cin >> RetVal;
	}

	return RetVal;
}