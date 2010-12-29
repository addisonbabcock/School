/***********************************************************
Project: Lab 03 - Drawing Line Thing w/ Sockets
Files: Form1.h, Form1.cpp, PenSelector.h, PenSelector.cpp,
	GetIPDialog.h, GetIPDialog.cpp, StdAfx.h
Date: 19 Oct 07
Author: Addison Babcock Class: CNT4K
Instructor: Simon Walker Course: CNT453
***********************************************************/
#include "stdafx.h"
#include "Form1.h"

using namespace LAB2;

/***********************************************
Function: Form1 ()
Description: Constructor for the Form1 Dialog.
Allocates and inits support dialogs as well.
***********************************************/
Form1::Form1 ()
{
	InitializeComponent ();

	//create the lines list
	m_hLines = gcnew LinkedList <SLineSeg>;
	m_hLines->Clear ();

	//create and init the PenSelector dialog
	m_hPenSelector = gcnew PenSelector;
	m_hPenSelector->colorChanged = gcnew delVoidVoid (
		this, &Form1::_ColorChanged);
	m_hPenSelector->p_iPenAlpha = gkuiDefPenAlpha;
	m_hPenSelector->p_iPenSize = gkuiDefPenSize;
	m_hPenSelector->p_PenColor = Color::Blue;
	m_hPenSelector->Show ();

	//create and init the GetIPDialog
	m_hIPDialog = gcnew GetIPDialog;
	m_hIPDialog->_ConnectClicked = gcnew _DelVoidVoid (
		this, &Form1::_EnterClientMode);

	//not sending an image
	m_bSendImage = false;

	//default into server mode
	m_hWorkerSockets = nullptr;
	m_hListeningSocket = nullptr;
	m_hClientSocket = nullptr;

	//allocate the RX buffer
	this->m_hRXBuf = gcnew array <unsigned char> (sizeof (SLineSeg));
}

/***********************************************
Function: Form1 ()
Description: Deallocates the UI and closes the app
***********************************************/
Form1::~Form1 ()
{
	if (components)
	{
		delete components;
	}
}

/***********************************************
Function: _ColorChanged ()
Description: Called from the PenSelector dialog
to indicate that the user has selected a new.
Updates the status strip to indicate the new
color and pen size.
***********************************************/
void Form1::_ColorChanged (void)
{
	//update the pen status strip
	this->UI_StatusStrip_PenSize->ForeColor = this->m_hPenSelector->p_PenColor;
	this->UI_StatusStrip_PenSize->Text = 
		"Pen (" + this->m_hPenSelector->p_iPenSize + ")";
}

/***********************************************
Function: _EnterServerMode ()
Description: Sets the app into server mode.
Disables all client functionality, then attempts
to start up the server functionality. If the 
server fails to start, this function will fall
back into a fresh client mode.
***********************************************/
void Form1::_EnterServerMode ()
{
	//Show that we are starting server mode
	UI_StatusStrip_SocketState->ForeColor = Color::Red;
	UI_StatusStrip_SocketState->Text = "Server";

	//delete the client socket
	_KillClientSockets ();

	//create the WorkerSockets list
	m_hWorkerSockets = gcnew LinkedList <Socket ^>;

	//create the listening socket and listen for a connection 
	//on port 1666
	try
	{
		m_hListeningSocket = _MakeNewSocket ();
		m_hListeningSocket->Bind 
			(gcnew IPEndPoint (IPAddress::Any, gkuiDefPortNumber));
		m_hListeningSocket->Listen (gkuiConnBackLog); //start backlogging conn.
		m_hListeningSocket->BeginAccept (
			gcnew AsyncCallback (this, &Form1::_AcceptCallback),
			m_hListeningSocket);
	}
	catch (SocketException ^ e)
	{
		System::Diagnostics::Trace::WriteLine 
			("_EnterServerMode::SocEx::Failed to bind port::" + e->Message);
		_EnterClientMode ();
		return;
	}

	//now in server mode
	m_bServerMode = true;
	UI_StatusStrip_SocketState->ForeColor = Color::Green;
	return;
}

