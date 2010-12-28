#pragma once

#include "DArray.h"

class CDDArray : public CDArray
{
protected:
	//no new members
public:
	CDDArray (int iSize, double dInit);
	~CDDArray ();

	double Sum () const;
	double Avg () const;
};