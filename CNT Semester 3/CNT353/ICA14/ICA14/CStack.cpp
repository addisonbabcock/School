#include <iostream>
#include <string>
#include "CStack.h"

using namespace std;

CStack::CStack () : _iStkSize (0), _pdStk (new double [0])
{}

CStack::CStack (CStack const & old) : _iStkSize (0), _pdStk (new double [0])
{
	*this = old;
}

CStack::~CStack ()
{
	delete [] _pdStk;
	_pdStk = 0;
	_iStkSize = 0;
}

CStack & CStack::operator = (CStack const & old)
{
	if (this == &old)
		return *this;

	CStack::~CStack ();

	_iStkSize = old._iStkSize;
	_pdStk = new double [_iStkSize];
	for (int i (0); i < _iStkSize; ++i)
		_pdStk [i] = old._pdStk [i];

	return *this;
}

CStack & CStack::operator << (double dNewVal)
{
	++_iStkSize;
	double * newStk (new double [_iStkSize]);

	for (int i (0); i < _iStkSize - 1; ++i)
		newStk [i] = _pdStk [i];
	newStk [_iStkSize - 1] = dNewVal;

	delete [] _pdStk;
	_pdStk = newStk;

	return *this;
}

CStack & CStack::operator -= (double & dPop)
{
	if (!_iStkSize || !_pdStk)
		throw string ("CStack::operator -= () : Stack underflow\n");

	--_iStkSize;
	dPop = _pdStk [_iStkSize];
	return *this;
}

ostream & operator << (ostream & out, CStack const & stk)
{
	for (int i (0); i < stk._iStkSize; ++i)
		out << stk._pdStk [i] << ", ";
	return out << endl;
}