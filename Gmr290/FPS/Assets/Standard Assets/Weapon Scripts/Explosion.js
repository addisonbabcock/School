
var explosionTime = 1.0;
var explosionRadius = 50000.0;
var explosionPower = 50000.0;

function Start ()
{
	Destroy (gameObject, explosionTime);
	var colliders : Collider [] = Physics.OverlapSphere (transform.position, explosionRadius);
	for (var hit in colliders)
	{
		if (hit.rigidbody)
		{
			hit.rigidbody.AddExplosionForce (explosionPower, transform.position, explosionRadius);
		}
		if (hit.gameObject)
		{
			hit.gameObject.SendMessageUpwards ("ApplyDamage", 30.0, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	if (particleEmitter)
	{
		particleEmitter.emit = true;
		yield WaitForSeconds (0.5);
		particleEmitter.emit = false;
	}
}
