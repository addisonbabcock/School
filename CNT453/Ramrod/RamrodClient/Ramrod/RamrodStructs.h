#pragma once

namespace Ramrod
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

	public value struct sPlayerStates //server to client
	{
		EFrameType m_eFrameType; //always == ePlayerState
		int m_iPlayerNum; //the players number
		int m_iShotTimer; //the players current shot timer status

		//P1
		bool m_bPlayer1Connected; //is player n connected?
		float m_fPlayer1X; //the location and heading of player n
		float m_fPlayer1Y; //center coords
		float m_fPlayer1Angle; //degrees
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
		unsigned int m_iFiringPlayer;	//player number that fired the bullet
	};

	const unsigned int gkuiDefPortNumber (1666); //the default port to connect to
	const unsigned int gkuiMaxObstacleCount (100); // how many obstacles will appear
	const unsigned int gkuiMaxBulletCount(10);	//max number of bullets in play

	//IF YOU CHANGE THIS CHANGE UI_ChatBox->MaxLength AS WELL!
	const unsigned int gkuiChatSize (40); //how many characters per chat message max
	const unsigned int gkuiChatLogLength (5); //how many messages to track
	const unsigned int gkuiShotCycles(80);	//number of cycles for shot timer
	const unsigned int gkuiTankWidth(10);	//the tank width
	const unsigned int gkuiTankHeight(20);	//the tank hei
	const unsigned int gkuiBarrelWidth (5);
	const unsigned int gkuiBarrelHeight (5);
	const float gkfCamFollowX(30.0f);	//the factor for the camera to follow by on the x
	const float gkfCamFollowZ(30.0f); //the factor for the camera to follow by on the z
	const float gkfCamFollowY(15.0f);	//the factor for the camera to follow by on the y
	const float gkfTankScaling (5.0f);
	const float gkfBulletScaling(0.50f);
} //namespace Ramrod