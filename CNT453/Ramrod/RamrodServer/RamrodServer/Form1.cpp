#include "stdafx.h"
#include "Form1.h"

using namespace RamrodServer;

void Form1::_AcceptCallback(IAsyncResult ^ ar)
{
	//try to finish up the connection, then send the completed connection to _HandleAccept
	try
	{
		Invoke(gcnew _DelVoidSock(this, &Form1::_HandleAccept), ((Socket ^)(ar->AsyncState))->EndAccept(ar));
	}
	catch(SocketException ^ e)
	{
		Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), "Form1::_AcceptCallback:: " + e->Message);
	}
	catch(ObjectDisposedException ^ e)
	{
		Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), "Form1::_AcceptCallback:: " + e->Message);
	}
}

void Form1::_HandleAccept(Socket ^ hSock)
{
	//this should never happen
	if(!hSock)
		return;

	//dont let more than 4 people play at a time
	if(m_iNumConnections >= 4)
	{
		//send the server full type frame to the client to notify that the server is full (4 players)
		sServerFull ssfTemp;
		ssfTemp.m_eFrameType = eServerFull;

		//array for sending the server full frame
		array<unsigned char> ^ aucTXData = gcnew array<unsigned char>(sizeof(sServerFull));
		//load the frame into the sending array
		for(unsigned int i(0); i < sizeof(sServerFull); ++i)
			aucTXData[i] = * (((unsigned char *)(&ssfTemp)) + i);

		try		//try sending the frame to the client
		{
			if(hSock->Connected)
				hSock->Send(aucTXData);
		}
		catch(SocketException ^ e)
		{
			System::Diagnostics::Trace::Write("Form1::_HandleAccept:: " + e->Message);
		}

		hSock->Close();		//close the socket and then start listening again
		try
		{
			//grab another connection
			m_hsListen->BeginAccept(gcnew AsyncCallback(this, &Form1::_AcceptCallback), m_hsListen);
		}
		catch(SocketException ^ e)
		{
			System::Diagnostics::Trace::Write("Form1::ServerInit:: " + e->Message);
			if(Windows::Forms::DialogResult::OK == MessageBox::Show("Could Not Open Listening Socket; Ramrod Closing", "Error Ramrod", Windows::Forms::MessageBoxButtons::OK))
				Close();
		}
		return;
	}

	//this for loop first goes through the players array until it finds the empty
	//player slot and then gives the new accepted socket to the empty slot
	for(int i(0); i < 4; ++i)
	{
		//is this slot open?
		if(m_haspPlayers[i]->m_shConnection == nullptr)
		{
			//yes! add the new player to the open slot
			m_haspPlayers[i]->m_shConnection = hSock;
			++m_iNumConnections;
			statlblConnections->Text = "Connections: " + m_iNumConnections;

			//initializing of the player coordinates and angle
			//and reset the players score
			_SpawnTank(m_haspPlayers[i]);
			m_haspPlayers [i]->m_uiPlayerDeaths = 0;
			m_haspPlayers [i]->m_uiPlayerKills = 0;

			//send the map to the player
			_SendMap (m_haspPlayers [i]->m_shConnection);

			try
			{
				//hSock->DontFragment = true;
				//hSock->NoDelay = true;
				m_haspPlayers[i]->m_shConnection->BeginReceive(
					m_haspPlayers[i]->m_aucRXData,
					0,
					sizeof(EFrameType),
					Sockets::SocketFlags::Peek,
					gcnew AsyncCallback(this, &Form1::_PeekCallback), m_haspPlayers[i]);
			}
			catch(SocketException ^ e)
			{
				//should probably null the socket and decrement number of players here (remove player)
				_HandleError("Form1::_HandleAccept:: " + e->Message);
				_KillSocket(m_haspPlayers[i]);
			}
			catch(ObjectDisposedException ^ e)
			{
				_HandleError("Form1::_HandleAccept:: " + e->Message);
			}
			//catch(ArgumentNullException ^ e)
			//{
			//	_HandleError("Form1::_HandleAccept:: " + e->Message);
			//}
			break;
		}
	}

	//restart the listening socket again
	try
	{
		m_hsListen->BeginAccept(gcnew AsyncCallback(this, &Form1::_AcceptCallback), m_hsListen);
	}
	catch(SocketException ^ e)
	{
		System::Diagnostics::Trace::Write("Form1::ServerInit:: " + e->Message);
		if(Windows::Forms::DialogResult::OK == MessageBox::Show("Could Not Open Listening Socket; Ramrod Closing", "Error Ramrod", Windows::Forms::MessageBoxButtons::OK))
			Close();
	}
} //Form1::_HandleAccept ()

void Form1::_PeekCallback (IAsyncResult ^ ar)
{
	sPlayer ^ sPlayerData = ((sPlayer^)ar->AsyncState);
	try
	{
		sPlayerData->m_shConnection->EndReceive (ar);
		Invoke (gcnew _DelVoidsPlayer (this, &Form1::_DetermineFrameType), sPlayerData);
	}
	catch (SocketException ^ e)
	{
		Invoke (gcnew _DelVoidString (this, &Form1::_HandleError), "Form1::_PeekCallback:: " + e->Message);
		Invoke (gcnew _DelVoidsPlayer (this, &Form1::_KillSocket), sPlayerData);
	}
	catch(ObjectDisposedException ^ e)
	{
		Invoke (gcnew _DelVoidString (this, &Form1::_HandleError), "Form1::_PeekCallback:: " + e->Message);
		//_KillSocket(sPlayerData);
	}
	catch(NullReferenceException ^ e)
	{
		Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), "Form1::_PeekCallback:: " + e->Message);
	}
}

void Form1::_DetermineFrameType (sPlayer ^ sPlayerData)
{
	//now that we have a confirmed response from the client, reset the timeout counter
	sPlayerData->m_uiCyclesSinceReply = 0;

	//switch based on EFrameType m_eFrameType member found in every frame
	switch (sPlayerData->m_aucRXData [0])
	{
		//frame contains keyboard state info
	case eKeyStates :
		//resize the rx buffer to handle a sKeyStates
		sPlayerData->m_aucRXData = gcnew array <unsigned char> (sizeof (sKeyStates));
		//rx the rest of the data as a key state frame
		try
		{
			sPlayerData->m_shConnection->BeginReceive (sPlayerData->m_aucRXData, 0, sizeof (sKeyStates),
				SocketFlags::None, gcnew AsyncCallback (this, &Form1::_ReceiveKeyStatesCallback),
				sPlayerData);
		}
		catch(SocketException ^ e)
		{
			System::Diagnostics::Trace::WriteLine ("Form1::_DetermineFrameType:: " + e->Message);
			_KillSocket(sPlayerData);
		}
		break;

	case eChatMessage:
		//resize the rx buffer to handle a chat message
		sPlayerData->m_aucRXData = gcnew array <unsigned char> 
			(sizeof (EFrameType) + gkuiChatSize + sizeof (unsigned char));
		//rx the rest of the data
		try
		{
			sPlayerData->m_shConnection->BeginReceive (sPlayerData->m_aucRXData, 0,
				sizeof (EFrameType) + gkuiChatSize + sizeof (unsigned char), SocketFlags::None,
				gcnew AsyncCallback (this, &Form1::_ReceiveChatMessageCallback),
				sPlayerData);
		}
		catch (SocketException ^ e)
		{
			System::Diagnostics::Trace::WriteLine ("Form1::_DetermineFrameType:: " + e->Message);
			_KillSocket (sPlayerData);
		}
		break;
		///////////////////////////////////////////////////////////////
		//		IF NEW FRAMES NEED TO BE HANDLED PUT CODE HERE!		 //
		///////////////////////////////////////////////////////////////
	}
}

