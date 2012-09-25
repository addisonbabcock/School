/***********************************************************
Project: Lab 03 - Drawing Line Thing w/ Sockets
Files: Form1.h, Form1.cpp, PenSelector.h, PenSelector.cpp,
	GetIPDialog.h, GetIPDialog.cpp, StdAfx.h
Date: 19 Oct 07
Author: Addison Babcock Class: CNT4K
Instructor: Simon Walker Course: CNT453
***********************************************************/
#pragma once
#include "PenSelector.h"
#include "GetIPDialog.h"

namespace LAB2 
{
	//contains all the information to draw a line
	//also the frame for the networking bits
	public value struct SLineSeg
	{
	public:
		System::Drawing::Color m_penColor; //color of the line
		System::Drawing::Point m_startingPoint; //where the line starts
		System::Drawing::Point m_endingPoint; //where the line ends
		int m_iThickness; //how thick the line is
	};

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace System::Net;
	using namespace System::Net::Sockets;
	using namespace System::Collections::Generic;

	delegate void _DelVoidSocket (Socket ^);
	delegate void _DelVoidSoxExc (SocketException ^);

	/// <summary>
	/// Summary for Form1
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	public ref class Form1 : public System::Windows::Forms::Form
	{
	private:

		PenSelector ^ m_hPenSelector; //the dialog for pen settings
		GetIPDialog ^ m_hIPDialog; //the dialog for getting the server ip

		//the list containing line info
		LinkedList <SLineSeg> ^ m_hLines;

		//when a line was last added, was the mouse down?
		bool m_bMouseDown;

		//should the next click position an image?
		bool m_bSendImage;

		//the previous mouse location
		Drawing::Point m_pPrevMouseLoc;

		//the worker sockets for server mode
		LinkedList <Socket ^> ^ m_hWorkerSockets;
		//the listening socket for server mode
		Socket ^ m_hListeningSocket;
		//the working socket for client mode
		Socket ^ m_hClientSocket;

		//receive buffer
		array <unsigned char> ^ m_hRXBuf;

		//Are we in server mode?
		bool m_bServerMode;
		
	public:
		Form1 (void);
	protected:
		~Form1 (void);
	private: 
		System::Windows::Forms::ToolStripMenuItem^  sendFileToolStripMenuItem;
		System::Windows::Forms::OpenFileDialog^  UI_fileDialog;
		System::Windows::Forms::StatusStrip^  UI_StatusStrip;
		System::Windows::Forms::ToolStripStatusLabel^  UI_StatusStrip_PenSize;
		System::Windows::Forms::ToolStripStatusLabel^  UI_StatusStrip_SegmentCount;
		System::Windows::Forms::ToolStripStatusLabel^  UI_StatusStrip_MouseCoords;
		System::Windows::Forms::ToolStripDropDownButton^  UI_StatusStrip_ActionsButton;
		System::Windows::Forms::ToolStripMenuItem^  UI_StatusStrip_ActionButton_Reset;
		System::Windows::Forms::ToolStripStatusLabel^  UI_StatusStrip_SocketState;
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			System::ComponentModel::ComponentResourceManager^  resources = (gcnew System::ComponentModel::ComponentResourceManager(Form1::typeid));
			this->UI_StatusStrip = (gcnew System::Windows::Forms::StatusStrip());
			this->UI_StatusStrip_PenSize = (gcnew System::Windows::Forms::ToolStripStatusLabel());
			this->UI_StatusStrip_SegmentCount = (gcnew System::Windows::Forms::ToolStripStatusLabel());
			this->UI_StatusStrip_MouseCoords = (gcnew System::Windows::Forms::ToolStripStatusLabel());
			this->UI_StatusStrip_ActionsButton = (gcnew System::Windows::Forms::ToolStripDropDownButton());
			this->UI_StatusStrip_ActionButton_Reset = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->sendFileToolStripMenuItem = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->UI_StatusStrip_SocketState = (gcnew System::Windows::Forms::ToolStripStatusLabel());
			this->UI_fileDialog = (gcnew System::Windows::Forms::OpenFileDialog());
			this->UI_StatusStrip->SuspendLayout();
			this->SuspendLayout();
			// 
			// UI_StatusStrip
			// 
			this->UI_StatusStrip->AutoSize = false;
			this->UI_StatusStrip->Items->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(5) {this->UI_StatusStrip_PenSize, 
				this->UI_StatusStrip_SegmentCount, this->UI_StatusStrip_MouseCoords, this->UI_StatusStrip_ActionsButton, this->UI_StatusStrip_SocketState});
			this->UI_StatusStrip->Location = System::Drawing::Point(0, 378);
			this->UI_StatusStrip->Name = L"UI_StatusStrip";
			this->UI_StatusStrip->Size = System::Drawing::Size(425, 22);
			this->UI_StatusStrip->TabIndex = 0;
			this->UI_StatusStrip->Text = L"statusStrip1";
			// 
			// UI_StatusStrip_PenSize
			// 
			this->UI_StatusStrip_PenSize->BackColor = System::Drawing::SystemColors::Control;
			this->UI_StatusStrip_PenSize->BorderSides = static_cast<System::Windows::Forms::ToolStripStatusLabelBorderSides>((((System::Windows::Forms::ToolStripStatusLabelBorderSides::Left | System::Windows::Forms::ToolStripStatusLabelBorderSides::Top) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Right) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Bottom));
			this->UI_StatusStrip_PenSize->BorderStyle = System::Windows::Forms::Border3DStyle::SunkenInner;
			this->UI_StatusStrip_PenSize->Name = L"UI_StatusStrip_PenSize";
			this->UI_StatusStrip_PenSize->Size = System::Drawing::Size(29, 17);
			this->UI_StatusStrip_PenSize->Text = L"Pen";
			this->UI_StatusStrip_PenSize->Click += gcnew System::EventHandler(this, &Form1::UI_StatusStrip_PenSize_Click);
			// 
			// UI_StatusStrip_SegmentCount
			// 
			this->UI_StatusStrip_SegmentCount->BackColor = System::Drawing::SystemColors::Control;
			this->UI_StatusStrip_SegmentCount->BorderSides = static_cast<System::Windows::Forms::ToolStripStatusLabelBorderSides>((((System::Windows::Forms::ToolStripStatusLabelBorderSides::Left | System::Windows::Forms::ToolStripStatusLabelBorderSides::Top) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Right) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Bottom));
			this->UI_StatusStrip_SegmentCount->BorderStyle = System::Windows::Forms::Border3DStyle::SunkenInner;
			this->UI_StatusStrip_SegmentCount->Name = L"UI_StatusStrip_SegmentCount";
			this->UI_StatusStrip_SegmentCount->Size = System::Drawing::Size(85, 17);
			this->UI_StatusStrip_SegmentCount->Text = L"Segment Count";
			// 
			// UI_StatusStrip_MouseCoords
			// 
			this->UI_StatusStrip_MouseCoords->BackColor = System::Drawing::SystemColors::Control;
			this->UI_StatusStrip_MouseCoords->BorderSides = static_cast<System::Windows::Forms::ToolStripStatusLabelBorderSides>((((System::Windows::Forms::ToolStripStatusLabelBorderSides::Left | System::Windows::Forms::ToolStripStatusLabelBorderSides::Top) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Right) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Bottom));
			this->UI_StatusStrip_MouseCoords->BorderStyle = System::Windows::Forms::Border3DStyle::SunkenInner;
			this->UI_StatusStrip_MouseCoords->Name = L"UI_StatusStrip_MouseCoords";
			this->UI_StatusStrip_MouseCoords->Size = System::Drawing::Size(79, 17);
			this->UI_StatusStrip_MouseCoords->Text = L"Mouse Coords";
			// 
			// UI_StatusStrip_ActionsButton
			// 
			this->UI_StatusStrip_ActionsButton->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Text;
			this->UI_StatusStrip_ActionsButton->DropDownItems->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(2) {this->UI_StatusStrip_ActionButton_Reset, 
				this->sendFileToolStripMenuItem});
			this->UI_StatusStrip_ActionsButton->Image = (cli::safe_cast<System::Drawing::Image^  >(resources->GetObject(L"UI_StatusStrip_ActionsButton.Image")));
			this->UI_StatusStrip_ActionsButton->ImageTransparentColor = System::Drawing::Color::Magenta;
			this->UI_StatusStrip_ActionsButton->Name = L"UI_StatusStrip_ActionsButton";
			this->UI_StatusStrip_ActionsButton->Size = System::Drawing::Size(67, 20);
			this->UI_StatusStrip_ActionsButton->Text = L"Actions...";
			// 
			// UI_StatusStrip_ActionButton_Reset
			// 
			this->UI_StatusStrip_ActionButton_Reset->Name = L"UI_StatusStrip_ActionButton_Reset";
			this->UI_StatusStrip_ActionButton_Reset->Size = System::Drawing::Size(140, 22);
			this->UI_StatusStrip_ActionButton_Reset->Text = L"Reset";
			this->UI_StatusStrip_ActionButton_Reset->Click += gcnew System::EventHandler(this, &Form1::UI_StatusStrip_ActionButton_Reset_Click);
			// 
			// sendFileToolStripMenuItem
			// 
			this->sendFileToolStripMenuItem->Name = L"sendFileToolStripMenuItem";
			this->sendFileToolStripMenuItem->Size = System::Drawing::Size(140, 22);
			this->sendFileToolStripMenuItem->Text = L"Send File...";
			this->sendFileToolStripMenuItem->Click += gcnew System::EventHandler(this, &Form1::sendFileToolStripMenuItem_Click);
			// 
			// UI_StatusStrip_SocketState
			// 
			this->UI_StatusStrip_SocketState->BackColor = System::Drawing::SystemColors::Control;
			this->UI_StatusStrip_SocketState->BorderSides = static_cast<System::Windows::Forms::ToolStripStatusLabelBorderSides>((((System::Windows::Forms::ToolStripStatusLabelBorderSides::Left | System::Windows::Forms::ToolStripStatusLabelBorderSides::Top) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Right) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Bottom));
			this->UI_StatusStrip_SocketState->BorderStyle = System::Windows::Forms::Border3DStyle::SunkenInner;
			this->UI_StatusStrip_SocketState->ForeColor = System::Drawing::Color::Red;
			this->UI_StatusStrip_SocketState->Name = L"UI_StatusStrip_SocketState";
			this->UI_StatusStrip_SocketState->Size = System::Drawing::Size(150, 17);
			this->UI_StatusStrip_SocketState->Spring = true;
			this->UI_StatusStrip_SocketState->Text = L"Server";
			// 
			// UI_fileDialog
			// 
			this->UI_fileDialog->FileName = L"openFileDialog1";
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->BackColor = System::Drawing::Color::White;
			this->ClientSize = System::Drawing::Size(425, 400);
			this->Controls->Add(this->UI_StatusStrip);
			this->Name = L"Form1";
			this->Text = L"Lab #3 - Addison Babcock";
			this->Paint += gcnew System::Windows::Forms::PaintEventHandler(this, &Form1::Form1_Paint);
			this->MouseUp += gcnew System::Windows::Forms::MouseEventHandler(this, &Form1::Form1_MouseUp);
			this->KeyPress += gcnew System::Windows::Forms::KeyPressEventHandler(this, &Form1::Form1_KeyPress);
			this->MouseMove += gcnew System::Windows::Forms::MouseEventHandler(this, &Form1::Form1_MouseMove);
			this->MouseDown += gcnew System::Windows::Forms::MouseEventHandler(this, &Form1::Form1_MouseDown);
			this->Load += gcnew System::EventHandler(this, &Form1::Form1_Load);
			this->UI_StatusStrip->ResumeLayout(false);
			this->UI_StatusStrip->PerformLayout();
			this->ResumeLayout(false);

		}
