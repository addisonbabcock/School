/***********************************************************
Project: Ramrod
Files: Form1.h, Form1.cpp, GetIPDialog.h, GetIPDialog.cpp, 
StdAfx.h
Date: 02 Nov 07
***********************************************************/
#include "stdafx.h"
#include "Form1.h"

using namespace Ramrod;

/***********************************************
Function: Form1_KeyDown

Description: Called when windows detects a key
was pressed. If a key we are interested in is 
down, this function will mark it as such. For
some keys this function will also update some
status variables like camera positioning and
radar visibility and chatbox.
***********************************************/
System::Void Form1::Form1_KeyDown(System::Object^  sender, System::Windows::Forms::KeyEventArgs^  e)
{
	//mark the key that was pressed as being down
	switch (e->KeyCode)
	{
	case System::Windows::Forms::Keys::W :
		Keys.m_bW = true;
		break;
	case System::Windows::Forms::Keys::A :
		Keys.m_bA = true;
		break;
	case System::Windows::Forms::Keys::S :
		Keys.m_bS = true;
		break;
	case System::Windows::Forms::Keys::D :
		Keys.m_bD = true;
		break;
	case System::Windows::Forms::Keys::Space :
		Keys.m_bSP = true;
		break;
	case System::Windows::Forms::Keys::F1 :
		m_hSplashDlg->ShowDialog();
		break;
	case System::Windows::Forms::Keys::M:
		_RadarHandler();
		break;
	case System::Windows::Forms::Keys::Enter :
		_OpenChatBox ();
		break;
	case System::Windows::Forms::Keys::PageUp:
		m_uiCameraAngle += 5;
		break;
	case System::Windows::Forms::Keys::PageDown:
		if(m_uiCameraAngle > 5)
			m_uiCameraAngle -= 5;
		break;
	case System::Windows::Forms::Keys::Subtract:
		m_uiCameraZoom += 5;
		break;
	case System::Windows::Forms::Keys::Add:
		m_uiCameraZoom -= 5;
		break;
	}
}

/***********************************************
Function: Form1_KeyUp

Description: Called when windows detects a key
was released. If a key we are interested in is 
up, this function will mark it as such. 
***********************************************/
System::Void Form1::Form1_KeyUp(System::Object^  sender, System::Windows::Forms::KeyEventArgs^  e)
{
	//mark the key that was pressed as being up
	switch (e->KeyCode)
	{
	case System::Windows::Forms::Keys::W :
		Keys.m_bW = false;
		break;
	case System::Windows::Forms::Keys::A :
		Keys.m_bA = false;
		break;
	case System::Windows::Forms::Keys::S :
		Keys.m_bS = false;
		break;
	case System::Windows::Forms::Keys::D :
		Keys.m_bD = false;
		break;
	case System::Windows::Forms::Keys::Space :
		Keys.m_bSP = false;
		break;
	}
}

/***********************************************
Function: Form1_Shown

Description: Called when windows shows the form.
Performs initialization code that cannot be done
inside a constructor such as setting up the
server connection and direct3d device
***********************************************/
System::Void Form1::Form1_Shown(System::Object^  sender, System::EventArgs^  e)
{
	static bool bAlreadyShown (false);
	this->Focus();

	if (!bAlreadyShown)
	{
		bAlreadyShown = true;	

		m_hSplashDlg->ShowDialog();

		//if the dialog came back ok then set the socket member
		//and start an asynchronous receive to get player data
		Windows::Forms::DialogResult result = m_hConnectDlg->ShowDialog ();
		if ( Windows::Forms::DialogResult::OK == result )
		{
			m_hConnection = m_hConnectDlg->p_hSocket;
			this->RenderTimer->Enabled = true;
			try
			{
				//m_hConnection->DontFragment = true;
				//m_hConnection->NoDelay = true;
				m_RxData = gcnew array <unsigned char> (sizeof (EFrameType));
				m_hConnection->BeginReceive(m_RxData,
					0,
					sizeof(EFrameType),
					Sockets::SocketFlags::Peek,
					gcnew AsyncCallback( this, &Form1::_PeekCallback), m_hConnection);
			}
			catch( SocketException ^ ex )
			{
				//print error message to the output pane if 
				//there is an error
				System::Diagnostics::Trace::WriteLine(
					"Form1::Form1_Shown::SocEx::BeginReceive::" + 
					ex->Message );
			}
		}
		if (Windows::Forms::DialogResult::Abort == result)
		{
			_ConnectionLost ();
		}
		if (Windows::Forms::DialogResult::Cancel == result)
		{
			Application::Exit ();
		}

		//try to set up the direct3d device
		try
		{
			PresentParameters ^ pparms = gcnew PresentParameters ();
			pparms->Windowed = true;
			pparms->SwapEffect = SwapEffect::Discard;
			pparms->BackBufferCount = 1;
			pparms->EnableAutoDepthStencil = true;
			pparms->AutoDepthStencilFormat = DepthFormat::D16;
			pparms->BackBufferFormat = Format::Unknown;

			// if you need to render to the backbuffer
			pparms->PresentFlag = PresentFlag::LockableBackBuffer;

			// create the D3D device
			m_d3ddev = gcnew Device (
				0,
				DeviceType::Hardware,	// hope you can get this...
				this,
				CreateFlags::MixedVertexProcessing | CreateFlags::FpuPreserve,
				pparms);

			// set standard render state (vertex format, ambient light, etc...)
			_SetRenderStateStandard ();

			// add event handlers for a lost/reset device
			m_d3ddev->DeviceLost += gcnew System::EventHandler (this, &Form1::_DevLostHandler);
			m_d3ddev->DeviceReset += gcnew System::EventHandler (this, &Form1::_DevResetHandler);

			//create the texture and mesh to use as our floor
			m_htFloor = Direct3D::TextureLoader::FromFile(m_d3ddev, "sand.dds");
			m_meshFloor = Mesh::FromFile("sandyfloor.x", MeshFlags::Managed, m_d3ddev);
			//old floor mesh, simple box
			//m_meshFloor = Mesh::Box(m_d3ddev, 640, 1, 480);
			m_meshBullet = Mesh::FromFile("bullet.x", MeshFlags::Managed, m_d3ddev);

			//load up the tank
			m_meshTank = Mesh::FromFile ("simple_tank.x", 
				MeshFlags::Managed, m_d3ddev);

			//the obstacle meshes
			m_meshEllipse = Mesh::Cylinder(m_d3ddev, 0.5, 0.5, 1, 25, 1);
			m_meshCube = Mesh::Box(m_d3ddev, 1, 1, 1);

			// start the rendering timer
			//UI_MAIN_TIMER->Start ();				 
		}
		catch (...)
		{
			MessageBox::Show ("UNABLE TO CREATE DIRECT3D DEVICE!", "Fatal error");
			Application::Exit ();
		}

	}
}

/***********************************************
Function: _SetRenderStateStandard

Description: Called when setting up a d3ddev.
Sets up the standard render state for the scene
***********************************************/
void Form1::_SetRenderStateStandard ()
{			
	// turn on some ambient light
	m_d3ddev->RenderState->Ambient = Color::Black;

	// describe flex. vertex format that this app intends to use
	m_d3ddev->VertexFormat = CustomVertex::PositionNormal::Format;

	// turn on a z-buffer
	m_d3ddev->RenderState->ZBufferEnable = true;
}

// this handler is called when the device is reset (window resized, etc...)
void Form1::_DevResetHandler (System::Object ^ hDev, System::EventArgs ^ hArgs)
{
	// device has been reset, so render state lost, reset
	_SetRenderStateStandard ();

	// restart the rendering timer
	this->RenderTimer->Enabled = true;
}