/***********************************************
Function: _EnterClientMode ()
Description: Sets the app into client mode.
Disables all server functionality, then attempts
to start up the client functionality. If the 
client fails to start, this function will fall
back into a fresh server mode.
***********************************************/
void Form1::_EnterClientMode ()
{
	//Show that are trying to go into client mode
	UI_StatusStrip_SocketState->ForeColor = Color::Red;
	UI_StatusStrip_SocketState->Text = "Client";

	//leave server mode
	m_bServerMode = false;
	_KillServerSockets ();

	//try to start up a connection
	try
	{
		//show a dialog and connect
		if (m_hIPDialog->ShowDialog () == Windows::Forms::DialogResult::OK)
		{
			//connection succeeded, save the socket and start receiving
			m_hClientSocket = m_hIPDialog->p_hSocket;
			m_hClientSocket->BeginReceive (m_hRXBuf, 0, sizeof (SLineSeg), 
				SocketFlags::None,
				gcnew AsyncCallback (this, &Form1::_ReceiveCallback), 
				m_hClientSocket);
		}
		else
		{
			//connection failed, go fail over into server mode
			_EnterServerMode ();
		}
	}
	catch (SocketException ^ e)
	{
		//client mode failed, fall back into server mode
		System::Diagnostics::Trace::WriteLine 
			("_EnterClientMode::SocEx::Failed to connect::" + e->Message);
		_EnterServerMode ();
	}

	//connection and receive started fine, show a good client state
	UI_StatusStrip_SocketState->ForeColor = Color::Green;
}

/***********************************************
Function: _KillServerSockets ()
Description: Destroys all server side sockets
as well as the list containing the worker sockets
***********************************************/
void Form1::_KillServerSockets ()
{
	//stop listening
	if (m_hListeningSocket)
	{
		delete m_hListeningSocket;
		m_hListeningSocket = nullptr;
	}

	//no list of client connections
	if (!m_hWorkerSockets)
		return;

	//kill all the connections to clients
	for each (Socket ^% rhSox in m_hWorkerSockets)
	{
		delete rhSox;
		rhSox = nullptr;
	}

	//delete the list
	delete m_hWorkerSockets;
	m_hWorkerSockets = nullptr;
}

/***********************************************
Function: _KillClientSockets ()
Description: Destroys the client side socket.
***********************************************/
void Form1::_KillClientSockets ()
{
	//destroy the client
	if (m_hClientSocket)
		m_hClientSocket->Close ();
	delete m_hClientSocket;
	m_hClientSocket = nullptr;
}

/***********************************************
Function: _AcceptCallback (IAsyncResult ^ ar)
Description: Asynchronous Callback for pending
server side connection accepts. Attempts to
finish the connection and calls _HandleAccept
when completed successfully.
***********************************************/
void Form1::_AcceptCallback (IAsyncResult ^ ar)
{
	//same as m_hListeningSocket in the main thread
	Socket^ hListeningSocket = (Socket ^)(ar->AsyncState);

	try
	{
		//get the new connection to a client and send it to the main thread
		Socket ^ hNewSock = hListeningSocket->EndAccept (ar);
		Invoke (gcnew _DelVoidSocket (this, &Form1::_HandleAccept), hNewSock);
	}
	catch (SocketException ^ e)
	{
		System::Diagnostics::Trace::WriteLine
			("_AcceptCallback::SocEx::EndAccept Failed:: " + e->Message);
	}
	catch (ObjectDisposedException ^ e)
	{
		//no big deal if something happens here
		//the server is just trying to accept a connection on a socket
		//thats no longer with us
		System::Diagnostics::Trace::WriteLine
			("_AcceptCallback::ObjDisEx::EndAccept Failed:: " + e->Message);
	}
}

/***********************************************
Function: _ReceiveCallback (IAsyncResult ^ ar)
Description: Asynchronous Callback for pending
receives. Attempts to complete the receive 
ignoring incomplete frames and calling 
_HandleReceive when the frame is completed.
***********************************************/
void Form1::_ReceiveCallback (IAsyncResult ^ ar)
{
	try
	{
		//finish the receive and get the number of bytes received
		int iBytesRX = ((Socket ^)ar->AsyncState)->EndReceive (ar);
		if (iBytesRX == sizeof (SLineSeg))
		{
			Invoke (gcnew _DelVoidVoid (this, &Form1::_HandleReceive));
		}
		else
		{
			System::Diagnostics::Trace::WriteLine 
				("_ReceiveCallback::Received wrong number of bytes");
		}
	}
	catch (SocketException ^ e)
	{
		System::Diagnostics::Trace::WriteLine 
			("_ReceiveCallback::SocEx::Bad Receive:: " + e->Message);
	}
	catch (ObjectDisposedException ^ e)
	{
		System::Diagnostics::Trace::WriteLine 
			("_ReceiveCallback::ObDisEx::Socket was deleted:: " + e->Message);
	}
}

