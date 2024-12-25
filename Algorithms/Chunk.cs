using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadBarrage.Algorithms
{
    internal class Chunk
    {
        private Game1 game;

        public int ChunkX { get; private set; }
        public int ChunkY { get; private set; }

        public Chunk(Game1 game, int chunkX, int chunkY)
        {
            this.game = game;

            ChunkX = chunkX;
            ChunkY = chunkY;
        }


    }
}
