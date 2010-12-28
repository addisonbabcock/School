#include <crtdbg.h>
#include <iostream>
#include <string>
#include "CStack.h"

using namespace std;

int main ()
{// ICA  14 Test Code - New
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF); 
	{ // ICA14 test code 
		CStack myOne, myTwo;
		myOne << 1.1 << 2.2 << 3.3 << 4.4;
		CStack const myThree( myOne );
		myTwo = myTwo = myThree;
		cout << "myOne:  " << myOne;
		cout << "myTwo:  " << myTwo;
		cout << "myThree:" << myThree;

		double dVal(0);
		for ( int i = 0; i <= 8; i++ )
		{ // try/catch goes here, this will be a process and continue example
			try
			{
				myOne -= dVal;
				cout << dVal << ", ";
			}
			catch (string s)
			{
				cerr << s;
			}
		}
		cout << endl << "myOne:  " << myOne;
		myOne = myTwo = myTwo = myThree;

		for ( int i = 0; i <= 1000; i++ )
		{ 
			myOne << ( rand() % 100 / 10.0 );
		}
		cout << myOne;
		cin.get();

		// try/catch goes here, this will be a process and break example, 
		// extract til ya can not extract no more...
		try
		{
			while(1)
			{ 
				myOne -= dVal;
				cout << dVal << ", ";
			}
		}
		catch (string s)
		{
			cerr << endl << s;
		}
		cout << "\nDone\n";
		cin.get();
	} // end ICA14 test code
	// end of ICA14 test code

	return 0;
}