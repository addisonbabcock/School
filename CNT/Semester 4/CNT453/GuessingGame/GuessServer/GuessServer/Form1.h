#pragma once
#include "stdlib.h"	//srand () and rand ()
#include <ctime>	//time ()

namespace GuessServer {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace System::Net;
	using namespace System::Net::Sockets;

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
		delegate void _DelVoidSocket (Socket ^);
		delegate void _DelVoidVoid (void);
		delegate void _DelVoidString (String ^);

		array <unsigned char> ^ m_aucRXData;
		array <unsigned char> ^ m_aucTooHigh;
		array <unsigned char> ^ m_aucTooLow;
		array <unsigned char> ^ m_aucJustRight;

		Socket ^ m_hListeningSock;
		Socket ^ m_hWorkSock;
		int m_iAnswer;

	public:
		Form1(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
			m_iAnswer = 0;

		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~Form1()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::ListBox^  UI_ListBox_Log;
	private: System::Windows::Forms::Label^  label1;
	private: System::Windows::Forms::TextBox^  UI_TextBox_Answer;
	private: System::Windows::Forms::Label^  UI_Label_ConnectionStatus;
	protected: 


	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->UI_TextBox_Answer = (gcnew System::Windows::Forms::TextBox());
			this->UI_Label_ConnectionStatus = (gcnew System::Windows::Forms::Label());
			this->UI_ListBox_Log = (gcnew System::Windows::Forms::ListBox());
			this->SuspendLayout();
			// 
			// label1
			// 
			this->label1->AutoSize = true;
			this->label1->Location = System::Drawing::Point(12, 13);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(82, 13);
			this->label1->TabIndex = 0;
			this->label1->Text = L"Current Answer:";
			// 
			// UI_TextBox_Answer
			// 
			this->UI_TextBox_Answer->Location = System::Drawing::Point(101, 10);
			this->UI_TextBox_Answer->Name = L"UI_TextBox_Answer";
			this->UI_TextBox_Answer->ReadOnly = true;
			this->UI_TextBox_Answer->Size = System::Drawing::Size(62, 20);
			this->UI_TextBox_Answer->TabIndex = 1;
			// 
			// UI_Label_ConnectionStatus
			// 
			this->UI_Label_ConnectionStatus->AutoSize = true;
			this->UI_Label_ConnectionStatus->Location = System::Drawing::Point(169, 13);
			this->UI_Label_ConnectionStatus->Name = L"UI_Label_ConnectionStatus";
			this->UI_Label_ConnectionStatus->Size = System::Drawing::Size(73, 13);
			this->UI_Label_ConnectionStatus->TabIndex = 2;
			this->UI_Label_ConnectionStatus->Text = L"Disconnected";
			// 
			// UI_ListBox_Log
			// 
			this->UI_ListBox_Log->FormattingEnabled = true;
			this->UI_ListBox_Log->Location = System::Drawing::Point(15, 36);
			this->UI_ListBox_Log->Name = L"UI_ListBox_Log";
			this->UI_ListBox_Log->ScrollAlwaysVisible = true;
			this->UI_ListBox_Log->Size = System::Drawing::Size(346, 238);
			this->UI_ListBox_Log->TabIndex = 3;
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(373, 284);
			this->Controls->Add(this->UI_ListBox_Log);
			this->Controls->Add(this->UI_Label_ConnectionStatus);
			this->Controls->Add(this->UI_TextBox_Answer);
			this->Controls->Add(this->label1);
			this->Name = L"Form1";
			this->Text = L"Guess Game Server - Addison Babcock";
			this->Load += gcnew System::EventHandler(this, &Form1::Form1_Load);
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion

	private: 
		System::Void Form1_Load(System::Object^  sender, System::EventArgs^  e) 
		{
			m_aucRXData = gcnew array <unsigned char> (sizeof (int));
			m_aucTooHigh = gcnew array <unsigned char> (sizeof (int));
			m_aucTooLow = gcnew array <unsigned char> (sizeof (int));
			m_aucJustRight = gcnew array <unsigned char> (sizeof (int));

			int iTemp (1);

			for (int i (0); i < sizeof (int); ++i)
				m_aucTooHigh [i] = *(((unsigned char *)(&iTemp)) + i);

			iTemp = -1;

			for (int i (0); i < sizeof (int); ++i)
				m_aucTooLow [i] = *(((unsigned char *)(&iTemp)) + i);

			iTemp = 0;

			for (int i (0); i < sizeof (int); ++i)
				m_aucJustRight [i] = *(((unsigned char *)(&iTemp)) + i);

			srand (static_cast <unsigned int> (time (static_cast<time_t> (0))));

			m_hListeningSock = gcnew Socket (
				AddressFamily::InterNetwork,
				SocketType::Stream,
				ProtocolType::Tcp);

			m_hListeningSock->Bind (gcnew IPEndPoint (IPAddress::Any, 1666));

			m_hListeningSock->Listen (5);

			Log ("Listening on port 1666...");

			m_hListeningSock->BeginAccept (gcnew AsyncCallback (this, &Form1::_AcceptCallback),
										   m_hListeningSock);
		}

		void _AcceptCallback (IAsyncResult ^ ar)
		{
			Socket ^ hListeningSock = (Socket ^)ar->AsyncState;

			try
			{
				Socket ^ hNewWorkerSock = hListeningSock->EndAccept (ar);
				Invoke (gcnew _DelVoidSocket (this, &Form1::_HandleAccept), hNewWorkerSock);
			}
			catch (SocketException ^ error)
			{
				Invoke (gcnew _DelVoidString (this, &Form1::Log), error->Message);
			}
		}

		void _ReceiveCallback (IAsyncResult ^ ar)
		{
			Socket ^ hWorkSock = (Socket ^)ar->AsyncState;

			try
			{
				hWorkSock->EndReceive (ar);
				Invoke (gcnew _DelVoidVoid (this, &Form1::_HandleReceive));
			}
			catch (SocketException ^ error)
			{
				Invoke (gcnew _DelVoidString (this, &Form1::Log), error->Message);
			}
		}

		void _HandleAccept (Socket ^ hNewWorkerSock)
		{
			m_hWorkSock = hNewWorkerSock;
			this->UI_Label_ConnectionStatus->Text = "Connected!";
			Log ("Accepted connection!");
			_ResetGame ();

			m_hWorkSock->BeginReceive (
				m_aucRXData, 0, sizeof (int),
				SocketFlags::None,
				gcnew AsyncCallback (this, &Form1::_ReceiveCallback),
				m_hWorkSock);
		}

		void _HandleReceive ()
		{
			int iReceived (0);
			unsigned char * pt = reinterpret_cast <unsigned char*> (&iReceived);

			for (int i (0); i < sizeof (int); ++i)
			{
				*pt = m_aucRXData [i];
				++pt;
			}

			Log ("Client guessed " + iReceived.ToString ());

			/*if (iReceived == 0)
			{
				_ResetGame ();
			}*/

			if (iReceived > m_iAnswer)
			{
				m_hWorkSock->Send (m_aucTooHigh);
			}

			if (iReceived < m_iAnswer && iReceived != 0)
			{
				m_hWorkSock->Send (m_aucTooLow);
			}

			if (iReceived == m_iAnswer)
			{
				m_hWorkSock->Send (m_aucJustRight);
				_ResetGame ();
			}

			m_hWorkSock->BeginReceive (
				m_aucRXData, 0, sizeof (int),
				SocketFlags::None,
				gcnew AsyncCallback (this, &Form1::_ReceiveCallback),
				m_hWorkSock);
		}

		void _ResetGame ()
		{
			m_iAnswer = (rand () % 1000 + 1);
			this->UI_TextBox_Answer->Text = m_iAnswer.ToString ();
			Log ("ResetGame () called");
		}

		void Log (String ^ text)
		{
			this->UI_ListBox_Log->Items->Add (text);
		}
	};
}