void Form1::_ReceiveKeyStatesCallback(IAsyncResult ^ ar)
{
	sPlayer ^ sPlayerData = ((sPlayer^)ar->AsyncState);

	try
	{
		int iNumBytes = sPlayerData->m_shConnection->EndReceive(ar);
		if(iNumBytes == sizeof(sKeyStates))
		{
			try
			{
				Invoke(gcnew _DelVoidsPlayer(this, &Form1::_ReceiveKeyStatesAction), 
					sPlayerData);
			}
			catch (NullReferenceException ^ e)
			{
				System::Diagnostics::Trace::WriteLine 
					("Form1::_ReceiveKeyStatesCallback::NullReferenceException " + 
					e->Message);
			}
		}
		else
		{
			Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), "Form1::_ReceiveKeyStatesCallback:: Incorrect Number of Bytes Read");

			try
			{
				//resize the rx buffer and get another frame type,
				//ignoring the bad frame that was just recieved
				sPlayerData->m_aucRXData = gcnew array <unsigned char> (sizeof (EFrameType));
				sPlayerData->m_shConnection->BeginReceive(
					sPlayerData->m_aucRXData,
					0,
					sizeof(EFrameType),
					Sockets::SocketFlags::Peek,
					gcnew AsyncCallback(this, &Form1::_PeekCallback), 
					sPlayerData);
			}
			//if there was a socket exception kill the socket/player
			catch(SocketException ^ e)
			{
				Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), 
					"Form1::_ReceiveKeyStatesCallback:: " + e->Message);
				Invoke(gcnew _DelVoidsPlayer(this, &Form1::_KillSocket),
					((sPlayer^)ar->AsyncState));
			}
			catch(ObjectDisposedException ^ e)
			{
				Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), 
					"Form1::_ReceiveKeyStatesCallback:: " + e->Message);
			}
		}
	}
	//if there was a socket exception kill the socket/player
	catch(SocketException ^ e)
	{
		Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), "Form1::_ReceiveCallback:: " + e->Message);
		Invoke(gcnew _DelVoidsPlayer(this, &Form1::_KillSocket), ((sPlayer^)ar->AsyncState));
	}
	catch(ObjectDisposedException ^ e)
	{
		Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), "Form1::_ReceiveCallback:: " + e->Message);
	}
}

void Form1::_ReceiveChatMessageCallback(IAsyncResult ^ ar)
{
	sPlayer ^ sPlayerData = ((sPlayer^)ar->AsyncState);

	try
	{
		int iNumBytes = sPlayerData->m_shConnection->EndReceive(ar);
		if(iNumBytes == sizeof(EFrameType) + gkuiChatSize + sizeof (unsigned char))
		{
			try
			{
				Invoke(gcnew _DelVoidsPlayer(this, &Form1::_ReceiveChatMessageAction), 
					sPlayerData);
			}
			catch (NullReferenceException ^ e)
			{
				System::Diagnostics::Trace::WriteLine 
					("Form1::_ReceiveChatMessageCallback::NullReferenceException " + 
					e->Message);
			}
		}
		else
		{
			Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), "Form1::_ReceiveChatMEssageCallback:: Incorrect Number of Bytes Read");

			try
			{
				//resize the rx buffer and get another frame type,
				//ignoring the bad frame that was just recieved
				sPlayerData->m_aucRXData = gcnew array <unsigned char> (sizeof (EFrameType));
				sPlayerData->m_shConnection->BeginReceive(
					sPlayerData->m_aucRXData,
					0,
					sizeof(EFrameType),
					Sockets::SocketFlags::Peek,
					gcnew AsyncCallback(this, &Form1::_PeekCallback), 
					sPlayerData);
			}
			//if there was a socket exception kill the socket/player
			catch(SocketException ^ e)
			{
				Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), 
					"Form1::_ReceiveChatMessageCallback:: " + e->Message);
				Invoke(gcnew _DelVoidsPlayer(this, &Form1::_KillSocket),
					((sPlayer^)ar->AsyncState));
			}
			catch(ObjectDisposedException ^ e)
			{
				Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), 
					"Form1::_ReceiveChatMessageCallback:: " + e->Message);
			}
		}
	}
	//if there was a socket exception kill the socket/player
	catch(SocketException ^ e)
	{
		Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), "Form1::_ReceiveChatMessageCallback:: " + e->Message);
		Invoke(gcnew _DelVoidsPlayer(this, &Form1::_KillSocket), ((sPlayer^)ar->AsyncState));
	}
	catch(ObjectDisposedException ^ e)
	{
		Invoke(gcnew _DelVoidString(this, &Form1::_HandleError), "Form1::_ReceiveChatMessageCallback:: " + e->Message);
	}
}

void Form1::_ReceiveChatMessageAction (sPlayer ^ hspPlayer)
{
	//only unpacking this for diagnostics
	//really though the server doesnt care what text was rx'd
	char szText [gkuiChatSize] = {0};
	unsigned char * pT = reinterpret_cast <unsigned char*> (szText);
	for (unsigned int i (0); i < gkuiChatSize; ++i)
	{
		*pT = hspPlayer->m_aucRXData [i+2];
		pT++;
	}
	//show the text the was rx'd
	String ^ text = gcnew String (szText);
	System::Diagnostics::Trace::WriteLine
		("Received chat message \"" + text + "\" from player " + hspPlayer->m_aucRXData[1]);

	//echo the receive message back to the clients
	for each (sPlayer ^ player in m_haspPlayers)
	{
		//if connected...
		if (player->m_shConnection != nullptr)
		{
			try
			{
				//send chat message
				player->m_shConnection->Send (hspPlayer->m_aucRXData);
			}
			//kill the player if anything goes sideways
			catch(SocketException ^ e)
			{
				_HandleError("Form1::PackAndSend:: " + e->Message);
				_KillSocket(player);
			}
			catch(NullReferenceException ^ e)
			{
				_HandleError("Form1::PackAndSend:: " + e->Message);
			}
		}
	}

	try
	{
		//start grabbing the next frame
		hspPlayer->m_aucRXData = gcnew array <unsigned char> (sizeof (EFrameType));
		hspPlayer->m_shConnection->BeginReceive(
			hspPlayer->m_aucRXData,
			0,
			sizeof(EFrameType),
			Sockets::SocketFlags::Peek,
			gcnew AsyncCallback(this, &Form1::_PeekCallback), hspPlayer);
	}
	catch (SocketException ^ e)
	{
		System::Diagnostics::Trace::WriteLine ("_ReceiveChatMessageAction::SoxEx::" + e->Message);
		_KillSocket (hspPlayer);
	}
}

