﻿using System.Numerics;

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

            public const int ResolutionX = WindowDimensions.Width / BlockSize;
            public const int ResolutionY = WindowDimensions.Height / BlockSize;
        }
    }
}
