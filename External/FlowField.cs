using Microsoft.Xna.Framework;
using System;

namespace RoadBarrage.External
{
    internal class FlowField
    {
        private Visuals visuals;

        public double[,] Angles { get; private set; } =
            new double[Constants.ChunkRes.ResolutionX, Constants.ChunkRes.ResolutionX];

        public FlowField(Visuals visuals)
        {
            this.visuals = visuals;
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

            //Color[,] colorGrid = new Color[Constants.ChunkRes.BlockSize, Constants.ChunkRes.BlockSize];
            //for (int x = 0; x < colorGrid.GetLength(0); x++)
            //{
            //    for (int y = 0; y < colorGrid.GetLength(1); y++)
            //    {
            //        int b = grid[y, x];
            //        colorGrid[x, y] = new Color(b, b, b);
            //    }
            //}

            return grid;
        }

        public void Visualize()
        {
            Color[] worldData = visuals.WorldData;

            for (int x = 0; x < Angles.GetLength(0); x++)
            {
                for (int y = 0; y < Angles.GetLength(1); y++)
                {
                    Color[,] arrow = new Color[Constants.ChunkRes.BlockSize, Constants.ChunkRes.BlockSize];
                }
            }
        }
    }
}
