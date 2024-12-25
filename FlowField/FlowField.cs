using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RoadBarrage.FlowField
{
    internal class FlowField
    {
        private Visuals visuals;

        private Random random = new Random();
        private FastNoiseLite noise = new FastNoiseLite();

        public double[,] Angles { get; private set; } =
            new double[Constants.ChunkRes.ResolutionX, Constants.ChunkRes.ResolutionX];

        private List<Influencer> influencers = new List<Influencer>();

        public FlowField(Visuals visuals)
        {
            this.visuals = visuals;
        }

        public void AddInfluencer(Influencer influencer)
        {
            influencers.Add(influencer);
            RecalculateField();
        }

        public void ClearInfluencers()
        {
            influencers.Clear();
            RecalculateField();
        }

        public bool RemoveInfluencer(Influencer influencer)
        {
            bool result = influencers.Remove(influencer);
            RecalculateField();

            return result;
        }

        public void RecalculateField()
        {
            for (int blockX = 0; blockX < Constants.ChunkRes.ResolutionX; blockX++)
            {
                for (int blockY = 0; blockY < Constants.ChunkRes.ResolutionY; blockY++)
                {
                    double weightedX = 0;
                    double weightedY = 0;

                    foreach (Influencer influencer in influencers)
                    {
                        weightedX += influencer.field[blockX, blockY, 0];
                        weightedY += influencer.field[blockX, blockY, 1];
                    }

                    double resultingDirection = Math.Atan2(weightedY, weightedX);
                    Angles[blockX, blockY] = resultingDirection;
                }
            }
        }
        public static Color[,] DrawCross(double angleRadians)
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
                    Color[,] arrow = DrawCross(Angles[x, y]);
                    visuals.UpdateWorldData(x, y, arrow, true);
                }
            }

            foreach (Influencer influencer in influencers)
            {
                Color[,] color;

                if (influencer is GridInfluencer)
                {
                    GridInfluencer _influencer = (GridInfluencer)influencer;
                    color = visuals.SquareColor(Color.Red);
                }
                else
                {
                    color = visuals.CircleColor(Color.Red);
                }

                visuals.UpdateWorldData(influencer.x, influencer.y, color, true);
            }

            visuals.SyncTexture();
        }
    }
}
