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
				l.Rect.Right > r.Rect.Left &&
				l.Rect.Left < r.Rect.Right &&
				l.Rect.Bottom > r.Rect.Top &&
				l.Rect.Top < r.Rect.Bottom;
		}

		public static bool IsColliding(Vector2 point, CollisionRectComponent rect)
		{
			return
				point.X > rect.Rect.Left &&
				point.X < rect.Rect.Right &&
				point.Y > rect.Rect.Top &&
				point.Y < rect.Rect.Bottom;
		}

		public static bool IsColliding(Vector2 point, string objName)
		{
			foreach (CollisionRectComponent component in Components)
			{
				if (component.entity.Name == objName)
				{
					if (IsColliding(point, component))
					{
						return true;
					}
				}
			}

			return false;
		}

		public static bool IsColliding(Entity ent, string objName)
		{
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
							return true;
						}
					}
				}
			}

			return false;
		}
	}
}
