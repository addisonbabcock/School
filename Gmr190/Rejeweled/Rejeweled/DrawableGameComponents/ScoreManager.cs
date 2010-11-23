using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Rejeweled
{
    class ScoreFloater
    {
        private Timer mFadeTimer;
        private SpriteFont mFont;
        private string mAmount;
        private Vector2 mPos;

        public ScoreFloater(TimeSpan fadeTime, SpriteFont font, int amount, Vector2 pos)
        {
            mFadeTimer = new Timer(fadeTime);
            mFont = font;
            mAmount = amount.ToString ();
            mPos = pos;

            Vector2 displaySize = mFont.MeasureString(mAmount);
            mPos.X -= displaySize.X / 2.0f;
            mPos.Y -= displaySize.Y / 2.0f;
        }

        public void Update(GameTime gameTime)
        {
            mFadeTimer.Update(gameTime.ElapsedGameTime);
        }

        public void Draw(SpriteBatch mSpriteBatch)
        {
            float fadeAlpha = (float)(1.0 - mFadeTimer.PercentComplete ());
            mSpriteBatch.DrawString(mFont, mAmount, mPos + ShadowOffset, new Color(Color.Red, fadeAlpha));
            mSpriteBatch.DrawString(mFont, mAmount, mPos, new Color(Color.White, fadeAlpha));
        }

        public bool Done
        {
            get { return mFadeTimer.PercentComplete() == 1.0; }
        }

        private Vector2 ShadowOffset
        {
            get { return new Vector2(1.0f, 1.0f); }
        }
    }

	class ScoreManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
        private SpriteFont mScoreTitleFont;
        private SpriteFont mScoreFont;
        private SpriteFont mScoreFloaterFont;

        private SpriteBatch mSpriteBatch;

        private int mScore;

        List<ScoreFloater> mFloaters;

		public ScoreManager(Game game)
			: base(game)
		{
			// TODO: Construct any child components here
            mScore = 0;
            mFloaters = new List<ScoreFloater>();
		}

		public override void Initialize()
		{
			// TODO: Add your initialization code here
            mSpriteBatch = new SpriteBatch(Game.GraphicsDevice);
			base.Initialize();
		}

        protected override void LoadContent()
        {
            base.LoadContent();

            mScoreTitleFont = Game.Content.Load<SpriteFont>("Fonts\\score_title");
            mScoreFont = Game.Content.Load<SpriteFont>("Fonts\\score");
            mScoreFloaterFont = Game.Content.Load<SpriteFont>("Fonts\\score_floater");
        }

		public override void Update(GameTime gameTime)
		{
			// TODO: Add your update code here

            foreach (ScoreFloater floater in mFloaters)
            {
                floater.Update(gameTime);
            }
            mFloaters.RemoveAll(i => i.Done);

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

            mSpriteBatch.Begin();
            
            mSpriteBatch.DrawString(mScoreTitleFont, "Score", TitlePos + ShadowOffset, Color.Black);
            mSpriteBatch.DrawString(mScoreTitleFont, "Score", TitlePos, Color.White);

            mSpriteBatch.DrawString(mScoreFont, mScore.ToString (), ScorePos + ShadowOffset, Color.Black);
            mSpriteBatch.DrawString(mScoreFont, mScore.ToString (), ScorePos, Color.White);

            foreach (ScoreFloater floater in mFloaters)
            {
                floater.Draw(mSpriteBatch);
            }

            mSpriteBatch.End();
		}

        public void CalculateScore(List<List<Gem>> validMoves)
        {
            //get everything sorted to remove duplicates easier
            foreach (List<Gem> move in validMoves)
            {
                move.Sort((first, second) => first.BoardLocation.Compare (second.BoardLocation));
            }
            validMoves.Sort((first, second) => first[0].BoardLocation.Compare(second[0].BoardLocation));

            //need to remove duplicates here too.
            //though im not sure we will actually get any!

            foreach (List<Gem> move in validMoves)
            {
                int add = 0;
                switch (move.Count)
                {
                    case 3:
                        add = 20;
                        break;

                    case 4:
                        add = 40;
                        break;

                    case 6:
                        add = 80;
                        break;

                    default:
                        add = 200;
                        break;
                }
                mScore += add;

                Vector2 pos = new Vector2 ();
                pos.X = (float)((move[0].OnScreenLocation.X + move[move.Count - 1].OnScreenLocation.X) / 2.0f);
                pos.Y = (float)((move[0].OnScreenLocation.Y + move[move.Count - 1].OnScreenLocation.Y) / 2.0f);
                pos.X += (float)GlobalVars.GemSizeX / 2.0f;
                pos.Y += (float)GlobalVars.GemSizeY / 2.0f;
                ScoreFloater floater = new ScoreFloater(
                    new TimeSpan(0, 0, 0, 0, 750),
                    mScoreFloaterFont, add, pos);
                mFloaters.Add (floater);
            }
        }

        private Vector2 TitlePos
        {
            get 
            {
                return new Vector2(845.0f, 20.0f);
            }
        }

        private Vector2 ScorePos
        {
            get
            {
                return new Vector2(845.0f, 80.0f);
            }
        }

        private Vector2 ShadowOffset
        {
            get
            {
                return new Vector2(2.0f, 2.0f);
            }
        }
	}
}