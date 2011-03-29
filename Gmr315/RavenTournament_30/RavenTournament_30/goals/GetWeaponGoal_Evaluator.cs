using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven.goals
{
    public class GetWeaponGoal_Evaluator : Goal_Evaluator
    {
        private int   m_iWeaponType;



        public GetWeaponGoal_Evaluator(float bias,
                          int   WeaponType):base(bias)
  {
      m_iWeaponType = WeaponType;
  }
                                            
  
        public override float CalculateDesirability(AbstractBot pBot)
        {
             //grab the distance to the closest instance of the weapon type
  float Distance = Raven_Feature.DistanceToItem(pBot, m_iWeaponType);

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
    const float Tweaker = 0.15f;

    float Health, WeaponStrength;

    Health = Raven_Feature.Health(pBot);

    WeaponStrength = Raven_Feature.IndividualWeaponStrength(pBot,
                                                             m_iWeaponType);
    
    float Desirability = (Tweaker * Health * (1-WeaponStrength)) / Distance;

    //ensure the value is in the range 0 to 1
    Utils.Clamp(ref Desirability, 0, 1);

    Desirability *= m_dCharacterBias;

    return Desirability;
  }
        }

        public override void SetGoal(AbstractBot pBot)
        {
           pBot.GetBrain().AddGoal_GetItem(m_iWeaponType);
        }

        public override void RenderInfo(SpriteBatch batch, SpriteFont font, Color color, Vector2 Position, AbstractBot pBot)
        {
            string s = "";              
  switch(m_iWeaponType)
  {
  case (int) Raven_Objects.type_rail_gun:
    s="RG: ";break;
  case (int)Raven_Objects.type_rocket_launcher:
    s="RL: "; break;
  case (int)Raven_Objects.type_shotgun:
    s="SG: "; break;
  }

  batch.DrawString(font, string.Format("{0}{1:F2}", s, CalculateDesirability(pBot)), Position, color);
        }
    }
}
