#ifndef MAIN_H
#define MAIN_H

#include <iostream>
#include <string>
#include <iomanip>

using namespace std;

const int kiNumberOfNames = 5;
const int kiSizeOfNames = 10;

void FlushCINBuffer (void);
void GetNames     (char szNames [kiNumberOfNames][kiSizeOfNames]);
void DisplayNames (char szNames [kiNumberOfNames][kiSizeOfNames]);

#endif //MAIN_H