/***********************************************************
Project: Ramrod
Files: Form1.h, Form1.cpp, GetIPDialog.h, GetIPDialog.cpp, 
StdAfx.h
Date: 02 Nov 07
***********************************************************/
#pragma once
#include "stdafx.h"
#include "GetIPDialog.h"
#include "frmSplash.h"

namespace Ramrod {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	//using namespace System::Drawing::Drawing2D;
	using namespace Microsoft::DirectX;
	using namespace Microsoft::DirectX::Direct3D;
	//using namespace Microsoft::DirectX::

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
	public:
		Form1(void)
		{
			InitializeComponent();
			m_hConnectDlg = gcnew GetIPDialog;
			m_hSplashDlg = gcnew frmSplash;
			m_RxData = gcnew array<unsigned char>(sizeof(sPlayerStates) );
			m_hObstacles = gcnew array <SObstacle> (gkuiMaxObstacleCount);
			m_haBullets = gcnew array <sBullet> (gkuiMaxBulletCount);
			m_haChatLog = gcnew array <String ^> (gkuiChatLogLength);
			for each (String ^ message in m_haChatLog)
			{
				message = nullptr;
			}
			m_uiTicksSinceLastChat = 0;
			m_uiCameraAngle = 20;
			m_uiCameraZoom = 30;
			
			_ClearMap ();

			m_d3ddev = nullptr;
			m_meshTank = nullptr;
			m_meshEllipse = nullptr;
			m_meshCube = nullptr;
			m_meshFloor = nullptr;
			m_meshBullet = nullptr;
			m_htFloor = nullptr;

			m_bShowRadar = false;
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
	private: 
		System::Windows::Forms::Timer^  RenderTimer;
	private: System::Windows::Forms::TextBox^  UI_ChatBox;


		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::IContainer^  components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->components = (gcnew System::ComponentModel::Container());
			this->RenderTimer = (gcnew System::Windows::Forms::Timer(this->components));
			this->UI_ChatBox = (gcnew System::Windows::Forms::TextBox());
			this->SuspendLayout();
			// 
			// RenderTimer
			// 
			this->RenderTimer->Interval = 50;
			this->RenderTimer->Tick += gcnew System::EventHandler(this, &Form1::RenderTimer_Tick);
			// 
			// UI_ChatBox
			// 
			this->UI_ChatBox->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->UI_ChatBox->Enabled = false;
			this->UI_ChatBox->Location = System::Drawing::Point(194, 416);
			this->UI_ChatBox->MaxLength = 38;
			this->UI_ChatBox->Name = L"UI_ChatBox";
			this->UI_ChatBox->Size = System::Drawing::Size(246, 20);
			this->UI_ChatBox->TabIndex = 0;
			this->UI_ChatBox->Visible = false;
			this->UI_ChatBox->PreviewKeyDown += gcnew System::Windows::Forms::PreviewKeyDownEventHandler(this, &Form1::UI_ChatBox_PreviewKeyDown);
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(634, 448);
			this->Controls->Add(this->UI_ChatBox);
			this->FormBorderStyle = System::Windows::Forms::FormBorderStyle::FixedSingle;
			this->MaximumSize = System::Drawing::Size(2000, 2000);
			this->MinimumSize = System::Drawing::Size(640, 480);
			this->Name = L"Form1";
			this->SizeGripStyle = System::Windows::Forms::SizeGripStyle::Hide;
			this->StartPosition = System::Windows::Forms::FormStartPosition::CenterScreen;
			this->Text = L"Ramrod client";
			this->Shown += gcnew System::EventHandler(this, &Form1::Form1_Shown);
			this->KeyUp += gcnew System::Windows::Forms::KeyEventHandler(this, &Form1::Form1_KeyUp);
			this->KeyDown += gcnew System::Windows::Forms::KeyEventHandler(this, &Form1::Form1_KeyDown);
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion

	private:
		sKeyStates Keys; //the state of the keys we care about
		GetIPDialog ^ m_hConnectDlg; //use this to connect to the server
		frmSplash ^ m_hSplashDlg;
		System::Net::Sockets::Socket ^ m_hConnection; //the connection to the server
		array<unsigned char> ^ m_RxData;//buffer to receive data
		sPlayerStates m_spsTankStates;//structure to draw in the RenderTimer_Tick event
		array <SObstacle> ^ m_hObstacles;
		array <sBullet> ^ m_haBullets;
		bool m_bShowRadar;
		array <String ^> ^ m_haChatLog;
		int m_uiTicksSinceLastChat;
		unsigned int m_uiCameraAngle;	//the camera angle
		unsigned int m_uiCameraZoom;	//the zoom amount
		//Drawing::Drawing2D::GraphicsPath ^ m_hMap;

		Microsoft::DirectX::Direct3D::Device ^ m_d3ddev;

		Mesh ^ m_meshTank;
		Mesh ^ m_meshEllipse;
		Mesh ^ m_meshCube;
		Mesh ^ m_meshFloor;
		Mesh ^ m_meshBullet;
		Texture ^ m_htFloor;



		void _ConnectionLost (void);

		void _ShowRadar( void );
		void _RadarHandler( void );

		void _PeekCallback (IAsyncResult ^ ar);
		void _DetermineFrameType (void);
		void _ReceivePlayerStatesCallback( IAsyncResult ^ ar );
		void _SuccessfulReceive( void );
		void _ReceiveObstacleCallback (IAsyncResult ^ ar);
		void _SuccessfulObstacleReceive (void);
		void _ReceiveChatMessageCallback (IAsyncResult ^ ar);
		void _SuccessfulChatMessageReceive (void);
		void _ClearMap (void);
		void _DrawObstacles(void);
		void _DrawTanks ();
		void _StartReceive ();

		void _DrawFloor(void);
		void _DrawBullets(void);
		void _DrawShotTimer ();
		void _DrawChatLog ();
		void _DrawScores ();
		void _SetupLights(void);
		Color _GetPlayerColor (int iPlayerNum);


		void _SetRenderStateStandard ();
		void _DevLostHandler(System::Object ^ hDev, System::EventArgs ^ hArgs);
		void _DevResetHandler(System::Object ^ hDev, System::EventArgs ^ hArgs);


		System::Void Form1_KeyDown(System::Object^  sender, System::Windows::Forms::KeyEventArgs^  e);
		System::Void Form1_KeyUp(System::Object^  sender, System::Windows::Forms::KeyEventArgs^  e);
		System::Void Form1_Shown(System::Object^  sender,	System::EventArgs^  e);
		System::Void RenderTimer_Tick(System::Object^  sender, System::EventArgs^  e);

		void _OpenChatBox ();
		System::Void UI_ChatBox_PreviewKeyDown(System::Object^  sender, 
			System::Windows::Forms::PreviewKeyDownEventArgs^  e);
	};
}

