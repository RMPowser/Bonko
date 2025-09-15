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
		
		public Level(LDtkWorld level)
			: base(level.Identifier)
		{
			Rooms = [];

			foreach (var room in level.Levels)
			{
				Rooms.Add(new Room(room));
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
