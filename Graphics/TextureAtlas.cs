using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.Json.Nodes;

namespace Graphics
{
	public class TextureAtlas
	{
		internal Dictionary<string, TextureRegion> Regions;
		internal Dictionary<string, Animation> Animations;
		public Texture2D Texture { get; set; }


		public TextureAtlas(ContentManager content, string filePath)
		{
			Regions = [];
			Animations = [];

			filePath = filePath.Replace('\\', '/');

			string contentStr = "./Content/";
			string fileExt = ".json";

			if (!filePath.StartsWith(contentStr))
			{
				filePath = contentStr + filePath;
			}

			if (!filePath.EndsWith(fileExt))
			{
				filePath += fileExt;
			}

			var file = JsonNode.Parse(File.ReadAllText(filePath))
				?? throw new Exception($"Unable to parse json file: \"{filePath}\"");

			string imagePath = file["meta"]["image"].GetValue<string>();
			while (Path.HasExtension(imagePath))
			{
				imagePath = imagePath[..imagePath.LastIndexOf('.')];
			}

			int begin = contentStr.Length;
			int end = filePath.LastIndexOf('/');
			string sub = filePath[begin..end];
			imagePath = sub + '/' + imagePath;

			Texture = content.Load<Texture2D>(imagePath);

			var frames = file["frames"].AsArray();
			foreach (var frame in frames)
			{
				AddRegion(
					frame["filename"].GetValue<string>(),
					frame["frame"]["x"].GetValue<int>(),
					frame["frame"]["y"].GetValue<int>(),
					frame["frame"]["w"].GetValue<int>(),
					frame["frame"]["h"].GetValue<int>()
				);
			}

			var animations = file["meta"]["frameTags"].AsArray();
			foreach (var anim in animations)
			{
				Animation spr = new();
				int from = anim["from"].GetValue<int>();
				int to = anim["to"].GetValue<int>();
				string name = anim["name"].GetValue<string>();
				for (int i = from; i <= to; i++)
				{
					string key = name + "_" + (i - from).ToString();
					spr.Frames.Add(GetRegion(key));
				}

				AddAnimation(name, spr);
			}
		}


		public void AddRegion(string name, int x, int y, int width, int height)
		{
			Regions.Add(name, new TextureRegion(Texture, x, y, width, height));
		}

		public TextureRegion GetRegion(string name)
		{
			return Regions[name];
		}

		public bool RemoveRegion(string name)
		{
			return Regions.Remove(name);
		}

		public Dictionary<string, TextureRegion>GetAllRegions()
		{
			return Regions;
		}

		public void AddAnimation(string name, Animation animatedSprite)
		{
			Animations.Add(name, animatedSprite);
		}

		public Animation GetAnimation(string name)
		{
			return Animations[name];
		}

		public bool RemoveAnimation(string name)
		{
			return Animations.Remove(name);
		}

		public Dictionary<string, Animation> GetAllAnimations()
		{
			return Animations;
		}

		public void Clear()
		{
			Regions.Clear();
			Animations.Clear();
		}
	}
}
