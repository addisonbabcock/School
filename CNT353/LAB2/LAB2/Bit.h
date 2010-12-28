#pragma once
#include <windows.h>
#include <iostream>

using namespace std;

char const gkszFileHeaderError [] = "Reading file header: ";
char const gkszInfoHeaderError [] = "Reading info header: ";
char const gkszBytesReadError [] = "Wrong number of bytes read.\n";
char const gkszBadFileType [] = "Wrong file type.\n";
char const gkszCompressionError [] = "Compression is enabled.\n";
char const gkszWidthError [] = "Width field is not a multiple of 4 bytes.\n";
char const gkszBitCountError [] = "bitCount is not 24.\n";
char const gkszFileOpenError [] = "Could not open file: ";
char const gkszNoFileLoadedError [] = "No file is loaded.\n";

class CBit
{
private:
	BITMAPFILEHEADER * _pFile;
	BITMAPINFOHEADER * _pInfo;
	RGBTRIPLE * _pBits;
	DWORD _dwSize;
	int _iHeight;
	int _iWidth;

	void Destroy ();
	RGBTRIPLE * GetPixel (int iCol, int iRow, RGBTRIPLE * Pixels) const;
	void SetPixel (int iCol, int iRow, RGBTRIPLE const * Color, RGBTRIPLE * Pixels);

public:
	CBit (char const * const szFileName);
	~CBit ();
	CBit (CBit const & bitmap);

	void Write (char const * const szFileName = "temp.bmp") const;
	void Info (ostream & out = cout) const;

	void Contrast (BYTE btNewContrast = 10);
	void UpsideDown ();
	void ExtraA ();
	void ExtraB ();
};