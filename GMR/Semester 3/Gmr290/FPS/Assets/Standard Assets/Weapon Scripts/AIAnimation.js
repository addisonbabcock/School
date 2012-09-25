
var minimumRunSpeed = 1.0;

function Start ()
{
	animation.wrapMode = WrapMode.Loop;
	
	animation["shoot"].wrapMode = WrapMode.Once;
	
	animation["idle"].layer = -1;
	animation["walk"].layer = -1;
	animation["run"].layer = -1;
	
	animation.Stop ();
}

function SetSpeed (speed : float)
{
	if (speed > minimumRunSpeed)
		animation.CrossFade ("run");
	else
		animation.CrossFade ("idle");
}

