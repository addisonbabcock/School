#include "Line.h"

class CDot : public CLine
{
public:
	CDot (int iStart, int iEnd);
	virtual void Draw (ostream & out) const;
	virtual CLine * Clone () const;
};