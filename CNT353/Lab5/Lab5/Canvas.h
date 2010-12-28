#pragma once
#include "GDIPDraw.h"
#include "Base.h"
#include <sstream>
#include <string>

using namespace std;

int const gkiGrowSize (3);

class CCanvas
{
private:
	void Grow (void);
	void CleanUp (void);

protected:
	CBase ** _pShapes;		//Heap based array of CBase pointers
	int _iSize;				//How big the CBase array is
	int _iShapeCount;		//How many CBase pointers the array actually holds
	static CGDIPDraw _draw;	//The drawing interface

public:
	CCanvas (void);
	CCanvas (CCanvas const & old);
	CCanvas & operator = (CCanvas const & old);
	virtual ~CCanvas (void);

	void Show (void) const;
	CCanvas & operator << (CBase * addMe);
	operator string (void) const;
};
