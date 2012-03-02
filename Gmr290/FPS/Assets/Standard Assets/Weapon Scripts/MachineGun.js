
var range = 100.0;
var fireRate = 0.05;
var force = 10.0;
var damage = 5.0;
var bulletsPerClip = 40;
var clips = 20;
var reloadTime = 0.5;
private var hitParticles : ParticleEmitter;
var muzzleFlash : Renderer;

private var bulletsLeft : int = 0;
private var nextFireTime = 0.0;
private var lastFrameShot = -2;

function Start ()
{
	hitParticles = GetComponentInChildren (ParticleEmitter);
	
	if (hitParticles)
	{
		hitParticles.emit = false;
	}
	
	audio.Stop ();
	muzzleFlash.enabled = false;
	
	bulletsLeft = bulletsPerClip;
}

function LateUpdate ()
{
	if (muzzleFlash)
	{
		if (lastFrameShot == Time.frameCount && Time.frameCount != 0)
		{
			muzzleFlash.transform.localRotation = Quaternion.AngleAxis (Random.value * 360, Vector3.forward);
			muzzleFlash.enabled = true;
			
			if (audio)
			{
				/*if (!audio.isPlaying)
				{
					audio.Play ();
				}*/
				if (audio.isPlaying)
					audio.Stop ();
				audio.Play ();
//				audio.loop = true;
			}
		}
		else
		{
			muzzleFlash.enabled = false;
			enabled = false;
			
			if (audio)
			{
//				audio.loop = false;
			}
		}
	}
}

function Fire ()
{
	if (bulletsLeft == 0)
	{
		return;
	}
	
	if (Time.time - fireRate > nextFireTime)
	{
		nextFireTime = Time.time - Time.deltaTime;
	}
	
	while (nextFireTime < Time.time && bulletsLeft != 0)
	{
		FireOneShot ();
		nextFireTime += fireRate;
	}
}

function FireOneShot ()
{
	var direction = transform.TransformDirection (Vector3.forward);
	var hit : RaycastHit;
	
	if (Physics.Raycast (transform.position, direction, hit, range))
	{
		if (hit.rigidbody)
		{
			hit.rigidbody.AddForceAtPosition (force * direction, hit.point);
		}
		
		if (hitParticles)
		{
			hitParticles.transform.position = hit.point;
			hitParticles.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
			hitParticles.Emit ();
		}
		
		hit.collider.SendMessageUpwards ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
	}
	
	bulletsLeft--;
	
	lastFrameShot = Time.frameCount;
	enabled = true;
	
	if (bulletsLeft == 0)
	{
		Reload ();
	}
}

function Reload ()
{
	yield WaitForSeconds (reloadTime);
	
	if (clips > 0)
	{
		clips--;
		bulletsLeft = bulletsPerClip;
	}
}

function GetBulletsLeft ()
{
	return bulletsLeft;
}




