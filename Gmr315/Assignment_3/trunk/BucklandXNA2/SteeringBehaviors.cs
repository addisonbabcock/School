using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XELibrary;

namespace Assignment_3
{
	public class SteeringBehaviors
	{
		public enum Deceleration
		{
			fast = 3,
			normal = 2,
			slow = 1
		}

		//go straight there
		public static Vector2 Arrive (Vehicle vehicle, Vector2 targetPosition,
								  Deceleration deceleration)
		{
			Vector2 directionToTarget = targetPosition - vehicle.CurrentPosition;
			float distance = directionToTarget.Length ();

			if (distance > 8.0f)
			{
				float speed = distance / ((float)deceleration * 0.1f);

				speed = Math.Min (speed, vehicle.MaxSpeed);

				Vector2 desiredVelocity = directionToTarget * speed / distance;
				Vector2 steerForce = desiredVelocity - vehicle.CurrentVelocity;

				if (steerForce.Length () > Vehicle.MaxForce)
				{
					System.Diagnostics.Debug.WriteLine ("Looks like steering force will be truncated...");
				}

				return steerForce;
			}

			return new Vector2 ();
		}

		//head towards a moving target
		public static Vector2 Seek (Vehicle vehicle, Vector2 targetPosition)
		{
			Vector2 desiredVelocity = targetPosition - vehicle.CurrentPosition;

			if (desiredVelocity.LengthSquared () > (float)vehicle.MaxSpeed)
			{
				desiredVelocity.Normalize ();
				desiredVelocity = desiredVelocity * (float)vehicle.MaxSpeed;
			}
			else
			{
				desiredVelocity = new Vector2 ();
			}

			return desiredVelocity;
		}

		//move away from target
		public static Vector2 Flee (Vehicle vehicle, Vector2 targetPosition)
		{
			Vector2 desiredVelocity = vehicle.CurrentPosition - targetPosition;
			desiredVelocity.Normalize ();
			desiredVelocity = desiredVelocity * (float)vehicle.MaxSpeed;

			return desiredVelocity;
			
		}

		//move randomly
		//the fancy circle in front of you thing
		public static Vector2 Wander (Vehicle vehicle)
		{
			return vehicle.CurrentPosition;
		}

		//move towards a given vehicle
		public static Vector2 Pursuit (Vehicle vehicle, Vehicle evader)
		{
			float timeToTarget =
				(vehicle.CurrentPosition - evader.CurrentPosition).Length () / evader.MaxSpeed;
			Vector2 predictedPos = 
				evader.CurrentPosition + evader.CurrentVelocity * timeToTarget;

			return Seek (vehicle, predictedPos);
		}


	
		
		
		
		
		
		//Path following creates a steering force that moves a vehicle along a series of waypoints forming a path.
		//Sometimes paths have a start and end point, and other times they loop back around on themselves forming a never-ending, closed path. 
		public static Vector2 PathFollowing (Vehicle vehicle)
		{
			throw new Exception ("Students must implement this...");
		}

		public static Vector2 OffsetPursuit (Vehicle vehicle, Vehicle leader, Vector2 offset)
		{
			throw new Exception ("Students must implement this...");
		}

		public static Vector2 Separation (Vehicle vehicle, List<Vehicle> neighbors)
		{
			throw new Exception ("Students must implement this...");
		}

		public static Vector2 Alignment (Vehicle vehicle, List<Vehicle> neighbors)
		{
			throw new Exception ("Students must implement this...");
		}

		public static Vector2 Cohesion (Vehicle vehicle, List<Vehicle> neighbors)
		{
			throw new Exception ("Students must implement this...");
		}
	}
}
