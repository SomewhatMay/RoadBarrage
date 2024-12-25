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
            // By definition, chunks must be square
            // Try to ensure that Size is a divisor of Width and Height
            public const int Size = 16;
            //public static readonly Vector2 ChunkCount = new Vector2(WindowDimensions.Width / Size, WindowDimensions.Height / Size);
            public const int ChunkCountX = WindowDimensions.Width / Size;
            public const int ChunkCountY = WindowDimensions.Height / Size;
        }
    }
}
