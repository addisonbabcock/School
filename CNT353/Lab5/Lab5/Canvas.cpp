#include <iomanip>
#include <typeinfo.h>
#include "Canvas.h"
#include "Box.h"
#include "Circle.h"
#include "TextString.h"

CGDIPDraw CCanvas::_draw;


// Function name   : CCanvas::Grow
// Description     : Increases the size of the _pShapes array,
//				   : preserving the contents of the old array
// Return type     : void  
// Argument        : void

void CCanvas::Grow (void)
{
	//make room for more Shapes
	CBase ** pNewShapes = new CBase * [_iSize + gkiGrowSize];
	int iCurShape (0); //The shape object being copied

	//Copy the old ones if they exist
	if (_iShapeCount)
	{
		//Copy all the existing shapes
		for ( ; iCurShape < _iShapeCount; ++iCurShape)
		{
			pNewShapes [iCurShape] = _pShapes [iCurShape];
		}

		//Set the unused parts to null
		for ( ; iCurShape < _iSize; ++iCurShape)
		{
			pNewShapes [iCurShape] = 0;
		}
	}

	//clean up the old array and use the new array from now on
	delete [] _pShapes;
	_pShapes = pNewShapes;

	//the array is a little bigger now
	_iSize += gkiGrowSize;
}


// Function name   : CCanvas::CleanUp
// Description     : Cleans up the DMA used by a CCanvas object
// Return type     : void  
// Argument        : void

void CCanvas::CleanUp (void)
{
	//delete all the CBase objects
	for (int i (0); i < _iShapeCount; ++i)
	{
		delete _pShapes [i];
	}
	//then delete the array that holds them
	delete [] _pShapes;
	_pShapes = 0;
}

// Function name   : CCanvas::CCanvas
// Description     : Initializes a CCanvas to a default state
//				   : Start off with gkiGrowSize elements in the array

CCanvas::CCanvas (void) : _pShapes (0), _iShapeCount (0), _iSize (0)
{
	//Since we have a canvas, we will likely be putting something in it
	//make some room
	Grow ();
}

// Function name   : CCanvas::CCanvas 
// Description     : Copy CTOR for a CCanvas

CCanvas::CCanvas (CCanvas const & old) : _pShapes (0), _iShapeCount (0), _iSize (0)
{
	//leverage assignment op
	*this = old;
}


// Function name   : CCanvas::operator = 
// Description     : Overloaded assignment operator for CCanvas
//				   : safely copies the data from one CCanvas to another
// Return type     : CCanvas & - Invoking instance to support chaining
// Argument        : CCanvas const & old - The object being copied

CCanvas & CCanvas::operator = (CCanvas const & old)
{
	//self assignment check
	if (this == &old)
		return *this;

	int iCurShape (0); //the shape currently being copied

	//Clean up the old *this
	CleanUp ();
	
	//perform the easy copies first
	_iSize = old._iSize;
	_iShapeCount = old._iShapeCount;

	//deep copy the shapes
	_pShapes = new CBase * [_iSize];
	for ( ; iCurShape < _iShapeCount; ++iCurShape)
		_pShapes [iCurShape] = old._pShapes [iCurShape]->Clone ();

	//set the empty part of the array to null
	for ( ; iCurShape < _iSize; ++iCurShape)
		_pShapes [iCurShape] = 0;

	return *this;
}

// Function name   : CCanvas::~CCanvas
// Description     : DTOR for the CCanvas class, releases DMA used by the
//				   : invoking instance

CCanvas::~CCanvas (void)
{
	CleanUp ();
}


// Function name   : CCanvas::Show 
// Description     : Shows the shapes contained in the invoking instance
//				   : onto _draw. Also increments the number of times the shapes
//				   : have been rendered.
// Return type     : void 
// Argument        : void

void CCanvas::Show (void) const
{
	int iCurShape (0); //The shape currently being drawn

	//clear the screen
	_draw.Clear ();

	//put all the object onto the screen
	for ( ; iCurShape < _iShapeCount; ++iCurShape)
	{
		_pShapes [iCurShape]->Draw (_draw);
		_pShapes [iCurShape]->IncStep ();
	}

	//now show the results
	_draw.Render ();
}


// Function name   : CCanvas::operator << 
// Description     : Adds a shape to the CCanvas. Will grow the array if
//				   : it is needed. After calling this function, it is 
//				   : assumed that addMe is now the property of the invoking
//				   : instance and will be deleted as such.
// Return type     : CCanvas & - invoking instance to support chaining
// Argument        : CBase * addMe - The shape to be added to the canvas.

CCanvas & CCanvas::operator << (CBase * addMe)
{
	//check to see if we need to make room for the new shape
	if (_iSize == _iShapeCount)
		Grow ();

	//add the new shape to the array and increment the counter to reflect that
	_pShapes [_iShapeCount++] = addMe;

	return *this;
}


// Function name   : CCanvas::operator string 
// Description     : Creates a string containing information about
//				   : what is contained in the _shapes array.
// Return type     : std::string
// Argument        : void

CCanvas::operator string (void) const
{
	stringstream str;	//string buffer for the result
	int iBoxes (0),		//How many boxes in _pShapes
		iCircles (0),	// "   "  circles "   "
		iStrings (0);	// "   "  strings "   "

	//go through each shape
	for (int i (0); i < _iShapeCount; ++i)
	{
		//is it a box?
		if (typeid (*_pShapes [i]) == typeid (CBox))
		{
			++iBoxes;
		}

		//is it a circle?
		if (typeid (*_pShapes [i]) == typeid (CCircle))
		{
			++iCircles;
		}

		//is it a string?
		if (typeid (*_pShapes [i]) == typeid (CTextString))
		{
			++iStrings;
		}
	}

	//build the string and return it
	str << "CBox : " << setw (2) << iBoxes << ", CCircle : " 
		<< setw (2) << iCircles << ", CTextString : " << setw (2) << iStrings;
	return str.str ();
}