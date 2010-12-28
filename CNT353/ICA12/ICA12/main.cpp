#include <crtdbg.h>
#include "DArray.h"

int main ()
{
	_CrtSetDbgFlag (_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
	// ICA  12 Test Code - New 
	{ // ICA12 test code 
		CDArray cOne( 24, 0 ); // Array size of 24, initialized to 0
		CDArray const cTwo( cOne ); // Copy of cOne
		CDArray cThree( 24, 88 );
		CDArray const cFour( 24, 2 );
		cOne = cTwo; // test assignment operator

		for ( int i = 0; i < cFour.Size(); i++ )
			cOne[i] += ++cOne[i] + cFour[i] + i;          // 4, 5, 6...

		cThree = cThree = cOne;
		for ( int i = -1; i < cOne.Size() + 1; i++ )
			cout << cOne[i] << ", ";                      // 4, 4, 5, 6
		cout << endl;
		for ( int i = -1; i < cTwo.Size() + 1; i++ )
			cout << cTwo[i] << ", ";                      // 0, 0, 0
		cout << endl;
		for ( int i = -1; i < cThree.Size() + 1; i++ )
			cout << cThree[i] << ", ";                      // 4, 4, 5, 6 ...
		cout << endl;
		cin.get();
	} // end ICA12 test code
	// end of ICA12 test code
	return 0;
}