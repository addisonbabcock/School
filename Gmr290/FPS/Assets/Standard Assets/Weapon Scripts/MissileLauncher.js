
var projectile : Rigidbody;
var speed = 100;

function Update () 
{
	if (Input.GetMouseButton (0))
	{
		var instantiatedProjectile : Rigidbody = Instantiate (
			projectile, transform.position, transform.rotation);
		instantiatedProjectile.velocity = transform.TransformDirection (
			Vector3 (0, 0, speed) );
		Physics.IgnoreCollision (instantiatedProjectile.collider, transform.root.collider );
	}
}