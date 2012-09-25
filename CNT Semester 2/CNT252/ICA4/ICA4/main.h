#ifndef MAIN_H
#define MAIN_H

#include <iostream>
#include <string>

using namespace std;

int const gkiMaxStrLen = 30;

struct SStudent
{
	char m_szName [gkiMaxStrLen];
	int m_iGrade;
};

void InputRecord (SStudent * const);
void DisplayRecord (SStudent const * const);
void DisplayMenu (void);
char GetMenuChoice (char const * const szValidChoices);
void FlushCINBuffer (void);

#endif //MAIN_H