using Microsoft.Xna.Framework;
using Graphics;

namespace Logic.Components
{
	public class TransformComponent : BaseComponent
	{
		private Vector2 _position;

		public Vector2 Position
		{ 
			get 
			{ 
				return _position - Origin; 
			} 
			set => _position = value; 
		}

		public Vector2 Scale 
		{ 
			get;
			set;
		}

		public float Depth 
		{ 
			get;
			set;
		}

		public float Rotation 
		{ 
			get;
			set;
		}

		public Vector2 Origin 
		{ 
			get;
			set;
		}

		public TransformComponent(
			Entity ent, 
			Vector2? pos = null, 
			Vector2? scale = null, 
			float? depth = null, 
			float? rot = null, 
			Vector2? origin = null) 
			: base(ent)
		{
			Position = pos ?? Vector2.Zero;
			Scale = scale ?? Vector2.One;
			Depth = depth ?? 0;
			Rotation = rot ?? 0;
			Origin = origin ?? Vector2.Zero;
		}

		/// <summary>
		/// requires a SpriteComponent attached to the same entity
		/// </summary>
		public void CenterOrigin()
		{
			var spr = entity.GetComponent<SpriteComponent>();
			if (spr != null)
			{
				Origin = new Vector2(spr.Region.Width, spr.Region.Height) * 0.5f;
			}
		}
	}
}
