//////////////////////////////////////////////////////////////////////////////
// 
// GDI Drawerer - Computer Engineering Technology
// NAIT - www.nait.ca
// Questions/Comments : Simon Walker (simonw@nait.ca)
// Low performance console drawing utility
// Dec 01 2006 : Version 2
//
//////////////////////////////////////////////////////////////////////////////

#pragma once

// how long a mutex lock will be attempted before throwing an exception
int const gciMutexTimeout (1000);

#include <windows.h>
#include <process.h>
#include <list>
using namespace std;

//////////////////////////////////////////////////////////////////////////////
// 
// CShape (Base for all shapes)
//
//////////////////////////////////////////////////////////////////////////////
class CShape
{
	friend class CGDIPDraw;

protected:
	int m_iXS;	// start
	int m_iYS;
	int m_iXE;	// end
	int m_iYE;

	// colour for the shape
	COLORREF m_crColour;

	// line thickness
	unsigned int m_uiThick;

public:

	// management
	CShape (COLORREF cr, unsigned int uiThick, int ixs, int iys, int ixe, int iye)
		: m_crColour (cr), m_uiThick (uiThick), m_iXS (ixs), m_iYS (iys), m_iXE (ixe), m_iYE (iye) { }
	virtual ~CShape (void) { }

	// mutators
	void SetThickness (unsigned int uiThick);
	void SetStart (int ix, int iy);
	void SetEnd (int ix, int iy);
	void SetColour (COLORREF crColour);
	void SetColour (unsigned char ucRed, unsigned char ucGreen, unsigned char ucBlue);

	// accessors
	COLORREF GetColour (void) const;
	int GetStartX (void) const;
	int GetStartY (void) const;
	int GetEndX (void) const;
	int GetEndY (void) const;

	// operations
	// polymorphic rendering function
	virtual void Render (HDC const & rhdc) const = 0;

	// operator == (Coords Logical)
	bool operator== (CShape const & rSource) const
	{
		if (m_iXE == rSource.m_iXE && m_iXS == rSource.m_iXS &&
			m_iYE == rSource.m_iYE && m_iYS == rSource.m_iYS)
			return true;
		return false;
	}	
};

//////////////////////////////////////////////////////////////////////////////
// 
// CEllipse
//
//////////////////////////////////////////////////////////////////////////////
class CEllipse : public CShape
{
protected:
	COLORREF m_crFillColour;

public:
	CEllipse (int iXS, int iYS, int iXE, int iYE)
		: CShape (RGB (255, 0, 0), 1, iXS, iYS, iXE, iYE), m_crFillColour (RGB(255, 255, 255)) { }
	CEllipse (int iXS, int iYS, int iSize, unsigned int uiThick = 1, COLORREF crLine = RGB (128, 128, 255), COLORREF crFill = RGB (255, 255, 255))
		: CShape (crLine, uiThick, iXS, iYS, iXS + iSize, iYS + iSize), m_crFillColour (crFill) { }
	
	void SetFillColour (COLORREF crFill) { m_crFillColour = crFill; }
	COLORREF GetFillColour (void) const { return (m_crFillColour); }

	virtual void Render (HDC const & rhdc) const;
};

//////////////////////////////////////////////////////////////////////////////
// 
// CRectangle
//
//////////////////////////////////////////////////////////////////////////////
class CRectangle : public CShape
{
protected:
	COLORREF m_crFillColour;

public:
	CRectangle (int iXS, int iYS, int iXE, int iYE)
		: CShape (RGB (255, 0, 0), 1, iXS, iYS, iXE, iYE), m_crFillColour (RGB(255, 255, 255)) { }
	CRectangle (int iXS, int iYS, int iSize, unsigned int uiThick = 1, COLORREF crLine = RGB (128, 128, 255), COLORREF crFill = RGB (255, 255, 255))
		: CShape (crLine, uiThick, iXS, iYS, iXS + iSize, iYS + iSize), m_crFillColour (crFill) { }
	
	void SetFillColour (COLORREF crFill) { m_crFillColour = crFill; }
	COLORREF GetFillColour (void) const { return (m_crFillColour); }

	virtual void Render (HDC const & rhdc) const;
};

//////////////////////////////////////////////////////////////////////////////
// 
// CText
//
//////////////////////////////////////////////////////////////////////////////
class CText : public CShape
{
protected:
	wstring m_csText;

public:
	CText (wstring csText)
		: CShape (RGB (128, 128, 128), 20, 0, 0, 800, 600), m_csText (csText) { }
	CText (wstring csText, int iXS, int iYS, int iXE, int iYE, unsigned int uiThick = 1, COLORREF cr = RGB (128, 128, 255))
		: CShape (cr, uiThick, iXS, iYS, iXE, iYE), m_csText (csText) { }
	
	virtual void Render (HDC const & rhdc) const;
};

//////////////////////////////////////////////////////////////////////////////
// 
// CLine
//
//////////////////////////////////////////////////////////////////////////////
class CLine : public CShape
{
public:
	CLine (int iXS, int iYS, int iXE, int iYE, unsigned int uiThick = 1, COLORREF cr = RGB (128, 128, 255))
		: CShape (cr, uiThick, iXS, iYS, iXE, iYE) { }
	
	virtual void Render (HDC const & rhdc) const;
};

//////////////////////////////////////////////////////////////////////////////
// 
// CInversion
//
//////////////////////////////////////////////////////////////////////////////
class CInversion : public CShape
{
public:
	CInversion (int iXS, int iYS, int iXE, int iYE)
		: CShape (RGB (0, 0, 0), 1, iXS, iYS, iXE, iYE) { }
	
	virtual void Render (HDC const & rhdc) const;
};

//////////////////////////////////////////////////////////////////////////////
// 
// CGDIPDraw
//
//////////////////////////////////////////////////////////////////////////////
class CGDIPDraw
{
private:
	// no copies or assignment
	CGDIPDraw (CGDIPDraw const & rSource) { }
	CGDIPDraw & operator= (CGDIPDraw const & rSource) { }

protected:
	// windows handle
	static HWND s_hWnd;

	// windows procedure callback
	static LRESULT CALLBACK sGDIPWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);

	// the one and only this for the static callback to use
	static CGDIPDraw * s_this;
	
	// flag to indicate if redrawing is required for the window
	static bool s_bRedrawFlag;

	// flag to indicate that the window should take itself out
	static bool s_bExit;

	// mutex handle for thread safety
	static HANDLE s_hListMutex;
	
	// list of lines
	list <CShape *> m_lShapes;

	// message loop thread entry point
	static void mloop (void * pIgnore);

public:
	CGDIPDraw(void);
	~CGDIPDraw(void);

	// public interface
	void AddLine (CLine const & rLine);					// add a line to the scene
	void AddText (CText const & rText);					// add text to the scene
	void AddEllipse (CEllipse const & rEllipse);		// add an ellipse to the scene
	void AddRectangle (CRectangle const & rRectangle);	// add a rectangle to the scene
	void AddInversion (CInversion const & rInversion);	// add an inversion to the scene
	void Render (void);									// render the scene
	void Clear (void);									// clear the scene

	// is the provided point found in any shape(s) (bounding box) in the scene?
	bool IsPointInShapeBounds (int iX, int iY) const;
};
