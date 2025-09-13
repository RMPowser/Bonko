using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using LDtk;

namespace Bonko
{
	public class Level
	{
		public string Name { get; }
		private List<Room> Rooms;
		
		public Level(string name, LDtkLevel[] rooms)
		{
			Name = name;
			Rooms = [];

			foreach (var room in rooms)
			{
				Rooms.Add(new Room(room.Identifier, new Vector2(room.WorldX, room.WorldY), room.LayerInstances));
			}
		}

		public Room GetRoom(string name)
		{
			Room? r = Rooms.Find(x => x.Name == name);
			if (r != null)
			{
				return r;
			}

			throw new Exception($"Room not found with name \"{name}\".");
		}
	}
}
