#include <iostream>
#include "Box.h"
#include "Circle.h"
#include "TextString.h"
#include "Canvas.h"

using namespace std;
//#include "e:\lab05_test.h"

void Lab05TestAddison ()
{
	//CGDIPDraw draw;

	//draw.Clear ();
	//CTextString text (0, 0, "HI!"), text2 (100, 100, "Stuff"), text3 (text);
	//text3.Draw (draw);
	//text = text2;
	//text.Draw (draw);

	//for (int i (0); i < 50; ++i)
	//{
	//	draw.Clear ();
	//	text.Draw (draw);
	//	text.IncStep ();
	//	draw.Render ();
	//	cout << i << endl;
	//	cin.get ();
	//}
	//CBox box (100, 100, 25);
	//
	//for (int j (0); j < 50; ++j)
	//{
	//	draw.Clear ();
	//	box.Draw (draw);
	//	box.IncStep ();
	//	draw.Render ();
	//	cin.get ();
	//}

	CCanvas A;
	CCircle * pCir = new CCircle (799, 599, 50 ); // make a new CCircle
	A << pCir; // add it to next slot in the canvas
	A.Show(); // output the canvas
	cin.get();
	// Since the pointer was given to the canvas, it owns it now
	// and it will delete it when done. You can use new to generate
	// the object too.
	A << ( new CTextString( 150,100,"Helo" ));
	A.Show();
	// Now you should ensure that your managers all work
	// Do this after you have basic functionality and understand
	// how the classes inter-operate.
	CCanvas B(A);
	CCanvas C;
	C = C;
	C = B = A;
	string str = C; // test your operator conversion
	cout << str; //Should see “CBox : 0 CCircle : 1 CTextString : 1”
	C.Show();

	cin.get ();
}

int main ()
{
	_CrtSetDbgFlag (_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
//	Lab05Test();
	Lab05TestAddison ();
	return 0;
}