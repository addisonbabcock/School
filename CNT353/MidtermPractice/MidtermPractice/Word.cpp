#include "Word.h"

CWord::CWord (char * szWord) : _szWord (0)
{
	szWord = szWord ? szWord : "";
	_szWord = new char [strlen (szWord) + 1];
	strcpy (_szWord, szWord);
}

CWord::~CWord (void)
{
	delete [] _szWord;
	_szWord = 0;
}

CWord & CWord::operator= (CWord const & rhs)
{
	if (this == &rhs)
		return *this;

	CWord::~CWord ();
	_szWord = new char [strlen (rhs._szWord) + 1];
	strcpy (_szWord, rhs._szWord);
	return *this;
}

CWord::CWord(CWord const & old) : _szWord (0)
{
	*this = old;
}

bool CWord::operator== (CWord const & rhs) const
{
	return !strcmp (_szWord, rhs._szWord);
}

CWord CWord::operator~ () const
{
	//make a copy of this
	CWord newWord (*this);
	int iLen (strlen (newWord._szWord));
	char cTemp (0);

	for (int i (0); i < iLen / 2; ++i)
	{
		cTemp = newWord._szWord [i];
		newWord._szWord [i] = newWord._szWord [iLen - i - 1];
		newWord._szWord [iLen - i - 1] = cTemp;
	}

	return newWord;
}

CWord operator+ (CWord const & lhs, CWord const & rhs)
{
	CWord newWord (0);
	delete [] newWord._szWord;
	newWord._szWord = new char [strlen (lhs._szWord) + strlen (rhs._szWord) + 1];

	strcpy (newWord._szWord, lhs._szWord);
	strcat (newWord._szWord, rhs._szWord);

	return newWord;
}

bool operator!= (CWord const & lhs, CWord const & rhs)
{
	return !(lhs == rhs);
}

CWord & operator+= (CWord & lhs, CWord const & rhs)
{
	return lhs = lhs + rhs;
}