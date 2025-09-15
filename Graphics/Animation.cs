using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Graphics
{
	public class Animation
	{
		public string Name { get; private set; }
		public List<TextureRegion> Frames {  get; set; }

		public Animation(string name, List<TextureRegion> frames)
		{
			Name = name;
			Frames = frames ?? [];
		}
	}
}
