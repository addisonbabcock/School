#include <iostream>
#include <iomanip>
#include "Clock.h"

CClock::CClock(int iHours, int iMinutes, int iSeconds)
{
	_iHours = iHours;
	_iMinutes = iMinutes;
	_iSeconds = iSeconds;

	Normalize ();
}

void CClock::Display(std::ostream &out) const
{
	out << setw (2) << setfill ('0') << _iHours << ':'
		<< setw (2) << setfill ('0') << _iMinutes << ':'
		<< setw (2) << setfill ('0') << _iSeconds << endl;
}

void CClock::Normalize()
{
	_iMinutes += _iSeconds / 60;
	_iSeconds %= 60;
	_iHours += _iMinutes / 60;
	_iMinutes %= 60;
	_iHours %= 24;
}

void CClock::Tick()
{
	_iSeconds++;
	Normalize ();
}