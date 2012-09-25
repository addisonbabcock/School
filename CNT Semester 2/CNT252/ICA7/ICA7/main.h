#ifndef MAIN_H
#define MAIN_H

#include <iostream>
#include <iomanip>

using namespace std;

struct STime
{
	unsigned short int m_suiHour;
	unsigned short int m_suiMinute;
	unsigned short int m_suiSecond;
};

union UTime
{
	STime sTime;
	double dTime;
};

#endif