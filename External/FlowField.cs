using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace RoadBarrage.External
{
    internal abstract class Influencer
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public double size { get; private set; }

        public Influencer(int x, int y, double size)
        {
            this.x = x;
            this.y = y;
            this.size = size;
        }
    }

    internal class GridInfluencer : Influencer
    {
        public double angle { get; private set; }
        public GridInfluencer(int x, int y, double size, double angle)
        : base(x, y, size)
        {
            this.angle = angle;
        }
    }

    internal class RadialInfluencer : Influencer
    {
        public double intensity { get; private set; }
        public RadialInfluencer(int x, int y, double size, double intensity)
        : base(x, y, size)
        {
            this.intensity = intensity;
        }
    }

    internal class FlowField
    {
        private Visuals visuals;

        private Random random = new Random();
        private FastNoiseLite noise = new FastNoiseLite();

        public double[,] Angles { get; private set; } =
            new double[Constants.ChunkRes.ResolutionX, Constants.ChunkRes.ResolutionX];

        public List<Influencer> influencers = new List<Influencer>();

        public FlowField(Visuals visuals)
        {
            this.visuals = visuals;

            influencers.Add(new GridInfluencer(1, 1, 10, 45));
            influencers.Add(new RadialInfluencer(12, 12, 10, 1));
            Debug.WriteLine(influencers.Count);
        }

        public static Color[,] DrawCross(double angleDegrees)
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

            foreach (Influencer influencer in influencers)
            {
                Color[,] color;

                if (influencer is GridInfluencer)
                {
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
