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
					Layers.Add(new Layer(layer._Identifier, WorldPosition, layer.EntityInstances));
				}
			}

			IsLoaded = true;
		}

		public virtual void Unload()
		{
			Layers = [];
			IsLoaded = false;
		}

		public virtual void AddObject(Entity obj)
		{
			Layer? layer = Layers.Find(x => x.Name == obj.LayerName);

			if (layer == null)
				throw new Exception($"No layer found with name \"{obj.LayerName}\".");

			layer?.AddObject(obj);
		}

		public virtual void Update(GameTime gameTime)
		{
			if (IsLoaded)
			{
				foreach (var layer in Layers)
				{
					layer.Update(gameTime);
				}
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch) 
		{
			if (IsLoaded)
			{
				foreach (var layer in Layers)
				{
					layer.Draw(spriteBatch);
				}
			}
		}
	}
}