void Form1::_DrawObstacles (BufferedGraphics ^ hBufferedGr)
{
	//Pen ^ hPen = gcnew Pen (Color::Black);
	SolidBrush ^ hBr = gcnew SolidBrush (Color::Black);
	for each (SObstacle sOb in m_aObstacles )
	{
		hBr->Color = sOb.m_color;
		switch (sOb.m_eObstacleType)
		{
		case eEllipse:

			hBufferedGr->Graphics->FillEllipse (hBr, sOb.m_fObstacleX, sOb.m_fObstacleY, 
				sOb.m_fObstacleWidth, sOb.m_fObstacleHeight);
			break;
		case eRectangle:
			hBufferedGr->Graphics->FillRectangle (hBr, sOb.m_fObstacleX, sOb.m_fObstacleY,
				sOb.m_fObstacleWidth, sOb.m_fObstacleHeight);
			break;
		case eNone:
			continue;
		}
	}
}
bool Form1::_HitDetection( sPlayer ^ hTank )
{	
	Pen ^ hPen = gcnew Pen(Color::Red );
	SolidBrush ^ hBrush = gcnew SolidBrush( Color::Black );
	Graphics ^ hGrTemp = this->CreateGraphics();
	// create back buffer (this needs to be double-buffered otherwise
	//  the images will stack/flicker)
	BufferedGraphicsContext ^ hBGC = gcnew BufferedGraphicsContext;
	BufferedGraphics ^ hBufferedGr = hBGC->Allocate (hGrTemp, this->DisplayRectangle);
	// clear back buffer

	hBufferedGr->Graphics->Clear ( this->BackColor );

	//matrix for rotation
	Matrix ^ hRotationMatrix = gcnew Matrix;

	//flip coord system so y points up
	Matrix ^ mat = gcnew Matrix( 1, 0, 0, -1, 0, 0);
	hBufferedGr->Graphics->Transform = mat;
	hBufferedGr->Graphics->TranslateTransform( 0, (float)this->ClientSize.Height, MatrixOrder::Append );

	//path for bullet drawing
	GraphicsPath ^ hGPBullet = gcnew GraphicsPath;



	//we need to pack this into another function for detecting bullet hits and giving points to the
	//firing player, and probably call it from tmrsend_tick.....maybe :S....i'm half drunk :S
	for(unsigned int i(0); i < gkuiMaxBulletCount; ++i)
	{
		if(m_aBullets[i].m_iFiringPlayer != 0)
		{
			//tank body
			RectangleF hRBulletBody = RectangleF(m_aBullets[i].m_fBulletX ,
				m_aBullets[i].m_fBulletY, 5, 5);

			hGPBullet->AddRectangle( hRBulletBody );

			//rotate from the center of the tank
			hRotationMatrix->Reset ();
			hRotationMatrix->RotateAt(90.0f - m_aBullets[i].m_fBulletAngle, 
				PointF(m_aBullets[i].m_fBulletX + 2.5f, m_aBullets[i].m_fBulletY + 2.5f));

			//apply the rotation and draw the thing
			hGPBullet->Transform( hRotationMatrix );
			hBufferedGr->Graphics->DrawPath( hPen, hGPBullet );
		}
	}

	//this is a tester for the tank to tank hit detection
	if(_TankToTank( hTank, hBufferedGr ))
	{
		System::Diagnostics::Trace::WriteLine("Tank-To-Tank Collision For Player: " + hTank->m_iPlayerNum);
		return true;
	}
	else if(_TankToObstacle( hTank, hBufferedGr ))
	{
		System::Diagnostics::Trace::WriteLine("Tank-To-Obstacle Collision For Player: " + hTank->m_iPlayerNum);
		return true;
	}
	else 
		return false;
}

bool Form1::_TankToObstacle( sPlayer ^ hsTank, BufferedGraphics ^ hBufferedGr )
{
	GraphicsPath ^ hObstaclePath = gcnew GraphicsPath;

	GraphicsPath ^ hGPTank;
	SolidBrush ^ hBrush = gcnew SolidBrush( Color::Black );


	//_BuildTank makes a tank graphics path object from an sPlayer handle
	hGPTank = _BuildTank( hsTank );

	//adding each obstacle to the obstacle path
	for (int i(0); i < gkuiMaxObstacleCount; ++i)  
	{
		//if the obstacle is close to the tank add it to the path
		//if ( Math::Abs( m_aObstacles[i].m_fObstacleX - hsTank->m_fXPos ) < 100 &&
		//	Math::Abs( m_aObstacles[i].m_fObstacleY - hsTank->m_fYPos ) < 100 )
		if(_ObstacleCloseToPoint(RectangleF(m_aObstacles[i].m_fObstacleX, m_aObstacles[i].m_fObstacleY, m_aObstacles[i].m_fObstacleWidth,
			m_aObstacles[i].m_fObstacleHeight), PointF(hsTank->m_fXPos, hsTank->m_fYPos)))
		{
			_AddObstacleToPath (m_aObstacles [i], hObstaclePath);
		}
	}
	//put the path into a region so that we can check
	//intersections!  get rid of the path asap
	Drawing::Region ^ hObstacleRegion = gcnew Drawing::Region( hObstaclePath );
	hBufferedGr->Graphics->FillRegion( hBrush, hObstacleRegion);
	delete hObstaclePath;


	//make a tank region from the graphics path that _BuildTank made
	Drawing::Region ^ hTankRegion = gcnew Drawing::Region( hGPTank );
	hBufferedGr->Graphics->FillRegion( hBrush, hTankRegion );

	//copy the tank region into the tank and obstacle region
	Drawing::Region ^ hTank_Obstacle = hTankRegion->Clone();

	hTank_Obstacle->Intersect( hObstacleRegion );

	//if there is an intersection, return true
	//otherwise return false
	if (hTank_Obstacle->IsEmpty( hBufferedGr->Graphics ))
	{
		/////delete all of the gcnew junk/////
		delete hTankRegion;
		delete hObstacleRegion;
		delete hTank_Obstacle;
		return false;
	}
	else 
	{
		/////delete all of the gcnew junk/////
		delete hTankRegion;
		delete hObstacleRegion;
		delete hTank_Obstacle;
		return true;
	}
}

void Form1::_BulletToObstacle (BufferedGraphics ^ hBufferedGr)
{
	SolidBrush ^ hBrush = gcnew SolidBrush (Color::Red);

	GraphicsPath ^ hGPBullet = gcnew GraphicsPath;
	GraphicsPath ^ hGPMap = gcnew GraphicsPath;

	for each (sBullet % sBul in m_aBullets)
	{
		if (sBul.m_iFiringPlayer != 0)
		{
			//draw the bullet in
			hGPBullet->Reset ();
			hGPBullet->AddRectangle (RectangleF (
				sBul.m_fBulletX, sBul.m_fBulletY,
				(float)gkuiBulletWidth, (float)gkuiBulletHeight));

			//draw in the obstacles close to the bullet
			hGPMap->Reset ();
			for each (SObstacle % sOb in m_aObstacles)
			{
				//only add obstacles that are close
				if (_ObstacleCloseToPoint (RectangleF 
					(sOb.m_fObstacleX, sOb.m_fObstacleY,
					sOb.m_fObstacleWidth, sOb.m_fObstacleHeight),
					PointF (sBul.m_fBulletX, sBul.m_fBulletY)))
				{
					_AddObstacleToPath (sOb, hGPMap);
				}
			}

			//convert both to regions so we can work with them
			System::Drawing::Region ^ hRegBullet = 
				gcnew System::Drawing::Region (hGPBullet);
			hBufferedGr->Graphics->FillRegion (hBrush, hRegBullet);
			System::Drawing::Region ^ hRegMap = 
				gcnew System::Drawing::Region (hGPMap);
			hBufferedGr->Graphics->FillRegion (hBrush, hRegMap);

			//check for a hit
			hRegBullet->Intersect (hRegMap);

			//if there was a hit, delete the bullet
			if (!hRegBullet->IsEmpty (hBufferedGr->Graphics))
			{
				sBul.m_iFiringPlayer = 0;
			}

			delete hRegBullet;
			delete hRegMap;
		}
	}
	delete hGPBullet;
	delete hGPMap;
	delete hBrush;
}

bool Form1::_ObstacleCloseToPoint (RectangleF obstacleLoc, PointF pointLoc)
{
	return (
		//within bottom left?
		pointLoc.X >= obstacleLoc.X - gkuiDetectRange && 
		pointLoc.Y >= obstacleLoc.Y - gkuiDetectRange &&
		//within bottom right?
		pointLoc.X <= obstacleLoc.X + obstacleLoc.Width + gkuiDetectRange &&
		pointLoc.Y >= obstacleLoc.Y - gkuiDetectRange &&
		//within top left?
		pointLoc.X >= obstacleLoc.X - gkuiDetectRange &&
		pointLoc.Y <= obstacleLoc.Y + obstacleLoc.Height + gkuiDetectRange &&
		//within top right?
		pointLoc.X <= obstacleLoc.X + obstacleLoc.Width + gkuiDetectRange &&
		pointLoc.Y <= obstacleLoc.Y + obstacleLoc.Height + gkuiDetectRange);
}

