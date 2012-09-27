// LevelStatus: Master level state machine script.


// This is where info like the number of items the player must collect in order to complete the level lives.
var exitGateway : GameObject;
var levelGoal : GameObject;
var unlockedSound : AudioClip;
var levelCompleteSound : AudioClip;
var mainCamera : GameObject;
var unlockedCamera : GameObject;
var levelCompletedCamera : GameObject;
var levelStartedCamera : GameObject;

var itemsNeeded: int = 20;	// This is how many fuel canisters the player must collect.

private var playerLink : GameObject;

// Awake(): Called by Unity when the script has loaded.
// We use this function to initialise our link to the Lerpz GameObject.
function Awake()
{
	levelGoal.GetComponent (MeshCollider).isTrigger = false;
	playerLink = GameObject.Find ("Player");
	if (!playerLink)
		Debug.Log ("Could not get link to Lerpz");
	levelGoal.GetComponent (MeshCollider).isTrigger = false;
	
	PlayIntroAnimation ();
}

function PlayIntroAnimation ()
{
	Debug.Log ("Trying to start up the animation...");
	var mainCamera = Camera.main;
	Camera.main.enabled = false;
//	playerLink.GetComponent (AudioListener).enabled = false;
	levelStartedCamera.active = true;
//	levelStartedCamera.GetComponent (AudioListener).enabled = true;
	playerLink.GetComponent (ThirdPersonController).HidePlayer ();
	levelStartedCamera.animation.Play ();
	Debug.Log ("Animation started.");
	
	yield WaitForSeconds (12);//(levelCompletedCamera.animation.clip.length);
	Debug.Log ("Finished opening animation.");
	levelStartedCamera.active = false;
//	playerLink.GetComponent (AudioListener).enabled = true;
//	Camera.main.enabled = true;
	mainCamera.enabled = true;
	playerLink.GetComponent (ThirdPersonController).ShowPlayer ();
}

function UnlockLevelExit ()
{
	mainCamera.GetComponent(AudioListener).enabled = false;
	unlockedCamera.active = true;
	unlockedCamera.GetComponent (AudioListener).enabled = true;
	exitGateway.GetComponent(AudioSource).Stop ();
	if (unlockedSound)
	{
		AudioSource.PlayClipAtPoint (unlockedSound, unlockedCamera.GetComponent (Transform).position, 2.0);
	}
	
	yield WaitForSeconds (1);
	
	exitGateway.active = false;
	yield WaitForSeconds (0.2);
	exitGateway.active = true;
	yield WaitForSeconds (0.2);
	exitGateway.active = false;
	
	levelGoal.GetComponent(MeshCollider).isTrigger = true;
	
	yield WaitForSeconds (4);
	
	unlockedCamera.active = false;
	unlockedCamera.GetComponent (AudioListener).enabled = false;
	mainCamera.GetComponent (AudioListener).enabled = true;
}

function LevelCompleted ()
{
	mainCamera.GetComponent (AudioListener).enabled = false;
//	levelCompletedCamera.active = true;
//	levelCompletedCamera.GetComponent (AudioListener).enabled = true;
	
	playerLink.GetComponent (ThirdPersonController).HidePlayer ();
	playerLink.transform.position += Vector3.up * 500.0;
	
	if (levelCompleteSound)
	{
/*		AudioSource.PlayClipAtPoint (levelCompleteSound, levelGoal.transform.position, 2.0);
		levelGoal.animation.Play ();
		levelCompletedCamera.animation.Play ();
		
		yield WaitForSeconds (levelGoal.animation.clip.length);*/
		
		Application.LoadLevel ("Winner");
	}
}