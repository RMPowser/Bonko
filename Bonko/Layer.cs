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

		public Layer(string name, Vector2 roomWorldPosition, float layerDepth, EntityInstance[] entities)
			: base(name)
		{
			AddComponent(new TransformComponent(this, default, default, layerDepth));
			Objects = [];

			foreach (var obj in entities)
			{
				switch (obj._Identifier)
				{
					case "collision_tile":
					{
						Entity instance = new Objects.Collision.CollisionTile(obj._Identifier, name);
						var transform = instance.GetComponent<TransformComponent>() ?? throw new Exception($"Missing component \"{nameof(TransformComponent)}\".");
						transform.Position = new Vector2((float)(obj._WorldX - roomWorldPosition.X)!, (float)(obj._WorldY - roomWorldPosition.Y)!);
						transform.Scale = new Vector2(obj.Width / obj._Tile!.W, obj.Height / obj._Tile.H);
						Objects.Add(instance);
						break;
					}
					case "collision_slope_left":
					case "collision_slope_right":
					{
						Entity instance = new Objects.Collision.CollisionSlope(obj._Identifier, name);
						var transform = instance.GetComponent<TransformComponent>() ?? throw new Exception($"Missing component \"{nameof(TransformComponent)}\".");
						transform.Position = new Vector2((float)(obj._WorldX - roomWorldPosition.X)!, (float)(obj._WorldY - roomWorldPosition.Y)!);
						transform.Scale = new Vector2(obj.Width / obj._Tile!.W, obj.Height / obj._Tile.H);
						Objects.Add(instance);
						break;
					}
					case "player_spawn":
					{

						break;
					}
					default:
					{
						break;
					}
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
		}
	}
}