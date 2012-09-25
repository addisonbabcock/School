#pragma once
#include "Gate.h"

class CXor : public CGate
{
public:
	virtual void Latch ();
	virtual string Name () const;
};