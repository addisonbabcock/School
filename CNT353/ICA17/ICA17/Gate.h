#pragma once
#include <string>
#include <iostream>

using namespace std;

class CGate
{
protected:
	bool _bIn1, _bIn2, _bOut;

public:
	void Set (bool, bool);
	bool Get () const;
	virtual void Latch () = 0;
	virtual string Name () const = 0;
};

ostream & operator << (ostream &, CGate &);