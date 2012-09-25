#include <iomanip>
#include <fstream>
#include "Bit.h"

using namespace std;

// Function name   : CBit::Destroy 
// Description     : Destroys all DMA'd members
// Return type     : void 

void CBit::Destroy ()
{
	delete _pFile;
	delete _pInfo;
	delete [] _pBits;
	_pFile = 0;
	_pInfo = 0;
	_pBits = 0;
	_dwSize = 0;
}

// Function name   : CBit::GetPixel 
// Description     : returns a pointer to the pixel located at the coords
//					 specified by iCol and iRow in the RGBTRIPLE array 
//					 specified by Pixels. No boundary checking is done because
//					 this is a private function.
// Return type     : RGBTRIPLE * - the pointer to the pixel requested
// Argument        : int iCol - the column the pixel is located in
// Argument        : int iRow - the row the pixel is located in
// Argument        : RGBTRIPLE * Pixels - the array containing the pixel

RGBTRIPLE * CBit::GetPixel (int iCol, int iRow, RGBTRIPLE * Pixels) const
{
	//if a bitmap was properly loaded, return the location of the pixel 
	//that was requested
	if (_pFile)
		return Pixels + (iCol * _iWidth) + iRow;
	else
		return 0;
}

// Function name   : CBit::SetPixel
// Description     : Sets a pixel in a given bitmap at a given set of coords
//					 to the Color value.
// Return type     : void
// Argument        : int iCol - the column where the pixel is located
// Argument        : int iRow - the row where the pixel is located
// Argument        : RGBTRIPLE const * Color - the color value to be set
// Argument        : RGBTRIPLE * Pixels - the array containing the bitmap

void CBit::SetPixel (int iCol, int iRow, RGBTRIPLE const * Color, 
					 RGBTRIPLE * Pixels)
{
	//if a bitmap was properly loaded, set the requested pixel
	if (_pFile)
		Pixels [iCol * _iWidth + iRow] = *Color; 
}

// Function name   : CBit::CBit
// Description     : Constructor for the CBit class, loads a bitmap into
//					 memory for further processing. Bitmap must be 24 bit, 
//					 uncompressed and have a width that is a multiple of 4
// Argument        : char const * const szFileName - the file to load

CBit::CBit (char const * const szFileName)
{
	fstream InFile; //the input file object

	//pointers are null by default
	_pFile = 0;
	_pInfo = 0;
	_pBits = 0;

	InFile.open (szFileName, ios::in | ios::binary);
	//did the file open fail?
	if (!InFile.is_open ())
	{
		cerr << gkszFileOpenError << szFileName << endl;
		return;
	}

	//read in the first file header
	_pFile = new BITMAPFILEHEADER;
	InFile.read (reinterpret_cast<char *> (_pFile), sizeof (BITMAPFILEHEADER));

	//wrong number of bytes read
	if (InFile.gcount () != sizeof (BITMAPFILEHEADER))
	{
		cerr << gkszFileHeaderError << gkszBytesReadError;
		Destroy ();
		return;
	}

	//wrong type
	if (_pFile->bfType != 19778)
	{
		cerr << gkszFileHeaderError << gkszBadFileType;
		Destroy ();
		return;
	}

	//file header was fine, read in the info header
	_pInfo = new BITMAPINFOHEADER;
	InFile.read (reinterpret_cast<char *> (_pInfo), sizeof (BITMAPINFOHEADER));

	//wrong number of bytes read
	if (InFile.gcount () != sizeof (BITMAPINFOHEADER))
	{
		cerr << gkszInfoHeaderError << gkszBytesReadError;
		Destroy ();
		return;
	}

	//compression is present
	if (_pInfo->biCompression)
	{
		cerr << gkszInfoHeaderError << gkszCompressionError;
		Destroy ();
		return;
	}

	//width is not a multiple of 4
	if (_pInfo->biWidth % 4)
	{
		cerr << gkszInfoHeaderError << gkszWidthError;
		Destroy ();
		return;
	}

	//bitCount is not 24
	if (_pInfo->biBitCount != 24)
	{
		cerr << gkszInfoHeaderError << gkszBitCountError;
		Destroy ();
		return;
	}

	//set _dwSize to the size of the array we are about to allocate
	_dwSize = _pInfo->biSizeImage ? _pInfo->biSizeImage : 
									_pFile->bfSize - _pFile->bfOffBits;
	_dwSize /= 3;
	_pBits = new RGBTRIPLE [_dwSize];
	InFile.read (reinterpret_cast<char *> (_pBits), _dwSize * 3);

	//finally, set the height and width accordingly
	_iHeight = _pInfo->biHeight;
	_iWidth = _pInfo->biWidth;
}

