using Logic.ComponentSystems;
using Logic.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;

namespace Logic.Components
{
	public class CollisionRectComponent : BaseComponent, IUpdateableComponent, IDrawableComponent
	{
		private bool IsVisible;
		private Texture2D Texture;
		private Rectangle _rect;
		public Rectangle Rect
		{
			get
			{
				var transform = entity.GetComponent<TransformComponent>();
				if (transform != null)
				{
					return new Rectangle(
						(_rect.Location.ToVector2() + transform.Position).ToPoint(),
						(_rect.Size.ToVector2() * transform.Scale).ToPoint()
						);
				}

				return _rect;
			}
			set
			{
				_rect = value;
			}
		}

		public CollisionRectComponent(Entity ent, int x, int y, int w, int h) 
			: base(ent)
		{
			IsVisible = false;
			Rect = new(x, y, w, h);
			
			Texture = new Texture2D(Core.GraphicsDevice, 1, 1);
			Texture.SetData([Color.DeepPink]);

			CollisionRectComponentSystem.Register(this);
			UpdateableComponentSystem.Register(this);
			DrawableComponentSystem.Register(this);
		}

		public override void Unload()
		{
			CollisionRectComponentSystem.Deregister(this);
			UpdateableComponentSystem.Deregister(this);
			DrawableComponentSystem.Deregister(this);
			base.Unload();
		}

		public void Update(GameTime gameTime)
		{
			if (InputInfo.WasKeyJustPressed(Keys.F1))
			{
				IsVisible = !IsVisible;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (IsVisible)
			{
				spriteBatch.Draw(Texture, Rect, null, Color.White * 0.6f, 0, Vector2.Zero, SpriteEffects.None, 1);
			}
		}
	}
}
