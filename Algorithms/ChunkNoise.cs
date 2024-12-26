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

        private static readonly int roadPositionAmp = 15;
        private static readonly FastNoiseLite roadPositionNoise = NoiseContainer.CreateNoise();

        private static readonly FastNoiseLite roadConnectionsNoise = NoiseContainer.CreateNoise();

        // <return> In the range [0, Constants.ChunkInfo.MaxRoadsPerSide]
        private static int RoadCount(int chunkX, int chunkY, int direction)
        {
            float n = roadCountNoise.GetNoise(chunkX + 24.12f, chunkY + 24.12f, direction + 24.12f);
            //return (int)Math.Abs(n * (Constants.ChunkInfo.MaxRoadsPerSide + 1));
            return Constants.ChunkInfo.MaxRoadsPerSide;
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
            int roadPrefix = direction * Constants.ChunkInfo.MaxRoadsPerSide;
            double defaultPosition = Constants.ChunkInfo.ChunkSize * 1.0 / (roadCount + 1);
            int[] roadPositions = new int[roadCount];

            for (int i = 0; i < roadCount; i++)
            {
                int pos = (int)(defaultPosition * (i + 1) + roadPositionNoise.GetNoise(chunkX, chunkY, roadPrefix + i) * roadPositionAmp);
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

        public static WorldPos[] LinearToWorldPos(int side, int[] rawPositions, int chunkX, int chunkY)
        {
            int chunkPixels = Constants.ChunkInfo.ChunkPixels;
            int blockSize = Constants.WorldRes.BlockSize;
            WorldPos[] positions = new WorldPos[rawPositions.Length];

            for (int i = 0; i < rawPositions.Length; i++)
            {
                int pos = rawPositions[i];
                switch (side)
                {
                    case 0: // right
                        positions[i] = new WorldPos((chunkX + 1) * chunkPixels - 1, chunkY * chunkPixels + pos * blockSize);
                        break;
                    case 1: // down
                        positions[i] = new WorldPos(chunkX * chunkPixels + pos * blockSize, (chunkY + 1) * chunkPixels - 1);
                        break;
                    case 2: // left
                        positions[i] = new WorldPos(chunkX * chunkPixels, chunkY * chunkPixels + pos * blockSize);
                        break;
                    case 3: // up
                        positions[i] = new WorldPos(chunkX * chunkPixels + pos * blockSize, chunkY * chunkPixels);
                        break;
                    default:
                        throw new Exception("Invalid side!");
                        break;
                }
            }

            return positions;
        }

        public static int GetRandomKey(Dictionary<int, float> weightedOptions, int x, int y, int z)
        {
            if (weightedOptions == null || weightedOptions.Count == 0)
                throw new ArgumentException("Dictionary must not be null or empty.");

            // Normalize weights to calculate cumulative distribution
            float totalWeight = weightedOptions.Values.Sum();
            var normalizedOptions = weightedOptions
                .Select(kvp => new { kvp.Key, NormalizedWeight = kvp.Value / totalWeight })
                .ToList();

            // Build cumulative weight map
            float cumulativeSum = 0;
            var cumulativeDistribution = new List<(int Key, float CumulativeWeight)>();

            foreach (var option in normalizedOptions)
            {
                cumulativeSum += option.NormalizedWeight;
                cumulativeDistribution.Add((option.Key, cumulativeSum));
            }

            // Generate Perlin noise for selection
            float noiseValue = roadConnectionsNoise.GetNoise(x, y, z);

            // Find corresponding key
            foreach (var entry in cumulativeDistribution)
            {
                if (noiseValue <= entry.CumulativeWeight)
                {
                    return entry.Key;
                }
            }

            // Fallback (should not occur)
            return cumulativeDistribution.Last().Key;
        }
    }
}
