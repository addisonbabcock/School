#include "And.h"

void CAnd::Latch ()
{
	CNand::Latch ();
	_bOut = !_bOut;
}

string CAnd::Name () const
{
	return string ("AND");
}