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

        public static class ChunkRes
        {
            // Number of pixels per block
            public const int BlockSize = 9;

            public const int ResolutionX = WindowDimensions.Width / BlockSize;
            public const int ResolutionY = WindowDimensions.Height / BlockSize;
        }
    }
}
