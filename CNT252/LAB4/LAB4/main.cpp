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

#include "CDrawInterface.h"
#include "graphics.h"
#include "utilities.h"
#include <iostream>
#include <crtdbg.h>
#include <ctime>
#include "main.h"

using namespace std;

/***************************************************************************\
| Function: void Menu ()													|
| Description: Displays the main menu screen on the command line.			|
| Parameters: None.															|
| Returns: None.															|
\***************************************************************************/
void Menu ()
{
	//dis ist das menu
	cout << "\t\t\tDrawing thinger\n\n"
		 << "a) Set the current color\n"
		 << "b) Set the background color\n"
		 << "c) Clear the screen\n"
		 << "d) Set a single pixel\n"
		 << "e) Draw a line\n"
		 << "f) Enter turtle mode\n"
		 << "h) Display a help menu\n"
		 << "x) Exit\n";
}

/***************************************************************************\
| Function: void Menu ()													|
| Description: Displays the help screen on the command line. Well,			|
|	supposedly anyways.														|
| Parameters: None.															|
| Returns: None.															|
\***************************************************************************/
void Help ()
{
	//clear the screen and display a rather unhelpful message
	//TODO: put something useful here
	system ("cls");
	cout << "\t\tHelp!!\n\n"
		 << "Your problems will be solved later.\n";
	system ("pause");
}

int main(void)
{
	//the current foreground color
	SColor sFGColor = {0, 0, 0, 0};
	//the background color
	SColor sBGColor = {0, 0, 0, 255};
	//keep drawing?
	bool bContinue = true;
	//variables to store the users input
	char cInput = 0,
		 cInput2 = 0;
	//when setting points, just use the first half of these two:
	//the coordinates for a byte-type line
	//{{x1, y}, {x1, y2}}
	SByteLine sByteCoords = {{6, 2}, {1, 1}};
	//the coordinates for a float-type line
	//{{x, y}, angle, length}
	SFloatLine sFloatVector = {{25,25},0,50};

	SetBGColor (sBGColor);
	//Force the game engine to init
	//Setting the fg color to be the bg color so no odd point shows up
	//when the program first starts
	SetColor (sBGColor);
	SetPoint (&sByteCoords.m_sStart);
	Clear ();
	//put the color back to normal
	SetColor (sFGColor);

	for (float fO = 0.0f; fO <= 360.0f; fO += 45.0f)
	{
		sFloatVector.m_fAngle = fO;
		DrawLine (&sFloatVector);
	}

	//until an 'x' or 'X' is entered...
	while (bContinue)
	{
		//clear the interface
		system ("cls");
		//show a menu
		Menu ();
		//get the users input
		cout << "Your input please: ";
		cInput = toupper (GetMenuChoice ("abcdefhxABCDEFHX"));

		switch (cInput)
		{
		case 'A':
			//Set foreground color
			GetColor (&sFGColor);
			SetColor (sFGColor);
			break;
		case 'B':
			//Set background color
			GetColor (&sBGColor);
			SetBGColor (sBGColor);
			break;
		case 'C':
			//Clear the output screen
			Clear ();
			break;
		case 'D':
			//Set a point
			cout << "How would you like to enter your point?\n"
				 << "a) an integer value\n"
				 << "b) a floating point value\n"
				 << "Your input please: ";
			cInput2 = toupper (GetMenuChoice ("aAbB"));

			switch (cInput2)
			{
			case 'A':
				//set a byte point
				GetPoint (&sByteCoords.m_sStart);
				SetPoint (&sByteCoords.m_sStart);
				break;
			case 'B':
				//set a float point

				//get the point as floats
				GetPoint (&sFloatVector.m_sStart);
				//convert the x to byte
				sByteCoords.m_sStart.m_byX = 
					static_cast <unsigned char> (sFloatVector.m_sStart.m_fX);
				//convert the y to byte
				sByteCoords.m_sStart.m_byY = 
					static_cast <unsigned char> (sFloatVector.m_sStart.m_fY);
				//draw the point using the bytes
				SetPoint (&sByteCoords.m_sStart);
				break;
			}
			break;
		case 'E':
			//draw a line
			cout << "How would you like to enter your line?\n"
				 << "a) 2 sets of coordinates\n"
				 << "b) 1 set of coordinates and a vector\n"
				 << "Your input please: ";
			cInput2 = toupper (GetMenuChoice ("aAbB"));

			switch (cInput2)
			{
			case 'A':
				//byte line:
				GetLine (&sByteCoords);
				DrawLine (&sByteCoords);
				break;
			case 'B':
				//float line:
				GetLine (&sFloatVector);
				DrawLine (&sFloatVector);
				break;
			}
			break;
		case 'F':
			//enter turtle mode
			Turtle ();
			break;
		case 'H':
			//show the help menu
			Help ();
			break;
		case 'X':
			//exit
			bContinue = false;
			break;
		}
	}

	cout << endl;
	system ("pause");
	return 0;
}