using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RoadBarrage.External
{
    internal abstract class Influencer
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int size { get; private set; }

        // [x, y, 0] -> x component of angle at pos (x, y)
        // [x, y, 1] -> y component of angle at pos (x, y)
        public double[,,] field { get; private set; }

        public Influencer(int x, int y, int size)
        {
            this.x = x;
            this.y = y;
            this.size = size;

            field = new double[Constants.ChunkRes.ResolutionX, Constants.ChunkRes.ResolutionY, 2];
        }

        protected abstract void CalculateField();
    }

    internal class GridInfluencer : Influencer
    {
        public double angle { get; private set; }
        private double dirX, dirY;
        public GridInfluencer(int x, int y, int size, double angle)
        : base(x, y, size)
        {
            this.angle = angle;

            dirX = Math.Cos(angle);
            dirY = Math.Sin(angle);

            CalculateField();
        }

        protected override void CalculateField()
        {
            for (int blockX = 0; blockX < Constants.ChunkRes.ResolutionX; blockX++)
            {
                for (int blockY = 0; blockY < Constants.ChunkRes.ResolutionY; blockY++)
                {
                    double distance = Math.Sqrt(Math.Pow(blockX - x, 2) + Math.Pow(blockY - y, 2));
                    double _dirX = dirX;
                    double _dirY = dirY;

                    if (distance > size)
                    {
                        _dirX = 0;
                        _dirY = 0;
                    }

                    field[blockX, blockY, 0] = _dirX;
                    field[blockX, blockY, 1] = _dirY;
                }
            }
        }
    }

    internal class RadialInfluencer : Influencer
    {
        public RadialInfluencer(int x, int y, int size)
        : base(x, y, size)
        {
            CalculateField();
        }

        protected override void CalculateField()
        {
            for (int blockX = 0; blockX < Constants.ChunkRes.ResolutionX; blockX++)
            {
                for (int blockY = 0; blockY < Constants.ChunkRes.ResolutionY; blockY++)
                {
                    int dx = blockX - x;
                    int dy = blockY - y;
                    double distance = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

                    if (distance > size)
                    {
                        dx = 0;
                        dy = 0;
                    }

                    field[blockX, blockY, 0] = dx;
                    field[blockX, blockY, 1] = dy;
                }
            }
        }
    }

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

            //AddInfluencer(new RadialInfluencer(20, 20, 30, 1));
            AddInfluencer(new GridInfluencer(35, 35, 15, Math.PI / 4));
            AddInfluencer(new RadialInfluencer(25, 25, 15));
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
