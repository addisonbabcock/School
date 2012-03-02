
var explosion : GameObject;

function OnCollisionEnter (collision : Collision)
{
	print ("Is this even getting called?");
	var contact : ContactPoint = collision.contacts [0];
	var rotation = Quaternion.FromToRotation (Vector3.up, contact.normal);
	var instatiatedExplosion : GameObject = Instantiate (explosion, contact.point, rotation);
	Destroy (gameObject);
}