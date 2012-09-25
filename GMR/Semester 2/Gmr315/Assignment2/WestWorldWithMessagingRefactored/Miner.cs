#region Using

using System;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;
using WestWorldWithMessagingRefactored.MinersStates;

#endregion

namespace WestWorldWithMessagingRefactored
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
        private readonly StateMachine<Miner> stateMachine;
        private int fatigue;

        //how many nuggets the miner has in his pockets
        private int goldCarried;

        private int moneyInBank;

        //the higher the value, the thirstier the miner
        private int thirst;
        private Location currentLocation;

        //the higher the value, the more tired the miner


        public Miner(int id) : base(id)
        {
            currentLocation = WestWorldWithMessagingRefactored.Location.Shack;
            goldCarried = 0;
            moneyInBank = 0;
            thirst = 0;
            fatigue = 0;
            stateMachine = new StateMachine<Miner>(this, GoHomeAndSleepTilRested.Instance);
            OutputMessageColor = ConsoleColor.Red;
        }

        public StateMachine<Miner> GetFSM()
        {
            return stateMachine;
        }


        public Location Location()
        {
            return currentLocation;
        }

        public void ChangeLocation(Location newLocation)
        {
            currentLocation = newLocation;
        }

        public int GoldCarried()
        {
            return goldCarried;
        }

        public void SetGoldCarried(int goldAmount)
        {
            goldCarried = goldAmount;
        }

        public bool PocketsFull()
        {
            return goldCarried >= MaxNuggets;
        }

        public void DecreaseFatigue()
        {
            fatigue -= 1;
        }

        public void IncreaseFatigue()
        {
            fatigue += 1;
        }

        public int Wealth()
        {
            return moneyInBank;
        }

        public void SetWealth(int moneyDeposit)
        {
            moneyInBank = moneyDeposit;
        }

        public void BuyAndDrinkAWhiskey()
        {
            thirst = 0;
            moneyInBank -= 2;
        }

        public void AddToGoldCarried(int nuggets)
        {
            goldCarried += nuggets;

            if (goldCarried < 0) goldCarried = 0;
        }

        public void AddToWealth(int nuggets)
        {
            moneyInBank += nuggets;

            if (moneyInBank < 0) moneyInBank = 0;
        }

        public bool Thirsty()
        {
            if (thirst >= ThirstLevel)
            {
                return true;
            }

            return false;
        }

        public override void Update()
        {
            thirst += 1;
            GetFSM().Update();
        }

        public override bool HandleMessage(Message message)
        {
            return GetFSM().HandleMessage(message);
        }

        public bool Fatigued()
        {
            if (fatigue > TirednessThreshold)
            {
                return true;
            }

            return false;
        }
    }
}