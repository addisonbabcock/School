/*Author:	Addison Babcock
//Class:	CNT15
*/

#include <iostream>
using namespace std;

void main (void)
{
	int iAge = 0xFFFFFFFF;
	char cChar = 0;
	bool bButt = true;
	double dValue = 1234567;
    
	cChar = 'A';

	//cin.get (cChar);

	//cout << cChar << endl;
	//cout << "iAge = " << iAge << " has " << sizeof (iAge) << " bytes." 
	//		<< endl;
	//cout << bButt << endl;

	cout << dValue << " at address " << &dValue << endl;

	system ("pause");
	return;
}