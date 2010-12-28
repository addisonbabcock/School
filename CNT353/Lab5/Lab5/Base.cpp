#include "Base.h"
#include <sstream>

using namespace std;


// Function name   : CBase::CBase
// Description     : Constructor for the CBase class
//				   : Throws a std::string if iX or iY are out of range
// Argument        : int iX - The X coord of the shape
// Argument        : int iY - The Y coord of the shape

CBase::CBase (int iX, int iY) : _iRenders (0), _iX (iX), _iY (iY)
{
	//Range checking X (0-799)
	if (_iX < 0 || _iX > gkiMaxX)
	{
		stringstream str;
		str << gkszXOutOfRange << _iX;
		throw str.str ();
	}

	//Range checking Y (0-599)
	if (_iY < 0 || _iY > gkiMaxY)
	{
		stringstream str;
		str << gkszYOutOfRange << _iY;
		throw str.str ();
	}
}

// Function name   : CBase::~CBase
// Description     : Destructor for the CBase class

CBase::~CBase (void)
{
}


// Function name   : CBase::IncStep ()
// Description     : Increments the number of times this object has been
//				   : rendered. _iRenders will not exceed 255
// Return type     : void 

void CBase::IncStep ()
{
	//"increment"
	_iRenders += 10;

	//make sure it didnt go out of range
	if (_iRenders > 255)
		_iRenders = 0;
}


// Function name   : CBase::operator== 
// Description     : Compares the coordinates of 2 CBase objects 
// Return type     : bool - true if the coords are the same, false otherwise
// Argument        : CBase const & rhs - The object that will be checked
//				   : against *this

bool CBase::operator == (CBase const & rhs) const
{
	return _iX == rhs._iX && _iY == rhs._iY;
}