void Form1::_SendCallback (IAsyncResult ^ ar)
{
	Socket ^ hSock = (Socket ^)ar->AsyncState;

	try
	{
		hSock->EndSend (ar);
		Invoke (gcnew _DelVoidVoid (this, &Form1::_HandleSend));
	}
	catch (SocketException ^ e)
	{
	}
}

/***********************************************
Function: _HandleAccept (Socket ^ hNewSock)
Description: Server side. Takes the completed 
and ready to use socket from _AcceptCallback () 
and adds it to the list of sockets.
***********************************************/
void Form1::_HandleAccept (Socket ^ hNewSock)
{
	//add the socket to the list
	this->m_hWorkerSockets->AddLast (hNewSock);

	//send segments asap
	hNewSock->NoDelay = true;

	//start recieving data on the new connection
	hNewSock->BeginReceive (m_hRXBuf, 0, sizeof (SLineSeg), SocketFlags::None,
		gcnew AsyncCallback (this, &Form1::_ReceiveCallback), hNewSock);

	//start listening for another connection
	if (m_hListeningSocket)
		m_hListeningSocket->BeginAccept (
			gcnew AsyncCallback (this, &Form1::_AcceptCallback),
			m_hListeningSocket);
}

/***********************************************
Function: _HandleReceive
Description: This function is called when a 
completed frame has arrived and the receive
buffer requires processing. This function 
unpacks the receive buffer and process the data
as needed. This function also restarts receiving
on all active worker sockets.
***********************************************/
void Form1::_HandleReceive (void)
{
	//this->m_hRXBuf is probably valid, at least the size is anyways
	
	SLineSeg sRXSeg; //holds the unpacked line

	//indexes into the line while its being unpacked
	unsigned char * pT = reinterpret_cast <unsigned char *> (&sRXSeg);

	//unpack the buffer
	for (unsigned int i(0); i < sizeof (SLineSeg); ++i)
	{
		*pT = m_hRXBuf [i];
		pT++;
	}

	//draw the received line
	_DrawSegment (sRXSeg);

	//if we are in server mode, spam it to the clients
	if (m_bServerMode)
	{
		_SendData (sRXSeg);
		//there is no way to tell which socket recieved the data
		//so start up receiving on all of them
		for each (Socket ^% rhSox in m_hWorkerSockets)
		{
			rhSox->BeginReceive (m_hRXBuf, 0, sizeof (SLineSeg), 
				SocketFlags::None, 
				gcnew AsyncCallback (this, &Form1::_ReceiveCallback), rhSox);
		}
	}
	else
	{
		m_hClientSocket->BeginReceive (m_hRXBuf, 0, sizeof (SLineSeg), 
			SocketFlags::None,
			gcnew AsyncCallback (this, &Form1::_ReceiveCallback), 
			m_hClientSocket);
	}

	//add the line to the end of the lsit and update the count
	m_hLines->AddLast (sRXSeg);
	UI_StatusStrip_SegmentCount->Text = "Segment Count: " + 
		m_hLines->Count.ToString ();

	//we are marking the mouse as being up to prevent bogus lines
	m_bMouseDown = false;
}

void Form1::_HandleSend (void)
{
	//nothing to do here really
}

/***********************************************
Function: _MakeNewSocket
Description: This function will build a socket
with the options required by this app.
Returns: A handle to the new socket.
***********************************************/
Socket ^ Form1::_MakeNewSocket ()
{
	//build a socket and send it back
	return gcnew Socket (AddressFamily::InterNetwork,
		SocketType::Stream, ProtocolType::Tcp);
}

