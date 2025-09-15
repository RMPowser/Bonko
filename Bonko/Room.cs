using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Logic;

namespace Bonko
{
	public class Room : Entity
	{
		protected List<Layer> Layers;
		private readonly LDtkLevel ldtkLevel;
		public Vector2 WorldPosition { get; private set; }
		public Vector2 Size { get; private set; }
		public bool IsLoaded { get; private set; }

		public Room(LDtkLevel room)
			: base(room.Identifier)
		{
			Layers = [];
			ldtkLevel = room;
			WorldPosition = new(ldtkLevel.WorldX, ldtkLevel.WorldY);
			IsLoaded = false;
		}

		public virtual void Load()
		{
			if (ldtkLevel != null)
			{
				foreach (var layer in ldtkLevel.LayerInstances!)
				{
					Layers.Add(new Layer(layer, WorldPosition));
				}
			}

			IsLoaded = true;
		}

		public virtual void AddObject(Entity obj, string layerName)
		{
			Layer layer = Layers.Find(x => x.Name == layerName) 
				?? throw new Exception($"No layer found with name \"{layerName}\".");

			layer.AddObject(obj);
		}

		public override void Unload()
		{
			foreach (var layer in Layers)
			{
				layer.Unload();
			}

			Layers = [];
			IsLoaded = false;

			base.Unload();
		}
	}
}