void Form1::_AddObstacleToPath (SObstacle % sOb, GraphicsPath ^ hGP)
{
	//put the obstacle into the path depending on what type it is
	switch (sOb.m_eObstacleType)
	{
	case eRectangle :
		hGP->AddRectangle (RectangleF (sOb.m_fObstacleX, sOb.m_fObstacleY,
			sOb.m_fObstacleWidth, sOb.m_fObstacleHeight));
		break;

	case eEllipse :
		hGP->AddEllipse (RectangleF (sOb.m_fObstacleX, sOb.m_fObstacleY,
			sOb.m_fObstacleWidth, sOb.m_fObstacleHeight));
		break;

	case eNone :
		break;
	}
}

GraphicsPath ^ Form1::_BuildTank( sPlayer ^ hsTank )
{
	GraphicsPath ^ hGPTank = gcnew GraphicsPath;

	//matrix for rotation
	Matrix ^ hRotationMatrix = gcnew Matrix;

	//make the rectangle and add it to the path in one step! holy moly!
	hGPTank->AddRectangle( RectangleF( 
		hsTank->m_fXPos - (float)gkuiTankWidth/2.0f,
		hsTank->m_fYPos - (float)gkuiTankHeight/2.0f, 
		(float)gkuiTankWidth, (float)gkuiTankHeight));

	//hRotationMatrix->Translate(hsTank->m_fXPos, hsTank->m_fYPos);
	hRotationMatrix->Reset();

	//make the rotation matrix
	hRotationMatrix->RotateAt(-( hsTank->m_fAngle * 180 / gkfPI ),
		PointF( hsTank->m_fXPos, hsTank->m_fYPos ));
	//apply it to the tank
	hGPTank->Transform( hRotationMatrix );
	delete hRotationMatrix;
	return hGPTank;
}
void Form1::_ReceiveKeyStatesAction(sPlayer ^ hspPlayer)
{
	//unpack the received frame
	sKeyStates sksTemp;
	unsigned char * pT = reinterpret_cast<unsigned char*>(&sksTemp);
	for(unsigned int i(0); i < sizeof(sKeyStates); ++i)
	{
		*pT = hspPlayer->m_aucRXData[i];
		pT++;
	}

	//save the keys that are pressed
	hspPlayer->m_sksKeyStates.m_bA = sksTemp.m_bA;
	hspPlayer->m_sksKeyStates.m_bD = sksTemp.m_bD;
	hspPlayer->m_sksKeyStates.m_bS = sksTemp.m_bS;
	hspPlayer->m_sksKeyStates.m_bW = sksTemp.m_bW;
	hspPlayer->m_sksKeyStates.m_bSP = sksTemp.m_bSP;

	try
	{
		//start grabbing the next frame
		hspPlayer->m_shConnection->BeginReceive(
			hspPlayer->m_aucRXData,
			0,
			sizeof(EFrameType),
			Sockets::SocketFlags::Peek,
			gcnew AsyncCallback(this, &Form1::_PeekCallback), hspPlayer);
	}
	//if there was a socket exception kill the socket/player
	catch(SocketException ^ e)
	{
		_HandleError("Form1::_ReceiveKeySatesAction:: " + e->Message);
		_KillSocket(hspPlayer);
	}
	catch(ObjectDisposedException ^ e)
	{
		_HandleError("Form1::_ReceiveKeyStatesAction:: " + e->Message);
	}
} 


void Form1::_AttemptToMove (sPlayer ^ hspPlayer)
{
	//save the players current location and heading in case of hit
	sPlayer spRevert;
	spRevert.m_fAngle = hspPlayer->m_fAngle;
	spRevert.m_fXPos = hspPlayer->m_fXPos;
	spRevert.m_fYPos = hspPlayer->m_fYPos;

	//if the user is pressing the 'a' key decrease their angle by pi/16 and check for wrapping
	//if the angle is already 0 then set the new angle to 2pi - pi/16
	if(hspPlayer->m_sksKeyStates.m_bA)
	{
		if(hspPlayer->m_fAngle <= 0)
			hspPlayer->m_fAngle = (2 * gkfPI) - (gkfPI / 32);
		else
			hspPlayer->m_fAngle -= gkfPI / 32;
	}

	//if the user is pressing the 'd' key increase the angle by pi/16 and if it reaches 2pi
	//then set it to 0 (wrapping)
	if(hspPlayer->m_sksKeyStates.m_bD)
	{	
		hspPlayer->m_fAngle += gkfPI / 32;
		if(hspPlayer->m_fAngle >= gkfPI * 2)
			hspPlayer->m_fAngle = 0;
	}

	//if the user is pressing the 'w' key (forward) calculate the new x and y coords 
	if(hspPlayer->m_sksKeyStates.m_bW)
	{
		hspPlayer->m_fXPos += static_cast<float>(Math::Sin(Convert::ToDouble(hspPlayer->m_fAngle)) * gkuiTankSpeed);
		hspPlayer->m_fYPos += static_cast<float>(Math::Cos(Convert::ToDouble(hspPlayer->m_fAngle)) * gkuiTankSpeed);
	}

	//if the user is pressing the 's' key (reverse) calculate the new x and y coords
	if(hspPlayer->m_sksKeyStates.m_bS)
	{
		hspPlayer->m_fXPos -= static_cast<float>(Math::Sin(Convert::ToDouble(hspPlayer->m_fAngle)) * gkuiTankSpeed);
		hspPlayer->m_fYPos -= static_cast<float>(Math::Cos(Convert::ToDouble(hspPlayer->m_fAngle)) * gkuiTankSpeed);
	}

	//if the players new coords are invalid, move him back
	if(_HitDetection(hspPlayer))
	{
		hspPlayer->m_fAngle = spRevert.m_fAngle;
		hspPlayer->m_fXPos = spRevert.m_fXPos;
		hspPlayer->m_fYPos = spRevert.m_fYPos;
	}
}

void Form1::_AttemptToShoot (sPlayer ^ hspPlayer)
{
	//if the user is pressing the space bar and shot timer is clear
	//create a new bullet from the player, from their coords
	if(hspPlayer->m_sksKeyStates.m_bSP && hspPlayer->m_uiShotTimer == 0)
	{
		for(unsigned int i(0); i < gkuiMaxBulletCount; ++i)	//go through the array of bullets
		{
			if(m_aBullets[i].m_iFiringPlayer == 0)	//check to see if the bullet is available/inactive
			{
				m_aBullets[i].m_iFiringPlayer = hspPlayer->m_iPlayerNum;	//set the firing player
				m_aBullets[i].m_fBulletAngle = hspPlayer->m_fAngle;	//set the angle to that of the player firing

				//center the bullet on the front of the tank firing
				//todo: this assumes that the tank is a circle when it is a rectangle,
				//the result is that the bullets dont start at the exact front of the tank
				//while not exact, this is probably good enough
				m_aBullets[i].m_fBulletX = 
					hspPlayer->m_fXPos +
					(float)Math::Sin (hspPlayer->m_fAngle) * gkfTankHypot;
				m_aBullets[i].m_fBulletY = 
					hspPlayer->m_fYPos + 
					(float)Math::Cos (hspPlayer->m_fAngle) * gkfTankHypot;

				hspPlayer->m_uiShotTimer = gkuiShotCycles;	//set the shot timer for the player
				break;
			}
		}
	}
}

