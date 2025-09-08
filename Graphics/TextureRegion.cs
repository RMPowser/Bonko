using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graphics
{
	public class TextureRegion
	{
		public Texture2D Texture { get; set; }
		public Rectangle SourceRectangle { get; set; }
		public int Width => SourceRectangle.Width;
		public int Height => SourceRectangle.Height;


		public TextureRegion()
		{
		}

		public TextureRegion(Texture2D texture, int x, int y, int width, int height)
		{
			Texture = texture;
			SourceRectangle = new Rectangle(x, y, width, height);
		}

		public void Draw(
			SpriteBatch spriteBatch,
			Vector2? position = null,
			Color? color = null,
			float? rotation = null,
			Vector2? origin = null,
			Vector2? scale = null,
			SpriteEffects? effects = null,
			float? layerDepth = null)
		{
			spriteBatch.Draw(
				Texture,
				position ?? Vector2.Zero,
				SourceRectangle,
				color ?? Color.White,
				rotation ?? 0.0f,
				origin ?? Vector2.Zero,
				scale ?? Vector2.One,
				effects ?? SpriteEffects.None,
				layerDepth ?? 0.0f
			);
		}

	}
}
