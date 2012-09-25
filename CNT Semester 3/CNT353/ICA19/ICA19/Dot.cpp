#include "Dot.h"

CDot::CDot (int iStart, int iEnd) : CLine (iStart, iEnd)
{}

void CDot::Draw (ostream & out) const
{
	for (int i (0); i < _iStart; ++i)
		out << ' ';

	for (int i (_iStart); i < _iEnd; ++i)
	{
		if (i % 2)
			out << char (220);
		else
			out << ' ';
	}
	out << endl;
}

CLine * CDot::Clone () const
{
	return new CDot (*this);
}