void Form1::_HandleError(String ^ hStr)
{
	System::Diagnostics::Trace::Write(hStr);
}
void Form1::ServerInit(void)
{
	if(m_hsListen != nullptr)
	{
		m_hsListen->Close();
		delete m_hsListen;
		m_hsListen = nullptr;
	}

	_InitBullets ();

	try
	{
		//try to start the listening socket
		m_hsListen = gcnew Socket(AddressFamily::InterNetwork,
			SocketType::Stream,
			ProtocolType::Tcp);

		IPAddress ^ hostIP = IPAddress::Any;
		IPEndPoint ^ ep = gcnew IPEndPoint(hostIP, 1666);
		m_hsListen->Bind(ep);
		m_hsListen->Listen(20);
		statlblStatus->ForeColor = Color::Green;

		//grab a user
		m_hsListen->BeginAccept(gcnew AsyncCallback(this, &Form1::_AcceptCallback), m_hsListen);
	}
	catch(SocketException ^ e)
	{
		//OH SHI-
		System::Diagnostics::Trace::Write("Form1::ServerInit:: " + e->Message);
		if(Windows::Forms::DialogResult::OK == MessageBox::Show("Could Not Open Listening Socket; Ramrod Closing", "Error Ramrod", Windows::Forms::MessageBoxButtons::OK))
			Close();
	}
}

void Form1::_BuildMap (void)
{
	GraphicsPath ^ hGPMap = gcnew GraphicsPath;	//holds the map as it is
	GraphicsPath ^ hGPCurOb; //holds the obstacle being built
	System::Drawing::Region ^ hRMap = gcnew System::Drawing::Region; //holds the valid map
	System::Drawing::Region ^ hRCurOb; //holds the obstacle being built
	Graphics ^ hGr = this->CreateGraphics (); //used to satisfy calls to Region::IsEmpty ()

	//build a rectangle around the edge of the map
	//left
	m_aObstacles [0].m_eFrameType = eObstacle;
	m_aObstacles [0].m_iObstacleNumber = 0;
	m_aObstacles [0].m_eObstacleType = eRectangle;
	m_aObstacles [0].m_fObstacleX = 0;
	m_aObstacles [0].m_fObstacleY = 0;
	m_aObstacles [0].m_fObstacleHeight = 480;
	m_aObstacles [0].m_fObstacleWidth = 10;
	m_aObstacles [0].m_color = Color::DodgerBlue;

	//right
	m_aObstacles [1].m_eFrameType = eObstacle;
	m_aObstacles [1].m_iObstacleNumber = 1;
	m_aObstacles [1].m_eObstacleType = eRectangle;
	m_aObstacles [1].m_fObstacleX = 630;
	m_aObstacles [1].m_fObstacleY = 0;
	m_aObstacles [1].m_fObstacleHeight = 480;
	m_aObstacles [1].m_fObstacleWidth = 10;
	m_aObstacles [1].m_color = Color::Red;

	//top
	m_aObstacles [2].m_eFrameType = eObstacle;
	m_aObstacles [2].m_iObstacleNumber = 2;
	m_aObstacles [2].m_eObstacleType = eRectangle;
	m_aObstacles [2].m_fObstacleX = 0;
	m_aObstacles [2].m_fObstacleY = 0;
	m_aObstacles [2].m_fObstacleHeight = 10;
	m_aObstacles [2].m_fObstacleWidth = 640;
	m_aObstacles [2].m_color = Color::Green;

	//bottom
	m_aObstacles [3].m_eFrameType = eObstacle;
	m_aObstacles [3].m_iObstacleNumber = 3;
	m_aObstacles [3].m_eObstacleType = eRectangle;
	m_aObstacles [3].m_fObstacleX = 0;
	m_aObstacles [3].m_fObstacleY = 470;
	m_aObstacles [3].m_fObstacleHeight = 10;
	m_aObstacles [3].m_fObstacleWidth = 640;
	m_aObstacles [3].m_color = Color::Blue;

	//put the walls in the path
	for (int i (0); i < 4; ++i)
	{
		_AddObstacleToPath (m_aObstacles [i], hGPMap);
	}

	hRMap = gcnew System::Drawing::Region (hGPMap);

	//now throw some objects around randomly
	for (int iOb (4); iOb < gkuiMaxObstacleCount; ++iOb)
	{
		//build an obstacle
		m_aObstacles [iOb].m_eFrameType = eObstacle;
		m_aObstacles [iOb].m_iObstacleNumber = iOb;
		m_aObstacles [iOb].m_eObstacleType = _GetRandObstacle ();
		m_aObstacles [iOb].m_fObstacleX = (float) _GetRandInt (10, 565);
		m_aObstacles [iOb].m_fObstacleY = (float) _GetRandInt (10, 405);
		m_aObstacles [iOb].m_fObstacleHeight = (float)_GetRandInt (25, 75);
		m_aObstacles [iOb].m_fObstacleWidth = (float)_GetRandInt (25,75);
		m_aObstacles [iOb].m_color = Color::FromArgb (_GetRandInt (0, 255),
			_GetRandInt (0, 255), _GetRandInt (0,255));

		//make the obstacle into a region
		hGPCurOb = gcnew GraphicsPath;
		_AddObstacleToPath (m_aObstacles [iOb], hGPCurOb);
		hRCurOb = gcnew System::Drawing::Region (hGPCurOb);
		//build the current map into a region
		hRMap = gcnew System::Drawing::Region (hGPMap);

		//check to see if the new obstacle overlaps an old one
		hRCurOb->Intersect (hRMap);

		if (hRCurOb->IsEmpty (hGr))
		{
			//new obstacle is fine, add it the map for the next round
			hGPMap->AddPath (hGPCurOb, false);
		}
		else
		{
			//obstacle intersects with another one, rebuild it
			--iOb;
			continue;
		}
	}

	////clear the rest of the obstacles
	//for (int iOb (4); iOb < gkuiMaxObstacleCount; ++iOb)
	//{
	//	m_aObstacles [iOb].m_eFrameType = eObstacle;
	//	m_aObstacles [iOb].m_iObstacleNumber = iOb;
	//	m_aObstacles [iOb].m_eObstacleType = eNone;
	//	//the rest of the values are unimportant as the obstacle should
	//	//not be drawn with a type of eNone
	//}
}

void Form1::_SendMap (Socket ^ hClient)
{
	//tx buffer, holds the data for an obstacle while it is being built and sent
	array <unsigned char> ^ hTxBuf = gcnew array <unsigned char> (sizeof (SObstacle));

	//pack every obstacle and send it to the client
	for each (SObstacle sOb in m_aObstacles)
	{
		//pack the obstacle
		for(unsigned int i(0); i < sizeof(SObstacle); ++i)
			hTxBuf [i] = * (((unsigned char *)(&sOb)) + i);

		try
		{
			hClient->Send (hTxBuf);
		}
		catch (SocketException ^ e)
		{
			_HandleError ("Form1::_SendMap ()::SocketException::" + e->Message);
		}
		catch (NullReferenceException ^ e)
		{
			_HandleError ("Form1::_SendMap ()::NullReferenceException::" + e->Message);
		}
	}
}

int Form1::_GetRandInt (int iLow, int iHigh)
{
	//for some reason, the .Net random generator is inclusive on the lower
	//bound and exclusive on the upper
	return m_rand->Next (iLow, iHigh + 1);
}

EObstacleType Form1::_GetRandObstacle ()
{
	//pick a random number then give the corresponding obstacle type
	switch (_GetRandInt (1, 2))
	{
	case 1:
		return eEllipse;
	case 2:
		return eRectangle;
	default:
		return eRectangle;
	}
}