// this handler is called when the device is lost (three finger salute, etc...)
void Form1::_DevLostHandler (System::Object ^ hDev, System::EventArgs ^ hArgs)
{
	// device has been lost, it could be temporary or permanent
	Device ^ hdev = (Device ^)(hDev);

	// stop the rendering timer
	RenderTimer->Enabled = false;

	// check to see if the loss is unrecoverable
	// this code will cause an exit if so, the device could be restored
	//  with some more advanced code, but that is beyond starter stuff
	if (false == hdev->CheckCooperativeLevel ())
		Application::Exit ();
}

/***********************************************
Function: _PeekCallback

Description: Async Callback used to receive the
first byte of a frame from the server. When the
first byte is received this function will invoke
_DetermineFrameType.
***********************************************/
void Form1::_PeekCallback (IAsyncResult ^ ar)
{
	Socket ^ hServer = (Socket ^)ar->AsyncState;
	try
	{
		hServer->EndReceive (ar);
		Invoke (gcnew _DelVoidVoid (this, &Form1::_DetermineFrameType));
	}
	catch (SocketException ^ ex)
	{
		System::Diagnostics::Trace::WriteLine("_PeekCallBack::SocketException::" + 
			ex->Message);
		Invoke (gcnew _DelVoidVoid (this, &Form1::_ConnectionLost));

	}
	catch (ObjectDisposedException ^ ex)
	{
		System::Diagnostics::Trace::WriteLine("_PeekCallBack::ObjDisposedException::"+
			ex->Message);
	}
}

/***********************************************
Function: _DetermineFrameType

Description: Begins a receive based on the first
byte of a frame.
***********************************************/
void Form1::_DetermineFrameType (void)
{
	switch (m_RxData [0])
	{
	case eServerFull :
		MessageBox::Show ("Server full, please try again later. ",
			"The server is currently full.");
		_ConnectionLost ();
		break;

	case ePlayerState :
		m_RxData = gcnew array <unsigned char> (sizeof (sPlayerStates) + 
			sizeof (sBullet) * gkuiMaxBulletCount);
		m_hConnection->BeginReceive (m_RxData, 0, 
			sizeof (sPlayerStates) + sizeof (sBullet) * gkuiMaxBulletCount,
			SocketFlags::None, 
			gcnew AsyncCallback (this, &Form1::_ReceivePlayerStatesCallback),
			m_hConnection);
		break;

	case eObstacle:
		m_RxData = gcnew array <unsigned char> (sizeof (SObstacle));
		m_hConnection->BeginReceive (m_RxData, 0, sizeof (SObstacle),
			SocketFlags::None, 
			gcnew AsyncCallback (this, &Form1::_ReceiveObstacleCallback),
			m_hConnection);
		break;

	case eChatMessage:
		m_RxData = gcnew array <unsigned char> 
			(sizeof (EFrameType) + gkuiChatSize + sizeof (unsigned char));
		m_hConnection->BeginReceive (m_RxData, 0, 
			sizeof (EFrameType) + gkuiChatSize + sizeof (unsigned char),
			SocketFlags::None,
			gcnew AsyncCallback (this, &Form1::_ReceiveChatMessageCallback),
			m_hConnection);
		break;

		///////////////////////////////////////////////////////////////////////////
		//			If additional frames need to be added, do it here!			 //
		///////////////////////////////////////////////////////////////////////////

	default:
		System::Diagnostics::Trace::WriteLine ("Received an undefined frame type, how strange.");
		_ConnectionLost ();
		//_StartReceive ();
	}
}

/***********************************************
Function: RenderTimer_Tick

Description: Calls the functions required to 
render our game in its current state. This is
called when the render timer expires.
***********************************************/
System::Void Form1::RenderTimer_Tick(System::Object^  sender, System::EventArgs^  e)
{
	//players coords, angles, and connected states
	//System::Diagnostics::Trace::Write ("P: " + m_spsTankStates.m_iPlayerNum + "  P1: X:" + m_spsTankStates.m_fPlayer1X + " Y:" 
	//	+ m_spsTankStates.m_fPlayer1Y + " A:" + m_spsTankStates.m_fPlayer1Angle +  
	//	" C: " + m_spsTankStates.m_bPlayer1Connected);
	//System::Diagnostics::Trace::Write ("  P2: X:" + m_spsTankStates.m_fPlayer2X + " Y:" 
	//	+ m_spsTankStates.m_fPlayer2Y + " A:" + m_spsTankStates.m_fPlayer2Angle + 
	//	" C: " + m_spsTankStates.m_bPlayer2Connected);
	//System::Diagnostics::Trace::Write ("  P3: X:" + m_spsTankStates.m_fPlayer3X + " Y:" 
	//	+ m_spsTankStates.m_fPlayer3Y + " A:" + m_spsTankStates.m_fPlayer3Angle + 
	//	" C: " + m_spsTankStates.m_bPlayer3Connected);
	//System::Diagnostics::Trace::WriteLine ("  P4: X:" + m_spsTankStates.m_fPlayer4X + " Y:" 
	//	+ m_spsTankStates.m_fPlayer4Y + " A:" + m_spsTankStates.m_fPlayer4Angle + 
	//	" C: " + m_spsTankStates.m_bPlayer4Connected);	

	m_d3ddev->Clear(ClearFlags::Target | ClearFlags::ZBuffer, Color::Black, 1.0f, 0);
	
	m_d3ddev->BeginScene();

	//create the view matrix and then according the player's number, place the camera behind their tank
	//at the selected height and zoom
	Microsoft::DirectX::Matrix matrixView;

	if(m_spsTankStates.m_iPlayerNum == 1)
	{
		matrixView = Microsoft::DirectX::Matrix::LookAtLH(
			Vector3(
				m_spsTankStates.m_fPlayer1X - (float)Math::Sin(m_spsTankStates.m_fPlayer1Angle) * m_uiCameraZoom, 
				m_uiCameraAngle, 
				m_spsTankStates.m_fPlayer1Y - (float)Math::Cos(m_spsTankStates.m_fPlayer1Angle) * m_uiCameraZoom),	//eye
			Vector3(
				m_spsTankStates.m_fPlayer1X, 10.0f, 
				m_spsTankStates.m_fPlayer1Y),	//at
			Vector3(0.0f, 1.0f, 0.0f));		//up
	}
	else if(m_spsTankStates.m_iPlayerNum == 2)
	{
		matrixView = Microsoft::DirectX::Matrix::LookAtLH(
			Vector3(
				m_spsTankStates.m_fPlayer2X - (float)Math::Sin(m_spsTankStates.m_fPlayer2Angle) * m_uiCameraZoom, 
				m_uiCameraAngle, 
				m_spsTankStates.m_fPlayer2Y - (float)Math::Cos(m_spsTankStates.m_fPlayer2Angle) * m_uiCameraZoom),	//eye
			Vector3(
				m_spsTankStates.m_fPlayer2X, 10.0f, 
				m_spsTankStates.m_fPlayer2Y),	//at
			Vector3(0.0f, 1.0f, 0.0f));		//up
	}
	else if(m_spsTankStates.m_iPlayerNum == 3)
	{
		matrixView = Microsoft::DirectX::Matrix::LookAtLH(
			Vector3(
				m_spsTankStates.m_fPlayer3X - (float)Math::Sin(m_spsTankStates.m_fPlayer3Angle) * m_uiCameraZoom, 
				m_uiCameraAngle, 
				m_spsTankStates.m_fPlayer3Y - (float)Math::Cos(m_spsTankStates.m_fPlayer3Angle) * m_uiCameraZoom),	//eye
			Vector3(
				m_spsTankStates.m_fPlayer3X, 10.0f, 
				m_spsTankStates.m_fPlayer3Y),	//at
			Vector3(0.0f, 1.0f, 0.0f));		//up
	}
	else if(m_spsTankStates.m_iPlayerNum == 4)
	{
		matrixView = Microsoft::DirectX::Matrix::LookAtLH(
			Vector3(
				m_spsTankStates.m_fPlayer4X - (float)Math::Sin(m_spsTankStates.m_fPlayer4Angle) * m_uiCameraZoom, 
				m_uiCameraAngle, 
				m_spsTankStates.m_fPlayer4Y - (float)Math::Cos(m_spsTankStates.m_fPlayer4Angle) * m_uiCameraZoom),	//eye
			Vector3(
				m_spsTankStates.m_fPlayer4X, 10.0f, 
				m_spsTankStates.m_fPlayer4Y),	//at
			Vector3(0.0f, 1.0f, 0.0f));		//up
	}

	//set the view transform
	m_d3ddev->SetTransform(TransformType::View, matrixView);

	Microsoft::DirectX::Matrix matrixProj;
	matrixProj = Microsoft::DirectX::Matrix::PerspectiveFovLH (
		float (Math::PI / 4),									// standard perspective
		float (ClientSize.Width) / ClientSize.Height + 0.1f,	// aspect ratio
		0.1f,													// near clip
		1000.0f);												// far clip
	m_d3ddev->SetTransform (TransformType::Projection, matrixProj);

	//3D stuff
	_DrawFloor();	//draw the floor
	_DrawObstacles();	//draw the obstacles
	_DrawTanks ();	//draw the tanks
	_DrawBullets();
	_SetupLights();


	m_d3ddev->SetTransform (TransformType::World, Matrix::Identity);
	m_d3ddev->EndScene();

	//UI overlay stuff
	if ( m_bShowRadar )
		_ShowRadar();
	_DrawShotTimer();
	_DrawChatLog ();
	_DrawScores ();

	try
	{
		m_d3ddev->Present();
	}
	catch(DeviceLostException ^ e)
	{
		Text = e->Message;
	}
	catch(InvalidCallException ^ e)
	{
		Text = e->Message;
	}
}

