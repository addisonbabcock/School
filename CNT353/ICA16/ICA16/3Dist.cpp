#include <iostream>
#include "3Dist.h"

using namespace std;

C3Dist::C3Dist (int iLat, int iLong, int iElev) : CDist (iLat, iLong), _iElev (iElev)
{
}

C3Dist::C3Dist (C3Dist const & cpy) : CDist (cpy), _iElev (cpy._iElev)
{
}

C3Dist & C3Dist::operator = (C3Dist const & rhs)
{
	CDist::operator = (rhs);
	_iElev = rhs._iElev;
	return *this;
}

C3Dist C3Dist::operator + (C3Dist const & rhs) const
{
	return C3Dist (*this) += rhs;
}

C3Dist & C3Dist::operator += (C3Dist const & rhs)
{
	*this += static_cast<CDist const &> (rhs);
	_iElev += rhs._iElev;
	return *this;
}

ostream & operator << (ostream & out, C3Dist const & rhs)
{
	return out << static_cast<CDist const &> (rhs) << (rhs._iElev >= 0 ? " +" : " ") << rhs._iElev << "m";
}