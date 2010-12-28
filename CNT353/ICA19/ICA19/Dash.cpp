#include "Dash.h"

CDash::CDash (int iStart, int iEnd) : CLine (iStart, iEnd), _piDashLen (0)
{
	_piDashLen = new int (rand () % 5 + 1);
}

CDash::CDash (CDash const & cpy) : CLine (cpy._iStart, cpy._iEnd), _piDashLen (0)
{
	_piDashLen = new int;
	*_piDashLen = *cpy._piDashLen;
}

CDash::~CDash ()
{
	delete _piDashLen;
	_piDashLen = 0;
}

void CDash::Draw (ostream & out) const
{
	for (int i (0); i < _iStart; ++i)
		out << ' ';

	for (int i (_iStart); i < _iEnd; ++i)
	{
		if (i % *_piDashLen)
			out << char (220);
		else
			out << ' ';
	}
	out << endl;
}

CLine * CDash::Clone () const
{
	return new CDash (*this);
}