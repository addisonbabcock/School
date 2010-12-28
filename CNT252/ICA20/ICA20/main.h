#ifndef MAIN_H
#define MAIN_H

#include <iostream>
#include <crtdbg.h>

using namespace std;

struct SData
{
	int m_iX;
	int m_iF;
	SData* m_pNext;
};

void FlushCINBuffer ();
void GetInput (int* const piValue, char const * const szPrompt,
			   int const iLow, int const iHigh);
char Menu (bool bFullMenu);
char GetMenuChoice (char const * const szValidChoices);
void DisplayForward (SData* pCur);
void DisplayReverse (SData* pCur);
SData* FindNode (SData* pHead, int iQuery);
SData* FindNodePrior (SData* pHead, int iQuery);
void DeleteNode (SData* &pHead, int iData);
void AddNode (SData* &pHead, int iData);
void DeleteList (SData* pHead);

#endif //MAIN_H