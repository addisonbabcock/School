
var projectile : Rigidbody;
var initialSpeed = 20.0;
var reloadTime = 0.033;
var ammoCount = 20;
private var lastShot = -10.0;

function Start ()
{
	if (audio)
		audio.Stop ();
}

function Fire ()
{
	if (ammoCount <= 0)
	{
		print ("Out of ammo. Cannot shoot.");
		return;
	}

	if (Time.time > reloadTime + lastShot)
	{
		var instantiatedProjectile : Rigidbody = Instantiate (projectile, transform.position, transform.rotation);
		instantiatedProjectile.velocity = transform.TransformDirection (Vector3 (0, 0, initialSpeed));
		Physics.IgnoreCollision (instantiatedProjectile.collider, transform.root.collider);
		
		lastShot = Time.time;
		ammoCount--;
		
		if (audio)
			audio.Play ();
	}
}

function Reload ()
{
	print ("Reloading");
	ammoCount = 20; 
}