/***********************************************
Function: _DrawTanks

Description: Called by RenderTimer_Tick. 
Places all the players tanks into the scene.
***********************************************/
void Form1::_DrawTanks ()
{
	Material player1Body, player2Body, player3Body, player4Body;
	Material tankGunMat, tankWheelsMat;
	MatrixStack ^ tankLoc = gcnew MatrixStack;

	//player 1 colors
	player1Body.Ambient = _GetPlayerColor (1);
	player1Body.Diffuse = _GetPlayerColor (1);

	//player 2 colors
	player2Body.Ambient = _GetPlayerColor (2);
	player2Body.Diffuse = _GetPlayerColor (2);

	//player 3 colors
	player3Body.Ambient = _GetPlayerColor (3);
	player3Body.Diffuse = _GetPlayerColor (3);

	//player 4 colors
	player4Body.Ambient = _GetPlayerColor (4);
	player4Body.Diffuse = _GetPlayerColor (4);

	//tank gun
	tankGunMat.Ambient = Color::Gray;
	tankGunMat.Diffuse = Color::Gray;

	//tank wheels
	tankWheelsMat.Ambient = Color::Brown;
	tankWheelsMat.Diffuse = Color::Brown;

	//draw player 1
	if (this->m_spsTankStates.m_bPlayer1Connected)
	{
		//position player 1. now with matrix stacks!
		tankLoc->Push ();
		//by default the tank draws upside down and really wierd
		tankLoc->RotateYawPitchRoll (m_spsTankStates.m_fPlayer1Angle + ((float)Math::PI * 3.0f) / 2.0f, 
			-((float)Math::PI * 3.0f) / 2.0f, 0.0f);
		//make the tank a decent size
		tankLoc->Scale (gkfTankScaling, gkfTankScaling, gkfTankScaling);
		//move the tank where it should be
		tankLoc->Translate (m_spsTankStates.m_fPlayer1X, 
			0, m_spsTankStates.m_fPlayer1Y);
		//tell d3d where to put the tank
		m_d3ddev->SetTransform (TransformType::World, tankLoc->Top);

		//the tracks and wheels will be brown
		m_d3ddev->Material = tankWheelsMat;
		//tank tracks and wheels subsets go from 0-27
		for (int iSubset (0); iSubset < 28; ++iSubset)
		{
			m_meshTank->DrawSubset (iSubset);
		}

		//the tanks gun will be grey
		m_d3ddev->Material = tankGunMat;
		//barrel is subset #28
		m_meshTank->DrawSubset (28);

		m_d3ddev->Material = player1Body;
		//body subset is #29
		m_meshTank->DrawSubset (29);

		tankLoc->Pop ();
	}

	//draw player 2
	if (this->m_spsTankStates.m_bPlayer2Connected)
	{
		//position player 2.
		tankLoc->Push ();
		//by default the tank draws upside down and really wierd
		tankLoc->RotateYawPitchRoll (m_spsTankStates.m_fPlayer2Angle + ((float)Math::PI * 3.0f) / 2.0f, 
			-((float)Math::PI * 3.0f) / 2.0f, 0.0f);
		//make the tank a decent size
		tankLoc->Scale (gkfTankScaling, gkfTankScaling, gkfTankScaling);
		//move the tank where it should be
		tankLoc->Translate (m_spsTankStates.m_fPlayer2X, 
			0, m_spsTankStates.m_fPlayer2Y);
		//tell d3d where to put the tank
		m_d3ddev->SetTransform (TransformType::World, tankLoc->Top);

		//the tracks and wheels will be brown
		m_d3ddev->Material = tankWheelsMat;
		//tank tracks and wheels subsets go from 0-27
		for (int iSubset (0); iSubset < 28; ++iSubset)
		{
			m_meshTank->DrawSubset (iSubset);
		}

		//the tanks gun will be grey
		m_d3ddev->Material = tankGunMat;
		//barrel is subset #28
		m_meshTank->DrawSubset (28);

		m_d3ddev->Material = player2Body;
		//body subset is #29
		m_meshTank->DrawSubset (29);

		tankLoc->Pop ();
	}

	//draw player 3
	if (this->m_spsTankStates.m_bPlayer3Connected)
	{
		//position player 3
		tankLoc->Push ();
		//by default the tank draws upside down and really wierd
		tankLoc->RotateYawPitchRoll (m_spsTankStates.m_fPlayer3Angle + ((float)Math::PI * 3.0f) / 2.0f, 
			-((float)Math::PI * 3.0f) / 2.0f, 0.0f);
		//make the tank a decent size
		tankLoc->Scale (gkfTankScaling, gkfTankScaling, gkfTankScaling);
		//move the tank where it should be
		tankLoc->Translate (m_spsTankStates.m_fPlayer3X, 
			0, m_spsTankStates.m_fPlayer3Y);
		//tell d3d where to put the tank
		m_d3ddev->SetTransform (TransformType::World, tankLoc->Top);

		//the tracks and wheels will be brown
		m_d3ddev->Material = tankWheelsMat;
		//tank tracks and wheels subsets go from 0-27
		for (int iSubset (0); iSubset < 28; ++iSubset)
		{
			m_meshTank->DrawSubset (iSubset);
		}

		//the tanks gun will be grey
		m_d3ddev->Material = tankGunMat;
		//barrel is subset #28
		m_meshTank->DrawSubset (28);

		m_d3ddev->Material = player3Body;
		//body subset is #29
		m_meshTank->DrawSubset (29);

		tankLoc->Pop ();
	}

	//draw player 4
	if (this->m_spsTankStates.m_bPlayer4Connected)
	{
		//position player 4
		tankLoc->Push ();
		//by default the tank draws upside down and really wierd
		tankLoc->RotateYawPitchRoll (m_spsTankStates.m_fPlayer4Angle + ((float)Math::PI * 3.0f) / 2.0f, 
			-((float)Math::PI * 3.0f) / 2.0f, 0.0f);
		System::Diagnostics::Trace::WriteLine ("Player 4 angle: " + m_spsTankStates.m_fPlayer4Angle);
		//make the tank a decent size
		tankLoc->Scale (gkfTankScaling, gkfTankScaling, gkfTankScaling);
		//move the tank where it should be
		tankLoc->Translate (m_spsTankStates.m_fPlayer4X, 
			0, m_spsTankStates.m_fPlayer4Y);
		//tell d3d where to put the tank
		m_d3ddev->SetTransform (TransformType::World, tankLoc->Top);

		//the tracks and wheels will be brown
		m_d3ddev->Material = tankWheelsMat;
		//tank tracks and wheels subsets go from 0-27
		for (int iSubset (0); iSubset < 28; ++iSubset)
		{
			m_meshTank->DrawSubset (iSubset);
		}

		//the tanks gun will be grey
		m_d3ddev->Material = tankGunMat;
		//barrel is subset #28
		m_meshTank->DrawSubset (28);

		m_d3ddev->Material = player4Body;
		//body subset is #29
		m_meshTank->DrawSubset (29);

		tankLoc->Pop ();
	}

	delete tankLoc;
}

