/****************************************************
Project: Lab 04 - Path-O-Matic
Files: Trek.h, Trek.cpp, Path.h, Path.cpp
Date: 23 March 2007
Author: Addison Babcock		Class: CNT2K
Instructor: Herb V.			Course: CNT353
****************************************************/

#include <math.h>
#include <sstream>
#include "Trek.h"

using namespace std;

// Function		: CTrek::CTrek (double dCoord)
// Description	: Extracts coordinates out of a double and creates a CTrek
//				: 000.000 -> (000, 000); 123.456 -> (123, 456);
// Argument		: dCoord - The coord to be extracted

CTrek::CTrek (double dCoord)
{
	_iX = static_cast<int> (floor (dCoord));
	dCoord -= _iX;
	//Scale the Y value and truncate whatever is left over
	_iY = static_cast<int> (dCoord  * gkiYScale ); 

	//is the X out of range?
	if (_iX < 0 || _iX > gkiMaxX)
	{
		stringstream str;
		str << gkszX_OOR << _iX;
		throw str.str ();
	}
	//is the Y invalid?
	if (_iY < 0 || _iY > gkiMaxY)
	{
		stringstream str;
		str << gkszY_OOR << _iY;
		throw str.str ();
	}
}

// Function		: bool CTrek::operator == (CTrek const & rhs) const
// Description	: Compares the invoking instance to another CTrek
// Argument		: rhs - The CTrek to compare against
// Returns		: true if both are the same, false otherwise

bool CTrek::operator == (CTrek const & rhs) const
{
	//are both sides the same?
	return _iX == rhs._iX && _iY == rhs._iY;
}

// Function		: bool CTrek::operator == (CTrek const & rhs) const
// Description	: Compares the invoking instance to another CTrek
// Argument		: rhs - The CTrek to compare against
// Returns		: false if both are the same, true otherwise

bool CTrek::operator != (CTrek const & rhs) const
{
	//are both sides not the same?
	return !(*this == rhs);
}