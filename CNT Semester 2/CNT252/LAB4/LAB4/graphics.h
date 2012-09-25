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

#ifndef GRAPHICS_H
#define GRAPHICS_H

#include "CDrawInterface.h"
#include "main.h"

typedef unsigned char byte;

void GetColor (SColor* const sColor);
void GetPoint (SBytePoint* const sPoint);
void GetPoint (SFloatPoint* const sPoint);
void GetLine (SByteLine* const sLine);
void GetLine (SFloatLine* const sLine);
void DrawLine (SByteLine const * const sLine);
void DrawLine (SFloatLine const * const sLine);
void Turtle ();
void SetColor (SColor sColor);
void SetBGColor (SColor sBG);
void SetPoint (SBytePoint const * const sPoint);
void Clear ();

#endif //GRAPHICS_H