/***********************************************
Function: _DrawShotTimer

Description: Called by RenderTimer_Tick after
direct3d is done rendering the scene. 
Draws the players shot timer with a changing
foreground color.
***********************************************/
void Form1::_DrawShotTimer ()
{

	//if the player can shoot, then there is no need to show a timer
	if (m_spsTankStates.m_iShotTimer == 0)
		return;

	// do backbuffer operations here
	Surface ^ hBBSurf = m_d3ddev->GetBackBuffer(0, 0, BackBufferType::Mono);
	Graphics ^ hGR = hBBSurf->GetGraphics ();

	//flip coord system so y points up
	System::Drawing::Drawing2D::Matrix ^ mat = gcnew System::Drawing::Drawing2D::Matrix( 1, 0, 0, -1, 0, 0);
	hGR->Transform = mat;
	hGR->TranslateTransform( 0, (float)this->ClientSize.Height, Drawing2D::MatrixOrder::Append );

	//make the moving bar of the timer a color between red and green depending on the shot timer
	int iRed (0), iGreen (0), iBlue (0);
	iRed = (m_spsTankStates.m_iShotTimer * 255) / gkuiShotCycles;
	iGreen = ((gkuiShotCycles - m_spsTankStates.m_iShotTimer) * 255) / gkuiShotCycles;
	System::Drawing::SolidBrush ^ hFGBrush = 
		gcnew System::Drawing::SolidBrush (Color::FromArgb (iRed, iGreen, iBlue));

	//make the back of the timer a dark grey
	System::Drawing::SolidBrush ^ hBGBrush = 
		gcnew System::Drawing::SolidBrush (Color::FromArgb (64, 64, 64));

	//draw the back of the shot timer
	hGR->FillRectangle (hBGBrush, 120, 400, 400, 20);

	//draw the front as a yellow bar fading from right to left
	hGR->FillRectangle (
		hFGBrush, 122, 402, 
		(m_spsTankStates.m_iShotTimer * 396) / gkuiShotCycles, 16);

	delete hFGBrush;
	delete hBGBrush;
	delete hGR;
	delete hBBSurf;
}

/***********************************************
Function: _DrawChatLog

Description: Called by RenderTimer_Tick after
direct3d is done rendering the scene. 
Shows the chat log and the chat log out if it
has been more then 200 ticks since a messages 
has been received.
***********************************************/
void Form1::_DrawChatLog ()
{
	// do backbuffer operations here
	Surface ^ hBBSurf = m_d3ddev->GetBackBuffer(0, 0, BackBufferType::Mono);
	Graphics ^ hGR = hBBSurf->GetGraphics ();

	//base the transparency of the chat log on how long its been since a message has been received
	int iAlpha = 255;//(255 - m_uiTicksSinceLastChat >= 0) ? 255 - m_uiTicksSinceLastChat : 0;
	if (m_uiTicksSinceLastChat < 200)
	{
		//dont fade before 10 seconds
		iAlpha = 255;
	}
	else
	{
		//gradually fade out after 10 seconds
		iAlpha = 255 - m_uiTicksSinceLastChat;

		//dont let alpha go below 0
		if (iAlpha < 0)
			iAlpha = 0;
	}

	//foreground color is based on player number and m_uiTicksSinceLastChat
	System::Drawing::SolidBrush ^ hFGBrush = 
		gcnew System::Drawing::SolidBrush (Color::FromArgb (iAlpha, Color::Black));
	//black text background
	System::Drawing::SolidBrush ^ hBGBrush = 
		gcnew System::Drawing::SolidBrush (Color::FromArgb (iAlpha, Color::Black));
	System::Drawing::Font ^ hFont = gcnew System::Drawing::Font (gcnew FontFamily ("Times New Roman"), 12);

	//draw each message in the log
	for (int i (0); i < gkuiChatLogLength; ++i)
	{
		if (m_haChatLog [i] != nullptr)
		{
			hFGBrush->Color = Color::FromArgb (iAlpha, _GetPlayerColor (m_haChatLog [i][0]));
			//draw the messages background
			hGR->DrawString (m_haChatLog [i]->Substring (1), hFont, hBGBrush, 319.0f, 349.0f + (float)i * 20.0f);
			//draw the messages foreground
			hGR->DrawString (m_haChatLog [i]->Substring (1), hFont, hFGBrush, 320.0f, 350.0f + (float)i * 20.0f);
		}
	}

	//one more tick since a message was received
	//one tick closer to fading the chat window
	++m_uiTicksSinceLastChat;

	delete hGR;
	delete hFGBrush;
	delete hBGBrush;
	delete hFont;
	delete hBBSurf;
}

/***********************************************
Function: _DrawScores

Description: Called by RenderTimer_Tick after
direct3d is done rendering the scene. 
Show the current kill and death tallies.
***********************************************/
void Form1::_DrawScores ()
{
	// do backbuffer operations here
	Surface ^ hBBSurf = m_d3ddev->GetBackBuffer(0, 0, BackBufferType::Mono);
	Graphics ^ hGR = hBBSurf->GetGraphics ();

	//foreground color is based on player number and m_uiTicksSinceLastChat
	System::Drawing::SolidBrush ^ hFGBrush = 
		gcnew System::Drawing::SolidBrush (Color::Black);
	//black text background
	System::Drawing::SolidBrush ^ hBGBrush = 
		gcnew System::Drawing::SolidBrush (Color::Black);
	System::Drawing::Font ^ hFont = gcnew System::Drawing::Font (gcnew FontFamily ("Times New Roman"), 12);

	hFGBrush->Color = Color::White;
	hGR->DrawString ("#    Kills      Deaths", hFont, hBGBrush, 5.0f, 20.0f);
	hGR->DrawString ("#    Kills      Deaths", hFont, hFGBrush, 6.0f, 21.0f);

	//player 1
	if (m_spsTankStates.m_bPlayer1Connected)
	{
		hFGBrush->Color = _GetPlayerColor (1);
		//build a string containing player 1s score and number
		String ^ hScore = gcnew String ("1");
		hScore += "    ";
		hScore += m_spsTankStates.m_uiPlayer1Kills.ToString ();
		hScore += "           ";
		hScore += m_spsTankStates.m_uiPlayer1Deaths.ToString ();

		//draw player 1s score background
		hGR->DrawString (hScore, hFont, 
			hBGBrush, 5.0f, 34.0f);
		//draw player 1s score foreground
		hGR->DrawString (hScore, hFont, 
			hFGBrush, 6.0f, 35.0f);
	}

	//player 2
	if (m_spsTankStates.m_bPlayer2Connected)
	{
		hFGBrush->Color = _GetPlayerColor (2);
		//build a string containing player 1s score and number
		String ^ hScore = gcnew String ("2");
		hScore += "    ";
		hScore += m_spsTankStates.m_uiPlayer2Kills.ToString ();
		hScore += "           ";
		hScore += m_spsTankStates.m_uiPlayer2Deaths.ToString ();

		//draw player 2s score background
		hGR->DrawString (hScore, hFont, 
			hBGBrush, 5.0f, 48.0f);
		//draw player 2s score foreground
		hGR->DrawString (hScore, hFont, 
			hFGBrush, 6.0f, 49.0f);
	}

	//player 3
	if (m_spsTankStates.m_bPlayer3Connected)
	{
		hFGBrush->Color = _GetPlayerColor (3);
		//build a string containing player 1s score and number
		String ^ hScore = gcnew String ("3");
		hScore += "    ";
		hScore += m_spsTankStates.m_uiPlayer3Kills.ToString ();
		hScore += "           ";
		hScore += m_spsTankStates.m_uiPlayer3Deaths.ToString ();

		//draw player 3s score background
		hGR->DrawString (hScore, hFont, 
			hBGBrush, 5.0f, 62.0f);
		//draw player 3s score foreground
		hGR->DrawString (hScore, hFont, 
			hFGBrush, 6.0f, 63.0f);
	}

	//player 4
	if (m_spsTankStates.m_bPlayer4Connected)
	{
		hFGBrush->Color = _GetPlayerColor (4);
		//build a string containing player 1s score and number
		String ^ hScore = gcnew String ("4");
		hScore += "    ";
		hScore += m_spsTankStates.m_uiPlayer4Kills.ToString ();
		hScore += "           ";
		hScore += m_spsTankStates.m_uiPlayer4Deaths.ToString ();

		//draw player 4s score background
		hGR->DrawString (hScore, hFont, 
			hBGBrush, 5.0f, 76.0f);
		//draw player 4s score foreground
		hGR->DrawString (hScore, hFont, 
			hFGBrush, 6.0f, 77.0f);
	}

	delete hGR;
	delete hFGBrush;
	delete hBGBrush;
	delete hFont;
	delete hBBSurf;
}

