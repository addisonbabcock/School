#ifndef MAIN_H
#define MAIN_H

void FlushCINBuffer (void);
void GetInput (int* const piValue, char const * const szPrompt,
			   int const iLow, int const iHigh);
char GetMenuChoice (char const * const szValidChoices);
char Menu (bool bFullMenu);
void AddToArray (int** ppiData, int* piMaxSize);
void DeleteFromArray (int** ppiData, int* iSize);
void DisplayArray (const int iArray [], const int iSize);

#endif //MAIN_H