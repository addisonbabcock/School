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
	public class ScoreManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
        private SpriteFont mVerdanaBold;
        private SpriteFont mBauhaus93;

        private SpriteBatch mSpriteBatch;

		public ScoreManager(Game game)
			: base(game)
		{
			// TODO: Construct any child components here
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

            mVerdanaBold = Game.Content.Load<SpriteFont>("Fonts\\verdana_bold");
            mBauhaus93 = Game.Content.Load<SpriteFont>("Fonts\\Bauhaus_93");
        }

		public override void Update(GameTime gameTime)
		{
			// TODO: Add your update code here

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

            mSpriteBatch.Begin();
            
            mSpriteBatch.DrawString(mVerdanaBold, "Score", TitlePos + ShadowOffset, Color.Black);
            mSpriteBatch.DrawString(mVerdanaBold, "Score", TitlePos, Color.White);

            mSpriteBatch.DrawString(mBauhaus93, "123456", ScorePos + ShadowOffset, Color.Black);
            mSpriteBatch.DrawString(mBauhaus93, "123456", ScorePos, Color.White);

            mSpriteBatch.End();
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