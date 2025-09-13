using LDtk;
using System;
using System.Collections.Generic;
using Logic;

namespace Bonko
{
	public class World
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
		internal static World instance;
#pragma warning restore CS8618 

		public static World Instance => instance;
		private LDtkFile WorldFile { get; }

		
		private List<Level> Levels;


		public World() 
		{
			instance = this;

			WorldFile = Core.Content.Load<LDtkFile>("levels/BonkoWorld");

			Levels = [];

			foreach (var level in WorldFile.Worlds)
			{
				Levels.Add(new Level(level.Identifier, level.Levels));
			}
		}

		public Room GetRoom(string name)
		{
			// name has an expected format. "{LevelName}_{RoomIndex}" ex: MainDeck_0
			var roomNameComponents = name.Split('_');
			string levelName = roomNameComponents[0];

			Level? level = Levels.Find(x => x.Name == levelName);
			if (level != null)
			{
				return level.GetRoom(name);
			}

			throw new Exception($"Level not found with name \"{levelName}\".");
		}
	}
}
