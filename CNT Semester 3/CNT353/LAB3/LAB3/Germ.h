/****************************************************
Project: Lab 03 - Petri dish
Files: Germ.h, Germ.cpp, Dish.h, Dish.cpp
Date: 06 Mar 2007
Author: Addison Babcock Class: CNT3K
Instructor: Herb V. Course: CNT353
Description: Petri dish simulation
****************************************************/

#pragma once
#include "CDrawInterface.h"
#include <iostream>

using namespace std;

/*constants*/
const int gkiMaxHealth (10);

/*error messages*/
//function names
const char gkszCGermSetHealth	[] = "CGerm::SetHealth ()";
const char gkszCGermCGerm		[] = "CGerm::CGerm ()";
//errors
const char iHealthOOR			[] = "iHealth out of range";

class CGerm
{
private:
	int _iHealth;
	int _iRow;
	int _iCol;

	static CDrawInterface _draw;
	static bool CheckHealth (int iHealth);

public:
	void Show () const;

	// Function name   : CGerm::GetHealth () const
	// Description     : Returns the health of a germ
	// Return type     : int - the health of the germ

	int GetHealth () const
	{
		return _iHealth;
	}
	inline void SetHealth (int iHealth);
	static void Clear ();
	CGerm(int iRow, int iCol);
};

// Function name   : CGerm::SetHealth()
// Description     : Sets the health of a germ to the int argument
// Argument        : int iHealth - what the germs new health will be
// Return type     : void

void CGerm::SetHealth (int iHealth)
{
	//bounds checking
	if (!CheckHealth (iHealth))
	{
		cerr << gkszCGermSetHealth << " : " 
			 << iHealthOOR << " : " << iHealth << endl;
		return;
	}

	_iHealth = iHealth;
}
