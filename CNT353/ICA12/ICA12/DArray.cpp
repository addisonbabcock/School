#include "DArray.h"

CDArray::CDArray(int iSize, double dInitVal) : _iSize (iSize), _pdArray (0)
{
	//make sure array size is valid
	_iSize = _iSize > 0 ? _iSize : 1;
	_pdArray = new double [_iSize];
	for (int i (0); i < _iSize; ++i)
		_pdArray [i] = dInitVal;
}

CDArray::~CDArray ()
{
	delete [] _pdArray;
	_pdArray = 0;
	_iSize = 0;
}

CDArray::CDArray (CDArray const & cpy) : _iSize (cpy._iSize), _pdArray (0)
{
	*this = cpy;
}

CDArray & CDArray::operator = (CDArray const & cpy)
{
	if (this == &cpy)
		return *this;

	CDArray::~CDArray ();
	_iSize = cpy._iSize;
	_pdArray = new double [_iSize];
	for (int i (0); i < _iSize; ++i)
		_pdArray [i] = cpy._pdArray [i];

	return *this;
}

double & CDArray::operator [] (int iIndex)
{
	if (iIndex < 0 || iIndex >= _iSize)
		return _pdArray [0];
	return _pdArray [iIndex];
}

double CDArray::operator [] (int iIndex) const
{
	if (iIndex < 0 || iIndex >= _iSize)
		return _pdArray [0];
	return _pdArray [iIndex];
}