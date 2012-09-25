#include "Line.h"

class CDash : public CLine
{
private:
	CDash & operator = (CDash const &);

protected:
	int * _piDashLen;

public:
	CDash (int iStart, int iEnd);
	CDash (CDash const & cpy);
	virtual ~CDash ();

	virtual void Draw (ostream & out) const;
	virtual CLine * Clone () const;
};