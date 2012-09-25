#pragma once
#include "Line.h"

class CFull : public CLine
{
public:
	CFull (int iStart, int iEnd);
	virtual void Draw (ostream & out) const;
	virtual CLine * Clone () const;
};