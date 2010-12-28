/***************************************************\
* Project:		Lab 4 - Drawing Thinger				*
* Files:		main.cpp, main.h, utilities.cpp		*
*				utilities.h, CDRrawInterface.h,		*
*				graphics.h, graphics.cpp			*
* Date:			02 Nov 2006							*
* Author:		Addison Babcock		Class:  CNT25	*
* Instructor:	J. D. Silver		Course: CNT252	*
* Description:	A primitive drawing program.		*
\***************************************************/

#include "graphics.h"
#include "utilities.h"
#include "main.h"
#include <cmath>

//All of the functions in this file make use of this
//It was declared global in the demo program, so I don't
//think it will be a problem for it to be global here
CDrawInterface interf;

/***************************************************************************\
| Function: void GetColor (SColor* const sColor)							|
| Description: Prompts the user to enter a 24-bit color value. One byte at	|
|	a time.																	|
| Parameters: An pointer for where to store the values entered.				|
| Returns: None.															|
\***************************************************************************/
void GetColor (SColor* const sColor)
{
	//Get each individual color value...
	cout << "Enter a color... \n";
	sColor->r = GetInput ("Enter a red value: ");
	sColor->g = GetInput ("Enter a green value: ");
	sColor->b = GetInput ("Enter a blue value: ");
	//No alpha channel is used
	sColor->a = 255;
}

/***************************************************************************\
| Function: void GetPoint (SBytePoint* const sPoint)						|
| Description: Prompts the user to enter a coordinate value. One byte at	|
|	a time.																	|
| Parameters: An pointer for where to store the values entered.				|
| Returns: None.															|
\***************************************************************************/
void GetPoint (SBytePoint* const sPoint)
{
	//Get each part of the coord...
	sPoint->m_byX = GetInput ("Enter X coord: ");
	sPoint->m_byY = GetInput ("Enter Y coord: ");
}

/***************************************************************************\
| Function: void GetPoint (SFloatPoint* const sPoint)						|
| Description: Prompts the user to enter a coordinate value. One float at	|
|	a time.																	|
| Parameters: An pointer for where to store the values entered.				|
| Returns: None.															|
\***************************************************************************/
void GetPoint (SFloatPoint* const sPoint)
{
	//These are used to call teh double version of GetInput ()
	double	dX,	dY;

	//Get the x coord
	GetInput (&dX, "Enter X coord: ", 0, 49.9);
	//Get the y coord
	GetInput (&dY, "Enter Y coord: ", 0, 49.9);

	//Convert them to float and store them in the struct
	sPoint->m_fX = static_cast <float> (dX);
	sPoint->m_fY = static_cast <float> (dY);
}

/***************************************************************************\
| Function: void GetLine (SByteLine* const sLine)							|
| Description: Prompts the user to enter 2 coordinate values.				|
| Parameters: An pointer for where to store the values entered.				|
| Returns: None.															|
\***************************************************************************/
void GetLine (SByteLine* const sLine)
{
	//Get each coord set, one at a time
	cout << "Enter the starting point...\n";
	GetPoint (&sLine->m_sStart);

	cout << "\nEnter the ending point...\n";
	GetPoint (&sLine->m_sEnd);
}

/***************************************************************************\
| Function: void GetLine (SFloatLine* const sLine)							|
| Description: Prompts the user to enter a set of coordinate values as well |
|	as a vector.															|
| Parameters: An pointer for where to store the values entered.				|
| Returns: None.															|
\***************************************************************************/
void GetLine (SFloatLine* const sLine)
{
	//temporary values, used to call the double version of GetInput ()
	//instead of making overloading errors
	double dLen, dAngle;

	//Get the starting point
	cout << "Enter the starting point:\n";
	GetPoint (&sLine->m_sStart);

	//Get the vector (length and angle)
	GetInput (&dLen, "Enter the line length: ", 0, sqrt (5000.0));
	sLine->m_fLength = static_cast <float> (dLen);
	GetInput (&dAngle, "Enter the angle: ", 0, 360);
	sLine->m_fAngle = static_cast <float> (dAngle);
}

