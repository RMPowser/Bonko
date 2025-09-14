using LDtk;
using System;
using System.Collections.Generic;
using Logic;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Bonko
{
	public class World
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
		internal static World instance;
#pragma warning restore CS8618 
		public static World Instance => instance;

		private LDtkFile WorldFile;
		private List<Level> Levels;
		private ContentManager ContentManager;
		private const string WorldFileName = "levels/BonkoWorld";


		public World() 
		{
			instance = this;

			ContentManager = new(Core.Content.ServiceProvider, Core.Content.RootDirectory);

			WorldFile = ContentManager.Load<LDtkFile>(WorldFileName);

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

		public void Reload()
		{
			foreach (var level in Levels)
			{
				level.Unload();
			}
			Levels = [];
			ContentManager.Unload();
			File.Copy("../../../Content/bin/DesktopGL/levels/BonkoWorld.xnb", "./Content/levels/BonkoWorld.xnb", true);
			WorldFile = ContentManager.Load<LDtkFile>(WorldFileName);

			foreach (var level in WorldFile.Worlds)
			{
				Levels.Add(new Level(level.Identifier, level.Levels));
			}
		}
	}
}
