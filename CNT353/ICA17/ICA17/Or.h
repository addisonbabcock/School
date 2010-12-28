#pragma once
#include "Gate.h"

class COr : public CGate
{
public:
	virtual void Latch ();
	virtual string Name () const;
};

