#include <iostream>
#include "3Dist.h"

using namespace std;

int main ()
{
	// ICA 16 Test Code - add to main, no other code required
	{// ica16 test code
		C3Dist const cA(5,-9, 50);
		C3Dist const cB(-2,7, -125);
		C3Dist C(0,0,0), D(0,0,0);
		cout << cA << endl;
		cout << cB << endl;
		cout << C << endl;
		D = cA + cB;
		cout << D << endl;
		cout << ( cA + cB ) << endl;
		D = cB;
		D += cA;
		cout << D << endl;
		C = ( C += cA ) + cA + cB;;
		cout << C << endl;
		for( int i(0); i < 1000; i++ )
			D += C3Dist(rand()%5-2, rand()%5-2, rand()%21-10); // What's this doing ?
		cout << D << endl; // Any guess ?
		cin.get();
	}
	return 0;
}