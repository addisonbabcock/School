
var hitPoints = 100.0;
var detonationDelay = 0.0;
var explosion : Transform;
var deadReplacement : Rigidbody;

function ApplyDamage (damage : float)
{
	if (hitPoints <= 0.0)
		return;
	
	hitPoints -= damage;
	if (hitPoints <= 0)
	{
		var emitter : ParticleEmitter = GetComponentInChildren (ParticleEmitter);
		if (emitter)
		{
			emitter.emit = true;
		}
		
		Invoke ("DelayedDetonate", detonationDelay);
	}
}

function DelayedDetonate ()
{
	BroadcastMessage ("Detonate");
}

function Detonate ()
{
	if (explosion)
	{
		Instantiate (explosion, transform.position, transform.rotation);
	}
	
	if (deadReplacement)
	{
		var dead : Rigidbody = Instantiate (deadReplacement, transform.position, transform.rotation);
		
		dead.velocity = gameObject.rigidbody.velocity;
		dead.angularVelocity = gameObject.rigidbody.angularVelocity;
	}
	
	var emitter : ParticleEmitter = GetComponentInChildren (ParticleEmitter);
	if (emitter)
	{
		emitter.emit = false;
		emitter.transform.parent = null;
	}

	Destroy (gameObject);
}

@script RequireComponent (Rigidbody)

