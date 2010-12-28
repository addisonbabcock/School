/****************************************************
Project: Lab 03 - Petri dish
Files: Germ.h, Germ.cpp, Dish.h, Dish.cpp
Date: 06 Mar 2007
Author: Addison Babcock Class: CNT3K
Instructor: Herb V. Course: CNT353
Description: Petri dish simulation
****************************************************/

#include "Dish.h"
#include "Germ.h"

CDrawInterface CGerm::_draw;

// Function name   : CGerm::CheckHealth ()
// Description     : Checks the supplied argument to see if it is valid.
// Argument        : int iHealth - the health to be checked for validity
// Return type     : bool - true if the supplied argument is a valid health

bool CGerm::CheckHealth (int iHealth)
{
	return iHealth >= 0 && iHealth <= gkiMaxHealth;
}

// Function name   : CGerm::Show ()
// Description     : Shows the germs health on the _draw interface
// Return type     : void

void CGerm::Show () const
{
	//show the germ on the display
	_draw.SetColor (_iHealth * 25, _iHealth * 25, 0);
	_draw.SetSpace (_iRow, _iCol);
}

// Function name   : CGerm::Clear ()
// Description     : Clears the drawing interface
// Return type     : void

void CGerm::Clear ()
{
	//set the bg color and clear the display
	_draw.SetBackroundColor (0,0, 0);
	_draw.Clear ();
}

// Function name   : CGerm::CGerm ()
// Description     : Initializes the germ, iRow and iCol are bounds checked
// Argument	       : int iRow - the row this germ is located in
// Argument	       : int iCol - the column this germ is located in

CGerm::CGerm(int iRow, int iCol) : _iHealth (0), _iRow (0), _iCol (0)
{
	if (iRow < 0 || iRow > gkiRows)
	{
		cerr << gkszCGermCGerm << " :  " << gkszRowOOR << " : " << iRow <<endl;
		return;
	}

	if (iCol < 0 || iCol > gkiCols)
	{
		cerr << gkszCGermCGerm << " : " << gkszColOOR << " : " << iCol << endl;
		return;
	}

	_iRow = iRow;
	_iCol = iCol;
}