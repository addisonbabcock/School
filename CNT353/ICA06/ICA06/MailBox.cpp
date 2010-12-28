#include "MailBox.h"

int CMailBox::_iTotalSize (0);

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
			_iTotalSize += strlen (cpy._szMsgs [i]) + 1;
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
		if (i < _iNumMsgs)
			_iTotalSize -= strlen (_szMsgs [i]) + 1;
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
		_iTotalSize += strlen (pNewMsg) + 1;
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

CMailBox& CMailBox::PutMsg (char const * pNewMsg)
{
	AddMsg (pNewMsg);
	return *this;
}

ostream& CMailBox::Size (ostream & out) const
{
	int iSize (0);
	for (int i (0); i < gkiSize; ++i)
	{
		if (_szMsgs [i])
		{
			iSize += strlen (_szMsgs [i]) + 1;
		}
	}
	
	return out << '[' << iSize << ']';
}

int CMailBox::GetTotalSize (void)
{
	return _iTotalSize;
}