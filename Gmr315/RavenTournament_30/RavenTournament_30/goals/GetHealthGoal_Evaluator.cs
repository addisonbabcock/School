using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven.goals
{
    public class GetHealthGoal_Evaluator : Goal_Evaluator
    {
        public GetHealthGoal_Evaluator(float CharacterBias) : base(CharacterBias)
        {
        }

        public override float CalculateDesirability(AbstractBot pBot)
        {
            //first grab the distance to the closest instance of a health item
  float Distance = Raven_Feature.DistanceToItem(pBot, (int) Raven_Objects.type_health);

  //if the distance feature is rated with a value of 1 it means that the
  //item is either not present on the map or too far away to be worth 
  //considering, therefore the desirability is zero
  if (Distance == 1)
  {
    return 0;
  }
  else
  {
    //value used to tweak the desirability
    const float Tweaker = 0.2f;
  
    //the desirability of finding a health item is proportional to the amount
    //of health remaining and inversely proportional to the distance from the
    //nearest instance of a health item.
    float Desirability = Tweaker * (1-Raven_Feature.Health(pBot)) / 
                        (Raven_Feature.DistanceToItem(pBot, (int) Raven_Objects.type_health));
 
    //ensure the value is in the range 0 to 1
    Utils.Clamp(ref Desirability, 0, 1);
  
    //bias the value according to the personality of the bot
    Desirability *= m_dCharacterBias;

    return Desirability;
  }
        }

        public override void SetGoal(AbstractBot pBot)
        {
            pBot.GetBrain().AddGoal_GetItem((int)Raven_Objects.type_health); 
        }

        public override void RenderInfo(SpriteBatch batch, SpriteFont font, Color color, Vector2 Position, AbstractBot pBot)
        {
            batch.DrawString(font, string.Format("H: {0:F2}", CalculateDesirability(pBot)), Position, color);
            

            string s = string.Format("{0}, {1}", 1 - Raven_Feature.Health(pBot),
                                     Raven_Feature.DistanceToItem(pBot, (int) Raven_Objects.type_health));
            batch.DrawString(font, s, Position + new Vector2(0, 15), color);
  
        }
    }
}
