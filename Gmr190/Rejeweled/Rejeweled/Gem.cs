using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rejeweled
{
	class Gem
	{
		private GemType mType;
		private GemAnimationState mAnimationState;
		private bool mIsExplosive;
        private PlayAreaCoords mBoardLocation;

		public Gem(GemType type)
		{
			mType = type;
			mAnimationState = GemAnimationState.Idle;
			mIsExplosive = false;
            mBoardLocation = new PlayAreaCoords();
		}

        public void Swap(Gem with)
        {
            PlayAreaCoords temp = with.mBoardLocation;
            with.mBoardLocation = mBoardLocation;
            mBoardLocation = temp;
        }

        public bool IsAt(PlayAreaCoords coords)
        {
            return mBoardLocation == coords;
        }

        public bool IsExplosive
        {
            get { return mIsExplosive; }
            set { mIsExplosive = value; }
        }

        public void Update()
        {
            //just shutting up compiler warnings for now...
            //fill this in later.
            switch (mAnimationState)
            {
                case GemAnimationState.Disappearing:
                    break;
            }
        }
	}
}
