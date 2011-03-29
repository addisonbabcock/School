using Common.Messaging;
using Common.Misc;
using Common.Time;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raven.goals;
using Raven.navigation;

namespace Raven.Bots.DanBot
{
    public class Dan_Bot : AbstractBot, IBot
    {
        protected static Dan_BotScriptor m_pScript = Dan_BotScriptor.Instance();

        public Dan_Bot(Raven_Game world, Vector2 pos) :
            base(world, pos)
        {
            m_pScript = Dan_BotScriptor.Instance();
            SetEntityType((int) Raven_Objects.type_bot);

            SetUpVertexBuffer();

            //a bot starts off facing in the direction it is heading
            m_vFacing = Heading();

            //create the navigation module
            m_pPathPlanner = new Raven_PathPlanner(this);

            //create the steering behavior class
            m_pSteering = new Dan_BotSteering(world, this);

            //create the regulators
            m_pWeaponSelectionRegulator = new Regulator(script.GetDouble("Bot_WeaponSelectionFrequency"));
            m_pGoalArbitrationRegulator = new Regulator(script.GetDouble("Bot_GoalAppraisalUpdateFreq"));
            m_pTargetSelectionRegulator = new Regulator(script.GetDouble("Bot_TargetingUpdateFreq"));
            m_pTriggerTestRegulator = new Regulator(script.GetDouble("Bot_TriggerUpdateFreq"));
            m_pVisionUpdateRegulator = new Regulator(script.GetDouble("Bot_VisionUpdateFreq"));

            //create the goal queue
            m_pBrain = new Goal_Think(this);

            //create the targeting system
            m_pTargSys = new Dan_TargetingSystem(this);

            m_pWeaponSys = new Dan_WeaponSystem(this,
                                                  script.GetDouble("Bot_ReactionTime"),
                                                  script.GetDouble("Bot_AimAccuracy"),
                                                  script.GetDouble("Bot_AimPersistance"));

            m_pSensoryMem = new Raven_SensoryMemory(this, script.GetDouble("Bot_MemorySpan"));
        }

        public new void Dispose()
        {
            Debug.WriteLine("deleteing raven bot (id = " + ID() + ")");


            m_pBrain.Dispose();
            m_pPathPlanner.Dispose();
            m_pSteering.Dispose();
            m_pWeaponSelectionRegulator.Dispose();
            m_pTargSys.Dispose();
            m_pGoalArbitrationRegulator.Dispose();
            m_pTargetSelectionRegulator.Dispose();
            m_pTriggerTestRegulator.Dispose();
            m_pVisionUpdateRegulator.Dispose();
            m_pWeaponSys.Dispose();
            m_pSensoryMem.Dispose();
        }

        public override Color GetBodyColor()
        {
            return Color.Blue;
        }

        public override Color GetHeadColor()
        {
            return Color.Brown;
        }

        public override string GetName()
        {
            return "Dan";
        }

        protected override void DoUpdate()
        {
            //process the currently active goal. Note this is required even if the bot
            //is under user control. This is because a goal is created whenever a user 
            //clicks on an area of the map that necessitates a path planning request.
            m_pBrain.Process();

            //Calculate the steering force and update the bot's velocity and position
            UpdateMovement();

            //if the bot is under AI control but not scripted
            if (!isPossessed())
            {
                //examine all the opponents in the bots sensory memory and select one
                //to be the current target
                if (m_pTargetSelectionRegulator.isReady())
                {
                    m_pTargSys.Update();
                }

                //appraise and arbitrate between all possible high level goals
                if (m_pGoalArbitrationRegulator.isReady())
                {
                    m_pBrain.Arbitrate();
                }

                //update the sensory memory with any visual stimulus
                if (m_pVisionUpdateRegulator.isReady())
                {
                    m_pSensoryMem.UpdateVision();
                }

                //select the appropriate weapon to use from the weapons currently in
                //the inventory
                if (m_pWeaponSelectionRegulator.isReady())
                {
                    m_pWeaponSys.SelectWeapon();
                }

                //this method aims the bot's current weapon at the current target
                //and takes a shot if a shot is possible
                m_pWeaponSys.TakeAimAndShoot();
            }
        }

        public override bool HandleMessage(Telegram msg)
        {
            //first see if the current goal accepts the message
            if (GetBrain().HandleMessage(msg)) return true;

            //handle any messages not handles by the goals
            switch (msg.Msg)
            {
                case (int) message_type.Msg_TakeThatMF:

                    //just return if already dead or spawning
                    if (isDead() || isSpawning()) return true;

                    //the extra info field of the telegram carries the amount of damage
                    ReduceHealth((int) msg.ExtraInfo);

                    //if this bot is now dead let the shooter know
                    if (isDead())
                    {
                        Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                               ID(),
                                               msg.Sender,
                                               (int) message_type.Msg_YouGotMeYouSOB,
                                               MessageDispatcher.NO_ADDITIONAL_INFO);
                    }

                    return true;

                case (int) message_type.Msg_YouGotMeYouSOB:

                    IncrementScore();

                    //the bot this bot has just killed should be removed as the target
                    m_pTargSys.ClearTarget();

                    return true;

                case (int) message_type.Msg_GunshotSound:

                    //add the source of this sound to the bot's percepts
                    GetSensoryMem().UpdateWithSoundSource((AbstractBot) msg.ExtraInfo);

                    return true;

                case (int) message_type.Msg_UserHasRemovedBot:
                    {
                        var pRemovedBot = (AbstractBot)msg.ExtraInfo;

                        GetSensoryMem().RemoveBotFromMemory(pRemovedBot);

                        //if the removed bot is the target, make sure the target is cleared
                        if (pRemovedBot == GetTargetSys().GetTarget())
                        {
                            GetTargetSys().ClearTarget();
                        }

                        return true;
                    }


                default:
                    return false;
            }
        }
    }
}