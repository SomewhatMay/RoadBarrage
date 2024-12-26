using Microsoft.Xna.Framework;
using RoadBarrage.Graphical;
using System.Diagnostics;

namespace RoadBarrage.Algorithms
{
    internal class Chunk
    {
        private readonly Game1 game;
        private readonly Visuals visuals;

        public int ChunkX { get; private set; }
        public int ChunkY { get; private set; }

        public Chunk(Game1 game, int chunkX, int chunkY)
        {
            this.game = game;

            // TODO FIXME This is dangerous! Visuals might not exist at this point!
            visuals = game.visuals;

            ChunkX = chunkX;
            ChunkY = chunkY;
        }

        public void Visualize(bool softUpdate = false)
        {
            int chunkPixelSize = Constants.WorldRes.BlockSize * Constants.ChunkInfo.ChunkSize;
            int chunkPixelX = ChunkX * chunkPixelSize;
            int chunkPixelY = ChunkY * chunkPixelSize;
            Color borderColor = Color.Gray;

            // Draw Chunk Borders
            for (int i = 0; i < chunkPixelSize; i++)
            {
                visuals.WorldData[visuals.CoordinatesToIndex(chunkPixelX + i, chunkPixelY)] = borderColor;
                visuals.WorldData[visuals.CoordinatesToIndex(chunkPixelX, chunkPixelY + i)] = borderColor;
                visuals.WorldData[visuals.CoordinatesToIndex(chunkPixelX + i, chunkPixelY + chunkPixelSize)] = borderColor;
                visuals.WorldData[visuals.CoordinatesToIndex(chunkPixelX + chunkPixelSize, chunkPixelY + i)] = borderColor;
            }

            // Draw the road positions and entrances
            int[] rightRoadPositions = ChunkNoise.RightRoadPositions(ChunkX, ChunkY);

            foreach (int position in rightRoadPositions)
            {
                int x = chunkPixelX + chunkPixelSize - 1;
                int y = chunkPixelY + position * Constants.WorldRes.BlockSize;
                visuals.WorldData[visuals.CoordinatesToIndex(x, y)] = Color.Red;
            }

            int[] leftRoadPositions = ChunkNoise.LeftRoadPositions(ChunkX, ChunkY);

            foreach (int position in leftRoadPositions)
            {
                int x = chunkPixelX + 1;
                int y = chunkPixelY + position * Constants.WorldRes.BlockSize;
                visuals.WorldData[visuals.CoordinatesToIndex(x, y)] = Color.Red;
            }

            if (!softUpdate)
            {
                visuals.SyncTexture();
            }
        }
    }
}
