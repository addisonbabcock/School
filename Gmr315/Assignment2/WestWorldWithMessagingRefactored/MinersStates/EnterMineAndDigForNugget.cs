#region Using

using WestWorldWithMessagingRefactored.FSM;

#endregion

namespace WestWorldWithMessagingRefactored.MinersStates
{
    public class EnterMineAndDigForNugget : State<Miner>
    {
        private static EnterMineAndDigForNugget instance;

        private EnterMineAndDigForNugget()
        {
        }

        public static EnterMineAndDigForNugget Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EnterMineAndDigForNugget();
                }

                return instance;
            }
        }

        public override void Enter(Miner miner)
        {
            //if the miner is not already located at the goldmine, he must
            //change location to the gold mine
            if (miner.Location() != Location.GoldMine)
            {
                miner.OutputStatusMessage("Walkin' to the goldmine");
                miner.ChangeLocation(Location.GoldMine);
            }
        }


        public override void Execute(Miner miner)
        {
            //Now the miner is at the goldmine he digs for gold until he
            //is carrying in excess of MaxNuggets. If he gets thirsty during
            //his digging he packs up work for a while and changes state to
            //gp to the saloon for a whiskey.
            miner.AddToGoldCarried(1);
            miner.IncreaseFatigue();
            miner.OutputStatusMessage("Pickin' up a nugget");

            //if enough gold mined, go and put it in the bank
            if (miner.PocketsFull())
            {
                miner.GetFSM().ChangeState(VisitBankAndDepositGold.Instance);
            }

            if (miner.Thirsty())
            {
                miner.GetFSM().ChangeState(QuenchThirst.Instance);
            }
        }


        public override void Exit(Miner miner)
        {
            miner.OutputStatusMessage("Ah'm leavin' the goldmine with mah pockets full o' sweet gold");
        }
    }
}