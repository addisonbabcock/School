#pragma once
#include <iostream>

using namespace std;
class CClock
{
private:
	int _iHours;
	int _iMinutes;
	int _iSeconds;
	void Normalize (void);
public:
	CClock (int iHours = 0, int iMinutes = 0, int iSeconds = 0);
	void Tick (void);
	void Display (ostream& out) const;
};