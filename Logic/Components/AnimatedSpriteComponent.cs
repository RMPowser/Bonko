using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Graphics;
using System.Collections.Generic;
using Logic.Interfaces;
using Logic.ComponentSystems;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace Logic.Components
{
	public class AnimatedSpriteComponent : BaseComponent, IUpdateableComponent, IDrawableComponent
	{
		private TimeSpan ElapsedTime;
		public int image_index { get; set; }
		public Dictionary<string, Animation> Animations { get; private set; }
		public Animation CurrAnimation { get; private set; }
		public int sprite_index;
		public Color Color { get; set; }
		public SpriteEffects Effects { get; set; }
		private readonly ContentManager ContentManager;
		public float image_speed { get; set; }

		public AnimatedSpriteComponent(Entity ent, string textureAtlasJsonFilePath)
			: base(ent)
		{
			ContentManager = new(Core.Content.ServiceProvider, Core.Content.RootDirectory);

			TextureAtlas atlas = new(ContentManager, textureAtlasJsonFilePath);
			Animations = atlas.GetAllAnimations();
			image_index = 0;
			CurrAnimation = Animations.First().Value;
			Color = Color.White;
			Effects = SpriteEffects.None;
			image_speed = 1;

			UpdateableComponentSystem.Register(this);
			DrawableComponentSystem.Register(this);
		}

		public void ChangeAnimation(string animName)
		{
			CurrAnimation = Animations[animName];
			image_index = 0;
		}

		public void ChangeAnimation(int animIndex)
		{
			CurrAnimation = Animations.Values.ToList()[animIndex];
			image_index = 0;
		}

		public int GetAnimationIndex()
		{
			return Animations.Keys.ToList().IndexOf(CurrAnimation.Name);
		}

		public void Update(GameTime gameTime)
		{
			if (CurrAnimation != null)
			{
				ElapsedTime += gameTime.ElapsedGameTime * Math.Abs(image_speed);

				if (ElapsedTime >= CurrAnimation.Frames[image_index].Duration)
				{
					ElapsedTime -= CurrAnimation.Frames[image_index].Duration;

					bool backwards = image_speed < 0;
					image_index += backwards ? -1 : 1;

					if (backwards && image_index < 0)
					{
						image_index = CurrAnimation.Frames.Count - 1;
					}
					else if (!backwards && image_index >= CurrAnimation.Frames.Count)
					{
						image_index = 0;
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			var transform = entity.GetComponent<TransformComponent>();

			CurrAnimation?.Frames[image_index].Draw(
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
