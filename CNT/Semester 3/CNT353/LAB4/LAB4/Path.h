/****************************************************
Project: Lab 04 - Path-O-Matic
Files: Trek.h, Trek.cpp, Path.h, Path.cpp
Date: 23 March 2007
Author: Addison Babcock		Class: CNT2K
Instructor: Herb V.			Course: CNT353
****************************************************/

#pragma once

#include "GDIPDraw.h"
#include "Trek.h"

//Global consts
const int gkiGrowSize (3); //How much the array should grow by each time

//Global strings
char const gkszDistance [] = "Distance travelled: "; //used in CPath::Show ()

//Error strings
char const gkszDupCTrek[]="CPath::operator += : Duplicate CTrek encountered";
	//used in CPath::operator +=

class CPath
{
	CTrek * _pCoords;	//The array of CTreks representing the path
	int _iCurSize;		//How big the array is
	int _iCurPathSize;	//how much of the array is used

	static CGDIPDraw _draw;	//drawing interface

	void CleanUp ();
	void Grow ();

public:
	CPath ();
	CPath (CPath const &);
	~CPath ();
	CPath & operator = (CPath const &);

	void Show () const;
	CPath & operator += (CTrek const &);
	CPath & operator += (CPath const &);
	bool operator == (CPath const &) const;
	operator double () const;

	friend bool operator == (CPath const &, CTrek const &);
};

bool operator != (CPath const &, CTrek const &);
CPath operator + (CPath const &, CPath const &);
bool operator != (CPath const &, CPath const &);