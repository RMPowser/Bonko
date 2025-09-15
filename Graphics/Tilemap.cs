using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

using LDtk;

namespace Graphics
{
	public class TileMap
	{
		private readonly TileSet Tileset;
		private readonly int[] Tiles;
		public int Rows { get; }
		public int Columns { get; }
		public int Count { get; }
		public Vector2 Scale { get; set; }
		public float TileWidth => Tileset.TileWidth * Scale.X;
		public float TileHeight => Tileset.TileHeight * Scale.Y;


		public TileMap(TileSet tileset, int columns, int rows)
		{
			Tileset = tileset;
			Rows = rows;
			Columns = columns;
			Count = Columns * Rows;
			Scale = Vector2.One;
			Tiles = new int[Count];
		}

		public void SetTile(int index, int tilesetID)
		{
			Tiles[index] = tilesetID;
		}

		public void SetTile(int column, int row, int tilesetID)
		{
			int index = row * Columns + column;
			SetTile(index, tilesetID);
		}

		public TextureRegion GetTile(int index)
		{
			return Tileset.GetTile(Tiles[index]);
		}

		public TextureRegion GetTile(int column, int row)
		{
			int index = row * Columns + column;
			return GetTile(index);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < Count; i++)
			{
				int tileSetIndex = Tiles[i];
				TextureRegion tile = Tileset.GetTile(tileSetIndex);

				int x = i % Columns;
				int y = i / Columns;

				Vector2 position = new(x * TileWidth, y * TileHeight);
				tile.Draw(spriteBatch, position, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 1.0f);
			}
		}
	}
}
