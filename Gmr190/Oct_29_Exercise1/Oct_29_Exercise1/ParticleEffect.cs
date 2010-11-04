using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Oct_29_Exercise1
{
	class ParticleEffect
	{
		private class Particle
		{
			Vector2 mPos;
			Vector2 mDirection;
			TimeSpan mLife;

			public bool Update (GameTime time, Vector2 gravity)
			{
				mDirection.X += gravity.X * (float)time.ElapsedGameTime.TotalSeconds;
				mDirection.Y += gravity.Y * (float)time.ElapsedGameTime.TotalSeconds;
				mPos += mDirection;

				mLife -= time.ElapsedGameTime;
				if (mLife.TotalMilliseconds <= 0)
				{
					return true;
				}

				return false;
			}

            public void Draw(SpriteBatch spriteBatch, Texture2D tex)
            {
                spriteBatch.Draw(
                    tex,
                    new Rectangle(
                        (int)mPos.X,
                        (int)mPos.Y,
                        3,
                        3),
                    Color.White);
            }

            public Particle (Vector2 pos, Vector2 direction, TimeSpan life)
			{
				mPos = pos;
				mDirection = direction;
				mLife = life;
			}
		}

		List<Particle> mParticles;
		Vector2 mGravity;
		Texture2D mTex;

		public void Update (GameTime time)
		{
			for (int i = 0; i < mParticles.Count; ++i)
			{
				Particle particle = mParticles [i];
				if (particle.Update (time, mGravity))
				{
					mParticles.Remove (particle);
				}
			}
		}

		public void AddParticles (int particleCount)
		{
			Random rand = new Random ();
			for (int i = 0; i < particleCount; ++i)
			{
				mParticles.Add (
					new Particle (
						new Vector2 (400.0f, 50.0f),
						new Vector2 (
							(float)(rand.NextDouble () * 3.0) - 1.5f,
							(float)(rand.NextDouble () * 3.0) - 1.5f),
						new TimeSpan (0, 0, 0, 3, 0)));
			}
		}

		public ParticleEffect (Vector2 gravity, Texture2D tex)
		{
			mParticles = new List<Particle> ();
			mGravity = gravity;
			mTex = tex;
		}

		public void Draw (SpriteBatch spriteBatch)
		{
			foreach (Particle particle in mParticles)
			{
				particle.Draw (spriteBatch, mTex);
			}
		}
	}
}
