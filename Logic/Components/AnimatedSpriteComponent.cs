using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Graphics;
using System.Collections.Generic;
using Logic.Interfaces;
using Logic.ComponentSystems;

namespace Logic.Components
{
	public class AnimatedSpriteComponent : BaseComponent, IUpdateableComponent, IDrawableComponent
	{
		private TimeSpan ElapsedTime;
		public int CurrFrame { get; set; }
		public Dictionary<string, Animation> Animations { get; private set; }
		public Animation? CurrAnimation { get; private set; }
		public Color Color { get; set; }
		public SpriteEffects Effects { get; set; }


		public AnimatedSpriteComponent(
			Entity ent,
			int? currFrame = null,
			Dictionary<string, Animation>? animations = null, 
			Animation? currAnimation = null,
			Color? color = null,
			SpriteEffects? effects = null)
			: base(ent)
		{
			CurrFrame = currFrame ?? 0;
			Animations = animations ?? [];
			CurrAnimation = currAnimation;
			Color = color ?? Color.White;
			Effects = effects ?? SpriteEffects.None;

			UpdateableComponentSystem.Register(this);
			DrawableComponentSystem.Register(this);
		}

		public void ChangeAnimation(string animName)
		{
			CurrAnimation = Animations[animName];
		}

		public void Update(GameTime gameTime)
		{
			if (CurrAnimation != null)
			{
				ElapsedTime += gameTime.ElapsedGameTime;

				if (ElapsedTime >= CurrAnimation.MillisecondsBetweenFrames)
				{
					ElapsedTime -= CurrAnimation.MillisecondsBetweenFrames;
					CurrFrame++;

					if (CurrFrame >= CurrAnimation.Frames.Count)
					{
						CurrFrame = 0;
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			var transform = entity.GetComponent<TransformComponent>();

			CurrAnimation?.Frames[CurrFrame].Draw(
					spriteBatch,
					transform?.Position ?? Vector2.Zero,
					Color,
					transform?.Rotation ?? 0,
					transform?.Origin ?? Vector2.Zero,
					transform?.Scale ?? Vector2.One,
					Effects,
					transform?.Depth ?? 0);
		}

		public override void Unload()
		{
			UpdateableComponentSystem.Deregister(this);
			DrawableComponentSystem.Deregister(this);
			base.Unload();
		}
	}
}
