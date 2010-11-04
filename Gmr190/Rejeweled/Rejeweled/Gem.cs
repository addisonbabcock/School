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

		public Gem(GemType type)
		{
			mType = type;
			mAnimationState = GemAnimationState.Idle;
			mIsExplosive = false;
		}
	}

	class GemGrid : List<List<Gem>>
	{
		//intentionally empty
	}
}
