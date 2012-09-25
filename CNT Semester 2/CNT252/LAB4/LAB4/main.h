/***************************************************\
* Project:		Lab 4 - Drawing Thinger				*
* Files:		main.cpp, main.h, utilities.cpp		*
*				utilities.h, CDRrawInterface.h,		*
*				graphics.h, graphics.cpp			*
* Date:			02 Nov 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	A primitive drawing program.		*
\***************************************************/

#ifndef MAIN_H
#define MAIN_H

typedef unsigned char byte;

//Whatever these are for, I have NO idea!
//Was included in the demo program, so yeah...
const int kiLeftMargin = 20;
const int kiDistanceBetweenBars = 10;

struct SBytePoint
{
	byte m_byX, m_byY;
};

struct SFloatPoint
{
	float m_fX, m_fY;
};

struct SByteLine
{
	SBytePoint m_sStart, m_sEnd;
};

struct SFloatLine
{
	SFloatPoint m_sStart;
	float m_fAngle, m_fLength;
};

void Menu ();
void Help ();

#endif // MAIN_H