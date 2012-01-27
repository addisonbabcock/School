#include <sstream>
#include "Box.h"

using namespace std;

// Function name   : CBox::CBox
// Description     : Constructor for the CBox class. Throws an std::string
//				   : if iSize is out of range.
// Argument        : int iX - the X location of the center of the box
// Argument        : int iY - the Y location of the center of the box
// Argument        : int iSize - the size of the box

CBox::CBox (int iX, int iY, int iSize) : CBase (iX, iY), _iSize (iSize)
{
	if (_iSize < 0)
	{
		stringstream str;
		str << gkszSizeTooSmall << _iSize;
		throw str.str ();
	}
}

// Function name   : CBox::~CBox
// Description     : DTOR for the CBox class 

CBox::~CBox(void)
{
}

// Function name   : CBox::Draw 
// Description     : Draws the CBox onto the CGDIPDraw passed to it
// Return type     : void 
// Argument        : CGDIPDraw & draw - where to draw the box

void CBox::Draw (CGDIPDraw & draw)
{	
	draw.AddRectangle (CRectangle (_iX - _iSize / 2, _iY - _iSize / 2, _iSize, 1, 
					RGB (128, 128, _iRenders), RGB (128, 128, _iRenders)));
}

// Function name   : CBox::Clone 
// Description     : Makes a copy of the invoking instance on the heap
// Return type     : CBase * - a pointer to the new box

CBase * CBox::Clone () const
{
	return new CBox (*this);
}