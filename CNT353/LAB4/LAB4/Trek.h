/****************************************************
Project: Lab 04 - Path-O-Matic
Files: Trek.h, Trek.cpp, Path.h, Path.cpp
Date: 23 March 2007
Author: Addison Babcock		Class: CNT2K
Instructor: Herb V.			Course: CNT353
****************************************************/

#pragma once

int const gkiMaxX (799);
int const gkiMaxY (599);
int const gkiYScale (1000);

//Errors
char const gkszX_OOR [] = "CTrek::CTrek : x coord is out of range (0-799) : ";
char const gkszY_OOR [] = "CTrek::CTrek : y coord is out of range (0-599) : ";

class CTrek
{
	int _iX;	//The X coord of a Trek
	int _iY;	//The Y coord of a Trek

public:
	CTrek (double dCoord = 0.0);

	bool operator == (CTrek const &) const;
	bool operator != (CTrek const &) const;

	friend class CPath;
};