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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
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

			var regions = file["frames"].AsArray();
			foreach (var region in regions)
			{
				AddRegion(
					region["filename"].GetValue<string>(),
					region["frame"]["x"].GetValue<int>(),
					region["frame"]["y"].GetValue<int>(),
					region["frame"]["w"].GetValue<int>(),
					region["frame"]["h"].GetValue<int>(),
					region["duration"].GetValue<int>()
				);
			}

			var animations = file["meta"]["frameTags"].AsArray();
			foreach (var anim in animations)
			{
				List<TextureRegion> frames = [];
				int from = anim["from"].GetValue<int>();
				int to = anim["to"].GetValue<int>();
				string name = anim["name"].GetValue<string>();
				for (int i = from; i <= to; i++)
				{
					string key = name + "_" + (i - from).ToString();
					frames.Add(GetRegion(key));
				}
				AddAnimation(new(name, frames));
			}
#pragma warning restore CS8602
		}


		public void AddRegion(string name, int x, int y, int width, int height, int durationInMilliseconds)
		{
			Regions.Add(name, new TextureRegion(Texture, x, y, width, height, durationInMilliseconds));
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

		public void AddAnimation(Animation animatedSprite)
		{
			Animations.Add(animatedSprite.Name, animatedSprite);
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
