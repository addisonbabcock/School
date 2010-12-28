#pragma once
#include <iostream>

using namespace std;

class CDist
{
	int _iSN;
	int _iEW;

public:
	friend CDist Add (CDist const &, CDist const &);
	friend CDist & Sum (CDist &, CDist const &);
	
	CDist(int iSN, int iEW) : _iSN (iSN), _iEW (iEW) {}

	ostream & Display (ostream &) const;
};
