/***********************************************************
Project: Lab 03 - Drawing Line Thing w/ Sockets
Files: Form1.h, Form1.cpp, PenSelector.h, PenSelector.cpp,
	GetIPDialog.h, GetIPDialog.cpp, StdAfx.h
Date: 19 Oct 07
Author: Addison Babcock Class: CNT4K
Instructor: Simon Walker Course: CNT453
***********************************************************/
#include "StdAfx.h"
#include "GetIPDialog.h"

using namespace LAB2;

/***********************************************
Function: GetIPDialog ()
Description: Constructor for the GetIPDialog.
***********************************************/
GetIPDialog::GetIPDialog ()
{
	InitializeComponent ();

	_ConnectClicked = nullptr;
}

/***********************************************
Function: ~GetIPDialog ()
Description: Deallocates the UI components of a
GetIPDialog.
***********************************************/
GetIPDialog::~GetIPDialog ()
{
	if (components)
	{
		delete components;
	}
}

/***********************************************
Function: UI_Button_OK_Click ()
Description: Occurs when the user clicks the OK
buttons to initiate a connection to the server.
Disables the controls for while the connection
attempt is under way and starts the timeout 
timer.
***********************************************/
System::Void GetIPDialog::UI_Button_OK_Click(System::Object^  sender, 
	System::EventArgs^  e)
{
	this->_DisableControls ();

	m_hSocket = gcnew Socket (AddressFamily::InterNetwork, SocketType::Stream,
		ProtocolType::Tcp);

	//no waiting to send
	m_hSocket->NoDelay = true;

	m_hSocket->BeginConnect (p_sipAddress, gkuiDefPortNumber, 
		gcnew AsyncCallback (this, &GetIPDialog::_ConnectCallback), 
		m_hSocket);
}

/***********************************************
Function: UI_Button_Cancel_Click ()
Description: Occurs when the user clicks the 
cancel button. Closes the dialog.
***********************************************/
System::Void GetIPDialog::UI_Button_Cancel_Click(System::Object^  sender, 
												 System::EventArgs^  e)
{
	this->DialogResult = Windows::Forms::DialogResult::Cancel;
}

/***********************************************
Function: GetIPDialog_Activated ()
Description: Automatically gives focus to the
text box for entering an IP address when the 
dialog gains focus.
***********************************************/
System::Void GetIPDialog::GetIPDialog_Activated(System::Object^  sender, 
								   System::EventArgs^  e)
{
	this->UI_TextBox_IpAddress->Focus ();
}

/***********************************************
Function: UI_Timer_ConnectionTimeOut_Tick ()
Description: Automatically occurs after every
tick from the timeout timer. If the timer ticks
hit their maximum, this cancels the attempt and
allows the user to try again.
***********************************************/
System::Void GetIPDialog::UI_Timer_ConnectionTimeOut_Tick(System::Object^  sender, 
	System::EventArgs^  e)
{
	if (this->UI_ProgessBar_ConnectionTimeOut->Value < 
		this->UI_ProgessBar_ConnectionTimeOut->Maximum)
	{
		this->UI_ProgessBar_ConnectionTimeOut->Increment (1);
	}
	else
	{
		this->UI_Timer_ConnectionTimeOut->Enabled = false;
		this->_EnableControls ();
		MessageBox::Show ("Connection failed", "Connection Timeout");
	}
}

/***********************************************
Function: _DisableControls ()
Description: Sets the dialogs controls to 
disabled while a connection attempt is in 
progress. Also starts the timer.
***********************************************/
void GetIPDialog::_DisableControls ()
{
	this->UI_Button_Cancel->Enabled = false;
	this->UI_Button_OK->Enabled = false;
	this->UI_TextBox_IpAddress->Enabled = false;
	this->UI_Timer_ConnectionTimeOut->Enabled = true;
	this->UI_ProgessBar_ConnectionTimeOut->Value = 
		this->UI_ProgessBar_ConnectionTimeOut->Minimum;
}

/***********************************************
Function: _EnableControls ()
Description: Sets the dialogs controls to 
enabled while a connection attempt is not in
progress. Also stops the timer.
***********************************************/
void GetIPDialog::_EnableControls ()
{	
	this->UI_Button_Cancel->Enabled = true;
	this->UI_Button_OK->Enabled = true;
	this->UI_TextBox_IpAddress->Enabled = true;
	this->UI_Timer_ConnectionTimeOut->Enabled = false;
	this->UI_ProgessBar_ConnectionTimeOut->Value = 
		this->UI_ProgessBar_ConnectionTimeOut->Minimum;
}

/***********************************************
Function: _ConnectCallback ()
Description: Asynchronous callback for connecting
a client socket to the server.
***********************************************/
void GetIPDialog::_ConnectCallback (IAsyncResult ^ ar)
{
	Socket ^ hSock = (Socket ^)ar->AsyncState;

	try
	{
		//try to finish the connection
		hSock->EndConnect (ar);
	}
	catch (SocketException ^ e)
	{
		System::Diagnostics::Trace::WriteLine 
			("GetIPDialog::_ConnectCallback::SocEx::EndAccept Failed::" + 
			e->Message);
	}

	//did the connection succeed?
	if (hSock->Connected)
	{
		Invoke (gcnew _DelVoidBool 
			(this, &GetIPDialog::_ConnectComplete), true);
	}
	else
	{
		Invoke (gcnew _DelVoidBool
			(this, &GetIPDialog::_ConnectComplete), false);
	}
}

/***********************************************
Function: _ConnectComplete (bool bSuccess)

Description: Called from _ConnectCallback to 
indicate to the form that a connection attempt
is complete, and it wether it was successful.
This will reenable the controls (to allow another
attempt), close the dialog and show an error
message if appropriate.

Argument: bSuccess is true if the connection 
attempt was successful, false otherwise.
***********************************************/
void GetIPDialog::_ConnectComplete (bool bSuccess)
{
	_EnableControls ();

	//do we have a working connection?
	if (bSuccess)
	{
		//yes, indicate to the main form that things went OK
		this->DialogResult = System::Windows::Forms::DialogResult::OK;
		this->Close ();
	}
	else
	{
		//no, show an error message and tell the main form things went bad
		MessageBox::Show ("Connection Failed", "Connection Timeout");
		this->DialogResult = System::Windows::Forms::DialogResult::Cancel;
		this->Close ();
	}
}

System::Void GetIPDialog::GetIPDialog_FormClosing(System::Object^  sender, 
	System::Windows::Forms::FormClosingEventArgs^  e)
{
	if (e->CloseReason == System::Windows::Forms::CloseReason::UserClosing)
	{
		this->_EnableControls ();
		delete this->m_hSocket;
		this->Close ();
	}
}