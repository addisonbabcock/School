#include "Full.h"
#include "Dot.h"
#include "Dash.h"
#include <iostream>

using namespace std;

int main ()
{
	{
		_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF); 

		CFull f(10,60);
		CDot d(20, 70);
		CDash e( 30, 79);

		cout << f << d << e;
		cin.get();

		CLine * aLines[22] = {0};
		for (int i=0; i<22; i++)
		{
			switch( rand()%3 )
			{
			case 0: aLines[i] = new CFull( rand()%35, (rand()%35)+35 ); break;
			case 1: aLines[i] = new CDot( rand()%35, (rand()%35)+35 ); break;
			case 2: aLines[i] = new CDash( rand()%35, (rand()%35)+35 ); break;
			}
		}

		//Now display the shape's info, like a simple array...
		//NOTE: This would NOT work without polymorphism.
		for (int i=0; i<22; i++)
			cout << *aLines[i];
		cin.get();

		CLine * bLines[22] = {0};
		// How would you "copy" all the lines from aLines to bLines...
		// Hint : Clone() could be useful here..

		for (int i (0); i < 22; ++i)
			bLines [i] = aLines [i]->Clone ();

		// Prove it, output aLines, then bLines
		for (int i=0; i<22; i++)
			cout << *aLines[i];
		cin.get();
		for (int i=0; i<22; i++)
			cout << *bLines[i];
		cin.get();

		for (int i=0; i<22; i++)
		{
			delete aLines[i];
			delete bLines[i];
		}
	}
	return 0;
}