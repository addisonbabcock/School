/****************************************************
Project: Lab 03 - Petri dish
Files: Germ.h, Germ.cpp, Dish.h, Dish.cpp
Date: 06 Mar 2007
Author: Addison Babcock Class: CNT3K
Instructor: Herb V. Course: CNT353
Description: Petri dish simulation
****************************************************/

#include "Dish.h"
#include <iostream>
#include <iomanip>

using namespace std;

// Function name   : CDish::CleanUp ()
// Description     : Destroys the dish (deallocates all memory taken)
// Return type     : void

void CDish::CleanUp ()
{
	//empty the germ array
	for (int iCol (0); iCol < gkiCols; ++iCol)
	{
		for (int iRow (0); iRow < gkiRows; ++iRow)
		{
			delete _pGerms [iCol][iRow];
			_pGerms [iCol][iRow] = 0;
		}
	}

	//no germs alive, the dish doesnt exist
	_iAlive = 0;
	_iDay = 0;
}

// Function name   : CDish::CheckCol (int iCol)
// Description     : Checks the supplied column for validity
// Argument        : int iCol - The column to check
// Return          : bool - true if the column is within range, false otherwise

bool CDish::CheckCol (int iCol)
{
	return iCol >= 0 && iCol < gkiCols;
}

// Function name   : CDish::CheckRow (int iRow)
// Description     : Checks the supplied row for validity
// Argument        : int iRow - The row to check
// Return          : bool - true if the row is within range, false otherwise

bool CDish::CheckRow (int iRow)
{
	return iRow >= 0 && iRow < gkiRows;
}

// Function name   : CDish::CheckAlive (int iAlive)
// Description     : Checks the supplied alive count for validity
// Argument        : int iAlive - The alive count to check
// Return          : bool - true if the alive count is within range,
//                   false otherwise

bool CDish::CheckAlive (int iAlive)
{
	return iAlive >= 0 && iAlive <= gkiAliveMax;
}

// Function name   : CDish::CDish (int iAlive)
// Description     : CTOR for CDish. Allocates the memory needed by the dish
//					 and creates the required amount of germs
// Argument	       : int iAlive - how many germs the dish should be populated
//					 with.

CDish::CDish (int iAlive) : _iAlive (0), _iDay (0)
{
	int iNewCol (0), iNewRow (0); //the location of a new germ

	//dont allow us to make more germs then we have room for
	//or negative germs
	if (!CheckAlive (iAlive))
	{
		cerr<<gkszCDishCDish<< " : " << gkszAliveOOR << " : " << iAlive <<endl;
		return;
	}

	//init the array to empty
	for (int iCol (0); iCol < gkiCols; ++iCol)
		for (int iRow (0); iRow < gkiRows; ++iRow)
			_pGerms [iCol][iRow] = new CGerm (iRow, iCol);

	while (_iAlive < iAlive)
	{
		//get a spot for a new germ
		iNewRow = rand () % gkiRows;
		iNewCol = rand () % gkiCols;

		//is the spot taken?
		if (!_pGerms [iNewCol][iNewRow]->GetHealth ())
		{
			_pGerms [iNewCol][iNewRow]->SetHealth (10);
			++_iAlive;
		}
	}
}

// Function name   : CDish::CDish (CDish const & copyme)
// Description     : Copy CTOR for the CDish class
// Argument        : CDish const & copyme - a reference to the dish being
//					 copied.

CDish::CDish (CDish const & copyme) 
			: _iAlive (copyme._iAlive), _iDay (copyme._iDay)
{
	//create all the germs, even the empty ones
	for (int iCol (0); iCol < gkiCols; ++iCol)
	{
		for (int iRow (0); iRow < gkiRows; ++iRow)
		{
			_pGerms [iCol][iRow] = new CGerm (*copyme._pGerms [iCol][iRow]);
		}
	}
}

// Function name   : CDish::operator= ()
// Description     : Copies from one dish to the other.
// Argument        : CDish const & copyme - a reference to the dish being
//					 copied.
// Return type     : CDish & - *this

CDish & CDish::operator= (CDish const & copyme)
{
	//self-assigment?
	if (this == &copyme)
		return *this;

	//empty the old dish
	CleanUp ();

	//make new germs
	for (int iCol (0); iCol < gkiCols; ++iCol)
	{
		for (int iRow (0); iRow < gkiRows; ++iRow)
		{
			_pGerms [iCol][iRow] = new CGerm (*copyme._pGerms [iCol][iRow]);
		}
	}
	//copy the other members
	_iAlive = copyme._iAlive;
	_iDay = copyme._iDay;

	return *this;
}

// Function name   : CDish::~CDish ()
// Description     : DTOR for the CDish class, dumps al DMA memory

CDish::~CDish (void)
{
	CleanUp ();
}

