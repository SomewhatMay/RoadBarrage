using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoadBarrage
{
    internal class Visuals
    {
        private Texture2D texture;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;

        // Coordinate points; not a matrix
        public Color[] WorldData { get; private set; } = 
            new Color[Constants.ChunkResolution.Size * Constants.ChunkResolution.Size];

        public Visuals(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;

            texture = new Texture2D(graphicsDevice, Constants.ChunkResolution.Size, Constants.ChunkResolution.Size);
        }

        public void UpdateWorldData(int x, int y, Color color)
        {
            WorldData[x + y * Constants.ChunkResolution.Size] = color;
            texture.SetData(WorldData);
        }

        public void SetWorldData(Color[] worldData)
        {
            WorldData = worldData;
            texture.SetData(WorldData);
        }

        public void Draw()
        {
            spriteBatch.Draw(
                texture, 
                new Rectangle(0, 0, Constants.WindowDimensions.Width, Constants.WindowDimensions.Height), 
                Color.White
            );
        }
    }
}
