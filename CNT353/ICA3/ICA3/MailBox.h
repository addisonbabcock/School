#pragma once
#include <iostream>

using namespace std;

int const gkiSize (5);

class CMailBox
{
private:
	int _iNumMsgs;
	char * _szMsgs [gkiSize];

public:
	CMailBox(void);
	CMailBox (CMailBox const &);
	~CMailBox(void);

	bool AddMsg (char const * pNewMsg);
	ostream& ShowMsg (ostream &);
};
