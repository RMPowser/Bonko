using Logic;
using Logic.Components;
using Logic.ComponentSystems;
using Logic.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Runtime.InteropServices.Marshalling;

namespace Bonko.Objects.Player
{
	public class PlayerObject : Entity, IUpdateableComponent
	{
		enum AnimIndex
		{
			idle,
			sit,
			bonk,
			crouch,
			jump,
			ledge_grab,
			ledge_climb,
			morph_intro,
			morph,
			spin,
			spin_fast,
			spin_attack,
			walk
		}
		
		Random random;
		
		private AnimatedSpriteComponent animation;
		private TransformComponent transform;

		bool idle;
		bool elevator;

		TimeSpan blinkTimer;

		public PlayerObject(string name) 
			: base(name)
		{
			random = new();
			idle = true;
			elevator = false;

			transform = new(this);
			transform.Origin = new Vector2(24, 48);
			AddComponent(transform);

			animation = new(this, "sprites/Player/Bonko");
			AddComponent(animation);

			UpdateableComponentSystem.Register(this);
		}

		public override void Unload()
		{
			UpdateableComponentSystem.Deregister(this);
			base.Unload();
		}

		public void Update(GameTime gameTime)
		{
			BeginStep(gameTime);
			Step(gameTime);
			UpdateSprite(gameTime);
		}

		private void BeginStep(GameTime gameTime)
		{
		}

		private void Step(GameTime gameTime)
		{
		}

		private void UpdateSprite(GameTime gameTime)
		{
			AnimIndex sprite_index = (AnimIndex)animation.sprite_index;
			if (!elevator)
			{
				if (idle)
				{
					if (sprite_index == AnimIndex.crouch)
					{
						if (animation.image_index > 0)
						{
							animation.image_speed = -1;
						}
						else
						{
							animation.ChangeAnimation((int)AnimIndex.idle);
							animation.image_index = 0;
						}
					}
					else if (sprite_index != AnimIndex.idle)
					{
						animation.ChangeAnimation((int)AnimIndex.idle);
						animation.image_index = 0;
					}
					else
					{
						blinkTimer -= gameTime.ElapsedGameTime;
						if (blinkTimer.TotalSeconds <= 0)
						{
							animation.image_speed = 1;
							animation.image_index = 1;

							blinkTimer += TimeSpan.FromSeconds((double)random.Next(0, 3) + random.NextDouble());
						}

						if (animation.image_index == 0)
						{
							animation.image_speed = 0;
						}
					}
				}
			}
		}
	}
}
