#pragma once

#include <iostream>

using namespace std;

class CStack
{
	double * _pdStk;
	int _iStkSize;

public:
	CStack ();
	CStack (CStack const & old);
	~CStack ();
	CStack & operator = (CStack const & old);

	CStack & operator << (double dNewVal);
	CStack & operator -= (double & dPop);

	friend ostream & operator << (ostream & out, CStack const & stk);
};