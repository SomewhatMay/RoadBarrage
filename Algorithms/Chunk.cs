using Microsoft.Xna.Framework;
using RoadBarrage.Graphical;
using RoadBarrage.Graphical.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace RoadBarrage.Algorithms
{
    internal class Chunk
    {
        private readonly Game1 game;
        private readonly Visuals visuals;

        public int ChunkX { get; private set; }
        public int ChunkY { get; private set; }

        private static float sameSideWeight = 0.5f;
        private static Dictionary<int, float> directions = new Dictionary<int, float> { { 0, sameSideWeight }, { 1, 1 }, { 2, 1 }, { 3, 1 } };

        private static FastNoiseLite sideChoiceNoise = NoiseContainer.CreateNoise(1f);

        public Chunk(Game1 game, int chunkX, int chunkY)
        {
            this.game = game;

            // TODO FIXME This is dangerous! Visuals might not exist at this point!
            visuals = game.visuals;

            ChunkX = chunkX;
            ChunkY = chunkY;
        }

        public void VisualizeRoads()
        {
            int chunkPixelSize = Constants.WorldRes.BlockSize * Constants.ChunkInfo.ChunkSize;
            int chunkPixelX = ChunkX * chunkPixelSize;
            int chunkPixelY = ChunkY * chunkPixelSize;

            List<WorldPos> options = new List<WorldPos>();
            WorldPos[][] positions = new WorldPos[][] {
                ChunkNoise.LinearToWorldPos(0, ChunkNoise.RightRoadPositions(ChunkX, ChunkY), ChunkX, ChunkY),
                ChunkNoise.LinearToWorldPos(2, ChunkNoise.LeftRoadPositions(ChunkX, ChunkY), ChunkX, ChunkY),
                ChunkNoise.LinearToWorldPos(1, ChunkNoise.DownRoadPositions(ChunkX, ChunkY), ChunkX, ChunkY),
                ChunkNoise.LinearToWorldPos(3, ChunkNoise.UpRoadPositions(ChunkX, ChunkY), ChunkX, ChunkY),
            };
            foreach (var i in positions)
            {
                foreach (var j in i)
                {
                    options.Add(j);
                }
            }

            int pairIndex = 0;
            while (options.Count > 1)
            {
                pairIndex++;
                int startIndex = (int)Math.Clamp(Math.Round(Math.Abs(sideChoiceNoise.GetNoise(ChunkX, ChunkY, pairIndex * 2)) * options.Count), 0, options.Count - 1);
                WorldPos start = options[startIndex];
                options.Remove(start);
                int endIndex = (int)Math.Clamp(Math.Round(Math.Abs(sideChoiceNoise.GetNoise(ChunkX, ChunkY, pairIndex * 2 + 1)) * options.Count), 0, options.Count - 1);
                WorldPos end = options[endIndex];
                options.Remove(end);

                game.drawablesContainer.Add(new Road(start.X, start.Y, end.X, end.Y, 3));
            }
        }

        public void Visualize(bool softUpdate = false)
        {
            int chunkPixelSize = Constants.WorldRes.BlockSize * Constants.ChunkInfo.ChunkSize;
            int chunkPixelX = ChunkX * chunkPixelSize;
            int chunkPixelY = ChunkY * chunkPixelSize;
            Color borderColor = Color.Gray;

            // Draw Chunk Borders
            //for (int i = 0; i < chunkPixelSize; i++)
            //{
            //    visuals.WorldData[visuals.CoordinatesToIndex(chunkPixelX + i, chunkPixelY)] = borderColor;
            //    visuals.WorldData[visuals.CoordinatesToIndex(chunkPixelX, chunkPixelY + i)] = borderColor;
            //    visuals.WorldData[visuals.CoordinatesToIndex(chunkPixelX + i, chunkPixelY + chunkPixelSize)] = borderColor;
            //    visuals.WorldData[visuals.CoordinatesToIndex(chunkPixelX + chunkPixelSize, chunkPixelY + i)] = borderColor;
            //}

            // Draw the road positions and entrances
            int[] rightRoadPositions = ChunkNoise.RightRoadPositions(ChunkX, ChunkY);

            foreach (int position in rightRoadPositions)
            {
                int x = chunkPixelX + chunkPixelSize - 1;
                int y = chunkPixelY + position * Constants.WorldRes.BlockSize;
                //visuals.WorldData[visuals.CoordinatesToIndex(x, y)] = Color.Red;
            }

            int[] leftRoadPositions = ChunkNoise.LeftRoadPositions(ChunkX, ChunkY);

            foreach (int position in leftRoadPositions)
            {
                int x = chunkPixelX + 1;
                int y = chunkPixelY + position * Constants.WorldRes.BlockSize;
                //visuals.WorldData[visuals.CoordinatesToIndex(x, y)] = Color.Red;
            }

            int[] downRoadPositions = ChunkNoise.DownRoadPositions(ChunkX, ChunkY);

            foreach (int position in downRoadPositions)
            {
                int x = chunkPixelX + position * Constants.WorldRes.BlockSize;
                int y = chunkPixelY + chunkPixelSize - 1;
                //visuals.WorldData[visuals.CoordinatesToIndex(x, y)] = Color.Red;
            }

            int[] upRoadPositions = ChunkNoise.UpRoadPositions(ChunkX, ChunkY);

            foreach (int position in upRoadPositions)
            {
                int x = chunkPixelX + position * Constants.WorldRes.BlockSize;
                int y = chunkPixelY + 1;
                //visuals.WorldData[visuals.CoordinatesToIndex(x, y)] = Color.Red;
            }

            if (!softUpdate)
            {
                visuals.SyncTexture();
            }
        }
    }
}
