using Microsoft.Xna.Framework;
using Graphics;
using Logic;

namespace Bonko.Objects.Collision
{
	public class CollisionTile : Entity
	{
		public CollisionTile(string name, string layerName)
			: base(name, layerName, new Rectangle(0, 0, 16, 16))
		{
			TextureAtlas = TextureAtlas.FromAsepriteJsonFile(ContentManager, "sprites/TileSets/GenericBlocks/collision/collision.json");
			Sprite = TextureAtlas.CreateAnimatedSprite(name);
		}
	}
}
