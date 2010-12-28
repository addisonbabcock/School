#pragma once
#include "base.h"

//Error Messages
char const gkszBadString [] = "CTextString:CTextString (int, int, char const * : szText is invalid";

class CTextString : public CBase
{
	char * _szText;

public:
	CTextString (int iX, int iY, char const * szText);
	virtual ~CTextString(void);
	CTextString (CTextString const &);
	CTextString & operator = (CTextString const & rhs);

	virtual void Draw (CGDIPDraw & draw);
	CBase * Clone () const;
};
