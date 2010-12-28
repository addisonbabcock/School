#pragma once
#include "Nand.h"

class CAnd : public CNand
{
public:
	virtual void Latch ();
	virtual string Name () const;
};