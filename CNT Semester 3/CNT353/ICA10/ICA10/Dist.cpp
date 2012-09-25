#include "Dist.h"

using namespace std;

CDist CDist::operator+ (CDist const & rhs) const
{
	return CDist (_iSN + rhs._iSN, _iEW + rhs._iEW);
}

CDist & CDist::operator -= (CDist const & rhs)
{
	return *this = *this - rhs;
}

CDist CDist::operator- () const
{
	return CDist (-_iSN, -_iEW);
}

CDist & CDist::operator= (int iVal)
{
	_iSN = _iEW = iVal;
	return *this;
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

bool operator! (CDist const & arg)
{
	return !(arg._iEW || arg._iSN);
}

