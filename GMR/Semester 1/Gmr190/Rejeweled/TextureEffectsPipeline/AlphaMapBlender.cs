using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

// TODO: replace these with the processor input and output types.
using TInput = Microsoft.Xna.Framework.Content.Pipeline.Graphics.Texture2DContent;
using TOutput = Microsoft.Xna.Framework.Content.Pipeline.Graphics.Texture2DContent;

namespace TextureEffectsPipeline
{
	/// <summary>
	/// Takes an alpha map, finds the corresponding texture and combines them.
	/// </summary>
	[ContentProcessor]
	public class AlphaMapBlender : TextureProcessor
	{
		private string mTextureDirectory = "Texture";

		public override bool ColorKeyEnabled
		{
			get
			{
				return false;// base.ColorKeyEnabled;
			}
			set
			{
				//base.ColorKeyEnabled = value;
			}
		}

		public string TextureDirectory
		{
			get { return mTextureDirectory; }
			set { mTextureDirectory = value; }
		}

		public override TextureContent Process (TextureContent input, ContentProcessorContext context)
		{
			TextureContent alphaMap = input;

			string alphaMapFileName = alphaMap.Identity.SourceFilename;

			//super hack!
			string colorMapFileName =
				Path.GetDirectoryName(alphaMapFileName).Replace("Alpha_Powerup", mTextureDirectory).Replace("Alpha", mTextureDirectory) + 
				"\\" +
				Path.GetFileName (alphaMapFileName);

			TextureContent colorMap = context.BuildAndLoadAsset<TextureContent, TextureContent> (
				new ExternalReference<TextureContent> (colorMapFileName),
				null);

			PixelBitmapContent<Color> colorBitmap = (PixelBitmapContent<Color>)colorMap.Faces [0] [0];
			PixelBitmapContent<Color> alphaBitmap = (PixelBitmapContent<Color>)alphaMap.Faces [0] [0];

			for (int x = 0; x < colorBitmap.Width && x < alphaBitmap.Width; ++x)
			{
				for (int y = 0; y < colorBitmap.Height && y < alphaBitmap.Height; ++y)
				{
					Color pixel = colorBitmap.GetPixel (x, y);
					Color alphaMapPixel = alphaBitmap.GetPixel (x, y);
					int alpha = (alphaMapPixel.R + alphaMapPixel.G + alphaMapPixel.B) / 3;

					alphaBitmap.SetPixel (
						x, y,
						new Color (pixel.R, pixel.G, pixel.B, alpha));
				}
			}

			return base.Process (alphaMap, context);
		}
	}
}