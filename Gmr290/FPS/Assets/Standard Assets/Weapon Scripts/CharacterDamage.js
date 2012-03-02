
var hitPoints = 100.0;
var deadReplacement : Transform;
var dieSound : AudioClip;

function ApplyDamage (damage : float)
{
	if (hitPoints <= 0.0)
	{
		return;
	}
	
	hitPoints -= damage;
	
	if (hitPoints <= 0.0)
	{
		Detonate ();
	}
}

function Detonate ()
{
	Destroy (gameObject);
	
	if (dieSound)
	{
		AudioSource.PlayClipAtPoint (dieSound, transform.position);
	}
	
	if (deadReplacement)
	{
		var dead : Transform = Instantiate (deadReplacement, transform.position, transform.rotation);
		
		CopyTransformRecurse (transform, dead);
	}
}

static function CopyTransformRecurse (src : Transform, dst : Transform)
{
	dst.position = src.position;
	dst.rotation = src.rotation;
	
	for (var child : Transform in dst)
	{
		var curSrc = src.Find (child.name);
		if (curSrc)
		{
			CopyTransformRecurse (curSrc, child);
		}
	}
}
