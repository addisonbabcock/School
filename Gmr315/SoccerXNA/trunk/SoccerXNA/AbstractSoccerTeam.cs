using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.FSM;
using Common.Game;
using Common.Interfaces;
using Common.Messaging;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoccerXNA.Teams.BucklandTeam.FieldPlayerStates;

namespace SoccerXNA
{
    public abstract class AbstractSoccerTeam : DrawableGameComponent
    {

        public AbstractSoccerTeam(Game game, Goal home_goal,
                          Goal opponents_goal,
                          SoccerPitch pitch,
                          team_color color) : base(game)
        {
            m_pOpponentsGoal = opponents_goal;
            m_pHomeGoal = home_goal;
            m_pOpponents = null;
            m_pPitch = pitch;
            m_Color = color;
            m_dDistSqToBallOfClosestPlayer = 0.0f;
            m_pSupportingPlayer = null;
            m_pReceivingPlayer = null;
            m_pControllingPlayer = null;
            m_pPlayerClosestToBall = null;
        }

        public enum team_color
        {
            blue,
            red
        } ;

        #region Drawing members
        private static SpriteFont gameFont;
        private static PrimitiveBatch primitiveBatch;
        private static SpriteBatch spriteBatch;

        protected readonly MessageDispatcher Dispatcher = MessageDispatcher.Instance();

         protected PrimitiveBatch PrimitiveBatch
        {
            get
            {
                if (primitiveBatch == null)
                {
                    IDrawingManager manager = (IDrawingManager) Game.Services.GetService(typeof (IDrawingManager));
                    primitiveBatch = manager.GetPrimitiveBatch();
                }
                return primitiveBatch;
            }
        }

        protected virtual SpriteFont GameFont
        {
            get
            {
                if (gameFont == null)
                {
                    IFontManager manager = (IFontManager) Game.Services.GetService(typeof (IFontManager));
                    gameFont = manager.GetFont("Default");
                }
                return gameFont;
            }
        }

        protected SpriteBatch SpriteBatch
        {
            get
            {
                if (spriteBatch == null)
                {
                    IDrawingManager manager = (IDrawingManager) Game.Services.GetService(typeof (IDrawingManager));
                    spriteBatch = manager.GetSpriteBatch();
                }
                return spriteBatch;
            }
        }
#endregion

        protected StateMachine<AbstractSoccerTeam> m_pStateMachine;

  //the team must know its own color!
        protected team_color m_Color;

  //pointers to the team members
  protected List<PlayerBase> m_Players;

  //a pointer to the soccer pitch
  protected SoccerPitch m_pPitch;

  //pointers to the goals
  protected Goal m_pHomeGoal;
  protected Goal m_pOpponentsGoal;
  
  //a pointer to the opposing team
        protected AbstractSoccerTeam m_pOpponents;
   
  //pointers to 'key' players
  protected PlayerBase m_pControllingPlayer;
  protected PlayerBase m_pSupportingPlayer;
  protected PlayerBase m_pReceivingPlayer;
  protected PlayerBase m_pPlayerClosestToBall;

  //the squared distance the closest player is from the ball
  protected float m_dDistSqToBallOfClosestPlayer;

  //players use this to determine strategic positions on the playing field
  protected SupportSpotCalculator    m_pSupportSpotCalc;

  // public virtual functions can be customized by subclasses

  //calling this changes the state of all field players to that of 
  //ReturnToHomeRegion. Mainly used when a goal keeper has
  //possession
  public virtual void        ReturnAllFieldPlayersToHome()
  {
      foreach (PlayerBase it in m_Players)
      {
          if (it.Role() != PlayerBase.player_role.goal_keeper)
          {
              Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                     1,
                                     it.ID(),
                                     (int) SoccerMessages.Msg_GoHome,
                                     null);
          }
      }
  }

