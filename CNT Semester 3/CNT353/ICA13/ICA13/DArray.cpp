#include <iostream>
#include <sstream>
#include <string>
#include "DArray.h"

using namespace std;

CDArray::CDArray(int iSize, double dInitVal) : _iSize (iSize), _pdArray (0)
{
	//make sure array size is valid
	if (_iSize < 0)
		throw string ("CDArray : CTOR value negative");

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
	if (iIndex < 0)
	{
		stringstream errorMsg;
		errorMsg << "CDArray::operator[] : index value too low : " << iIndex ;
		throw errorMsg.str ();
	}
		
	if (iIndex >= _iSize)
	{
		stringstream errorMsg;
		errorMsg << "CDArray::operator[] : index value too high : " << iIndex ;
		throw errorMsg.str ();
	}
	return _pdArray [iIndex];
}

double CDArray::operator [] (int iIndex) const
{
	if (iIndex < 0)
	{
		stringstream errorMsg;
		errorMsg << "CDArray::operator[] : index value too low : " << iIndex ;
		throw errorMsg.str ();
	}
		
	if (iIndex >= _iSize)
	{
		stringstream errorMsg;
		errorMsg << "CDArray::operator[] : index value too high : " << iIndex ;
		throw errorMsg.str ();
	}
	return _pdArray [iIndex];
}