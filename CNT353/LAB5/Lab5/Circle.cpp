#include "Circle.h"
#include <sstream>

using namespace std;


// Function name   : CCircle::CCircle
// Description     : Constructor for the CCircle class. Throws an std::string
//				   : if iRadius is out of range.
// Argument        : int iX - the X location of the center of the circle
// Argument        : int iY - the Y location of the center of the circle
// Argument        : int iRadius - the radius of the circle

CCircle::CCircle(int iX, int iY, int iRadius) : CBase (iX, iY), _iRadius (iRadius)
{
	//range checking in _iRadius
	if (_iRadius < 0)
	{
		stringstream str;
		str << gkszRadiusTooSmall << _iRadius;
		throw str.str ();
	}
}

// Function name   : CCircle::~CCircle
// Description     : DTOR for the CCircle class 

CCircle::~CCircle(void)
{
}

// Function name   : CCircle::Draw 
// Description     : Draws the CCircle onto the CGDIPDraw passed to it
// Return type     : void 
// Argument        : CGDIPDraw & draw - where to draw the circle

void CCircle::Draw (CGDIPDraw & draw)
{
	draw.AddEllipse (CEllipse (_iX - _iRadius , _iY - _iRadius , _iRadius*2, 1, 
						RGB (128, _iRenders, 128), RGB (128, _iRenders, 128)));
}

// Function name   : CCircle::Clone 
// Description     : Makes a copy of the invoking instance on the heap
// Return type     : CBase * - a pointer to the new circle

CBase * CCircle::Clone () const
{
	return new CCircle (*this);
}