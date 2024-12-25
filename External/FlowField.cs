using Microsoft.Xna.Framework;
using System;

namespace RoadBarrage.External
{
    internal class FlowField
    {
        private Visuals visuals;

        private Random random = new Random();
        private FastNoiseLite noise = new FastNoiseLite();

        public double[,] Angles { get; private set; } =
            new double[Constants.ChunkRes.ResolutionX, Constants.ChunkRes.ResolutionX];

        public FlowField(Visuals visuals)
        {
            this.visuals = visuals;

            for (int x = 0; x < Angles.GetLength(0); x++)
            {
                for (int y = 0; y < Angles.GetLength(1); y++)
                {
                    Angles[x, y] = noise.GetNoise(x * 1f, y * 1f) * 360;
                }
            }
        }

        public static Color[,] DrawCross(int angleDegrees)
        {
            Color[,] grid = new Color[Constants.ChunkRes.BlockSize, Constants.ChunkRes.BlockSize];
            for (int i = 0; i < Constants.ChunkRes.BlockSize; i++)
            {
                for (int j = 0; j < Constants.ChunkRes.BlockSize; j++)
                {
                    grid[i, j] = new Color(0, 0, 0);
                }
            }

            int centerX = Constants.ChunkRes.BlockSize / 2, centerY = Constants.ChunkRes.BlockSize / 2;

            double angleRadians = angleDegrees * Math.PI / 180.0;

            for (int x = 0; x < Constants.ChunkRes.BlockSize; x++)
            {
                for (int y = 0; y < Constants.ChunkRes.BlockSize; y++)
                {
                    int dx = x - centerX;
                    int dy = y - centerY;

                    double rotatedX = dx * Math.Cos(angleRadians) + dy * Math.Sin(angleRadians);
                    double rotatedY = -dx * Math.Sin(angleRadians) + dy * Math.Cos(angleRadians);

                    if (Math.Abs(rotatedX) < 1 || Math.Abs(rotatedY) < 1)
                    {
                        double gradient = 1 - Math.Min(Math.Abs(rotatedX), Math.Abs(rotatedY));
                        grid[x, y] = new Color((int)(255 * gradient), (int)(255 * gradient), (int)(255 * gradient));
                    }
                }
            }

            return grid;
        }

        public void Visualize()
        {
            for (int x = 0; x < Angles.GetLength(0); x++)
            {
                for (int y = 0; y < Angles.GetLength(1); y++)
                {
                    Color[,] arrow = DrawCross((int)Angles[x, y]);
                    visuals.UpdateWorldData(x, y, arrow, true);
                }
            }

            visuals.SyncTexture();
        }
    }
}
