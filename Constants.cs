using System.Numerics;

namespace RoadBarrage
{
    internal class Constants
    {
        public static class WindowDimensions
        {
            public const int Width = 720;
            public const int Height = 720;
        }

        public static class WorldRes
        {
            // Number of pixels per block
            public const int BlockSize = 9;

            public const int ResolutionX = WindowDimensions.Width / BlockSize;
            public const int ResolutionY = WindowDimensions.Height / BlockSize;
        }

        public static class ChunkInfo
        {
            public const int ChunkSize = 2; // In Blocks; Chunks are by definition square
            public const int ChunkPixels = ChunkSize * WorldRes.BlockSize; // Nmber of pixels per chunk

            public const int MaxRoadsPerSide = 1;
        }
    }
}
