using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rejeweled
{
	interface IRenderable
	{
		public void Update(Microsoft.Xna.Framework.GameTime gameTime);
		public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch);
	}
}
