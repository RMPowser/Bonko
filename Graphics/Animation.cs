using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Graphics
{
	public class Animation
	{
		public List<TextureRegion> Frames {  get; set; }
		public TimeSpan MillisecondsBetweenFrames { get; set; }

		public Animation()
		{
			Frames = [];
			MillisecondsBetweenFrames = TimeSpan.FromMilliseconds(100);
		}

		public Animation(List<TextureRegion> frames, TimeSpan millisecondsBetweenFrames)
		{
			Frames = frames;
			MillisecondsBetweenFrames = millisecondsBetweenFrames;
		}
	}
}
