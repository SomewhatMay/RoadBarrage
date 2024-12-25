using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RoadBarrage.Graphical
{
    internal class Visuals
    {
        private Texture2D texture;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;

        // Coordinate points; not a matrix
        public Color[] WorldData { get; private set; } =
            new Color[Constants.WindowDimensions.Width * Constants.WindowDimensions.Height];

        public Visuals(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;

            texture = new Texture2D(
                graphicsDevice,
                Constants.WindowDimensions.Width,
                Constants.WindowDimensions.Height
            );
        }

        public int CoordinatesToIndex(int x, int y)
        {
            return x + y * Constants.WindowDimensions.Width;
        }

        public Color[,] SquareColor(Color color)
        {
            Color[,] square = new Color[Constants.ChunkRes.BlockSize, Constants.ChunkRes.BlockSize];

            for (int x = 0; x < Constants.ChunkRes.BlockSize; x++)
            {
                for (int y = 0; y < Constants.ChunkRes.BlockSize; y++)
                {
                    square[x, y] = color;
                }
            }

            return square;
        }

        public Color[,] CircleColor(Color color)
        {
            Color[,] circle = new Color[Constants.ChunkRes.BlockSize, Constants.ChunkRes.BlockSize];
            int centerX = Constants.ChunkRes.BlockSize / 2;
            int centerY = Constants.ChunkRes.BlockSize / 2;
            int radius = Constants.ChunkRes.BlockSize / 2;

            for (int x = 0; x < Constants.ChunkRes.BlockSize; x++)
            {
                for (int y = 0; y < Constants.ChunkRes.BlockSize; y++)
                {
                    int dx = x - centerX;
                    int dy = y - centerY;

                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        circle[x, y] = color;
                    }
                    else
                    {
                        circle[x, y] = Color.Black;
                    }
                }
            }

            return circle;
        }

        // Update an entire block to a specific color
        public void UpdateWorldData(int x, int y, Color color, bool softUpdate = false)
        {
            for (int blockX = 0; blockX < Constants.ChunkRes.BlockSize; blockX++)
            {
                for (int blockY = 0; blockY < Constants.ChunkRes.BlockSize; blockY++)
                {
                    int trueX = x * Constants.ChunkRes.BlockSize + blockX;
                    int trueY = y * Constants.ChunkRes.BlockSize + blockY;
                    WorldData[CoordinatesToIndex(trueX, trueY)] = color;
                }
            }

            if (!softUpdate)
            {
                texture.SetData(WorldData);
            }
        }

        // Update each specific pixel in a block
        public void UpdateWorldData(int x, int y, Color[,] colors, bool softUpdate = false)
        {
            if (colors.GetLength(0) != Constants.ChunkRes.BlockSize || colors.GetLength(1) != Constants.ChunkRes.BlockSize)
                throw new Exception("Size of sprite out of bounds!");

            for (int blockX = 0; blockX < Constants.ChunkRes.BlockSize; blockX++)
            {
                for (int blockY = 0; blockY < Constants.ChunkRes.BlockSize; blockY++)
                {
                    int trueX = x * Constants.ChunkRes.BlockSize + blockX;
                    int trueY = y * Constants.ChunkRes.BlockSize + blockY;
                    WorldData[CoordinatesToIndex(trueX, trueY)] = colors[blockX, blockY];
                }
            }

            if (!softUpdate)
            {
                texture.SetData(WorldData);
            }
        }

        // Overwrite the worldColor data to something new.
        // Useful if you want to adjust an arbitrary area of the colors.
        public void SetWorldData(Color[] worldData, bool softUpdate = false)
        {
            if (worldData.Length != WorldData.Length)
                throw new Exception("New world data does not match required world data!");

            WorldData = worldData;

            if (!softUpdate)
            {
                texture.SetData(WorldData);
            }
        }

        public void SyncTexture()
        {
            texture.SetData(WorldData);
        }

        public Color[] foreachBlock(Func<int, int, Color, Color> predicate)
        {
            Color[] newWorldData = new Color[WorldData.Length];

            for (int blockX = 0; blockX < Constants.ChunkRes.BlockSize; blockX++)
            {
                for (int blockY = 0; blockY < Constants.ChunkRes.BlockSize; blockY++)
                {
                    int x = blockX * Constants.ChunkRes.BlockSize;
                    int y = blockY * Constants.ChunkRes.BlockSize;

                    int i = CoordinatesToIndex(x, y);
                    Color result = predicate(blockX, blockY, WorldData[i]);

                    for (int sizeX = 0; sizeX < Constants.ChunkRes.BlockSize; sizeX++)
                    {
                        for (int sizeY = 0; sizeY < Constants.ChunkRes.BlockSize; sizeY++)
                        {
                            int trueX = x + sizeX;
                            int trueY = y + sizeY;
                            newWorldData[CoordinatesToIndex(trueX, trueY)] = result;
                        }
                    }
                }
            }

            return newWorldData;
        }

        public Color[] foreachPixel(Func<int, int, Color, Color> predicate)
        {
            Color[] newWorldData = new Color[WorldData.Length];

            for (int x = 0; x < Constants.WindowDimensions.Width; x++)
            {
                for (int y = 0; y < Constants.WindowDimensions.Height; y++)
                {
                    int i = CoordinatesToIndex(x, y);
                    newWorldData[i] = predicate(x, y, WorldData[i]);

                }
            }

            return newWorldData;
        }

        public void Draw()
        {
            spriteBatch.Draw(
                texture,
                Vector2.Zero,
                Color.White
            );
        }
    }
}
