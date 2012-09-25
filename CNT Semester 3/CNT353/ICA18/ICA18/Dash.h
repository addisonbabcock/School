#include "Line.h"

class CDash : public CLine
{
public:
	CDash (int iStart, int iEnd);
	virtual void Draw (ostream & out) const;
	virtual CLine * Clone () const;
};