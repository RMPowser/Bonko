using Logic;
using Logic.Components;

namespace Bonko.Objects.Collision
{
	public class CollisionSlope : Entity
	{
		public CollisionSlope(string name, string layerName)
			: base(name)
		{
			AddComponent(new CollisionRectComponent(this, 0, 0, 16, 16));
			AddComponent(new TransformComponent(this));
			AddComponent(new SpriteComponent(this, "sprites/TileSets/GenericBlocks/collision/collision.json", name + "_0"));
		}
	}
}