  //returns true if player has a clean shot at the goal and sets ShotTarget
  //to a normalized vector pointing in the direction the shot should be
  //made. Else returns false and sets heading to a zero vector
  //------------------------ CanShoot --------------------------------------
  //
  //  Given a ball position, a kicking power and a reference to a Vector2
  //  this function will sample random positions along the opponent's goal-
  //  mouth and check to see if a goal can be scored if the ball was to be
  //  kicked in that direction with the given power. If a possible shot is 
  //  found, the function will immediately return true, with the target 
  //  position stored in the vector ShotTarget.
  //------------------------------------------------------------------------
  public virtual bool CanShoot(Vector2 BallPos,
                       float power)
  {
      Vector2 target = new Vector2();
      return CanShoot(BallPos, power, ref target);
  }

  public virtual bool CanShoot(Vector2 BallPos,
                       float power,
                       ref Vector2 ShotTarget)
  {
      ShotTarget = new Vector2();
      //the number of randomly created shot targets this method will test 
      int NumAttempts = Prm.NumAttemptsToFindValidStrike;


      while (NumAttempts-- > 0)
      {
          //choose a random position along the opponent's goal mouth. (making
          //sure the ball's radius is taken into account)
          ShotTarget = OpponentsGoal().Center();

          //the y value of the shot position should lay somewhere between two
          //goalposts (taking into consideration the ball diameter)
          int MinYVal = (int)OpponentsGoal().LeftPost().Y + (int)Pitch().Ball().BRadius();
          int MaxYVal = (int)OpponentsGoal().RightPost().Y - (int)Pitch().Ball().BRadius();

          ShotTarget.Y = Utils.RandInt(MinYVal, MaxYVal);

          //make sure striking the ball with the given power is enough to drive
          //the ball over the goal line.
          float time = Pitch().Ball().TimeToCoverDistance(BallPos,
                                                          ShotTarget,
                                                          power);

          //if it is, this shot is then tested to see if any of the opponents
          //can intercept it.
          if (time >= 0)
          {
              if (isPassSafeFromAllOpponents(BallPos, ShotTarget, null, power))
              {
                  return true;
              }
          }
      }

      return false;
  }

  //The best pass is considered to be the pass that cannot be intercepted 
  //by an opponent and that is as far forward of the receiver as possible  
  //If a pass is found, the receiver's address is returned in the 
  //reference, 'receiver' and the position the pass will be made to is 
  //returned in the  reference 'PassTarget'
  //-------------------------- FindPass ------------------------------
  //
  //  The best pass is considered to be the pass that cannot be intercepted 
  //  by an opponent and that is as far forward of the receiver as possible
  //------------------------------------------------------------------------

  public virtual bool FindPass(PlayerBase passer,
                       ref PlayerBase receiver,
                       ref Vector2 PassTarget,
                       float power,
                       float MinPassingDistance)
  {
      //receiver = null;
      //PassTarget = new Vector2();
      float ClosestToGoalSoFar = float.MaxValue;
      Vector2 Target = new Vector2();

      //iterate through all this player's team members and calculate which
      //one is in a position to be passed the ball 
      foreach (PlayerBase curPlyr in Members())
      {
          //make sure the potential receiver being examined is not this player
          //and that it is further away than the minimum pass distance
          if ((curPlyr != passer) &&
              (Vector2.DistanceSquared(passer.Pos(), (curPlyr).Pos()) >
               MinPassingDistance * MinPassingDistance))
          {
              if (GetBestPassToReceiver(passer, curPlyr, ref Target, power))
              {
                  //if the pass target is the closest to the opponent's goal line found
                  // so far, keep a record of it
                  float Dist2Goal = Math.Abs(Target.X - OpponentsGoal().Center().X);

                  if (Dist2Goal < ClosestToGoalSoFar)
                  {
                      ClosestToGoalSoFar = Dist2Goal;

                      //keep a record of this player
                      receiver = curPlyr;

                      //and the target
                      PassTarget = Target;
                  }
              }
          }
      } //next team member


      if (receiver != null) return true;

      return false;
  }


