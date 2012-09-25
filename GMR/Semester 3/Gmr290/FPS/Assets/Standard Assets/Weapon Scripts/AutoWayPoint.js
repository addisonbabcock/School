
static var waypoints = Array ();
var connected = Array ();
static var kLineOfSightCapsuleRadius = 0.25;

static function FindClosest (pos : Vector3) : AutoWayPoint
{
	var closest : AutoWayPoint;
	var closestDistance = 100000.0;
	for (var cur : AutoWayPoint in waypoints)
	{
		var distance = Vector3.Distance (cur.transform.position, pos);
		if (distance < closestDistance)
		{
			closestDistance = distance;
			closest = cur;
		}
	}
	
	return closest;
}

@ContextMenu ("Update Waypoints")
function UpdateWaypoints ()
{
	RebuildWaypointList ();
}

function Awake ()
{
	RebuildWaypointList ();
}

function OnDrawGizmos ()
{
	Gizmos.DrawIcon (transform.position, "Waypoint.tif");
}

function OnDrawGizmosSelected ()
{
	if (waypoints.length == 0)
	{
		RebuildWaypointList ();
	}
	
	for (var p : AutoWayPoint in connected)
	{
		if (Physics.Linecast (transform.position, p.transform.position))
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine (transform.position, p.transform.position);
		}
		else
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine (transform.position, p.transform.position);
		}
	}
}

function RebuildWaypointList ()
{
	var objects : Object [] = FindObjectsOfType (AutoWayPoint);
	waypoints = Array (objects);
	
	for (var point : AutoWayPoint in waypoints)
	{
		point.RecalculateConnectedWaypoints ();
	}
}

function RecalculateConnectedWaypoints ()
{
	connected = Array ();
	
	for (var other : AutoWayPoint in waypoints)
	{
		if (other == this)
			continue;
		
		if (!Physics.CheckCapsule (transform.position, other.transform.position, kLineOfSightCapsuleRadius))
		{
			connected.Add (other);
		}
	}
}



