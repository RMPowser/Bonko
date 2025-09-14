using Microsoft.Xna.Framework;

namespace Logic.Components
{
	public class CollisionRectComponent : BaseComponent
	{
		private Rectangle _rect;
		public Rectangle Rect
		{
			get
			{
				var transform = entity.GetComponent<TransformComponent>();
				if (transform != null)
				{
					return new Rectangle(
						_rect.Location,
						new Point(
							(int)(_rect.Size.X * transform.Scale.X),
							(int)(_rect.Size.Y * transform.Scale.Y)
							)
						);
				}

				return _rect;
			}
			set
			{
				_rect = value;
			}
		}

		public CollisionRectComponent(Entity ent, int x, int y, int w, int h) 
			: base(ent)
		{
			Rect = new(x, y, w, h);
		}
	}
}