  //Three potential passes are calculated. One directly toward the receiver's
  //current position and two that are the tangents from the ball position
  //to the circle of radius 'range' from the receiver.
  //These passes are then tested to see if they can be intercepted by an
  //opponent and to make sure they terminate within the playing area. If
  //all the passes are invalidated the function returns false. Otherwise
  //the function returns the pass that takes the ball closest to the 
  //opponent's goal area.
  //---------------------- GetBestPassToReceiver ---------------------------
  //
  //  Three potential passes are calculated. One directly toward the receiver's
  //  current position and two that are the tangents from the ball position
  //  to the circle of radius 'range' from the receiver.
  //  These passes are then tested to see if they can be intercepted by an
  //  opponent and to make sure they terminate within the playing area. If
  //  all the passes are invalidated the function returns false. Otherwise
  //  the function returns the pass that takes the ball closest to the 
  //  opponent's goal area.
  //------------------------------------------------------------------------

  public virtual bool GetBestPassToReceiver(PlayerBase passer,
                                    PlayerBase receiver,
                                    ref Vector2 PassTarget,
                                    float power)
  {
      //first, calculate how much time it will take for the ball to reach 
      //this receiver, if the receiver was to remain motionless 
      float time = Pitch().Ball().TimeToCoverDistance(Pitch().Ball().Pos(),
                                                      receiver.Pos(),
                                                      power);

      //return false if ball cannot reach the receiver after having been
      //kicked with the given power
      if (time < 0) return false;

      //the maximum distance the receiver can cover in this time
      float InterceptRange = time * receiver.MaxSpeed();

      //Scale the intercept range
      float ScalingFactor = 0.3f;
      InterceptRange *= ScalingFactor;

      //now calculate the pass targets which are positioned at the intercepts
      //of the tangents from the ball to the receiver's range circle.
      Vector2 ip1 = new Vector2();
      Vector2 ip2 = new Vector2();

      Geometry.GetTangentPoints(receiver.Pos(),
                                InterceptRange,
                                Pitch().Ball().Pos(),
                                ref ip1,
                                ref ip2);

      int NumPassesToTry = 3;
      Vector2[] Passes = new[] { ip1, receiver.Pos(), ip2 };


      // this pass is the best found so far if it is:
      //
      //  1. Further upfield than the closest valid pass for this receiver
      //     found so far
      //  2. Within the playing area
      //  3. Cannot be intercepted by any opponents

      float ClosestSoFar = float.MaxValue;
      bool bResult = false;

      for (int pass = 0; pass < NumPassesToTry; ++pass)
      {
          float dist = Math.Abs(Passes[pass].X - OpponentsGoal().Center().X);

          if ((dist < ClosestSoFar) &&
              Pitch().PlayingArea().Inside(Passes[pass]) &&
              isPassSafeFromAllOpponents(Pitch().Ball().Pos(),
                                         Passes[pass],
                                         receiver,
                                         power))
          {
              ClosestSoFar = dist;
              PassTarget = Passes[pass];
              bResult = true;
          }
      }

      return bResult;
  }

  //test if a pass from positions 'from' to 'target' kicked with force 
  //'PassingForce'can be intercepted by an opposing player
  //----------------------- isPassSafeFromOpponent -------------------------
  //
  //  test if a pass from 'from' to 'to' can be intercepted by an opposing
  //  player
  //------------------------------------------------------------------------