/***************************************************************************\
| Function: void DrawLine (SByteLine const * const sLine)					|
| Description: Draws a line on the output screen. This is an implementation	|
|	of the Bresenham (sp?) line drawing algorithm using only integer math.	|
|	It does this by replacing the slope, m, with 2 integers, byDenominator	|
|	and byNumerator. This is possible becaus m will always be rational.		|
|	Also, this has been designed to draw all 8 types of lines accurately.	|
| Parameters: An pointer for where to get the 2 coordinates needed to draw	|
|	the line.																|
| Returns: None.															|
\***************************************************************************/
void DrawLine (SByteLine const * const sLine)
{
	// The difference between the x's
	byte byDeltaX = abs(sLine->m_sEnd.m_byX - sLine->m_sStart.m_byX);
	// The difference between the y's
	byte byDeltaY = abs(sLine->m_sEnd.m_byY - sLine->m_sStart.m_byY);
	//The current point to be drawn
	SBytePoint sPt;
	//These help determine which direction to draw the line
	char cXInc1, cXInc2,
		 cYInc1, cYInc2;
	//the denominator of the slope
	byte byDenominator,
	//the numerator of the slope
		 byNumerator,
	//this will be deltax or deltay, whichever is smaller
		 byNumeratorInc,
	//how many pixels need to be drawn
		 byPixelCount;

	// Start x off at the first pixel
	sPt.m_byX = sLine->m_sStart.m_byX;
	// Start y off at the first pixel
	sPt.m_byY = sLine->m_sStart.m_byY;

	//the next two if's are for determining which quadrant the line is in,
	//if the lines starting point is taken as being 0, 0

	//The X values are increasing
	if (sLine->m_sEnd.m_byX >= sLine->m_sStart.m_byX)
		cXInc1 = cXInc2 = 1;
	//The x-values are decreasing
	else
		cXInc1 = cXInc2 = -1;
	
	//The y-values are increasing
	if (sLine->m_sEnd.m_byY >= sLine->m_sStart.m_byY)
		cYInc1 = cYInc2 = 1;
	//The y-values are decreasing
	else
		cYInc1 = cYInc2 = -1;

	//There is at least one x-value for every y-value
	//because there are more x values to be displayed, we will loop through the
	//x axis and vice versa
	if (byDeltaX >= byDeltaY)
	{
		//Don't change the x when numerator >= denominator
		cXInc1 = 0;
		//Don't change the y for every iteration
		//change it only when num >= den
		cYInc2 = 0;
		byDenominator = byDeltaX;
		byNumerator = byDeltaX / 2;
		byNumeratorInc = byDeltaY;
		//Loop through x
		byPixelCount = byDeltaX;
	}
	//There is at least one y-value for every x-value
	else
	{
		//Don't change the x for every iteration
		cXInc2 = 0;
		//Don't change the y when numerator >= denominator
		//change it only when num >= den
		cYInc1 = 0;
		byDenominator = byDeltaY;
		byNumerator = byDeltaY / 2;
		byNumeratorInc = byDeltaX;
		//Loop through y
		byPixelCount = byDeltaY;
	}

	//Loop through each pixel to be drawn
	for (byte byCurPixel = 0; byCurPixel <= byPixelCount; byCurPixel++)
	{
		//Draw the current pixel
		SetPoint (&sPt);
		//Increase the numerator by the top of the fraction
		byNumerator += byNumeratorInc;
		//Check if numerator >= denominator
		if (byNumerator >= byDenominator)
		{
			//if it is, that means that we will need to increment x or y,
			//depending on wether byDeltaY or byDeltaX is bigger, respectively

			//Calculate the new numerator value
			byNumerator -= byDenominator;
			//Change the x as appropriate
			sPt.m_byX += cXInc1;
			//Change the y as appropriate
			sPt.m_byY += cYInc1;
		}
		//Change the x as appropriate
		sPt.m_byX += cXInc2;
		//Change the y as appropriate
		sPt.m_byY += cYInc2;
	}
}

