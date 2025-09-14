using Logic.Components;
using System.Collections.Generic;

namespace Logic
{
	public class Entity
	{
		public string Name { get; set; }
		private List<BaseComponent> Components;

		public Entity(string name) 
		{
			Name = name;
			Components = [];
		}

		public virtual void AddComponent(BaseComponent component)
		{
			Components.Add(component);
		}

		public virtual void RemoveComponent(BaseComponent component)
		{
			Components.Remove(component);
		}

		public T? GetComponent<T>() where T : BaseComponent
		{
			foreach (BaseComponent component in Components)
			{
				if (component.GetType() == typeof(T))
				{
					return (T)component;
				}
			}

			return null;
		}

		public virtual void Unload()
		{
			foreach (var comp in Components)
			{
				comp.Unload();
			}

			Components = [];
		}
	}
}
