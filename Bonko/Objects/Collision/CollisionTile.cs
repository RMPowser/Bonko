using Logic;
using Logic.Components;

namespace Bonko.Objects.Collision
{
	public class CollisionTile : Entity
	{
		public CollisionTile(string name)
			: base(name)
		{
			AddComponent(new CollisionRectComponent(this, 0, 0, 16, 16));
			AddComponent(new TransformComponent(this));
			AddComponent(new SpriteComponent(this, "sprites/TileSets/GenericBlocks/collision/collision.json", "collision_tile_0"));
		}
	}
}
