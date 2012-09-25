using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Goals;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven.goals
{
	public class Goal_Think : Goal_Composite, IDisposable
	{

		private AbstractBot owner;
		private
		  List<Goal_Evaluator> m_Evaluators;



		public Goal_Think (AbstractBot pBot)
			: base (pBot, (int)Raven_Goal_Types.goal_think)
		{
			owner = pBot;
			m_Evaluators = new List<Goal_Evaluator> ();
			//these biases could be loaded in from a script on a per bot basis
			//but for now we'll just give them some random values
			const float LowRangeOfBias = 0.5f;
			const float HighRangeOfBias = 1.5f;

			float HealthBias = Utils.RandInRange (LowRangeOfBias, HighRangeOfBias);
			float ShotgunBias = Utils.RandInRange (LowRangeOfBias, HighRangeOfBias);
			float RocketLauncherBias = Utils.RandInRange (LowRangeOfBias, HighRangeOfBias);
			float RailgunBias = Utils.RandInRange (LowRangeOfBias, HighRangeOfBias);
			float ExploreBias = Utils.RandInRange (LowRangeOfBias, HighRangeOfBias);
			float AttackBias = Utils.RandInRange (LowRangeOfBias, HighRangeOfBias);

			//create the evaluator objects
			m_Evaluators.Add (new GetHealthGoal_Evaluator (HealthBias));
			m_Evaluators.Add (new ExploreGoal_Evaluator (ExploreBias));
			m_Evaluators.Add (new AttackTargetGoal_Evaluator (AttackBias));
			m_Evaluators.Add (new GetWeaponGoal_Evaluator (ShotgunBias,
															   (int)Raven_Objects.type_shotgun));
			m_Evaluators.Add (new GetWeaponGoal_Evaluator (RailgunBias,
															   (int)Raven_Objects.type_rail_gun));
			m_Evaluators.Add (new GetWeaponGoal_Evaluator (RocketLauncherBias,
															   (int)Raven_Objects.type_rocket_launcher));
		}
		public new void Dispose ()
		{
			foreach (Goal_Evaluator curDes in m_Evaluators)
			{
				curDes.Dispose ();
			}
		}

		//this method iterates through each goal evaluator and selects the one
		//that has the highest score as the current goal
		public void Arbitrate ()
		{
			float best = 0;
			Goal_Evaluator MostDesirable = null;

			//iterate through all the evaluators to see which produces the highest score
			foreach (Goal_Evaluator curDes in m_Evaluators)
			{



				float desirabilty = curDes.CalculateDesirability (owner);

				if (desirabilty >= best)
				{
					best = desirabilty;
					MostDesirable = curDes;
				}
			}

			if (MostDesirable == null)
			{
				throw new Exception ("No Evaluator Selected");
			}

			MostDesirable.SetGoal (owner);
		}

		//returns true if the given goal is not at the front of the subgoal list
		public bool notPresent (int GoalType)
		{
			if (m_SubGoals.Count != 0)
			{
				return m_SubGoals [0].GetGoalType () != GoalType;
			}

			return true;
		}

		//the usual suspects
		public override int Process ()
		{
			ActivateIfInactive ();

			int SubgoalStatus = ProcessSubgoals ();

			if (SubgoalStatus == (int)GoalStatus.completed || SubgoalStatus == (int)GoalStatus.failed)
			{
				if (!owner.isPossessed ())
				{
					m_iStatus = (int)GoalStatus.inactive;
				}
			}

			return m_iStatus;
		}
		public override void Activate ()
		{
			if (!owner.isPossessed ())
			{
				Arbitrate ();
			}

			m_iStatus = (int)GoalStatus.active;
		}

		public override void Terminate () { }

		//top level goal types
		public void AddGoal_MoveToPosition (Vector2 pos)
		{
			AddSubgoal (new Goal_MoveToPosition (owner, pos));
		}
		public void AddGoal_GetItem (int ItemType)
		{
			if (notPresent (Goal_GetItem.ItemTypeToGoalType (ItemType)))
			{
				RemoveAllSubgoals ();
				AddSubgoal (new Goal_GetItem (owner, ItemType));
			}
		}
		public void AddGoal_Explore ()
		{
			if (notPresent ((int)Raven_Goal_Types.goal_explore))
			{
				RemoveAllSubgoals ();
				AddSubgoal (new Goal_Explore (owner));
			}
		}
		public void AddGoal_AttackTarget ()
		{
			if (notPresent ((int)Raven_Goal_Types.goal_attack_target))
			{
				RemoveAllSubgoals ();
				AddSubgoal (new Goal_AttackTarget (owner));
			}
		}

		//this adds the MoveToPosition goal to the *back* of the subgoal list.
		public void QueueGoal_MoveToPosition (Vector2 pos)
		{
			m_SubGoals.Add (new Goal_MoveToPosition (owner, pos));
		}

		//this renders the evaluations (goal scores) at the specified location
		public void RenderEvaluations (SpriteBatch batch, SpriteFont font, int left, int top)
		{
			foreach (Goal_Evaluator curDes in m_Evaluators)
			{
				curDes.RenderInfo (batch, font, Color.Black, new Vector2 (left, top), owner);
				left += 75;
			}

		}
		public override void Render (PrimitiveBatch batch)
		{
			foreach (Goal curG in m_SubGoals)
			{
				curG.Render (batch);
			}
		}
	}
}