Form1::Form1(void)
{
	InitializeComponent();
	m_hsListen = nullptr;
	m_haspPlayers = gcnew array<sPlayer ^>(4);
	//m_aucRXData = gcnew array<unsigned char>(sizeof(EFrameType));
	m_aObstacles = gcnew array <SObstacle> (gkuiMaxObstacleCount);
	m_rand = gcnew System::Random;
	m_aBullets = gcnew array<sBullet>(gkuiMaxBulletCount);

	for(unsigned int i(0); i < 4; ++i)	//initializing of the player structures
	{
		m_haspPlayers[i] = gcnew sPlayer;
		m_haspPlayers[i]->m_iPlayerNum = i + 1;
		m_haspPlayers[i]->m_shConnection = nullptr;
		m_haspPlayers[i]->m_uiCyclesSinceReply = 0;
		m_haspPlayers[i]->m_uiShotTimer = 0;
		m_haspPlayers[i]->m_sksKeyStates.m_bA = false;
		m_haspPlayers[i]->m_sksKeyStates.m_bD = false;
		m_haspPlayers[i]->m_sksKeyStates.m_bS = false;
		m_haspPlayers[i]->m_sksKeyStates.m_bSP = false;
		m_haspPlayers[i]->m_sksKeyStates.m_bW = false;
		m_haspPlayers[i]->m_aucRXData = gcnew array<unsigned char>(sizeof(EFrameType));
	}

	for(unsigned int i(0); i < gkuiMaxBulletCount; ++i)	//initializing the bullet array to all inactive(0)
		m_aBullets[i].m_iFiringPlayer = 0;

	m_iNumConnections = 0;	//initialize number of connections to 0

	statlblStatus->Text = "Server Ramrod";
	statlblStatus->ForeColor = Color::Red;
	statlblConnections->Text = "Connections: " + m_iNumConnections;
}

System::Void Form1::Form1_Shown(System::Object^  sender, System::EventArgs^  e)
{
	static bool bShown (false);
	if (!bShown)
	{
		//start things up!
		ServerInit();
		_BuildMap ();
		bShown = true;
	}
}

void Form1::_CheckForTimeouts ()
{
	//for each player, if they are connected increment the CyclesSinceReply
	//member, and if the member reaches 20 then delete the socket and set it to null
	//also, take care of decrementing the shot timer if it's active for each player
	for(int i(0); i < 4; ++i)
	{
		if(m_haspPlayers[i]->m_shConnection != nullptr)
		{
			++m_haspPlayers[i]->m_uiCyclesSinceReply;
			if(m_haspPlayers[i]->m_uiCyclesSinceReply == 20)
				_KillSocket(m_haspPlayers[i]);

			//decrement the shot timer if it's greater than 0
			if(m_haspPlayers[i]->m_uiShotTimer > 0)
				--m_haspPlayers[i]->m_uiShotTimer;
		}
	}
}

void Form1::_PackAndSend ()
{
	sPlayerStates spsTemp;	//new temporary frame for sending out the player states
	spsTemp.m_eFrameType = ePlayerState;	//set the frame type

	//set player 1 connected variable and if connected then set angle(converted to degrees), x and y
	spsTemp.m_bPlayer1Connected = m_haspPlayers[0]->m_shConnection != nullptr;
	if(spsTemp.m_bPlayer1Connected)
	{
		//spsTemp.m_fPlayer1Angle = m_haspPlayers[0]->m_fAngle * 180 / gkfPI;
		spsTemp.m_fPlayer1Angle = m_haspPlayers[0]->m_fAngle;
		spsTemp.m_fPlayer1X = m_haspPlayers[0]->m_fXPos;
		spsTemp.m_fPlayer1Y = m_haspPlayers[0]->m_fYPos;
		spsTemp.m_uiPlayer1Deaths = m_haspPlayers [0]->m_uiPlayerDeaths;
		spsTemp.m_uiPlayer1Kills = m_haspPlayers [0]->m_uiPlayerKills;
	}
	//set player 2 connected variable and if connected then set angle(converted to degrees), x and y
	spsTemp.m_bPlayer2Connected = m_haspPlayers[1]->m_shConnection != nullptr;
	if(spsTemp.m_bPlayer2Connected)
	{
		//spsTemp.m_fPlayer2Angle = m_haspPlayers[1]->m_fAngle * 180 / gkfPI;
		spsTemp.m_fPlayer2Angle = m_haspPlayers[1]->m_fAngle;
		spsTemp.m_fPlayer2X = m_haspPlayers[1]->m_fXPos;
		spsTemp.m_fPlayer2Y = m_haspPlayers[1]->m_fYPos;
		spsTemp.m_uiPlayer2Deaths = m_haspPlayers [1]->m_uiPlayerDeaths;
		spsTemp.m_uiPlayer2Kills = m_haspPlayers [1]->m_uiPlayerKills;

	}
	//set player 3 connected variable and if connected then set angle(converted to degrees), x and y
	spsTemp.m_bPlayer3Connected = m_haspPlayers[2]->m_shConnection != nullptr;
	if(spsTemp.m_bPlayer3Connected)
	{
		//spsTemp.m_fPlayer3Angle = m_haspPlayers[2]->m_fAngle * 180 / gkfPI;
		spsTemp.m_fPlayer3Angle = m_haspPlayers[2]->m_fAngle;		
		spsTemp.m_fPlayer3X = m_haspPlayers[2]->m_fXPos;
		spsTemp.m_fPlayer3Y = m_haspPlayers[2]->m_fYPos;
		spsTemp.m_uiPlayer3Deaths = m_haspPlayers [2]->m_uiPlayerDeaths;
		spsTemp.m_uiPlayer3Kills = m_haspPlayers [2]->m_uiPlayerKills;
	}
	//set player 4 connected variable and if connected then set angle(converted to degrees), x and y
	spsTemp.m_bPlayer4Connected = m_haspPlayers[3]->m_shConnection != nullptr;
	if(spsTemp.m_bPlayer4Connected)
	{
		//spsTemp.m_fPlayer4Angle = m_haspPlayers[3]->m_fAngle * 180 / gkfPI;
		spsTemp.m_fPlayer4Angle = m_haspPlayers[3]->m_fAngle;
		spsTemp.m_fPlayer4X = m_haspPlayers[3]->m_fXPos;
		spsTemp.m_fPlayer4Y = m_haspPlayers[3]->m_fYPos;
		spsTemp.m_uiPlayer4Deaths = m_haspPlayers [3]->m_uiPlayerDeaths;
		spsTemp.m_uiPlayer4Kills = m_haspPlayers [3]->m_uiPlayerKills;
	}

	array<unsigned char> ^ aucTXData = gcnew array<unsigned char>(sizeof(sPlayerStates) + (sizeof(sBullet) * gkuiMaxBulletCount));
	//reading the spsTemp data into the array
	for(unsigned int i(0); i < sizeof(sPlayerStates); ++i)
		aucTXData[i] = * (((unsigned char *)(&spsTemp)) + i);
	//reading the array of bullets into the array
	for(unsigned int iBullet(0); iBullet < gkuiMaxBulletCount; ++iBullet)
	{
		//make a copy of the bullet on the stack so we can index into it
		sBullet sTempBullet (m_aBullets [iBullet]);
		//sTempBullet.m_fBulletAngle = sTempBullet.m_fBulletAngle * 180 / gkfPI;
		
		//index into each bullet and place it in the TXBuffer one byte at a time
		for (unsigned int iByte(0); iByte < sizeof (sBullet); ++iByte)
		{
			aucTXData[iByte + iBullet * sizeof (sBullet) + sizeof(sPlayerStates)] = 
				* (((unsigned char *)(&sTempBullet)) + iByte);
		}
	}

	//go through the m_haspPlayers array and attempt to send to each connected player
	for(int iPlayer(0); iPlayer < 4; ++iPlayer)
	{
		if(m_haspPlayers[iPlayer]->m_shConnection != nullptr)
		{
			//update the player number and then re-read in the first part of the structure
			//(up to and including the player #) into the sending array.  This is because
			//each sPlayerStates frame includes the unique player number that's receiving the frame
			spsTemp.m_iPlayerNum = m_haspPlayers[iPlayer]->m_iPlayerNum;
			spsTemp.m_iShotTimer = m_haspPlayers[iPlayer]->m_uiShotTimer;
			for(unsigned int iByte(0); iByte < sizeof(EFrameType) + sizeof(int) * 2; ++iByte)
				aucTXData[iByte] = * (((unsigned char *)(&spsTemp)) + iByte);

			try
			{
				if(m_haspPlayers[iPlayer]->m_shConnection->Connected)
				{
					//System::Diagnostics::Trace::WriteLine ("Sending " + aucTXData->Length + " bytes.");
					m_haspPlayers[iPlayer]->m_shConnection->Send(aucTXData);
				}
				////kill the socket/player if it's not connected
				//else
				//{
				//	_KillSocket(m_haspPlayers[i]);
				//}
			}
			catch(SocketException ^ e)
			{
				_HandleError("Form1::PackAndSend:: " + e->Message);
				_KillSocket(m_haspPlayers[iPlayer]);
			}
			catch(NullReferenceException ^ e)
			{
				_HandleError("Form1::PackAndSend:: " + e->Message);
			}
		} // if (m_haspPlayers...
	} // for (int iPlayer(0)...
}

