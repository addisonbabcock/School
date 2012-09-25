using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven.goals
{
    public class ExploreGoal_Evaluator : Goal_Evaluator
    {
        

  public ExploreGoal_Evaluator(float bias):base(bias){}
  
  public override float CalculateDesirability(AbstractBot pBot)
  {
      float Desirability = 0.05f;

      Desirability *= m_dCharacterBias;

      return Desirability;
  }

  public override void SetGoal(AbstractBot pEnt)
  {
      pEnt.GetBrain().AddGoal_Explore();
  }
        public override void RenderInfo(SpriteBatch batch, SpriteFont font, Color color, Vector2 Position, AbstractBot pBot)
        {
            batch.DrawString(font, string.Format("EX: {0:F2}", CalculateDesirability(pBot)), Position, color);
        }
    }
}