// Function name   : CBit::~CBit 
// Description     : Destructor for the CBit class, unloads all DMA.

CBit::~CBit ()
{
	Destroy ();
}

// Function name   : CBit::CBit
// Description     : Copy constructor for the CBit class
// Argument        : CBit const & bitmap

CBit::CBit (CBit const & bitmap)
{
	//make sure something is loaded
	if (!bitmap._pFile)
	{
		cerr << gkszNoFileLoadedError;
		_dwSize = 0;
		_iHeight = 0;
		_iWidth = 0;
		_pBits = 0;
		_pInfo = 0;
		_pFile = 0;
		return;
	}

	//the shallow copies
	_dwSize = bitmap._dwSize;
	_iHeight = bitmap._iHeight;
	_iWidth = bitmap._iWidth;

	//alloc mem for the deep copies
	_pBits = new RGBTRIPLE [_dwSize];
	_pFile = new BITMAPFILEHEADER;
	_pInfo = new BITMAPINFOHEADER;

	//perform the deep copies
	for (unsigned int uiPixel(0); uiPixel < _dwSize; ++uiPixel)
		_pBits [uiPixel] = bitmap._pBits [uiPixel];
	*_pFile = *bitmap._pFile;
	*_pInfo = *bitmap._pInfo;
}

// Function name   : CBit::Write 
// Description     : Saves the bitmap object to a file.
// Return type     : void 
// Argument        : char const * const szFileName - name of the target file

void CBit::Write (char const * const szFileName) const
{
	fstream OutFile; //the output file

	//make sure something is loaded
	if (!_pFile)
	{
		cerr << gkszNoFileLoadedError;
		return;
	}

	//attempt to open the file
	OutFile.open (szFileName, ios::out | ios::binary);
	if (!OutFile.is_open ())
	{
		cerr << gkszFileOpenError << szFileName << endl;
		return;
	}

	//barf the bitmap to a file
	OutFile.write (reinterpret_cast<char *> (_pFile), 
		sizeof (BITMAPFILEHEADER));
	OutFile.write (reinterpret_cast<char *> (_pInfo),
		sizeof (BITMAPINFOHEADER));
	OutFile.write (reinterpret_cast<char *> (_pBits), _dwSize * 3);

	OutFile.close ();
}

// Function name   : CBit::Info 
// Description     : 
// Return type     : void 
// Argument        : ostream & out

void CBit::Info (ostream & out) const
{
	//make sure something is loaded
	if (!_pFile)
	{
		cerr << gkszNoFileLoadedError;
		return;
	}

	//some file info
	out << "File Size       : " << _pFile->bfSize << " bytes\n"
		<< "Image Width     : " << _iWidth << '\n'
		<< "Image Height    : " << _iHeight << '\n'
		<< "Bits per Pixel  : " << _pInfo->biBitCount << '\n'
		<< "Image data size : " << _dwSize * 3 << " bytes\n";
}

// Function name   : CBit::Contrast
// Description     : Changes the contrast of the image by btNewContrast
// Return type     : void 
// Argument        : BYTE btNewContrast = 10

