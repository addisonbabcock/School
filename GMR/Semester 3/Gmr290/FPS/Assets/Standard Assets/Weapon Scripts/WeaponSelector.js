

@script ExecuteInEditMode ()

var gSkin : GUISkin;

private var isPaused = false;

function OnGUI ()
{
	if (!isPaused)
		return;
	
	if (gSkin)
	{
		GUI.skin = gSkin;
	}
	else
	{
		Debug.Log ("WeaponSelector: GUISkin object missing!");
	}
	
	var gos : Object [] = FindObjectsOfType (GameObject);
	
	GUI.Label (Rect (Screen.width / 2, 50, 0, 100), "Paused!", "mainMenuTitle");
	
	if (GUI.Button (Rect ((Screen.width / 2) - 200, Screen.height - 350, 400, 70), "Machine Gun"))
	{
		for (var go : GameObject in gos)
		{
			go.SendMessage ("SelectWeapon", 0, SendMessageOptions.DontRequireReceiver);
			go.SendMessage ("SetPause", false, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	if (GUI.Button (Rect ((Screen.width / 2) - 200, Screen.height - 250, 400, 70), "Rockets"))
	{
		for (var go : GameObject in gos)
		{
			go.SendMessage ("SelectWeapon", 1, SendMessageOptions.DontRequireReceiver);
			go.SendMessage ("SetPause", false, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	if (GUI.Button (Rect ((Screen.width / 2) - 200, Screen.height - 150, 400, 70), "Shotgun"))
	{
		for (var go : GameObject in gos)
		{
			go.SendMessage ("SelectWeapon", 2, SendMessageOptions.DontRequireReceiver);
			go.SendMessage ("SetPause", false, SendMessageOptions.DontRequireReceiver);
		}
	}
}

function DidPause (pause : boolean)
{
	isPaused = pause;
}

