using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven.goals
{
    public abstract class Goal_Evaluator : IDisposable
    {
        public void Dispose()
        {
        }

        //when the desirability score for a goal has been evaluated it is multiplied 
        //by this value. It can be used to create bots with preferences based upon
        //their personality
        protected float m_dCharacterBias;



        public Goal_Evaluator(float CharacterBias)
        {
            m_dCharacterBias = CharacterBias;
        }


        //returns a score between 0 and 1 representing the desirability of the
        //strategy the concrete subclass represents
        public abstract float CalculateDesirability(AbstractBot pBot);

        //adds the appropriate goal to the given bot's brain
        public abstract void SetGoal(AbstractBot pBot);

        //used to provide debugging/tweaking support
        public abstract void RenderInfo(SpriteBatch batch, SpriteFont font, Color color, Vector2 Position, AbstractBot pBot);
    }
}