/***********************************************
Function: _SendData (SLineSeg sSeg)
Description: This function packs and sends sSeg
to the appropriate sockets. If the app is in 
server mode and dead sockets are discovered,
this function handles the removal of said sockets
***********************************************/
void Form1::_SendData (SLineSeg sSeg)
{
	//holds the packed frame
	array <unsigned char> ^ hTXData = 
		gcnew array <unsigned char> (sizeof (SLineSeg));

	//pack it into hTXData
	for (unsigned int i (0); i < sizeof (SLineSeg); ++i)
		hTXData[i] = * (((unsigned char *)(&sSeg)) + i);

	//if we are in server mode, we need to send this segment
	//to all connected clients
	if (this->m_bServerMode)
	{
		//list to keep track of dead sockets
		LinkedList <Socket ^> ^ hDeadSockets = gcnew LinkedList <Socket ^>;

		//spam the line to each connected client
		for each (Socket ^ % rhSox in m_hWorkerSockets)
		{
			try
			{
				//spam the line
				rhSox->BeginSend (hTXData, 0, sizeof (SLineSeg), SocketFlags::None,
					gcnew AsyncCallback (this, &Form1::_SendCallback), rhSox);
			}
			catch (SocketException ^ e)
			{
				//save the dead socket for later killing
				hDeadSockets->AddFirst (rhSox);

				System::Diagnostics::Trace::WriteLine 
					("_SendData::SocEx::Dead Client:: " + e->Message);
			}
		}
		for each (Socket ^ % rhSox in hDeadSockets)
		{
			//socket is likely disconnected
			//attempt to close it
			try
			{
				rhSox->Close ();
			}
			catch (SocketException ^)
			{
				//we already know this socket is brokenated
				//no need to do any further error checking
			}
			m_hWorkerSockets->Remove (rhSox);
		}
	}
	else //client mode
	{
		//is this test needed?
		if (this->m_hClientSocket)
		{
			try
			{
				//send the frame to the server
				m_hClientSocket->BeginSend (hTXData, 0, sizeof (SLineSeg), SocketFlags::None,
					gcnew AsyncCallback (this, &Form1::_SendCallback), m_hClientSocket);
			}
			catch (SocketException ^ e)
			{
				//not connected to server?
				//flip into server mode
				System::Diagnostics::Trace::WriteLine 
					("_SendData::SocEx::Dead Server:: " + e->Message);
				_EnterServerMode ();
			}
		}
	}
}

/***********************************************
Function: DrawSegment (SLineSeg sLineSegment)
Description: Draws a given line segment to the
main form.
Arguments: sLineSegment is the line that should 
be drawn to the form.
***********************************************/
void Form1::_DrawSegment (SLineSeg sLineSegment)
{
	//the graphics interface
	Graphics ^ hGr (this->CreateGraphics ());

	//Set up the pen using the color and width passed to the function
	Pen ^ hPen = gcnew Pen (sLineSegment.m_penColor, 
		static_cast <float> (sLineSegment.m_iThickness));

	//smooth the ends of the pen
	hPen->SetLineCap (Drawing2D::LineCap::Round,
		Drawing2D::LineCap::Round, Drawing2D::DashCap::Round);

	//finally, add the line
	hGr->DrawLine (hPen, sLineSegment.m_startingPoint, 
		sLineSegment.m_endingPoint);
}

/***********************************************
Function: UI_StatusStrip_ActionButton_Reset_Click
Description: Clears the lines that are saved and
redraws the window.
***********************************************/
System::Void Form1::UI_StatusStrip_ActionButton_Reset_Click (
	System::Object^  sender, System::EventArgs^  e) 
{
	m_hLines->Clear ();
	this->Invalidate ();
}

/***********************************************
Function: Form1_MouseMove
Description: Adds a line to the list if the 
left mouse button is held down. Draws the line
that was added, and updates the status strip 
with the mouse coordinates.
***********************************************/
System::Void Form1::Form1_MouseMove (System::Object^  sender, 
									System::Windows::Forms::MouseEventArgs^  e)
{
	//only add lines if the left mouse button is down
	if (e->Button == System::Windows::Forms::MouseButtons::Left)
	{
		//next mouse movement, we will connect the segment that was
		//just added to the one that will be created
		m_bMouseDown = true;

		//build the new line segment
		SLineSeg newLineSeg;

		//is this the first segment of a line?
		if (m_hLines->Count == 0 || !m_bMouseDown)
		{
			//the first segment will start and end at the mouse coords
			newLineSeg.m_startingPoint = e->Location;
		}
		else
		{
			//the line goes from the end of the last line
			newLineSeg.m_startingPoint = m_pPrevMouseLoc;
		}

		//the the current mouse location
		newLineSeg.m_endingPoint = e->Location;

		//the thickness and color are obtained from the dialog
		newLineSeg.m_iThickness = m_hPenSelector->p_iPenSize;
		newLineSeg.m_penColor = m_hPenSelector->p_PenColor;

		//Now add the line to the screen and list and broadcast if server mode
		//client mode will wait for the line to come back from the server
		if (this->m_bServerMode)
		{
			_DrawSegment (newLineSeg);
			m_hLines->AddLast (newLineSeg);
		}
		_SendData (newLineSeg);
	}
	else
	{
		m_bMouseDown = false;
	}

	//save the current mouse location
	m_pPrevMouseLoc = e->Location;

	//update the status strip
	UI_StatusStrip_SegmentCount->Text = "Segment Count: " + 
		m_hLines->Count.ToString ();
	UI_StatusStrip_MouseCoords->Text = "X:" + e->Location.X.ToString ("0000") +
									   " Y:" + e->Location.Y.ToString ("0000");
}

