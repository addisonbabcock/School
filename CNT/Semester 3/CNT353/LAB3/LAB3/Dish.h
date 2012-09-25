/****************************************************
Project: Lab 03 - Petri dish
Files: Germ.h, Germ.cpp, Dish.h, Dish.cpp
Date: 06 Mar 2007
Author: Addison Babcock Class: CNT3K
Instructor: Herb V. Course: CNT353
Description: Petri dish simulation
****************************************************/

#pragma once

#include "Germ.h"

/*constants*/
const int gkiCols (50); //max columns
const int gkiRows (40); //max rows
const int gkiAliveDef (gkiCols * gkiRows / 2); //default alive count
const int gkiAliveMax (gkiCols * gkiRows); //max alive count

/*Error messages*/
//errors
const char gkszDishEmpty	[] = "Dish is empty and cannot be shown.\n";
const char gkszAliveOOR		[] = "iAlive out of range";
const char gkszColOOR		[] = "iCol out of range";
const char gkszRowOOR		[] = "iRow out of range";
//function names
const char gkszCDishLive	[] = "CDish::Live ()";
const char gkszCDishDay		[] = "CDish::Day ()";
const char gkszCDishShow	[] = "CDish::Show ()";
const char gkszCDishCDish	[] = "CDish::CDish ()";
const char gkszCDishSetCell	[] = "CDish::SetCell ()";

class CDish
{
private:
	CGerm * _pGerms [gkiCols][gkiRows];
	int _iDay;
	int _iAlive;

	void CleanUp ();
	static bool CheckRow (int iRow);
	static bool CheckCol (int iCol);
	static bool CheckAlive (int iAlive);

public:
	CDish (int iAlive = gkiAliveDef);
	CDish (CDish const & copyme);
	CDish & operator= (CDish const & copyme);
	~CDish (void);
	CDish & SetCell (int iRow, int iCol, int iHP = 10);
	int Live (int iCol, int iRow);
	int Day ();
	void Show () const;

	// Function name   : CGerm::GetAlive () const
	// Description     : Returns the current alive count
	// Return type     : int - the current alive count

	int GetAlive () const
	{
		return _iAlive;
	}

	// Function name   : CGerm::GetDay () const
	// Description     : Returns the current day count
	// Return type     : int - the current day count.

	int GetDay () const
	{
		return _iDay;
	}
};
