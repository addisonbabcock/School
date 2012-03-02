
var gSkin : GUISkin;
var backdrop : Texture2D;
private var isLoading = false;

function OnGUI ()
{
	if (gSkin)
	{
		GUI.skin = gSkin;
	}
	else
	{
		Debug.Log ("StartMenuGUI: GUISkin object missing!");
	}
	
	var backgroundStyle : GUIStyle = new GUIStyle ();
	backgroundStyle.normal.background = backdrop;
	GUI.Label (Rect ((Screen.width - (Screen.height * 2)) * 0.75, 0, Screen.height * 2, Screen.height), "", backgroundStyle);
	
}

