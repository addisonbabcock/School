#pragma once
#include "RamrodStructs.h"

namespace RamrodServer {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace System::Drawing::Drawing2D;
	using namespace System::Net::Sockets;
	using namespace System::Net;

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
		Socket ^ m_hsListen;
		int m_iNumConnections;
		array<sPlayer^> ^ m_haspPlayers;
		//array<unsigned char> ^ m_aucRXData;	//receiving data buffer
		array<SObstacle> ^ m_aObstacles; //obstacles on the map
		System::Random ^ m_rand; //random number generator

			 array<sBullet> ^ m_aBullets;	//bullets in the world	

		//****************************************************************************************************
		// All of the delegate declarations
		//****************************************************************************************************
		delegate void _DelVoidString(String ^ hStr);
		delegate void _DelVoidSock(Socket ^ hSock);
		delegate void _DelVoidsPlayer(sPlayer ^ hspPlayer);
		delegate void _DelVoidVoid(void);

		//****************************************************************************************************
		// All of the callbacks and delegate functions for server functionality
		//****************************************************************************************************		
		void _AcceptCallback(IAsyncResult ^ ar);
		void _HandleAccept(Socket ^ hSock);
		void _PeekCallback (IAsyncResult ^ ar);
		void _DetermineFrameType (sPlayer ^ sPlayerData);
		void _ReceiveKeyStatesCallback(IAsyncResult ^ ar);
		void _ReceiveKeyStatesAction(sPlayer ^ hspPlayer);
		void _ReceiveChatMessageCallback (IAsyncResult ^ ar);
		void _ReceiveChatMessageAction (sPlayer ^ sPlayerData);

		void _HandleError(String ^ hStr);

		bool _HitDetection( sPlayer ^ hTank );
		bool _TankToObstacle( sPlayer ^ hTank, BufferedGraphics ^ hBufferedGr );
		bool _TankToTank( sPlayer ^ hspPlayer, BufferedGraphics ^ hBufferedGr );
		//bool _TankToObstacle( sPlayer ^ hTank );
		//bool _TankToTank( sPlayer ^ hspPlayer );		
		void _BulletToTank( BufferedGraphics ^ hBufferedGr );
		void _BulletToObstacle( BufferedGraphics ^ hBufferedGr );
		bool _ObstacleCloseToPoint (RectangleF obstacleLoc, PointF pointLoc);
		void _AddObstacleToPath (SObstacle % sOb, GraphicsPath ^ hGP);

		GraphicsPath ^ _BuildTank( sPlayer ^ );
		
		void ServerInit(void);
		void _BuildMap (void);
		void _SendMap (Socket ^ hClient);
		int _GetRandInt (int iLow, int iHigh);
		EObstacleType _GetRandObstacle ();

		void _KillSocket(sPlayer ^ sPlayerToKill);
		void _MoveBullets ();
		void _InitBullets ();

		void _AttemptToShoot (sPlayer ^ hspPlayer);
		void _AttemptToMove (sPlayer ^ hspPlayer);
		void _CheckForTimeouts ();
		void _PackAndSend ();
		void _SpawnTank(sPlayer ^ hspPlayer);

		void _DrawObstacles( BufferedGraphics ^ hBufferedGr );
		void _DrawScene(BufferedGraphics ^ hBufferedGr);
		void _DrawTanks(BufferedGraphics ^ hBufferedGr);
		void _DrawBullets(BufferedGraphics ^ hBufferedGr);


