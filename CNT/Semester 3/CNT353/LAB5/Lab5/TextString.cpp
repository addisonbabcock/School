#include <sstream>
#include "TextString.h"

using namespace std;

// Function name   : CTextString::CTextString
// Description     : Constructor for the CTextString class. Throws a string
//				   : if szText does not point to a valid string.
// Argument        : int iX - the X location of the top left of the text
// Argument        : int iY - the Y location of the top left of the text
// Argument        : int szText - A pointer to the null terminated string
//				   : that will be used as the text.
CTextString::CTextString (int iX, int iY, char const * szText) : CBase (iX, iY)
{
	//does the string exist?
	if (!szText)
	{
		stringstream str;
		str << gkszBadString;
		throw str.str ();
	}

	//copy the string for our uses
	_szText = new char [strlen (szText) + 1];
	strcpy (_szText, szText);
}

// Function name   : CTextString::~CTextString
// Description     : DTOR for the CTextString class. Releases the DMA used

CTextString::~CTextString(void)
{
	delete [] _szText;
	_szText = 0;
}

// Function name   : CTextString::CTextString
// Description     : Copy CTOR for the CTextString class.

CTextString::CTextString (CTextString const & cpy) 
	: CBase (cpy._iX, cpy._iY), _szText (0)
{
	//leverage assignment op
	*this = cpy;
}


// Function name   : CTextString::operator = 
// Description     : assignment operator for CTextString. copies the contents
//				   : of one CTextString into another.
// Return type     : CTextString & - the invoking instance to support chaining
// Argument        : CTextString const & cpy - the object that is being copied

CTextString & CTextString::operator = (CTextString const & cpy)
{
	if (this == &cpy)
		return *this;

	//Copy base
	this->CBase::operator = (cpy);

	//clean up first
	delete [] _szText;

	//copy derived
	_szText = new char [strlen (cpy._szText) + 1];
	strcpy (_szText, cpy._szText);

	return *this;
}

// Function name   : CTextString::Draw 
// Description     : Draws the CTextString onto the CGDIPDraw passed to it
// Return type     : void 
// Argument        : CGDIPDraw & draw - where to draw the text

void CTextString::Draw (CGDIPDraw & draw)
{
	wstringstream str;
	str << _szText;
	draw.AddText (CText (str.str (), _iX, _iY, _iX + 100, _iY + 50, 12, 
						RGB (_iRenders, 0, 0)));
}

// Function name   : CTextString::Clone 
// Description     : Makes a copy of the invoking instance on the heap
// Return type     : CBase * - a pointer to the new text string

CBase * CTextString::Clone () const
{
	return new CTextString (*this);
}