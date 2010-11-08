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
using TInput = System.String;
using TOutput = System.String;

namespace TextureEffectsPipeline
{
	/// <summary>
	/// This class will be instantiated by the XNA Framework Content Pipeline
	/// to apply custom processing to content data, converting an object of
	/// type TInput to TOutput. The input and output types may be the same if
	/// the processor wishes to alter data without changing its type.
	///
	/// This should be part of a Content Pipeline Extension Library project.
	///
	/// TODO: change the ContentProcessor attribute to specify the correct
	/// display name for this processor.
	/// </summary>
	[ContentProcessor]
	public class AlphaMapBlender : TextureProcessor
	{
		public override TextureContent Process (TextureContent input, ContentProcessorContext context)
		{
			TextureContent alphaMap = input;

			string alphaMapFileName = alphaMap.Identity.SourceFilename;
			string colorMapFileName = 
				Path.GetDirectoryName (alphaMapFileName).Replace ("Alpha_Powerup", "Texture").Replace ("Alpha", "Texture") + 
				"\\" +
				Path.GetFileName (alphaMapFileName);

			TextureContent colorMap = context.BuildAndLoadAsset<TextureContent, TextureContent> (
				new ExternalReference<TextureContent> (colorMapFileName),
				null);

			PixelBitmapContent<Color> colorBitmap = (PixelBitmapContent<Color>)colorMap.Faces [0] [0];
			PixelBitmapContent<Color> alphaBitmap = (PixelBitmapContent<Color>)alphaMap.Faces [0] [0];

			for (int x = 0; x < colorBitmap.Width && x < alphaBitmap.Width; ++x)
			{
				for (int y = 0; y < alphaBitmap.Height && y < alphaBitmap.Height; ++y)
				{
					Color pixel = colorBitmap.GetPixel (x, y);
					Color alphaMapPixel = alphaBitmap.GetPixel (x, y);
					byte alpha = (byte)((alphaMapPixel.R + alphaMapPixel.G + alphaMapPixel.B) / 3);

					alphaBitmap.SetPixel (
						x, y,
						new Color (pixel, alpha));
				}
			}

			return base.Process (alphaMap, context);
		}
	}
}