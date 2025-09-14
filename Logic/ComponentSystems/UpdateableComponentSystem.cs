using Logic.Interfaces;
using Microsoft.Xna.Framework;

namespace Logic.ComponentSystems
{
	public class UpdateableComponentSystem : BaseSystem<IUpdateableComponent>
	{
		public static void Update(GameTime gameTime)
		{
			foreach (var component in Components)
			{
				component.Update(gameTime);
			}
		}
	}
}