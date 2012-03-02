
@script ExecuteInEditMode ()

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
	GUI.Label (Rect (Screen.width / 2, 50, 0, 100), "Lerpz Loots!", "mainMenuTitle");
	
	GUI.Label (Rect (Screen.width / 2, 150, 0, 50), "Collect the coins and escape!", "instructions");
	
	if (GUI.Button (Rect ((Screen.width / 2) - 70, Screen.height - 160, 140, 70), "Play"))
	{
		isLoading = true;
		Application.LoadLevel ("TheGame");
	}
	
	var isWebPlayer = (
		Application.platform == RuntimePlatform.OSXWebPlayer || 
		Application.platform == RuntimePlatform.WindowsWebPlayer);
	if (!isWebPlayer)
	{
		if (GUI.Button (Rect ((Screen.width / 2) - 70, Screen.height - 80, 140, 70), "Quit"))
		{
			Application.Quit ();
		}
	}
	
	if (isLoading)
	{
		GUI.Label (Rect ((Screen.width / 2) - 110, (Screen.height / 2) - 60, 400, 70), "Loading", "mainMenuTitle");
	}
}

