#ifndef MAIN_H
#define MAIN_H

#include <iostream>
#include <crtdbg.h>

using namespace std;

struct SData
{
	int m_iData;
	SData* m_pLeft;
	SData* m_pRight;
};

char Menu (bool bFullMenu);
void AddNode (SData* &pCurrent, SData* pNew);
void DisplayTree (SData* pCurrent);
void DeleteTree (SData* &Current);
void GetInput (int* const piValue, char const * const szPrompt,
			   int const iLow, int const iHigh);
char GetMenuChoice (char const * const szValidChoices);
void FlushCINBuffer ();

#endif //MAIN_H