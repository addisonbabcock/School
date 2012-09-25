#pragma once
#include "base.h"

//Error messages
char const gkszSizeTooSmall [] = 
	"CBox::CBox (int, int, int) : iSize is less then 0 : ";

class CBox : public CBase
{
protected:
	int _iSize; //How big the box is

public:
	CBox (int iX, int iY, int iSize);
	virtual ~CBox (void);

	virtual void Draw (CGDIPDraw & draw);
	virtual CBase * Clone () const;
};
