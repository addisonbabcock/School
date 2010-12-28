#ifndef UTILITIES_H
#define UTILITIES_H

#include <iostream>
using namespace std;

int GetInt (int, int);
double GetDouble (double, double);

template <typename Type>
Type GetValue (Type LowerBound, Type UpperBound);

#endif //UTILITIES_H