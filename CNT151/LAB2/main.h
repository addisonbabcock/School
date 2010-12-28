#ifndef MAIN_H
#include <iostream>
#include <iomanip>
#include <cmath>
#include <ctime>
#define MAIN_H

//The maximum and minimum values that can be on
//the lottery ticket
const int kiMinTicketNumber = 1;
const int kiMaxTicketNumber = 20;

void		Instructions ();
int			GetInt (int , int);
inline int	GetRandInt (int, int);
void		GetTicketNumbers (int*, int*, int*, int);
void		Draw (int*, int*, int*, int*);
void		Simulate (const int, const int, const int, const int, const int);
bool		EqualityTest (const int, const int, const int, const int);

#endif