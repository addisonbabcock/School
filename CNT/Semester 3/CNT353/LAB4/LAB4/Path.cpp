/****************************************************
Project: Lab 04 - Path-O-Matic
Files: Trek.h, Trek.cpp, Path.h, Path.cpp
Date: 23 March 2007
Author: Addison Babcock		Class: CNT2K
Instructor: Herb V.			Course: CNT353
****************************************************/

#include <iomanip>
#include <sstream>
#include <string>
#include <math.h>
#include "GDIPDraw.h"
#include "Path.h"

using namespace std;

CGDIPDraw CPath::_draw; //the drawing interface

// Function		: CPath::CleanUp ()
// Description	: deletes all allocated memory for the CPath class
// Returns		: void

void CPath::CleanUp ()
{
	delete [] _pCoords;
	_pCoords = 0;
}

// Function		: CPath::Grow ()
// Description	: Increases the size of the _pCoords array by gkiGrowSize
// Returns		: void

void CPath::Grow ()
{
	//make a new array and move all the old CTreks into it
	CTrek * newArr = new CTrek [_iCurSize + gkiGrowSize];
	for (int i (0); i < _iCurPathSize; ++i)
	{
		newArr [i] = _pCoords [i];
	}
	//the array is now slightly bigger
	_iCurSize += gkiGrowSize;

	//note: there is no need to recreate each individual CTrek because they
	//are saved in newArr
	delete [] _pCoords;
	_pCoords = newArr;
}

// Function		: CPath::CPath ()
// Description	: Creates and nulls the _pCoords array, inits everything else

CPath::CPath () : _iCurPathSize (0), _iCurSize (gkiGrowSize), 
				  _pCoords (new CTrek [gkiGrowSize])
{
	//don't need to do anything else here
}

// Function		: CPath::CPath (CPath const &)
// Description	: Constructs a copy of a CPath
// Argument		: cpy - What is being copied

CPath::CPath (CPath const & cpy) : _iCurPathSize (0), _iCurSize (0),
								   _pCoords (0)
{
	//leverage assignment operator
	*this = cpy;
}

// Function		: CPath::~CPath ()
// Description	: Cleans up all DMA for a CPath

CPath::~CPath ()
{
	CleanUp ();
}

// Function		: CPath::operator = (CPath const &)
// Description	: Copies a CPath into another CPath, supports chaining
// Argument		: cpy - What is being copied
// Returns		: CPath & - The invokign instance

CPath & CPath::operator = (CPath const & cpy)
{
	//prevent self-assignment
	if (this == &cpy)
		return *this;

	//clean up first
	CleanUp ();

	//these can be shallow copies
	_iCurSize = cpy._iCurSize;
	_iCurPathSize = cpy._iCurPathSize;

	//_pCoords and the CTreks it points to have to be deep copies
	_pCoords = new CTrek [_iCurSize];
	for (int i (0); i < _iCurPathSize; ++i)
	{
		_pCoords [i] = cpy._pCoords [i];
	}

	return *this;
}

// Function		: CPath::Show () const
// Description	: Shows the path and the distance travelled on the display
// Returns		: void

void CPath::Show () const
{
	//clear the interface
	_draw.Clear ();

	//add some lines to the interface
	for (int i (0); i < _iCurPathSize - 1; ++i)
	{
		_draw.AddLine (CLine(	_pCoords [i]._iX,
								_pCoords [i]._iY,
								_pCoords [i + 1]._iX,
								_pCoords [i + 1]._iY));
	}

	wstringstream output; //contains text that will be displayed on _draw

	//show a message about how far the Path is
	output << gkszDistance << *this;
	_draw.AddText (output.str ());

	//render it!
	_draw.Render ();
}

// Function		: CPath::operator += (CPath const &)
// Description	: Concatenates a CPath onto the end of another
// Argument		: rhs - Concatenate this path onto the end of the 
//				  invoking instance.
// Returns		: CPath & - the invoking instance

CPath & CPath::operator += (CPath const & rhs)
{
	//add each coord from the RHS to the end of the LHS
	for (int i (0); i < rhs._iCurPathSize; ++i)
	{
		try
		{
			*this += rhs._pCoords [i];
		}
		//ignore any exceptions that come out of CPath += CTrek
		catch (string)
		{
		}
	}
	return *this;
}

