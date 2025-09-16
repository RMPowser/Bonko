using Logic.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ComponentSystems
{
	public class CollisionRectComponentSystem : BaseSystem<CollisionRectComponent>
	{
		public static List<CollisionRectComponent> GetAll()
		{
			return Components;
		}

		public static bool IsColliding(CollisionRectComponent l, CollisionRectComponent r)
		{
			return
				l.Right > r.Left &&
				l.Left < r.Right &&
				l.Bottom > r.Top &&
				l.Top < r.Bottom;
		}

		public static bool IsColliding(int x, int y, CollisionRectComponent rect)
		{
			return
				x > rect.Left &&
				x < rect.Right &&
				y > rect.Top &&
				y < rect.Bottom;
		}

		public static bool IsColliding(float x, float y, CollisionRectComponent rect)
		{
			return
				x > rect.Left &&
				x < rect.Right &&
				y > rect.Top &&
				y < rect.Bottom;
		}

		public static bool IsColliding(Vector2 point, CollisionRectComponent rect)
		{
			return
				point.X > rect.Left &&
				point.X < rect.Right &&
				point.Y > rect.Top &&
				point.Y < rect.Bottom;
		}

		public static bool IsColliding(int x, int y, string objName, out CollisionRectComponent? collider)
		{
			collider = null;

			foreach (CollisionRectComponent component in Components)
			{
				if (component.entity.Name == objName)
				{
					if (IsColliding(x, y, component))
					{
						collider = component;
						return true;
					}
				}
			}

			return false;
		}

		public static bool IsColliding(float x, float y, string objName, out CollisionRectComponent? collider)
		{
			collider = null;

			foreach (CollisionRectComponent component in Components)
			{
				if (component.entity.Name == objName)
				{
					if (IsColliding(x, y, component))
					{
						collider = component;
						return true;
					}
				}
			}

			return false;
		}

		public static bool IsColliding(Vector2 point, string objName, out CollisionRectComponent? collider)
		{
			collider = null;

			foreach (CollisionRectComponent component in Components)
			{
				if (component.entity.Name == objName)
				{
					if (IsColliding(point, component))
					{
						collider = component;
						return true;
					}
				}
			}

			return false;
		}

		public static bool IsColliding(Entity ent, string objName, out CollisionRectComponent? collider)
		{
			collider = null;

			var collisionRect = ent.GetComponent<CollisionRectComponent>();
			if (collisionRect == null)
			{
				return false;
			}

			foreach (CollisionRectComponent component in Components)
			{
				if (component != collisionRect)
				{
					if (component.entity.Name == objName)
					{
						if (IsColliding(collisionRect, component))
						{
							collider = component;
							return true;
						}
					}
				}
			}

			return false;
		}
	}
}
