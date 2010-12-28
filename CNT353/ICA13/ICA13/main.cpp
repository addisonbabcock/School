#include <iostream>
#include <string>
#include <crtdbg.h>
#include "DArray.h"

using namespace std;

int main ()
{
	_CrtSetDbgFlag (_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
	// ICA  13 Test Code - New 
	{ // ICA13 test code 
		CDArray cOne( 12, 0 ); // Array size of 12, initialized to 0

		// try/catch the following :
		try
		{
			CDArray cTwo( -12, 2 );
		}
		catch (string & s)
		{
			cerr << s << endl;
		}

		for ( int i = -1; i <= cOne.Size(); i++ )
		{ // try/catch goes here, this will be a process and continue example
			try
			{
				cOne[i] += ++cOne[i] + cOne[i] + i;          // 4, 5, 6...
			}
			catch (string & s)
			{
				cerr << s << endl;
			}
		}
		// try/catch goes here ( wrap the for() loop in it ), here if we catch an
		// exception we give up the loop
		try
		{
			for ( int i = 0; i <= cOne.Size() + 1; i++ )
			{ 
				cout << cOne[i] << ", ";                      // 4, 5, 6
			}
		}
		catch (string & s)
		{
			cerr <<  endl << s << endl;
		}
		cout << endl;
		cin.get();
	} // end ICA13 test code
	// end of ICA13 test code
	return 0;
}