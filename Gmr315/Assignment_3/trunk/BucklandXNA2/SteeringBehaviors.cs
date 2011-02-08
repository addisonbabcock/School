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
			throw new Exception ("Students must implement this...");
		}

		//head towards a moving target
		//seek can overshoot
		public static Vector2 Seek (Vehicle vehicle, Vector2 targetPosition)
		{
			Vector2 desiredVelocity = targetPosition - vehicle.CurrentPosition;

			if (desiredVelocity.LengthSquared () > (float)vehicle.MaxSpeed)
			{
				desiredVelocity.Normalize ();
				desiredVelocity = desiredVelocity * (float)vehicle.MaxSpeed;
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
			throw new Exception ("Students must implement this...");
		}

		//move towards a given vehicle
		public static Vector2 Pursuit (Vehicle vehicle, Vehicle evader)
		{
			throw new Exception ("Students must implement this...");
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
