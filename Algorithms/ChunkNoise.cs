using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadBarrage.Algorithms
{
    internal static class ChunkNoise
    {
        private static readonly FastNoiseLite roadCountNoise = NoiseContainer.CreateNoise();

        private static readonly int roadPositionAmp = 50;
        private static readonly FastNoiseLite roadPositionNoise = NoiseContainer.CreateNoise();

        // <return> In the range [0, Constants.ChunkInfo.MaxRoadsPerSide]
        private static int RoadCount(int chunkX, int chunkY, int direction)
        {
            float n = roadCountNoise.GetNoise(chunkX + 24.12f, chunkY + 24.12f, direction + 24.12f);
            Debug.WriteLine(n);
            return (int)Math.Abs(n * (Constants.ChunkInfo.MaxRoadsPerSide + 1));
        }

        private static (int, int, int) RightBorder(int chunkX, int chunkY)
        {
            return (chunkX, chunkY, 0);
        }

        private static (int, int, int) LeftBorder(int chunkX, int chunkY)
        {
            return (chunkX - 1, chunkY, 0);
        }

        private static (int, int, int) DownBorder(int chunkX, int chunkY)
        {
            return (chunkX, chunkY, 1);
        }

        private static (int, int, int) UpBorder(int chunkX, int chunkY)
        {
            return (chunkX, chunkY - 1, 1);
        }

        public static int RightSideRoadCount(int chunkX, int chunkY)
        {
            var (x, y, direction) = RightBorder(chunkX, chunkY);
            return RoadCount(x, y, direction);
        }

        public static int LeftSideRoadCount(int chunkX, int chunkY)
        {
            var (x, y, direction) = LeftBorder(chunkX, chunkY);
            return RoadCount(x, y, direction);
        }

        public static int DownSideRoadCount(int chunkX, int chunkY)
        {
            var (x, y, direction) = DownBorder(chunkX, chunkY);
            return RoadCount(x, y, direction);
        }

        public static int UpSideRoadCount(int chunkX, int chunkY)
        {
            var (x, y, direction) = UpBorder(chunkX, chunkY);
            return RoadCount(x, y, direction);
        }

        // <return> Position of road in blocks
        public static int[] RoadPositions(int chunkX, int chunkY, int direction)
        {
            int roadCount = RoadCount(chunkX, chunkY, direction);
            double defaultPosition = Constants.ChunkInfo.ChunkSize * 1.0 / (roadCount + 1);
            int[] roadPositions = new int[roadCount];

            for (int i = 0; i < roadCount; i++)
            {
                int pos = (int)(defaultPosition * (i + 1) + roadPositionNoise.GetNoise(chunkX, chunkY, direction) * roadPositionAmp);
                pos = pos % Constants.ChunkInfo.ChunkSize;
                roadPositions[i] = pos;
            }

            return roadPositions;
        }

        public static int[] RightRoadPositions(int chunkX, int chunkY)
        {
            var (x, y, direction) = RightBorder(chunkX, chunkY);
            return RoadPositions(x, y, direction);
        }

        public static int[] LeftRoadPositions(int chunkX, int chunkY)
        {
            var (x, y, direction) = LeftBorder(chunkX, chunkY);
            return RoadPositions(x, y, direction);
        }

        public static int[] DownRoadPositions(int chunkX, int chunkY)
        {
            var (x, y, direction) = DownBorder(chunkX, chunkY);
            return RoadPositions(x, y, direction);
        }
        public static int[] UpRoadPositions(int chunkX, int chunkY)
        {
            var (x, y, direction) = UpBorder(chunkX, chunkY);
            return RoadPositions(x, y, direction);
        }
    }
}
