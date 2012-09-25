#include "Nand.h"

void CNand::Latch ()
{
	_bOut = !(_bIn1 && _bIn2);
}

string CNand::Name () const
{
	return string ("NAND");
}