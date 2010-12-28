#include "word.h"
#include <iostream>
#include <crtdbg.h>

using namespace std;

int main ()
{
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
	// ICA Midterm Test Code
	{ // ICAMidterm test code -- Attempt to identify the what methods/operators are in play 
		cout << "ICAMidterm Test Code - Start" << endl;
		CWord const One( "Rossi" );
		CWord const Two( "Hayden" );
		CWord Three( Two );
		cout << One << " " << Two << " " << Three << endl; // Rossi Hayden Hayden
		Three = ( Three = One );
		// end of manager function test
		cout << One << " " << Two << " " << Three << endl; // Rossi Hayden Rossi

		cout << "Equal ? " << One << " " << Three << " " << ( One == Three ) << endl;
		cout << "Not - Equal ? " << One << " " << Three << " " << ( One != Three ) << endl;
		cout << "Equal ? " << One << " " << Two << " " << ( One == Two ) << endl;
		cout << "Not - Equal ? " << One << " " << Two << " " << ( One != Two ) << endl;

		cout << "~ of " << One << " is " << ~One << endl;
		cout << "+ of both " << (One+Two+One) << endl;
		cout << "~ + of both is " << (One + ~Two) << endl;
		cout << "+=" << (( Three = ( Three += One )) += Two ) << endl;
		cout << "ICAMidterm Test Code -  End" << endl;
		cin.get();
	} // end ICAMidterm test code
	// end of ICAMidterm test code
	return 0;
}