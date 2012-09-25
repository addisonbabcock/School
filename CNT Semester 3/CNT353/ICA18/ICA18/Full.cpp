#include "Full.h"

CFull::CFull (int iStart, int iEnd) : CLine (iStart, iEnd)
{}

void CFull::Draw (ostream & out) const
{
	for (int i (0); i < _iStart; ++i)
		out << ' ';

	for (int i (_iStart); i < _iEnd; ++i)
		out << char (220);
	out << endl;
}

CLine * CFull::Clone () const
{
	return new CFull (*this);
}