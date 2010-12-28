#include <iostream>
#include <crtdbg.h>
#include "Meter.h"

using namespace std;

int main ()
{
	{
		_CrtSetDbgFlag (_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
		CMeter const myMeter (9, 1, 10);
		myMeter.Display (cout);
		cin.get ();
		CMeter mA (50);
		for (int i = 0; i <= 60; ++i )
		{
			if( !(i%5) )
				mA.Display(cout);
			mA.Step();
		}
		cin.get();
		mA.Reset();
		mA.Display( cout );
		CMeter mB( 3, 3, 8 ); // goofy range test
		for( int i = 0; i < 7; ++i )
		{
			mB.Display(cout);
			mB.Step();
		}
		cin.get();
		CMeter mC( 0, 0, 99 ); // incremental test
		for( int i = 0; i < 100; ++i )
		{
			mC.Display(cout);
			mC.Step();
			for( int j = 0; j < 900000; ++j )
				int i = rand() * rand() * rand();
		}
		cin.get();
	}// end ica02
}