#pragma endregion
	public:
		//Lab 2 delegate callbacks
		void _ColorChanged (void);
	private: 
		//Lab 3
		void _EnterServerMode ();
		void _EnterClientMode ();
		void _KillServerSockets ();
		void _KillClientSockets ();
		Socket ^ _MakeNewSocket ();
		void _SendData (SLineSeg sSeg);

		//Lab 3 socket callbacks
		void _AcceptCallback (IAsyncResult ^ ar);
		void _ReceiveCallback (IAsyncResult ^ ar);
		void _SendCallback (IAsyncResult ^ ar);

		//Lab 3 socket callback callbacks
		void _HandleAccept (Socket ^ hNewSock);
		void _HandleReceive (void);
		void _HandleSend (void);
		
		//Lab 2 event handlers + misc
		void _DrawSegment (SLineSeg sLineSegment);
		System::Void UI_StatusStrip_ActionButton_Reset_Click (
			System::Object^  sender, System::EventArgs^  e);
		System::Void Form1_MouseMove (System::Object^  sender, 
			System::Windows::Forms::MouseEventArgs^  e);
		System::Void Form1_Paint (System::Object^  sender, 
			System::Windows::Forms::PaintEventArgs^  e);
		System::Void UI_StatusStrip_PenSize_Click (System::Object^  sender, 
			System::EventArgs^  e);
		System::Void Form1_MouseUp (System::Object^  sender, 
			System::Windows::Forms::MouseEventArgs^  e);
		System::Void Form1_KeyPress(System::Object^  sender, 
			System::Windows::Forms::KeyPressEventArgs^  e);	
		System::Void Form1_Load(System::Object^  sender, 
			System::EventArgs^  e);
		System::Void sendFileToolStripMenuItem_Click(System::Object^  sender, 
			System::EventArgs^  e);
		System::Void Form1_MouseDown(System::Object^  sender, 
			System::Windows::Forms::MouseEventArgs^  e);
};
}