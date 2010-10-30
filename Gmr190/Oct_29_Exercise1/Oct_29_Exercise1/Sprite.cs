using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oct_29_Exercise1
{
    class Sprite
    {
        private Texture2D mTexture;
        private Vector2 mPos;
        private double mRot;
        private Vector2 mSpeed; //pixels per millisecond
        private double mSpinSpeed; //radians per millisecond
        private bool mWrapPosition;
        private GraphicsDeviceManager mGraphics;

        public Sprite(
            Texture2D texture, 
            Vector2 pos, 
            Vector2 speed, 
            double spinSpeed, 
            bool wrapPosition, 
            GraphicsDeviceManager graphics)
        {
            mTexture = texture;
            mPos = pos;
            mRot = 0.0;
            mSpeed = speed;
            mSpinSpeed = spinSpeed;
            mWrapPosition = wrapPosition;
            mGraphics = graphics;
        }

        public void Update(GameTime time)
        {
            mPos.X += mSpeed.X * (float)time.ElapsedGameTime.TotalMilliseconds;
            mPos.Y += mSpeed.Y * (float)time.ElapsedGameTime.TotalMilliseconds;
            mRot += mSpinSpeed * time.ElapsedGameTime.TotalMilliseconds;

            if (mWrapPosition)
            {
                while (mPos.X >= mGraphics.GraphicsDevice.Viewport.Width)
                    mPos.X -= mGraphics.GraphicsDevice.Viewport.Width;
                while (mPos.X < 0)
                    mPos.X += mGraphics.GraphicsDevice.Viewport.Width;
                while (mPos.Y >= mGraphics.GraphicsDevice.Viewport.Height)
                    mPos.Y -= mGraphics.GraphicsDevice.Viewport.Height;
                while (mPos.Y < 0)
                    mPos.Y += mGraphics.GraphicsDevice.Viewport.Height;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                mTexture, 
                mPos, 
                new Rectangle (0, 0, mTexture.Width, mTexture.Height), 
                Color.White, 
                (float)mRot, 
                new Vector2(
                    mTexture.Width / 2,
                    mTexture.Height / 2),
                1.0f,
                SpriteEffects.None, 
                0.0f);
        }
    }
}
