#include "Dist.h"

using namespace std;

/*
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
*/

CDist CDist::operator+ (CDist const & rhs) const
{
	return CDist (_iSN + rhs._iSN, _iEW + rhs._iEW);
}

CDist & CDist::operator -= (CDist const & rhs)
{
	return *this = *this - rhs;
}

ostream & operator<< (ostream & out, CDist const & rhs)
{
	return out << abs(rhs._iSN) << ((rhs._iSN & 0x80000000) ? 'S' : 'N') << ' '
			   << abs(rhs._iEW) << ((rhs._iEW & 0x80000000) ? 'W' : 'E');
}

CDist operator- (CDist const & lhs, CDist const & rhs)
{
	return CDist (lhs._iSN - rhs._iSN, lhs._iEW - rhs._iEW);
}

CDist & operator+= (CDist & lhs, CDist const & rhs)
{
	return lhs = lhs + rhs;
}

