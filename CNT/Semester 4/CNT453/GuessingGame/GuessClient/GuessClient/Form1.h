#pragma once


namespace GuessClient {

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
		array <unsigned char> ^ m_aucRXData;
		array <unsigned char> ^ m_aucTXData;
		array <unsigned char> ^ m_aucResetSignal;

	private: System::Windows::Forms::ListBox^  UI_ListBox_Log;
	private: System::Windows::Forms::Label^  UI_Label_LowerRange;
	private: System::Windows::Forms::Label^  UI_Label_UpperRange;


		delegate void _DelVoidBool (bool);
		delegate void _DelVoidVoid (void);
		delegate void _DelVoidString (String ^);
		Socket ^ m_hSock;

	public:
		Form1(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//

			int iTemp (0);	//Holds the reset signal until it is placed in the array
			m_aucTXData = gcnew array <unsigned char> (sizeof (int));
			m_aucRXData = gcnew array <unsigned char> (sizeof (int));
			m_aucResetSignal = gcnew array <unsigned char> (sizeof (int));

			//build the array to contain the reset signal
			for (int i (0); i < sizeof (int); ++i)
				m_aucResetSignal [i] = *(((unsigned char *)(&iTemp)) + i);
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
	private: System::Windows::Forms::GroupBox^  UI_GB_Conn;
	private: System::Windows::Forms::TextBox^  UI_TextBox_ServerAddr;

	private: System::Windows::Forms::Button^  UI_Button_Connect;
	private: System::Windows::Forms::Label^  label2;
	private: System::Windows::Forms::Label^  label1;
	private: System::Windows::Forms::TextBox^  UI_TextBox_Port;

	private: System::Windows::Forms::TrackBar^  UI_TrackBar_Guess;
	private: System::Windows::Forms::Label^  UI_Label_Guess;


	private: System::Windows::Forms::TextBox^  UI_TextBox_Guess;

	private: System::Windows::Forms::Button^  UI_Button_Guess;
	private: System::Windows::Forms::Label^  UI_Label_ConnectResult;
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
			this->UI_GB_Conn = (gcnew System::Windows::Forms::GroupBox());
			this->UI_Label_ConnectResult = (gcnew System::Windows::Forms::Label());
			this->UI_Button_Connect = (gcnew System::Windows::Forms::Button());
			this->label2 = (gcnew System::Windows::Forms::Label());
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->UI_TextBox_Port = (gcnew System::Windows::Forms::TextBox());
			this->UI_TextBox_ServerAddr = (gcnew System::Windows::Forms::TextBox());
			this->UI_TrackBar_Guess = (gcnew System::Windows::Forms::TrackBar());
			this->UI_Label_Guess = (gcnew System::Windows::Forms::Label());
			this->UI_TextBox_Guess = (gcnew System::Windows::Forms::TextBox());
			this->UI_Button_Guess = (gcnew System::Windows::Forms::Button());
			this->UI_ListBox_Log = (gcnew System::Windows::Forms::ListBox());
			this->UI_Label_LowerRange = (gcnew System::Windows::Forms::Label());
			this->UI_Label_UpperRange = (gcnew System::Windows::Forms::Label());
			this->UI_GB_Conn->SuspendLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->UI_TrackBar_Guess))->BeginInit();
			this->SuspendLayout();
			// 
			// UI_GB_Conn
			// 
			this->UI_GB_Conn->Controls->Add(this->UI_Label_ConnectResult);
			this->UI_GB_Conn->Controls->Add(this->UI_Button_Connect);
			this->UI_GB_Conn->Controls->Add(this->label2);
			this->UI_GB_Conn->Controls->Add(this->label1);
			this->UI_GB_Conn->Controls->Add(this->UI_TextBox_Port);
			this->UI_GB_Conn->Controls->Add(this->UI_TextBox_ServerAddr);
			this->UI_GB_Conn->Location = System::Drawing::Point(13, 13);
			this->UI_GB_Conn->Name = L"UI_GB_Conn";
			this->UI_GB_Conn->Size = System::Drawing::Size(319, 69);
			this->UI_GB_Conn->TabIndex = 0;
			this->UI_GB_Conn->TabStop = false;
			this->UI_GB_Conn->Text = L"Connection Information";
			// 
			// UI_Label_ConnectResult
			// 
			this->UI_Label_ConnectResult->AutoSize = true;
			this->UI_Label_ConnectResult->Location = System::Drawing::Point(238, 20);
			this->UI_Label_ConnectResult->Name = L"UI_Label_ConnectResult";
			this->UI_Label_ConnectResult->Size = System::Drawing::Size(73, 13);
			this->UI_Label_ConnectResult->TabIndex = 5;
			this->UI_Label_ConnectResult->Text = L"Disconnected";
			// 
			// UI_Button_Connect
			// 
			this->UI_Button_Connect->Location = System::Drawing::Point(238, 33);
			this->UI_Button_Connect->Name = L"UI_Button_Connect";
			this->UI_Button_Connect->Size = System::Drawing::Size(75, 23);
			this->UI_Button_Connect->TabIndex = 4;
			this->UI_Button_Connect->Text = L"Connect";
			this->UI_Button_Connect->UseVisualStyleBackColor = true;
			this->UI_Button_Connect->Click += gcnew System::EventHandler(this, &Form1::UI_Button_Connect_Click);
			// 
			// label2
			// 
			this->label2->AutoSize = true;
			this->label2->Location = System::Drawing::Point(192, 20);
			this->label2->Name = L"label2";
			this->label2->Size = System::Drawing::Size(26, 13);
			this->label2->TabIndex = 3;
			this->label2->Text = L"Port";
			// 
			// label1
			// 
			this->label1->AutoSize = true;
			this->label1->Location = System::Drawing::Point(7, 20);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(122, 13);
			this->label1->TabIndex = 2;
			this->label1->Text = L"IP Address / Host Name";
			// 
			// UI_TextBox_Port
			// 
			this->UI_TextBox_Port->Location = System::Drawing::Point(195, 36);
			this->UI_TextBox_Port->MaxLength = 5;
			this->UI_TextBox_Port->Name = L"UI_TextBox_Port";
			this->UI_TextBox_Port->ScrollBars = System::Windows::Forms::ScrollBars::Vertical;
			this->UI_TextBox_Port->Size = System::Drawing::Size(37, 20);
			this->UI_TextBox_Port->TabIndex = 1;
			this->UI_TextBox_Port->Text = L"1666";
			// 
			// UI_TextBox_ServerAddr
			// 
			this->UI_TextBox_ServerAddr->Location = System::Drawing::Point(6, 36);
			this->UI_TextBox_ServerAddr->Name = L"UI_TextBox_ServerAddr";
			this->UI_TextBox_ServerAddr->Size = System::Drawing::Size(183, 20);
			this->UI_TextBox_ServerAddr->TabIndex = 0;
			this->UI_TextBox_ServerAddr->Text = L"hammerofdoom.ath.cx";
			// 
			// UI_TrackBar_Guess
			// 
			this->UI_TrackBar_Guess->Enabled = false;
			this->UI_TrackBar_Guess->Location = System::Drawing::Point(13, 89);
			this->UI_TrackBar_Guess->Maximum = 1000;
			this->UI_TrackBar_Guess->Minimum = 1;
			this->UI_TrackBar_Guess->Name = L"UI_TrackBar_Guess";
			this->UI_TrackBar_Guess->Size = System::Drawing::Size(319, 45);
			this->UI_TrackBar_Guess->TabIndex = 1;
			this->UI_TrackBar_Guess->TickFrequency = 100;
			this->UI_TrackBar_Guess->Value = 1;
			this->UI_TrackBar_Guess->Scroll += gcnew System::EventHandler(this, &Form1::UI_TrackBar_Guess_Scroll);
			// 
			// UI_Label_Guess
			// 
			this->UI_Label_Guess->AutoSize = true;
			this->UI_Label_Guess->Enabled = false;
			this->UI_Label_Guess->Location = System::Drawing::Point(23, 162);
			this->UI_Label_Guess->Name = L"UI_Label_Guess";
			this->UI_Label_Guess->Size = System::Drawing::Size(65, 13);
			this->UI_Label_Guess->TabIndex = 2;
			this->UI_Label_Guess->Text = L"Your Guess:";
			// 
			// UI_TextBox_Guess
			// 
			this->UI_TextBox_Guess->Enabled = false;
			this->UI_TextBox_Guess->Location = System::Drawing::Point(108, 159);
			this->UI_TextBox_Guess->Name = L"UI_TextBox_Guess";
			this->UI_TextBox_Guess->Size = System::Drawing::Size(48, 20);
			this->UI_TextBox_Guess->TabIndex = 3;
			this->UI_TextBox_Guess->TextChanged += gcnew System::EventHandler(this, &Form1::UI_TextBox_Guess_TextChanged);
			// 
			// UI_Button_Guess
			// 
			this->UI_Button_Guess->Enabled = false;
			this->UI_Button_Guess->Location = System::Drawing::Point(251, 157);
			this->UI_Button_Guess->Name = L"UI_Button_Guess";
			this->UI_Button_Guess->Size = System::Drawing::Size(75, 23);
			this->UI_Button_Guess->TabIndex = 5;
			this->UI_Button_Guess->Text = L"Guess!";
			this->UI_Button_Guess->UseVisualStyleBackColor = true;
			this->UI_Button_Guess->Click += gcnew System::EventHandler(this, &Form1::UI_Button_Guess_Click);
			// 
			// UI_ListBox_Log
			// 
			this->UI_ListBox_Log->FormattingEnabled = true;
			this->UI_ListBox_Log->Location = System::Drawing::Point(12, 186);
			this->UI_ListBox_Log->Name = L"UI_ListBox_Log";
			this->UI_ListBox_Log->Size = System::Drawing::Size(321, 186);
			this->UI_ListBox_Log->TabIndex = 6;
			// 
			// UI_Label_LowerRange
			// 
			this->UI_Label_LowerRange->AutoSize = true;
			this->UI_Label_LowerRange->Location = System::Drawing::Point(13, 141);
			this->UI_Label_LowerRange->Name = L"UI_Label_LowerRange";
			this->UI_Label_LowerRange->Size = System::Drawing::Size(13, 13);
			this->UI_Label_LowerRange->TabIndex = 7;
			this->UI_Label_LowerRange->Text = L"1";
			// 
			// UI_Label_UpperRange
			// 
			this->UI_Label_UpperRange->AutoSize = true;
			this->UI_Label_UpperRange->Location = System::Drawing::Point(293, 141);
			this->UI_Label_UpperRange->Name = L"UI_Label_UpperRange";
			this->UI_Label_UpperRange->Size = System::Drawing::Size(31, 13);
			this->UI_Label_UpperRange->TabIndex = 8;
			this->UI_Label_UpperRange->Text = L"1000";
			this->UI_Label_UpperRange->TextAlign = System::Drawing::ContentAlignment::TopRight;
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->AutoSize = true;
			this->ClientSize = System::Drawing::Size(345, 381);
			this->Controls->Add(this->UI_Label_UpperRange);
			this->Controls->Add(this->UI_Label_LowerRange);
			this->Controls->Add(this->UI_ListBox_Log);
			this->Controls->Add(this->UI_Button_Guess);
			this->Controls->Add(this->UI_TextBox_Guess);
			this->Controls->Add(this->UI_Label_Guess);
			this->Controls->Add(this->UI_TrackBar_Guess);
			this->Controls->Add(this->UI_GB_Conn);
			this->MaximumSize = System::Drawing::Size(353, 415);
			this->MinimumSize = System::Drawing::Size(353, 415);
			this->Name = L"Form1";
			this->Text = L"Guessing Game Client - Addison Babcock";
			this->UI_GB_Conn->ResumeLayout(false);
			this->UI_GB_Conn->PerformLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->UI_TrackBar_Guess))->EndInit();
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion

