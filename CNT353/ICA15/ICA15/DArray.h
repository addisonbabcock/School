#pragma once

#include <iostream>

using namespace std;

class CDArray
{
protected:
	int _iSize;
	double * _pdArray;

public:
	CDArray (int, double);
	~CDArray ();
	CDArray (CDArray const &);
	CDArray & operator = (CDArray const &);

	int Size () const
	{
		return _iSize;
	}
	double & operator [] (int);
	double operator [] (int) const;
};