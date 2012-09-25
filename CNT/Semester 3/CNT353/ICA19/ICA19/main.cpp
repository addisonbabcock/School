#include "Full.h"
#include "Dot.h"
#include "Dash.h"
#include <iostream>

using namespace std;

void Tester (ostream & out, CLine & line)
{
	if (dynamic_cast <CFull *> (&line))
	{
		out << "I'm a CFull\n";
		return;
	}

	if (dynamic_cast <CDash *> (&line))
	{
		out << "I'm a CDash\n";
		return;
	}

	if (dynamic_cast <CDot *> (&line))
	{
		out << "I'm a CDot\n";
		return;
	}
}

int main ()
{
	{
		_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF); 

		CFull f(15,60);
		CDot d(16, 70);
		CDash e( 17, 79);

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
		{
			Tester( cout, *aLines[i] );// both args
			cout << *aLines[i];
		}
		cout << "End of Block Tester\n";
		cin.get();

		CLine * bLines[22] = {0};
		// "copy" all the lines from aLines to bLines...
		// Hint : as before, Clone() could be useful here..
		for (int i (0); i < 22; ++i)
			bLines [i] = aLines [i]->Clone ();

		// Prove it, output aLines, then bLines
		for (int i=0; i<22; i++)
			cout << *aLines[i];
		cout << "End of Block aLines\n";
		cin.get();
		for (int i=0; i<22; i++)
			cout << *bLines[i];
		cout << "End of Block bLines\n";
		cin.get();

		for (int i=0; i<22; i++)
		{
			delete aLines[i];
			delete bLines[i];
		}
	}
	return 0;
}