using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Logic;

namespace Bonko
{
	public class Room
	{
		public string Name { get; }
		protected List<Layer> Layers;
		private readonly LayerInstance[]? LayersOnDisk;
		public Vector2 WorldPosition { get; }
		public bool IsLoaded { get; private set; }

		public Room(string name, Vector2 worldPosition, LayerInstance[]? layers = null)
		{
			Name = name;
			Layers = [];
			LayersOnDisk = layers;
			WorldPosition = worldPosition;
			IsLoaded = false;
		}

		public virtual void Load()
		{
			if (LayersOnDisk != null)
			{
				foreach (var layer in LayersOnDisk)
				{
					Layers.Add(new Layer(layer._Identifier, WorldPosition, 0, layer.EntityInstances));
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

		public virtual void Unload()
		{
			foreach (var layer in Layers)
			{
				layer.Unload();
			}

			Layers = [];
			IsLoaded = false;
		}
	}
}
