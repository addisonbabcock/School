#include "Line.h"

CLine::CLine (int iStart, int iEnd) : _iStart (iStart), _iEnd (iEnd)
{}

CLine ::~CLine ()
{}

ostream & operator << (ostream & out, CLine const & line)
{
	out << typeid (line).name ();
	line.Draw (out);
	return out;
}