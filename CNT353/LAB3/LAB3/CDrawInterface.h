#pragma once
#include <vector>
#include <windows.h>

class IrrlichtDevice;
struct SColor
{
	unsigned char a,r,g,b;
};

class CDrawInterface
{
	friend DWORD WINAPI ThreadProc(LPVOID);
	friend class CRectangle;

private:
	std::vector<CRectangle*> _rects;
	IrrlichtDevice* _device;
	SColor _bgcolor;
	SColor _drawcolor;
	const unsigned int _uiNumberOfRows;
	const unsigned int _uiNumberOfCols;
	const unsigned int _uiScreenWidth;
	const unsigned int _uiScreenHeight;
	const unsigned int _uiColWidth;
	const unsigned int _uiRowHeight;
	HANDLE _hMutex;
	bool _drawthreadvalid;
	static bool _singletonexists;
	bool _killthread;

	void Draw(void);

public:
	CDrawInterface();
	~CDrawInterface();

	//Interface
	void SetColor(unsigned char ucRed, unsigned char ucGreen, unsigned char ucBlue);
	void SetSpace(unsigned int uiX, unsigned int uiY);
	void Clear(void);
	void SetBackroundColor(unsigned char ucRed, unsigned char ucGreen, unsigned char ucBlue);
};