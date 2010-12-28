#include "Xor.h"

void CXor::Latch ()
{
	_bOut = _bIn1 ^ _bIn2;
}

string CXor::Name () const
{
	return string ("XOR");
}