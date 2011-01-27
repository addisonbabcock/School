#region Using

using System;

#endregion

namespace Westworld1
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
        private int m_iFatigue;

        //how many nuggets the miner has in his pockets
        private int m_iGoldCarried;

        private int m_iMoneyInBank;

        //the higher the value, the thirstier the miner
        private int m_iThirst;
        private location_type m_Location;
        private State m_pCurrentState;

        //the higher the value, the more tired the miner


        public Miner(int id) : base(id)
        {
            m_Location = location_type.shack;
            m_iGoldCarried = 0;
            m_iMoneyInBank = 0;
            m_iThirst = 0;
            m_iFatigue = 0;
            m_pCurrentState = GoHomeAndSleepTilRested.Instance();
        }


        //this method changes the current state to the new state. It first
        //calls the Exit() method of the current state, then assigns the
        //new state to m_pCurrentState and finally calls the Entry()
        //method of the new state.
        public void ChangeState(State pNewState)
        {
            //make sure both states are both valid before attempting to 
            //call their methods
            if (m_pCurrentState == null || pNewState == null)
            {
                throw new Exception("Assertion exception from original code");
            }

            //call the exit method of the existing state
            m_pCurrentState.Exit(this);

            //change state to the new state
            m_pCurrentState = pNewState;

            //call the entry method of the new state
            m_pCurrentState.Enter(this);
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
            m_iThirst += 1;

            if (m_pCurrentState != null)
            {
                m_pCurrentState.Execute(this);
            }
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