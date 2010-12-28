#include <iostream>
#include "Clock.h"

int main ()
{
	{
		CClock const myClock;
		myClock.Display (cout);
		CClock myOther (7, 568, 485);
		myOther.Display (cout);
		for (int i = 0; i < 12212332; i++)
			myOther.Tick ();
		myOther.Display (cout);
		cin.get ();
	}
	cout << endl;
	{
		CClock const * const pMyClock = new CClock;
		pMyClock->Display (cout);
		CClock * const pOtherClock = new CClock (7, 568, 485);
		pOtherClock->Display (cout);
		for (int i = 0; i < 12212332; i++)
			pOtherClock->Tick ();
		pOtherClock->Display (cout);
		cin.get ();
	}

	return 0;
}