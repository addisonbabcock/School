#pragma once
#include "GDIPDraw.h"

//Global consts
int const gkiMaxX (799); //X goes from 0 to 799
int const gkiMaxY (599); //Y goes from 0 to 599

//Error messages
char const gkszXOutOfRange [] = 
	"CBase::CBase (int, int) : iX is out of range : ";
char const gkszYOutOfRange [] = 
	"CBase::CBase (int, int) : iY is out of range : ";

class CBase
{
protected:
	int _iX; //X Coord of the object on the canvas
	int _iY; //Y Coord of the object
	int _iRenders; //How many times this object was rendered

public:
	CBase (int iX, int iY);
	virtual ~CBase (void);

	virtual void Draw (CGDIPDraw & draw) = 0;
	virtual CBase * Clone () const = 0;
	void IncStep ();
	bool operator == (CBase const &) const;
};
