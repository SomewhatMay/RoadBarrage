using System.Numerics;

namespace RoadBarrage
{
    internal class Constants
    {
        public static class WindowDimensions
        {
            public const int Width = 640;
            public const int Height = 640;
        }

        public static class ChunkResolution
        {
            // Number of pixels per block
            public const int BlockSize = 4;

            // Number of Blocks per Chunk
            // By definition, chunks must be square
            // Try to ensure that Size is a divisor of Width and Height
            public const int Size = 16;

            public const int ResolutionX = WindowDimensions.Width / BlockSize;
            public const int ResolutionY = WindowDimensions.Height / BlockSize;
        }
    }
}