// Function name   : CDish::SetCell ()
// Description     : Sets a cell to the spec'd health.
// Argument        : int iRow - the row of the germ in question
// Argument        : int iCol - the column of the germ in question
// Argument	       : int iHP - set the cells health to this value
// Return type     : CDish & - invoking instance

CDish & CDish::SetCell (int iRow, int iCol, int iHP)
{
	//boundary checking
	if (!CheckRow (iRow))
	{
		//error
		cerr << gkszCDishSetCell << " : " << gkszRowOOR << " : " << iRow<<endl;
		return *this;
	}

	//boundary checking
	if	(!CheckCol (iCol))
	{
		//error
		cerr<<gkszCDishSetCell << " : " << gkszColOOR << " : " << iCol << endl;
		return *this;
	}

	//if the germ is currently dead, we may be "reviving" it
	if (!_pGerms [iCol][iRow]->GetHealth () && iHP)
		++_iAlive;

	//everything is fine, set the health
	_pGerms [iCol][iRow]->SetHealth (iHP);

	return *this;
}

// Function name   : CDish::Live ()
// Description     : Returns the sum of all the germs surrounding the coords
//					 passed to the function.
// Argument        : int iCol - the column to look around
// Argument        : int iRow - the row to look around
// Return type     : int - the total health of the surrounding germs

int CDish::Live (int iCol, int iRow)
{
	//dish is empty?
	if (!_pGerms [0][0])
	{
		cerr << gkszCDishLive << ", " << gkszDishEmpty;
		return 0;
	}

	if (!CheckCol (iCol))
	{
		cerr << gkszCDishLive << " : " << gkszColOOR << " : " << iCol << endl;
		return 0;
	}

	if (!CheckRow (iRow))
	{
		cerr << gkszCDishLive << " : " << gkszRowOOR << " : " << iRow << endl;
		return 0;
	}

	int iHealthSum (0); //the sum of the health of the surrounding cells

	//XXX
	//X X
	//XXX
	//go through all the cells around this germ
	for (int iX (-1); iX <= 1; ++iX)
	{
		for (int iY (-1); iY <= 1; ++iY)
		{
			//is the germ being checked outside the dish?
			//is the germ being checked the germ in the middle?
			if (iX + iCol >= 0 && iX + iCol < gkiCols 
				&& iY + iRow >= 0 && iY + iRow < gkiRows 
				&& !(iX == 0 && iY == 0))
			{
				//germ is valid, add its health to the total
				iHealthSum += _pGerms [iCol + iX][iRow + iY]->GetHealth ();
			}
		}
	}

	return iHealthSum;
}

// Function name   : CDish::Day ()
// Description     : Advances the lives of the germs by one day
// Return type     : int - how many germs are still alive

int CDish::Day ()
{
	//dish is empty?
	if (!_pGerms [0][0])
	{
		cerr << gkszCDishDay << ", " << gkszDishEmpty;
		return 0;
	}

	int iNewHealth [gkiCols][gkiRows] = {{0}}; //contains the health adjustments

	//recalculate the alive count each day
	_iAlive = gkiAliveMax;

	//go through each germ	
	for (int iCol (0); iCol < gkiCols; ++iCol)
	{
		for (int iRow (0); iRow < gkiRows; ++iRow)
		{
			//based on the health of the current germs neighbours,
			//calculate the new health of the current germ
			switch (iNewHealth [iCol][iRow] = Live (iCol, iRow) / 10)
			{
			case 0:
			case 1:
				//dead germ
				iNewHealth [iCol][iRow] = 0;
				//one less germ in the dish
				--_iAlive;
				break;
			case 2:
			case 3:
				//sligtly stronger germ
				++iNewHealth [iCol][iRow];
				break;
			case 4:
			case 5:
			case 6:
				//much stronger germ
				iNewHealth [iCol][iRow] += 2;
				break;
			default: //7-10
				//very strong germ
				iNewHealth [iCol][iRow] = 10;
			}
		}
	}

	//now set the health of all the germs in the dish to the calculated values
	for (int iCol (0); iCol < gkiCols; ++iCol)
		for (int iRow (0); iRow < gkiRows; ++iRow)
			_pGerms [iCol][iRow]->SetHealth (iNewHealth [iCol][iRow]);
	
	++_iDay;

	return _iAlive;
}

// Function name   : CDish::Show () const
// Description     : Displays the dish and some info about the dish
// Return type     : void

void CDish::Show () const
{
	//dish is empty?
	if (!_pGerms [0][0])
	{
		cerr << gkszCDishShow << ", " << gkszDishEmpty;
		return;
	}

	//make each germ show itself
	for (int iRow (0); iRow < gkiRows; ++iRow)
		for (int iCol (0); iCol < gkiCols; ++iCol)
			_pGerms [iCol][iRow]->Show ();
		
	//some info about the dish
	cout << "Day : " << setw (3) << _iDay << ", Alive : " << setw (3) << _iAlive;
}