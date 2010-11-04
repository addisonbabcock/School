using System.Collections.Generic;

namespace Rejeweled
{
	enum GemType
	{
		Blue,
		Green,
		Red,
		Purple,
		Yellow,
		White,

		HyperCube
	}

    enum GemAnimationState
    {
        SlidingDown,
        SlidingUp,
        SlidingLeft,
        SlidingRight,

        Falling,

        Disappearing,

        Exploding,

        Idle
    }
}
