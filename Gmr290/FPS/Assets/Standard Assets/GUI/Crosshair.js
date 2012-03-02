
var crosshairTexture : Texture2D;
var position : Rect;

function Start ()
{
	position = Rect (
		(Screen.width - crosshairTexture.width / 2) / 2,
		(Screen.height - crosshairTexture.height / 2) / 2,
		crosshairTexture.width / 2,
		crosshairTexture.height / 2);
//	position = Rect (-16, -16, 32, 32);
}

function OnGUI ()
{
	GUI.DrawTexture (position, crosshairTexture);
}