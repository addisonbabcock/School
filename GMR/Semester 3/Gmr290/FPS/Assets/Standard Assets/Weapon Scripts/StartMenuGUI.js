
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
	GUI.Label (Rect (Screen.width / 2, 50, 0, 100), "FPS!", "mainMenuTitle");
	
	GUI.Label (Rect (Screen.width / 2, 150, 0, 50), "Kill the sentry guns and robot", "instructions");
	GUI.Label (Rect (Screen.width / 2, 200, 0, 50), "while trying not to die.", "instructions");
	GUI.Label (Rect (Screen.width / 2, 250, 0, 50), "WASD - Move", "instructions");
	GUI.Label (Rect (Screen.width / 2, 300, 0, 50), "1. Machine gun", "instructions");
	GUI.Label (Rect (Screen.width / 2, 350, 0, 50), "2. Rocket launcher", "instructions");
	GUI.Label (Rect (Screen.width / 2, 400, 0, 50), "3. Shotgun", "instructions");
	GUI.Label (Rect (Screen.width / 2, 450, 0, 50), "Click play to start", "instructions");
	
	if (GUI.Button (Rect ((Screen.width / 2) - 70, Screen.height - 160, 140, 70), "Play"))
	{
		isLoading = true;
		Application.LoadLevel ("MysteryScene");
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
		GUI.Label (Rect ((Screen.width / 2), 550, 0, 50), "Loading", "mainMenuTitle");
	}
}

