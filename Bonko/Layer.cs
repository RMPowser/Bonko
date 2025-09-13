using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Logic;

namespace Bonko
{
	public class Layer
	{
		public string Name { get; }
		protected List<Entity> Objects;

		public Layer(string name, Vector2 roomWorldPosition, EntityInstance[] entities)
		{
			Name = name;
			Objects = [];

			foreach (var obj in entities)
			{
				if (obj._Identifier == "collision_tile")
				{
					var collisionTile = new Objects.Collision.CollisionTile(obj._Identifier, this.Name);
					collisionTile.Position = new Vector2((float)(obj._WorldX - roomWorldPosition.X), (float)(obj._WorldY - roomWorldPosition.Y));
					collisionTile.Scale = new Vector2(obj.Width / obj._Tile.W, obj.Height / obj._Tile.H);
					Objects.Add(collisionTile);
				}
			}
		}

		public virtual void AddObject(Entity obj)
		{
			Objects.Add(obj);
		}

		public virtual void Update(GameTime gameTime)
		{
			foreach (var obj in Objects)
			{
				obj.Update(gameTime);
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			foreach (var obj in Objects)
			{
				obj.Draw(spriteBatch);
			}
		}
	}
}