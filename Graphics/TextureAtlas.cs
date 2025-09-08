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


		public TextureAtlas()
		{
			Regions = [];
			Animations = [];
		}

		public TextureAtlas(Texture2D texture)
		{
			Texture = texture;
			Regions = [];
			Animations = [];
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

		public void Clear()
		{
			Regions.Clear();
			Animations.Clear();
		}

		public Sprite CreateSprite(string regionName)
		{
			TextureRegion region = GetRegion(regionName);
			return new Sprite(region);
		}

		public AnimatedSprite CreateAnimatedSprite(string animationName)
		{
			Animation animation = GetAnimation(animationName);
			return new AnimatedSprite(animation);
		}

		public static TextureAtlas FromAsepriteJsonFile(ContentManager content, string fileName)
		{
			
			if (!fileName.StartsWith(@".\Content\"))
			{
				fileName = @".\Content\" + fileName;
			}

			if (!fileName.EndsWith(@".json"))
			{
				fileName += ".json";
			}

			var file = JsonNode.Parse(File.ReadAllText(fileName)) 
				?? throw new Exception($"Unable to parse json file: \"{fileName}\"");

			string imagePath = file["meta"]["image"].GetValue<string>();
			while (Path.HasExtension(imagePath))
			{
				imagePath = imagePath[..imagePath.LastIndexOf('.')];
			}
			string sub = fileName[@".\Content\".Length..fileName.LastIndexOf('\\')];
			imagePath = Path.Combine(sub, imagePath);

			TextureAtlas atlas = new()
			{
				Texture = content.Load<Texture2D>(imagePath)
			};

			var frames = file["frames"].AsArray();
			foreach (var frame in frames)
			{
				atlas.AddRegion(
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
					string key = name + "_" + i.ToString();
					spr.Frames.Add(atlas.GetRegion(key));
				}

				atlas.AddAnimation(name, spr);
			}

			return atlas;
		}
	}
}
