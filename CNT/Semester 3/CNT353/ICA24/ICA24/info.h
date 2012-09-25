#pragma once
#include <iostream>
#include "Stack.h"

using namespace std;

template <typename T>
void info (ostream & out, TStack<T> const & stuff)
{
	out << typeid (stuff).name () << endl;
}