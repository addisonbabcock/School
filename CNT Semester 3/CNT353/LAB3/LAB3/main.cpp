#include "Germ.h"
#include "Dish.h"
#include <iostream>
#include <crtdbg.h>
#include <ctime>

using namespace std;
//#include "e:\lab03_test.h"

void CopyTest (CDish dish)
{
	cout << "Copy test...\n";
	dish.Show ();
	cin.get ();
	CDish dish2;
	dish2 = dish;
	dish2.Show ();
	cin.get ();
}

void Lab3Test ()
{

	int iAlive;

	srand (static_cast <unsigned int> (time (static_cast<time_t> (0))));

	CGerm::Clear();
	CDish dish1 (iAlive = 0);
	//CopyTest (dish1);

	dish1.Show ();
	dish1.SetCell (0, 0, 10).SetCell (0, 1, 1);
	dish1.SetCell (0, 2, 0);
	dish1.Show ();
	//cout << dish1.GetAlive () << " " << dish1.GetDay () << endl;
	cin.get ();

	CDish dish (iAlive = 2000);

	while (iAlive)
	{
		dish.Show ();
		iAlive = dish.Day ();
		cin.get ();
	}
	dish.Show ();
	cout << "\nTHEY ALL DIED!!! WHYYYYY????\n";
	cin.get ();
	/*
	CDish dish(2000);
	dish.Show();
	cin.get();
	while( dish.Day() ) // while Day returns any alive germs
	{
	dish.Show();
	cin.get();
	}
	dish.Show();
	cin.get();
	// This block loops until the dish dies off
	// If your code is correct, it starts full then
	// Day 164 = 12 left alive
	// Day 165 = 4 left alive
	// Day 166 = 0 left alive - done
	*/
}

int main ()
{
	_CrtSetDbgFlag (_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
	CGerm::Clear();
	Lab3Test ();
	return 0;
}