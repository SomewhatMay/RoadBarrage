using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RoadBarrage
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

        // Update an entire block to a specific color
        public void UpdateWorldData(int x, int y, Color color)
        {
            for (int blockX = 0; blockX < Constants.ChunkRes.BlockSize; blockX++)
            {
                for (int blockY = 0; blockY < Constants.ChunkRes.BlockSize; blockY++)
                {
                    int trueX = x * Constants.ChunkRes.BlockSize + blockX;
                    int trueY = y * Constants.ChunkRes.BlockSize + blockY;
                    WorldData[trueX + trueY * Constants.WindowDimensions.Width] = color;
                }
            }

            texture.SetData(WorldData);
        }

        // Update each specific pixel in a block
        public void UpdateWorldData(int x, int y, Color[,] colors)
        {
            if (colors.GetLength(0) != Constants.ChunkRes.BlockSize || colors.GetLength(1) != Constants.ChunkRes.BlockSize)
                throw new Exception("Size of sprite out of bounds!");

            for (int blockX = 0; blockX < Constants.ChunkRes.BlockSize; blockX++)
            {
                for (int blockY = 0; blockY < Constants.ChunkRes.BlockSize; blockY++)
                {
                    int trueX = x * Constants.ChunkRes.BlockSize + blockX;
                    int trueY = y * Constants.ChunkRes.BlockSize + blockY;
                    WorldData[trueX + trueY * Constants.WindowDimensions.Width] = colors[blockX, blockY];
                }
            }

            texture.SetData(WorldData);
        }

        // Overwrite the worldColor data to something new.
        // Useful if you want to adjust an arbitrary area of the colors.
        public void SetWorldData(Color[] worldData)
        {
            if (worldData.Length != WorldData.Length)
                throw new Exception("New world data does not match required world data!");

            WorldData = worldData;
            texture.SetData(WorldData);
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
