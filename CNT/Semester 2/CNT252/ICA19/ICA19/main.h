#ifndef MAIN_H
#define MAIN_H

#include <iostream>
#include <crtdbg.h>

using namespace std;

struct SData
{
	int m_iX;
	SData* m_pNext;
};

void FlushCINBuffer ();
char Menu (bool bFullMenu);
char GetMenuChoice (char const * const szValidChoices);
void DisplayForward (SData* pCur);
void DisplayReverse (SData* pCur);
void AddToHead (SData** pHead);
void AddToEnd (SData** pHead);
void DeleteList (SData* pHead);

#endif //MAIN_H