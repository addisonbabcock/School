#include "or.h"

void COr::Latch ()
{
	_bOut = _bIn1 || _bIn2;
}

string COr::Name () const
{
	return string ("OR");
}