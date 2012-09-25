#ifndef GETINPUT
#define GETINPUT

#include <iostream>
#include <float.h>

using namespace std;

void GetInput (int* const piValue, char const * const szPrompt,
			   int const iLow = INT_MIN, int const iHigh = INT_MAX);
void GetInput (double* const pdValue, char const * const szPrompt,
			   double const dLow = -DBL_MAX, double const dHigh = DBL_MAX);
void Flush ();

#endif //GETINPUT