  public virtual bool isPassSafeFromOpponent(Vector2 from,
                                     Vector2 target,
                                     PlayerBase receiver,
                                     PlayerBase opp,
                                     float PassingForce)
  {
      //move the opponent into local space.
      Vector2 ToTarget = target - from;
      if(ToTarget == Vector2.Zero)
      {
          return false;
      }
      Vector2 ToTargetNormalized = Vector2.Normalize(ToTarget);

      Vector2 LocalPosOpp = Transformations.PointToLocalSpace(opp.Pos(),
                                                              ToTargetNormalized,
                                                              ToTargetNormalized.Perp(),
                                                              from);

      //if opponent is behind the kicker then pass is considered okay(this is 
      //based on the assumption that the ball is going to be kicked with a 
      //velocity greater than the opponent's max velocity)
      if (LocalPosOpp.X < 0)
      {
          return true;
      }

      //if the opponent is further away than the target we need to consider if
      //the opponent can reach the position before the receiver.
      if (Vector2.DistanceSquared(from, target) < Vector2.DistanceSquared(opp.Pos(), from))
      {
          if (receiver != null)
          {
              if (Vector2.DistanceSquared(target, opp.Pos()) >
                  Vector2.DistanceSquared(target, receiver.Pos()))
              {
                  return true;
              }


              return false;
          }

          return true;
      }

      //calculate how long it takes the ball to cover the distance to the 
      //position orthogonal to the opponents position
      float TimeForBall =
          Pitch().Ball().TimeToCoverDistance(new Vector2(0, 0),
                                             new Vector2(LocalPosOpp.X, 0),
                                             PassingForce);

      //now calculate how far the opponent can run in this time
      float reach = opp.MaxSpeed() * TimeForBall +
                    Pitch().Ball().BRadius() +
                    opp.BRadius();

      //if the distance to the opponent's y position is less than his running
      //range plus the radius of the ball and the opponents radius then the
      //ball can be intercepted
      if (Math.Abs(LocalPosOpp.Y) < reach)
      {
          return false;
      }

      return true;
  }


  //tests a pass from position 'from' to position 'target' against each member
  //of the opposing team. Returns true if the pass can be made without
  //getting intercepted
  //---------------------- isPassSafeFromAllOpponents ----------------------
  //
  //  tests a pass from position 'from' to position 'target' against each member
  //  of the opposing team. Returns true if the pass can be made without
  //  getting intercepted
  //------------------------------------------------------------------------

  public virtual bool isPassSafeFromAllOpponents(Vector2 from,
                                         Vector2 target,
                                         PlayerBase receiver,
                                         float PassingForce)
  {
      foreach (PlayerBase opp in Opponents().Members())
      {
          if (!isPassSafeFromOpponent(from, target, receiver, opp, PassingForce))
          {
              return false;
          }
      }

      return true;
  }

  //this tests to see if a pass is possible between the requester and
  //the controlling player. If it is possible a message is sent to the
  //controlling player to pass the ball asap.
  //------------------------- RequestPass ---------------------------------------
  //
  //  this tests to see if a pass is possible between the requester and
  //  the controlling player. If it is possible a message is sent to the
  //  controlling player to pass the ball asap.
  //-----------------------------------------------------------------------------
  public virtual void RequestPass(FieldPlayer requester)
  {
      //maybe put a restriction here
      if (Utils.RandFloat() > 0.1) return;

      if (isPassSafeFromAllOpponents(ControllingPlayer().Pos(),
                                     requester.Pos(),
                                     requester,
                                     Prm.MaxPassingForce))
      {
          //tell the player to make the pass
          //let the receiver know a pass is coming 
          Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                 requester.ID(),
                                 ControllingPlayer().ID(),
                                 (int)SoccerMessages.Msg_PassToMe,
                                 requester);
      }
  }

  //calculates the best supporting position and finds the most appropriate
  //attacker to travel to the spot
  //------------- DetermineBestSupportingAttacker ------------------------
  //
  // calculate the closest player to the SupportSpot
  //------------------------------------------------------------------------
  public virtual PlayerBase DetermineBestSupportingAttacker()
  {
      float ClosestSoFar = float.MaxValue;

      PlayerBase BestPlayer = null;

      foreach (PlayerBase it in m_Players)
      {
          //only attackers utilize the BestSupportingSpot
          if (((it).Role() == PlayerBase.player_role.attacker) && ((it) != m_pControllingPlayer))
          {
              //calculate the dist. Use the squared value to avoid sqrt
              float dist = Vector2.DistanceSquared((it).Pos(), m_pSupportSpotCalc.GetBestSupportingSpot());

              //if the distance is the closest so far and the player is not a
              //goalkeeper and the player is not the one currently controlling
              //the ball, keep a record of this player
              if ((dist < ClosestSoFar))
              {
                  ClosestSoFar = dist;

                  BestPlayer = (it);
              }
          }
      }

      return BestPlayer;
  }



	// pure public abstract functions that subclasses must implement
	 //creates all the players for this team
  public abstract void CreatePlayers();

  public abstract void InitStateMachine();

  public abstract void InitPlayers();

  public abstract void PrepareForKickoff();

  public abstract string Name();

	// Non-public virtual functions

                protected static readonly ParamLoader Prm = ParamLoader.Instance;