/***********************************************
Function: _ConnectionLost

Description: Called by any function that deals
with the socket if something bad happens.
Reshows the connection dialog and resets the
game state
***********************************************/
void Form1::_ConnectionLost (void)
{
	//destroy the saved map data, it will be re-sent
	_ClearMap ();

	//empty the chat log
	for each (String ^ message in m_haChatLog)
	{
		delete message;
		message = nullptr;
	}
	//setting this arbitrarily large so no messages will show
	m_uiTicksSinceLastChat = 9001;

	//completely destroy this connection and attempt a new one
	delete this->m_hConnection;
	delete this->m_hConnectDlg;
	this->m_hConnectDlg = gcnew GetIPDialog;
	this->m_hConnectDlg->ShowDialog ();
	if (m_hConnectDlg->DialogResult == Windows::Forms::DialogResult::Cancel //|| 
		//m_hConnectDlg->DialogResult == Windows::Forms::DialogResult::Abort
		)
		Application::Exit ();
	this->m_hConnection = this->m_hConnectDlg->p_hSocket;
	m_RxData = gcnew array <unsigned char> (sizeof (EFrameType));

	//try again to receive junk from the server
	try
	{
		m_hConnection->DontFragment = true;
		m_hConnection->NoDelay = true;
		this->m_hConnection->BeginReceive (m_RxData, 0, sizeof (EFrameType),
			SocketFlags::Peek, gcnew AsyncCallback (this, &Form1::_PeekCallback),
			m_hConnection);
		this->RenderTimer->Enabled = true;
	}
	catch (SocketException ^ ex)
	{
		System::Diagnostics::Trace::WriteLine("_ConnectionLost::SocketException::BeginReceive::" 
			+ ex->Message);
		_ConnectionLost ();
	}
	catch (ObjectDisposedException ^ ex)
	{
		System::Diagnostics::Trace::WriteLine("_ConnectionLost::ObjDisposedException::" 
			"BeginReceive" + ex->Message);
		_ConnectionLost ();
	}

	//we only found this happening when the dialog failed to connect, and then was closed
	//in any case if the user is cancelling the dialog, he wants the app to exit
	catch (NullReferenceException ^ )
	{
		Application::Exit ();
	}
}

/***********************************************
Function: _ReceivePlayerStatesCallback

Description: Called by _DetermineFrameType.
Finishes the receiving of game state from
the server and invokes _SuccessfulReceive
when done.
***********************************************/
void Form1::_ReceivePlayerStatesCallback(System::IAsyncResult ^ar)
{
	try 
	{
		//get number of bytes read from the socket
		int iNumRead = ((Socket ^)ar->AsyncState)->EndReceive(ar);


		//Right number of bytes read?  If so invoke the function in the main
		//thread to deal with the data.
		if (iNumRead == sizeof(sPlayerStates) + sizeof (sBullet) * gkuiMaxBulletCount)
			Invoke(gcnew _DelVoidVoid(this, &Form1::_SuccessfulReceive));
		else
		{
			//something has gone awry 

			System::Diagnostics::Trace::WriteLine("Uh oh.  Wrong number of bytes? Got " + iNumRead + " wanted " + 
				(sizeof(sPlayerStates) + sizeof (sBullet) * gkuiMaxBulletCount));

			Invoke (gcnew _DelVoidVoid(this, &Form1::_StartReceive));
		}
	}
	catch (SocketException ^ ex)
	{
		System::Diagnostics::Trace::WriteLine("_ReceiveCallback::SocEx::End Receive::" +
			ex->Message);
		Invoke (gcnew _DelVoidVoid (this, &Form1::_ConnectionLost));
	}
}

/***********************************************
Function: _SuccessfulReceive

Description: Handles the receiving of a player
state frame. Unpacks and saves the state 
information for future rendering.
***********************************************/
void Form1::_SuccessfulReceive( void )
{
	/***************Dealing with the player states that were received********/
	//put the received data back into a structure
	sPlayerStates s_PlayerStates;
	unsigned char * pT = reinterpret_cast<unsigned char*> (&s_PlayerStates);

	for (unsigned int i(0); i < sizeof( sPlayerStates ); ++i )
	{
		*pT = m_RxData[i];
		++pT;
	}


	//put the the tanks in a struct so that we can render
	//it in the RenderTimer_Tick event.
	m_spsTankStates = s_PlayerStates;
	this->Text = "Player #: " + m_spsTankStates.m_iPlayerNum.ToString();
	switch (m_spsTankStates.m_iPlayerNum)
	{
	case 1:
		this->Text += " Kills: " + m_spsTankStates.m_uiPlayer1Kills + 
			" Deaths: " + m_spsTankStates.m_uiPlayer1Deaths;
		break;
	case 2:
		this->Text += " Kills: " + m_spsTankStates.m_uiPlayer2Kills + 
			" Deaths: " + m_spsTankStates.m_uiPlayer2Deaths;
		break;
	case 3:
		this->Text += " Kills: " + m_spsTankStates.m_uiPlayer3Kills + 
			" Deaths: " + m_spsTankStates.m_uiPlayer3Deaths;
		break;
	case 4:
		this->Text += " Kills: " + m_spsTankStates.m_uiPlayer4Kills + 
			" Deaths: " + m_spsTankStates.m_uiPlayer4Deaths;
		break;
	}

	//now pull the bullet information out of the buffer
	sBullet sTempBullet; //holds a bullet while it is rebuilt
	unsigned char * pByte;
	for (int iBulNum (0); iBulNum < gkuiMaxBulletCount; ++iBulNum)
	{
		pByte = reinterpret_cast <unsigned char *> (&sTempBullet);
		for (unsigned int uiByteNum (0); uiByteNum < sizeof (sBullet); ++uiByteNum)
		{
			*pByte = m_RxData [sizeof (sPlayerStates) + uiByteNum + iBulNum * sizeof (sBullet)];
			++pByte;
		}
		m_haBullets [iBulNum] = sTempBullet;
	}

	//start to receive again
	try
	{
		m_RxData = gcnew array <unsigned char> (sizeof (EFrameType));
		m_hConnection->BeginReceive(m_RxData,
			0,
			sizeof(EFrameType),
			Sockets::SocketFlags::Peek,
			gcnew AsyncCallback( this, &Form1::_PeekCallback), m_hConnection);
	}
	catch( SocketException ^ ex )
	{
		//print error message to the output pane if 
		//there is an error
		System::Diagnostics::Trace::WriteLine(
			"Form1::Form1_Shown::SocEx::BeginReceive::" + 
			ex->Message );
	}
	catch( ObjectDisposedException ^ ex )
	{		
		System::Diagnostics::Trace::WriteLine(
			"Form1::Form1_Shown::ObjDisposedException::BeginReceive::" + 
			ex->Message );
	}
	/***********Sending new keystates back to the server***************/
	//create the array we're going to send
	array <unsigned char> ^ aucTxData = gcnew array<unsigned char> ( sizeof(sKeyStates) );

	//populate the structure
	sKeyStates s_Keys;
	s_Keys = this->Keys;


	//package the player structure up in the array so that we can send it
	for (unsigned int i(0); i < sizeof(sKeyStates); ++i)
		aucTxData[i] = * (((unsigned char*)(&s_Keys)) + i);

	//try to send the junk!
	try
	{
		m_hConnection->Send( aucTxData );
	}
	catch ( SocketException ^ ex )
	{
		System::Diagnostics::Trace::WriteLine( 
			"_Successful::SocEx::Send Failed::" + 
			ex->Message);
	}
	/*****************************************************************/
}

