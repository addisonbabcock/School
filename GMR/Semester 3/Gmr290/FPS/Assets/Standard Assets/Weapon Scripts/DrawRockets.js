
@script ExecuteInEditMode ()

var rocketTexture : Texture2D;
var posX : float;
var posY : float;
var rocketWidth : float = 10;

var drawWidth : float = 256;
var drawHeight : float = 64;

private var maxRockets : int = 0;
private var rocketCount : int = 20;

function Start ()
{
	maxRockets = rocketTexture.width / rocketWidth;
}

function OnGUI ()
{
	GUI.BeginGroup (Rect (
		posX, 
		Screen.height - posY, 
		rocketTexture.width - ((maxRockets - rocketCount) * rocketWidth), 
		rocketTexture.height));
	GUI.DrawTexture (Rect (
			0, 
			0, 
			drawWidth, 
			drawHeight), 
		rocketTexture);
	GUI.EndGroup ();
}

function UpdateRockets (input : int)
{
	rocketCount = input;
}