#pragma once
#include "Gate.h"

class CNand : public CGate
{
public:
	virtual void Latch ();
	virtual string Name () const;
};