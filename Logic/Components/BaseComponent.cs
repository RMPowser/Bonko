using Logic.Interfaces;

namespace Logic.Components
{
	public class BaseComponent : IBaseComponent
	{
		public Entity entity;

		public BaseComponent(Entity ent)
		{
			entity = ent;
		}

		public virtual void Unload()
		{
			// do nothing by default
		}
	}
}