System::Void Form1::tmrSend_Tick(System::Object^  sender, System::EventArgs^  e)
{
	if(m_iNumConnections < 1)
		return;

	//check for response timeouts; also decrements shot timer if active
	_CheckForTimeouts();

	//try moving all the players
	for each (sPlayer ^% rhPlayer in this->m_haspPlayers)
	{
		if(rhPlayer->m_shConnection != nullptr)
		{
			_AttemptToMove (rhPlayer);
		}
	}

	//update the bullets and prune the ones that hit something
	_MoveBullets ();

	//these will be used to detect the bullets hitting an obstacle
	BufferedGraphicsContext ^ hBGC = gcnew BufferedGraphicsContext;
	BufferedGraphics ^ hBufferedGr = hBGC->Allocate 
		(this->CreateGraphics (), this->DisplayRectangle);

	//prune the bullets which hit something
	_BulletToObstacle (hBufferedGr);
	_BulletToTank (hBufferedGr);

	//let the players shoot
	for each (sPlayer ^% rhPlayer in this->m_haspPlayers)
	{
		_AttemptToShoot (rhPlayer);
	}

	//send the results to the connected clients
	_PackAndSend ();

	//draws the scene out on the server side
	_DrawScene(hBufferedGr);

	delete hBufferedGr;
	delete hBGC;

} // tmrSend_Tick ()

void Form1::_MoveBullets ()
{
	//for each (sBullet sBul in m_aBullets)
	for (int iBulNum (0); iBulNum < gkuiMaxBulletCount; ++iBulNum)
	{
		//if the bullet is active
		if (m_aBullets [iBulNum].m_iFiringPlayer)
		{
			//update the bullets location based on trajectory
			m_aBullets [iBulNum].m_fBulletX += (float)(Math::Sin (m_aBullets [iBulNum].m_fBulletAngle) * gkuiBulletSpeed);
			m_aBullets [iBulNum].m_fBulletY += (float)(Math::Cos (m_aBullets [iBulNum].m_fBulletAngle) * gkuiBulletSpeed);

			//kill the bullet if it goes outside
			//shouldnt be necessary but this is a catch-all type measure
			if (m_aBullets [iBulNum].m_fBulletX > 640 || m_aBullets [iBulNum].m_fBulletX < 0 ||
				m_aBullets [iBulNum].m_fBulletY > 480 || m_aBullets [iBulNum].m_fBulletY < 0)
				m_aBullets [iBulNum].m_iFiringPlayer = 0;
		}
	}
}

void Form1::_InitBullets ()
{
	//just start off each bullet in a "normal" state
	for each (sBullet sBul in m_aBullets)
	{
		sBul.m_iFiringPlayer = 0;
		sBul.m_fBulletAngle = 0.0f;
		sBul.m_fBulletX = 0.0f;
		sBul.m_fBulletY = 0.0f;
	}
}

void Form1::_KillSocket(sPlayer ^ sPlayerToKill)
{
	//something bad happened to that players connection
	//make a note of it, then proceed to kill the connection
	System::Diagnostics::Trace::WriteLine ("Killing a socket! Player: " + 
		sPlayerToKill->m_iPlayerNum.ToString ());
	delete sPlayerToKill->m_shConnection;
	sPlayerToKill->m_shConnection = nullptr;

	//recalculating number of connections because this function can
	//be called more then once when a connection dies
	m_iNumConnections = 0;
	for(unsigned int i(0); i < 4; ++i)
	{
		if(m_haspPlayers[i]->m_shConnection != nullptr)
			++m_iNumConnections;
	}
	statlblConnections->Text = "Connections: " + m_iNumConnections;
}

void Form1::_SpawnTank(sPlayer ^ hspPlayer)
{
	//initializing of the player coordinates, angle and state compensating for laying tank on other objects; using hit detection
	do
	{
		//spawn the tank at a random location
		hspPlayer->m_fXPos = static_cast<float>(_GetRandInt(15+gkuiTankWidth/2, 625-gkuiTankWidth/2));
		hspPlayer->m_fYPos = static_cast<float>(_GetRandInt(15+gkuiTankHeight/2, 465-gkuiTankHeight/2));
		hspPlayer->m_fAngle = 0;

	//test to see if the player is stuck at spawn
	//if he is, respawn
	}while(_HitDetection(hspPlayer));

	//for testing _DrawTank () on client
	/*hspPlayer->m_fXPos = 320;
	hspPlayer->m_fYPos = 240;
	hspPlayer->m_fAngle = 0;*/

	//clear out the keystates
	hspPlayer->m_sksKeyStates.m_bA = false;
	hspPlayer->m_sksKeyStates.m_bD = false;
	hspPlayer->m_sksKeyStates.m_bS = false;
	hspPlayer->m_sksKeyStates.m_bSP = false;
	hspPlayer->m_sksKeyStates.m_bW = false;

	//reset the timers
	hspPlayer->m_uiCyclesSinceReply = 0;
	hspPlayer->m_uiShotTimer = 0;
}

