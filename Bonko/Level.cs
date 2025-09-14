using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using LDtk;
using Logic;

namespace Bonko
{
	public class Level : Entity
	{
		private List<Room> Rooms;
		
		public Level(string name, LDtkLevel[] rooms)
			: base(name)
		{
			Rooms = [];

			foreach (var room in rooms)
			{
				Rooms.Add(new Room(room.Identifier, new Vector2(room.WorldX, room.WorldY), room.LayerInstances));
			}
		}

		public Room GetRoom(string name)
		{
			return Rooms.Find(x => x.Name == name) 
				?? throw new Exception($"Room not found with name \"{name}\".");
		}

		public override void Unload()
		{
			foreach (var room in Rooms)
			{
				room.Unload();
			}

			Rooms = [];
			base.Unload();
		}
	}
}
