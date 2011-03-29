using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven.goals
{
    public class AttackTargetGoal_Evaluator : Goal_Evaluator
    {


        public AttackTargetGoal_Evaluator(float bias) : base(bias) { }

        public override float CalculateDesirability(AbstractBot pBot)
        {
            float Desirability = 0.0f;

            //only do the calculation if there is a target present
            if (pBot.GetTargetSys().isTargetPresent())
            {
                const float Tweaker = 1.0f;

                Desirability = Tweaker *
                               goals.Raven_Feature.Health(pBot) *
                               goals.Raven_Feature.TotalWeaponStrength(pBot);

                //bias the value according to the personality of the bot
                Desirability *= m_dCharacterBias;
            }

            return Desirability;
        }

        public override void SetGoal(AbstractBot pBot)
        {
            pBot.GetBrain().AddGoal_AttackTarget();
        }

        public override void RenderInfo(SpriteBatch batch, SpriteFont font, Color color, Vector2 Position, AbstractBot pBot)
        {
            batch.DrawString(font, string.Format("AT: {0:F2}",CalculateDesirability(pBot)), Position, color);
            string s = goals.Raven_Feature.Health(pBot) + ", " + goals.Raven_Feature.TotalWeaponStrength(pBot);
            batch.DrawString(font, s, Position + new Vector2(0, 12), color);
        }
    }
}