/***************************************************************************\
| Function: void DrawLine (SFloatLine const * const sLine)					|
| Description: Converts the floating point line into a byte type line.		|
|	A little bit of trig is involved, but it's not too bad.	Once the the 	|
|	coords have been converted, it calls the normal DrawLine () routine. 	|
|	The ending x and y values are checked to be in range so the angles don't|
|	get messed up.															|
| Parameters: An pointer for where to get the coordinate and vector needed	|
|	to draw	the line.														|
| Returns: None.															|
\***************************************************************************/
void DrawLine (SFloatLine const * const sLine)
{
	/*
	0,49				49,49
	1,0					  1,1
	|-----------------------|
	|						|
	|						|
	|						|
	|-----------------------|
	0,0					  0,1
	0,0					 49,0
	*/
	//This algorithm just converts the float point and vector
	//to a byte line
	SByteLine sbyLine;
	//The angle in radians
	const float fAngle = sLine->m_fAngle * (3.14158f / 180.0f);
	//a temporary variable
	char cTemp = 0;

	//The starting point can be basically just casted
	sbyLine.m_sStart.m_byX = static_cast<unsigned char> (sLine->m_sStart.m_fX);
	sbyLine.m_sStart.m_byY = static_cast<unsigned char> (sLine->m_sStart.m_fY);

	//Some trig is needed for the ending point, however
	//cos (theta) = adj/hyp
	//cos (theta) * hyp = adj = deltaX
	//deltaX + X1 = X2
	cTemp = round (cos (fAngle) * sLine->m_fLength) 
			+ sbyLine.m_sStart.m_byX;
	//The minimum value of x is 0
	if (cTemp < 0)
		cTemp = 0;
	//the maximum value of x is 49
	if (cTemp > 49)
		cTemp = 49;
	sbyLine.m_sEnd.m_byX = cTemp;

	//sin (theta) = opp / hyp
	//sin (theta) * hyp) = opp = deltaY
	//deltaY + Y1 = Y2
	cTemp = round (sin (fAngle) * sLine->m_fLength) 
			+ sbyLine.m_sStart.m_byY;
	//The minimum value of y is 0
	if (cTemp < 0)
		cTemp = 0;
	//the maximum value of y is 49
	if (cTemp > 49)
		cTemp = 49;
	sbyLine.m_sEnd.m_byY = cTemp;

	//Now that the conversion is done, draw the line
	DrawLine (&sbyLine);
}

