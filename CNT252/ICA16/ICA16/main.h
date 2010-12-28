#ifndef MAIN_H
#define MAIN_H

#include <iostream>
#include <string>
#include <fstream>

using namespace std;

int const gkiMaxStrLen = 30;
unsigned int const kuiMaxSize = 10;

struct SStudent
{
	char m_szName [gkiMaxStrLen];
	int m_iGrade;
};

unsigned int InputRecord (SStudent * const);
void DisplayRecord (SStudent const * const, unsigned int const);
void DisplayMenu (unsigned int const uiStudentCount);
char GetMenuChoice (char const * const szValidChoices);
void FlushCINBuffer (void);
void SaveClass (SStudent const * const Students, unsigned int const StudentCount);
unsigned int LoadClass (SStudent const * const Students);
int QuickSort (SStudent * const Students, int iLeft, int iRight, bool bIsTop = true);
int StudentCompare (void const * sLeft, void const * sRight);


#endif //MAIN_H