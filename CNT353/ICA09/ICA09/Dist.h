#pragma once
#include <iostream>

using namespace std;

class CDist
{
	int _iSN;
	int _iEW;

public:	
	CDist(int iSN, int iEW) : _iSN (iSN), _iEW (iEW) {}

	CDist operator+ (CDist const &) const;
	CDist & operator -= (CDist const &);

	friend ostream & operator<< (ostream &, CDist const &);
	friend CDist operator- (CDist const & lhs, CDist const & rhs);
};

CDist & operator+= (CDist & lhs, CDist const & rhs);