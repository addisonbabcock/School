#pragma once
#include <iostream>

using namespace std;

class CWord
{
	char * _szWord;
public:
	CWord (char *);
	~CWord (void);
	CWord (CWord const &);
	CWord & operator= (CWord const &);

	friend ostream & operator<< (ostream & out, CWord const & rhs)
		{	return out << rhs._szWord;	}
	bool operator== (CWord const & rhs) const;
	CWord operator~ () const;
	friend CWord operator+ (CWord const &, CWord const &);
};

bool operator!= (CWord const & lhs, CWord const & rhs);
CWord & operator+= (CWord &, CWord const &);