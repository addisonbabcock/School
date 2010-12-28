#pragma once
#include "base.h"

//Error messages
char const gkszRadiusTooSmall [] = 
	"CCircle::CCircle (int, int, int) : iRadius is less then 0 : ";

class CCircle : public CBase
{
protected:
	int _iRadius; //the radius of the circle

public:
	CCircle (int iX, int iY, int iRadius);
	virtual ~CCircle (void);

	virtual void Draw (CGDIPDraw & draw);
	virtual CBase * Clone () const;
};
