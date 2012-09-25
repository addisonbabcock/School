using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;
using Common.Misc;
using Microsoft.Xna.Framework;

namespace SoccerXNA.Teams.ThomahawksTeam.FieldPlayerStates
{
    public class GlobalPlayerState : State<FieldPlayer>
    {
        private static GlobalPlayerState instance;
        private readonly MessageDispatcher Dispatcher = MessageDispatcher.Instance();
        private readonly ParamLoader Prm = ParamLoader.Instance;

        private GlobalPlayerState()
        {
        }

        public static GlobalPlayerState Instance()
        {
            if (instance == null)
            {
                instance = new GlobalPlayerState();
            }
            return instance;
        }

        public override void Enter(FieldPlayer player)
        {
        }

        public override void Execute(FieldPlayer player)
        {
            //if a player is in possession and close to the ball reduce his max speed
            if ((player.BallWithinReceivingRange()) && (player.isControllingPlayer()))
            {
                player.SetMaxSpeed(Prm.PlayerMaxSpeedWithBall);
            }

            else
            {
                player.SetMaxSpeed(Prm.PlayerMaxSpeedWithoutBall);
            }
        }

        public override void Exit(FieldPlayer player)
        {
        }

        public override bool OnMessage(FieldPlayer player, Telegram telegram)
        {
            switch (telegram.Msg)
            {
                case (int)SoccerMessages.Msg_ReceiveBall:

                    //set the target
                    player.Steering().SetTarget((Vector2)telegram.ExtraInfo);

                    //change state 
                    player.GetFSM().ChangeState(ReceiveBall.Instance());

                    return true;


                case (int)SoccerMessages.Msg_SupportAttacker:

                    //if already supporting just return
                    if (player.GetFSM().isInState(SupportAttacker.Instance()))
                    {
                        return true;
                    }

                    //set the target to be the best supporting position
                    player.Steering().SetTarget(player.Team().GetSupportSpot());

                    //change the state
                    player.GetFSM().ChangeState(SupportAttacker.Instance());

                    return true;


                case (int)SoccerMessages.Msg_Wait:

                    //change the state
                    player.GetFSM().ChangeState(Wait.Instance());

                    return true;


                case (int)SoccerMessages.Msg_GoHome:

                    player.SetDefaultHomeRegion();

                    player.GetFSM().ChangeState(ReturnToHomeRegion.Instance());

                    return true;


                case (int)SoccerMessages.Msg_PassToMe:


                    //get the position of the player requesting the pass 
                    FieldPlayer receiver = (FieldPlayer)telegram.ExtraInfo;

                    Debug.WriteLine("Player " + player.ID() + " received request from " + receiver.ID() + " to make pass");

                    //if the ball is not within kicking range or their is already a 
                    //receiving player, this player cannot pass the ball to the player
                    //making the request.
                    if (player.Team().Receiver() != null ||
                        !player.BallWithinKickingRange())
                    {
                        Debug.WriteLine("Player " + player.ID() + " cannot make requested pass <cannot kick ball>");

                        return true;
                    }

                    //make the pass   
                    player.Ball().Kick(receiver.Pos() - player.Ball().Pos(),
                                       Prm.MaxPassingForce);


                    Debug.WriteLine("Player " + player.ID() + " passed ball to requesting player");

                    //let the receiver know a pass is coming 
                    Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                           player.ID(),
                                           receiver.ID(),
                                           (int)SoccerMessages.Msg_ReceiveBall,
                                           receiver.Pos());


                    //change state   
                    player.GetFSM().ChangeState(Defense.Instance());

                    player.FindSupport();

                    return true;
            } //end switch

            return false;
        }
    }
}