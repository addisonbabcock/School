#include "DDArray.h"

CDDArray::CDDArray (int iSize, double dInit) : CDArray (iSize, dInit)
{}

CDDArray::~CDDArray ()
{
	cout << "Derived DTOR\n";
}

double CDDArray::Sum () const
{
	double dSum (0.0);

	for (int i (0); i < _iSize; ++i)
		dSum += _pdArray [i];

	return dSum;
}

double CDDArray::Avg () const
{
	return Sum () / _iSize;
}