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

		public Animation(List<TextureRegion>? frames = null, TimeSpan? millisecondsBetweenFrames = null)
		{
			Frames = frames ?? [];
			MillisecondsBetweenFrames = millisecondsBetweenFrames ?? TimeSpan.FromMilliseconds(100);
		}
	}
}