//the usual suspects
        //--------------------------- Render -------------------------------------
        //
        //  renders the players and any team related info
        //------------------------------------------------------------------------
        public override void Draw(GameTime gameTime)
        {
            foreach (PlayerBase it in m_Players)
            {
                it.Draw(gameTime);
            }

            //show the controlling team and player at the top of the display
            if (Prm.bShowControllingTeam)
            {
                Vector2 textPosition = new Vector2(20, 3);


                if ((Color() == team_color.blue) && InControl())
                {
                    SpriteBatch.DrawString(GameFont, "Blue in Control", textPosition,
                                           Microsoft.Xna.Framework.Graphics.Color.White);
                }
                else if ((Color() == team_color.red) && InControl())
                {
                    SpriteBatch.DrawString(GameFont, "Red in Control", textPosition,
                                           Microsoft.Xna.Framework.Graphics.Color.White);
                }
                if (m_pControllingPlayer != null)
                {
                    SpriteBatch.DrawString(GameFont, "Controlling Player: " + m_pControllingPlayer.ID(),
                                           new Vector2(Pitch().cxClient() - 150, 3),
                                           Microsoft.Xna.Framework.Graphics.Color.White);
                }
            }

            //render the sweet spots
            if (Prm.bSupportSpots && InControl())
            {
                m_pSupportSpotCalc.Draw(gameTime);
            }

        
            base.Draw(gameTime);
        }

        //called each frame. Sets m_pClosestPlayerToBall to point to the player
        //closest to the ball. 
        //------------------------ CalculateClosestPlayerToBall ------------------
        //
        //  sets m_iClosestPlayerToBall to the player closest to the ball
        //------------------------------------------------------------------------
        private void CalculateClosestPlayerToBall()
        {
            float ClosestSoFar = float.MaxValue;

            foreach (PlayerBase it in m_Players)
            {
                //calculate the dist. Use the squared value to avoid sqrt
                float dist = Vector2.DistanceSquared(it.Pos(), Pitch().Ball().Pos());

                //keep a record of this value for each player
                it.SetDistSqToBall(dist);

                if (dist < ClosestSoFar)
                {
                    ClosestSoFar = dist;

                    m_pPlayerClosestToBall = it;
                }
            }

            m_dDistSqToBallOfClosestPlayer = ClosestSoFar;
        }

        //-------------------------- update --------------------------------------
        //
        //  iterates through each player's update function and calculates 
        //  frequently accessed info
        //------------------------------------------------------------------------
        public override void Update(GameTime gameTime)
        {
            //this information is used frequently so it's more efficient to 
            //calculate it just once each frame
            CalculateClosestPlayerToBall();

            //the team state machine switches between attack/defense behavior. It
            //also handles the 'kick off' state where a team must return to their
            //kick off positions before the whistle is blown
            m_pStateMachine.Update();

            //now update each player
            foreach (PlayerBase it in m_Players)
            {
                it.Update(gameTime);
            }
        }

    //returns true if there is an opponent within radius of position
        public bool isOpponentWithinRadius(Vector2 pos, float rad)
        {
            foreach (PlayerBase it in Opponents().Members())
            {
                if (Vector2.DistanceSquared(pos, (it).Pos()) < rad * rad)
                {
                    return true;
                }
            }

            return false;
        }

        protected readonly EntityManager EntityMgr = EntityManager.Instance();

	public void	RegisterPlayers()
	{
        //register the players with the entity manager
        foreach (PlayerBase it in m_Players)
        {
            EntityMgr.RegisterEntity(it);
        }
	}

   public List<PlayerBase> Members()
        {
            return m_Players;
        }
  public StateMachine<AbstractSoccerTeam> GetFSM()
        {
            return m_pStateMachine;
        }

  public Goal HomeGoal()
        {
            return m_pHomeGoal;
        }
        
  public Goal OpponentsGoal()
        {
            return m_pOpponentsGoal;
        }