/***********************************************
Function: _ReceiveObstacleCallback

Description: Called by _DetermineFrameType.
Finishes the receiving of an obstacle from
the server and invokes _SuccessfulObstacleReceive
when done.
***********************************************/
void Form1::_ReceiveObstacleCallback (IAsyncResult ^ ar)
{
	Socket ^ hServer = (Socket ^)(ar->AsyncState);

	try
	{
		int iBytes = hServer->EndReceive (ar);
		if (sizeof (SObstacle) == iBytes)
		{
			Invoke (gcnew _DelVoidVoid (this, &Form1::_SuccessfulObstacleReceive));
		}
		else
		{
			System::Diagnostics::Trace::WriteLine ("Wrong number of bytes received. Got " + 
				iBytes.ToString () + ", wanted " + (sizeof (SObstacle)).ToString () + ".");
			Invoke (gcnew _DelVoidVoid (this, &Form1::_StartReceive));
		}
	}
	catch (SocketException ^ e)
	{
		System::Diagnostics::Trace::WriteLine ("_ReceiveObstacleCallback::SocEx::EndReceive::" + 
			e->Message);
		Invoke (gcnew _DelVoidVoid (this, &Form1::_ConnectionLost));
	}
}

/***********************************************
Function: _SuccessfulObstacleReceive

Description: Places a received obstacle frame
into the map array.
***********************************************/
void Form1::_SuccessfulObstacleReceive (void)
{
	//unpack the received data into sOb
	SObstacle sOb;
	unsigned char * pT = reinterpret_cast<unsigned char*> (&sOb);
	for (unsigned int i(0); i < sizeof( SObstacle ); ++i )
	{
		*pT = m_RxData[i];
		++pT;
	}

	//add the obstacle to the list
	m_hObstacles [sOb.m_iObstacleNumber] = sOb;

	//let receiving resume
	try
	{
		m_RxData = gcnew array <unsigned char> (sizeof (EFrameType));
		m_hConnection->BeginReceive (m_RxData, 0, sizeof (EFrameType),
			SocketFlags::Peek, 
			gcnew AsyncCallback (this, &Form1::_PeekCallback),
			m_hConnection);
	}
	catch (SocketException ^ e)
	{
		//print error message to the output pane if 
		//there is an error
		System::Diagnostics::Trace::WriteLine(
			"Form1::_SuccessfulObstacleReceive::SocEx::BeginReceive::" + 
			e->Message );
	}
}

/***********************************************
Function: _SuccessfulChatMessageReceive

Description: Called by _DetermineFrameType.
Finishes the receiving of a chat message from
the server and invokes _SuccessfulChatMessageReceive
when done.
***********************************************/
void Form1::_ReceiveChatMessageCallback (IAsyncResult ^ ar)
{
	Socket ^ hServer = (Socket ^)(ar->AsyncState);

	try
	{
		int iBytes = hServer->EndReceive (ar);
		if (sizeof (EFrameType) + gkuiChatSize + sizeof (unsigned char) == iBytes)
		{
			Invoke (gcnew _DelVoidVoid (this, &Form1::_SuccessfulChatMessageReceive));
		}
		else
		{
			System::Diagnostics::Trace::WriteLine (
				"Wrong number of bytes received. Got " + 
				iBytes.ToString () + ", wanted " + 
				(sizeof (EFrameType) + gkuiChatSize + sizeof (unsigned char)).ToString () + ".");
			Invoke (gcnew _DelVoidVoid (this, &Form1::_StartReceive));
		}
	}
	catch (SocketException ^ e)
	{
		System::Diagnostics::Trace::WriteLine ("_ReceiveChatMessageCallback::SoxEx::EndReceive::" +
			e->Message);
		Invoke (gcnew _DelVoidVoid (this, &Form1::_ConnectionLost));
	}
}

/***********************************************
Function: _SuccessfulChatMessageReceive

Description: Called when a chat message has been
completely received. Adds the message to the 
log, pushing out the oldest message when needed.
***********************************************/
void Form1::_SuccessfulChatMessageReceive ()
{
	//unpack the chat message
	char szText [gkuiChatSize] = {0};
	unsigned char * pT = reinterpret_cast <unsigned char*> (szText);
	for (unsigned int i (0); i < gkuiChatSize; ++i)
	{
		*pT = m_RxData [i+1];
		pT++;
	}
	//finally convert the text to a format thats useable
	String ^ RxText = gcnew String (szText);

	//show what we got for debugging
	System::Diagnostics::Trace::WriteLine
		("Receive chat message \"" + RxText + "\".");

	//move each message up in the log
	for (int i (0); i < gkuiChatLogLength - 1; ++i)
	{
		m_haChatLog [i] = m_haChatLog [i + 1];
	}
	//add the new message to the end
	m_haChatLog [gkuiChatLogLength - 1] = RxText;

	//chat message received successfully, reset the transperancy timer
	m_uiTicksSinceLastChat = 0;

	_StartReceive ();
}

/***********************************************
Function: _ClearMap

Description: Clears out all map information.
***********************************************/
void Form1::_ClearMap ()
{
	//go through all the obstacles
	//set them to the most brutal obstacle of all...
	//NOTHING
	for each (SObstacle sOb in m_hObstacles)
	{
		sOb.m_eFrameType = eObstacle;
		sOb.m_eObstacleType = eNone;
		sOb.m_fObstacleHeight = 0;
		sOb.m_fObstacleWidth = 0;
		sOb.m_fObstacleX = 0;
		sOb.m_fObstacleY = 0;
	}
}

/***********************************************
Function: _StartReceive

Description: Begins the receiving process over
again.
***********************************************/
void Form1::_StartReceive ()
{
	try //to start receiving again
	{
		m_RxData = gcnew array <unsigned char> (sizeof (EFrameType));
		m_hConnection->BeginReceive (m_RxData, 0, sizeof (EFrameType), SocketFlags::Peek,
			gcnew AsyncCallback (this, &Form1::_PeekCallback), m_hConnection);
	}
	catch (SocketException ^ e)
	{
		System::Diagnostics::Trace::WriteLine ("Form1::_StartReceive::SocEx::" + e->Message);
		_ConnectionLost ();
	}
}

/***********************************************
Function: _DrawFloor

Description: Called by RenderTimer_Tick.
positions and adds a sandy floor to the scene.
***********************************************/
void Form1::_DrawFloor(void)
{
	Material matFloor;	//"material" for the floor
	matFloor.Ambient = Color::Silver;
	matFloor.Diffuse = Color::Silver;

	//MatrixStack for floor
	MatrixStack ^ msFloor = gcnew MatrixStack;

	//scale and translate the floor
	msFloor->Push();
	msFloor->Scale(Vector3(6.5f, 0, 4.5f));
	msFloor->Translate(Vector3(320,0,240));
	
	//set the texture (sandy floor)
	m_d3ddev->SetTexture(0, m_htFloor);

	m_d3ddev->SetTransform(TransformType::World, msFloor->Top);

	m_d3ddev->Material = matFloor;

	m_meshFloor->DrawSubset(0);

	m_d3ddev->SetTexture(0, nullptr);

	msFloor->Pop();

	delete msFloor;
}

