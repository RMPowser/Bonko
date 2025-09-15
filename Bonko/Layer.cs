using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Logic;
using Logic.Components;
using LDtk;

namespace Bonko
{
	public class Layer : Entity
	{
		protected List<Entity> Objects;

		public Layer(LDtk.LayerInstance layerInstance, Vector2 roomWorldPosition)
			: base(layerInstance._Identifier)
		{
			Objects = [];

			foreach (var obj in layerInstance.EntityInstances)
			{
				Entity? instance = null;
				switch (obj._Identifier)
				{
					case "player_spawn":
					{
						instance = new Objects.Player.PlayerObject(obj._Identifier);
						var transform = instance.GetComponent<TransformComponent>() ?? throw new Exception($"Missing component \"{nameof(TransformComponent)}\".");
						transform.Position = new Vector2((float)(obj._WorldX - roomWorldPosition.X)!, (float)(obj._WorldY - roomWorldPosition.Y)!);
						break;
					}
					case "collision_tile":
					{
						instance = new Objects.Collision.CollisionTile(obj._Identifier);
						var transform = instance.GetComponent<TransformComponent>() ?? throw new Exception($"Missing component \"{nameof(TransformComponent)}\".");
						transform.Position = new Vector2((float)(obj._WorldX - roomWorldPosition.X)!, (float)(obj._WorldY - roomWorldPosition.Y)!);
						transform.Scale = new Vector2(obj.Width / (obj._Tile?.W ?? 1), obj.Height / (obj._Tile?.H ?? 1));
						break;
					}
					case "collision_slope_left":
					case "collision_slope_right":
					{
						instance = new Objects.Collision.CollisionSlope(obj._Identifier);
						var transform = instance.GetComponent<TransformComponent>() ?? throw new Exception($"Missing component \"{nameof(TransformComponent)}\".");
						transform.Position = new Vector2((float)(obj._WorldX - roomWorldPosition.X)!, (float)(obj._WorldY - roomWorldPosition.Y)!);
						transform.Scale = new Vector2(obj.Width / (obj._Tile?.W ?? 1), obj.Height / (obj._Tile?.H ?? 1));
						break;
					}
					default:
					{
						break;
					}
				}

				if (instance != null)
				{
					Objects.Add(instance);
				}
			}
		}

		public virtual void AddObject(Entity obj)
		{
			Objects.Add(obj);
		}

		public override void Unload()
		{
			if (Objects.Count == 0)
			{
				return;
			}

			do
			{
				var obj = Objects[0];
				obj.Unload();
				Objects.Remove(obj);
			} while (Objects.Count > 0);

			Objects = [];

			base.Unload();
		}
	}
}