
private var paused = false;

function Start ()
{
	if (Application.platform == RuntimePlatform.OSXWebPlayer ||
		Application.platform == RuntimePlatform.WindowsWebPlayer)
	{
		SetPause (true);
	}
	else
	{
		SetPause (false);
		Screen.lockCursor = true;
	}
}

function OnApplicationQuit ()
{
	Time.timeScale = 1;
}

function SetPause (pause : boolean)
{
	print ("Where did this come from!?!");
	Input.ResetInputAxes ();
	var gos : Object [] = FindObjectsOfType (GameObject);
	for (var go : GameObject in gos)
		go.SendMessage ("DidPause", pause, SendMessageOptions.DontRequireReceiver);
	
	transform.position = Vector3.zero;
	
	if (pause)
	{
		Time.timeScale = 0;
		transform.position = Vector3 (0.5, 0.5, 0);
		guiText.anchor = TextAnchor.MiddleCenter;
	}
	else
	{
		guiText.anchor = TextAnchor.UpperLeft;
		transform.position = Vector3 (0, 1, 0);
		Time.timeScale = 1;
	}
}

function DidPause (pause : boolean)
{
	if (pause)
	{
		guiText.enabled = true;
		guiText.text = "Hit escape to start playing";
	}
	else
	{
		guiText.enabled = true;
		guiText.text = "Escape to show the cursor";
	}
	
	paused = pause;
}

function OnMouseDown ()
{
	Screen.lockCursor = true;
}

//private var wasLocked = false;

function LateUpdate ()
{
/*	if (Input.GetMouseButton (0))
	{
		if (paused)
			SetPause (false);
	}*/
	
	if (Input.GetKeyDown (KeyCode.Escape))
	{
		SetPause (!paused);
	}
}











