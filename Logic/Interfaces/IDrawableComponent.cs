using Microsoft.Xna.Framework.Graphics;

namespace Logic.Interfaces
{
	public interface IDrawableComponent : IBaseComponent
	{
		public void Draw(SpriteBatch spriteBatch);
	}
}