/***********************************************
Function: Form1_Paint
Description: Redraws all the stored lines to 
the screen.
***********************************************/
System::Void Form1::Form1_Paint (System::Object^  sender, 
								System::Windows::Forms::PaintEventArgs^  e)
{
	//nothing to do if the list is empty
	if (m_hLines->Count <= 1)
	{
		return;
	}

	//points at the segment being drawn
	LinkedList<SLineSeg>::Enumerator segment (m_hLines->GetEnumerator ());

	do
	{
		//draw a line
		_DrawSegment (segment.Current);
	} while (segment.MoveNext ()); //go the next line
}

/***********************************************
Function: UI_StatusStrip_PenSize_Click
Description: Re-opens the penSelector dialog
when the pen size label is clicked.
***********************************************/
System::Void Form1::UI_StatusStrip_PenSize_Click (System::Object^  sender, 
												 System::EventArgs^  e) 
{
	//bring the selector back up and give it focus
	m_hPenSelector->Show ();
	m_hPenSelector->Focus ();
}

/***********************************************
Function: Form1_MouseUp
Description: If the left mouse button is 
released, it will set m_bMouseDown to indicate
that so the next line drawn will be seperate
from the lines previous.
***********************************************/
System::Void Form1::Form1_MouseUp (System::Object^  sender, 
								  System::Windows::Forms::MouseEventArgs^  e)
{
	//mark the mouse button as released
	if (e->Button == System::Windows::Forms::MouseButtons::Left)
	{
		m_bMouseDown = false;
	}
}

/***********************************************
Function: Form1_KeyPress
Description: If the app is in server mode and 
the 'c' button is pressed, this function 
triggers the transition into client mode. If the
app is in client mode and the 's' button is 
pressed, this function triggers the transition
into server mode.
***********************************************/
System::Void Form1::Form1_KeyPress(System::Object^  sender, 
							System::Windows::Forms::KeyPressEventArgs^  e)
{
	//transition to server mode when s is pressed and we are in client mode
	if ((e->KeyChar == 's' || e->KeyChar == 'S') && !m_bServerMode)
		_EnterServerMode ();		

	//transition to client mode when c is pressed and the app is in server mode
	if ((e->KeyChar == 'c' || e->KeyChar == 'C') && m_bServerMode)
	{
		//show a dialog to get an IP and attempt to connect
		_EnterClientMode ();
	}
}

/***********************************************
Function: Form1_Load
Description: Defaults the app into server mode
***********************************************/
System::Void Form1::Form1_Load(System::Object^  sender, 
	System::EventArgs^  e)
{
	//default into server mode
	_EnterServerMode ();
}	

System::Void Form1::sendFileToolStripMenuItem_Click(System::Object^  sender, 
	System::EventArgs^  e)
{
	this->UI_fileDialog->ShowDialog ();

	this->m_bSendImage = true;
}

System::Void Form1::Form1_MouseDown(System::Object^  sender, 
	System::Windows::Forms::MouseEventArgs^  e)
{
	if (e->Button == System::Windows::Forms::MouseButtons::Left && m_bSendImage)
	{
		System::Drawing::Bitmap ^ hImage = gcnew
			System::Drawing::Bitmap (this->UI_fileDialog->FileName);

//		System::Drawing::Bitmap::Bitmap (this-

		SLineSeg sSeg;

		sSeg.m_iThickness = 1;

		for (int x = e->Location.X; x < e->Location.X + hImage->Width; ++x)
		{
			for (int y = e->Location.Y; y < e->Location.Y + hImage->Height; ++y)
			{
				sSeg.m_startingPoint = Drawing::Point (x, y);
				sSeg.m_endingPoint = Drawing::Point (x + 1, y);
				sSeg.m_penColor = hImage->GetPixel (x - e->Location.X, y - e->Location.Y);

				if (m_bServerMode)
				{
					_DrawSegment (sSeg);
					m_hLines->AddLast (sSeg);
				}
				_SendData (sSeg);
			}
		}
	}

	m_bSendImage = false;
}