/***********************************************
Function: _DrawObstacles

Description: Called by RenderTimer_Tick.
Positions and adds the obstacles to the scene.
***********************************************/
void Form1::_DrawObstacles(void)
{
	MatrixStack ^ msObstacles = gcnew MatrixStack;
	Material matObstacle;

	for each(SObstacle sOb in m_hObstacles)
	{
		if(sOb.m_eObstacleType != eNone)
		{
			matObstacle.Ambient = sOb.m_color;
			matObstacle.Diffuse = sOb.m_color;
			msObstacles->Push();
			switch (sOb.m_eObstacleType)
			{
			case eEllipse:
				msObstacles->RotateYawPitchRoll (
					m_spsTankStates.m_fPlayer1Angle + ((float)Math::PI * 3.0f) / 2.0f, 
					-((float)Math::PI * 3.0f) / 2.0f, 0.0f);
				msObstacles->Scale(Vector3(sOb.m_fObstacleWidth, 20, sOb.m_fObstacleHeight));
				msObstacles->Translate(Vector3(sOb.m_fObstacleX + sOb.m_fObstacleWidth / 2, 10, sOb.m_fObstacleY + sOb.m_fObstacleHeight / 2));
				m_d3ddev->SetTransform(TransformType::World, msObstacles->Top);
				m_d3ddev->Material = matObstacle;
				m_meshEllipse->DrawSubset(0);
				break;
			case eRectangle:
				msObstacles->Scale(Vector3(sOb.m_fObstacleWidth, 20, sOb.m_fObstacleHeight));
				msObstacles->Translate(Vector3(sOb.m_fObstacleX + sOb.m_fObstacleWidth / 2, 10, sOb.m_fObstacleY + sOb.m_fObstacleHeight / 2));
				m_d3ddev->SetTransform(TransformType::World, msObstacles->Top);
				m_d3ddev->Material = matObstacle;
				m_meshCube->DrawSubset(0);
				break;
			case eNone:
				continue;
			}
			msObstacles->Pop();
		}
	}
	delete msObstacles;
}

/***********************************************
Function: _RadarHandler

Description: Called by Form1_KeyDown. Toggles
the radar when M key is pressed.
***********************************************/
void Form1::_RadarHandler( void )
{
	m_bShowRadar ? m_bShowRadar = false : m_bShowRadar = true;	
	//m_bShowRadar = !m_bShowRadar; lolololololwut?
}

/***********************************************
Function: _ShowRadar

Description: Called by RenderTimer_Tick after
direct3d rendering is complete. Draws the radar
on top of the resulting scene.
***********************************************/
void Form1::_ShowRadar( void )
{
	// do backbuffer operations here
	Surface ^ hBBSurf = m_d3ddev->GetBackBuffer(0, 0, BackBufferType::Mono);
	Graphics ^ hGR = hBBSurf->GetGraphics ();
	
	//flip coord system so y points up
	System::Drawing::Drawing2D::Matrix ^ mat = gcnew System::Drawing::Drawing2D::Matrix( 1, 0, 0, -1, 0, 0);
	hGR->Transform = mat;
	hGR->TranslateTransform( 0, (float)this->ClientSize.Height, Drawing2D::MatrixOrder::Append );

	//enemy pen
	Pen ^ hEnemyPen = gcnew Pen(Color::Red);
	//friendly pen
	Pen ^ hFriendPen = gcnew Pen(Color::Green);
	Pen ^ hObstaclePen = gcnew Pen(Color::White);

	//matrix for rotation of tanks/obstacles
	Drawing2D::Matrix ^ hMatrix = gcnew Drawing2D::Matrix;

	//matrix for scaling the map
	Drawing2D::Matrix ^ hMapScaleMatrix = gcnew Drawing2D::Matrix;
	hMapScaleMatrix->Scale(0.3f, 0.3f);//scale smaller
	
	//Needed graphics Paths
	Drawing2D::GraphicsPath ^ hGPMap = gcnew Drawing2D::GraphicsPath;
	Drawing2D::GraphicsPath ^ hGPTank1 = gcnew Drawing2D::GraphicsPath;
	Drawing2D::GraphicsPath ^ hGPTank2 = gcnew Drawing2D::GraphicsPath;
	Drawing2D::GraphicsPath ^ hGPTank3 = gcnew Drawing2D::GraphicsPath;
	Drawing2D::GraphicsPath ^ hGPTank4 = gcnew Drawing2D::GraphicsPath;
	Drawing2D::GraphicsPath ^ hGPObstacles = gcnew Drawing2D::GraphicsPath;

	

	//add the obstacles to the obstacle path,
	//obstacle path later goes into map path
	for each (SObstacle % rObstacle in m_hObstacles)
	{
		switch (rObstacle.m_eObstacleType)
		{
		case eEllipse:
			hGPObstacles->AddEllipse( rObstacle.m_fObstacleX,
									rObstacle.m_fObstacleY, 
									rObstacle.m_fObstacleWidth,
									rObstacle.m_fObstacleHeight);
			
			break;
		case eRectangle:
			hGPObstacles->AddRectangle( RectangleF(rObstacle.m_fObstacleX,
												rObstacle.m_fObstacleY,
												rObstacle.m_fObstacleWidth,
												rObstacle.m_fObstacleHeight)
												);
			break;
		case eNone:
			break;
		}
	}
	
	hGPObstacles->Transform( hMapScaleMatrix );
	hGR->DrawPath(hObstaclePen, hGPObstacles);


	//checking for connections first
	if (m_spsTankStates.m_bPlayer1Connected)
	{
			RectangleF hRTankBody = RectangleF(m_spsTankStates.m_fPlayer1X- (float)gkuiTankWidth / 2.0f,
										m_spsTankStates.m_fPlayer1Y - (float)gkuiTankHeight / 2.0f, 
										(float)gkuiTankWidth, (float)gkuiTankHeight);

			hGPTank1->AddRectangle( hRTankBody );

			//rotate from the center of the tank
			hMatrix->Reset ();

			hMatrix->RotateAt(-m_spsTankStates.m_fPlayer1Angle * 180 / gkfPI , 
									PointF(m_spsTankStates.m_fPlayer1X,
										   m_spsTankStates.m_fPlayer1Y));

			//apply the rotation and draw the thing
			hGPTank1->Transform( hMatrix );
			hMatrix->Reset();
			hGPTank1->Transform( hMapScaleMatrix );//scale down

			if (m_spsTankStates.m_iPlayerNum == 1)
				hGR->DrawPath(hFriendPen, hGPTank1);
			else
				hGR->DrawPath(hEnemyPen, hGPTank1);


	}

	if (m_spsTankStates.m_bPlayer2Connected)
	{
			RectangleF hRTankBody = RectangleF(m_spsTankStates.m_fPlayer2X- (float)gkuiTankWidth / 2.0f,
										m_spsTankStates.m_fPlayer2Y - (float)gkuiTankHeight / 2.0f, 
										(float)gkuiTankWidth, (float)gkuiTankHeight);

			hGPTank2->AddRectangle( hRTankBody );

			//rotate from the center of the tank
			hMatrix->Reset ();

			hMatrix->RotateAt(-m_spsTankStates.m_fPlayer2Angle * 180 / gkfPI , 
									PointF(m_spsTankStates.m_fPlayer2X,
										   m_spsTankStates.m_fPlayer2Y));

			hGPTank2->Transform (hMatrix);
			hMatrix->Reset();
			hGPTank2->Transform( hMapScaleMatrix );//scale down

			if (m_spsTankStates.m_iPlayerNum == 2)
				hGR->DrawPath(hFriendPen, hGPTank2);
			else
				hGR->DrawPath(hEnemyPen, hGPTank2);
	}
	if (m_spsTankStates.m_bPlayer3Connected)
	{
			RectangleF hRTankBody = RectangleF(m_spsTankStates.m_fPlayer3X- (float)gkuiTankWidth / 2.0f,
										m_spsTankStates.m_fPlayer3Y - (float)gkuiTankHeight / 2.0f, 
										(float)gkuiTankWidth, (float)gkuiTankHeight);

			hGPTank3->AddRectangle( hRTankBody );

			//rotate from the center of the tank
			hMatrix->Reset ();

			hMatrix->RotateAt(-m_spsTankStates.m_fPlayer3Angle * 180 / gkfPI , 
									PointF(m_spsTankStates.m_fPlayer3X,
										   m_spsTankStates.m_fPlayer3Y));

			//apply the rotation and draw the thing
			hGPTank3->Transform( hMatrix );
			hMatrix->Reset();

			hGPTank3->Transform( hMapScaleMatrix );//scale down

			if (m_spsTankStates.m_iPlayerNum == 3)
				hGR->DrawPath(hFriendPen, hGPTank3);
			else
				hGR->DrawPath(hEnemyPen, hGPTank3);
	}
	if (m_spsTankStates.m_bPlayer4Connected)
	{
			RectangleF hRTankBody = RectangleF(m_spsTankStates.m_fPlayer4X- (float)gkuiTankWidth / 2.0f,
										m_spsTankStates.m_fPlayer4Y - (float)gkuiTankHeight / 2.0f, 
										(float)gkuiTankWidth, (float)gkuiTankHeight);

			hGPTank4->AddRectangle( hRTankBody );

			//rotate from the center of the tank
			hMatrix->Reset ();

			hMatrix->RotateAt(-m_spsTankStates.m_fPlayer4Angle * 180 / gkfPI , 
									PointF(m_spsTankStates.m_fPlayer4X,
										   m_spsTankStates.m_fPlayer4Y));

			//apply the rotation and draw the thing
			hGPTank4->Transform( hMatrix );
			hMatrix->Reset();

			hGPTank4->Transform( hMapScaleMatrix );//scale down

			if (m_spsTankStates.m_iPlayerNum == 4)
				hGR->DrawPath(hFriendPen, hGPTank4);
			else
				hGR->DrawPath(hEnemyPen, hGPTank4);
	}

	
	//deterministic deletionationationation
	delete hGPTank1;
	delete hGPTank2;
	delete hGPTank3;
	delete hGPTank4;
	delete hGPObstacles;
	delete hGR;
	delete hMapScaleMatrix;
	delete hMatrix;
	delete hFriendPen;
	delete hEnemyPen;
	delete hObstaclePen;
	delete hBBSurf;
}

