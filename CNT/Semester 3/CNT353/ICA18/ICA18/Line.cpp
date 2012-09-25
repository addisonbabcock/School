#include "Line.h"

CLine::CLine (int iStart, int iEnd) : _iStart (iStart), _iEnd (iEnd)
{}

ostream & operator << (ostream & out, CLine const & line)
{
	line.Draw (out);
	return out;
}