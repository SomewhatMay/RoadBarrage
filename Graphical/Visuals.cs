using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadBarrage.Graphical.Components;
using System;
using System.Collections.Generic;

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
            Color[,] square = new Color[Constants.WorldRes.BlockSize, Constants.WorldRes.BlockSize];

            for (int x = 0; x < Constants.WorldRes.BlockSize; x++)
            {
                for (int y = 0; y < Constants.WorldRes.BlockSize; y++)
                {
                    square[x, y] = color;
                }
            }

            return square;
        }

        public Color[,] CircleColor(Color color)
        {
            Color[,] circle = new Color[Constants.WorldRes.BlockSize, Constants.WorldRes.BlockSize];
            int centerX = Constants.WorldRes.BlockSize / 2;
            int centerY = Constants.WorldRes.BlockSize / 2;
            int radius = Constants.WorldRes.BlockSize / 2;

            for (int x = 0; x < Constants.WorldRes.BlockSize; x++)
            {
                for (int y = 0; y < Constants.WorldRes.BlockSize; y++)
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
            for (int blockX = 0; blockX < Constants.WorldRes.BlockSize; blockX++)
            {
                for (int blockY = 0; blockY < Constants.WorldRes.BlockSize; blockY++)
                {
                    int trueX = x * Constants.WorldRes.BlockSize + blockX;
                    int trueY = y * Constants.WorldRes.BlockSize + blockY;
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
            if (colors.GetLength(0) != Constants.WorldRes.BlockSize || colors.GetLength(1) != Constants.WorldRes.BlockSize)
                throw new Exception("Size of sprite out of bounds!");

            for (int blockX = 0; blockX < Constants.WorldRes.BlockSize; blockX++)
            {
                for (int blockY = 0; blockY < Constants.WorldRes.BlockSize; blockY++)
                {
                    int trueX = x * Constants.WorldRes.BlockSize + blockX;
                    int trueY = y * Constants.WorldRes.BlockSize + blockY;
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

            for (int blockX = 0; blockX < Constants.WorldRes.BlockSize; blockX++)
            {
                for (int blockY = 0; blockY < Constants.WorldRes.BlockSize; blockY++)
                {
                    int x = blockX * Constants.WorldRes.BlockSize;
                    int y = blockY * Constants.WorldRes.BlockSize;

                    int i = CoordinatesToIndex(x, y);
                    Color result = predicate(blockX, blockY, WorldData[i]);

                    for (int sizeX = 0; sizeX < Constants.WorldRes.BlockSize; sizeX++)
                    {
                        for (int sizeY = 0; sizeY < Constants.WorldRes.BlockSize; sizeY++)
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