/***********************************************
Function: _DrawBullets

Description: Called by RenderTimer_Tick.
Positions and adds bullets to the scene.
***********************************************/
void Form1::_DrawBullets(void)
{
	MatrixStack ^ msBullets = gcnew MatrixStack;
	Material matBullet;
	matBullet.Ambient = Color::Silver;
	matBullet.Diffuse = Color::Silver;
	m_d3ddev->Material = matBullet;

	for each(sBullet sB in m_haBullets)
	{
		if(sB.m_iFiringPlayer != 0)
		{
			msBullets->Push();
			msBullets->RotateYawPitchRoll (
				sB.m_fBulletAngle + ((float)Math::PI * 3.0f) / 2.0f, 
				-((float)Math::PI * 3.0f) / 2.0f, 0.0f);
			msBullets->Scale(Vector3(gkfBulletScaling, gkfBulletScaling, gkfBulletScaling));
			msBullets->Translate(Vector3(sB.m_fBulletX, 7.0f, sB.m_fBulletY));
			m_d3ddev->SetTransform(TransformType::World, msBullets->Top);
			m_meshBullet->DrawSubset(0);			
			msBullets->Pop();
		}
	}
	delete msBullets;
}

/***********************************************
Function: _SetupLights

Description: Called by RenderTimer_Tick.
Positions and adds lights to the scene.
***********************************************/
void Form1::_SetupLights(void)
{
	m_d3ddev->Lights [0]->Type = LightType::Directional;
	//m_d3ddev->Lights [0]->Position = Vector3 (320.0f, 500.0f, 240.0f);
	m_d3ddev->Lights[0]->Direction = (Vector3(1,0,0));
	m_d3ddev->Lights [0]->Ambient = Color::DarkGray;
	m_d3ddev->Lights [0]->Diffuse = Color::DarkGray;
	//m_d3ddev->Lights [0]->Range = 1000.0f;
	//m_d3ddev->Lights [0]->
	m_d3ddev->Lights [0]->Update ();
	m_d3ddev->Lights [0]->Enabled = true;
}

/***********************************************
Function: _OpenChatBox

Description: Called by Form1_KeyDown when enter
is pressed. Opens and focuses a chat box so the
player can type a message into it.
***********************************************/
void Form1::_OpenChatBox ()
{
	if (UI_ChatBox->Visible == false)
	{
		//show the chat box!
		UI_ChatBox->Text = "";
		UI_ChatBox->Visible = true;
		UI_ChatBox->Enabled = true;
		UI_ChatBox->Focus ();

		//turning off the keys so the tank doesnt sit there spinning or
		//something while the user is typing
		Keys.m_bA = false;
		Keys.m_bD = false;
		Keys.m_bS = false;
		Keys.m_bSP = false;
		Keys.m_bW = false;

	}
	else
	{
		//hide the chat box
		UI_ChatBox->Text = "";
		UI_ChatBox->Visible = false;
		UI_ChatBox->Enabled = false;
		this->Focus ();
	}
}

/***********************************************
Function: UI_ChatBox_PreviewKeyDown

Description: Checks to see if enter or escape
is typed into the chatbox. If enter was pressed,
this function will send the message off to the
server. If escape is pressed this function will
close the chatbox without sending the message.
***********************************************/
System::Void Form1::UI_ChatBox_PreviewKeyDown (System::Object ^ sender, 
	System::Windows::Forms::PreviewKeyDownEventArgs ^ e)
{
	//array to hold the frame as its built
	array <unsigned char> ^ aucTxData = gcnew array <unsigned char> 
		(sizeof (EFrameType) + gkuiChatSize + sizeof (unsigned char));
	//the chat message
	array <wchar_t> ^ strText = UI_ChatBox->Text->ToCharArray ();

	switch (e->KeyCode)
	{
	case System::Windows::Forms::Keys::Enter :
		//dont do anything if the user didnt type anything
		if (strText->Length != 0)
		{
			//build the frame as a null terminated string
			//putting the frame type ID in front
			aucTxData [0] = eChatMessage;
			//next comes the number of the player sending the message
			aucTxData [1] = (unsigned char)m_spsTankStates.m_iPlayerNum;
			for (int i (0); i < strText->Length; ++i)
			{
				aucTxData [i + 2] = (unsigned char)strText [i];
			}
			aucTxData [strText->Length + 2] = 0; //null

			m_hConnection->Send (aucTxData);
		}
		//NO BREAK! FALL THROUGH!
	case System::Windows::Forms::Keys::Escape :
		//clear out the message and hide the chat box
		UI_ChatBox->Text = "";
		UI_ChatBox->Enabled = false;
		UI_ChatBox->Visible = false;
		this->Focus ();
		break;
	}

	delete aucTxData;
	delete strText;
}

/***********************************************
Function: _GetPlayerColor

Description: Creates a System::Drawing::Color
based on the player number passed to it.
***********************************************/
Color Form1::_GetPlayerColor (int iPlayerNum)
{
	//return a color based on iPlayerNum
	switch (iPlayerNum)
	{
	case 1:
		return Color::Red;
	case 2:
		return Color::Green;
	case 3:
		return Color::Blue;
	case 4:
		return Color::LightBlue;
	}

	//OH SHI-
	return Color::Black;
}