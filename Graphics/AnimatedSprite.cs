using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Graphics
{
	public class AnimatedSprite : Sprite
	{
		private int _currFrame;
		private TimeSpan _elapsedTime;
		private Animation _animation;
		public Animation Animation
		{
			get => _animation;
			set
			{
				_animation = value;
				Region = _animation.Frames[0];
			}
		}


		public AnimatedSprite()
		{
		}

		public AnimatedSprite(Animation animation)
		{
			Animation = animation;
		}

		public void Update(GameTime gameTime)
		{
			_elapsedTime += gameTime.ElapsedGameTime;

			if (_elapsedTime >= _animation.MillisecondsBetweenFrames)
			{
				_elapsedTime -= _animation.MillisecondsBetweenFrames;
				_currFrame++;

				if (_currFrame >= _animation.Frames.Count)
				{
					_currFrame = 0;
				}

				Region = _animation.Frames[_currFrame];
			}
		}
	}
}
