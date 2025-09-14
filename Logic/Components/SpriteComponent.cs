using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Graphics;
using Logic.Interfaces;
using Logic.ComponentSystems;

namespace Logic.Components
{
	public class SpriteComponent : BaseComponent, IDrawableComponent
	{
		public TextureRegion Region { get; set; }
		public Color Color { get; set; }
		public SpriteEffects Effects { get; set; }
		private readonly ContentManager ContentManager;

		public SpriteComponent(Entity ent, string textureAtlasJsonFilePath, string regionName, Color? color = null, SpriteEffects? effects = null) 
			: base(ent)
		{
			ContentManager = new(Core.Content.ServiceProvider, Core.Content.RootDirectory);
			var atlas = new TextureAtlas(ContentManager, textureAtlasJsonFilePath);
			Region = atlas.GetRegion(regionName);
			Color = color ?? Color.White;
			Effects	= effects ?? SpriteEffects.None;

			DrawableComponentSystem.Register(this);
		}
		
		public void Draw(SpriteBatch spriteBatch)
		{
			var transform = entity.GetComponent<TransformComponent>();

			Region.Draw(
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
			ContentManager.Unload();
			DrawableComponentSystem.Deregister(this);
			base.Unload();
		}
	}
}
