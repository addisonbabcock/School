
var selectedWeapon : int;

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
	else if (Input.GetKeyDown ("3"))
	{
		SelectWeapon (2);
	}
	
	if (Input.GetKeyDown ("r"))
	{
		BroadcastMessage ("Reload");
	}
}

function SelectWeapon (index : int)
{
	print ("SelectWeapon responding");
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
	
	selectedWeapon = index;
}

function SelectShotgun ()
{
	print ("SelectShotgun responding");
	SelectWeapon (2);
}