using Microsoft.Xna.Framework;

namespace Logic.Interfaces
{
	public interface IUpdateableComponent : IBaseComponent
	{
		public void Update(GameTime gameTime);
	}
}
