#pragma once

namespace RamrodServer
{
	const float gkfPI (3.14159265f);
	enum EFrameType //first byte of every frame, used to describe the content of the frame
	{
		eKeyStates = 0,
		ePlayerState,
		eServerFull,
		eObstacle,
		eChatMessage //bidirectional, no struct is associated with this type 
					 //but each frame is a total of gkuiChatSize + sizeof (EFrameType) bytes
	};

	enum EObstacleType
	{
		eNone = 0, //reserved for used powerup and for unused array element
		eRectangle,
		eEllipse
		//if you add new obstacles, be sure to modify _GetRandObstacle and BuildMap
	};

	public value struct sServerFull //server to client
	{
		EFrameType m_eFrameType; //always == eServerFull
	};
	public value struct sKeyStates //client to server
	{
		EFrameType m_eFrameType; //always == eKeyStates
		bool m_bW; //true if the key is down, false if up
		bool m_bA;
		bool m_bS;
		bool m_bD;
		bool m_bSP;
	};

	public value struct sPlayer //server data
	{
		int m_iPlayerNum; //the players number
		System::Net::Sockets::Socket ^ m_shConnection; //connection to the player
		sKeyStates m_sksKeyStates; //the state of the players keys
		unsigned int m_uiCyclesSinceReply; //player will be kicked when this hits max of 20
		unsigned int m_uiShotTimer;	//This is the shot timer for the player
		array<unsigned char> ^ m_aucRXData;	//receiving data buffer

		float m_fXPos; //where the player is on the map and his/her heading
		float m_fYPos;
		float m_fAngle; //radians

		unsigned int m_uiPlayerKills;
		unsigned int m_uiPlayerDeaths;
	};

	public value struct sPlayerStates //server to client
	{
		EFrameType m_eFrameType; //always == ePlayerState
		int m_iPlayerNum; //the players number
		int m_iShotTimer; //the players current shot timer status

		//P1
		bool m_bPlayer1Connected; //is player n connected?
		float m_fPlayer1X; //the location and heading of player n
		float m_fPlayer1Y; //center coords
		float m_fPlayer1Angle; //radians
		unsigned int m_uiPlayer1Deaths;
		unsigned int m_uiPlayer1Kills;

		//p2
		bool m_bPlayer2Connected;
		float m_fPlayer2X;
		float m_fPlayer2Y;
		float m_fPlayer2Angle; //radians
		unsigned int m_uiPlayer2Deaths;
		unsigned int m_uiPlayer2Kills;

		//p3
		bool m_bPlayer3Connected;
		float m_fPlayer3X;
		float m_fPlayer3Y;
		float m_fPlayer3Angle; //radians
		unsigned int m_uiPlayer3Deaths;
		unsigned int m_uiPlayer3Kills;

		//p4
		bool m_bPlayer4Connected;
		float m_fPlayer4X;
		float m_fPlayer4Y;
		float m_fPlayer4Angle; //radians
		unsigned int m_uiPlayer4Deaths;
		unsigned int m_uiPlayer4Kills;
	};

	public value struct SObstacle //server to client, sent on connect
	{
		EFrameType m_eFrameType;
		int m_iObstacleNumber; //used so we can remove obstacles (powerups)
		EObstacleType m_eObstacleType;
		float m_fObstacleX; //top left
		float m_fObstacleY; //top right
		float m_fObstacleWidth;
		float m_fObstacleHeight;
		System::Drawing::Color m_color;
	};

	//the bullet struct; contained within the sPlayerStates frame/structure;
	//also, there will be a member collection of these in the server (keeping track)
	public value struct sBullet
	{
		float m_fBulletX;		//x coord of the bullet
		float m_fBulletY;		//y coord of the bullet
		float m_fBulletAngle;	//angle of the bullet (radians)
		unsigned int m_iFiringPlayer;	//player number that fired the bullet, 0 means inactive
	};

	const unsigned int gkuiDefPortNumber (1666); //the default port to connect to
	const unsigned int gkuiMaxObstacleCount (20); // how many obstacles will appear
	const unsigned int gkuiMaxBulletCount(10);	//max number of bullets in play
	const unsigned int gkuiChatSize (40);	//how many characters per chat message max
	const unsigned int gkuiShotCycles(80);	//number of cycles for shot timer
	const unsigned int gkuiTankWidth(10);	//the tank width
	const unsigned int gkuiTankHeight(20);	//the tank height
	const unsigned int gkuiTankSpeed(3);	//the tank movement speed
	const unsigned int gkuiBulletWidth(2); //the bullet width
	const unsigned int gkuiBulletHeight(2);//the bullet height
	const float gkfTankHypot (11.180f); //SQR ((.5height)^2 + (.5width)^2)
	const unsigned int gkuiBulletSpeed (8);
	const unsigned int gkuiDetectRange (50); //how far away to do hit detect

} //namespace RamrodServer