private: 
		System::Void UI_Button_Connect_Click(System::Object^  sender, System::EventArgs^  e) 
		{
			//disable the buttons until the connection attempt completes
			this->UI_Button_Connect->Enabled = false;
			this->UI_TextBox_Port->Enabled = false;
			this->UI_TextBox_ServerAddr->Enabled = false;

			//build the socket and attempt to connect to the server
			m_hSock = gcnew Socket (AddressFamily::InterNetwork,
									SocketType::Stream,
									ProtocolType::Tcp);
			m_hSock->BeginConnect (UI_TextBox_ServerAddr->Text,
								   Convert::ToInt32 (UI_TextBox_Port->Text),
								   gcnew System::AsyncCallback (this, &Form1::_ConnectionCallback),
								   m_hSock);
		}

		System::Void UI_TrackBar_Guess_Scroll(System::Object^  sender, System::EventArgs^  e) 
		{
			//synchronize the trackbar and textbox
			this->UI_TextBox_Guess->Text = this->UI_TrackBar_Guess->Value.ToString ();
		}

		System::Void UI_TextBox_Guess_TextChanged(System::Object^  sender, System::EventArgs^  e) 
		{
			//range checking the length eliminates a possible exception
			if (this->UI_TextBox_Guess->Text->Length > 0 && this->UI_TextBox_Guess->Text->Length < 5)
			{
				//set the trackbar to either the value of the textbox or a boundary
				//if the value of the textbox is invalid
				int iGuess (Convert::ToInt32 (this->UI_TextBox_Guess->Text));
				iGuess = iGuess > this->UI_TrackBar_Guess->Minimum ? 
						 iGuess : 
						 this->UI_TrackBar_Guess->Minimum;
				iGuess = iGuess < this->UI_TrackBar_Guess->Maximum ? 
						 iGuess :
						 this->UI_TrackBar_Guess->Maximum;
				this->UI_TrackBar_Guess->Value = iGuess;
			}
		}

		System::Void UI_Button_Guess_Click(System::Object^  sender, System::EventArgs^  e) 
		{
			//disable the buttons while a guess is being attempted
			_Guessing ();

			int iData (this->UI_TrackBar_Guess->Value);	//contains the guess

			//pack the guess into an array
			for (int i (0); i < sizeof (int); ++i)
			{
				m_aucTXData [i] = *(((unsigned char *)(&iData)) + i);
			}

			//try to send the guess to the server and get a response
			try
			{
				this->m_hSock->Send (m_aucTXData);

				this->m_hSock->BeginReceive (m_aucRXData, 0, sizeof(int),
											System::Net::Sockets::SocketFlags::None,
											gcnew AsyncCallback (this, &Form1::_ReceiveCallback),
											m_hSock);
			}
			catch (SocketException ^ error)
			{
				//if the transmission fails, show the client as being disconnected and
				//make a remark in the log
				_Disconnected ();
				Invoke (gcnew _DelVoidString (this, &Form1::_Log), error->Message);
			}
		}

		void _ConnectionCallback (IAsyncResult ^ ar)
		{
			//the working socket
			Socket ^ hSock = (Socket ^)ar->AsyncState;

			//try to complete the connection
			try
			{
				hSock->EndConnect (ar);
			}
			catch (SocketException ^ error)
			{
				//connection attempt failed, log the error
				Invoke (gcnew _DelVoidString (this, &Form1::_Log), error->Message);
			}

			//let the main thread decide what to do based on the connection status
			if (hSock->Connected)
			{
				Invoke (gcnew _DelVoidBool (this, &Form1::_DoneConnecting), true);
			}
			else
			{
				Invoke (gcnew _DelVoidBool (this, &Form1::_DoneConnecting), false);
			}
		}

		void _ReceiveCallback (IAsyncResult ^ ar)
		{
			//the working socket
			Socket ^ hSock = (Socket ^)ar->AsyncState;

			//try to finish receiving the servers response
			try
			{
				hSock->EndReceive (ar);
				//tell the main form the receive completed succesfully
				Invoke (gcnew _DelVoidVoid (this, &Form1::_DoneReceiving));
			}
			catch (SocketException ^ error)
			{
				//log the error
				Invoke (gcnew _DelVoidString (this, &Form1::_Log), error->Message);
			}
		}

		void _DoneConnecting (bool bConnected)
		{
			if (bConnected)
			{
				//connection was succesful!
				//show the guessing buttons
				_Connected ();
			}
			else
			{
				//connection failed!
				//show the connection buttons again
				_Disconnected ();
			}
		}

		void _DoneReceiving (void)
		{
			int iReceivedData (0);	//holds the response after unpacking

			//unpack the data
			unsigned char * pt = reinterpret_cast <unsigned char *> (&iReceivedData);
			for (int i (0); i < sizeof (int); ++i)
			{
				*pt = m_aucRXData [i];
				++pt;
			}

			//was the guess correct?
			if (iReceivedData == 0)
			{
				//tell the user he/she wins
				_Log ("Your guess, " + this->UI_TrackBar_Guess->Value + ", was: PERFECT!");

				//reset the game
				this->UI_TrackBar_Guess->Minimum = 1;
				this->UI_TrackBar_Guess->Maximum = 1000;
				this->UI_Label_LowerRange->Text = (1).ToString ();
				this->UI_Label_UpperRange->Text = (1000).ToString ();

				//give the server the signal to reset
				//m_hSock->Send (m_aucResetSignal);
			}

			//was the guess too high?
			if (iReceivedData == 1)
			{
				//tell the user his/her guess was wrong
				_Log ("Your guess, " + this->UI_TrackBar_Guess->Value + ", was: too high!");

				//limit the range of guesses appropriately
				this->UI_TrackBar_Guess->Value = this->UI_TrackBar_Guess->Value - 1;
				this->UI_TrackBar_Guess->Maximum = this->UI_TrackBar_Guess->Value;
				this->UI_Label_UpperRange->Text = this->UI_TrackBar_Guess->Value.ToString ();
			}

			//was the guess too low?
			if (iReceivedData == -1)
			{
				//tell the user his/her guess was wrong
				_Log ("Your guess, " + this->UI_TrackBar_Guess->Value + ", was: too low!");

				//limit the range of guesses appropriately
				this->UI_TrackBar_Guess->Value = this->UI_TrackBar_Guess->Value + 1;
				this->UI_TrackBar_Guess->Minimum = this->UI_TrackBar_Guess->Value;
				this->UI_Label_LowerRange->Text = this->UI_TrackBar_Guess->Value.ToString ();
			}
			//update the text box to be within the new range
			this->UI_TextBox_Guess->Text = this->UI_TrackBar_Guess->Value.ToString ();

			//enable all the controls now that we are done with the guess attempt
			_DoneGuessing ();
		}

		void _Guessing (void)
		{
			//disable all the buttons and tell the user we are contacting the server
			this->UI_Button_Guess->Enabled = false;
			this->UI_Label_Guess->Enabled = false;
			this->UI_TextBox_Guess->Enabled = false;
			this->UI_TrackBar_Guess->Enabled = false;
			_Log ("Contacting server....");
		}

		void _DoneGuessing (void)
		{
			//guessing complete, reenable all the controls
			this->UI_Button_Guess->Enabled = true;
			this->UI_Label_Guess->Enabled = true;
			this->UI_TextBox_Guess->Enabled = true;
			this->UI_TrackBar_Guess->Enabled = true;
		}

		void _Connected (void)
		{
			//show the connection was succesful
			this->UI_Label_ConnectResult->Text = "Connected!";
			_Log ("Connected!");

			//diable the connection controls
			this->UI_TextBox_ServerAddr->Enabled = false;
			this->UI_Button_Connect->Enabled = false;
			this->UI_TextBox_Port->Enabled = false;

			//enable the guessing controls
			this->UI_Button_Guess->Enabled = true;
			this->UI_Label_Guess->Enabled = true;
			this->UI_TextBox_Guess->Enabled = true;
			this->UI_TrackBar_Guess->Enabled = true;

			//set the min/max of the trackbar correctly
			this->UI_TrackBar_Guess->Maximum = 1000;
			this->UI_TrackBar_Guess->Minimum = 1;
		}

		void _Disconnected (void)
		{
			//tell the user we are disconnected
			this->UI_Label_ConnectResult->Text = "Disconnected!";
			_Log ("Disconnected!");

			//turn on the connection controls
			this->UI_TextBox_ServerAddr->Enabled = true;
			this->UI_Button_Connect->Enabled = true;
			this->UI_TextBox_Port->Enabled = true;

			//turn off the guessing controls
			this->UI_Button_Guess->Enabled = false;
			this->UI_Label_Guess->Enabled = false;
			this->UI_TextBox_Guess->Enabled = false;
			this->UI_TrackBar_Guess->Enabled = false;
		}

		void _Log (String ^ text)
		{
			//add a line to the log
			this->UI_ListBox_Log->Items->Add (text);
		}
	};
}

