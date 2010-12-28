#include "MailBox.h"

CMailBox::CMailBox(void) : _meter (0, 0, gkiSize), _iNumMsgs (0)
{
	for (int i (0); i < gkiSize; ++i)
		_szMsgs [i] = 0;
}

CMailBox::CMailBox (CMailBox const & cpy) : _meter (cpy._meter), _iNumMsgs (cpy._iNumMsgs)
{
	for (int i (0); i < gkiSize; ++i)
	{
		if (cpy._szMsgs [i])
		{
			_szMsgs [i] = new char [strlen (cpy._szMsgs [i]) + 1];
			strcpy (_szMsgs [i], cpy._szMsgs [i]);
		}
		else
		{
			_szMsgs [i] = 0;
		}
	}
}

CMailBox::~CMailBox(void)
{
	for (int i (0); i < gkiSize; ++i)
	{
		delete [] _szMsgs [i];
		_szMsgs [i] = 0;
	}
	_iNumMsgs = 0;
}

bool CMailBox::AddMsg (char const * pNewMsg)
{
	if (_iNumMsgs < gkiSize)
	{
		pNewMsg = pNewMsg ? pNewMsg : "";
		_szMsgs [_iNumMsgs] = new char [strlen (pNewMsg) + 1];
		strcpy (_szMsgs [_iNumMsgs], pNewMsg);
		++_iNumMsgs;
		_meter.Step ();
		return true;
	}
	else
		return false;
}

ostream & CMailBox::ShowMsg (ostream & out) const
{
	_meter.Display (out);

	for (int i(0); i < gkiSize; ++i)
	{
		out << '[' << i << "] : ";
		if (_szMsgs [i])
		{
			out << _szMsgs [i] << endl;
		}
		else
			out << "[EMPTY]\n";
	}

	return out;
}