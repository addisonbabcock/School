
function Start ()
{
	SelectWeapon (0);
}

function Update ()
{
	if (Input.GetButton ("Fire1"))
//	if (Input.GetMouseButton (0))
	{
		BroadcastMessage ("Fire");
	}
	
	if (Input.GetKeyDown ("1"))
	{
		SelectWeapon (0);
	}
	else if (Input.GetKeyDown ("2"))
	{
		SelectWeapon (1);
	}
	
	if (Input.GetKeyDown ("r"))
	{
		BroadcastMessage ("Reload");
	}
}

function SelectWeapon (index : int)
{
	for (var i = 0; i < transform.childCount; ++i)
	{
		if (i == index)
		{
			transform.GetChild (i).gameObject.SetActiveRecursively (true);
			
			if (transform.audio)
			{
				transform.audio.Stop ();
			}
		}
		else
		{
			transform.GetChild (i).gameObject.SetActiveRecursively (false);
		}
	}
}