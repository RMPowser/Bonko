using Logic.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace Logic.ComponentSystems
{
	public class DrawableComponentSystem : BaseSystem<IDrawableComponent>
	{
		public static void Draw(SpriteBatch spriteBatch)
		{
			foreach (var component in Components)
			{
				component.Draw(spriteBatch);
			}
		}
	}
}
