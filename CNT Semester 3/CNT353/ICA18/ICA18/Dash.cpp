#include "Dash.h"

CDash::CDash (int iStart, int iEnd) : CLine (iStart, iEnd)
{}

void CDash::Draw (ostream & out) const
{
	for (int i (0); i < _iStart; ++i)
		out << ' ';

	for (int i (_iStart); i < _iEnd; ++i)
	{
		if (i % 3)
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