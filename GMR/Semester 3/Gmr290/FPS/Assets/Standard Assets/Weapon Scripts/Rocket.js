
var explosion : GameObject;
var timeOut = 3.0;

function Start ()
{
	Invoke ("Kill", timeOut);
}

function OnCollisionEnter (collision : Collision)
{
	print ("Colliding");
	var contact : ContactPoint = collision.contacts [0];
	var rotation = Quaternion.FromToRotation (Vector3.up, contact.normal);
	Instantiate (explosion, contact.point, rotation);
	
	if (collision.gameObject)
		collision.gameObject.SendMessageUpwards ("ApplyDamage", 30.0, SendMessageOptions.DontRequireReceiver);
	
	Kill ();
}

function Kill ()
{
	var emitter : ParticleEmitter = GetComponentInChildren (ParticleEmitter);
	if (emitter)
		emitter.emit = false;
	
	transform.DetachChildren ();
	
	print ("Destroying " + gameObject.name);
	Destroy (gameObject);
}