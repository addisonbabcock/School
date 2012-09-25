#include "Bit.h"
#include <iostream>
#include <crtdbg.h>
//#include "e:\lab02_test.h"

using namespace std;

// Function name   : CopyCTORTest
// Description     : A function made to test the CBit copy constructor
// Return type     : void
// Argument        : CBit bits - Any old bitmap that should be tested

void CopyCTORTest (CBit bits)
{
	bits.UpsideDown ();
	bits.Write ("a3.bmp");
}

// Function name   : Lab02Test
// Description     : Tests the various functions contained in the CBit class
// Return type     : void

void Lab02Testy ()
{
	CBit cS ("fun.bmp");
	cS.Contrast ();
	cout << endl;

	//return;

	CBit cA ("a.bmp");
	cA.Info (cout);
	cA.UpsideDown ();
	cA.Contrast ();
	cA.ExtraA ();
	cA.ExtraB ();
	CopyCTORTest (cA);
	cA.Write ("a2.bmp");

	//return;
	cout << endl;

	CBit const cT ("t.bmp");
	cT.Info (cout);
	cT.Write ("t2.bmp");
}

int main ()
{
	_CrtSetDbgFlag (_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
	Lab02Testy ();
	cin.get ();
	return 0;
}