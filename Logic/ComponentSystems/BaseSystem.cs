using Logic.Interfaces;
using System.Collections.Generic;

namespace Logic.ComponentSystems
{
	public class BaseSystem<T> where T : IBaseComponent
	{
		private protected static List<T> Components = [];


		public static void Register(T component)
		{
			Components.Add(component);
		}

		public static void Deregister(T component)
		{
			Components.Remove(component);
		}
	}
}
