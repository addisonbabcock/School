/***********************************************************
Project: Lab 03 - Drawing Line Thing w/ Sockets
Files: Form1.h, Form1.cpp, PenSelector.h, PenSelector.cpp,
	GetIPDialog.h, GetIPDialog.cpp, StdAfx.h
Date: 19 Oct 07
Author: Addison Babcock Class: CNT4K
Instructor: Simon Walker Course: CNT453
***********************************************************/
#include "StdAfx.h"
#include "PenSelector.h"

using namespace LAB2;

/***********************************************
Function: PenSelector(void)
Description: Initializes a PenSelector dialog
Parameters: void
Returns: nothing
***********************************************/
PenSelector::PenSelector (void)
{
	InitializeComponent ();

	//Populate the domain for the pen size
	for (int i (256); i >= 1; --i)
		UI_Domain_PenSize->Items->Add (i);
	UI_Domain_PenSize->SelectedIndex = gkuiDefPenSize;

	//Populate the domain for the pen alpha
	for (int i (255); i >= 0; --i)
		UI_Domain_PenAlpha->Items->Add (i);
	UI_Domain_PenAlpha->SelectedIndex = gkuiDefPenAlpha;

	p_PenColor = Color::Blue;

	//Set up the delegates
	colorChanged = nullptr;
}

/***********************************************
Function: ~PenSelector
Description: Deallocates the UI components for
a PenSelector dialog.
***********************************************/
PenSelector::~PenSelector ()
{
	if (components)
	{
		delete components;
	}
}

/***********************************************
Function: UI_Label_CurrentColourDisplay_Click
Description: Event handler for when the label
for displaying the color is clicked. Opens a 
color picker dialog and saves the selected
color.
***********************************************/
System::Void PenSelector::UI_Label_CurrentColourDisplay_Click (
	System::Object^  sender, System::EventArgs^  e) 
{
	//set the default value of the color picker to be the currently
	//selected color, then get and save a new color from the dialog
	colorPicker->Color = p_PenColor;
	colorPicker->ShowDialog ();
	p_PenColor = Color::FromArgb (p_iPenAlpha, colorPicker->Color.R,
						 colorPicker->Color.G, colorPicker->Color.B);
}

/***********************************************
Function: UI_Domain_PenAlpha_TextChanged
Description: Event handler to allow changes in 
the text of the UI_Domain_PenAlpha control to be
reflected in p_PenColor.
***********************************************/
System::Void PenSelector::UI_Domain_PenAlpha_TextChanged (
	System::Object^  sender, System::EventArgs^  e) 
{
	//rebuild the color and save it
	System::Drawing::Color oldColor = p_PenColor;
	p_PenColor = Color::FromArgb (p_iPenAlpha, oldColor.R, 
								  oldColor.G, oldColor.B);
}

/***********************************************
Function: PenSelector_FormClosing
Description: Occurs when the user attempts to 
close the form. This function cancels the close
and hides the form instead so all the data is 
still valid.
***********************************************/
System::Void PenSelector::PenSelector_FormClosing (System::Object^  sender, 
	System::Windows::Forms::FormClosingEventArgs^  e) 
{
	//prevent the user from closing the window, just hide it
	if (e->CloseReason == System::Windows::Forms::CloseReason::UserClosing)
	{
		Hide ();
		e->Cancel = true;
	}
}