#ifndef MAIN_H
#define MAIN_H

#include <iostream>
#include <string>

using namespace std;

int const gkiMaxStrLen = 30;
unsigned int const kuiMaxSize = 5;

struct SStudent
{
	char m_szName [gkiMaxStrLen];
	int m_iGrade;
};

unsigned int InputRecord (SStudent * const);
void DisplayRecord (SStudent const * const, unsigned int);
void DisplayMenu (void);
char GetMenuChoice (char const * const szValidChoices);
void FlushCINBuffer (void);

#endif //MAIN_H