	public:
		Form1(void);
	private:

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
	private: System::Windows::Forms::StatusStrip^  statusStrip1;
	private: System::Windows::Forms::ToolStripStatusLabel^  statlblStatus;
	private: System::Windows::Forms::ToolStripStatusLabel^  statlblConnections;
	private: System::Windows::Forms::Timer^  tmrSend;
	private: System::ComponentModel::IContainer^  components;
 	private: System::Windows::Forms::ToolStripDropDownButton^  toolStripDropDownButton1;
	private: System::Windows::Forms::ToolStripMenuItem^  generateSendNewMapToolStripMenuItem;

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>


#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->components = (gcnew System::ComponentModel::Container());
			System::ComponentModel::ComponentResourceManager^  resources = (gcnew System::ComponentModel::ComponentResourceManager(Form1::typeid));
			this->statusStrip1 = (gcnew System::Windows::Forms::StatusStrip());
			this->statlblStatus = (gcnew System::Windows::Forms::ToolStripStatusLabel());
			this->statlblConnections = (gcnew System::Windows::Forms::ToolStripStatusLabel());
			this->toolStripDropDownButton1 = (gcnew System::Windows::Forms::ToolStripDropDownButton());
			this->generateSendNewMapToolStripMenuItem = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->tmrSend = (gcnew System::Windows::Forms::Timer(this->components));
			this->statusStrip1->SuspendLayout();
			this->SuspendLayout();
			// 
			// statusStrip1
			// 
			this->statusStrip1->Items->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(3) {this->statlblStatus, 
				this->statlblConnections, this->toolStripDropDownButton1});
			this->statusStrip1->Location = System::Drawing::Point(0, 422);
			this->statusStrip1->Name = L"statusStrip1";
			this->statusStrip1->Size = System::Drawing::Size(630, 22);
			this->statusStrip1->SizingGrip = false;
			this->statusStrip1->TabIndex = 0;
			this->statusStrip1->Text = L"statusStrip1";
			// 
			// statlblStatus
			// 
			this->statlblStatus->AutoSize = false;
			this->statlblStatus->Name = L"statlblStatus";
			this->statlblStatus->Size = System::Drawing::Size(120, 17);
			this->statlblStatus->Text = L"toolStripStatusLabel1";
			this->statlblStatus->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// statlblConnections
			// 
			this->statlblConnections->AutoSize = false;
			this->statlblConnections->Name = L"statlblConnections";
			this->statlblConnections->Size = System::Drawing::Size(120, 17);
			this->statlblConnections->Text = L"toolStripStatusLabel1";
			this->statlblConnections->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// toolStripDropDownButton1
			// 
			this->toolStripDropDownButton1->DropDownItems->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(1) {this->generateSendNewMapToolStripMenuItem});
			this->toolStripDropDownButton1->Image = (cli::safe_cast<System::Drawing::Image^  >(resources->GetObject(L"toolStripDropDownButton1.Image")));
			this->toolStripDropDownButton1->ImageTransparentColor = System::Drawing::Color::Magenta;
			this->toolStripDropDownButton1->Name = L"toolStripDropDownButton1";
			this->toolStripDropDownButton1->Size = System::Drawing::Size(62, 20);
			this->toolStripDropDownButton1->Text = L"Menu";
			// 
			// generateSendNewMapToolStripMenuItem
			// 
			this->generateSendNewMapToolStripMenuItem->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Text;
			this->generateSendNewMapToolStripMenuItem->Name = L"generateSendNewMapToolStripMenuItem";
			this->generateSendNewMapToolStripMenuItem->Size = System::Drawing::Size(152, 22);
			this->generateSendNewMapToolStripMenuItem->Text = L"New Map";
			this->generateSendNewMapToolStripMenuItem->Click += gcnew System::EventHandler(this, &Form1::generateSendNewMapToolStripMenuItem_Click);
			// 
			// tmrSend
			// 
			this->tmrSend->Enabled = true;
			this->tmrSend->Interval = 50;
			this->tmrSend->Tick += gcnew System::EventHandler(this, &Form1::tmrSend_Tick);
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(630, 444);
			this->Controls->Add(this->statusStrip1);
			this->FormBorderStyle = System::Windows::Forms::FormBorderStyle::Fixed3D;
			this->MaximizeBox = false;
			this->Name = L"Form1";
			this->Text = L"Ramrod Server V1.0";
			this->Shown += gcnew System::EventHandler(this, &Form1::Form1_Shown);
			this->statusStrip1->ResumeLayout(false);
			this->statusStrip1->PerformLayout();
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
		System::Void Form1_Shown(System::Object^  sender, System::EventArgs^  e);
		System::Void tmrSend_Tick(System::Object^  sender, System::EventArgs^  e);
		System::Void Form1::generateSendNewMapToolStripMenuItem_Click(System::Object^  sender, System::EventArgs^  e);

}; //class Form1
} //namespace RamrodServer

