#pragma once

#include <iostream>
#include "Dist.h"

using namespace std;

class C3Dist : public CDist
{
	int _iElev;
public:
	C3Dist (int iLat, int iLong, int iElev);
	C3Dist (C3Dist const & cpy);
	C3Dist & operator = (C3Dist const & rhs);
	C3Dist & operator += (C3Dist const & rhs);
	C3Dist operator + (C3Dist const & rhs) const;

	friend ostream & operator << (ostream & out, C3Dist const & dist);
};