void CBit::Contrast (BYTE btNewContrast)
{
	RGBTRIPLE * pCurPixel (0); // a pointer to the pixel thats having its 
							   // contrast adjusted
	BYTE btCurPixelContrast (0); //the contrast of the current pixel

	BYTE btMaxColor (255 - btNewContrast); //the maximum and minimum values
	BYTE btMinColor (btNewContrast);	   //that each color can be before
										   //adding or subtracting will
										   //make that color wrap around
	
	//make sure something is loaded
	if (!_pFile)
	{
		cerr << gkszNoFileLoadedError;
		return;
	}

	//go through each pixel one at a time
	for (int iRow (0); iRow < _iWidth; ++iRow)
	{
		for (int iCol (0); iCol < _iHeight; ++iCol)
		{
			//Get a pixel and determine its contrast
			pCurPixel = GetPixel (iCol, iRow, _pBits);
			btCurPixelContrast = (pCurPixel->rgbtBlue + pCurPixel->rgbtGreen +
								  pCurPixel->rgbtRed) / 3;

			//add to the pixel if its bright
			if (btCurPixelContrast > 128)
			{
				pCurPixel->rgbtBlue = pCurPixel->rgbtBlue > btMaxColor 
					? 255 : pCurPixel->rgbtBlue + btNewContrast;
				pCurPixel->rgbtGreen = pCurPixel->rgbtGreen > btMaxColor
					? 255 : pCurPixel->rgbtGreen + btNewContrast;
				pCurPixel->rgbtRed = pCurPixel->rgbtRed > btMaxColor 
					? 255 : pCurPixel->rgbtRed + btNewContrast;
			}
			//subtract from the pixel if its dark
			else
			{
				pCurPixel->rgbtBlue = pCurPixel->rgbtBlue < btMinColor 
					? 0 : pCurPixel->rgbtBlue - btNewContrast;
				pCurPixel->rgbtGreen = pCurPixel->rgbtGreen < btMinColor
					? 0 : pCurPixel->rgbtGreen - btNewContrast;
				pCurPixel->rgbtRed = pCurPixel->rgbtRed < btMinColor 
					? 0 : pCurPixel->rgbtRed - btNewContrast;
			}
		}
	}
}

// Function name   : CBit::UpsideDown
// Description     : Flips the image upside down
// Return type     : void 

void CBit::UpsideDown ()
{

	//make sure something is loaded
	if (!_pFile)
	{
		cerr << gkszNoFileLoadedError;
		return;
	}

	RGBTRIPLE * pNewImage = new RGBTRIPLE [_dwSize]; //the new pixel data
	RGBTRIPLE * pCurrentPixel (0); //a pointer to the pixel thats being copied

	//go through each pixel one at a time
	for (int iRow (0); iRow < _iWidth; ++iRow)
	{
		for (int iCol (0); iCol < _iHeight; ++iCol)
		{
			//get a pixel from the bottom of the old image and copy it 
			//to the top of the new image
			pCurrentPixel = GetPixel (_iHeight - iCol, iRow, _pBits);
			SetPixel (iCol, iRow, pCurrentPixel, pNewImage);
		}
	}

	//replace the old image with the new one
	delete [] _pBits;
	_pBits = pNewImage;
	pNewImage = 0;
}

// Function name   : CBit::ExtraB
// Description     : Inverts the colors of the image. (Negative)
// Return type     : void 

void CBit::ExtraB ()
{
	//make sure something is loaded
	if (!_pFile)
	{
		cerr << gkszNoFileLoadedError;
		return;
	}

	RGBTRIPLE * pNewImage = new RGBTRIPLE [_dwSize]; //the new pixel data
	RGBTRIPLE * pCurPix (0); //the pixel being worked on

	//go through each pixel one at a time
	for (int iRow (0); iRow < _iWidth; ++iRow)
	{
		for (int iCol (0); iCol < _iHeight; ++iCol)
		{
			//get a pixel to work on
			pCurPix = GetPixel (iCol, iRow, _pBits);

			//invert it
			pCurPix->rgbtBlue  = 255 - pCurPix->rgbtBlue;
			pCurPix->rgbtGreen = 255 - pCurPix->rgbtGreen;
			pCurPix->rgbtRed   = 255 - pCurPix->rgbtRed;

			//and save it in the new data
			SetPixel (iCol, iRow, pCurPix, pNewImage);
		}
	}

	//replace the old image with the new one
	delete [] _pBits;
	_pBits = pNewImage;
	pNewImage = 0;
}

// Function name   : CBit::ExtraA
// Description     : Changes the image to black and white
// Return type     : void 

void CBit::ExtraA ()
{
	BYTE btAverage (0); //the average value of a pixel
	RGBTRIPLE * pCurPix (0); //the pixel being worked on

	//make sure something is loaded
	if (!_pFile)
	{
		cerr << gkszNoFileLoadedError;
		return;
	}

	//go through each pixel one at a time
	for (int iRow (0); iRow < _iWidth; ++iRow)
	{
		for (int iCol (0); iCol < _iHeight; ++iCol)
		{
			//get a pixel to work on
			pCurPix = GetPixel (iCol, iRow, _pBits);
			
			//set each color to the average value
			btAverage = (pCurPix->rgbtBlue + pCurPix->rgbtGreen + 
						 pCurPix->rgbtRed) / 3;
			pCurPix->rgbtBlue = pCurPix->rgbtGreen = pCurPix->rgbtRed 
				= btAverage;
		}
	}
}