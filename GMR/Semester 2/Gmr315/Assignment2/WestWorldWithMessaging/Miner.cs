#region Using

using System;
using WestWorldWithMessaging.FSM;
using WestWorldWithMessaging.Messaging;

#endregion

namespace WestWorldWithMessaging
{
    public class Miner : BaseGameEntity
    {
        //the amount of gold a miner must have before he feels comfortable
        public const int ComfortLevel = 5;

        //the amount of nuggets a miner can carry
        public const int MaxNuggets = 3;

        //above this value a miner is thirsty
        public const int ThirstLevel = 5;

        //above this value a miner is sleepy
        public const int TirednessThreshold = 5;
        private readonly StateMachine<Miner> m_pStateMachine;
        private int m_iFatigue;

        //how many nuggets the miner has in his pockets
        private int m_iGoldCarried;

        private int m_iMoneyInBank;

        //the higher the value, the thirstier the miner
        private int m_iThirst;
        private location_type m_Location;

        //the higher the value, the more tired the miner


        public Miner(int id) : base(id)
        {
            m_Location = location_type.shack;
            m_iGoldCarried = 0;
            m_iMoneyInBank = 0;
            m_iThirst = 0;
            m_iFatigue = 0;
            m_pStateMachine = new StateMachine<Miner>(this);
            m_pStateMachine.SetCurrentState(GoHomeAndSleepTilRested.Instance());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                m_pStateMachine.Dispose();
            }
        }

        public StateMachine<Miner> GetFSM()
        {
            return m_pStateMachine;
        }


        public location_type Location()
        {
            return m_Location;
        }

        public void ChangeLocation(location_type loc)
        {
            m_Location = loc;
        }

        public int GoldCarried()
        {
            return m_iGoldCarried;
        }

        public void SetGoldCarried(int val)
        {
            m_iGoldCarried = val;
        }

        public bool PocketsFull()
        {
            return m_iGoldCarried >= MaxNuggets;
        }

        public void DecreaseFatigue()
        {
            m_iFatigue -= 1;
        }

        public void IncreaseFatigue()
        {
            m_iFatigue += 1;
        }

        public int Wealth()
        {
            return m_iMoneyInBank;
        }

        public void SetWealth(int val)
        {
            m_iMoneyInBank = val;
        }

        public void BuyAndDrinkAWhiskey()
        {
            m_iThirst = 0;
            m_iMoneyInBank -= 2;
        }

        //-----------------------------------------------------------------------------
        public void AddToGoldCarried(int val)
        {
            m_iGoldCarried += val;

            if (m_iGoldCarried < 0) m_iGoldCarried = 0;
        }


//-----------------------------------------------------------------------------
        public void AddToWealth(int val)
        {
            m_iMoneyInBank += val;

            if (m_iMoneyInBank < 0) m_iMoneyInBank = 0;
        }


//-----------------------------------------------------------------------------
        public bool Thirsty()
        {
            if (m_iThirst >= ThirstLevel)
            {
                return true;
            }

            return false;
        }


//-----------------------------------------------------------------------------
        public override void Update()
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            m_iThirst += 1;
            m_pStateMachine.Update();
            Console.ForegroundColor = currentColor;
        }

        public override bool HandleMessage(Telegram msg)
        {
            return m_pStateMachine.HandleMessage(msg);
        }


//-----------------------------------------------------------------------------
        public bool Fatigued()
        {
            if (m_iFatigue > TirednessThreshold)
            {
                return true;
            }

            return false;
        }
    }
}