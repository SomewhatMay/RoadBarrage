using System.Numerics;

namespace RoadBarrage
{
    internal class Constants
    {
        public static class WindowDimensions
        {
            public const int Width = 540;
            public const int Height = 540;
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
            public const int ChunkSize = 16; // In Blocks; Chunks are by definition square

            public const int MaxRoadsPerSide = 4;
        }
    }
}
