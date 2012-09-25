// icarcles
#include <iostream>
#include <iomanip>
#include <string>
#include <vector>
#include <algorithm>
#include "gdipdraw.h"

using namespace std;

typedef vector<CEllipse> vectinator;
typedef list<CEllipse> listinator;


CGDIPDraw _gr;
// Complete an overloaded < operator for CShape objects
// return true if the area of the LHS is less than the RHS
// Hint: starts like (lhs.GetEndX() - lhs.GetStartX()) * (lhs.GetEndY()...

bool operator < (CShape const & lhs, CShape const & rhs)
{
	return ((lhs.GetEndX () - lhs.GetStartX ()) * (rhs.GetEndY () - rhs.GetStartY ())) <
		((rhs.GetEndX () - rhs.GetStartX ()) * (rhs.GetEndY () - rhs.GetStartY ()));
}

// complete the ShowVectorF() method, accepts a vector of CEllipse by reference,
//  Clear() your global draw object, then using an iterator AddEllipse(), then finally Render()
//  Use cout to indicate the current size of the vector

void ShowVectorF (vectinator const & shapes)
{
	_gr.Clear ();
	//	for (unsigned int i (0); i < shapes.size (); ++i)
	//		_gr.AddEllipse (shapes [i]);

	vectinator::const_iterator iter (shapes.begin ());
	while (iter != shapes.end ())
	{
		_gr.AddEllipse (*iter);
		++iter;
	}
	_gr.Render ();
}

// complete the ShowVectorR() method, accepts a vector of CEllipse by reference,
//  Clear() your global draw object, then using a REVERSE iterator AddEllipse(), then finally Render()
//  Use cout to indicate the current size of the vector

void ShowVectorR (vectinator const & shapes)
{
	_gr.Clear ();

	vectinator::const_reverse_iterator riter (shapes.rbegin ());
	while (riter != shapes.rend ())
	{
		_gr.AddEllipse (*riter);
		++riter;
	}
	_gr.Render ();
}

// complete your Transform() method, accepts a CEllipse object by value
//  using SetStart(), increase the x and y by 20, return the object by value

CEllipse Transform (CEllipse ellipse)
{
	ellipse.SetStart (ellipse.GetStartX () + 20, ellipse.GetStartY () + 20);
	return ellipse;
}

void ShowListF (listinator const & shapes)
{
	_gr.Clear ();
	listinator::const_iterator iter (shapes.begin ());
	while (iter != shapes.end ())
	{
		_gr.AddEllipse (*iter);
		++iter;
	}
	_gr.Render ();
}

void ShowListR (listinator const & shapes)
{
	_gr.Clear ();
	listinator::const_reverse_iterator riter (shapes.rbegin ());
	while (riter != shapes.rend ())
	{
		_gr.AddEllipse (*riter);
		++riter;
	}
	_gr.Render ();
}

