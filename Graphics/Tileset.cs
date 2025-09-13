﻿
namespace Graphics
{
	public class Tileset
	{
		private readonly TextureRegion[] Tiles;
		public int TileWidth { get; }
		public int TileHeight { get; }
		public int Columns { get; }
		public int Rows { get; }
		public int Count { get; }


		public Tileset(TextureRegion textureRegion, int tileWidth, int tileHeight)
		{
			TileWidth = tileWidth;
			TileHeight = tileHeight;
			Columns = textureRegion.Width / tileWidth;
			Rows = textureRegion.Height / tileHeight;
			Count = Columns * Rows;

			Tiles = new TextureRegion[Count];

			for (int i = 0; i < Count; i++)
			{
				int x = i % Columns * tileWidth;
				int y = i / Columns * tileHeight;
				Tiles[i] = new TextureRegion(textureRegion.Texture, textureRegion.SourceRectangle.X + x, textureRegion.SourceRectangle.Y + y, tileWidth, tileHeight);
			}
		}

		public TextureRegion GetTile(int index) 
		{
			return Tiles[index]; 
		}

		public TextureRegion GetTile(int column, int row)
		{
			int index = row * Columns + column;
			return GetTile(index);
		}
	}
}
