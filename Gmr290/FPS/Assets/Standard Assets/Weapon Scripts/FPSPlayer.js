
var maximumHitPoints = 100.0;
var hitPoints = 100.0;

var bulletGUI : GUIText;
var rocketGUI : DrawRockets;
var healthGUI : GUITexture;

var walkSounds : AudioClip [];
var painLittle : AudioClip;
var painBig : AudioClip;
var die : AudioClip;
var audioStepLength = 0.3;

private var machineGun : MachineGun;
private var rocketLauncher : RocketLauncher;
private var shotgun : Shotgun;
private var healthGUIWidth = 0.0;
private var gotHitTimer = -1.0;

var rocketTextures : Texture [];

function Awake ()
{
	machineGun = GetComponentInChildren (MachineGun);
	rocketLauncher = GetComponentInChildren (RocketLauncher);
	shotgun = GetComponentInChildren (Shotgun);
	
	PlayStepSounds ();
	
	healthGUIWidth = healthGUI.pixelInset.width;
}

function ApplyDamage (damage : float)
{
	if (hitPoints < 0.0)
		return;
	
	hitPoints -= damage;
	
	if (Time.time > gotHitTimer && painBig && painLittle)
	{
		if (hitPoints < maximumHitPoints * 0.2 || damage > 20)
		{
			audio.PlayOneShot (painBig, 1.0 / audio.volume);
			gotHitTimer = Time.time + Random.Range (painBig.length * 2, painBig.length * 3);
		}
		else
		{
			audio.PlayOneShot (painLittle, 1.0 / audio.volume);
			gotHitTimer = Time.time + Random.Range (painLittle.length * 2, painLittle.length * 3);
		}
	}
	
	if (hitPoints < 0.0)
		Die ();
}

function Die ()
{
	if (die)
		AudioSource.PlayClipAtPoint (die, transform.position);
	
	var coms : Component [] = GetComponentsInChildren (MonoBehaviour);
	for (var b in coms)
	{
		var p : MonoBehaviour = b as MonoBehaviour;
		if (p)
			p.enabled = false;
	}
	
	LevelLoadFade.FadeAndLoadLevel (Application.loadedLevel, Color.white, 2.0);
}

function LateUpdate ()
{
	UpdateGUI ();
}

function PlayStepSounds ()
{
	var controller : CharacterController = GetComponent (CharacterController);
	while (true)
	{
		if (controller.isGrounded && controller.velocity.magnitude > 0.3)
		{
			audio.clip = walkSounds [Random.Range (0, walkSounds.length)];
			audio.Play ();
			yield WaitForSeconds (audioStepLength);
		}
		else
		{
			yield;
		}
	}
}

function UpdateGUI ()
{
	var healthFraction = Mathf.Clamp01 (hitPoints / maximumHitPoints);
	
	healthGUI.pixelInset.xMax = healthGUI.pixelInset.xMin + healthGUIWidth * healthFraction;
	
	var playerWeapons = GetComponentInChildren (PlayerWeapons);
	var selectedWeapon = 0;
	if (playerWeapons)
		selectedWeapon = playerWeapons.selectedWeapon;
	
	if (machineGun && selectedWeapon == 0)
		bulletGUI.text = machineGun.GetBulletsLeft ().ToString ();
	else if (rocketLauncher && selectedWeapon == 1)
		bulletGUI.text = rocketLauncher.GetRocketsLeft ().ToString ();
	else if (shotgun && selectedWeapon == 2)
		bulletGUI.text = shotgun.GetBulletsLeft ().ToString ();
	
	if (rocketLauncher)
	{
		rocketGUI.UpdateRockets (rocketLauncher.ammoCount);
	}
}
