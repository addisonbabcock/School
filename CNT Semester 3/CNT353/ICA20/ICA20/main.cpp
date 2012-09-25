#pragma warning (disable : 4996)
#pragma warning (disable : 4244)

#include <iostream>
#include <iomanip>
#include <algorithm>

using namespace std;

void output (double * pArr, int iElems)
{
	for (int i(0); i < iElems; ++i)
	{
		cout << fixed << setprecision (4) << pArr [i] << " ";
		if (!((i + 1) % 10))
			cout << endl;
	}
	cin.get ();
}

double gen ()
{
	static double dVal (-0.01);
	return dVal += 0.01;
}

double random ()
{
	return static_cast <double> (rand ()) / RAND_MAX;
}

bool GreaterThenHalf (double dVal)
{
	return dVal > 0.5;
}

double quadruple (double dVal)
{
	return dVal * 4.0;
}

char ToggleCase (char cIn)
{
	if (cIn == toupper (cIn))
	{
		return tolower (cIn);
	}
	else
	{
		return toupper (cIn);
	}
}

bool IsVowel (char cIn)
{
	switch (cIn)
	{
	case 'a':
	case 'A':
	case 'e':
	case 'E':
	case 'i':
	case 'I':
	case 'o':
	case 'O':
	case 'u':
	case 'U':
	case 'y':	//???
	case 'Y':
		return true;
	}
	return false;
}

int main ()
{
	//4
	cout << "Part 4.\n";
	double dA [100];
	double dB [100];
	double dC [100];

	fill (dA, dA + 100, 0.5);
	generate (dB, dB + 100, gen);
	generate (dC, dC + 100, random);

	//5
	cout << "Part 5.\n";
	cout << "dA\n";
	output (dA, 100);
	cout << "dB\n";
	output (dB, 100);
	cout << "dC\n";
	output (dC, 100);

	//6	
	cout << "Part 6.\n";
	cout << "Min of dC : " << *min_element (dC, dC + 100) << endl
		 << "Max of dC : " << *max_element (dC, dC + 100) << endl;

	//7
	cout << "Part 7.\n";
	cout << "Number of values greater then 0.5 : " << static_cast <int> (count_if (dC, dC + 100, GreaterThenHalf)) << endl;

	//8
	cout << "Part 8.\n";
	double dIn (0);
	double * dFound (0);
	for (int i(0); i < 2; ++i)
	{
		cout << "Enter a double : ";
		cin >> dIn;
		dFound = find (dB, dB + 100, dIn);
		if (dFound  != dB + 100)
		{
			cout << "Value found\n";
			*dFound += 1.0;
		}
		else
		{
			cout << "Not found\n";
		}
	}

	//9
	cout << "Part 9.\n";
	random_shuffle (dB, dB + 100);
	output (dB, 100);

	//10
	cout << "Part 10.\n";
	cout << endl;
	transform (dB, dB + 100, dB, quadruple);
	output (dB, 100);

	//11
	cout << "Part 11.\n";
	sort (dC, dC + 100);
	output (dC, 100);

	//12
	cout << "Part 12.\n";
	copy (dC, dC + 100, dA);
	output (dA, 100);

	//13
	cout << "Part 13.\n";
	fill (dC, dC + 100, 0.0);

	//14
	cout << "Part 14.\n";
	sort (dA, dA + 100);
	sort (dB, dB + 100);
	set_intersection (dA, dA + 100, dB, dB + 100, dC);
	output (dC, 100);

	//15
	char sz1 [80] = {0};
	char sz2 [80] = {0};
	cout << "Enter a string: ";
	cin.get (sz1, 80);

	copy (sz1, sz1 + 80, sz2);
	transform (sz2, sz2 + 80, sz2, toupper);
	cout << sz2 << endl;
	cin.get ();

	copy (sz1, sz1 + 80, sz2);
	transform (sz2, sz2 + 80, sz2, ToggleCase);
	cout << sz2 << endl;
	cin.get ();

	copy (sz1, sz1 + 80, sz2);
	cout << "String 2 contains " << count_if (sz2, sz2 + 80, IsVowel) << " vowels." << endl;
	cin.get ();

	sort (sz2, sz2 + strlen (sz2));
	cout << sz2 << endl;

	copy (sz1, sz1 + 80, sz2);
	random_shuffle (sz2, sz2 + strlen (sz2));
	cout << sz2 << endl;

	cin.get ();
	return 0;
}