#pragma once
#include <iostream>
#include "Meter.h"

using namespace std;

int const gkiSize (5);

class CMailBox
{
private:
	CMeter _meter;
	int _iNumMsgs;
	char * _szMsgs [gkiSize];

public:
	CMailBox(void);
	CMailBox (CMailBox const &);
	~CMailBox(void);

	bool AddMsg (char const * pNewMsg);
	ostream& ShowMsg (ostream &) const;
	CMailBox& PutMsg (char const * pNewMsg);
	ostream& Size (ostream & out) const;
};