/***************************************************************************\
| Function: void Turtle ()													|
| Description: Handles just about everything needed for turtle mode.		|
| Parameters: None.															|
| Returns: None.															|
\***************************************************************************/
void Turtle ()
{
	//The single character user input
	char cInput = 0;
	//Is the pen on the paper?
	bool bPenOn = true;
	//Should we draw a point this turn?
	//Should be set to false if a help menu or something was displayed
	bool bDraw = true;
	//The coords that the pen is currently located at
	//declared static so that the pen position can be saved each time
	static SBytePoint sPenLoc;
	//Should the pen be initialized?
	static bool bFirstRun = true;

	//If this is the first time this code was exec'd
	if (bFirstRun)
	{
		//This is no longer the first run
		bFirstRun = false;
		//Init the pen to 0,0
		sPenLoc.m_byX = 0;
		sPenLoc.m_byY = 0;
	}

	//f for quit
	while ('f' != cInput && 'F' != cInput)
	{
		//Draw a point by default
		bDraw = true;

		//The command prompt
		cout << "Enter a command (h for help)" 
			 << " (" << setw (2) << static_cast <unsigned int> (sPenLoc.m_byX) << "," 
			 << setw (2) << static_cast <unsigned int> (sPenLoc.m_byY) << "): ";
		FlushCINBuffer ();
		cin >> cInput;

		switch (cInput)
		{
		case 'w':
		case 'W':
			//up
			if (sPenLoc.m_byY < 49)
				sPenLoc.m_byY++;
			break;
		case 'e':
		case 'E':
			//up right
			if (sPenLoc.m_byX < 49)
				sPenLoc.m_byX++;
			if (sPenLoc.m_byY < 49)
				sPenLoc.m_byY++;
			break;
		case 'd':
		case 'D':
			//right
			if (sPenLoc.m_byX < 49)
				sPenLoc.m_byX++;
			break;
		case 'c':
		case 'C':
			//down right
			if (sPenLoc.m_byY > 0)
				sPenLoc.m_byY--;
			if (sPenLoc.m_byX < 49)
				sPenLoc.m_byX++;
			break;
		case 'x':
		case 'X':
			//down
			if (sPenLoc.m_byY > 0)
				sPenLoc.m_byY--;
			break;
		case 'z':
		case 'Z':
			//down left
			if (sPenLoc.m_byX > 0)
				sPenLoc.m_byX--;
			if (sPenLoc.m_byY > 0)
				sPenLoc.m_byY--;
			break;
		case 'a':
		case 'A':
			//left
			if (sPenLoc.m_byX > 0)
				sPenLoc.m_byX--;
			break;
		case 'q':
		case 'Q':
			//up left
			if (sPenLoc.m_byY < 49)
				sPenLoc.m_byY++;
			if (sPenLoc.m_byX > 0)
				sPenLoc.m_byX--;
			break;
		case 's':
		case 'S':
			//toggle the pen
			bPenOn = !bPenOn;
			//dont draw during this cycle
			bDraw = false;
			if (bPenOn)
				cout << "The pen is now on.\n";
			else
				cout << "The pen is now off.\n";
			break;
		case 'h':
		case 'H':
			//Display a help menu
			cout << "qwe\na d\nzxc\n move around the screen, one point at a time.\n"
				 << "s toggles the pen.\n"
				 << "h displays this menu.\n"
				 << "f quits turtle mode.\n";
			bDraw = false;
			break;
		case 'f': //quiting is handled by the loop
		case 'F':
			break;
		default:
			cout << "Unrecognized command.\n";
			bDraw = false;
		}

		//if the pen is on and a point should be drawn
		if (bPenOn && bDraw)
			//draw a point!
			SetPoint (&sPenLoc);
	}
}

/***************************************************************************\
| Function: void SetColor (SColor sColor)									|
| Description: A wrapper function for setting the current drawing color for	|
|	the output interface.													|
| Parameters: The RGBA color by value.										|
| Returns: None.															|
\***************************************************************************/
void SetColor (SColor sColor)
{
	interf.SetColor (sColor.r, sColor.g, sColor.b);
}

/***************************************************************************\
| Function: void SetBGColor (SColor sBG)									|
| Description: Sets the background color for the output interface.			|
| Parameters: sBG is the object containing the red green and blue values	|
|	to be used as a background.												|
| Returns: None.															|
\***************************************************************************/
void SetBGColor (SColor sBG)
{
	interf.SetBackroundColor (sBG.r, sBG.g, sBG.b);
}

/***************************************************************************\
| Function: void SetPoint (SBytePoint const * const sPoint)					|
| Description: Places a point on the output interface, makes (0,0) the		|
|	bottom left part of the screen.											|
| Parameters: sPoint is the set of coordinates for where the point should	|
|	be placed.																|
| Returns: None.															|
\***************************************************************************/
void SetPoint (SBytePoint const * const sPoint)
{
	//Reversing the points and inverting the actual x coord
	//so that 0,0 is at the bottom left of the screen, like a normal
	//graph
	if (sPoint->m_byX >= 0 && sPoint->m_byX <= 49 &&
		sPoint->m_byY >= 0 && sPoint->m_byY <= 49)
		interf.SetSpace (static_cast <unsigned int> (49 - sPoint->m_byY), 
						 static_cast <unsigned int> (sPoint->m_byX));
}

/***************************************************************************\
| Function: void Clear (void)												|
| Description: Clears the output interface. All that remains is the			|
|	background color is all thats left.										|
| Parameters: None.															|
| Returns: None.															|
\***************************************************************************/
void Clear (void)
{
	interf.Clear ();
}