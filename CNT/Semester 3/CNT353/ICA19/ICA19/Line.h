#pragma once
#include <iostream>

using namespace std;

class CLine
{
protected:
	int _iStart, _iEnd;

public:
	CLine (int iStart, int iEnd);
	virtual ~CLine ();
	virtual void Draw (ostream & out) const = 0;
	virtual CLine * Clone () const = 0;
};

ostream & operator << (ostream & out, CLine const & line);