int main( int argc, char ** argv )
{
	{	// Declare a vector of CEllipse, no size ( #1 )

		vectinator ellipses;

		// Iterate for 500 times, using push_back(), add ellipses of these properties
		// startx = random between 0 and 799,
		// starty = random between 0 and 599
		// size = random between 40 and 79
		// thickness = 5,
		// line color black
		// fill color = red( between 0 and 254 ), green = 255, blue = 0

		for (int i (0); i < 500; ++i)
		{
			ellipses.push_back (CEllipse (rand () % 800, rand () % 600, rand () % 40 + 40, 
				5, RGB (0, 0, 0), RGB (rand () % 255, 255, 0)));
		}

		// Invoke Show Forward

		ShowVectorF (ellipses);
		cout << "Showing 1 For\n";
		cin.get();

		// Invoke Show Reverse

		ShowVectorR (ellipses);
		cout << "Showing 1 Rev\n";
		cin.get();

		// Shuffle your whole vector, Show Forward

		random_shuffle (ellipses.begin (), ellipses.end ());
		ShowVectorF (ellipses);
		cout << "Showing 1 Shuffled\n";
		cin.get();
		// Sort your vector ** this requires your operator < to be complete and correct, Show Forward

		sort (ellipses.begin (), ellipses.end ());
		ShowVectorF (ellipses);
		cout << "Showing 1 Sorted\n";
		cin.get();
		// make new vector of CEllipse (#2)
		vectinator ellipses2;
		// assign new = old, Show Forward #2
		ellipses2 = ellipses;

		ShowVectorF (ellipses2);
		cout << "Showing 2 For\n";
		cin.get();
		// using size() and [], iterate thru #2 up to and not including last 2 elements,
		// set fill color ( red = 0, green and blue = loop_count * 10 ), Show Forward

		for (int i (0);  i < ellipses2.size () - 2; ++i)
		{
			ellipses2 [i].SetFillColour (RGB (0, i * 10, i * 10));
		}
		ShowVectorF (ellipses2);

		cout << "Showing 2 - Colored\n";
		cin.get();

		// transform() #2, use Xform() function above, Show Forward

		transform (ellipses2.begin (), ellipses2.end (), ellipses2.begin (), Transform);
		ShowVectorF (ellipses2);
		cout << "Showing 2 Xformed - shifted\n";
		cin.get();

		// sort #2, show forward

		sort (ellipses2.begin (), ellipses2.end ());
		ShowVectorF (ellipses2);
		cout << "Showing 2 Sorted\n";
		cin.get();
		// declare another vector, same type as #3
		vectinator ellipses3;
		// use merge() to combine #1 and #2 into #3, ** must use back_inserter(), show forward

		merge (	ellipses.begin (), ellipses.end(), 
			ellipses2.begin (), ellipses2.end (), 
			back_inserter (ellipses3));
		ShowVectorF (ellipses3);
		cout << "Showing 3 Merged For\n";
		cin.get();

		// show reverse #3
		ShowVectorR (ellipses3);
		cout << "Showing 3 Merged Rev\n";
		cin.get();
		// clear #3
		ellipses3.clear ();
		// find common elements, insert into #3 ( set_intersection, back_inserter ), show forward

		set_intersection (ellipses.begin (), ellipses.end (), 
			ellipses2.begin (), ellipses2.end (), 
			back_inserter (ellipses3));
		ShowVectorF (ellipses3);
		cout << "Showing 3 Intersected Rev\n";
		cin.get();
	}
	//return 0;
	// now do this again with lists... use an iterator in place of the subscript step[]
	{  
		// Declare a vector of CEllipse, no size ( #1 )
		listinator ellipses;
		// Iterate for 500 times, using push_back(), add ellipses of these properties
		// startx = random between 0 and 799,
		// starty = random between 0 and 599
		// size = random between 40 and 79
		// thickness = 5,
		// line color black
		// fill color = red( between 0 and 254 ), green = 255, blue = 0
		for (int i (0); i < 500; ++i)
			ellipses.push_back (CEllipse (rand () % 799, rand () % 599, rand () % 40 + 40,
											5, RGB (0, 0, 0), RGB (rand () % 255, 255, 0)));

		// Invoke Show Forward
		ShowListF (ellipses);
		cout << "Showing 1 For\n";
		cin.get();

		// Invoke Show Reverse
		ShowListR (ellipses);
		cout << "Showing 1 Rev\n";
		cin.get();

		// Sort your vector ** this requires your operator < to be complete and correct, Show Forward
		ellipses.sort ();
		ShowListF (ellipses);
		cout << "Showing 1 Sorted\n";
		cin.get();
		// make new vector of CEllipse (#2)
		listinator ellipses2;
		// assign new = old, Show Forward #2
		ellipses2 = ellipses;
		ShowListF (ellipses2);
		cout << "Showing 2 For\n";
		cin.get();

		// using size() and [], iterate thru #2 up to and not including last 2 elements,
		// set fill color ( red = 0, green and blue = loop_count * 10 ), Show Forward
		int loop_count (0);
		for (listinator::iterator iter (ellipses2.begin ()); 
			iter != ellipses2.end () && loop_count != 498; ++iter)
		{
			iter->SetFillColour (RGB (0, loop_count * 10, loop_count * 10));
			++loop_count;
		}
		ShowListF (ellipses2);
		cout << "Showing 2 - Colored\n";
		cin.get();

		// transform() #2, use Xform() function above, Show Forward
		transform (ellipses2.begin (), ellipses2.end (), ellipses2.begin (), Transform);
		ShowListF (ellipses2);
		cout << "Showing 2 Xformed - shifted\n";
		cin.get();

		// sort #2, show forward
		ellipses2.sort (operator <);
		ShowListF (ellipses2);
		cout << "Showing 2 Sorted\n";
		cin.get();

		// declare another vector, same type as #3
		listinator ellipses3;
		// use merge() to combine #1 and #2 into #3, ** must use back_inserter(), show forward
		merge (ellipses.begin (), ellipses.end (),
				ellipses2.begin (), ellipses2.end (), 
				back_inserter (ellipses3));
		ShowListF (ellipses3);
		cout << "Showing 3 Merged For\n";
		cin.get();

		// show reverse #3
		ShowListR (ellipses3);
		cout << "Showing 3 Merged Rev\n";
		cin.get();
		// clear #3
		ellipses3.clear ();
		// find common elements, insert into #3 ( set_intersection, back_inserter ), show forward
		set_intersection (ellipses.begin (), ellipses.end (),
							ellipses2.begin (), ellipses2.end (),
							back_inserter (ellipses3));
		ShowListR (ellipses3);
		cout << "Showing 3 Intersected Rev\n";
		cin.get();
	}
	return 0;
}