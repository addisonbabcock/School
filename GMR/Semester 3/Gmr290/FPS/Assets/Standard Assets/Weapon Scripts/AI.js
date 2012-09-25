
var speed = 3.0;
var rotationSpeed = 5.0;
var shootRange = 15.0;
var attackRange = 30.0;
var shootAngle = 4.0;
var dontComeCloserRange = 5.0;
var delayShootTime = 0.35;
var pickNextWaypointDistance = 2.0;
var target : Transform;

private var lastShot = -10.0;

@script RequireComponent (CharacterController)

function Start ()
{
	if (target == null && GameObject.FindWithTag ("Player"))
	{
		target = GameObject.FindWithTag ("Player").transform;
	}
	
	Patrol ();
}

function Patrol ()
{
	var curWaypoint = AutoWayPoint.FindClosest (transform.position);
	while (true)
	{
		var waypointPosition = curWaypoint.transform.position;
		
		if (Vector3.Distance (waypointPosition, transform.position) < pickNextWaypointDistance)
		{
			curWaypoint = PickNextWaypoint (curWaypoint);
		}
		
		if (CanSeeTarget ())
		{
			yield StartCoroutine ("AttackPlayer");
		}
		
		MoveTowards (waypointPosition);
		
		yield;
	}
}

function CanSeeTarget () : boolean
{
	if (Vector3.Distance (transform.position, target.position) > attackRange)
		return false;
	
	var hit : RaycastHit;
	if (Physics.Linecast (transform.position, target.position, hit))
		return hit.transform == target;
	
	return false;
}

function Shoot ()
{
	animation.CrossFade ("shoot", 0.3);
	
	yield WaitForSeconds (delayShootTime);
	
	BroadcastMessage ("Fire");
	
	yield WaitForSeconds (animation ["shoot"].length - delayShootTime);
}

function AttackPlayer ()
{
	var lastVisiblePlayerPosition = target.position;
	
	while (true)
	{
		if (CanSeeTarget ())
		{
			if (target == null)
				return;
			
			var distance = Vector3.Distance (transform.position, target.position);
			if (distance > shootRange * 3)
				return;
			
			lastVisiblePlayerPosition = target.position;
			if (distance > dontComeCloserRange)
				MoveTowards (lastVisiblePlayerPosition);
			else
				RotateTowards (lastVisiblePlayerPosition);
			
			var forward = transform.TransformDirection (Vector3.forward);
			var targetDirection = lastVisiblePlayerPosition - transform.position;
			targetDirection.y = 0;
			
			var angle = Vector3.Angle (targetDirection, forward);
			
			if (distance < shootRange && angle < shootAngle)
			{
				yield StartCoroutine ("Shoot");
			}
		}
		else
		{
			yield StartCoroutine ("SearchPlayer", lastVisiblePlayerPosition);
			
			if (!CanSeeTarget ())
				return;
		}
		
		yield;
	}
}

function SearchPlayer (position : Vector3)
{
	var timeout = 3.0;
	while (timeout > 0.0)
	{
		MoveTowards (position);
		
		if (CanSeeTarget ())
			return;
		
		timeout -= Time.deltaTime;
		yield;
	}
}

function RotateTowards (position : Vector3)
{
	SendMessage ("SetSpeed", 0.0);
	
	var direction = position - transform.position;
	direction.y = 0;
	if (direction.magnitude < 0.1)
		return;
	
	transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), rotationSpeed * Time.deltaTime);
	transform.eulerAngles = Vector3 (0, transform.eulerAngles.y, 0);
}

function MoveTowards (position : Vector3)
{
	var direction = position - transform.position;
	direction.y = 0;
	if (direction.magnitude < 0.5)
	{
		SendMessage ("SetSpeed", 0.0);
		return;
	}
	
	transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), rotationSpeed * Time.deltaTime);
	transform.eulerAngles = Vector3 (0, transform.eulerAngles.y, 0);
	
	var forward = transform.TransformDirection (Vector3.forward);
	var speedModifier = Vector3.Dot (forward, direction.normalized);
	speedModifier = Mathf.Clamp01 (speedModifier);
	
	direction = forward * speed * speedModifier;
	GetComponent (CharacterController).SimpleMove (direction);
	
	SendMessage ("SetSpeed", speed * speedModifier, SendMessageOptions.DontRequireReceiver);
}

function PickNextWaypoint (currentWaypoint : AutoWayPoint)
{
	var forward = transform.TransformDirection (Vector3.forward);
	
	var best = currentWaypoint;
	var bestDot = -10.0;
	
	for (var cur : AutoWayPoint in currentWaypoint.connected)
	{
		var direction = Vector3.Normalize (cur.transform.position - transform.position);
		var dot = Vector3.Dot (direction, forward);
		
		if (dot > bestDot && cur != currentWaypoint)
		{
			bestDot = dot;
			best = cur;
		}
	}
	
	return best;
}



