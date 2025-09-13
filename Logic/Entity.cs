using Microsoft.Xna.Framework;
using Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Logic
{
	public class Entity
	{
		public string Name { get; }
		public string LayerName { get; set; }
		public Rectangle? CollisionRect { get; set; }
		bool IsVisible { get; set; }
		public Vector2 Position
		{
			get
			{
				return Sprite.Position;
			}
			set
			{
				Sprite.Position = value;
			}
		}
		public Vector2 Scale
		{
			get
			{
				return Sprite.Scale;
			}
			set
			{
				Sprite.Scale = value;
			}
		}
		public float Rotation
		{
			get
			{
				return Sprite.Rotation;
			}
			set
			{
				Sprite.Rotation = value;
			}
		}

		protected AnimatedSprite? Sprite;
		protected TextureAtlas? TextureAtlas;

		protected ContentManager ContentManager;

		public Entity(string name, string layerName, Rectangle? collisionRect = null) 
		{
			Name = name;
			LayerName = layerName;
			CollisionRect = collisionRect;
			IsVisible = true;
			ContentManager = new(Core.Content.ServiceProvider, Core.Content.RootDirectory);
		}

		public virtual void Update(GameTime gameTime)
		{
			// do nothing by default
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (IsVisible)
			{
				Sprite?.Draw(spriteBatch);
			}
		}
	}
}