// Function		: CPath::operator += (CTrek const &)
// Description	: Adds another CTrek onto the end of the invoking instance
// Argument		: rhs - Add this coord to the invoking instance
// Returns		: CPath & - the invoking instance

CPath & CPath::operator += (CTrek const & rhs)
{
	//look to see if the coords are already in the array
	if (*this == rhs)
	{
		//Duplicate CTrek found, throw an exception
		stringstream str; //contains the error that is about to be thrown
		str << gkszDupCTrek << ": " << rhs._iX << ", " << rhs._iY;
		throw str.str ();
	}

	//At this point, CTrek is not a duplicate, add it

	//make room if needed
	if (_iCurPathSize == _iCurSize)
		Grow ();

	//add the CTrek
	_pCoords [_iCurPathSize++] = rhs;

	return *this;
}

// Function		: CPath::operator == (CPath const &) const
// Description	: Checks to see if the two CPaths are the same coords in the 
//				  same order.
// Argument		: rhs - Check this path against the invoking instance
// Returns		: true if the two are the same, false otherwise

bool CPath::operator == (CPath const & rhs) const
{
	//if the paths arent the same size, they cant be the same
	if (_iCurPathSize != rhs._iCurPathSize)
		return false;

	//check for non-matching CTreks
	for (int i (0); i < _iCurPathSize; ++i)
	{
		if (_pCoords [i] != rhs._pCoords [i])
			return false;
	}

	//all tests clear
	return true;
}

// Function		: CPath::operator double () const
// Description	: Returns the overall distance travelled by the CPath
// Returns		: double - The distance travelled

CPath::operator double () const
{
	//there can't be a distance if there are less then 2 CTreks
	if (_iCurPathSize < 2)
		return 0.0;

	//sqrt ((X1 - X2) ^ 2 + (Y1 - Y2) ^ 2)
	return sqrt (static_cast<double>(
					(_pCoords [0]._iX - _pCoords [_iCurPathSize - 1]._iX) * 
					(_pCoords [0]._iX - _pCoords [_iCurPathSize - 1]._iX) +
					(_pCoords [0]._iY - _pCoords [_iCurPathSize - 1]._iY) * 
					(_pCoords [0]._iY - _pCoords [_iCurPathSize - 1]._iY)));
}

// Function		: bool operator == (CPath const & lhs, CTrek const & rhs)
// Description	: Tests to see if a CTrek exists in a CPath
// Argument		: lhs - The path to look in
// Argument		: rhs - The CTrek to look for
// Returns		: true if the CTrek was found, false otherwise

bool operator == (CPath const & lhs, CTrek const & rhs)
{
	//Go through the CPath trying to find a CTrek
	for (int i (0); i < lhs._iCurPathSize; ++i)
	{
		if (lhs._pCoords [i] == rhs)
		{
			//The CTrek was found!
			return true;
		}
	}
	//The CTrek was not found
	return false;
}

// Function		: bool operator != (CPath const & lhs, CTrek const & rhs)
// Description	: Checks if a CTrek does not exist in a CPath
// Argument		: lhs - The CPath to look in
// Argument		: rhs - The CTrek to look for
// Returns		: false if the CTrek was found, true if it wasn't

bool operator != (CPath const & lhs, CTrek const & rhs)
{
	//Are they different?
	return !(lhs == rhs);
}

// Function		: CPath operator + (CPath const & lhs, CPath const & rhs)
// Description	: Creates a new CPath with the lhs and rhs concatenated
// Argument		: lhs - The beginning of the new path
// Argument		: rhs - The end of the new path
// Returns		: A new CPath with rhs concatenated to lhs

CPath operator + (CPath const & lhs, CPath const & rhs)
{
	//make a new CPath based on lhs and concat the rhs
	return CPath (lhs) += rhs;
}

// Function		: bool operator != (CPath const & lhs, CPath const & rhs)
// Description	: Compares 2 CPaths for the same coords in the same order
// Argument		: lhs - A CPath to check against the other one
// Argument		: rhs - A CPath to check against the other one
// Returns		: true if the paths are different, false if the are the same

bool operator != (CPath const & lhs, CPath const & rhs)
{
	//are they different?
	return !(lhs == rhs);
}