public SoccerPitch Pitch()
        {
            return m_pPitch;
        }

        public AbstractSoccerTeam Opponents()
        {
            return m_pOpponents;
        }

        public void SetOpponents(AbstractSoccerTeam opps)
        {
            m_pOpponents = opps;
        }

        public team_color Color()
        {
            return m_Color;
        }

        public void SetPlayerClosestToBall(PlayerBase plyr)
        {
            m_pPlayerClosestToBall = plyr;
        }

        public PlayerBase PlayerClosestToBall()
        {
            return m_pPlayerClosestToBall;
        }

        public float ClosestDistToBallSq()
        {
            return m_dDistSqToBallOfClosestPlayer;
        }

        public Vector2 GetSupportSpot()
        {
            return m_pSupportSpotCalc.GetBestSupportingSpot();
        }

        public PlayerBase SupportingPlayer()
        {
            return m_pSupportingPlayer;
        }

        public void SetSupportingPlayer(PlayerBase plyr)
        {
            m_pSupportingPlayer = plyr;
        }

        public PlayerBase Receiver()
        {
            return m_pReceivingPlayer;
        }

        public void SetReceiver(PlayerBase plyr)
        {
            m_pReceivingPlayer = plyr;
        }

        public PlayerBase ControllingPlayer()
        {
            return m_pControllingPlayer;
        }

        public void SetControllingPlayer(PlayerBase plyr)
        {
            m_pControllingPlayer = plyr;

            //rub it in the opponents faces!
            Opponents().LostControl();
        }


        public bool InControl()
        {
            if (m_pControllingPlayer != null) return true;
            return false;
        }

        public void LostControl()
        {
            m_pControllingPlayer = null;
        }
  public PlayerBase GetPlayerFromID(int id)
        {
            foreach (PlayerBase it in m_Players)
            {
                if ((it).ID() == id) return it;
            }

            return null;
        }
  

 public void SetPlayerHomeRegion(int plyr, int region)
        {
            if (plyr < 0 || plyr >= m_Players.Count)
            {
                throw new Exception("Original Code Assertion");
            }

            m_Players[plyr].SetHomeRegion(region);
        }

          public void DetermineBestSupportingPosition()
        {
            m_pSupportSpotCalc.DetermineBestSupportingPosition();
        }



          public virtual void UpdateTargetsOfWaitingPlayers()
          {
              foreach (PlayerBase it in m_Players)
              {
                  if ((it).Role() != PlayerBase.player_role.goal_keeper)
                  {
                      //cast to a field player
                      FieldPlayer plyr = (FieldPlayer)(it);

                      if (plyr.GetFSM().isInState(Wait.Instance()) ||
                          plyr.GetFSM().isInState(ReturnToHomeRegion.Instance()))
                      {
                          plyr.Steering().SetTarget(plyr.HomeRegion().Center());
                      }
                  }
              }
          }

   //returns false if any of the team are not located within their home region
        //--------------------------- AllPlayersAtHome --------------------------------
        //
        //  returns false if any of the team are not located within their home region
        //-----------------------------------------------------------------------------
        public bool AllPlayersAtHome()
        {
            foreach (PlayerBase it in m_Players)
            {
                if ((it).InHomeRegion() == false)
                {
                    return false;
                }
            }

            return true;
        }

	 public void ChangePlayerHomeRegions (int[] NewRegions)
        {
            for (int plyr = 0; plyr < NewRegions.Length; ++plyr)
            {
                SetPlayerHomeRegion(plyr, NewRegions[plyr]);
            }
        }
    }
}
