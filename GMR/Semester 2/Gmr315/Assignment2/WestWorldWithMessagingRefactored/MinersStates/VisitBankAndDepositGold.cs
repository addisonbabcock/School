using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;

namespace WestWorldWithMessagingRefactored.MinersStates
{
    public class VisitBankAndDepositGold : State<Miner>
    {
        private static VisitBankAndDepositGold instance;

        private VisitBankAndDepositGold()
        {
        }

        public static VisitBankAndDepositGold Instance
        { get{
            if (instance == null)
            {
                instance = new VisitBankAndDepositGold();
            }

            return instance;}
        }


        public override void Enter(Miner miner)
        {
            if (miner.Location() != Location.Bank)
            {
                miner.OutputStatusMessage("Goin' to the bank. Yes siree");
                miner.ChangeLocation(Location.Bank);
            }
        }


        public override void Execute(Miner miner)
        {
            //deposit the gold
            miner.AddToWealth(miner.GoldCarried());
            miner.SetGoldCarried(0);
            miner.OutputStatusMessage(string.Format("Depositing gold.  Total savings now: {0}", miner.Wealth()));

            if (miner.Wealth() >= Miner.ComfortLevel)
            {
                miner.OutputStatusMessage("WooHoo! Rich enough for now. Back home to mah li'lle lady");
                miner.GetFSM().ChangeState(GoHomeAndSleepTilRested.Instance);
            }
            else
            {
                miner.GetFSM().ChangeState(EnterMineAndDigForNugget.Instance);
            }
        }


        public override void Exit(Miner miner)
        {
            miner.OutputStatusMessage("Leavin' the bank");

        }
    }
}
