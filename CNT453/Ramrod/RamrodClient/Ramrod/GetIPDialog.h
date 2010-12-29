/***********************************************************
Project: Ramrod
Files: Form1.h, Form1.cpp, GetIPDialog.h, GetIPDialog.cpp, 
	StdAfx.h
Date: 02 Nov 07
***********************************************************/
#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;
using namespace System::Net::Sockets;
using namespace System::Net;


namespace Ramrod {

	public delegate void _DelVoidVoid (void);

	/// <summary>
	/// Summary for GetIPDialog
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	public ref class GetIPDialog : public System::Windows::Forms::Form
	{
	public:
		delegate void _DelVoidBool (bool);
		_DelVoidVoid ^ _ConnectClicked;
		_DelVoidBool ^ _ConnectCompleted;

		GetIPDialog (void);

	protected:
		~GetIPDialog ();
		
	private: 
		System::Windows::Forms::Label^  UI_Label_GetIp;
		System::Windows::Forms::TextBox^  UI_TextBox_IpAddress;
		System::Windows::Forms::Button^  UI_Button_OK;
		System::Windows::Forms::Button^  UI_Button_Cancel;
		System::Windows::Forms::ProgressBar^  UI_ProgessBar_ConnectionTimeOut;
		System::Windows::Forms::Timer^  UI_Timer_ConnectionTimeOut;
		System::ComponentModel::IContainer^  components;


#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->components = (gcnew System::ComponentModel::Container());
			this->UI_Label_GetIp = (gcnew System::Windows::Forms::Label());
			this->UI_TextBox_IpAddress = (gcnew System::Windows::Forms::TextBox());
			this->UI_Button_OK = (gcnew System::Windows::Forms::Button());
			this->UI_Button_Cancel = (gcnew System::Windows::Forms::Button());
			this->UI_ProgessBar_ConnectionTimeOut = (gcnew System::Windows::Forms::ProgressBar());
			this->UI_Timer_ConnectionTimeOut = (gcnew System::Windows::Forms::Timer(this->components));
			this->SuspendLayout();
			// 
			// UI_Label_GetIp
			// 
			this->UI_Label_GetIp->AutoSize = true;
			this->UI_Label_GetIp->Location = System::Drawing::Point(13, 13);
			this->UI_Label_GetIp->Name = L"UI_Label_GetIp";
			this->UI_Label_GetIp->Size = System::Drawing::Size(144, 13);
			this->UI_Label_GetIp->TabIndex = 0;
			this->UI_Label_GetIp->Text = L"Enter the servers IP Address:";
			// 
			// UI_TextBox_IpAddress
			// 
			this->UI_TextBox_IpAddress->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->UI_TextBox_IpAddress->Location = System::Drawing::Point(16, 29);
			this->UI_TextBox_IpAddress->Name = L"UI_TextBox_IpAddress";
			this->UI_TextBox_IpAddress->Size = System::Drawing::Size(266, 20);
			this->UI_TextBox_IpAddress->TabIndex = 1;
			this->UI_TextBox_IpAddress->Text = L"localhost";
			// 
			// UI_Button_OK
			// 
			this->UI_Button_OK->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Bottom) 
				| System::Windows::Forms::AnchorStyles::Left));
			this->UI_Button_OK->Location = System::Drawing::Point(16, 55);
			this->UI_Button_OK->Name = L"UI_Button_OK";
			this->UI_Button_OK->Size = System::Drawing::Size(128, 32);
			this->UI_Button_OK->TabIndex = 2;
			this->UI_Button_OK->Text = L"Connect";
			this->UI_Button_OK->UseVisualStyleBackColor = true;
			this->UI_Button_OK->Click += gcnew System::EventHandler(this, &GetIPDialog::UI_Button_OK_Click);
			// 
			// UI_Button_Cancel
			// 
			this->UI_Button_Cancel->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Bottom) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->UI_Button_Cancel->DialogResult = System::Windows::Forms::DialogResult::Cancel;
			this->UI_Button_Cancel->Location = System::Drawing::Point(152, 55);
			this->UI_Button_Cancel->Name = L"UI_Button_Cancel";
			this->UI_Button_Cancel->Size = System::Drawing::Size(130, 32);
			this->UI_Button_Cancel->TabIndex = 3;
			this->UI_Button_Cancel->Text = L"Cancel";
			this->UI_Button_Cancel->UseVisualStyleBackColor = true;
			this->UI_Button_Cancel->Click += gcnew System::EventHandler(this, &GetIPDialog::UI_Button_Cancel_Click);
			// 
			// UI_ProgessBar_ConnectionTimeOut
			// 
			this->UI_ProgessBar_ConnectionTimeOut->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->UI_ProgessBar_ConnectionTimeOut->Location = System::Drawing::Point(16, 93);
			this->UI_ProgessBar_ConnectionTimeOut->Maximum = 30;
			this->UI_ProgessBar_ConnectionTimeOut->Name = L"UI_ProgessBar_ConnectionTimeOut";
			this->UI_ProgessBar_ConnectionTimeOut->Size = System::Drawing::Size(266, 15);
			this->UI_ProgessBar_ConnectionTimeOut->Step = 1;
			this->UI_ProgessBar_ConnectionTimeOut->TabIndex = 4;
			// 
			// UI_Timer_ConnectionTimeOut
			// 
			this->UI_Timer_ConnectionTimeOut->Interval = 1000;
			this->UI_Timer_ConnectionTimeOut->Tick += gcnew System::EventHandler(this, &GetIPDialog::UI_Timer_ConnectionTimeOut_Tick);
			// 
			// GetIPDialog
			// 
			this->AcceptButton = this->UI_Button_OK;
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->CancelButton = this->UI_Button_Cancel;
			this->ClientSize = System::Drawing::Size(294, 120);
			this->Controls->Add(this->UI_ProgessBar_ConnectionTimeOut);
			this->Controls->Add(this->UI_Button_Cancel);
			this->Controls->Add(this->UI_Button_OK);
			this->Controls->Add(this->UI_TextBox_IpAddress);
			this->Controls->Add(this->UI_Label_GetIp);
			this->FormBorderStyle = System::Windows::Forms::FormBorderStyle::FixedDialog;
			this->MaximizeBox = false;
			this->MaximumSize = System::Drawing::Size(300, 152);
			this->MinimizeBox = false;
			this->MinimumSize = System::Drawing::Size(300, 152);
			this->Name = L"GetIPDialog";
			this->SizeGripStyle = System::Windows::Forms::SizeGripStyle::Hide;
			this->StartPosition = System::Windows::Forms::FormStartPosition::CenterScreen;
			this->Text = L"Enter Server IP Adress";
			this->Activated += gcnew System::EventHandler(this, &GetIPDialog::GetIPDialog_Activated);
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	public:
		//the actual ip address string is stored inside the text property
		//of UI_TextBox_IpAddress
		property String ^ p_sipAddress
		{
			String ^ get ()
			{
				return this->UI_TextBox_IpAddress->Text;
			}
			void set (String ^ ip)
			{
				this->UI_TextBox_IpAddress->Text = ip;
			}
		}

		//the actual socket ^ is stored in m_hSocket
		property Socket ^ p_hSocket
		{
			Socket ^ get ()
			{
				return m_hSocket;
			}
			void set (Socket ^)
			{
				//set is not used in this app
			}
		}

	private:
		Socket ^ m_hSocket;

		System::Void UI_Button_OK_Click(System::Object^  sender, System::EventArgs^  e);
		System::Void UI_Button_Cancel_Click(System::Object^  sender, System::EventArgs^  e);
		System::Void GetIPDialog_Activated(System::Object^  sender, System::EventArgs^  e);
		System::Void UI_Timer_ConnectionTimeOut_Tick(System::Object^  sender, System::EventArgs^  e);

		void _EnableControls ();
		void _DisableControls ();

		void _ConnectCallback (IAsyncResult ^ ar);
		void _ConnectComplete (bool bSuccess);
		System::Void GetIPDialog_FormClosing(System::Object^  sender, System::Windows::Forms::FormClosingEventArgs^  e);
	};
}