bool Form1::_TankToTank(sPlayer ^ hspPlayer, BufferedGraphics ^ hBufferedGr)
{
	//if there are 1 or less players there is no need to perform tank to tank hit detection, so return false
	if(m_iNumConnections <= 1)
		return false;

	GraphicsPath ^ hgpPlayerTank;	//path for the player's tank
	GraphicsPath ^ hgpOtherTanks = gcnew GraphicsPath;	//path fot the other tanks
	SolidBrush ^ hBrush = gcnew SolidBrush( Color::Black );

	hgpPlayerTank = _BuildTank(hspPlayer);	//build player graphics path

	//build a path of the tanks that are close to the player/argument tank, excluding itself
	for(unsigned int i(0); i < 4; ++i)
	{
		//only add if connected and not the player/argument tank
		if(m_haspPlayers[i]->m_shConnection != nullptr && i != hspPlayer->m_iPlayerNum - 1)
		{
			if(Math::Abs(hspPlayer->m_fXPos - m_haspPlayers[i]->m_fXPos) < 100 &&
				Math::Abs(hspPlayer->m_fYPos - m_haspPlayers[i]->m_fYPos) < 100)
			{
				hgpOtherTanks->AddPath(_BuildTank(m_haspPlayers[i]), false);
			}
		}
	}

	//put the path into a region so that we can check
	//intersections!  get rid of the path asap
	Drawing::Region ^ hPlayerRegion = gcnew Drawing::Region( hgpPlayerTank );
	delete hgpPlayerTank;
	hBufferedGr->Graphics->FillRegion( hBrush, hPlayerRegion );

	//make a tank region from the graphics path that _BuildTank made
	Drawing::Region ^ hOtherTanksRegion = gcnew Drawing::Region( hgpOtherTanks );
	delete hgpOtherTanks;
	hBufferedGr->Graphics->FillRegion( hBrush, hOtherTanksRegion );

	//copy the tank region into the tank and other tanks region
	Drawing::Region ^ hTank_OtherTanks = hPlayerRegion->Clone();

	hTank_OtherTanks->Intersect( hOtherTanksRegion );

	//if there is an intersection, return true
	//otherwise return false
	//if (hTank_OtherTanks->IsEmpty( hBufferedGr->Graphics ))
	if(hTank_OtherTanks->IsEmpty(hBufferedGr->Graphics))
	{
		/////delete all of the gcnew junk/////
		delete hPlayerRegion;
		delete hOtherTanksRegion;
		delete hTank_OtherTanks;
		return false;
	}
	else 
	{
		/////delete all of the gcnew junk/////
		delete hPlayerRegion;
		delete hOtherTanksRegion;
		delete hTank_OtherTanks;
		return true;
	}
}
void Form1::_BulletToTank ( BufferedGraphics ^ hBufferedGr )
{
	SolidBrush ^ hBrush = gcnew SolidBrush (Color::Red);

	GraphicsPath ^ hGPTank;// = gcnew GraphicsPath;

	for each (sBullet % sBul in m_aBullets)
	{
		if (sBul.m_iFiringPlayer != 0)
		{
			GraphicsPath ^ hGPBullet = gcnew GraphicsPath;
			//draw the bullet in
			hGPBullet->Reset ();
			hGPBullet->AddRectangle (RectangleF (
				sBul.m_fBulletX, sBul.m_fBulletY,
				(float)gkuiBulletWidth, (float)gkuiBulletHeight));
			
			//convert the bullet to a region so we can work with it
			System::Drawing::Region ^ hRegBullet = 
				gcnew System::Drawing::Region (hGPBullet);
				hBufferedGr->Graphics->FillRegion (hBrush, hRegBullet);
					

			for each (sPlayer ^ hsPlayer in m_haspPlayers)
			{
				if (hsPlayer->m_shConnection != nullptr)
				{
					//draw in the tank
					hGPTank = this->_BuildTank (hsPlayer);

					//convert the tank to a region so we can work with them
					System::Drawing::Region ^ hRegTank = 
						gcnew System::Drawing::Region (hGPTank);
					hBufferedGr->Graphics->FillRegion (hBrush, hRegTank);

					//check for a hit
					hRegTank->Intersect (hRegBullet);

					//if there was a hit, delete the bullet and update scores
					if (!hRegTank->IsEmpty (hBufferedGr->Graphics))
					{
						hsPlayer->m_uiPlayerDeaths++;
						m_haspPlayers [sBul.m_iFiringPlayer - 1]->m_uiPlayerKills++;
						_SpawnTank (hsPlayer);
						sBul.m_iFiringPlayer = 0;
					}

					delete hRegTank;
					delete hGPTank;
				}
			}

			delete hGPBullet;
			delete hRegBullet;
		}
	}

	delete hBrush;
}
void Form1::_DrawScene(BufferedGraphics ^ hBufferedGr)
{
	hBufferedGr->Graphics->Clear(this->BackColor);

	//flip coord system so y points up
	System::Drawing::Drawing2D::Matrix ^ mat = gcnew System::Drawing::Drawing2D::Matrix( 1, 0, 0, -1, 0, 0);
	hBufferedGr->Graphics->Transform = mat;
	hBufferedGr->Graphics->TranslateTransform( 0, (float)this->ClientSize.Height, MatrixOrder::Append );

	_DrawObstacles(hBufferedGr);
	_DrawTanks(hBufferedGr);
	_DrawBullets(hBufferedGr);

	hBufferedGr->Render();
}
void Form1::_DrawTanks(BufferedGraphics ^ hBufferedGr)
{
	//matrix for rotation
	System::Drawing::Drawing2D::Matrix ^ hRotationMatrix = gcnew System::Drawing::Drawing2D::Matrix;
	SolidBrush ^ sbBrush = gcnew SolidBrush(Color::Black);

	for each(sPlayer ^ hsPlayer in m_haspPlayers)
	{
		if(hsPlayer->m_shConnection != nullptr)
		{
			GraphicsPath ^ hGPTank = gcnew GraphicsPath;
			//tank body
			RectangleF hRTankBody = RectangleF(hsPlayer->m_fXPos - (float)gkuiTankWidth / 2.0f ,
				hsPlayer->m_fYPos - (float)gkuiTankHeight / 2.0f, 
				(float)gkuiTankWidth, (float)gkuiTankHeight);

			hGPTank->AddRectangle( hRTankBody );

			hRotationMatrix->Translate (hsPlayer->m_fXPos, hsPlayer->m_fYPos);

			//rotate from the center of the tank
			hRotationMatrix->Reset ();

			hRotationMatrix->RotateAt(-hsPlayer->m_fAngle * 180 / gkfPI , 
				PointF(hsPlayer->m_fXPos, hsPlayer->m_fYPos));

			//apply the rotation and draw the thing
			hGPTank->Transform( hRotationMatrix );
			
			//set the brush color depending on the player number
			if(hsPlayer->m_iPlayerNum == 1)
				sbBrush->Color = Color::Red;
			else if(hsPlayer->m_iPlayerNum == 2)
				sbBrush->Color = Color::Green;
			else if(hsPlayer->m_iPlayerNum == 3)
				sbBrush->Color = Color::Blue;
			else if(hsPlayer->m_iPlayerNum == 4)
				sbBrush->Color = Color::Black;

			hBufferedGr->Graphics->FillPath(sbBrush, hGPTank);

			delete hGPTank;
		}
	}
	delete hRotationMatrix;
	delete sbBrush;
}
void Form1::_DrawBullets(BufferedGraphics ^ hBufferedGr)
{
	System::Drawing::Drawing2D::Matrix ^ hRotationMatrix = gcnew System::Drawing::Drawing2D::Matrix;
	SolidBrush ^ sbBrush = gcnew SolidBrush(Color::Black);

	for(unsigned int i(0); i < gkuiMaxBulletCount; ++i)
	{
		if(m_aBullets[i].m_iFiringPlayer != 0)
		{
			GraphicsPath ^ hGPBullet = gcnew GraphicsPath;

			RectangleF hRBulletBody = RectangleF(m_aBullets[i].m_fBulletX ,
				m_aBullets[i].m_fBulletY, (float)gkuiBulletWidth, (float)gkuiBulletHeight);

			hGPBullet->AddRectangle( hRBulletBody );

			hRotationMatrix->Reset ();

			//rotate from the center of the tank
			hRotationMatrix->Reset ();
			hRotationMatrix->RotateAt(-m_aBullets[i].m_fBulletAngle * 180 / gkfPI, 
				PointF(m_aBullets[i].m_fBulletX + 2.5f, m_aBullets[i].m_fBulletY + 2.5f));

			//apply the rotation and draw the thing
			hGPBullet->Transform( hRotationMatrix );
			switch (m_aBullets[i].m_iFiringPlayer)
			{
			case 1:
				sbBrush->Color = Color::Red;
				break;
			case 2:
				sbBrush->Color = Color::Green;
				break;
			case 3:
				sbBrush->Color = Color::Blue;
				break;
			case 4:
				sbBrush->Color = Color::LightBlue;
				break;
			}
			hBufferedGr->Graphics->FillPath(sbBrush, hGPBullet);
			delete hGPBullet;
		}
	}
	delete hRotationMatrix;
	delete sbBrush;
}
System::Void Form1::generateSendNewMapToolStripMenuItem_Click(System::Object^  sender, System::EventArgs^  e)
{
	_BuildMap();
	for each(sPlayer ^ hsPlayer in m_haspPlayers)
	{
		if(hsPlayer->m_shConnection != nullptr)
		{
			_SpawnTank(hsPlayer);
			_SendMap(hsPlayer->m_shConnection);
		}
	}
}