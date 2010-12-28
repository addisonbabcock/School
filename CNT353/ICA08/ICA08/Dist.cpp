#include "Dist.h"

using namespace std;

CDist Add (CDist const & argA, CDist const & argB)
{
	return CDist (argA._iSN + argB._iSN, argA._iEW + argB._iEW);
}

CDist & Sum (CDist & argA, CDist const & argB)
{
	argA._iEW += argB._iEW;
	argA._iSN += argB._iSN;
	return argA;
}

ostream & CDist::Display (ostream & out) const
{
	return out << abs(_iSN) << ((_iSN & 0x80000000) ? 'S' : 'N') << ' '
			   << abs(_iEW) << ((_iEW & 0x80000000) ? 'W' : 'E');
}
