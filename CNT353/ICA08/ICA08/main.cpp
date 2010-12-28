#include "Dist.h"

int main ()
{// ICA 08 Test Code - add to main
	{// ica08 test code
		CDist const cA(5,-9);
		CDist const cB(-2,7);
		CDist C(0,0), D(0,0);
		cA.Display( cout ) << endl;
		cB.Display( cout ) << endl;
		C.Display( cout ) << endl;
		D = Add( cA, cB );
		D.Display( cout ) << endl;
		Add( cA, cB ).Display( cout ) << endl;
		D = cB;
		Sum( D, cA ).Display( cout ) << endl;
		D.Display( cout ) << endl;
		D = C;
		for( int i(0); i < 1000; i++ )
			Sum( D, CDist(rand()%5-2, rand()%5-2)); // What's this doing ?
		D.Display( cout ) << endl; // Any guess ?
		cin.get();
	}
	return 0;
}