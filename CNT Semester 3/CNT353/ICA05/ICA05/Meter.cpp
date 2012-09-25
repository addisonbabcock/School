#include <iostream>
#include <iomanip>
#include "Meter.h"

using namespace std;

int CMeter::Convert () const
{
	return 100 * _piRange [ePos] / (_piRange [eMin] + _piRange [eMax]);
}

CMeter::CMeter (int iCurrent, int iMin, int iMax)
{
	_piRange = new int [3];
	_piRange [eMin] = iMin;
	_piRange [eMax] = iMax;

	if (iCurrent >= iMin && iCurrent <= iMax)
		_piRange [ePos] = iCurrent;
	else
		_piRange [ePos] = iMin;
}

CMeter::CMeter (CMeter const & copy) : _piRange (new int [3])
{
	_piRange [eMin] = copy._piRange [eMin];
	_piRange [eMax] = copy._piRange [eMax];
	_piRange [ePos] = copy._piRange [ePos];
}

CMeter::~CMeter ()
{
	delete [] _piRange;
	_piRange = 0;
}

void CMeter::Step ()
{
	if (_piRange [ePos] < _piRange [eMax])
		++(_piRange [ePos]);
}

void CMeter::Reset ()
{
	_piRange [ePos] = _piRange [eMin];
}

void CMeter::Display (ostream & out) const
{
	out << setw ((Convert () * gkiScreenWidth) / 100) << setfill (gkcBlock) << Convert () << "%\n";
}