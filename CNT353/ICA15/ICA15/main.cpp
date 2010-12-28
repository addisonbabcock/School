#include <iostream>
#include <string>
#include <crtdbg.h>
#include "DDArray.h"

using namespace std;

int main ()
{
	_CrtSetDbgFlag (_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
	{
		// ICA  15 Test Code
		{ // ICA15 test code
			CDDArray cOne(11,0);
			CDDArray const cTwo( 11, 2.0 );

			CDDArray cThree( cTwo ); // automagic copy ctor at work here

			cOne = cTwo; // should work transparently with existing assignment op 

			double dValue;
			cout << "cOne : " << cOne.Avg() <<  ", " << cOne.Sum() << endl;
			cout << "cTwo : " << cTwo.Avg() <<  ", " << cTwo.Sum() << endl;
			cout << "cThree : " << cThree.Avg() <<  ", " << cThree.Sum() << endl;
			for ( int i = 0; i < cOne.Size(); i++ )
			{
				cOne[i] += ++cOne[i] + cTwo[i] + i;
			}
			cout << "cOne Now : " << cOne.Avg() <<  ", " << cOne.Sum() << endl;
			cin.get();
		} // end ICA15 test code
		cin.get(); // should see dtor for each, what order ?
	}// end of ICA15 test code
	return 0;
}