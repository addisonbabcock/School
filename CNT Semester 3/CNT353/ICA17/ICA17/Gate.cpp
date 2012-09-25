#include "Gate.h"

void CGate::Set (bool bIn1, bool bIn2)
{
	_bIn1 = bIn1;
	_bIn2 = bIn2;
}

bool CGate::Get () const
{
	return _bOut;
}

ostream & operator << (ostream & out, CGate & gate)
{
	out << "A B  " << gate.Name () << endl;
	for (int A (0); A < 2; ++A)
	{
		for (int B (0); B < 2; ++B)
		{
			gate.Set (A, B);
			gate.Latch ();
			out << A << " " << B << "  " << (gate.Get () ? "1" : "0") << endl;
		}
	}
	return out << endl;
}